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
using ProtectWizardTests.StepsProtectWizard;
using ProtectWizardTests.Helpers;
using Replay.Core.Contracts.Agents;
using Replay.Logging;
using Replay.Core.Client;

namespace ProtectWizardTests
{
    public class ProtectionWizardTests : TestBase 
    {
        public ProtectionWizardTests(BrowserType browser)
            : base(browser)
        {
        }

        [Test]
        [TestCaseSource("WelcomeStepPositiveTestCases")]
        public void WelcomeStepPositive(ProtectionType type)
        {
            UITest(() =>
           {
               var welcomeStep = new WelcomeStep(Driver);
               welcomeStep.SetProtectionType(type);

               welcomeStep.GoToFinish();
               Assert.AreEqual(welcomeStep.GetStepId(), string.Empty);
           }, "WelcomeStepPositive");
        }

        [Test]
        [TestCaseSource("ConnectionStepPositiveTestCases")]
        public void ConnectionStepPositive(ProtectionType type, string ip, string port, string username, string password)
        {
            UITest(() =>
           {
               string connection = "connection";
               var connectionStep = new ConnectionStep(Driver);
               connectionStep.GoToThisStep(type);
               connectionStep.SetConnectionValues(ip, port, username, password);
               var next = connectionStep.GoNext();
               Assert.AreNotEqual(next.GetStepId(), connection);

               next.GoToFinish();
               Assert.AreEqual(next.GetStepId(), string.Empty);
           }, "ConnectionStepPositive");
        }

        [Test]
        [TestCaseSource("ConnectionStepNegativeTestCases")]
        public void ConnectionStepNegative(ProtectionType type, string ip, string port, string username, string password)
        {
            UITest(() =>
            {
                string connection = "connection";
                var connectionStep = new ConnectionStep(Driver);
                connectionStep.GoToThisStep(type);
                connectionStep.SetConnectionValues(ip, port, username, password);
                var next = connectionStep.GoNext();
                Assert.AreEqual(next.GetStepId(), connection);
            }, "ConnectionStepNegative");
        }

        [Test]
        [TestCaseSource("ProtectionStepPositiveTestCases")]
        public void ProtectionStepPositive(ProtectionType type, string displayName, ProtectionSchedule schedule)
        {
            UITest(() =>
              {
                  string protection = "protection";
                  var protectionStep = new ProtectionStep(Driver);
                  protectionStep.GoToThisStep(type);
                  protectionStep.SetProtectionValues(displayName, schedule, false);
                  var next = protectionStep.GoNext();
                  Assert.AreNotEqual(next.GetStepId(), protection);

                  next.GoToFinish();
                  Assert.AreEqual(next.GetStepId(), string.Empty);
              }, "ProtectionStepPositive");
        }

        [Test]
        [TestCaseSource("ProtectionStepNegativeTestCases")]
        public void ProtectionStepNegative(ProtectionType type, string displayName, ProtectionSchedule schedule)
        {
            UITest(() =>
              {
                  string protection = "protection";
                  var protectionStep = new ProtectionStep(Driver);
                  protectionStep.GoToThisStep(type);
                  protectionStep.SetProtectionValues(displayName, schedule, false);
                  var next = protectionStep.GoNext();
                  Assert.AreEqual(next.GetStepId(), protection);
              }, "ProtectionStepNegative");
        }

        [Test]
        [TestCaseSource("VolumesStepPositiveTestCases")]
        public void VolumesStepPositive(Volumes volumesForProtection, ProtectionType type)
        {
            UITest(() =>
              {
                  string volumes = "volumes";
                  var volumesStep = new VolumesStep(Driver);
                  volumesStep.GoToThisStep(type);
                  volumesStep.SetValidData();
                  volumesStep.SelectVolumes(volumesForProtection);
                  var next = volumesStep.GoNext();
                  Assert.AreNotEqual(next.GetStepId(), volumes);

                  next.GoToFinish();
                  Assert.AreEqual(next.GetStepId(), string.Empty);
              }, "VolumesStepPositive");
        }

