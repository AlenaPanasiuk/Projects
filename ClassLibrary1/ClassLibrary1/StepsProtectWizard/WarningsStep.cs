using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtectWizardTests.StepsProtectWizard;

namespace ProtectWizardTests.StepsProtectWizard
{
    class WarningsStep : StepBase
    {
        public WarningsStep(IWebDriver driver)
            : base(driver)
        {
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

        public override StepBase GoToThisStep(ProtectionType type)
        {
            throw new NotImplementedException();
        }
    }
}
