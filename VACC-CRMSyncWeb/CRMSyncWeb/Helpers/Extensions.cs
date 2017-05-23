using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMSyncWeb.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// get a field value from the payload and you can specify if it's required or not
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="name"></param>
        /// <param name="required"></param>
        /// <returns></returns>
        public static string val(this Payload payload, string name, bool required)
        {
            return CRMSyncWebService.getPayloadFieldValueByName(payload, name, required);
        }


        /// <summary>
        /// get a field value from the payload - not required
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string val(this Payload payload, string name)
        {
            return CRMSyncWebService.getPayloadFieldValueByName(payload, name, false);
        }


        /// <summary>
        /// get a field value from the payload - not required
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string valReq(this Payload payload, string name)
        {
            return CRMSyncWebService.getPayloadFieldValueByName(payload, name, true);
        }


        /// <summary>
        /// make a string safe for using in a sql string
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string sqlStringSafe(this string txt)
        {
            return txt.Replace("'", "''");
        }
    }
}