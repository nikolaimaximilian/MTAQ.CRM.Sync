// <copyright file="ProcessSyncItems.cs" company="">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author></author>
// <date>12/19/2014 10:23:12 AM</date>
// <summary>Implements the ProcessSyncItems Workflow Activity.</summary>
namespace VACC.CRM.Sync.Workflow
{
    using System;
    using System.Activities;
    using System.ServiceModel;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;
    using Microsoft.Xrm.Sdk.Query;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using CRMSyncWeb;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class ProcessSyncItems : CodeActivity
    {
        // Define Input/Output Arguments
        [RequiredArgument]
        [Input("InputEntity")]
        [ReferenceTarget("vacc_syncitem")]
        public InArgument<EntityReference> inputEntity { get; set; }

        //[Output("TaskCreated")]
        //[ReferenceTarget("task")]
        //public OutArgument<EntityReference> taskCreated { get; set; }

        /// <summary>
        /// Executes the workflow activity.
        /// </summary>
        /// <param name="executionContext">The execution context.</param>
        protected override void Execute(CodeActivityContext executionContext)
        {

            System.Diagnostics.Debugger.Launch();
            System.Diagnostics.Debug.WriteLine("In Execute");

            // Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            if (tracingService == null)
            {
                throw new InvalidPluginExecutionException("Failed to retrieve tracing service.");
            }

            tracingService.Trace("Entered ProcessSyncItems.Execute(), Activity Instance Id: {0}, Workflow Instance Id: {1}",
                executionContext.ActivityInstanceId,
                executionContext.WorkflowInstanceId);

            // Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            System.Diagnostics.Debug.WriteLine("Have context");

            if (context == null)
            {
                throw new InvalidPluginExecutionException("Failed to retrieve workflow context.");
            }

            tracingService.Trace("ProcessSyncItems.Execute(), Correlation Id: {0}, Initiating User: {1}",
                context.CorrelationId,
                context.InitiatingUserId);

            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            System.Diagnostics.Debug.WriteLine("Have service");

            try
            {


                // TODO: Implement your custom Workflow business logic.
                //throw new InvalidWorkflowException();

                // Retrieve the id
                Guid id = this.inputEntity.Get(executionContext).Id;
                System.Diagnostics.Debug.WriteLine("Id:{0}", id.ToString());


                tracingService.Trace("syncItem  Id: {0}", id);

                

                Entity item = service.Retrieve("vacc_syncitem", id, new ColumnSet { AllColumns = true });

                tracingService.Trace("retrieved syncItem  data");


                // TODO: Process the XML for each item that is pulled back
                System.Diagnostics.Debug.WriteLine("before Xdoc");

                //need to extract info from data
                XDocument config = XDocument.Parse((item["vacc_xmldata"].ToString()));
                XAttribute entity = config.XPathSelectElement("/data").Attribute("entity");
                XAttribute crmId = config.XPathSelectElement("/data").Attribute("crmid");
                XAttribute mappingId = config.XPathSelectElement("/data").Attribute("mappingid");
                XAttribute url = config.XPathSelectElement("/data").Attribute("wsurl");
                XAttribute method = config.XPathSelectElement("/data").Attribute("method");
                XAttribute action = config.XPathSelectElement("/data").Attribute("action");
                XAttribute username = config.XPathSelectElement("/data").Attribute("wsuser");
                XAttribute pwd = config.XPathSelectElement("/data").Attribute("wspwd");
                //get the name of any sync field values
                XAttribute synced = config.XPathSelectElement("/data").Attribute("synced");

                System.Diagnostics.Debug.WriteLine("After Xdoc");

                tracingService.Trace("Extracted xml data. url:{0}\nmethod:{1}\nusername:{2}\npwd:{3}", url.Value, method.Value, username.Value, pwd.Value);

                // TODO: Create the Authentication header
                //syncService.ClientCredentials.UserName.UserName = "sdfsdf";
                //syncService.ClientCredentials.UserName.Password = "sdfsdf";

                // TODO: Call the defined WS
                tracingService.Trace("Create payload for syncWebService");
                tracingService.Trace("contactid:{0}", item.Id.ToString());
                Payload payload = new Payload { id = (Guid.Parse(mappingId.Value)), xmlString = item["vacc_xmldata"].ToString() };
                System.Diagnostics.Debug.WriteLine("payload created");
                tracingService.Trace("called webservice");
                //*
                CRMSyncWeb.Result r = null;
                try
                {
                    System.Diagnostics.Debug.WriteLine("call webservice");
                    r = RestService.callRestService(url.Value,
                                                    method.Value,
                                                    operationType(action.Value),
                                                    username.Value,
                                                    pwd.Value,
                                                    payload //new Payload { id = (Guid)item["id"], xmlString = item["vacc_xmldata"].ToString() }
                                                    );

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    tracingService.Trace("Error in CallRestService:{0}", ex.Message);
                }

                /*/
                Result r = new Result { success = false, isClientError = false, errorMessage = "" };
                //*/
                tracingService.Trace("Completed WebRequest");

                // TODO: Check the results from call
                if (r.success)
                {
                    System.Diagnostics.Debug.WriteLine("success - {0}:{1}:{2}", r.success, r.isClientError, r.errorMessage);
                        //
                    if (action.Value.ToLower() == "create")
                    {
                        Entity e = null;
                        e = service.Retrieve(entity.Value, Guid.Parse(crmId.Value), new ColumnSet { AllColumns = true });
                        Entity e2 = new Entity(entity.Value);
                        e2.Id = e.Id;

                        e2["vacc_synced"] = synced.Value;

                        //for a contact, get the logonname back.
                        XAttribute responseKeyValue = config.XPathSelectElement("/data").Attribute("response");
                        if (responseKeyValue != null)
                        {
                            XDocument res = XDocument.Parse(r.data);
                            List<string> keyValues = new List<string>(responseKeyValue.Value.Split(','));

                            //get response info
                            /*
                            var v = from c in res.Descendants("field") //Root.Elements("field")
                                    where c.Attribute("name").Value.ToLower().Equals(items[0])
                                    select new KeyValuePair <string,string> (c.Attribute("name").Value.ToLower(), c.Attribute("value").Value.ToLower());
                            */
                            var v = res.Descendants("field").ToDictionary(c => c.Attribute("name").Value.ToLower(),
                                                                           c => c.Attribute("value").Value.ToLower());
                            /*
                            List<string> fields = new List<string>();
                            foreach (string keyValue in keyValues)
                            {
                                string[] items = keyValue.ToLower().Split('=');
                                fields.Add(items[1]);
                            }

                            fields.Add("vacc_synced");
                            e = service.Retrieve(entity.Value, Guid.Parse(crmId.Value), new ColumnSet("vacc_synced"));
                            */
                            foreach (string keyValue in keyValues)
                            {
                                string[] items = keyValue.ToLower().Split('=');

                               // XAttribute resItem = res.XPathSelectElement("field").Attribute(items[0]);

                                if (v.ContainsKey(items[0]))
                                {
                                    try
                                    {
                                        e2[items[1]] = v[items[0]];
                                    }
                                    catch (Exception ex)
                                    {
                                        Entity log = new Entity("vacc_synclog");
                                        log["vacc_logtype"] = "Warning";
                                        log["vacc_logmessage"] = "Did not find Key in entity. syncItemId:" + id.ToString() + "\n" + ex.Message;
                                        service.Create(log);
                                    }
                                }
                                /*
                                if (e.Contains(items[1]))
                                {
                                    e[items[1]] = resItem.Value;
                                }
                                */
                            }
                        }
                        /*
                        else
                            e = service.Retrieve(entity.Value, Guid.Parse(crmId.Value), new ColumnSet("vacc_synced"));

                        e["vacc_synced"] = synced.Value;
                        */
                        service.Update(e2);                      
                  }

                    
                    tracingService.Trace("success webservice:{0},{1},{2}", r.success.ToString(), r.isClientError.ToString(), r.errorMessage);
                    //remove the item after processing
                    //service.Delete("vacc_syncitem", id);
                    tracingService.Trace("Removed item after success:{0}", id.ToString());

                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("{0}:{1}:{2}",r.success, r.isClientError, r.errorMessage);
                    tracingService.Trace("failed webservice:{0},{1},{2}", r.success.ToString(), r.isClientError.ToString(), r.errorMessage);
                    Entity log = new Entity("vacc_synclog");
                    log["vacc_logtype"] = "Warning";
                    log["vacc_logmessage"] = "syncItemId:" + id.ToString() + "\n" +  r.errorMessage;
                    service.Create(log);
                }


                // TODO: Implement your custom Workflow business logic.
                //throw new InvalidWorkflowException();
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                tracingService.Trace("Exception: {0}", e.ToString());

                // Handle the exception.
                throw;
            }

            tracingService.Trace("Exiting ProcessSyncItems.Execute(), Correlation Id: {0}", context.CorrelationId);
            //throw new InvalidWorkflowException();
        }

        private string operationType(string action)
        {
            string method = string.Empty;

            switch (action.ToLower())
            {   
                case "create":
                    method = "POST";
                    break;
                case "update":
                    method = "PUT";
                    break;
                case "delete":
                    method = "DELETE";
                    break;
                default:
                    method = "GET";
                    break;
            }

            return method.ToUpper();
        }

    }
}