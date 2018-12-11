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
  /// Mode
  /// </summary>
  public partial class Mode : UserControl, IPage
  {
    private SelectJob _parent;

    /// <summary>
    /// Creates a Mode user control with the specified SelectJob parent.
    /// </summary>
    /// <param name="parent"></param>
    public Mode(SelectJob parent)
    {
      InitializeComponent();

      _parent = parent;
    }

    /// <summary>
    /// CompleteStatusChanged
    /// </summary>
    public event EventHandler<EventArgs> CompleteStatusChanged;

    /// <summary>
    /// IsComplete
    /// </summary>
    public bool IsComplete
    {
      get
      {
        return _parent.SelectedBlendMode != AvayaMoagentClient.Enumerations.BlendMode.Undefined;
      }
    }

    /// <summary>
    /// Init
    /// </summary>
    public void Init()
    {
      lstBlendMode.Items.Clear();
      foreach (BlendMode blendMode in Enum.GetValues(typeof(BlendMode)))
      {
        if (blendMode != BlendMode.Undefined)
        {
          lstBlendMode.Items.Add(Enum.GetName(typeof(BlendMode), blendMode));
        }
      }
    }

    private void BlendMode_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (lstBlendMode.SelectedItem != null)
      {
        _parent.SelectedBlendMode = (BlendMode)Enum.Parse(typeof(BlendMode), lstBlendMode.SelectedItem.ToString(), true);
      }

      if (CompleteStatusChanged != null)
      {
        CompleteStatusChanged(this, EventArgs.Empty);
      }
    }
  }
}
