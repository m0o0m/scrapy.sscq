/*
Project Orleans Cloud Service SDK ver. 1.0
 
Copyright (c) Microsoft Corporation
 
All rights reserved.
 
MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
associated documentation files (the ""Software""), to deal in the Software without restriction,
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Threading.Tasks;
//using Microsoft.Win32;
//using System.Runtime.InteropServices;
//using System.Windows.Forms;

namespace sscq.server
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            
            // The Orleans silo environment is initialized in its own app domain in order to more
            // closely emulate the distributed situation, when the client and the server cannot
            // pass data via shared memory.
            AppDomain hostDomain = AppDomain.CreateDomain("OrleansHost", null, new AppDomainSetup
            {
                AppDomainInitializer = InitSilo,
                AppDomainInitializerArguments = args,
            });

            Orleans.GrainClient.Initialize("DevTestClientConfiguration.xml");

            // TODO: once the previous call returns, the silo is up and running.
            //       This is the place your custom logic, for example calling client logic
            //       or initializing an HTTP front end for accepting incoming requests.                 

            Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            Console.ReadLine();

            hostDomain.DoCallBack(ShutdownSilo);            
        }

        private static OrleansHostWrapper hostWrapper;
        static void InitSilo(string[] args)
        {
            hostWrapper = new OrleansHostWrapper(args);

            if (!hostWrapper.Run())
            {
                Console.Error.WriteLine("Failed to initialize Orleans silo");
            }
        }

        static void ShutdownSilo()
        {
            if (hostWrapper != null)
            {
                hostWrapper.Dispose();
                GC.SuppressFinalize(hostWrapper);
            }
        }

        //private static IntPtr consoleHander;     

        //[DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        //static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        //[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]      
        //static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        //[DllImport("user32.dll", EntryPoint = "SendMessageA")]
        //public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
 
        ///// <summary>
        ///// 隐藏控制台.
        ///// </summary>
        ///// <param name="consoleTitle">窗体的Title</param>
        //static void HideConsoleWindow()
        //{
        //    string path = System.Windows.Forms.Application.StartupPath;
        //    consoleHander = FindWindow("ConsoleWindowClass", null);
        //    if (consoleHander != IntPtr.Zero)
        //        ShowWindow(consoleHander, 0);//隐藏窗口
        //    else
        //         Console.Error.WriteLine("PhantomBrowser.HideBrowser(). can't hide console window");
        //}

        
    }
}
