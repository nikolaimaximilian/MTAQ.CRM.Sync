using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Client.Services;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace CustomSyncEntitiesCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start.");

            // Connect to CRM
            CrmConnection con = new CrmConnection("VACC-CRM-Training");
            IOrganizationService service = new OrganizationService(con);

            /*
            try
            {
                //
                // Setup Names
                //
                string syncLogEntityName = "vacc_SyncLog1";
                string syncItemEntityName = "vacc_SyncItem1";
                string syncConfigurationEntityName = "vacc_SyncConfiguration1";

                //
                // Delete Entities:
                //
                try
                {
                    DeleteEntityRequest derSyncLog = new DeleteEntityRequest()
                    {
                        LogicalName = syncLogEntityName
                    };
                    service.Execute(derSyncLog);
                    Console.WriteLine(syncLogEntityName + " successfully deleted.");
                } catch (Exception e) {
                    Console.WriteLine(syncLogEntityName + " does not exist.");
                }

                try
                {
                    DeleteEntityRequest derSyncItem = new DeleteEntityRequest()
                    {
                        LogicalName = syncItemEntityName
                    };
                    service.Execute(derSyncItem);
                    Console.WriteLine(syncItemEntityName + " successfully deleted.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(syncItemEntityName + " does not exist.");
                }

                try
                {
                    DeleteEntityRequest derSyncConfiguration = new DeleteEntityRequest()
                    {
                        LogicalName = syncConfigurationEntityName
                    };
                    service.Execute(derSyncConfiguration);
                    Console.WriteLine(syncConfigurationEntityName + " successfully deleted.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(syncConfigurationEntityName + " does not exist.");
                }


                //
                // Create SyncLog
                //
                CreateEntityRequest cerSyncLog = new CreateEntityRequest
                {
                    Entity = new EntityMetadata
                    {
                        SchemaName = syncLogEntityName,
                        DisplayName = new Label("Sync Log", 1033),
                        DisplayCollectionName = new Label("Sync Logs", 1033),
                        Description = new Label("Log for Sync", 1033),
                        OwnershipType = OwnershipTypes.UserOwned,
                        IsActivity = false
                    },

                    PrimaryAttribute = new StringAttributeMetadata
                    {
                        SchemaName = "vacc_LogType",
                        RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.ApplicationRequired),
                        MaxLength = 100,
                        FormatName = StringFormatName.Text,
                        DisplayName = new Label("Log Type", 1033),
                        Description = new Label("", 1033)
                    }
                };
                service.Execute(cerSyncLog);

                CreateAttributeRequest carLogMessage = new CreateAttributeRequest
                {
                    EntityName = syncLogEntityName,
                    Attribute = new StringAttributeMetadata
                    {
                        SchemaName = "vacc_LogMessage",
                        RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.ApplicationRequired),
                        MaxLength = 3000,
                        FormatName = StringFormatName.Text,
                        DisplayName = new Label("Log Message", 1033),
                        Description = new Label("", 1033)
                    }
                };
                service.Execute(carLogMessage);
                Console.WriteLine(syncLogEntityName + " successfully created.");

                //
                // Create SyncItem
                //
                CreateEntityRequest cerSyncItem = new CreateEntityRequest
                {
                    Entity = new EntityMetadata
                    {
                        SchemaName = syncItemEntityName,
                        DisplayName = new Label("Sync Item", 1033),
                        DisplayCollectionName = new Label("Sync Items", 1033),
                        Description = new Label("Holds all items that have to be pushed to apps still", 1033),
                        OwnershipType = OwnershipTypes.UserOwned,
                        IsActivity = false
                    },

                    PrimaryAttribute = new StringAttributeMetadata
                    {
                        SchemaName = "vacc_ItemReferenceId",
                        RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.ApplicationRequired),
                        MaxLength = 100,
                        FormatName = StringFormatName.Text,
                        DisplayName = new Label("Item Reference Id", 1033),
                        Description = new Label("", 1033)
                    }
                };
                service.Execute(cerSyncItem);

                CreateAttributeRequest carXmlData = new CreateAttributeRequest
                {
                    EntityName = syncItemEntityName,
                    Attribute = new StringAttributeMetadata
                    {
                        SchemaName = "vacc_XmlData",
                        RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.ApplicationRequired),
                        MaxLength = 10000,
                        FormatName = StringFormatName.TextArea,
                        DisplayName = new Label("Xml Data", 1033),
                        Description = new Label("", 1033)
                    }
                };
                service.Execute(carXmlData);
                Console.WriteLine(syncItemEntityName + " successfully created.");

                //
                // Create SyncConfiguration
                //
                CreateEntityRequest cerSyncConfiguration = new CreateEntityRequest
                {
                    Entity = new EntityMetadata
                    {
                        SchemaName = syncConfigurationEntityName,
                        DisplayName = new Label("Sync Configuration", 1033),
                        DisplayCollectionName = new Label("Sync Configurations", 1033),
                        Description = new Label("", 1033),
                        OwnershipType = OwnershipTypes.UserOwned,
                        IsActivity = false
                    },

                    PrimaryAttribute = new StringAttributeMetadata
                    {
                        SchemaName = "vacc_EntityName",
                        RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.ApplicationRequired),
                        MaxLength = 100,
                        FormatName = StringFormatName.Text,
                        DisplayName = new Label("Entity Name", 1033),
                        Description = new Label("", 1033)
                    }
                };
                service.Execute(cerSyncConfiguration);

                CreateAttributeRequest carSyncConfigurationXmlData = new CreateAttributeRequest
                {
                    EntityName = syncConfigurationEntityName,
                    Attribute = new StringAttributeMetadata
                    {
                        SchemaName = "vacc_XmlData",
                        RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.ApplicationRequired),
                        MaxLength = 10000,
                        FormatName = StringFormatName.TextArea,
                        DisplayName = new Label("Xml Data", 1033),
                        Description = new Label("", 1033)
                    }
                };
                service.Execute(carSyncConfigurationXmlData);

                CreateAttributeRequest carSyncConfigurationActive = new CreateAttributeRequest
                {
                    EntityName = syncConfigurationEntityName,
                    Attribute = new BooleanAttributeMetadata
                    {
                        SchemaName = "vacc_Active",
                        RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.ApplicationRequired),
                        DisplayName = new Label("Active", 1033),
                        Description = new Label("", 1033)
                    }
                };
                service.Execute(carSyncConfigurationActive);

                CreateAttributeRequest carSyncConfigurationWebserviceUrl = new CreateAttributeRequest
                {
                    EntityName = syncConfigurationEntityName,
                    Attribute = new StringAttributeMetadata
                    {
                        SchemaName = "vacc_WebserviceUrl",
                        RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.ApplicationRequired),
                        MaxLength = 1000,
                        FormatName = StringFormatName.Text,
                        DisplayName = new Label("Webservice Url", 1033),
                        Description = new Label("", 1033)
                    }
                };
                service.Execute(carSyncConfigurationWebserviceUrl);

                CreateAttributeRequest carSyncConfigurationWebserviceUser = new CreateAttributeRequest
                {
                    EntityName = syncConfigurationEntityName,
                    Attribute = new StringAttributeMetadata
                    {
                        SchemaName = "vacc_WebserviceUser",
                        RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                        MaxLength = 1000,
                        FormatName = StringFormatName.Text,
                        DisplayName = new Label("Webservice User", 1033),
                        Description = new Label("", 1033)
                    }
                };
                service.Execute(carSyncConfigurationWebserviceUser);

                CreateAttributeRequest carSyncConfigurationWebservicePwd = new CreateAttributeRequest
                {
                    EntityName = syncConfigurationEntityName,
                    Attribute = new StringAttributeMetadata
                    {
                        SchemaName = "vacc_WebservicePwd",
                        RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                        MaxLength = 1000,
                        FormatName = StringFormatName.Text,
                        DisplayName = new Label("Webservice Pwd", 1033),
                        Description = new Label("", 1033)
                    }
                };
                service.Execute(carSyncConfigurationWebservicePwd);
                Console.WriteLine(syncConfigurationEntityName + " successfully created.");

                Console.WriteLine("-->> Everything successfully created.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Console.WriteLine("Start.");
                Console.Read();
            }
            */
        }
    }
}
