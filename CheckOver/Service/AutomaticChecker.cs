using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CheckOver
{
    public class AutomaticChecker
    {
        public string RelativePath { get; set; }
        public string ParametersToCheck { get; set; }
        public string[] ParametersToCheckList { get; set; }
        public string ValidCode { get; set; }
        public string FileOfValidCode { get; set; }
        public string CodeToCheck { get; set; }
        public string FileOfCodeToCheck { get; set; }
        public string AlgorithmOutcome { get; set; }
        public string Errors { get; set; }

        public AutomaticChecker(string parameters, string validcode, string codetocheck, string relativepath = null)
        {
            ParametersToCheck = parameters;
            ValidCode = validcode;
            CodeToCheck = codetocheck;
            if (relativepath == null) { RelativePath = Environment.CurrentDirectory + "\\Dane"; }
            else { RelativePath = relativepath; }
            ParametersToCheckList = ParametersToCheck.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }

        public void makeFilesToCompile()
        {
            FileOfCodeToCheck = "CodeToCheck.cs";
            using (StreamWriter file = new StreamWriter(RelativePath + "\\" + FileOfCodeToCheck))
            {
                file.WriteLine(CodeToCheck);
            }
            FileOfValidCode = "ValidCode.cs";
            using (StreamWriter file = new StreamWriter(RelativePath + "\\" + FileOfValidCode))
            {
                file.WriteLine(ValidCode);
            }
        }

        public void CheckIfExeExist(string file)
        {
            file = Path.ChangeExtension(file, ".exe");
            if (File.Exists(RelativePath + "\\" + file))
            {
                File.Delete(RelativePath + "\\" + file);
            }
        }

        public void makeExe(string file)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            Thread.Sleep(500);
            cmd.StandardInput.WriteLine("cd " + RelativePath);
            Thread.Sleep(500);
            cmd.StandardInput.WriteLine("csc " + file);
            Thread.Sleep(500);
            cmd.Close();
        }

        public string makeProcess(string file, string arguments)
        {
            string outcome = "";
            file = Path.ChangeExtension(file, ".exe");
            Process myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.RedirectStandardError = true;
            myProcess.StartInfo.FileName = RelativePath + "\\" + file;
            myProcess.StartInfo.CreateNoWindow = false;
            myProcess.StartInfo.RedirectStandardOutput = true;
            myProcess.StartInfo.RedirectStandardInput = true;
            myProcess.StartInfo.Arguments = arguments;
            myProcess.Start();
            StreamWriter sw = myProcess.StandardInput;
            sw.WriteLine(arguments);
            outcome = myProcess.StandardOutput.ReadToEnd();
            Errors += myProcess.StandardError.ReadToEnd() + "\n";
            myProcess.WaitForExit();
            myProcess.Close();

            return outcome;
        }

        public async Task<string> run()
        {
            try
            {
                makeFilesToCompile();
                CheckIfExeExist(FileOfValidCode);
                CheckIfExeExist(FileOfCodeToCheck);
                makeExe(FileOfValidCode);
                makeExe(FileOfCodeToCheck);
            }
            catch (Exception e)
            {
                Errors += "Exception caught: " + e + "\n";
            }
            if (!File.Exists(RelativePath + "\\" + Path.ChangeExtension(FileOfValidCode, ".exe")))
            {
                Errors += "Prawidłowy kod nie skompilował się\n";
                return Errors;
            }
            if (!File.Exists(RelativePath + "\\" + Path.ChangeExtension(FileOfCodeToCheck, ".exe")))
            {
                Errors += "Kod do sprawdzenia nie skompilował się\n";
                return Errors;
            }
            AlgorithmOutcome = "Argumenty\tWyjście kodu prawidłowego\tWyjście kodu sprawdzego\tDecyzja\n";
            for (int i = 0; i < ParametersToCheckList.Length; i++)
            {
                string outcome1 = makeProcess(FileOfValidCode, ParametersToCheckList[i].Replace(" ", "\n")).Replace("\r\n", " ");
                string outcome2 = makeProcess(FileOfCodeToCheck, ParametersToCheckList[i].Replace(" ", "\n")).Replace("\r\n", " ");
                AlgorithmOutcome += ParametersToCheckList[i] + "\t\t\t" + outcome1 + "\t\t\t" + outcome2 + "\t\t\t";
                if (outcome1 == outcome2) { AlgorithmOutcome += "Yes\n"; }
                else { AlgorithmOutcome += "No\n"; }
            }
            return AlgorithmOutcome;
        }
    }
}