using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Xml.Linq;
using VACC.Common;
using VACC.Distribution;
using VACC.Logging;
using VACC.Security;
using CRMSyncWeb.Helpers;


using sec = VACC.Security;

namespace CRMSyncWeb
{
    public class CRMSyncWebService : ICRMSyncService
    {
        #region Class Variables

        private Result r;
        private FileLogger log;
        private Guid syncUserId = Guid.Empty;

        #endregion

        public CRMSyncWebService()
            : base()
        {
            // Create the logger
            log = new FileLogger(WebConfigurationManager.AppSettings["logDir"] + "sync.txt");
            log.logLevel = int.Parse(WebConfigurationManager.AppSettings["logLevel"]);

            processLog("Request received x.", log, null, VACC.Logging.Logger.INFO);

            r = new Result() { success = true, errorMessage = string.Empty, isClientError = false, data = string.Empty };
            processLog("Created result.", log, null, VACC.Logging.Logger.INFO);

            // Set the connection strings

            try
            {
                var sc = WebConfigurationManager.AppSettings["SecurityConnection"];
                processLog("Got AppSettings: " + sc, log, null, VACC.Logging.Logger.INFO);
                string dsc = sec::Encryption.Crypto.decryptString(sc);
                processLog("decrypted AppSettings: " + dsc, log, null, VACC.Logging.Logger.INFO);
                sec::Settings.connectionString = dsc;
                processLog("Set Security connection string.", log, null, VACC.Logging.Logger.INFO);

                VACC.Common.Settings.connectionString = sec::Encryption.Crypto.decryptString(WebConfigurationManager.AppSettings["CommonConnection"]);
                processLog("Set Common connection string.", log, null, VACC.Logging.Logger.INFO);
                VACC.Distribution.Settings.connectionString = sec::Encryption.Crypto.decryptString(WebConfigurationManager.AppSettings["NotificationsConnection"]);
                processLog("Set Distribution connection string.", log, null, VACC.Logging.Logger.INFO);


                //*
                DNNData.connectionString = WebConfigurationManager.AppSettings["DNNConnection"];
                /*/
                DNNData.connectionString = "Data Source=172.16.1.25;Initial Catalog=mtaq.com.au;User ID=memberHosting;Password=V@ccSQL2008";
                DNNData.connectionString = "Data Source=172.17.1.20;Initial Catalog=t422.vaccwebdesign.com.au;User ID=slavin2;Password=P@ssw0rd";
                //*/
                processLog("Set DNNData connection string.", log, null, VACC.Logging.Logger.INFO);

                processLog("security connection string:" + dsc, log, null, VACC.Logging.Logger.INFO);				
            }
            catch(Exception e)
            {
                processLog("exception: " + e.Message + "\nstack: " + e.StackTrace, log, null, VACC.Logging.Logger.INFO);
            }
        }

        #region REST User

        public Result getUser(string id)
        {
            if(!AuthenticateUser())
            {
                r = UnauthorisedResult();
            }
            else
            {
                processGetUser(id);
            }

            setHttpStatusCodeForResult(r);

            return r;
        }

        public Result createUser(Payload payload)
        {
            if(!AuthenticateUser())
            {
                r = UnauthorisedResult();
            }
            else
            {
                processCreateUser(payload);
            }

            setHttpStatusCodeForResult(r);

            return r;
        }

        public Result updateUser(string id, Payload payload)
        {
            if(!AuthenticateUser())
            {
                r = UnauthorisedResult();
            }
            else
            {
                processUpdateUser(id, payload);
            }

            setHttpStatusCodeForResult(r);

            return r;
        }

        public Result deleteUser(string id)
        {
            if(!AuthenticateUser())
            {
                r = UnauthorisedResult();
            }
            else
            {
                processDeleteUser(id);
            }

            setHttpStatusCodeForResult(r);

            return r;
        }

        #endregion

        #region REST Business

