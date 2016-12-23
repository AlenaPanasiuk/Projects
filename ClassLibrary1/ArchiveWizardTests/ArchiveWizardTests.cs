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
using ArchiveWizardTests.StepsArchiveWizard;
using ArchiveWizardTests.Helpers;
using Replay.Core.Contracts.Agents;
using Replay.Logging;
using Replay.Core.Client;

namespace ArchiveWizardTests
{
    public class ArchiveWizardTests : TestBase
    {
        public ArchiveWizardTests(BrowserType browser)
            : base(browser)
        {
        }


        [Test]
        [TestCaseSource("ArchiveTypeStepPositiveTestCases")]
        public void ArchiveTypeStepPositive(ArchiveType type)
        {
            UITest(() =>
            {
                var archiveTypeStep = new ArchiveTypeStep(Driver);
                archiveTypeStep.SetArchiveType(type);

              //  welcomeStep.GoToFinish();
             //   Assert.AreEqual(welcomeStep.GetStepId(), string.Empty);
            }, "ArchiveStepPositive");
        }


        [Test]
        [TestCaseSource("ArchiveTypeStepPositiveTestCases")]
        public void LocationStepPositive(ArchiveType type)
        {
            UITest(() =>
            {
                var locationStep = new LocationStep(Driver);
                locationStep.GoToThisStep(type);
                locationStep.setLocation(LocationType.Cloud, @"C:\ewe", @"\\10.35.175.175\d3", "q1", "123");
               // locationStep.SetArchiveType(type);

                //  welcomeStep.GoToFinish();
                //   Assert.AreEqual(welcomeStep.GetStepId(), string.Empty);
            }, "ArchiveStepPositive");
        }


        public static IEnumerable ArchiveTypeStepPositiveTestCases
        {
            get
            {
                yield return new TestCaseData(ArchiveType.OneTime);
                yield return new TestCaseData(ArchiveType.Continuous);
            }
        }
    }
}
