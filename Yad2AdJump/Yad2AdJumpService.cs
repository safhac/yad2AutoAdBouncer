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
using OpenQA.Selenium.Chrome;

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
            //FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"timerserv.txt", FileMode.OpenOrCreate, FileAccess.Write);
            FileStream fs = new FileStream(@"c: timerserv.txt", FileMode.OpenOrCreate, FileAccess.Write);

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
            BounceAd();
            //// event handler
            //timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);

            //// a little over 4 hours
            //timer.Interval = 14500000;
            //// enable the timer
            //timer.Enabled = true;
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
            // call the bounce ad method
            BounceAd();
        }





        private void BounceAd()
        {

            ChromeOptions options = new ChromeOptions();
            options.AddArguments("user-data-dir=C:/Users/dell/AppData/Local/Google/Chrome/User Data");
            options.AddArguments("--start-maximized");


            using (var driver = new ChromeDriver(options))
            {

                //// url of yad2 personal area link
                //string personalAreaUrl = @"https://my.yad2.co.il/newOrder/index.php?action=personalAreaIndex";
                //// navigate to personal area
                //driver.Navigate().GoToUrl(personalAreaUrl);



                // in order to make this a more general purpose app consider starting at `personalAreaUrl`, 
                // collect all the available ad types and loop over them
                string url = @"https://my.yad2.co.il/newOrder/index.php?action=personalAreaFeed&CatID=3&SubCatID=0";


                driver.Navigate().GoToUrl(url);

                new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until<IWebElement>((t) =>
                {
                    // get the link to the ads table //*[@id="feed"] //*[@id="feed"]/tbody
                    IWebElement tableElement = t.FindElement(By.XPath("//*[@id='feed']"));

                    if (tableElement.Displayed && tableElement.Enabled && tableElement.GetAttribute("aria-disabled") == null)
                    {

                        IList<IWebElement> tableRow = tableElement.FindElements(By.XPath("//tr[contains(@class,'item')]"));

                        // loop over the table rows
                        foreach (IWebElement row in tableRow)
                        {
                            // click to expand the row
                            //row.Click();

                            driver.SwitchTo().Frame(1);
                            // find the ad bounce link
                            var bounceElem = driver.FindElement(By.XPath("//*[@id='bounceRatingOrderBtn']"));
                            // click to bounce the ad.
                            bounceElem.Click();
                        }
                        return tableElement;
                    }
                    return null;
                });
            }
        }

    }
}
