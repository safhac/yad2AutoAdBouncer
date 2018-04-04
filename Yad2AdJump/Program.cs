using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Yad2AdJump
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            Yad2AdJumpService service = new Yad2AdJumpService();
            service.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

#else


            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Yad2AdJumpService()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
