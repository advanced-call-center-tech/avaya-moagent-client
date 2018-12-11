using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvayaDialerTestClient
{
  internal interface IPage
  {
    event EventHandler<EventArgs> CompleteStatusChanged;

    bool IsComplete { get; }

    void Init();
  }
}
