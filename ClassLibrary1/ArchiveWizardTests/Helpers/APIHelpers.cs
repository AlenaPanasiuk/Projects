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
using ArchiveWizardTests.StepsArchiveWizard;
using Replay.Core.Client;
using Replay.Core.Contracts.Agents;
using Replay.Logging;
using Replay.Core.Contracts.Repositories;
using Replay.Core.Contracts.Cloud;
using Replay.Common.Contracts.Cloud.Credentials;
using Replay.Common.Contracts.Cloud;
using WCFClientBase;
using System.Collections.ObjectModel;
using Replay.Common.Contracts.Metadata;
using Replay.Core.Contracts.Transfer.TransferSchedule;
using Replay.Agent;
using Microsoft.Win32;

namespace ArchiveWizardTests.Helpers
{
    public class APIHelpers
    {
        private string host;
        private int port;

        private string AzureAccountName = Properties.Settings.Default.CloudAzureAccountDisplayName;
        private string AzureKey = Properties.Settings.Default.CloudAzureAccessKey;
        private string AzureStorageName = Properties.Settings.Default.CloudAzureStorageAccountName;

        private string OpenStackName = Properties.Settings.Default.CloudOpenStackAccountDisplayName;
        private string OpenStackUserName = Properties.Settings.Default.CloudOpenStackUserName;
        private string OpenStackAPIKey = Properties.Settings.Default.CloudOpenStackAPIKey;

        private ulong RepositorySizeGB = Properties.Settings.Default.RepositorySizeGB;

        private string AgentHostName = Properties.Settings.Default.AgentHostName;
        private int AgentApiPort = Properties.Settings.Default.AgentApiPort;
        private string AgentUserName = Properties.Settings.Default.AgentUserName;
        private string AgentPassword = Properties.Settings.Default.AgentPassword;
        private string AgentDisplayName = Properties.Settings.Default.AgentDisplayName;
        private string AgentVolumeForProtect = Properties.Settings.Default.AgentSmallVolumeToProtect;





        // The name of DVM repository that will be added
        private string RepositoryName = Properties.Settings.Default.RepositoryName;

        // The path(format Disk:\Dir) to the place where the created data of the storage location is stored
        private string DataPath = Properties.Settings.Default.RepositoryPath;

        // The path(format Disk:\Dir) to the place where the created metadata of the storage location is stored
        private string MetadataPath = Properties.Settings.Default.RepositoryMetadataPath;

        private string RackspaceName = Properties.Settings.Default.CloudRackspaceAccountDisplayName;


       public Guid amazonAccountId;
       public Guid azureAccountId;
       public Guid openStackAccountId;
       public Guid rackspaceAccountId;

        public APIHelpers(string CoreHost, int CorePort)
        {
            host = CoreHost;
            port = CorePort;
        }

        public void TestResults()
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

