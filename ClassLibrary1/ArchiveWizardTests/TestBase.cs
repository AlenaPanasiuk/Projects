using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using OpenQA.Selenium.Support.UI;
using System.Globalization;
using System.Linq;
using OpenQA.Selenium.Support.PageObjects;
using NUnit.Framework;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support;
using NUnit;
using NUnit.Compatibility;
//using Replay.Core.Contracts.Agents;
using Replay.Logging;
//using Replay.Core.Client;
using System.Drawing.Imaging;
using System.Xml.Linq;
using System.Drawing;
using System.ComponentModel;
using ArchiveWizardTests.Helpers;
using ArchiveWizardTests.StepsArchiveWizard;


namespace ArchiveWizardTests
{
    [TestFixture(BrowserType.Chrome)]
  //  [TestFixture(BrowserType.Firefox)]
 //   [TestFixture(BrowserType.IE)]
    public class TestBase : WebDriveFactory
    {
        public const string Agent1UserName = "administrator";
        public const string Agent1Password = "123asdQ";
        public const string CoreUserName = "administrator";
        public const string CorePassword = "123asdQ";
        public const string Agent1Port = "8006";


        // The host name or IPv4 address of machine where the AppRecovery core is located
        private string CoreHostName = Properties.Settings.Default.CoreHostName;

        // The port of machine where the AppRecovery core is located
        private int CoreApiPort = Properties.Settings.Default.CorePort;

        public TestBase(BrowserType browser)
            : base( browser)
        {
        }

        protected void UITest(Action action, string testName)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot(); 
                DateTime time = DateTime.Now;
                string dateToday = "_date_" + time.ToString("yyyy-MM-dd") + "_time_" + time.ToString("HH-mm-ss");
                var filePath = @"C:\screenshots\";
                var filename = filePath + testName + dateToday + ".png";
                screenshot.SaveAsFile(filename, ImageFormat.Png);
                Bitmap bitmap = new Bitmap(filename);
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Bitmap));
                throw;
            } 
        }

        public static XElement ImageToXElement(System.Drawing.Image image)
        {
            Bitmap bitmap = new Bitmap(image);
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Bitmap));
            return new XElement("Image",
                new XAttribute("PixelData",
                    Convert.ToBase64String(
                    (byte[])converter.ConvertTo(image, typeof(byte[])))));
        }


        [OneTimeSetUp]
        public void CreateAmazoneAccounts()
        {
            APIHelpers helper = new APIHelpers(CoreHostName, CoreApiPort);
            helper.AddCloudAccounts();
            helper.CreateRepository();
            helper.ProtectAgent();
        }

        [SetUp]
        public void OpenWizard()
        {
            
            string url = String.Format("https://{0}:{1}@{2}:{3}/apprecovery/admin", CoreUserName, CorePassword, CoreHostName, CoreApiPort);
            Driver.Navigate().GoToUrl(url);
            Driver.Manage().Window.Maximize();
            WebDriverWait wait = new WebDriverWait(Driver, new TimeSpan(0, 0, 1800));
            try
            {
                Driver.FindElement(By.XPath(".//*[@id='popup1']/div/div[5]/div/div[5]/button[1]")).Click();
            }
            catch { }
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[@id='archiveMachine']/div[2]/div/span")));
            Driver.FindElement(By.XPath(".//*[@id='archiveMachine']/div[2]/div/span")).Click() ;
            Driver.FindElement(By.XPath(".//*[@id='archiveMachine']/div[2]/ul/li[1]/a")).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("wizardContentContainer")));
            
        }

        [TearDown]
        public void CleanUp()
        {
            APIHelpers helper = new APIHelpers(CoreHostName, CoreApiPort);
           // helper.TestResults();
          //  helper.DeleteAgents();
           // helper.DeleteRepositories();
           // helper.DeleteCloudAccounts();
        }

        public StepBase GoToStep(string targetStepName, SetDataType setDataType)
        {
            StepBase step = new ArchiveTypeStep(Driver);
            while (targetStepName != step.GetName())
            {
                if (setDataType == SetDataType.Default)
                {
                    step.SetValidData();
                    step = step.GoNext();
                }
                else 
                {
                    step.SetCustomeValidData();
                    step = step.GoNext();
                }
            }
            return step;
        }

        public enum SetDataType
        {
            Default,
            Custom
        }
    }
}
