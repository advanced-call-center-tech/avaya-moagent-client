using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AvayaDialerTestClient
{
  /// <summary>
  /// SelectJob
  /// </summary>
  public partial class SelectJob : Form
  {
    private const string _DEFAULT_REGEX_FILTER = ".*";
    private AvayaControl _parent;
    private Dictionary<Page, IPage> _pages;
    private Page _currentPage;

    /// <summary>
    /// Creates a SelectJob form with the specified AvayaControl parent.
    /// </summary>
    /// <param name="parent"></param>
    public SelectJob(AvayaControl parent)
    {
      InitializeComponent();

      _parent = parent;
      SelectedBlendMode = AvayaMoagentClient.Enumerations.BlendMode.Undefined;
      JobRegexFilter = _DEFAULT_REGEX_FILTER;
    }

    private enum Page
    {
      Job,
      Mode
    }

    /// <summary>
    /// JobRegexFilter
    /// </summary>
    public string JobRegexFilter { get; set; }

    /// <summary>
    /// Jobs
    /// </summary>
    public List<AvayaMoagentClient.Job> Jobs { get; private set; }

    /// <summary>
    /// Selected job.
    /// </summary>
    public AvayaMoagentClient.Job SelectedJob { get; set; }

    /// <summary>
    /// Selected blend mode.
    /// </summary>
    public AvayaMoagentClient.Enumerations.BlendMode SelectedBlendMode { get; set; }

    /// <summary>
    /// RefreshJobs
    /// </summary>
    public void RefreshJobs()
    {
      Jobs = _parent.GetAllJobs().Where(t => Regex.IsMatch(t.JobName, JobRegexFilter)).ToList();
    }

    private void SelectJob_Load(object sender, EventArgs e)
    {
      _pages = new Dictionary<Page, IPage>();
      var job = new Job(this);
      job.CompleteStatusChanged += _JobCompleteStatusChanged;
      _pages.Add(Page.Job, job);
      var mode = new Mode(this);
      mode.CompleteStatusChanged += _ModeCompleteStatusChanged;
      _pages.Add(Page.Mode, mode);

      btnBack.Enabled = false;
      btnNext.Enabled = false;
      btnFinish.Enabled = false;

      _Move(Page.Job);
    }

    private void _JobCompleteStatusChanged(object sender, EventArgs e)
    {
      btnBack.Enabled = false;
      var page = (Job)_pages[Page.Job];
      if (page.SelectedWorkClass != AvayaMoagentClient.Enumerations.WorkClass.Undefined && SelectedJob != null)
      {
        switch (page.SelectedWorkClass)
        {
          case AvayaMoagentClient.Enumerations.WorkClass.Blend:
            {
              btnNext.Enabled = true;
              btnFinish.Enabled = false;
              break;
            }

          default:
            {
              btnNext.Enabled = false;
              btnFinish.Enabled = true;
              break;
            }
        }
      }
      else
      {
        btnNext.Enabled = false;
        btnFinish.Enabled = false;
      }
    }

    private void _ModeCompleteStatusChanged(object sender, EventArgs e)
    {
      btnBack.Enabled = true;
      btnNext.Enabled = false;
      btnFinish.Enabled = true;
    }

    private void Back_Click(object sender, EventArgs e)
    {
      btnNext.Enabled = true;
      btnFinish.Enabled = false;

      _Move(Page.Job);
    }

    private void Next_Click(object sender, EventArgs e)
    {
      btnBack.Enabled = true;
      btnNext.Enabled = false;

      if (_pages[_currentPage].IsComplete)
      {
        switch (_currentPage)
        {
          case Page.Job:
            {
              if (SelectedJob != null)
              {
                if (SelectedJob.WorkClass == AvayaMoagentClient.Enumerations.WorkClass.Blend)
                {
                  _Move(Page.Mode);
                }
                else
                {
                  //Get out
                  btnFinish.PerformClick();
                }
              }

              break;
            }

          case Page.Mode:
            {
              //Get out
              btnFinish.PerformClick();
              break;
            }

          default:
            {
              //Uh...don't know...
              break;
            }
        }
      }
      else
      {
        MessageBox.Show("Please complete this page before continuing.",
          "Cannot Continue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void Finish_Click(object sender, EventArgs e)
    {
      if (_pages[_currentPage].IsComplete)
      {
        this.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.Close();
      }
      else
      {
        MessageBox.Show("Please complete this page before proceeding.", "Cannot Finish",
          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void Refresh_Click(object sender, EventArgs e)
    {
      tmrRefresh.Stop();

      btnRefresh.Enabled = false;
      RefreshJobs();

      tmrRefresh.Start();
    }

    private void Refresh_Tick(object sender, EventArgs e)
    {
      btnRefresh.Enabled = true;
    }

    private void _Move(Page page)
    {
      var ctrl = (UserControl)_pages[page];

      pnlBody.Controls.Clear();
      pnlBody.Controls.Add(ctrl);
      ctrl.Dock = DockStyle.Fill;

      _currentPage = page;

      btnRefresh.Enabled = (_currentPage == Page.Job);
      btnRefresh.Visible = (_currentPage == Page.Job);

      _pages[page].Init();
    }
  }
}
