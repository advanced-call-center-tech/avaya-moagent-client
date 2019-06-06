using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using AvayaPDSEmulator;

namespace AvayaPdsService
{
  public partial class AvayaPdsService : ServiceBase
  {
    private AvayaPdsServer _server = null;

    public AvayaPdsService()
    {
      InitializeComponent();

      Logging.EventLogged += Logging_EventLogged;
    }

    private void Logging_EventLogged(object sender, LogEventArgs e)
    {
      this.EventLog.WriteEntry(e.Message);
    }

    public AvayaPdsServer Server
    {
      get
      {
        return _server;
      }
    }

    protected override void OnStart(string[] args)
    {
      Start();
    }

    protected override void OnStop()
    {
      _Stop();
    }

    internal void Start()
    {
      _Stop();

      _server = new AvayaPdsServer();
      _server.StartListening();
    }

    private void _Stop()
    {
      try
      {
        if (_server != null)
        {
          _server.StopListening();
        }
      }
      finally
      {
        _server = null;
      }
    }
  }
}
