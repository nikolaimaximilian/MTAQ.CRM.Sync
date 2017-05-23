using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Linq;

namespace CRMSyncWeb
{
    [ServiceContract]
    public interface ICRMSyncService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "user/{id}", Method = "GET", RequestFormat = WebMessageFormat.Xml)]
        Result getUser(string id);

        [OperationContract]
        [WebInvoke(UriTemplate = "user", Method = "POST", RequestFormat = WebMessageFormat.Xml)]
        Result createUser(Payload payload);

        [OperationContract]
        [WebInvoke(UriTemplate = "user/{id}", Method = "PUT", RequestFormat = WebMessageFormat.Xml)]
        Result updateUser(string id, Payload payload);

        [OperationContract]
        [WebInvoke(UriTemplate = "user/{id}", Method = "DELETE", RequestFormat = WebMessageFormat.Xml)]
        Result deleteUser(string id);

        [OperationContract]
        [WebInvoke(UriTemplate = "business/{id}", Method = "GET", RequestFormat = WebMessageFormat.Xml)]
        Result getBusiness(string id);

        [OperationContract]
        [WebInvoke(UriTemplate = "business", Method = "POST", RequestFormat = WebMessageFormat.Xml)]
        Result creatBusiness(Payload payload);
        
        [OperationContract]
        [WebInvoke(UriTemplate = "business/{id}", Method = "PUT", RequestFormat = WebMessageFormat.Xml)]
        Result updateBusiness(string id, Payload payload);

        [OperationContract]
        [WebInvoke(UriTemplate = "business/{id}", Method = "DELETE", RequestFormat = WebMessageFormat.Xml)]
        Result deleteBusiness(string id);

        [OperationContract]
        [WebInvoke(UriTemplate = "service/{id}", Method = "GET", RequestFormat = WebMessageFormat.Xml)]
        Result getService(string id);

        [OperationContract]
        [WebInvoke(UriTemplate = "service", Method = "POST", RequestFormat = WebMessageFormat.Xml)]
        Result createService(Payload payload);

        [OperationContract]
        [WebInvoke(UriTemplate = "service/{id}", Method = "PUT", RequestFormat = WebMessageFormat.Xml)]
        Result updateService(string id, Payload payload);

        [OperationContract]
        [WebInvoke(UriTemplate = "service", Method = "DELETE", RequestFormat = WebMessageFormat.Xml)]
        Result deleteService(Payload payload);
    }


    /// <summary>
    /// Describes the result of the method call
    /// </summary>
    [DataContract]
    public class Result
    {
        /// <summary>
        /// Indicates the overall result of the method call
        /// </summary>
        [DataMember(IsRequired = true, Order = 10)]
        public bool success;

        /// <summary>
        /// Indicates if the error was caused by the client
        /// </summary>
        [DataMember(IsRequired = true, Order = 20)]
        public bool isClientError;

        /// <summary>
        /// Data data that needs to be passed back to the caller
        /// </summary>
        [DataMember(Order = 40)]
        public string data;

        /// <summary>
        /// Description of the error, if any
        /// </summary>
        [DataMember(Order = 30)]
        public string errorMessage;

        public string print()
        {
            return @"{ success: " + this.success + ", isClientError: " + this.isClientError + ", errorMessage: " + this.errorMessage + ", data: " + this.data + " }";
        }
    }

    [DataContract]
    public class Payload
    {
        [DataMember(IsRequired=true, Order=10)]
        public Guid id { get; set; }

        [DataMember(IsRequired = true, Order = 20)]
        public string xmlString
        {
            get 
            {
                if (xml != null)
                    return xml.ToString();
                else
                    return "";
            }
            set
            {
                this.xml = XDocument.Parse(value);
            }
        }

        public XDocument xml { get; set; }
    }

}
