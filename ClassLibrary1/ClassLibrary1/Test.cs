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
using ProtectWizardTests.Steps;
using ProtectWizardTests.Helpers;
using Replay.Core.Contracts.Agents;
using Replay.Logging;
using Replay.Core.Client;

namespace ProtectWizardTests
{
    public class Test : TestBase 
    {
        public Test(BrowserType browser)
            : base( browser)
        {
        }

        [Test]
        public void ConnectionStep_Positive()
        {
            string connection = "Connection";

            var connectionStep = GoToStep(connection, SetDataType.Default);

            connectionStep.SetValidData();

            var next = connectionStep.GoNext();

            Assert.AreNotEqual(next.GetName(), connection);
        }

        [Test]
        [TestCase("", "", "", "")]
        [TestCase("=/*+", "546846", "a", "b")]
        [TestCase("10.35.176.255", "8006", "administrator", "123asdQ")]
        public void ConnectionStep_Negative(string ip, string port, string username, string password)
        {
            string connection = "Connection";

            var connectionStep = (ConnectionStep)GoToStep(connection, SetDataType.Default);

            connectionStep.SetConnectionValues(ip, port, username, password);

            var next = connectionStep.GoNext();

            Assert.AreEqual(next.GetName(), connection);
        }


        [Test]
        public void ProtectionStep_Positive()
        {
            string protection = "Protection";

            var protectionStep = GoToStep(protection, SetDataType.Default);

            protectionStep.SetValidData();

            var next = protectionStep.GoNext();

            Assert.AreNotEqual(next.GetName(), protection);
        }


        [Test]
        public void VolumesStep_Positive()
        {
            string volumes = "Protection Volumes";

            var volumesStep = GoToStep(volumes, SetDataType.Custom);

            volumesStep.SetValidData();

            //var next = volumesStep.GoNext();

           // Assert.AreNotEqual(next.GetName(), volumes);
        }



    }
}
