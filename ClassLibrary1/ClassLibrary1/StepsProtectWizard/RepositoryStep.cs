using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtectWizardTests.StepsProtectWizard;

namespace ProtectWizardTests.StepsProtectWizard
{
    class RepositoryStep : StepBase
    {
        public RepositoryStep(IWebDriver driver)
            : base(driver)
        {
        }

        public override StepBase GetNext()
        {
            if (GetStepId() == "encryption")
            {
                return new EncryptionStep(driver);
            }
            else if (GetStepId() == "repoConfiguration")
            {
                return new ConfigurationStep(driver);
            }
            return this;
        }
       
        public override StepBase GoToThisStep(ProtectionType type)
        {
            ScheduleStep schedule = new ScheduleStep(driver);
            schedule.GoToThisStep(type);
            schedule.SetValidData();
            var repository = (RepositoryStep)schedule.GoNext();
            return repository;
        }

        /// <summary>
        /// Sets default valid data
        /// path - C:\repoTest; metadatapath - C:\repoTestMetadata
        /// </summary>
        public override void SetValidData()
        {
            SetRepository("newTestRepository", RepositoryLocationType.Local, @"C:\repoTest", string.Empty, string.Empty, @"C:\repoTestMetadata");
        }

        private void SetRepository(string repositoryName, RepositoryLocationType locationtype, string location, string user, string password, string locationMetadata )
        {
            try
            {
                driver.FindElement(By.Id("createRepository")).Click();
            }
            catch 
            {
            }
            sendText(driver.FindElement(By.Id("newRepoName")), repositoryName);
            sendText(driver.FindElement(By.Id("newRepoLocation")), location);
            if (locationtype == RepositoryLocationType.CIFS)
            {
                try
                {
                    sendText(driver.FindElement(By.Id("newRepoUserName")), user);
                    sendText(driver.FindElement(By.Id("newRepoPassword")), password);
                }
                catch
                { }
            }
            if (locationtype == RepositoryLocationType.Local)
            {
                sendText(driver.FindElement(By.Id("newRepoMetadataLocation")), locationMetadata);
            }
        }

        /// <summary>
        /// Creates new CIFS repository.
        /// </summary>
        public void SetRepository(string repositoryName, string location, string user, string password)
        {
            SetRepository(repositoryName, RepositoryLocationType.CIFS, location, user, password, string.Empty);
        }

        /// <summary>
        /// Creates new local repository
        /// </summary>
        public void SetRepository(string repositoryName, string location, string locationMetadata)
        {
            SetRepository(repositoryName, RepositoryLocationType.Local, location, string.Empty, string.Empty, locationMetadata);
        }

        /// <summary>
        /// Selects first existing repository
        /// </summary>
        public void SetRepository()
        {
            driver.FindElement(By.Id("dropdown-visual-input-existedRepositories")).Click();
            IList<IWebElement> listrepositories = driver.FindElement(By.Id("existedRepositories")).FindElements(By.TagName("option"));
            listrepositories.Last().Click(); 
        }
    }

    public enum RepositoryLocationType
    {
        Local,
        CIFS,
        None
    }
}
