using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using OpenQA.Selenium.Support.UI;
using System.Globalization;
using System.Linq;
using Replay.Logging;
using Replay.Core.Contracts.Agents;
using Replay.Core.Client;
using OpenQA.Selenium.Support.PageObjects;
using NUnit.Framework;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support;
using WebDriverFactoryDotNet;
using NUnit;
using NUnit.Compatibility;

namespace TestAutomation
{
    [TestFixture(BrowserType.Chrome)]
    [TestFixture(BrowserType.Firefox)]
    [TestFixture(BrowserType.IE)]

   public class MainPage : WebDriveFactory 
    {
        public MainPage(BrowserType browser) : base(browser)
            {

            }

            [Test]
            public void SetUpEnvironment()
            {

                string Url = @"https://administrator:123asdQ@10.35.176.166:8006/apprecovery/admin";
                 Driver.Navigate().GoToUrl(Url);
            }
        }
    }

