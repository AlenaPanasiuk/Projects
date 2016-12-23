using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace ProtectWizardTests.StepsProtectWizard
{
    class ConfigurationStep : StepBase
    {
        public ConfigurationStep(IWebDriver driver)
            : base(driver)
        {
        }

        public override StepBase GetNext()
        {
            return new EncryptionStep(driver);
        }

        public override StepBase GoToThisStep(ProtectionType type)
        {
            RepositoryStep repository = new RepositoryStep(driver);
            repository.GoToThisStep(type);
            repository.SetValidData();
            var next = repository.GoNext();
            return (ConfigurationStep)next;
        }

        /// <summary>
        /// Sets the repository size as half of proposed size
        /// </summary>
        public override void SetValidData()
        {
            double sizeValid = Convert.ToDouble(driver.FindElement(By.Id("repoConfigSize")).GetAttribute("value").ToString()) / 2;
            SetConfiguration(sizeValid.ToString());
        }

        /// <summary>
        /// Configurations without advenced settings.
        /// </summary>
        public void SetConfiguration(string size)
        {
            SetConfiguration(size, BytesPerSector.Bytes512, "8192", WriteCachingPolicy.On);
        }

        /// <summary>
        /// Configurations with advanced settings
        /// </summary>
        public void SetConfiguration(string size, BytesPerSector bytesPerSector, string bytesPerRecord, WriteCachingPolicy writeCachingPolicy)
        {
            driver.FindElement(By.Id("repoConfigSize")).Clear();
            driver.FindElement(By.Id("repoConfigSize")).SendKeys(size);
            driver.FindElement(By.Id("toggleRepoAdvancedConfiguration")).Click();
            driver.FindElement(By.Id("dropdown-wrapper-bytesPerSector")).Click();
            IList<IWebElement> listOptionsBytesPerSector = driver.FindElement(By.Id("dropdown-menu-bytesPerSector")).FindElements(By.TagName("li"));
            listOptionsBytesPerSector[(int)bytesPerSector].Click();

            driver.FindElement(By.Id("bytesPerRecord")).Clear();
            driver.FindElement(By.Id("bytesPerRecord")).SendKeys(bytesPerRecord.ToString());

            driver.FindElement(By.Id("dropdown-wrapper-cachingPolicy")).Click();
            IList<IWebElement> listOptionsWriteCachingPolicy = driver.FindElement(By.Id("dropdown-menu-cachingPolicy")).FindElements(By.TagName("li"));
            listOptionsWriteCachingPolicy[(int)writeCachingPolicy].Click();
        }
    }

    public enum BytesPerSector
    {
        Bytes512,
        Bytes1024,
        Bytes2048,
        Bytes4096
    }

    public enum WriteCachingPolicy
    {
        On,
        Off,
        Sync
    }


}
