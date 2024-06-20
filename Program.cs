using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DriverReplace
{
    internal class Program
    {
        [DllImport("libwdi.dll", EntryPoint = "DriverReplace")]
        public static extern void DriverReplace(int vid, int pid);
        static void Main(string[] args)
        {
            Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            var pri = (WindowsPrincipal)Thread.CurrentPrincipal;
            
            if (!pri.IsInRole(WindowsBuiltInRole.Administrator))
            {
                var proc = new ProcessStartInfo()
                {
                    WorkingDirectory = Environment.CurrentDirectory,
                    FileName = Assembly.GetEntryAssembly().Location,
                    Verb = "RunAs",
                    Arguments = string.Join(" ", args)
                };
                
                Process.Start(proc);
                return;
            }

            var vid = int.Parse(args[0]);
            var pid = int.Parse(args[1]);
            DriverReplace(vid, pid);
        }
    }
}