        [Test]
        [TestCaseSource("ScheduleStepPeriodsPositiveTestCases")]
        public void ScheduleStepPeriodsPositive(ProtectionType type, bool weekdays, bool weekends, bool protectWeekdaysRest, string from, string to, string weekdaysPeriopd, string weekendsPeriod, string weekdaysRestPeriod, bool pause)
        {
            UITest(() =>
              {
                  string schedule = "schedule";
                  var scheduleStep = new ScheduleStep(Driver);
                  scheduleStep.GoToThisStep(type);
                  scheduleStep.SetSchedule(weekdays, weekends, protectWeekdaysRest, from, to, weekdaysPeriopd, weekendsPeriod, weekdaysRestPeriod, pause);
                  var next = scheduleStep.GoNext();
                  Assert.AreNotEqual(next.GetStepId(), schedule);

                  next.GoToFinish();
                  Assert.AreEqual(next.GetStepId(), string.Empty);
              }, "ScheduleStepPeriodsPositive");
        }

        [Test]
        [TestCaseSource("ScheduleStepDailyPositiveTestCases")]
        public void ScheduleStepDailyPositive(ProtectionType type, string protectionTimeDaily, bool pause)
        {
            UITest(() =>
              {
                  string schedule = "schedule";
                  var scheduleStep = new ScheduleStep(Driver);
                  scheduleStep.GoToThisStep(type);
                  scheduleStep.SetSchedule(protectionTimeDaily, pause);
                  var next = scheduleStep.GoNext();
                  Assert.AreNotEqual(next.GetStepId(), schedule);

                  next.GoToFinish();
                  Assert.AreEqual(next.GetStepId(), string.Empty);
              }, "ScheduleStepDailyPositive");
        }

        [Test]
        [TestCaseSource("ScheduleStepPeriodsNegativeTestCases")]
        public void ScheduleStepPeriodsNegative(ProtectionType type, bool weekdays, bool weekends, bool protectWeekdaysRest, string from, string to, string weekdaysPeriopd, string weekendsPeriod, string weekdaysRestPeriod, bool pause)
        {
            UITest(() =>
              {
                  string schedule = "schedule";
                  var scheduleStep = new ScheduleStep(Driver);
                  scheduleStep.GoToThisStep(type);
                  scheduleStep.SetSchedule(weekdays, weekends, protectWeekdaysRest, from, to, weekdaysPeriopd, weekendsPeriod, weekdaysRestPeriod, pause);
                  var next = scheduleStep.GoNext();
                  Assert.AreEqual(next.GetStepId(), schedule);
              }, "ScheduleStepPeriodsNegative");
        }

        [Test]
        [TestCaseSource("ScheduleStepDailyNegativeTestCases")]
        public void ScheduleStepDailyNegative(ProtectionType type, string protectionTimeDaily, bool pause)
        {
            UITest(() =>
              {
                  string schedule = "schedule";
                  var scheduleStep = new ScheduleStep(Driver);
                  scheduleStep.GoToThisStep(type);
                  scheduleStep.SetSchedule(protectionTimeDaily, pause);
                  var next = scheduleStep.GoNext();
                  Assert.AreEqual(next.GetStepId(), schedule);
              }, "ScheduleStepDailyNegative");

        }

        [Test]
        [TestCaseSource("RepositoryStepNewLocalPositiveTestCases")]
        public void RepositoryStepNewLocalPositive(string repoName, string dataPath, string metadataPath)
        {
            UITest(() =>
              {
                  string repository = "repository";
                  var repositoryStep = new RepositoryStep(Driver);
                  repositoryStep.GoToThisStep(ProtectionType.Advanced);
                  repositoryStep.SetRepository(repoName, dataPath, metadataPath);
                  var next = repositoryStep.GoNext();
                  Assert.AreNotEqual(next.GetStepId(), repository);

                  next.GoToFinish();
                  Assert.AreEqual(repositoryStep.GetStepId(), string.Empty);
              }, "RepositoryStepNewLocalPositive");
        }

        [Test]
        [TestCaseSource("RepositoryStepNewCIFSPositiveTestCases")]
        public void RepositoryStepNewCIFSPositive(string repoName, string cifsLocation, string cifsUser, string cifsPassword)
        {
            UITest(() =>
               {
                   string repository = "repository";
                   var repositoryStep = new RepositoryStep(Driver);
                   repositoryStep.GoToThisStep(ProtectionType.Advanced);
                   repositoryStep.SetRepository(repoName, cifsLocation, cifsUser, cifsPassword);
                   var next = repositoryStep.GoNext();
                   Assert.AreNotEqual(next.GetStepId(), repository);

                   next.GoToFinish();
                   Assert.AreEqual(repositoryStep.GetStepId(), string.Empty);
               }, "RepositoryStepNewCIFSPositive");
        }

