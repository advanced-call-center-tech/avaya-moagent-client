using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AvayaMoagentClient;
using AvayaMoagentClient.Commands;
using AvayaMoagentClient.Enumerations;
using AvayaMoagentClient.Messages;

namespace AvayaPDSEmulator
{
  public class AvayaPdsServer : DialerServer
  {
    private int _nextInvokeId = 1;

    public AvayaPdsServer() : base()
    {
      Jobs = new List<Job>();
      Jobs.Add(new Job(Job.JobType.Outbound, "TEST_JOB1"));
      Jobs.Add(new Job(Job.JobType.Outbound, "TEST_JOB2"));
    }

    public List<Job> Jobs { get; private set; }

    protected override void NewClientConnected(DialerConnection conn)
    {
      //Send the initial message
      conn.SendToClient(_NotificationMsg(Notification.Start, "AGENT_STARTUP"));
    }

    protected override void HandleClientMessage(DialerConnection conn, string msg)
    {
      HandleClientMessage(conn, Message.ParseMessage(msg));
    }

    public void HandleClientMessage(DialerConnection conn, Message m)
    {
      var handled = _HandleSimulatorCommand(conn, m);
      if (!handled)
      {
        //Not a simulator command, so treat it as a regular dialer message
        _HandleMessageFromClient(conn, m);
      }
    }

    private Message _PendingMsg(string command, params string[] contents)
    {
      return _Msg(MessageType.Pending, command, contents);
    }
    
    private Message _DataMsg(string command, params string[] contents)
    {
      return _Msg(MessageType.Data, command, contents);
    }

    private Message _ResponseMsg(string command, params string[] contents)
    {
      return _Msg(MessageType.Response, command, contents);
    }

    private Message _NotificationMsg(string command, params string[] contents)
    {
      return _Msg(MessageType.Notification, command, contents);
    }

    private Message _Msg(MessageType type, string command, params string[] contents)
    {
      return new Message
      {
        Command = command,
        Type = type,
        OrigId = "Agent server",
        ProcessId = "26621",
        InvokeId = (_nextInvokeId++).ToString(),
        Contents = contents.ToList()
      };
    }

    private void _ForAllClients(DialerConnection sender, Action<DialerConnection> action)
    {
      foreach (var c in Connections.Values)
      {
        if (sender == null || c.Id != sender.Id)
        {
          action(c);
        }
      }
    }

    private bool _HandleSimulatorCommand(DialerConnection conn, Message m)
    {
      bool ret = true;

      switch (m.Command.Trim())
      {
        case SimulatorCommand.SetJobs:
          {
            Jobs.Clear();
            m.Contents.ForEach(t =>
            {
              var parts = t.Split(new char[] { ',' });

              Jobs.Add(new Job((Job.JobType)parts[0][0], parts[1]));
            });

            break;
          }

        case SimulatorCommand.InboundCall:
          {
            _ForAllClients(conn, (c) =>
            {
              if (c.CurrentState == AvayaDialer.STATUS_ON_JOB_READY)
              {
                c.CurrentState = AvayaDialer.STATUS_ON_JOB_ON_CALL;
                c.SendToClient(_NotificationMsg(Notification.CallNotify, AvayaDialer.CODE_ADDITIONAL_DATA,
                  "INBOUND CALL * 11-20 SECS. WAITING", "INBOUND"));
                c.SendToClient(_NotificationMsg(Notification.CallNotify, AvayaDialer.CODE_COMPLETE));
              }
            });

            break;
          }

        case SimulatorCommand.OutboundCall:
          {
            _ForAllClients(conn, (c) =>
            {
              if (c.CurrentState == AvayaDialer.STATUS_ON_JOB_READY)
              {
                c.CurrentState = AvayaDialer.STATUS_ON_JOB_ON_CALL;
                c.SendToClient(_NotificationMsg(Notification.CallNotify, m.Contents.Take(4).ToArray()));

                var contents = m.Contents.Take(1).ToList();
                contents.AddRange(m.Contents.Skip(3));
                c.SendToClient(_NotificationMsg(Notification.CallNotify, contents.ToArray()));
                c.SendToClient(_NotificationMsg(Notification.CallNotify, AvayaDialer.CODE_COMPLETE));
              }
            });

            break;
          }

        case SimulatorCommand.ManagedCall:
          {
            _ForAllClients(conn, (c) =>
            {
              if (c.CurrentState == AvayaDialer.STATUS_ON_JOB_READY)
              {
                c.CurrentState = AvayaDialer.STATUS_ON_JOB_ON_CALL;
                c.SendToClient(_NotificationMsg(Notification.PreviewRecord, m.Contents.Take(4).ToArray()));

                var contents = m.Contents.Take(1).ToList();
                contents.AddRange(m.Contents.Skip(3));
                c.SendToClient(_NotificationMsg(Notification.PreviewRecord, contents.ToArray()));
                c.SendToClient(_NotificationMsg(Notification.PreviewRecord, AvayaDialer.CODE_COMPLETE));

                Thread.Sleep(5000);

                c.SendToClient(_PendingMsg(ManagedCall.Name, AvayaDialer.CODE_PENDING));

                Thread.Sleep(7000);

                c.SendToClient(_DataMsg(ManagedCall.Name, AvayaDialer.CODE_ADDITIONAL_DATA, "(CONNECT)"));
                c.SendToClient(_ResponseMsg(ManagedCall.Name, AvayaDialer.CODE_COMPLETE));
              }
            });

            break;
          }

        case SimulatorCommand.JobTransfer:
          {
            _ForAllClients(conn, (c) =>
            {
              if (c.CurrentState == AvayaDialer.STATUS_ON_JOB_READY)
              {
                c.SendToClient(_NotificationMsg(Notification.JobTransferLink, AvayaDialer.CODE_ADDITIONAL_DATA, m.Contents[0]));
                c.SendToClient(_NotificationMsg(Notification.JobTransferLink, AvayaDialer.CODE_COMPLETE));
              }
            });

            break;
          }

        default:
          {
            ret = false;

            break;
          }
      }

      return ret;
    }

