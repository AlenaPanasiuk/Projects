using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtectWizardTests.StepsProtectWizard
{
    public class UpgradeStep : StepBase
    {
        public UpgradeStep(IWebDriver driver)
            : base(driver)
        {
        }

        public override StepBase GetNext()
        {
            return new ProtectionStep(driver);
        }

        public override StepBase GoToThisStep(ProtectionType type)
        {
            throw new NotImplementedException();
        }
    }
}
