using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;

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
            FileStream fs = new FileStream(@"timerserv.txt", FileMode.OpenOrCreate, FileAccess.Write);

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

            //debug
            //BounceAd();
            // event handler
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);

            // a little over 4 hours 14500000;
            // every 12 hours
            timer.Interval = 43200000;
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
                // string url = @"https://my.yad2.co.il/newOrder/index.php?action=personalAreaFeed&CatID=3&SubCatID=0";

                // lazy option 
                // goto the specific frame url
                string frameUrl = @"https://my.yad2.co.il/newOrder/index.php?action=personalAreaViewDetails&CatID=3&SubCatID=0&OrderID=35345021";



                driver.Navigate().GoToUrl(frameUrl);

                new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until<IWebElement>((t) =>
                {
                    // get the link to the ads table //*[@id="feed"] //*[@id="feed"]/tbody
                    IWebElement bounceButton = t.FindElement(By.XPath("//*[@id='bounceRatingOrderBtn']"));

                    if (bounceButton.Displayed && bounceButton.Enabled && bounceButton.GetAttribute("aria-disabled") == null)
                    {
                        bounceButton.Click();
                        #region scrape iFrames
                        //IList<IWebElement> tableRow = tableElement.FindElements(By.XPath("//tr[contains(@class,'item')]"));

                        //// loop over the table rows
                        //foreach (IWebElement row in tableRow)
                        //{
                        //    // click to expand the row
                        //    //row.Click();

                        //    driver.SwitchTo().Frame(1);
                        //    // find the ad bounce link
                        //    var bounceElem = driver.FindElement(By.XPath("//*[@id='bounceRatingOrderBtn']"));
                        //    // click to bounce the ad.
                        //    bounceElem.Click();
                        //} 
                        #endregion
                        return bounceButton;
                    }
                    return null;
                });
            }
        }

    }
}