        [Test]
        [TestCaseSource("RepositoryStepNewLocalNegativeTestCases")]
        public void RepositoryStepNewLocalNegative(string repoName, string dataPath, string metadataPath)
        {
            UITest(() =>
               {
                   string repository = "repository";
                   var repositoryStep = new RepositoryStep(Driver);
                   repositoryStep.GoToThisStep(ProtectionType.Advanced);
                   repositoryStep.SetRepository(repoName, dataPath, metadataPath);
                   var next = repositoryStep.GoNext();
                   Assert.AreEqual(next.GetStepId(), repository);
               }, "RepositoryStepNewLocalNegative");
        }

        [Test]
        [TestCaseSource("RepositoryStepNewCIFSNegativeTestCases")]
        public void RepositoryStepNewCIFSNegative(string repoName, string cifsLocation, string cifsUser, string cifsPassword)
        {
            UITest(() =>
                {
                    string repository = "repository";
                    var repositoryStep = new RepositoryStep(Driver);
                    repositoryStep.GoToThisStep(ProtectionType.Advanced);
                    repositoryStep.SetRepository(repoName, cifsLocation, cifsUser, cifsPassword);
                    var next = repositoryStep.GoNext();
                    Assert.AreEqual(next.GetStepId(), repository);
                }, "RepositoryStepNewCIFSNegative");
        }

        [Test]
        [TestCaseSource("RepositoryConfigurationStepPositiveTestCases")]
        public void RepositoryConfigurationStepPositive(string size, BytesPerSector bytesPerSector, string bytesPerRecord, WriteCachingPolicy writeCachingPolicy)
        {
            UITest(() =>
                {
                    string configuration = "repoConfiguration";
                    var configurationStep = new ConfigurationStep(Driver);
                    configurationStep.GoToThisStep(ProtectionType.Advanced);
                    configurationStep.SetConfiguration(size, bytesPerSector, bytesPerRecord, writeCachingPolicy);
                    var next = configurationStep.GoNext();
                    Assert.AreNotEqual(next.GetStepId(), configuration);

                    next.GoToFinish();
                    Assert.AreEqual(next.GetStepId(), string.Empty);
                }, "RepositoryConfigurationStepPositive");
        }

        [Test]
        [TestCaseSource("RepositoryConfigurationStepNegativeTestCases")]
        public void RepositoryConfigurationStepNegative(string size, BytesPerSector bytesPerSector, string bytesPerRecord, WriteCachingPolicy writeCachingPolicy)
        {
            UITest(() =>
                {
                    string configuration = "repoConfiguration";
                    var configurationStep = new ConfigurationStep(Driver);
                    configurationStep.GoToThisStep(ProtectionType.Advanced);
                    configurationStep.SetConfiguration(size, bytesPerSector, bytesPerRecord, writeCachingPolicy);
                    var next = configurationStep.GoNext();
                    Assert.AreEqual(next.GetStepId(), configuration);
                }, "RepositoryConfigurationStepNegative");
        }

        [Test]
        public void EncryptionStepExistingKeyPositive()
        {
            UITest(() =>
                {
                    var encryptionStep = new EncryptionStep(Driver);
                    encryptionStep.GoToThisStep();
                    encryptionStep.ConfigureEncryption();
                    var next = encryptionStep.GoNext();
                    Assert.AreEqual(next, null);
                }, "EncryptionStepExistingKeyPositive");
        }

        [Test]
        [TestCaseSource("EncryptionStepNewKeyPositiveTestCases")]
        public void EncryptionStepNewKeyPositive(string keyName, string keyDescription, string keyPassphrase, string keyPassphraseConfirmation)
        {
            UITest(() =>
                {
                    Random random = new Random();
                    string keyNumber = (random.Next(0, 100000000)).ToString();
                    var encryptionStep = new EncryptionStep(Driver);
                    encryptionStep.GoToThisStep();
                    encryptionStep.ConfigureEncryption(keyName + keyNumber, keyDescription, keyPassphrase + keyNumber, keyPassphraseConfirmation + keyNumber);
                    var next = encryptionStep.GoNext();
                    Assert.AreEqual(next, null);
                }, "EncryptionStepNewKeyPositiveTestCases");
        }

