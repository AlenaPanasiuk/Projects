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
using ProtectWizardTests.StepsProtectWizard;
using Replay.Core.Contracts.Agents;
using Replay.Logging;
using Replay.Core.Client;
using Replay.Core.Contracts.Repositories;

namespace ProtectWizardTests.Helpers
{
    public class APIHelpers
    { public string host;
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

        public void DeleteRepositories()
        {
            try
            {
                // Create the CoreClient
                var coreClient = GetDefaultCoreClient(host, port);

                // Retrieves first available DVM repository
                var listRepositories = coreClient.RepositoryManagement.GetDvmRepositories();

                foreach ( var repository in listRepositories)
                {
                   // var repository = coreClient.RepositoryManagement.GetDvmRepositories().FirstOrDefault();
                    if (repository == null)
                    {
                        Console.WriteLine("WARNING: Can't find a Dvm repository. Ensure that at least one Dvm repository exists on the core.");
                        return;
                    }
                    Console.WriteLine("Starting a job to delete a repository {0}:", repository.RepositoryName);

                    // Starts delete DVM repository job and clean all data therein
                    var deleteRepositoryJob = coreClient.RepositoryManagement.DeleteRepository(repository.Id.ToString());

                    Console.WriteLine("Job to delete a repository {0} has started. JobId:{1}{2}", repository.RepositoryName, Environment.NewLine, deleteRepositoryJob);
                    // coreClient.
                }
                
             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                // NOTE: Logging must be stopped before your app exits. 
                // Otherwise your app will hang.
                LoggerFactory.Instance.EndSession(false);
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
