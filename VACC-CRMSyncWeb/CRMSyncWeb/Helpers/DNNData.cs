using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

using VACC.Extensions;
using VACC.Logging;
using VACC.Web;

namespace CRMSyncWeb.Helpers
{
    public class DNNData
    {
        public DNNData(FileLogger logToUse)
            : this()
        {
            logr = logToUse;
        }

        FileLogger logr = null;

        private void log(string message, Exception ex)
        {
            if(logr != null)
            {
                logr.logMessage(message + "\r\nstack: " + ex.StackTrace, Logger.CRIT, true);
            }
        }

        private void log(string message)
        {
            if(logr != null)
            {
                logr.logMessage(message, Logger.INFO, true);
            }
        }

        /*
        protected string htmlTemplate =
            @"
&lt;p&gt;summary&lt;/p&gt;
&lt;div class=&quot;leftStuff&quot;&gt;
	&lt;h2&gt;Services&lt;/h2&gt;
	&lt;div class=&quot;service1&quot;&gt;
		&lt;ul&gt;
			&lt;li&gt;Logbook Servicing&lt;/li&gt;
		&lt;/ul&gt;
	&lt;/div&gt;
	&lt;div class=&quot;contactdetails&quot;&gt;
		&lt;ul&gt;
			&lt;li&gt;Address: 657-659 Deception Bay Rd&lt;br /&gt;      Deception Bay&lt;br /&gt;      QLD 4508&lt;/li&gt;
			&lt;li&gt;Phone: (07) 5498 3969&lt;/li&gt;
			&lt;li&gt;Email: sales@junctiontyres.com.au&lt;/li&gt;
			&lt;li&gt;Website: www.1300autotrans.com.au&lt;/li&gt;
		&lt;/ul&gt;
	&lt;/div&gt;
&lt;/div&gt;
&lt;div class=&quot;rightStuff&quot;&gt;
	&lt;div class=&quot;Openinghours&quot;&gt;
		&lt;h2&gt;Opening Hours&lt;/h2&gt;
		&lt;ul&gt;
			&lt;li class=&quot;oh&quot;&gt;Monday: 7:30am - 5:00pm&lt;/li&gt;
			&lt;li class=&quot;oh&quot;&gt;Tuesday: 7:30am - 5:00pm&lt;/li&gt;
			&lt;li class=&quot;oh&quot;&gt;Wednesday: 7:30am - 5:00pm&lt;/li&gt;
			&lt;li class=&quot;oh&quot;&gt;Thursday: 7:30am - 5:00pm&lt;/li&gt;
			&lt;li class=&quot;oh&quot;&gt;Friday: 7:30am - 5:00pm&lt;/li&gt;
			&lt;li class=&quot;oh&quot;&gt;Saturday: 7:30am - 12:00pm&lt;/li&gt;
			&lt;li class=&quot;oh&quot;&gt;Sunday: Closed&lt;/li&gt;
		&lt;/ul&gt;
	&lt;/div&gt;
&lt;/div&gt;
";
 
        /*/

        protected string addressDetail = "&lt;li&gt;{0}&lt;br /&gt;      {1}&lt;br /&gt;      {2} {3}&lt;/li&gt;";
        protected string contactDetail = "&lt;li&gt;{0}: {1}&lt;/li&gt;";

        protected string htmlTemplate =
            @"
&lt;div class=&quot;leftStuff&quot;&gt;
    &lt;div id=&quot;serviceLocator&quot; /&gt;
	&lt;div class=&quot;contactdetails&quot;&gt;
		&lt;ul&gt;
			{0}
			{1}
		&lt;/ul&gt;
	&lt;/div&gt;
&lt;/div&gt;
&lt;div class=&quot;rightStuff&quot;&gt;
	&lt;div class=&quot;Openinghours&quot;&gt;
		&lt;h2&gt;Opening Hours&lt;/h2&gt;
		&lt;ul&gt;
			&lt;li class=&quot;oh&quot;&gt;Monday: 7:30am - 5:00pm&lt;/li&gt;
			&lt;li class=&quot;oh&quot;&gt;Tuesday: 7:30am - 5:00pm&lt;/li&gt;
			&lt;li class=&quot;oh&quot;&gt;Wednesday: 7:30am - 5:00pm&lt;/li&gt;
			&lt;li class=&quot;oh&quot;&gt;Thursday: 7:30am - 5:00pm&lt;/li&gt;
			&lt;li class=&quot;oh&quot;&gt;Friday: 7:30am - 5:00pm&lt;/li&gt;
			&lt;li class=&quot;oh&quot;&gt;Saturday: 7:30am - 12:00pm&lt;/li&gt;
			&lt;li class=&quot;oh&quot;&gt;Sunday: Closed&lt;/li&gt;
		&lt;/ul&gt;
	&lt;/div&gt;
&lt;/div&gt;
&lt;div id=&quot;{2}&quot; /&gt; 
";