        [Test]
        [TestCaseSource("EncryptionStepNewKeyNegativeTestCases")]
        public void EncryptionStepNewKeyNegative(string keyName, string keyDescription, string keyPassphrase, string keyPassphraseConfirmation)
        {
            UITest(() =>
                {
                    string encryption = "encryption";
                    var encryptionStep = new EncryptionStep(Driver);
                    encryptionStep.GoToThisStep();
                    encryptionStep.ConfigureEncryption(keyName, keyDescription, keyPassphrase, keyPassphraseConfirmation);
                    var next = encryptionStep.GoNext();
                    Assert.AreEqual(next.GetStepId(), encryption);
                }, "EncryptionStepNewKeyNegativeTestCases");
        }


        public static IEnumerable EncryptionStepNewKeyNegativeTestCases
        {
            get
            {
                yield return new TestCaseData(string.Empty, "description", "passphrase", "passphrase");
                yield return new TestCaseData("key", string.Empty, string.Empty, "passphrase");
                yield return new TestCaseData("key", "description", "passphrase", string.Empty);
                yield return new TestCaseData("!:::OIO(I", "description", "passphrase", "passphrase");
            }
        }

        public static IEnumerable EncryptionStepNewKeyPositiveTestCases
        {
            get
            {
                yield return new TestCaseData("key", "description", "passphrase", "passphrase");
                yield return new TestCaseData("key", string.Empty, "passphrase", "passphrase");
            }
        }

        public static IEnumerable RepositoryConfigurationStepPositiveTestCases
        {
            get
            {
                yield return new TestCaseData("10.00", BytesPerSector.Bytes1024, "8092", WriteCachingPolicy.On);
                yield return new TestCaseData("10.00", BytesPerSector.Bytes2048, "8092", WriteCachingPolicy.On);
                yield return new TestCaseData("10.00", BytesPerSector.Bytes512, "8092", WriteCachingPolicy.On);
                yield return new TestCaseData("10.00", BytesPerSector.Bytes4096, "8092", WriteCachingPolicy.On);
                yield return new TestCaseData("10.00", BytesPerSector.Bytes512, "512", WriteCachingPolicy.On);
                yield return new TestCaseData("10.00", BytesPerSector.Bytes512, "513", WriteCachingPolicy.On);
                yield return new TestCaseData("10.00", BytesPerSector.Bytes512, "131072", WriteCachingPolicy.On);
                yield return new TestCaseData("10.00", BytesPerSector.Bytes512, "131071", WriteCachingPolicy.On);
                yield return new TestCaseData("10.00", BytesPerSector.Bytes512, "8092", WriteCachingPolicy.Off);
                yield return new TestCaseData("10.00", BytesPerSector.Bytes512, "8092", WriteCachingPolicy.Sync);
                yield return new TestCaseData("100000000000000.00", BytesPerSector.Bytes2048, "8092", WriteCachingPolicy.On);
                yield return new TestCaseData("0", BytesPerSector.Bytes2048, "8092", WriteCachingPolicy.On);
                yield return new TestCaseData("10sdsd.dds00", BytesPerSector.Bytes512, "8092", WriteCachingPolicy.Sync);
            }
        }

        public static IEnumerable RepositoryConfigurationStepNegativeTestCases
        {
            get
            {
                yield return new TestCaseData("10.00", BytesPerSector.Bytes2048, string.Empty, WriteCachingPolicy.On);
                yield return new TestCaseData("10.00", BytesPerSector.Bytes2048, "dfdfdf", WriteCachingPolicy.On);
                yield return new TestCaseData("10.00", BytesPerSector.Bytes2048, "0", WriteCachingPolicy.On);
                yield return new TestCaseData("10.00", BytesPerSector.Bytes2048, "511", WriteCachingPolicy.On);
                yield return new TestCaseData("10.00", BytesPerSector.Bytes2048, "131073", WriteCachingPolicy.On);
                yield return new TestCaseData(string.Empty, BytesPerSector.Bytes2048, "8092", WriteCachingPolicy.On);
               
                

            }
        }

        public static IEnumerable RepositoryStepNewCIFSPositiveTestCases
        {
            get
            {
                yield return new TestCaseData("repoCifstest1repoCifstest1repoCifstest1repoCifstest1repoCifstest1repoCifstest1repoCifstest1repoCifstest1repoCifstest1repoCifstest1repoCifstest1repoCifstest1repoCifstest1repoCifstest1repoCifstest1repoCifstest1repoCifstest1repoCifstest1repoCifstest1repoCifstest1repoCifstest1", @"\\10.35.175.175\share\repo1\storage1", "q1", "123");
                yield return new TestCaseData("repoCifstest1", @"\\10.35.175.175\share\repo1", "q1", "123" );
                yield return new TestCaseData("repoCifstest1", @"\\10.35.175.175\share\repo1\storage1", "q1", "123");
               
            }
        }

