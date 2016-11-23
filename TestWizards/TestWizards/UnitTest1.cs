using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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

namespace UnitTestProject4
{
    [TestClass]
    public class UnitTest1
    {
        IWebDriver driver;
        private const string Agent1IP = "10.35.176.166";
        private const string Agent1UserName = "administrator";
        private const  string Agent1Password = "123asdQ";
        private const string CoreHost = "10.35.176.167";
        private const string CoreUserName = "administrator";
        private const string CorePassword = "123asdQ";
        private const int CorePort = 8006;

        string url = String.Format("https://{0}:{1}@{2}:{3}/apprecovery/admin", CoreUserName, CorePassword, CoreHost, CorePort);

        [TestInitialize]
        public void TestSetup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
        }

        [TestCleanup]
        public void Cleanup()
        {
            try
            {

                Console.WriteLine("RESULTS");
                var coreClient = GetDefaultCoreClient(CoreHost, CorePort);
                var protectedAgents = coreClient.AgentsManagement.GetProtectedAgents();
                Console.WriteLine("Protected agents:");
                DisplayAgents(protectedAgents);
                var agentId = coreClient.AgentsManagement.GetProtectedAgents().FirstOrDefault().Id.ToString();
                coreClient.AgentsManagement.DeleteAgent(agentId, new DeleteAgentRequest { DeleteRecoveryPoints = true });
                Console.WriteLine("Agent is deleted");
            }
            finally
            {
                LoggerFactory.Instance.EndSession(false);
            }

            driver.Quit();
        }

        [TestMethod]
        public void TestProtectMachineWizardTypicalProtectDefault()
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 90));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='protectEntity']")));
            //Open Protect Machine Wizard
            driver.FindElement(By.XPath(".//*[@id='protectEntity']")).Click();
            driver.FindElement(By.XPath(".//*[@id='protectMachine']/div[2]/ul/li[1]/a")).Click();

            //Protect Machine Wizard -> Go to Connection page
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnWizardDefault")));
            driver.FindElement(By.Id("btnWizardDefault")).Click();

            driver.FindElement(By.Id("hostName")).SendKeys(Agent1IP);
            driver.FindElement (By.Id("userName")).SendKeys(Agent1UserName);
            driver.FindElement (By.Id("password")).SendKeys(Agent1Password);

            //Go to Protection page
            driver.FindElement(By.Id("btnWizardDefault")).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnWizardDefault")));
            
            //Protecting
            driver.FindElement(By.Id("btnWizardDefault")).Submit();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//*[@id='apgProtected']/div[2]/div/a/div[3]"))); 

        }


         [TestMethod]
        public void TestProtectMachineWizardAdvancedProtectDefault()
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 90));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='protectEntity']")));
            //Open Protect Machine Wizard
            driver.FindElement(By.XPath(".//*[@id='protectEntity']")).Click();
            driver.FindElement(By.XPath(".//*[@id='protectMachine']/div[2]/ul/li[1]/a")).Click();
            driver.FindElement(By.Id("advancedWizard"));

            //Protect Machine Wizard -> Go to Connection page
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnWizardDefault")));
            driver.FindElement(By.Id("btnWizardDefault")).Click();
            driver.FindElement(By.Id("hostName")).SendKeys(Agent1IP);
            driver.FindElement (By.Id("userName")).SendKeys(Agent1UserName);
            driver.FindElement (By.Id("password")).SendKeys(Agent1Password);



            //Go to Protection page
            driver.FindElement(By.Id("btnWizardDefault")).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnWizardDefault")));
            


             //Go to Repository page

            driver.FindElement(By.Id("createRepository")).Click();
            driver.FindElement(By.Id("newRepoName")).SendKeys("newtestrepo");
            driver.FindElement(By.Id("newRepoLocation")).SendKeys("C:\newtestrepo");
             driver.FindElement(By.Id("btnWizardDefault")).Click();

             //Go to repository configuration step
             driver.FindElement(By.Id("repoConfigSize")).SendKeys("10");


            //Protecting
            driver.FindElement(By.Id("btnWizardDefault")).Submit();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//*[@id='apgProtected']/div[2]/div/a/div[3]"))); 

        }

       


        private static void DisplayAgents(AgentInfoCollection agentInfoCollection)
        {
            if (agentInfoCollection.Any())
            {
                foreach (var agent in agentInfoCollection)
                {
                    Console.WriteLine(agent.DisplayName);
                }
            }
            else
            {
                Console.WriteLine("There no agents");
            }
        }

        private static ICoreClient GetDefaultCoreClient(string host, int port)
        {
            var coreClientFactory = new Replay.Core.Client.CoreClientFactory();

            var coreClient = coreClientFactory.Create(CreateApiUri(host, port));
            coreClient.UseDefaultCredentials();

            return coreClient;
        }

        private static Uri CreateApiUri(string host, int port)
        {
            var apiUri = string.Format(
                                       CultureInfo.InvariantCulture,
                                       "https://{0}:{1}/{2}/api/core/",
                                       host,
                                       port,
                                       WCFClientBase.WcfConstants.RootOfServiceHostAddress);

            return new Uri(apiUri);
        }

    }
}
