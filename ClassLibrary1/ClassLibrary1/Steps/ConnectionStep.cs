using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtectWizardTests.Steps;

namespace ProtectWizardTests.Steps
{
    public class ConnectionStep : StepBase
    {
        private const string Agent1IP = "10.35.176.166";
        private const string Agent1Port = "8006";
        private const string Agent1UserName = "administrator";
        private const string Agent1Password = "123asdQ";

        public ConnectionStep(IWebDriver driver)
            : base(driver)
        {
        }

        public void SetConnectionValues(string ip, string port, string username, string password)
        {
            driver.FindElement(By.Id("hostName")).SendKeys(ip);
            driver.FindElement(By.Id("port")).Clear();
            driver.FindElement(By.Id("port")).SendKeys(port);
            driver.FindElement(By.Id("userName")).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);
        }

        public override void SetValidData()
        {
            SetConnectionValues(Agent1IP, Agent1Port, Agent1UserName, Agent1Password);
        }


        public ConnectionStep GoToConnectionStepTypical()
        {
            WelcomeStep welcome = new WelcomeStep(driver);
            welcome.SetProtectionType(ProtectionType.Typical);
            welcome.GoNext();
            return new ConnectionStep(driver);
           
        }

        public ConnectionStep GoToConnectionStepAdvanced()
        {
            WelcomeStep welcome = new WelcomeStep(driver);
            welcome.SetProtectionType(ProtectionType.Advanced);
            welcome.GoNext();
            return new ConnectionStep(driver);
        }

        public override StepBase GetNext()
        {
            if (GetStepId() == "upgrade")
            {
                return new UpgradeStep(driver);
            }
            else if (GetStepId() == "protection")
            { 
                return new ProtectionStep(driver); 
            }

            return this;
        }
    }
}
