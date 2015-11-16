﻿using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace sscq.spiderman
{
    public class SpiderLogger
    {
        private const string PATH = "Log";
        private const string FILE_NAME = ".txt";
        private const string FULL_NAME = PATH + "/" + FILE_NAME;
        public static readonly object Locker = new object();
        private static StreamWriter WRITER;
        private static string DATE;

        private static string ContinueWriteCaches;
        private static readonly Stopwatch Continue_WriteSw;
        private static int ContinueTime = 300; // 300毫秒以后，连续写操作，都统一到一块操作
        private static int ContinueCountMax = 100; // 当连续写操作次数上限到指定的数值后，都写一次操作，之后的重新再计算
        private static int ContinueCount = 0;
        public static int AllWriteCount = 0;

        static SpiderLogger()
        {
            Continue_WriteSw = new Stopwatch();
        }

        private static string ProjectFullName
        {
            get
            {
                if (string.IsNullOrEmpty(DATE))
                    DATE =DateTime.Now.Date.ToString("yyyMMdd");
                return PATH + "/" +DATE+ FILE_NAME;
            }
        }

        private static void Write(string msg)
        {
            if (string.IsNullOrEmpty(msg)) return;

            lock (Locker)
            {
                if (Continue_WriteSw.IsRunning && Continue_WriteSw.ElapsedMilliseconds < ContinueTime)
                {
                    if (ContinueWriteCaches == null) ContinueWriteCaches = msg;
                    else ContinueWriteCaches += msg + "\r\n";
                    ContinueCount++;
                    if (ContinueCount > ContinueCountMax)
                    {
                        _Write();
                    }
                    return;
                }

                if (!Continue_WriteSw.IsRunning) Continue_WriteSw.Start();
                ContinueWriteCaches = msg;

                new Task(() =>
                {
                    Thread.Sleep(ContinueTime);
                    _Write();
                }).Start();
            }
        }

        private static void _Write()
        {
            if (ContinueWriteCaches != null)
            {
                if (!File.Exists(ProjectFullName))
                {
                    if (!Directory.Exists(PATH))
                        Directory.CreateDirectory(PATH);
                    //File.Create(ProjectFullName);
                }

                WRITER = new StreamWriter(ProjectFullName, true, Encoding.UTF8);
                WRITER.WriteLine(ContinueWriteCaches);
                WRITER.Flush();
                WRITER.Close();
            }
            Continue_WriteSw.Stop();
            Continue_WriteSw.Reset();
            ContinueWriteCaches = null;
            ContinueCount = 0;

            Interlocked.Increment(ref AllWriteCount);
        }

        public static void Debug(string msg)
        {
            Console.WriteLine(msg);
            msg = string.Format("[{0} {1}] : {2}", "Debug", DateTime.Now.ToString(), msg);
            Write(msg);
        }

        public static void Info(string msg)
        {
            Console.WriteLine(msg);
            msg = string.Format("[{0} {1}] : {2}", "Info", DateTime.Now.ToString(), msg);
            Write(msg);
        }

        public static void Error(string msg)
        {
            Console.WriteLine(msg);
            msg = string.Format("[{0} {1}] : {2}", "Error", DateTime.Now.ToString(), msg);
            Write(msg);
        }
    }
}
