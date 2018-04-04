using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Timers; 
using System.IO; 
using System.Text;
using System.Threading.Tasks;

namespace Yad2AdJump
{
    

    public partial class Yad2AdJumpService : ServiceBase
    {

        //Initialize the timer
        Timer timer = new Timer();





        public Yad2AdJumpService()
        {
            InitializeComponent();
        }





        /// <summary>
        /// running in debug mode
        /// </summary>
        public void OnDebug()
        {
            OnStart(null);
        }






        /// <summary>
        /// writes text to a file
        /// </summary>
        /// <param name="text">the text to be written</param>
        private void WriteToFile(string text)
        {

            //set up a filestream
            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"timerserv.txt", FileMode.OpenOrCreate, FileAccess.Write);

            //set up a streamwriter for adding text
            StreamWriter sw = new StreamWriter(fs);

            //find the end of the underlying filestream
            sw.BaseStream.Seek(0, SeekOrigin.End);

            //add the text 
            sw.WriteLine(text);
            
            //add the text to the underlying filestream
            sw.Flush();
            //close the writer
            sw.Close();
        }






        protected override void OnStart(string[] args)
        {
            WriteToFile("Starting Service");

            // event handler
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);

            // a little over 4 hours
            timer.Interval = 14500000;

            // enable the timer
            timer.Enabled = true;
        }






        protected override void OnStop()
        {

            WriteToFile("Starting Service");
            // disable the timer
            timer.Enabled = false;


        }






        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteToFile("Another entry");
        }

    }
}
