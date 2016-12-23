using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtectWizardTests.StepsProtectWizard;

namespace ProtectWizardTests.StepsProtectWizard
{
    public class ConnectionStep : StepBase
    {
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
            SetConnectionValues(Properties.Settings.Default.agentIP.ToString(), TestBase.Agent1Port, TestBase.Agent1UserName, TestBase.Agent1Password);
        }


        public override StepBase GoToThisStep(ProtectionType type)
        {
            WelcomeStep welcome = new WelcomeStep(driver);
            welcome.SetProtectionType(type);
            var connection = (ConnectionStep)welcome.GoNext();
            return connection;
           
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
            else if (GetStepId() == "warnings")
            {
                return new WarningsStep(driver);
            }
            else if (GetStepId() == "pushInstall")
            {
                return new InstallStep(driver);
            }

            return this;
        }
    }
}
