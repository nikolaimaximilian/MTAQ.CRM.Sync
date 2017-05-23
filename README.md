VACC.CRM.Sync
=============

##CRM Custom Entities Required For This Solution
</br>
####SyncLog (vacc_SyncLog)
#####Holds data about an event that occured in our processing of entities and fields
----------------------------------------------------------------------------------------------------------------
|Field Name| Type| Description|
|------------- |-------------|-----|
|vacc_syncLogId				|	Primary Key				|	Unique Id for entity instance|
|vacc_LogType				|	Single Line of Text		|	Type of Log (Error, Critial, Info, etc)|
|vacc_LogMessage			|	Single Line of Text		|	Log Message|

####SyncItem (vacc_SyncItem)
#####Data that is passed to the external source
--------------------------------------------------------------------------
|Field Name| Type| Description|
|------------- |-------------|-----|
|vacc_syncItemId			|	Primary Key				|	Unique Id for entity instance
|vacc_ItemReferenceId		|	Single Line of Text		|	Id of the entity instance (ID for the row item) 
|vacc_XmlData				|	Multiple Lines of Text	|	Holds the xml that defined what external source is being called and data being passed

####SyncConfiguration (vacc_SyncConfiguration)	
#####Holds information relating to what Entities and fields are to be sync'ed to an external source
-------------------------------------------------------------------------
|Field Name| Type| Description|
|------------- |-------------|-----|
|vacc_SyncConfigurationId	|	Primary Key				|	Unique Id for entity instance
|vacc_EntityName			|	Single Line of Text		|	Name of Entity this config row is processed against
|vacc_XmlData 				|	Multiple Lines of Text	|	Holds XML that defined what Entities and Fields are to be sync'ed with external Sources 
|vacc_Active				|	Two Options				|	Yes/No
|vacc_WebserviceUrl			|	Single Line of Text		|	URL to Call
|vacc_WebserviceUser		|	Single Line of Text		|	Username for authentication need by URL
|vacc_WebservicePwd			|	Single Line of Text		|	Password for authentication need by URL

####SyncObjectMapping (vacc_SyncObjectMapping)	
#####Holds the relationship bewteen a CRM item and our application data (Security/Address/Mapping/etc)
#####This entity only holds the matching ids if they are different, for items created only within the CRM no entry will exist in this entity since we will create the data for the external source using the same Id
--------------------------------------------------------------------------
|Field Name| Type| Description|
|------------- |-------------|-----|
|vacc_SyncObjectMappingId	|	Primary Key				|	Unique Id for entity instance
|vacc_ItemReferenceId		|	Single Line of Text		|	Id of the entity instance (ID for the row item)
|vacc_ExternalObjectId		|	Single Line of Text		|	Id of the external instance 

