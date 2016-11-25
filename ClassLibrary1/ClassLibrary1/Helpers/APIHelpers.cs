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
using ProtectWizardTests.Steps;
using Replay.Core.Contracts.Agents;
using Replay.Logging;
using Replay.Core.Client;

namespace ProtectWizardTests.Helpers
{
    public class APIHelpers
    {
        public string host;
        public int port;
        public APIHelpers(string CoreHost, int CorePort)
        {
            host = CoreHost;
            port = CorePort;
        }

        public void TestResults ()
        {
            try
            {
                Console.WriteLine("RESULTS");
                var coreClient = GetDefaultCoreClient(host, port);
                var protectedAgents = coreClient.AgentsManagement.GetProtectedAgents();
                Console.WriteLine("Protected agents:");
                DisplayAgents(protectedAgents);
            }
            finally
            {
                LoggerFactory.Instance.EndSession(false);
            }

        }


        public void DeleteAgents ()
        {
            try
            {
                var coreClient = GetDefaultCoreClient(host, port);
                var protectedAgents = coreClient.AgentsManagement.GetProtectedAgents();
                var agent = coreClient.AgentsManagement.GetProtectedAgents().FirstOrDefault();
                if (agent != null)
                {
                    coreClient.AgentsManagement.DeleteAgent(agent.Id.ToString(), new DeleteAgentRequest { DeleteRecoveryPoints = true });
                    Console.WriteLine("Agent is deleted");
                }
            }
            finally
            {
                LoggerFactory.Instance.EndSession(false);
            }

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