        public Result getBusiness(string id)
        {
            if(!AuthenticateUser())
            {
                r = UnauthorisedResult();
            }
            else
            {
                processGetBusiness(id);
            }

            setHttpStatusCodeForResult(r);

            return r;
        }

        public Result creatBusiness(Payload payload)
        {
            processLog("Create business.", log, null, VACC.Logging.Logger.INFO);
            //*
            if(!AuthenticateUser())
            {
                r = UnauthorisedResult();
            }
            else
            {
                processCreateBusiness(payload);
            }

            setHttpStatusCodeForResult(r);

            //*/
            return r;
        }

        public Result updateBusiness(string id, Payload payload)
        {
            if(!AuthenticateUser())
            {
                r = UnauthorisedResult();
            }
            else
            {
                processUpdateBusiness(id, payload);
            }

            setHttpStatusCodeForResult(r);

            return r;
        }

        public Result deleteBusiness(string id)
        {
            if(!AuthenticateUser())
            {
                r = UnauthorisedResult();
            }
            else
            {
                processDeleteBusiness(id);
            }

            setHttpStatusCodeForResult(r);

            return r;
        }

        #endregion


        #region REST service

        public Result getService(string id)
        {
            if(!AuthenticateUser())
            {
                r = UnauthorisedResult();
            }
            else
            {
                processGetService(id);
            }

            setHttpStatusCodeForResult(r);

            return r;
        }


        public Result createService(Payload payload)
        {
            processLog("Create service.", log, null, VACC.Logging.Logger.INFO);
            if(!AuthenticateUser())
            {
                r = UnauthorisedResult();
            }
            else
            {
                processCreateService(payload);
            }

            setHttpStatusCodeForResult(r);

            return r;
        }


        public Result updateService(string id, Payload payload)
        {
            processLog("Update service.", log, null, VACC.Logging.Logger.INFO);
            if(!AuthenticateUser())
            {
                r = UnauthorisedResult();
            }
            else
            {
                processUpdateService(id, payload);
            }

            setHttpStatusCodeForResult(r);

            return r;
        }


        public Result deleteService(Payload payload)
        {
            processLog("Detele service.", log, null, VACC.Logging.Logger.INFO);
            if(!AuthenticateUser())
            {
                r = UnauthorisedResult();
            }
            else
            {
                processDeleteService(payload);
            }

            setHttpStatusCodeForResult(r);

            return r;
        }

        #endregion

        #region Process User
        private void processGetUser(string id)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch(NotImplementedException nie)
            {
                processLog(nie.Message, log, nie, VACC.Logging.Logger.INFO);
            }
        }