        protected string serviceContainer =
              @"     
	&lt;h2&gt;Services&lt;/h2&gt;
	&lt;div class=&quot;service1&quot;&gt;
		&lt;ul&gt;
			{0}
		&lt;/ul&gt;
	&lt;/div&gt;     
    ";

        protected string serviceLine = @"
			&lt;li id=&quot;{1}&quot;&gt;{0}&lt;/li&gt;";

        //*/



        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="memberNumber"></param>
        /// <param name="tradingName"></param>
        /// <param name="addressline1"></param>
        /// <param name="suburb"></param>
        /// <param name="state"></param>
        /// <param name="postcode"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="website"></param>
        public void insertBusiness(string businessId, string memberNumber, string tradingName, string addressline1, string suburb, string state, string postcode, string phone, string email, string website)
        {
            string addressDetails = string.Format(addressDetail, addressline1, suburb, state, postcode);

            string contactDetails = string.Empty;
            contactDetails += string.Format(contactDetail, "Phone", phone);
            //contactDetails += fax.ifHasValue(string.Format(contactDetail, "Fax", website));  
            contactDetails += string.Format(contactDetail, "Email", email);
            contactDetails += website.ifHasValue(string.Format(contactDetail, "Website", website));

            string article = string.Format(htmlTemplate, addressDetails, contactDetails, memberNumber, string.Empty);               // no services at creation - they'll always get added afterwards - they get synced separately
            Geocode.Result g = Geocode.google(addressline1 + ", " + suburb + ", " + postcode, state)[1];

            doInsert(tradingName, string.Empty, article, g.latitude, g.longitude);
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="memberNumber"></param>
        /// <param name="tradingName"></param>
        /// <param name="addressline1"></param>
        /// <param name="suburb"></param>
        /// <param name="state"></param>
        /// <param name="postcode"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="website"></param>
        public void updateBusiness(string businessId, string memberNumber, string tradingName, string addressline1, string suburb, string state, string postcode, string phone, string email, string website, bool isActive)
        {

            DataSet ds = query(@"SELECT *
                                  FROM [dbo].[EasyDNNNews]
                                  where Article like '%id=&quot;" + memberNumber + "&quot;%'");

            DataRow row = ds.Tables[0].Rows[0];
            int articleID = (int)row[0];
            string title = getBest(tradingName, (string)row[3]);
            string article = (string)row[6];

            log("articleID: " + articleID + " title: " + title + " article: " + article);

            Match addressMatch = addressPattern.Match(article);

            string addressDetails = string.Format(addressDetail,
                                                    getBest(addressline1, addressMatch.Groups[1].Value),
                                                    getBest(suburb, addressMatch.Groups[2].Value),
                                                    getBest(state, addressMatch.Groups[3].Value),
                                                    getBest(postcode, addressMatch.Groups[4].Value));

            article = addressPattern.Replace(article, addressDetails);

            log("article after add: " + article);

            string contactDetails = string.Empty;

            contactDetails += setContactDetail("Phone", phone, article);
            //contactDetails += setContactDetail("Fax", fax, article);
            contactDetails += setContactDetail("Email", email, article);
            contactDetails += setContactDetail("Website", website, article);

            log("contactDetails: " + contactDetails);

            MatchCollection mc = detailPattern.Matches(article);


            for(int i = mc.Count - 1; i >= 0; i--)                                                                     // go backwards through the matches (otherwise the indexes get out of whack)
            {
                log("iter: " + i);

                Match m = mc[i];                                                                                        // grab the match   
                article = article.Remove(m.Index, m.Length);                                                            // cut out the matched string - it's old
            }

            article = article.Substring(0, mc[0].Index) + contactDetails + article.Substring(mc[0].Index);          // replace all the details we've cut out with the new ones we've built up

            log("article after CD: " + article);

            doUpdate(articleID, title, article, isActive);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNumber"></param>
        /// <param name="serviceName"></param>
        public void updateService(string memberNumber, string serviceName, string businessServiceId)
        {
            log("updateService-start");

            DataSet ds = query(@"SELECT *
                                  FROM [dbo].[EasyDNNNews]
                                  where Article like '%id=&quot;" + memberNumber + "&quot;%'");

            DataRow row = ds.Tables[0].Rows[0];
            int articleID = (int)row[0];
            string article = (string)row[6];


            log("updateService-articleID: " + articleID + " article: " + article);

            Match match = serviceSection.Match(article);
            string serviceBlock = (match.Success ? match.Groups[0].Value : serviceContainer);                           // grab the whole service section so we can work on it in isolation

            log("updateService-serviceBlock: " + serviceBlock);

            serviceBlock = ensureServiceIn(serviceBlock, serviceName, businessServiceId);

            log("updateService-after add: " + serviceBlock);


            if(match.Success)                                                               // should always match in future - after this version
            {
                article = article.replaceSubstring(match.Index, match.Length, serviceBlock);
                log("updateService-article after replace");
            }
            /*
            else                                                                            // temporarily needed for businesses published in test - after this version
            {
                string tmp = "class=&quot;leftStuff&quot;&gt;";                                 // temporarily needed for businesses published in test
                int idx = article.IndexOf(tmp) + tmp.Length;                                    // temporarily needed for businesses published in test
                article = article.Insert(idx, serviceBlock);                                    // temporarily needed for businesses published in test
                log("updateService-article after insert @ " + idx);
            }
            //*/

            log("updateService-final article: " + article);

            doUpdate(articleID, (string)row[3], article);
        }


        public void deleteService(string businessServiceId)
        {
            log("deleteService-start");

            DataSet ds = query(@"SELECT *
                                  FROM [dbo].[EasyDNNNews]
                                  where Article like '%id=&quot;" + businessServiceId + "&quot;%'");

            DataRow row = ds.Tables[0].Rows[0];
            int articleID = (int)row[0];
            string article = (string)row[6];

            Regex service = new Regex("&lt;li id=&quot;" + businessServiceId + "&quot;&gt;((?:(?!&lt;/li&gt;).)+)&lt;/li&gt;", RegexOptions.IgnoreCase);

            article = service.Replace(article, string.Empty);

            log("deleteService-final article: " + article);

            doUpdate(articleID, (string)row[3], article);
        }



        Regex serviceSection = new Regex(@"&lt;h2&gt;Services[&lt;/h2g\s]+(?:(?!&lt;/div&gt;)[\S\s])+&lt;/div&gt;|&lt;div id=&quot;serviceLocator&quot; /&gt;", RegexOptions.Singleline | RegexOptions.Compiled);
        Regex servicePattern = new Regex(@"&lt;li id=&quot;(?:(?!&quot;).)+&quot;&gt;((?:(?!&lt;/li&gt;).)+)&lt;/li&gt;", RegexOptions.Compiled);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceBlock"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        protected string ensureServiceIn(string serviceBlock, string serviceName, string businessServiceId)
        {
            bool found = false;

            string services = string.Empty;
            foreach(Match m in servicePattern.Matches(serviceBlock))
            {
                found = found || (m.Groups[1].Value == serviceName);
                services += m.Groups[0].Value;
            }

            if(!found)
            {
                services += string.Format(serviceLine, serviceName, businessServiceId);
            }

            return string.Format(serviceContainer, services);
        }


        Regex addressPattern = new Regex(@"&lt;li&gt;([\s\w]+)&lt;br /&gt;      (\w[\s\w]+)&lt;br /&gt;      (\w+) (\d+)&lt;/li&gt;");
        Regex detailPattern = new Regex(@"&lt;li&gt;(\w+): ((?:(?!&lt;/li&gt;).)+)&lt;/li&gt;");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="article"></param>
        /// <param name="detailName"></param>
        /// <returns></returns>
        protected string extractContactDetailFrom(string article, string detailName)
        {
            string value = string.Empty;

            foreach(Match m in detailPattern.Matches(article))
            {
                if(m.Groups[1].Value == detailName)
                {
                    value = m.Groups[2].Value;
                }
            }

            return value;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="detailName"></param>
        /// <param name="value"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        protected string setContactDetail(string detailName, string value, string article)
        {
            //*
            if(value.Length == 0)
            {
                value = extractContactDetailFrom(article, detailName);
            }

            return (value.Length == 0 ? string.Empty : string.Format(contactDetail, detailName, value));
            /*/
            value = value.ifHasValue(extractFrom(article, detailName));                                                     // think this is equivalent, but I know the other one works

            return value.ifHasValue(string.Format(contactDetail, detailName, value));
            //*/
        }


        protected string getBest(string possibleValue, string d3fault)
        {
            return (string.IsNullOrWhiteSpace(possibleValue) ? d3fault : possibleValue);
        }


        protected string cleanArticle(string article)
        {
            return escapedQuot.Replace(escapedAmp.Replace(escapedTags.Replace(article, string.Empty), "&"), "\"");
        }



        protected string urliseName(string tradingName)
        {
            return tradingName.Replace(" ", "-").Replace("&", string.Empty);
        }

        #region --- DB statements -------------------------

        protected Regex escapedTags = new Regex("&lt;((?!&gt;).)+&gt;", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        protected Regex escapedAmp = new Regex("&amp;amp;", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        protected Regex escapedQuot = new Regex("&quot;", RegexOptions.Compiled | RegexOptions.IgnoreCase);



        /// <summary>
        /// takes the article that has been built - and will create an entry in the the DB.
        /// This method cleans the article and does other things necessary for the DNN DB module
        /// </summary>
        /// <param name="tradingName"></param>
        /// <param name="summary"></param>
        /// <param name="article"></param>
        protected void doInsert(string tradingName, string summary, string article, float latitude, float longitude)
        {
            string urlisedName = urliseName(tradingName).sqlStringSafe();
            string cleanedArticle = cleanArticle(article).sqlStringSafe();
            tradingName = tradingName.sqlStringSafe();

            string insertSql = string.Format(insertArticle, tradingName, summary.sqlStringSafe(), article.sqlStringSafe(), urlisedName, cleanedArticle);

            query(insertSql);

            DataSet ds = query("select ArticleID from [EasyDNNNews] where [title] = '" + tradingName + "'");
            int articleID = int.Parse(ds.Tables[0].Rows[0][0].ToString());

            insertSql = string.Format(insertMap, tradingName, latitude, longitude);

            query(insertSql);

            ds = query("select GoogleMapID from [EasyDNNnewsGoogleMapsData] where [pointData] like '%" + tradingName + "%'");
            int mapID = int.Parse(ds.Tables[0].Rows[0][0].ToString());


            query(@"INSERT INTO [EasyDNNNewsCategories]
                                           ([ArticleID] ,[CategoryID])
                                     VALUES
                                           (" + articleID + " ,2) ");

            query(@"INSERT INTO [EasyDNNNewsSocialSecurity]
                                           ([ArticleID] ,[SecurityKey])
                                     VALUES
                                           (" + articleID + " ,'E')  ");

            query(@"INSERT INTO [EasyDNNNewsArticleGoogleMapConnection]
                                           ([ArticleID] ,[GoogleMapID])
                                     VALUES
                                           (" + articleID + " ," + mapID + ")");


            query(@"INSERT INTO [EasyDNNNewsDNNSearchItems]
                                           ([ModuleID] ,[ArticleID])
                                     VALUES
                                           (470 ," + articleID + ")");

            query(@"INSERT INTO [EasyDNNNewsDNNSearchItems]
                                           ([ModuleID] ,[ArticleID])
                                     VALUES
                                           (471 ," + articleID + ")");

            /*
            query(@"INSERT INTO [EasyDNNNewsDNNSearchItems]
                                           ([ModuleID] ,[ArticleID])
                                     VALUES
                                           (492 ," + articleID + ")");
            //*/
        }


        protected string insertArticle = @"
                INSERT INTO [EasyDNNNews]
                           ([PortalID], [UserID], [Title], [SubTitle], [Summary], [Article], [ArticleImage], [DateAdded], [LastModified], [PublishDate], [ExpireDate], [NumberOfViews], [RatingValue], [RatingCount], [TitleLink], [DetailType], [DetailTypeData], [DetailsTemplate], [DetailsTheme], [GalleryPosition], [GalleryDisplayType], [CommentsTheme], [ArticleImageFolder], [NumberOfComments], [MetaDecription], [MetaKeywords], [DisplayStyle], [DetailTarget], [CleanArticleData], [ArticleFromRSS], [HasPermissions], [EventArticle], [DetailMediaType], [DetailMediaData], [AuthorAliasName], [ShowGallery], [ArticleGalleryID], [MainImageTitle], [MainImageDescription], [HideDefaultLocale], [Featured], [Approved], [AllowComments], [Active], [ShowMainImage], [ShowMainImageFront], [ArticleImageSet], [CFGroupeID], [DetailsDocumentsTemplate], [DetailsLinksTemplate], [DetailsRelatedArticlesTemplate])
                     VALUES
                           (0,1
                           ,'{0}'
                           ,''
                           ,'{1}'
                           ,'{2}'
                           ,''                       
                           ,DATEADD(day,-2,getUTCdate())
                           ,getUTCdate()
                           ,DATEADD(day,-1,getUTCdate())
                           ,'3016-02-09 03:48:00.000'
                           ,0 ,0.0000 ,0
                           ,'{3}'
                           ,'Text'
                           ,''
                           ,'DEFAULT'
                           ,'DEFAULT'
                           ,'bottom'
                           ,'chameleon'
                           ,'DEFAULT'
                           ,'' ,0 ,'' ,''
                           ,'DEFAULT'
                           ,'_self'
                           ,'{4}'
                           ,0 ,0 ,0 ,'' ,'' ,''
                           ,0 ,null
                           ,'' ,''
                           ,0 ,0 ,1 ,1 ,1
                           ,0 ,0 ,0
                           ,null ,null ,null ,null)";



        protected string insertMap = @"
                INSERT INTO [dbo].[EasyDNNnewsGoogleMapsData]
                           ([PortalID], [UserID], [Global], [DateAdded], [Latitude], [Longitude], [MapType], [MapZoom], [Scrollwheel], [MapWidth], [MapHeight], [PointData], [FullToken], [Position])
                     VALUES
                           (0, 1, 0
                           ,DATEADD(day,-1,getUTCdate())
                           ,{1} ,{2}
                           ,'ROADMAP', 17, 'true', 400, 300
                           ,'({1}:{2}*{0}*)'
                           ,'[GoogleMap|{1}:{2}|type:ROADMAP|zoom:17|scrollwheel:true|width:400|height:300|({1}:{2}*{0}*)]' , null)";



        /// <summary>
        /// 
        /// </summary>
        /// <param name="tradingName"></param>
        /// <param name="article"></param>
        /// <param name="isActive"></param>
        protected void doUpdate(int articleID, string tradingName, string article, bool? isActive = null)
        {
            string urlisedName = urliseName(tradingName).sqlStringSafe();
            string cleanedArticle = cleanArticle(article).sqlStringSafe();

            string active = string.Empty;
            if(isActive != null)
            {
                active = string.Format(",[Active] = {0}", ((bool)isActive ? "1" : "0"));
            }

            string updateSql = string.Format(updateArticle, tradingName.sqlStringSafe(), article.sqlStringSafe(), urlisedName, cleanedArticle, active, articleID);

            query(updateSql);
        }


        protected string updateArticle = @"
                UPDATE [dbo].[EasyDNNNews]
                   SET 
                       [Title] = '{0}'
                      ,[Summary] = ''
                      ,[Article] = '{1}'
                      ,[LastModified] = DATEADD(day,-1,getUTCdate())
                      ,[TitleLink] = '{2}'
                      ,[CleanArticleData] = '{3}'
                      {4}
                 WHERE articleID = {5}";



        #endregion



        #region --- DB layer stuff ----------------------

        private System.Data.SqlClient.SqlConnection sqlcon;
        private System.Data.SqlClient.SqlDataAdapter da;
        private System.Data.SqlClient.SqlCommand cmdQuery;

        /// <summary>
        /// 
        /// </summary>
        public DNNData()
        {
            this.sqlcon = new System.Data.SqlClient.SqlConnection();
            this.da = new System.Data.SqlClient.SqlDataAdapter();
            this.cmdQuery = new System.Data.SqlClient.SqlCommand();
            this.cmdQuery.CommandType = System.Data.CommandType.Text;
            this.cmdQuery.Connection = this.sqlcon;
        }

        public static string connectionString { get; set; }

        /// <summary>
        /// default data query method
        /// </summary>
        public DataSet query(string queryString)
        {
            da.SelectCommand = cmdQuery;
            cmdQuery.Connection.ConnectionString = connectionString;


            cmdQuery.CommandText = queryString;
            cmdQuery.Connection.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);


            cmdQuery.Connection.Close();
            return ds;
        }

        #endregion
    }
}