using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AvayaPdsService
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main(string[] args)
    {
      if (args.Contains("/test"))
      {
        var service = new AvayaPdsService();
        service.Start();

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey(true);
      }
      else
      {
        var servicesToRun = new ServiceBase[]
        {
          new AvayaPdsService()
        };

        ServiceBase.Run(servicesToRun);
      }
    }
  }
}