        private void processCreateUser(Payload payload)
        {
            try
            {
                processLog("Starting User Creation.", log, null, VACC.Logging.Logger.INFO);

                // Get the required fields
                // If one of those fields is not in the payload then an exception is thrown!
                string contactIdString = getPayloadFieldValueByName(payload, "id", true);
                string firstname = getPayloadFieldValueByName(payload, "firstname", true);
                string lastname = getPayloadFieldValueByName(payload, "lastname", true);
                string businessIdString = getPayloadFieldValueByName(payload, "businessid", true);

                Guid contactId = Guid.Parse(contactIdString);
                Guid businessId = Guid.Parse(businessIdString);

                // Does the User/Contact already exist in the Security DB?
                User u = User.getObject(contactId);
                if(u != null)
                {
                    throw new DataMismatchException("User/Contact (Id = " + contactId + ") does already exist in the Security DB. Please perform an update for any further changes.");
                }

                // Does the Business/Account/Group exist? We need it in there to add the User.
                Group g = Group.getObject(businessId);
                if(g == null)
                {
                    throw new DataMismatchException("Business/Account/Group (Id = " + businessId + ") does not exist in Security DB yet. Please sync Business/Account/Group before Contact/User (Id = " + contactId + ")");
                }

                // Remove spaces and quotes from names
                string cleanFirstName = firstname.Replace("'", "").Replace(" ", "");
                string cleanLastName = lastname.Replace("'", "").Replace(" ", "");

                // Create a logonname
                string logonname = string.Empty;

                // Use email if in payload and no User/Contact with that email exists in the Security DB
                string email = getPayloadFieldValueByName(payload, "email", false);
                if(email.Length > 0 && User.getByLogonName(email) == null)
                {
                    logonname = email;
                }

                // Otherwise make it the first letter of the firstname and the full lastname in one word.
                // And this logonname is already taken, keep adding 1 to the end until we have an available logonname
                else
                {
                    logonname = cleanFirstName.Substring(0, 1).ToLower() + cleanLastName;
                    int originalLogonnameLength = logonname.Length;

                    while(User.getByLogonName(logonname) != null)
                    {
                        int number;
                        if(logonname.Equals(cleanFirstName.Substring(0, 1).ToLower() + cleanLastName))
                            number = 1;
                        else
                        {
                            number = int.Parse(logonname.Substring(originalLogonnameLength, logonname.Length - originalLogonnameLength));
                            number++;
                        }
                        logonname = cleanFirstName.Substring(0, 1).ToLower() + cleanLastName + number;
                        logonname = logonname.ToLower();
                    }
                }

                processLog("Logonname for user: " + logonname, log, null, VACC.Logging.Logger.INFO);

                // Create the User
                sec::User newUser = new sec::User(contactId, logonname, WebConfigurationManager.AppSettings["NewUserPassword"], UserSecretQuestion.defaultQuestionID, "", DateTime.MinValue, true, DateTime.MinValue, 0, syncUserId);
                newUser.save();

                processLog("User saved.", log, null, VACC.Logging.Logger.INFO);

                // And add the User to the Business
                UserGroup.save(newUser.ID, businessId, 0, syncUserId);

                // Save Firstname and Lastname in Common
                VACC.Common.Profile.save(CommonGroups.personalDetails, contactId, "First_Name", firstname, 0, syncUserId);
                VACC.Common.Profile.save(CommonGroups.personalDetails, contactId, "Last_Name", lastname, 0, syncUserId);

                processLog("Firstname (" + firstname + ") and Lastname (" + lastname + ") saved.", log, null, VACC.Logging.Logger.INFO);

                // And also the Email if we have received it
                if(email.Length > 0)
                {
                    VACC.Common.Profile.save(CommonGroups.personalDetails, contactId, "Primary_Email", email, 0, syncUserId);
                    processLog("Email (" + email + ") saved.", log, null, VACC.Logging.Logger.INFO);
                }

                processLog("Created new user: " + contactId.ToString(), log, null, VACC.Logging.Logger.INFO);

                r.data = "<fields><field name=\"logonname\" value=\"" + logonname + "\"/></fields>";

                processLog("User Creation done.", log, null, VACC.Logging.Logger.INFO);

            }
            catch(InvalidWebServiceCallException iwsce)
            {
                processLog(iwsce.Message, log, iwsce, VACC.Logging.Logger.INFO);
            }
            catch(DataMismatchException dme)
            {
                processLog(dme.Message, log, dme, VACC.Logging.Logger.INFO);
            }
            catch(Exception e)
            {
                processLog(e.Message, log, e, VACC.Logging.Logger.INFO);
            }
        }

