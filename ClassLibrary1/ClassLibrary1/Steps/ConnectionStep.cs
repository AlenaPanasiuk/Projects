using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public override void SetCustomeValidData()
        {
            SetConnectionValues(Agent1IP, Agent1Port, Agent1UserName, Agent1Password);
        }

        public override StepBase GetNext()
        {
            if (GetName() == "Upgrade Agent")
            {
                return new UpgradeStep(driver);
            }
            else if (GetName() == "Protection")
            { 
                return new ProtectionStep(driver); 
            }

            return this;
        }
    }
}
