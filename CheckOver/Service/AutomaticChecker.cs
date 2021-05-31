using System;
using System.IO;
using System.Globalization;
using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System.Diagnostics;

namespace CheckOver
{
    public class AutomaticChecker
    {
        public static void Run()
        {
            string path = Directory.GetCurrentDirectory();
            string sciezka = path + "\\Skrypty";
            string argumenty = "5";
            string plik = "Program1.cs";
            egzekutor(sciezka, plik);
            string plik2 = "Program3.cs";
            egzekutor(sciezka, plik2);
            System.Threading.Thread.Sleep(1000);
            var wynik = Wywolywanie(sciezka + "\\Program1.exe", argumenty);
            var wynik2 = Wywolywanie(sciezka + "\\Program3.exe", argumenty);
            if (wynik == wynik2)
                Console.WriteLine("Działa");
            else
                Console.WriteLine("Nie dziala");
        }

        public static string Wywolywanie(string a, string arguments)
        {
            string wynik = "";
            string plik = a;
            Process myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.FileName = plik;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.RedirectStandardOutput = true;
            myProcess.StartInfo.Arguments = arguments;
            myProcess.Start();
            wynik = myProcess.StandardOutput.ReadToEnd();
            myProcess.WaitForExit();
            myProcess.Close();
            return wynik;
        }

        public static void egzekutor(string sciezka, string plik)
        {
            System.Threading.Thread.Sleep(100);
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            System.Threading.Thread.Sleep(100);
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            System.Threading.Thread.Sleep(100);
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            System.Threading.Thread.Sleep(100);
            cmd.StandardInput.WriteLine("cd " + sciezka);
            System.Threading.Thread.Sleep(100);
            cmd.StandardInput.WriteLine("csc " + plik);
            System.Threading.Thread.Sleep(100);
            cmd.Close();
        }
    }
}