        private void processUpdateUser(string id, Payload payload)
        {
            try
            {
                string contactIdString = id; // getPayloadFieldValueByName(payload, "id", true);
                Guid contactId = Guid.Parse(contactIdString);

                processLog("Starting Updates for User (id = " + contactId + " ).", log, null, VACC.Logging.Logger.INFO);

                // Update User row entry in the User Table
                User u = User.getObject(contactId);
                if(u == null)
                {
                    throw new DataMismatchException("The User (Id = " + contactId + ") you are trying to update does not exist in the DB.");
                }

                // Set password if received
                string password = getPayloadFieldValueByName(payload, "password", false);
                if(password.Length > 0)
                {
                    u.password = password;
                    processLog("New Password set.", log, null, VACC.Logging.Logger.INFO);
                }

                // Save user
                u.zUserID = syncUserId;
                u.save();

                processLog("User saved.", log, null, VACC.Logging.Logger.INFO);

                // Set firstname if received
                string firstname = getPayloadFieldValueByName(payload, "firstname", false);
                if(firstname.Length > 0)
                {
                    VACC.Common.Profile.save(CommonGroups.personalDetails, contactId, "First_Name", firstname, 0, syncUserId);
                    processLog("Firstname (" + firstname + ") saved.", log, null, VACC.Logging.Logger.INFO);
                }

                // Set lastname if received
                string lastname = getPayloadFieldValueByName(payload, "lastname", false);
                if(lastname.Length > 0)
                {
                    VACC.Common.Profile.save(CommonGroups.personalDetails, contactId, "Last_Name", lastname, 0, syncUserId);
                    processLog("Lastname (" + lastname + ") saved.", log, null, VACC.Logging.Logger.INFO);
                }


                // Set email if received
                string email = getPayloadFieldValueByName(payload, "email", false);
                if(email.Length > 0)
                {
                    VACC.Common.Profile.save(CommonGroups.personalDetails, contactId, "Primary_Email", email, 0, syncUserId);
                    processLog("Email (" + email + ") saved.", log, null, VACC.Logging.Logger.INFO);
                }


                // Set account/business/gourp if received
                string businessId = getPayloadFieldValueByName(payload, "businessid", false);
                if(businessId.Length > 0)
                {
                    UserGroup.save(contactId, Guid.Parse(businessId), 0, syncUserId);
                    processLog("UserGroup saved.", log, null, VACC.Logging.Logger.INFO);
                }
                else
                {
                    // Delete all UserGroup entries for User/Contact
                    DataSet ugs = UserGroup.getList("UserID = '" + contactId + "'");
                    int count = 0;
                    foreach(DataRow ug in ugs.Tables[0].Rows)
                    {
                        UserGroup.delete(contactId, Guid.Parse(ug["GroupID"].ToString()));
                        count++;
                    }
                    processLog("UserGroup(s) deleted (" + count + ").", log, null, VACC.Logging.Logger.INFO);
                }

                processLog("All Updates for User done.", log, null, VACC.Logging.Logger.INFO);

                r.success = true;
                r.isClientError = false;
                r.errorMessage = string.Empty;
                r.data = "";
            }
            catch(InvalidWebServiceCallException iwsce)
            {
                processLog(iwsce.Message, log, iwsce, VACC.Logging.Logger.INFO);
            }
            catch(DataMismatchException dme)
            {
                processLog(dme.Message, log, dme, VACC.Logging.Logger.INFO);
            }
            catch(Exception e)
            {
                processLog(e.Message, log, e, VACC.Logging.Logger.INFO);
            }
        }

        private void processDeleteUser(string id)
        {
            try
            {
                Guid contactId = Guid.Parse(id);

                processLog("Starting to delete User (id = " + contactId + ")", log, null, VACC.Logging.Logger.INFO);

                // Check if the User/Contact exists
                User u = User.getObject(contactId);
                if(u == null)
                {
                    throw new DataMismatchException("The User (Id = " + contactId + ") you are trying to delete does not exist in the DB.");
                }

                // Delete all UserGroup entries for User/Contact
                DataSet ugs = UserGroup.getList("UserID = '" + contactId + "'");
                foreach(DataRow ug in ugs.Tables[0].Rows)
                {
                    UserGroup.delete(contactId, Guid.Parse(ug["GroupID"].ToString()));
                }

                // Delete the User
                u.delete();

                processLog("User is removed.", log, null, VACC.Logging.Logger.INFO);

            }
            catch(InvalidWebServiceCallException iwsce)
            {
                processLog(iwsce.Message, log, iwsce, VACC.Logging.Logger.INFO);
            }
            catch(DataMismatchException dme)
            {
                processLog(dme.Message, log, dme, VACC.Logging.Logger.INFO);
            }
            catch(Exception e)
            {
                processLog(e.Message, log, e, VACC.Logging.Logger.INFO);
            }
        }