        public static IEnumerable RepositoryStepNewCIFSNegativeTestCases
        {
            get
            {
                
                yield return new TestCaseData("repoCifstest1", @"\\10.35.175.175\share\repo1\storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1storage1", "q1", "123");
                yield return new TestCaseData("!!^&::?>&%$FFHHBBN%%^^^", @"\\10.35.175.175\share\repo1", "q1", "123");
                yield return new TestCaseData("repoCifstest1", @"\\10.35.175.175\share\repo1\storage1!.!^88/*&%", "q1", "123");
                yield return new TestCaseData("repoCifstest1", @"\\10.35.175.175\share\repo1\storage1", "someuser", "123");
                yield return new TestCaseData("repoCifstest1", @"\\10.35.175.175\share\repo1\storage1", "q1", "somepassword");
                yield return new TestCaseData(string.Empty,    @"\\10.35.175.175\share\repo1\storage1", "q1", "123");
                yield return new TestCaseData("repoCifstest1", @"\\10.35.175.175\share\repo1\storage1", string.Empty, "somepassword");
                yield return new TestCaseData("repoCifstest1", @"\\10.35.175.175\share\repo1", "q1", string.Empty);
              
            }
        }

        public static IEnumerable RepositoryStepNewLocalPositiveTestCases
        {
            get
            {
                yield return new TestCaseData("repotest1", @"C:\repotest1", @"C:\repotest1metadata");
                yield return new TestCaseData("repotest1", @"C:\repotest1", @"C:\repotest1");
                yield return new TestCaseData("repotest1", @"C:\repotest1\storage1", @"C:\repotest1");
                yield return new TestCaseData("repotest1", @"C:\repotest1\storage1", @"C:\repotest1\storage2");
                yield return new TestCaseData("repotest1", @"C:\repotest1", string.Empty);
                yield return new TestCaseData("repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repo", @"C:\repotest1", @"C:\repotest1");
                yield return new TestCaseData("repotest1", @"C:\repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11", @"C:\repotest1");
                yield return new TestCaseData("repotest1", @"C:\repotest1", @"C:\repotest11rrepotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11epotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11repotest11");  
            }
        }

