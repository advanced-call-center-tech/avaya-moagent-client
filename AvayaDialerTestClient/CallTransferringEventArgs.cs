using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvayaMoagentClient;

namespace AvayaDialerTestClient
{
  /// <summary>
  /// CallTransferringEventArgs
  /// </summary>
  public class CallTransferringEventArgs : EventArgs
  {
    /// <summary>
    /// TransferNumber
    /// </summary>
    public TransferNumber TransferNumber { get; set; }
  }
}