        #endregion

        #region Process Business


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        private void processGetBusiness(string id)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch(NotImplementedException nie)
            {
                processLog(nie.Message, log, nie, VACC.Logging.Logger.INFO);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        private void processCreateBusiness(Payload payload)
        {
            try
            {

                processLog("Starting Business Creation.", log, null, VACC.Logging.Logger.INFO);

                // Get the required fields
                // If one of those fields is not in the payload then an exception is thrown!
                /*
                string businessIdString = getPayloadFieldValueByName(payload, "id", true);
                string memberNumber = getPayloadFieldValueByName(payload, "memberno", true);
                string businessName = getPayloadFieldValueByName(payload, "tradingname", true);
                /*/
                string businessIdString = payload.valReq("id");
                string memberNo = payload.valReq("memberno");
                string tradingName = payload.valReq("tradingname");
                //*/

                /*
                string memberOf = getPayloadFieldValueByName(payload, "memberof", true);
                /*/
                string memberOf = "mtaq";                                                       /// TODO: fix this nasty hard-coded hack for MTAQ
                //*/

                Guid businessId = Guid.Parse(businessIdString);

                // Does the Business/Account/Group exist? We need it in there to add the User.
                Group g = Group.getObject(businessId);
                if(g != null)
                {
                    throw new DataMismatchException("Business/Account/Group (Id = " + businessId + ") already exists in the Security DB. Please perform an update for any further changes.");
                }

                Guid securityGroupId = getSecurityGroupIdForMemberType(memberOf);

                // Create new Group/Business
                g = new Group();
                g.ID = businessId;
                g.name = memberNo;
                g.description = tradingName;
                g.zUserID = syncUserId;
                g.save();
                processLog("Group/Business saved (" + businessIdString + ", " + memberNo + ", " + tradingName + ").", log, null, VACC.Logging.Logger.INFO);

                // Set the Trading Name for the business
                VACC.Common.Profile.save(CommonGroups.businessDetails, businessId, "Trading_Name", tradingName, 0, syncUserId);

                // Set finacial status TRUE for a newly created business, as the CRM will just send a business once it has become financial
                VACC.Common.Profile.save(CommonGroups.businessDetails, businessId, "isFinancial", "True", 0, syncUserId);
                processLog("Trading_Name (" + tradingName + ") and Finanical Status (True) saved in Profile-Tbl.", log, null, VACC.Logging.Logger.INFO);

                // Add Business to Security Group
                GroupGroup.save(securityGroupId, businessId, 0, syncUserId);
                processLog("Business added to security group (" + memberOf + ").", log, null, VACC.Logging.Logger.INFO);

                new DNNData().insertBusiness(businessIdString, memberNo, tradingName, payload.val("addressline1"), payload.val("suburb"), payload.val("state"), payload.val("postcode"), payload.val("phone"), payload.val("email"), payload.val("websiteurl"));

                processLog("Business successfully created.", log, null, VACC.Logging.Logger.INFO);

                r.success = true;
                r.isClientError = false;
                r.errorMessage = string.Empty;
                r.data = "";

            }
            catch(InvalidWebServiceCallException iwsce)
            {
                processLog(iwsce.Message, log, iwsce, VACC.Logging.Logger.INFO);
            }
            catch(DataMismatchException dme)
            {
                processLog(dme.Message, log, dme, VACC.Logging.Logger.INFO);
            }
            catch(Exception e)
            {
                processLog(e.Message, log, e, VACC.Logging.Logger.INFO);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="payload"></param>
        private void processUpdateBusiness(string id, Payload payload)
        {
            try
            {
                string businessIdString = id; // getPayloadFieldValueByName(payload, "id", true);
                Guid businessId = Guid.Parse(businessIdString);

                processLog("Starting Updates for Group/Business (id = " + businessId + " ).", log, null, VACC.Logging.Logger.INFO);

                // Update Group/Business row entry in the Group Table
                Group g = Group.getObject(businessId);
                if(g == null)
                {
                    throw new DataMismatchException("The Group (Id = " + businessId + ") you are trying to update does not exist in the DB.");
                }

                // Update Membernumber
                //string memberNo = getPayloadFieldValueByName(payload, "memberno", false);
                string memberNo = payload.val("memberno");
                if(memberNo.Length > 0)
                {
                    g.name = memberNo;
                    processLog("New Member Number (" + memberNo + ") set.", log, null, VACC.Logging.Logger.INFO);
                }
                memberNo = g.name;

                // Update Trading Name
                //string tradingName = getPayloadFieldValueByName(payload, "tradingname", false);
                string tradingName = payload.val("tradingname");
                if(tradingName.Length > 0)
                {
                    g.description = tradingName;
                    VACC.Common.Profile.save(CommonGroups.businessDetails, businessId, "Trading_Name", tradingName, 0, syncUserId);
                    processLog("New Trading Name (" + tradingName + ") set (In Group Object, and saved in Profile DB).", log, null, VACC.Logging.Logger.INFO);
                }

                // Save Group/Business
                g.zUserID = syncUserId;
                g.save();
                processLog("Group saved.", log, null, VACC.Logging.Logger.INFO);





                // Update Financial Status and Security
                /*
                string financialStatus = getPayloadFieldValueByName(payload, "isfinancial", false);
                string memberOf = getPayloadFieldValueByName(payload, "memberof", false);
                /*/
                string financialStatus = payload.val("isfinancial");
                string memberOf = "mtaq";
                //*/

                bool isFinancial = financialStatus.ToLower() != "false";                                                                                    // the only time we want to remove rights is if the value is a valid response and is "false" - any other value *doesn't* mean false

                processLog("about to update member: " + memberNo, log, null, VACC.Logging.Logger.INFO);

                new DNNData(log).updateBusiness(businessIdString, memberNo, tradingName, payload.val("addressline1"), payload.val("suburb"), payload.val("state"), payload.val("postcode"), payload.val("phone"), payload.val("email"), payload.val("websiteurl"), isFinancial);

                if(financialStatus.Length > 0 && memberOf.Length > 0)
                {
                    if(financialStatus.ToLower().Equals("true") || financialStatus.ToLower().Equals("false"))
                    {
                        VACC.Common.Profile.save(CommonGroups.businessDetails, businessId, "isFinancial", financialStatus, 0, syncUserId);
                        processLog("Financial Status (" + financialStatus + ") saved.", log, null, VACC.Logging.Logger.INFO);

                        Guid securityGroupId = getSecurityGroupIdForMemberType(memberOf);

                        // Add or remove Security group
                        if(financialStatus.ToLower().Equals("true"))
                        {
                            // Add Business to Security Group
                            GroupGroup.save(securityGroupId, businessId, 0, syncUserId);
                            processLog("Business added to security group (" + memberOf + ").", log, null, VACC.Logging.Logger.INFO);
                        }
                        else
                        {
                            // Remove Business from Security Group
                            GroupGroup.delete(securityGroupId, businessId);
                            processLog("Business removed from Security group.", log, null, VACC.Logging.Logger.INFO);

                            // And remove users from the business
                            DataSet ugs = UserGroup.getList("GroupID = '" + businessId + "'");
                            foreach(DataRow ug in ugs.Tables[0].Rows)
                            {
                                Guid userId = Guid.Parse(ug["UserID"].ToString());
                                Guid groupId = Guid.Parse(ug["GroupID"].ToString());
                                UserGroup.delete(userId, groupId);
                                processLog("User (" + userId + ") removed from Business (" + groupId + ").", log, null, VACC.Logging.Logger.INFO);
                            }
                            processLog("All users removed from Business.", log, null, VACC.Logging.Logger.INFO);
                        }

                    }
                    else
                    {
                        throw new DataMismatchException("Wrong value received for financial status (boolean).");
                    }
                }


                processLog("All Updates for Group/Business (" + businessIdString + ") done.", log, null, VACC.Logging.Logger.INFO);

                r.success = true;
                r.isClientError = false;
                r.errorMessage = string.Empty;
                r.data = "";

            }
            catch(InvalidWebServiceCallException iwsce)
            {
                processLog(iwsce.Message, log, iwsce, VACC.Logging.Logger.INFO);
            }
            catch(DataMismatchException dme)
            {
                processLog(dme.Message, log, dme, VACC.Logging.Logger.INFO);
            }
            catch(Exception e)
            {
                processLog(e.Message, log, e, VACC.Logging.Logger.INFO);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        private void processDeleteBusiness(string id)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch(NotImplementedException nie)
            {
                processLog(nie.Message, log, nie, VACC.Logging.Logger.INFO);
            }
        }

        #endregion


        #region Process Service


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        private void processGetService(string id)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch(NotImplementedException nie)
            {
                processLog(nie.Message, log, nie, VACC.Logging.Logger.INFO);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        private void processCreateService(Payload payload)
        {
            try
            {
                /*
                string businessId = payload.valReq("id");
                string businessServiceId = payload.valReq("cent_businessserviceid");
                string serviceName = payload.valReq("cent_name");

                Group g = Group.getObject(Guid.Parse(businessId));
                if(g == null)
                {
                    throw new DataMismatchException("The Group (Id = " + businessId + ") you are trying to add a service to does not exist in the security DB.");
                }
                processLog("Business: " + g.description, log, null, VACC.Logging.Logger.INFO);

                serviceName = serviceName.Replace(g.description + " - ", string.Empty);                 // remove the trading name from the start of the serviceName (CRM puts this on)
                processLog("MemberNo: " + g.name, log, null, VACC.Logging.Logger.INFO);
                processLog("serviceName: " + serviceName, log, null, VACC.Logging.Logger.INFO);

                new DNNData(log).updateService(g.name, serviceName, businessServiceId);

                processLog("Service successfully added.", log, null, VACC.Logging.Logger.INFO);
                //*/

                r.success = true;
                r.isClientError = false;
                r.errorMessage = string.Empty;
                r.data = "";
                 
            }
            catch(Exception e)
            {
                processLog(e.Message, log, e, VACC.Logging.Logger.INFO);
            }
        }


        /// <summary>
        /// i don't think this is needed? - maybe call the create from here?
        /// </summary>
        /// <param name="id"></param>
        /// <param name="payload"></param>
        private void processUpdateService(string id, Payload payload)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch(NotImplementedException nie)
            {
                processLog(nie.Message, log, nie, VACC.Logging.Logger.INFO);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        private void processDeleteService(Payload payload)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch(NotImplementedException nie)
            {
                processLog(nie.Message, log, nie, VACC.Logging.Logger.INFO);
            }
        }

        #endregion



        #region Authentication


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool AuthenticateUser()
        {
            try
            {
                WebOperationContext ctx = WebOperationContext.Current;
                string authHeader = ctx.IncomingRequest.Headers[HttpRequestHeader.Authorization];
                string basicAuth = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(authHeader.Replace("Basic ", "")));

                        processLog("Authentic!", log, null, VACC.Logging.Logger.INFO);
						
                string[] authParts = basicAuth.Split(':');
                if(authParts.Length == 2)
                {
                    string userName = authParts[0];
                    string password = authParts[1];

                    if(sec::User.isAuthentic(userName, password) && sec::User.isMemberOfGroup(userName, "CRMSyncWebService"))
                    {
                        syncUserId = User.getByLogonName(userName).ID;
                        processLog("Authentic!", log, null, VACC.Logging.Logger.INFO);
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch(Exception ex)
            {
                processLog(ex.Message + "\n" + ex.StackTrace, log, null, VACC.Logging.Logger.INFO);
                processLog(ex.Message, log, ex, VACC.Logging.Logger.CRIT);
                return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Result UnauthorisedResult()
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
            WebOperationContext.Current.OutgoingRequest.Headers.Add("WWW-Authenticate", "Basic realm=\"CRMSyncWebService\"");
            return new Result { success = false, errorMessage = "Invalid credentials or no access rights. Please contact administrator.", isClientError = true };
        }

        #endregion



        #region Helpers


        /// <summary>
        /// 
        /// </summary>
        /// <param name="membertype"></param>
        /// <returns></returns>
        private Guid getSecurityGroupIdForMemberType(string membertype)
        {
            string securityGroup = string.Empty;
            if(membertype.ToLower().Contains("vacc"))
            {
                securityGroup = "vacc";
            }
            else if(membertype.ToLower().Contains("tacc"))
            {
                securityGroup = "tacc";
            }
            else if(membertype.ToLower().Contains("mtaq"))
            {
                securityGroup = "mtaq";
            }

            // Check if security group received has valid value (vacc or tacc)
            //if (!(securityGroup.ToLower().Equals("vacc") || securityGroup.ToLower().Equals("tacc")))
            if(securityGroup == string.Empty)
            {
                throw new DataMismatchException("Wrong value received for security group. Business has to be a member of VACC or TACC or MTAQ.");
            }

            // Check if security group exists
            DataSet secG = Group.getByName(securityGroup);
            if(secG == null)
            {
                throw new DataMismatchException("Security Group (" + securityGroup + ") does not exist in the Security DB.");
            }

            return new Guid(secG.Tables[0].Rows[0]["ID"].ToString());

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        private void setHttpStatusCodeForResult(Result result)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
            processLog("Response: " + r.print(), log, null, VACC.Logging.Logger.INFO);

            /*
            if (result.success)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = result.isClientError ? HttpStatusCode.BadRequest : HttpStatusCode.InternalServerError;
            }
            */
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="name"></param>
        /// <param name="required"></param>
        /// <returns></returns>
        internal static string getPayloadFieldValueByName(Payload payload, string name, bool required)
        {
            string value = string.Empty;

            var v = from c in payload.xml.Descendants("field")
                    where c.Attribute("name").Value.ToLower().Equals(name)
                    select c.Attribute("value").Value;

            if(v.Count() == 1)
            {
                value = v.First<string>();
            }
            else
            {
                if(required)
                {
                    throw new InvalidWebServiceCallException(@"Payload does not have required field (" + name + ") or it is in there more than once.");
                }
            }

            return value;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logToUse"></param>
        /// <param name="ex"></param>
        /// <param name="logLevel"></param>
        private void processLog(string message, FileLogger logToUse, Exception ex, int logLevel)
        {
            // Send Email if CRIT or WARN log (error)
            //if (logLevel == VACC.Logging.Logger.INFO || logLevel == VACC.Logging.Logger.CRIT)
            if(logLevel == VACC.Logging.Logger.CRIT)
            {
                QueuedMail email = new QueuedMail();
                email.id = Guid.NewGuid();
                email.sender = "VACC CRM Sync Service <noreply@vacc.com.au>";
                email.recipient = WebConfigurationManager.AppSettings["ErrorEmailTo"];
                email.cc = WebConfigurationManager.AppSettings["ErrorEmailCC"];
                email.subject = "Error from Coresoft (Web)Service";
                email.body = System.Security.SecurityElement.Escape(message);
                if(ex != null)
                    email.body += System.Security.SecurityElement.Escape(Environment.NewLine + Environment.NewLine + ex.StackTrace);
                email.status = "";
                email.bcc = "";
                email.zUserID = syncUserId;
                email.save();
            }

            logToUse.logMessage(message + (ex != null ? "\nstack: " + ex.StackTrace : string.Empty), logLevel, true);

            if(logLevel != VACC.Logging.Logger.INFO)
            {
                r.success = false;
                r.isClientError = true;
                r.errorMessage = message;
            }

        }

        #endregion
    }

}