        public static IEnumerable RepositoryStepNewLocalNegativeTestCases
        {
            get
            {
                yield return new TestCaseData(string.Empty, @"C:\repotest1", @"C:\repotest1metadata");
                yield return new TestCaseData("repotest1", string.Empty, @"C:\repotest1");
                yield return new TestCaseData("repotest1", @"C:\\\\repotest1\\\", @"C:\repotest1");
                yield return new TestCaseData(@"C:\sasa", @"C:\\\\repotest1\\\", @"C:\repotest1");
                yield return new TestCaseData("^!:+jj*!", @"C:\repotest1", @"C:\repotest1");
                yield return new TestCaseData("repotest1", "hdjs&^$(_", @"C:\repotest1");
                yield return new TestCaseData("repotest1", @"C:\repotest1", "hdjs&^$(_");
            }
        }

        public static IEnumerable ScheduleStepPeriodsNegativeTestCases
        {
            get 
            {
                //(ProtectionType type, bool weekdays, bool weekends, bool protectWeekdaysRest, string from, string to, 
                //                   string weekdaysPeriopd, string weekendsPeriod, string weekdaysRestPeriod, bool pause
                yield return new TestCaseData(ProtectionType.Typical, false, false, false, "13:00 AM", "15:00 PM", "75", "55", "33", false);
                yield return new TestCaseData(ProtectionType.Typical, false, false, true, "13:00 AM", "15:00 PM", "75", "55", "33", true);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, string.Empty, "15:00 PM", "75", "55", "33", false);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", string.Empty, "75", "55", "33", true);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", string.Empty, "55", "33", false);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", string.Empty, "55", "33", true);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", "75", string.Empty, "33", false);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", "75", string.Empty, "33", true);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", "75", "55", string.Empty, false);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", "0", "55", "33", true);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", "75", "0", "33", false);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", "75", "0", "33", true);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", "75", "55", "0", false);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", "4", "55", "33", true);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", "75", "55", "gfgf", false);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", "75", "gfgf", "75", true);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", "gfgfg", "55", "75", false);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "gfgfg", "75", "75", "33", true);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "gfgfg", "15:00 PM", "75", "55", "33", false);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", "75", "55", "100500", true);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", "75", "100500", "33", false);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", "100500", "55", "33", true);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", "75", "55", "4", false);
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", "75", "4", "33", true);


                yield return new TestCaseData(ProtectionType.Advanced, false, false, false, "13:00 AM", "15:00 PM", "75", "55", "33", false);
                yield return new TestCaseData(ProtectionType.Advanced, false, false, true, "13:00 AM", "15:00 PM", "75", "55", "33", true);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, string.Empty, "15:00 PM", "75", "55", "33", false);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", string.Empty, "75", "55", "33", true);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", string.Empty, "55", "33", false);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", string.Empty, "55", "33", true);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", "75", string.Empty, "33", false);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", "75", string.Empty, "33", true);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", "75", "55", string.Empty, false);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", "0", "55", "33", true);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", "75", "0", "33", false);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", "75", "0", "33", true);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", "75", "55", "0", false);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", "4", "55", "33", true);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", "75", "55", "gfgf", false);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", "75", "gfgf", "75", true);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", "gfgfg", "55", "75", false);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "gfgfg", "75", "75", "33", true);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "gfgfg", "15:00 PM", "75", "55", "33", false);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", "75", "55", "100500", true);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", "75", "100500", "33", false);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", "100500", "55", "33", true);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", "75", "55", "4", false);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", "75", "4", "33", true);

                //yield return new TestCaseData(false, false, false, "13:00 AM", "15:00 PM", "75", "55", "33", true);
                //yield return new TestCaseData(false, false, true, "13:00 AM", "15:00 PM", "75", "55", "33", true);
                //yield return new TestCaseData(true, true, true, string.Empty, "15:00 PM", "75", "55", "33", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", string.Empty, "75", "55", "33", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", string.Empty, "55", "33", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", string.Empty, "55", "33", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", "75", string.Empty, "33", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", "75", string.Empty, "33", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", "75", "55", string.Empty, true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", "0", "55", "33", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", "75", "0", "33", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", "75", "0", "33", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", "75", "55", "0", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", "4", "55", "33", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", "75", "55", "gfgf", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", "75", "gfgf", "75", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", "gfgfg", "55", "75", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "gfgfg", "75", "75", "33", true);
                //yield return new TestCaseData(true, true, true, "gfgfg", "15:00 PM", "75", "55", "33", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", "75", "55", "100500", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", "75", "100500", "33", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", "100500", "55", "33", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", "75", "55", "4", true);
                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", "75", "4", "33", true);


            }
        }

        public static IEnumerable ScheduleStepDailyPositiveTestCases
        {
            get
            {
                yield return new TestCaseData(ProtectionType.Typical, "1:00 AM", false);
                yield return new TestCaseData(ProtectionType.Typical, "18:33 PM", false);
                yield return new TestCaseData(ProtectionType.Typical, "1:00 AM", true);
                yield return new TestCaseData(ProtectionType.Typical, "18:33 PM", true);

                yield return new TestCaseData(ProtectionType.Advanced, "1:00 AM", false);
                yield return new TestCaseData(ProtectionType.Advanced, "18:33 PM", false);
                yield return new TestCaseData(ProtectionType.Advanced, "1:00 AM", true);
                yield return new TestCaseData(ProtectionType.Advanced, "18:33 PM", true);
            }
        }

        public static IEnumerable ScheduleStepDailyNegativeTestCases
        {
            get
            {
                yield return new TestCaseData(ProtectionType.Typical, string.Empty, false);
                yield return new TestCaseData(ProtectionType.Typical, "188:33 PM", false);
                yield return new TestCaseData(ProtectionType.Typical, string.Empty, true);
                yield return new TestCaseData(ProtectionType.Typical, "18:33:44 PM", true);
                yield return new TestCaseData(ProtectionType.Typical, "8:33 PP", true);
                yield return new TestCaseData(ProtectionType.Typical, "fde3322", true);

                yield return new TestCaseData(ProtectionType.Advanced, string.Empty, false);
                yield return new TestCaseData(ProtectionType.Advanced, "188:33 PM", false);
                yield return new TestCaseData(ProtectionType.Advanced, string.Empty, true);
                yield return new TestCaseData(ProtectionType.Advanced, "18:33:44 PM", true);
                yield return new TestCaseData(ProtectionType.Advanced, "8:33 PP", true);
                yield return new TestCaseData(ProtectionType.Advanced, "fde3322", true);
            }
        }

        public static IEnumerable ScheduleStepPeriodsPositiveTestCases
        {
            get
            {
                yield return new TestCaseData(ProtectionType.Typical, true, true, true, "13:00 AM", "15:00 PM", "75", "55", "33", false);
                yield return new TestCaseData(ProtectionType.Typical, false, true, true, "13:00 AM", "15:00 PM", "75", "55", "33", true);
                yield return new TestCaseData(ProtectionType.Typical, true, false, true, "13:00 AM", "15:00 PM", "75", "55", "33", false);
                yield return new TestCaseData(ProtectionType.Typical, true, true, false, "13:00 AM", "15:00 PM", "75", "55", "33", true);
                yield return new TestCaseData(ProtectionType.Typical, true, false, false, "13:00 AM", "15:00 PM", "75", "55", "33", false);
                yield return new TestCaseData(ProtectionType.Typical, false, true, false, "13:00 AM", "15:00 PM", "75", "55", "33", true);

                yield return new TestCaseData(ProtectionType.Advanced, true, true, true, "13:00 AM", "15:00 PM", "75", "55", "33", false);
                yield return new TestCaseData(ProtectionType.Advanced, false, true, true, "13:00 AM", "15:00 PM", "75", "55", "33", true);
                yield return new TestCaseData(ProtectionType.Advanced, true, false, true, "13:00 AM", "15:00 PM", "75", "55", "33", false);
                yield return new TestCaseData(ProtectionType.Advanced, true, true, false, "13:00 AM", "15:00 PM", "75", "55", "33", true);
                yield return new TestCaseData(ProtectionType.Advanced, true, false, false, "13:00 AM", "15:00 PM", "75", "55", "33", false);
                yield return new TestCaseData(ProtectionType.Advanced, false, true, false, "13:00 AM", "15:00 PM", "75", "55", "33", true);

                //yield return new TestCaseData(true, true, true, "13:00 AM", "15:00 PM", "75", "55", "33", true);
                //yield return new TestCaseData(false, true, true, "13:00 AM", "15:00 PM", "75", "55", "33", true);
                //yield return new TestCaseData(true, false, true, "13:00 AM", "15:00 PM", "75", "55", "33", true);
                //yield return new TestCaseData(true, true, false, "13:00 AM", "15:00 PM", "75", "55", "33", true);
                //yield return new TestCaseData(true, false, false, "13:00 AM", "15:00 PM", "75", "55", "33", true);
                //yield return new TestCaseData(false, true, false, "13:00 AM", "15:00 PM", "75", "55", "33", true);
            }
        }

        public static IEnumerable ConnectionStepNegativeTestCases
        {
            get
            {
                yield return new TestCaseData(ProtectionType.Advanced, string.Empty, string.Empty, string.Empty, string.Empty);
                yield return new TestCaseData(ProtectionType.Advanced, "=/*+", "546846", "a", "b");
                yield return new TestCaseData(ProtectionType.Advanced, "10.35.176.255", Agent1Port, Agent1UserName, Agent1Password);
                yield return new TestCaseData(ProtectionType.Advanced, Properties.Settings.Default.agentIP.ToString(), "65538", Agent1UserName, Agent1Password);
                yield return new TestCaseData(ProtectionType.Advanced, Properties.Settings.Default.agentIP.ToString(), string.Empty, Agent1UserName, Agent1Password);
                yield return new TestCaseData(ProtectionType.Advanced, string.Empty, Agent1Port, Agent1UserName, Agent1Password);
                yield return new TestCaseData(ProtectionType.Advanced, Properties.Settings.Default.agentIP.ToString(), Agent1Port, string.Empty, Agent1Password);
                yield return new TestCaseData(ProtectionType.Advanced, Properties.Settings.Default.agentIP.ToString(), Agent1Port, Agent1UserName, string.Empty);
                yield return new TestCaseData(ProtectionType.Advanced, Properties.Settings.Default.agentIP.ToString(), "565ghsagh", Agent1UserName, Agent1Password);

                yield return new TestCaseData(ProtectionType.Typical, string.Empty, string.Empty, string.Empty, string.Empty);
                yield return new TestCaseData(ProtectionType.Typical, "=/*+", "546846", "a", "b");
                yield return new TestCaseData(ProtectionType.Typical, "10.35.176.255", Agent1Port, Agent1UserName, Agent1Password);
                yield return new TestCaseData(ProtectionType.Typical, Properties.Settings.Default.agentIP.ToString(), "65538", Agent1UserName, Agent1Password);
                yield return new TestCaseData(ProtectionType.Typical, Properties.Settings.Default.agentIP.ToString(), string.Empty, Agent1UserName, Agent1Password);
                yield return new TestCaseData(ProtectionType.Typical, string.Empty, Agent1Port, Agent1UserName, Agent1Password);
                yield return new TestCaseData(ProtectionType.Typical, Properties.Settings.Default.agentIP.ToString(), Agent1Port, string.Empty, Agent1Password);
                yield return new TestCaseData(ProtectionType.Typical, Properties.Settings.Default.agentIP.ToString(), Agent1Port, Agent1UserName, string.Empty);
                yield return new TestCaseData(ProtectionType.Typical, Properties.Settings.Default.agentIP.ToString(), "565ghsagh", Agent1UserName, Agent1Password);
        
            }
        }

        public static IEnumerable ConnectionStepPositiveTestCases
        {
            get
            {
                yield return new TestCaseData(ProtectionType.Typical, Properties.Settings.Default.agentIP.ToString(), Agent1Port, Agent1UserName, Agent1Password);
                //yield return new TestCaseData(ProtectionType.Typical, Agent1IP, "1", Agent1UserName, Agent1Password);
                //yield return new TestCaseData(ProtectionType.Typical, Agent1IP, "65535", Agent1UserName, Agent1Password);
                yield return new TestCaseData(ProtectionType.Typical, "10.35.176.171", "65", Agent1UserName, Agent1Password);
            }
        }

        public static IEnumerable ProtectionStepPositiveTestCases
        {
            get
            {
                yield return new TestCaseData(ProtectionType.Typical, Properties.Settings.Default.agentIP.ToString(), ProtectionSchedule.Default);
                yield return new TestCaseData(ProtectionType.Advanced, Properties.Settings.Default.agentIP.ToString(), ProtectionSchedule.Default);
                yield return new TestCaseData(ProtectionType.Typical, Properties.Settings.Default.agentIP.ToString(), ProtectionSchedule.Custom);
                yield return new TestCaseData(ProtectionType.Advanced, Properties.Settings.Default.agentIP.ToString(), ProtectionSchedule.Custom);
                yield return new TestCaseData(ProtectionType.Typical, "protectionMachine1", ProtectionSchedule.Custom);
                yield return new TestCaseData(ProtectionType.Advanced, "protectionMachine1", ProtectionSchedule.Custom);
                yield return new TestCaseData(ProtectionType.Typical, "Agent1DisplayNameAgent1DisplayNameAgent1DisplayNameAgent1DisplayNameAgent1DisplayNameAgent1DisplayNameAgent1DisplayName", ProtectionSchedule.Default);
            }
        }

        public static IEnumerable ProtectionStepNegativeTestCases
        {
            get
            {
                yield return new TestCaseData(ProtectionType.Typical, string.Empty, ProtectionSchedule.Default);
                yield return new TestCaseData(ProtectionType.Typical, " !@dsasa&&&:::", ProtectionSchedule.Default);
            }
        }

        public static IEnumerable WelcomeStepPositiveTestCases
        {
            get
            {
                yield return new TestCaseData(ProtectionType.Typical);
                yield return new TestCaseData(ProtectionType.Advanced);
            }
        }

        public static IEnumerable VolumesStepPositiveTestCases
        {
            get
            {
                yield return new TestCaseData(Volumes.All, ProtectionType.Typical);
                yield return new TestCaseData(Volumes.Fisrt, ProtectionType.Typical);
                yield return new TestCaseData(Volumes.None, ProtectionType.Typical);

                yield return new TestCaseData(Volumes.All, ProtectionType.Advanced);
                yield return new TestCaseData(Volumes.Fisrt, ProtectionType.Advanced);
                yield return new TestCaseData(Volumes.None, ProtectionType.Advanced);
            }
        }

    }
}
