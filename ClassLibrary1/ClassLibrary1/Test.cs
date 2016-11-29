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
        public void ConnectionStepViaTypical_Positive()
        {
            string connection = "connection";
            var connectionStep = new ConnectionStep(Driver);
            connectionStep.GoToConnectionStepTypical();
            connectionStep.SetValidData();
            var next = connectionStep.GoNext();
            Assert.AreNotEqual(next.GetStepId(), connection);
        }

        [Test]
        [TestCase("", "", "", "")]
        [TestCase("=/*+", "546846", "a", "b")]
        [TestCase("10.35.176.255", "8006", "administrator", "123asdQ")]
        public void ConnectionStepViaTypical_Negative(string ip, string port, string username, string password)
        {
            string connection = "connection";
            var connectionStep = new ConnectionStep(Driver);
            connectionStep.GoToConnectionStepTypical();
            connectionStep.SetConnectionValues(ip, port, username, password);
            var next = connectionStep.GoNext();
            Assert.AreEqual(next.GetStepId(), connection);
        }

        [Test]
        [TestCase("", "", "", "")]
        [TestCase("=/*+", "546846", "a", "b")]
        [TestCase("10.35.176.255", "8006", "administrator", "123asdQ")]
        public void ConnectionStepViaAdvanced_Negative(string ip, string port, string username, string password)
        {
            string connection = "connection";
            var connectionStep = new ConnectionStep(Driver);
            connectionStep.GoToConnectionStepAdvanced();
            connectionStep.SetConnectionValues(ip, port, username, password);
            
            var next = connectionStep.GoNext();
            Assert.AreEqual(next.GetStepId(), connection);
        }

        [Test]
        public void ProtectionStepViaTypical_Positive()
        {
            string protection = "protection";
            var protectionStep = new ProtectionStep(Driver);
            protectionStep.GoToProtectionStep(ProtectionType.Typical);
            protectionStep.SetValidData();
            var next = protectionStep.GoNext();
            Assert.AreNotEqual(next.GetStepId(), protection);
        }


        [Test]
        public void ProtectionStepViaAdvanced_Positive()
        {
            string protection = "protection";
            var protectionStep = new ProtectionStep(Driver);
            protectionStep.GoToProtectionStep(ProtectionType.Advanced);
            protectionStep.SetValidData();
            var next = protectionStep.GoNext();
            Assert.AreNotEqual(next.GetStepId(), protection);
        }


        [Test]
        public void VolumesStepFistVolume_Positive()
        {
            string volumes = "volumes";

            var volumesStep = new VolumesStep(Driver);
            volumesStep.GoToVolumesStep(ProtectionType.Advanced);
            volumesStep.SetValidData();
            volumesStep.SelectVolumes(Volumes.Fisrt);
            var next = volumesStep.GoNext();

           Assert.AreNotEqual(next.GetStepId(), volumes);
        }

        [Test]
        public void VolumesStepNoneVolumes_Positive()
        {
            string volumes = "volumes";

            var volumesStep = new VolumesStep(Driver);
            volumesStep.GoToVolumesStep(ProtectionType.Advanced);
            volumesStep.SetValidData();
            volumesStep.SelectVolumes(Volumes.None);
            var next = volumesStep.GoNext();

            Assert.AreNotEqual(next.GetStepId(), volumes);
        }

        [Test]
        public void ScheduleStep_Positive()
        {
            string schedule = "schedule";

            var scheduleStep = new ScheduleStep(Driver);
            scheduleStep.GoToScheduleStep(ProtectionType.Advanced);
           // volumesStep.SetValidData();
          //  volumesStep.SelectVolumes(Volumes.None);
            scheduleStep.SetSchedulePeriods(true, false, false, "13:00 AM", "15:00 PM", "75", "55");
            var next = scheduleStep.GoNext();

            Assert.AreNotEqual(next.GetStepId(), schedule);
        }


    }
}
