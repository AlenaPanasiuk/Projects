using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtectWizardTests.StepsProtectWizard
{
    class EncryptionStep : StepBase
    {
        public EncryptionStep(IWebDriver driver)
            : base(driver)
        {
        }

        public override StepBase GetNext()
        {
            StepBase step = null;
            try
            {
                if (driver.FindElement(By.Id("wizardContentContainer")).Displayed)
                {
                    step = new EncryptionStep(driver);
                }
            }
            catch  
            {
            }
            return step;
        }

        public override void SetValidData()
        {
            ConfigureEncryption();
        }

        /// <summary>
        /// Sets the firts available existing encryption key if it exist, or creates key with default values
        /// </summary>
        public void ConfigureEncryption()
        {

            try
            {
                driver.FindElement(By.Id("enabledEncryption")).Click();
            }
            catch { }

            if (!driver.FindElement(By.Id("existEncryptionKey")).Enabled)
            {
                ConfigureEncryption("keyName", "keyDescription", "passphrase", "passphrase");
            }
            else
            {
                driver.FindElement(By.Id("existEncryptionKey")).Click();
                driver.FindElement(By.Id("dropdown-wrapper-existingEncrKeys")).Click();
                IList<IWebElement> listKeys = driver.FindElement(By.Id("dropdown-menu-existingEncrKeys")).FindElements(By.TagName("li"));
                listKeys[1].Click();
            }
        }

        /// <summary>
        /// Creates new encryption key
        /// </summary>
        public void ConfigureEncryption(string keyName, string keyDescription, string keyPassphrase, string keyPasspharseConfirmation)
        {
            try
            {
                driver.FindElement(By.Id("enabledEncryption")).Click();
            }
            catch { }

            driver.FindElement(By.Id("createEncryptionKey")).Click();
            driver.FindElement(By.Id("newKeyName")).SendKeys(keyName);
            driver.FindElement(By.Id("newKeyDescription")).SendKeys(keyDescription);
            driver.FindElement(By.Id("newKeyPassphrase")).SendKeys(keyPassphrase);
            driver.FindElement(By.Id("newKeyPassConfirm")).SendKeys(keyPasspharseConfirmation);
        }

        public override StepBase GoToThisStep(ProtectionType type)
        {
            var repository = new RepositoryStep(driver);
            repository.GoToThisStep(type);
            repository.SetValidData();
            var next = repository.GoNext();
            if (next.GetStepId() == "repoConfiguration")
            {
                next = next.GoNext();
            }
            return (EncryptionStep)next;
        }

        public StepBase GoToThisStep()
        {
            GoToThisStep(ProtectionType.Advanced);
            return this;
        }

    }
}
