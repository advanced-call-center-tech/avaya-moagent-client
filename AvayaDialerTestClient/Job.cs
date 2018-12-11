using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AvayaMoagentClient.Enumerations;

namespace AvayaDialerTestClient
{
  /// <summary>
  /// Job
  /// </summary>
  public partial class Job : UserControl, IPage
  {
    private SelectJob _parent;

    /// <summary>
    /// Creates a Job user control with the specified SelectJob parent.
    /// </summary>
    /// <param name="parent"></param>
    public Job(SelectJob parent)
    {
      InitializeComponent();

      _parent = parent;
      SelectedWorkClass = WorkClass.Undefined;
    }

    /// <summary>
    /// CompleteStatusChanged
    /// </summary>
    public event EventHandler<EventArgs> CompleteStatusChanged;

    /// <summary>
    /// SelectedWorkClass
    /// </summary>
    public WorkClass SelectedWorkClass { get; set; }

    /// <summary>
    /// IsComplete
    /// </summary>
    public bool IsComplete
    {
      get
      {
        return _parent.SelectedJob != null;
      }
    }

    /// <summary>
    /// Init
    /// </summary>
    public void Init()
    {
      _parent.RefreshJobs();

      var selectedIndex = -1;
      var counts = _parent.Jobs.Where(t => t.JobStatus == JobStatus.Active)
        .GroupBy(t => t.WorkClass).ToDictionary(k => k.Key, v => v.Count());
      lstWorkClasses.Items.Clear();
      foreach (WorkClass workClass in Enum.GetValues(typeof(WorkClass)))
      {
        if (workClass != WorkClass.Undefined)
        {
          var count = counts.ContainsKey(workClass) ? counts[workClass] : 0;
          var wci = new WorkClassItem(workClass, count);
          var index = lstWorkClasses.Items.Add(wci);

          if (workClass == SelectedWorkClass)
          {
            selectedIndex = index;
          }
        }
      }

      lstWorkClasses.SelectedIndex = selectedIndex;

      _InitJobList();
    }

    private void _InitJobList()
    {
      lstJobs.Items.Clear();
      foreach (var job in _parent.Jobs.Where(t => t.WorkClass == SelectedWorkClass && t.JobStatus == JobStatus.Active))
      {
        lstJobs.Items.Add(job);
      }

      lstJobs.SelectedIndex = -1;
      _parent.SelectedJob = null; //TODO: why is this necessary? SelectedIndexChanged should change it
    }

    private void Jobs_SelectedIndexChanged(object sender, EventArgs e)
    {
      _parent.SelectedJob = (AvayaMoagentClient.Job)lstJobs.SelectedItem;
      if (CompleteStatusChanged != null)
      {
        CompleteStatusChanged(this, EventArgs.Empty);
      }
    }

    private void WorkClasses_SelectedIndexChanged(object sender, EventArgs e)
    {
      var selected = (WorkClassItem)lstWorkClasses.SelectedItem;
      SelectedWorkClass = selected != null ? selected.WorkClass : WorkClass.Undefined;

      _InitJobList();
      if (CompleteStatusChanged != null)
      {
        CompleteStatusChanged(this, EventArgs.Empty);
      }
    }

    private class WorkClassItem
    {
      public WorkClassItem(WorkClass workClass, int count)
      {
        WorkClass = workClass;
        Count = count;
      }

      public WorkClass WorkClass { get; set; }

      public int Count { get; set; }

      public override string ToString()
      {
        string ret;

        if (Count > 0)
        {
          ret = string.Format("{0} ({1})", Enum.GetName(typeof(WorkClass), WorkClass), Count);
        }
        else
        {
          ret = string.Format("{0}", Enum.GetName(typeof(WorkClass), WorkClass));
        }

        return ret;
      }
    }
  }
}