    private void _HandleMessageFromClient(DialerConnection conn, Message m)
    {
      switch (m.Command.Trim())
      {
        case Logon.Name:
          {
            conn.SendToClient(_PendingMsg(Logon.Name, AvayaDialer.CODE_PENDING));
            conn.SendToClient(_ResponseMsg(Logon.Name, AvayaDialer.CODE_COMPLETE));

            break;
          }

        case ReserveHeadset.Name:
          {
            conn.SendToClient(_PendingMsg(ReserveHeadset.Name, AvayaDialer.CODE_PENDING));

            Thread.Sleep(500);

            conn.SendToClient(_ResponseMsg(ReserveHeadset.Name, AvayaDialer.CODE_COMPLETE));

            break;
          }

        case ConnectHeadset.Name:
          {
            conn.SendToClient(_PendingMsg(ConnectHeadset.Name, AvayaDialer.CODE_PENDING));

            Thread.Sleep(500);

            conn.SendToClient(_ResponseMsg(ConnectHeadset.Name, AvayaDialer.CODE_COMPLETE));

            break;
          }

        case ListState.Name:
          {
            string content;

            //If on a job, add the job name to the message
            if (conn.CurrentState != AvayaDialer.STATUS_NOT_ON_JOB)
            {
              content = conn.CurrentState + "," + conn.CurrentJob;
            }
            else
            {
              content = conn.CurrentState;
            }

            conn.SendToClient(_DataMsg(ListState.Name, content));
            conn.SendToClient(_ResponseMsg(ListState.Name, AvayaDialer.CODE_COMPLETE));

            break;
          }

        case SetWorkClass.Name:
          {
            conn.SendToClient(_ResponseMsg(SetWorkClass.Name, AvayaDialer.CODE_COMPLETE));

            break;
          }

        case AttachJob.Name:
          {
            conn.CurrentState = AvayaDialer.STATUS_ON_JOB_UNAVAILABLE;
            conn.CurrentJob = m.Contents[0];
            conn.SendToClient(_ResponseMsg(AttachJob.Name, AvayaDialer.CODE_COMPLETE));
          }

          break;

        case SetNotifyKeyField.Name:
          {
            conn.SendToClient(_ResponseMsg(SetNotifyKeyField.Name, AvayaDialer.CODE_COMPLETE));
          }

          break;

        case SetDataField.Name:
          {
            conn.SendToClient(_ResponseMsg(SetDataField.Name, AvayaDialer.CODE_COMPLETE));
          }

          break;

        case AvailableWork.Name:
          {
            conn.CurrentState = AvayaDialer.STATUS_ON_JOB_IDLE_NOT_READY;
            conn.SendToClient(_PendingMsg(AvailableWork.Name, AvayaDialer.CODE_PENDING));
            conn.SendToClient(_ResponseMsg(AvailableWork.Name, AvayaDialer.CODE_COMPLETE));
          }

          break;

        case TransferCall.Name:
          {
            conn.SendToClient(_ResponseMsg(TransferCall.Name, AvayaDialer.CODE_COMPLETE));
          }

          break;

        case ReleaseLine.Name:
          {
            conn.SendToClient(_PendingMsg(ReleaseLine.Name, AvayaDialer.CODE_PENDING));
            conn.SendToClient(_ResponseMsg(ReleaseLine.Name, AvayaDialer.CODE_COMPLETE));
          }

          break;

        case ReadyNextItem.Name:
          {
            if (conn.CurrentState == AvayaDialer.STATUS_NOT_ON_JOB)
            {
              conn.SendToClient(_ResponseMsg(ReadyNextItem.Name, ErrorCode.NotAttachedToJob));
            }
            else
            {
              conn.CurrentState = AvayaDialer.STATUS_ON_JOB_READY;
              conn.SendToClient(_ResponseMsg(ReadyNextItem.Name, AvayaDialer.CODE_COMPLETE));
            }
          }

          break;

        case FinishedItem.Name:
          {
            conn.CurrentState = AvayaDialer.STATUS_ON_JOB_IDLE_NOT_READY;
            conn.SendToClient(_PendingMsg(FinishedItem.Name, AvayaDialer.CODE_PENDING));
            conn.SendToClient(_ResponseMsg(FinishedItem.Name, AvayaDialer.CODE_COMPLETE));

            if (conn.IsLeavingJob)
            {
              conn.CurrentState = AvayaDialer.STATUS_ON_JOB_UNAVAILABLE;
              conn.SendToClient(_ResponseMsg(NoFurtherWork.Name, AvayaDialer.CODE_COMPLETE));
            }
          }

          break;

        case NoFurtherWork.Name:
          conn.SendToClient(_PendingMsg(NoFurtherWork.Name, AvayaDialer.CODE_PENDING));

          if (conn.CurrentState != AvayaDialer.STATUS_ON_JOB_ON_CALL)
          {
            conn.CurrentState = AvayaDialer.STATUS_ON_JOB_UNAVAILABLE;
            conn.SendToClient(_ResponseMsg(NoFurtherWork.Name, AvayaDialer.CODE_COMPLETE));
          }
          else
          {
            conn.IsLeavingJob = true;
          }

          break;

        case DetachJob.Name:
          conn.CurrentState = AvayaDialer.STATUS_NOT_ON_JOB;
          conn.CurrentJob = string.Empty;
          conn.IsLeavingJob = false;
          conn.SendToClient(_ResponseMsg(DetachJob.Name, AvayaDialer.CODE_COMPLETE));

          break;

        case DisconnectHeadset.Name:
          conn.SendToClient(_PendingMsg(DisconnectHeadset.Name, AvayaDialer.CODE_PENDING));
          conn.SendToClient(_ResponseMsg(DisconnectHeadset.Name, AvayaDialer.CODE_COMPLETE));

          break;

        case FreeHeadset.Name:
          conn.SendToClient(_ResponseMsg(FreeHeadset.Name, AvayaDialer.CODE_COMPLETE));

          break;

        case ListJobs.Name:
          var jobs = Jobs.Select(t => (char)t.Type + "," + t.JobName + ",A").ToList();
          jobs.Insert(0, AvayaDialer.CODE_ADDITIONAL_DATA);
          conn.SendToClient(_DataMsg(ListJobs.Name, jobs.ToArray()));
          conn.SendToClient(_ResponseMsg(ListJobs.Name, AvayaDialer.CODE_COMPLETE));

          break;

        case Logoff.Name:
          conn.IsDisconnecting = true;
          conn.SendToClient(_ResponseMsg(Logoff.Name, AvayaDialer.CODE_COMPLETE));

          break;
      }
    }
  }
}
