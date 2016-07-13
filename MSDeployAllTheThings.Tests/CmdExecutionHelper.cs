using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDeployAllTheThings.Tests.AppDeploy
{
    [TestFixture]
    public class CmdExecutionHelper : BaseTest
    {
        static internal void ExecuteCommand(string[] args, string workingDirectory, string fileName = "cmd.exe")
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = string.Join(" ", args),
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            Console.WriteLine($"command = {startInfo.Arguments}");
            var proc = Process.Start(startInfo);
            Console.WriteLine(proc.StandardOutput.ReadToEnd());
            Console.WriteLine(proc.StandardError.ReadToEnd());
            proc.WaitForExit();
        }
    }
}
