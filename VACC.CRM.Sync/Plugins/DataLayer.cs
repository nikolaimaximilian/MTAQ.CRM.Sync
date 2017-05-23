using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace VACC.CRM.Sync.Plugins
{
    public static class DataLayer
    {
        public static QueryExpression GetSyncItemQueryExpression(Guid id)
        {
            // Get all configurations for that entity.
            ConditionExpression ceEntityName = new ConditionExpression("vacc_syncitemid", ConditionOperator.Equal, id);

            FilterExpression fe = new FilterExpression()
            {
                Conditions = { ceEntityName }
            };

            QueryExpression qe = new QueryExpression()
            {
                EntityName = "SyncItem",
                ColumnSet = new ColumnSet{AllColumns=true}
            };

            qe.Criteria.AddFilter(fe);

            return qe;
        }

    }

}
