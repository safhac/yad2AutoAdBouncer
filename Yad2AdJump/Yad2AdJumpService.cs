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
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

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

            // path to phantom driver
            const string PathToPhantonDriver = @"C:\Program Files (x86)\phantomjs-2.1.1-windows\bin";
            // holds the url to reach
            string url = string.Empty;


            using (var driver = new PhantomJSDriver(PathToPhantonDriver))
            {

                url = @"https://il.investing.com/stock-screener/?sp=country::23|sector::a|industry::a|equityType::a%3Ceq_market_cap;";

                driver.Navigate().GoToUrl(url);

            }
        }





        private void JumpAd()
        {
            // path to phantom driver
            const string PathToPhantonDriver = @"C:\Program Files (x86)\phantomjs-2.1.1-windows\bin";
           


            using (var driver = new PhantomJSDriver(PathToPhantonDriver))
            {
                
                // url of yad2 personal area link
                string personalAreaUrl = @"https://my.yad2.co.il/newOrder/index.php?action=personalAreaIndex";

                // in order to make this a more general purpose app consider starting at `personalAreaUrl`, 
                // collect all the available ads and loop over them
                string url = @"https://my.yad2.co.il/newOrder/index.php?action=personalAreaFeed&CatID=3&SubCatID=0";

                driver.Navigate().GoToUrl(url);

                new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until<IWebElement>((t) =>
                {
                    // get the link to the ad
                    IWebElement linkElement = t.FindElement(By.XPath("//*[@id='resultsTable']/tbody"));

                    if (linkElement.Displayed && linkElement.Enabled && linkElement.GetAttribute("aria-disabled") == null)
                    {
                        return linkElement;
                    }
                    return null;
                });
            }
        }

    }
}
