using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AvayaDialerTestClient
{
  /// <summary>
  /// Startup
  /// </summary>
  public static class Startup
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main()
    {
      Application.Run(new Main());
    }
  }
}