        public void ProtectAgent()
        {
            try
            {
                var coreClient = GetDefaultCoreClient(host, port);
                var credentials = new AgentCredentials
                {
                    UserName = AgentUserName,
                    Password = AgentPassword
                };

                var agentDescriptor = new AgentDescriptor
                {
                    AgentType = AgentType.Protected,
                    Credentials = credentials,
                    DisplayName = AgentDisplayName,
                    MetadataCredentials = new MetadataCredentials
                    {
                        DefaultCredentials = new BaseCredentials
                        {
                            Password = AgentPassword,
                            UserName = AgentUserName
                        }
                    },
                    HostUri = GetAgentUri(AgentHostName, AgentApiPort)
                };

                // Get volumes that are available for protection
                var availableVolumes = coreClient.AgentsManagement.GetAgentVolumesAvailableForProtection(agentDescriptor);

                // Create one protection group
                var protectionGroupConfig = new AgentProtectionGroupConfiguration
                {
                    TransferScheduleConfiguration = new TransferScheduleConfiguration(),
                   // VolumeNames = new AgentVolume(@"E:\")
                    VolumeNames = new AgentVolumeCollection(new AgentVolume(@"E:\"))
                };

                // Get first available repository
                var repoInfo = coreClient.RepositoryManagement.GetRepositories().FirstOrDefault();

                if (repoInfo == null)
                {
                    return;
                }

                var agentProtectionConfiguration = new AgentProtectionConfiguration
                {
                    ProtectionGroups = new AgentProtectionGroupConfigurationCollection { protectionGroupConfig },
                    StorageConfiguration = new AgentProtectionStorageConfiguration { RepositoryId = repoInfo.Id }
                };

                var agentRequest = new AddAgentRequest
                {
                    AgentProtectionConfiguration = agentProtectionConfiguration,
                    Descriptor = agentDescriptor
                };

                // Add the Agent under protection and get its id 
                var agentId = coreClient.AgentsManagement.AddAgent(agentRequest).Id.ToString();

                // Find the Agent with a specified id 
                var agent = coreClient.AgentsManagement.FindAgentById(agentId);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            finally
            {
                LoggerFactory.Instance.EndSession(false);
            }
        }

        public void DeleteAgents()
        {
            try
            {
                var coreClient = GetDefaultCoreClient(host, port);
                var protectedAgents = coreClient.AgentsManagement.GetProtectedAgents();

                foreach (var protectedAgent in protectedAgents)
                {
                   // var agent = coreClient.AgentsManagement.GetProtectedAgents().FirstOrDefault();
                    if (protectedAgent != null)
                    {
                        coreClient.AgentsManagement.DeleteAgent(protectedAgent.Id.ToString(), new DeleteAgentRequest { DeleteRecoveryPoints = true });
                        Console.WriteLine("Agent is deleted");
                    }
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

                foreach (var repository in listRepositories)
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

        public void AddCloudAccounts()
        {
            try
            {
                var coreClient = GetDefaultCoreClient(host, port);

                var Amazon = new CloudAccount
                                  {
                                      CloudType = CloudType.Amazon,
                                      DisplayName = Properties.Settings.Default.CloudAmazoneAccountDisplayName,
                                      Credentials = new AmazonCredentials
                                                        {
                                                            AccountKey = Properties.Settings.Default.CloudAmazonSecretKey,
                                                            AccountName = Properties.Settings.Default.CloudAmazonAccessKey
                                                        }
                                  };



                var Azure = new CloudAccount
                                  {
                                      CloudType = CloudType.WindowsAzure,
                                      DisplayName = AzureAccountName,
                                      Credentials = new WindowsAzureCredentials
                                                        {
                                                            AccountKey = AzureKey,
                                                            AccountName = AzureStorageName
                                                        }
                                  };

                var OpenStack = new CloudAccount
                                  {
                                      CloudType = CloudType.OpenStack,
                                      DisplayName = OpenStackName,
                                      Credentials = new OpenStackCredentials
                                                        {
                                                            UserName = OpenStackUserName,
                                                            APIKey = OpenStackAPIKey
                                                        }

                                  };

                var Rackspace = new CloudAccount
                {
                    CloudType = CloudType.Rackspace,
                    DisplayName = RackspaceName,
                    Credentials = new RackspaceCredentials
                    {
                        UserName = OpenStackUserName,
                        APIKey = OpenStackAPIKey
                    }

                };


                //var Google = new CloudAccount
                //{
                //    CloudType = CloudType.Google,
                //    DisplayName = "Google",
                //    Credentials = new GoogleCredentials
                //     {
                //         PrivateKey = "notasecret",
                //         ProjectId = "direct-archery-87418",
                //         ServiceAccountEmail = "499778906700-epsg8907nib4oh54f1gjmqi0q9ev9a3l@developer.gserviceaccount.com"
                //     }

                //};

                // Add new cloud accounts
                try
                {
                    amazonAccountId = coreClient.CloudManagement.AddConfigurationAccount(Amazon);
                    azureAccountId = coreClient.CloudManagement.AddConfigurationAccount(Azure);
                    openStackAccountId = coreClient.CloudManagement.AddConfigurationAccount(OpenStack);
                    //rackspaceAccountId = coreClient.CloudManagement.AddConfigurationAccount(Rackspace);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to add new account, exception message = '{0}'", e.Message);
                    return;
                }
            }
            catch
            {
            }
        }

        public void DeleteCloudAccounts()
        {
            RegistryKey localKey;
            if (Environment.Is64BitOperatingSystem)
                localKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, host, RegistryView.Registry64);
            else
                localKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, host, RegistryView.Registry32);
            int countAccounts = localKey.OpenSubKey(@"SOFTWARE\AppRecovery\Core\CloudServiceConfiguration\Accounts").SubKeyCount;
            try
            {
                var coreClient = GetDefaultCoreClient(host, port);
                for (int i = countAccounts - 1; i >= 0 ; i--)
                {
                    var key = String.Format(@"SOFTWARE\AppRecovery\Core\CloudServiceConfiguration\Accounts\{0}", i);
                    coreClient.CloudManagement.DeleteConfigurationAccount(localKey.OpenSubKey(key).GetValue("AccountId").ToString());
                }
            }
            catch
            {  }
        }

        public void CreateRepository()
        {
            //const ulong GB = RepositorySizeGB;
            //The total size(in bytes) of DVM repository(including data and metadata) that will be added
            ulong RepositorySize = RepositorySizeGB * 1024 * 1024 * 1024UL;
            try
            {
                // Create the CoreClient
                var coreClient = GetDefaultCoreClient(host, port);
                // Validating existence of name of DVM repository
                var isNameAlreadyUsed = coreClient.RepositoryManagement.GetDvmRepositories().Any(x => x.Specification.Name == RepositoryName);
                if (isNameAlreadyUsed)
                {
                    return;
                }
                // Prepare the free space request
                var freeSpaceRequest = new FreeSpaceRequest { Path = DataPath };
                // Retrieves the free disk space in the directory or UNC share
                var freeDiskSpace = coreClient.RepositoryManagement.GetFreeDiskSpace(freeSpaceRequest);
                if (RepositorySize > freeDiskSpace)
                {
                    return;
                }

                // Prepare the DVM repository file specification
                var filesSpecification = new RepositoryFilesSpecification
                {
                    FileSpecifications = new Collection<RepositoryFileSpecification>
                    {
                        new RepositoryFileSpecification
                        {
                               DataPath = DataPath,
                               MetadataPath = MetadataPath,
                               Size = RepositorySize
                        }
                    }
                };
                // Verifies paths and free space on the specified devices
                coreClient.RepositoryManagement.VerifyFileSpecifications(filesSpecification);
                // Prepare the new DVM repository request
                var newRepository = new NewRepository
                {
                    FileSpecifications = filesSpecification.FileSpecifications,
                    Specification = new RepositorySpecification
                    {
                        EnableCompression = true,
                        EnableDedupe = true,
                        Name = RepositoryName
                    }
                };
                // Creates a new DVM repository
                var repositoryId = coreClient.RepositoryManagement.CreateRepository(newRepository);
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



        public string Convert(ulong bytes)
        {
            const int scale = 1024;
            var orders = new[] { "TB", "GB", "MB", "KB", "Bytes" };

            var max = (ulong)Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (bytes > max)
                {
                    return string.Format(CultureInfo.InvariantCulture, "{0:##.##} {1}", decimal.Divide(bytes, max), order);
                }

                max /= scale;
            }

            return "0 Bytes";
        }


        private static Uri GetAgentUri(string agentHostName, int agentPort)
        {
            var uriBuilder = new UriBuilder(Uri.UriSchemeHttps, agentHostName, (int)agentPort, string.Format(CultureInfo.InvariantCulture, "{0}/api/agent/", WcfConstants.RootOfServiceHostAddress));

            return uriBuilder.Uri;
        }

    }    

    
}
