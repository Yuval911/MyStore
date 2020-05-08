using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStore.Forms;

namespace MyStore
{
    class Program
    {
        static void Main(string[] args)
        {
            MainForm mainForm = new MainForm(TestMode.Off);
            mainForm.StartScreen();
        }
    }
}