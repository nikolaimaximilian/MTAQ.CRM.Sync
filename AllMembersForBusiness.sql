select
	
	g.ID as 'Business Id',
	g.Name as 'Business No',
	g.[Description] as 'Business Name',
	
	(select AddressLine1 from [Mapping]..[Address] where AddressID = (select Value from [Common]..[Profile] where [Key] = 'Address-Location' and UserID = g.ID)) as 'Business Address',
	(select SuburbOrCity from [Mapping]..[Address] where AddressID = (select Value from [Common]..[Profile] where [Key] = 'Address-Location' and UserID = g.ID)) as 'Business SuburbOrCity',
	(select StateOrRegion from [Mapping]..[Address] where AddressID = (select Value from [Common]..[Profile] where [Key] = 'Address-Location' and UserID = g.ID)) as 'Business StateOrRegion',
	(select PostalCode from [Mapping]..[Address] where AddressID = (select Value from [Common]..[Profile] where [Key] = 'Address-Location' and UserID = g.ID)) as 'Business PostalCode',
	(select Country from [Mapping]..[Address] where AddressID = (select Value from [Common]..[Profile] where [Key] = 'Address-Location' and UserID = g.ID)) as 'Business Country',
	
	u.ID as 'User Id',
	u.LogonName as 'User Logonname',
	(select Value from [Common]..[Profile] where [Key] = 'First_Name' and UserID = u.ID) as 'User Firstname',
	(select Value from [Common]..[Profile] where [Key] = 'Last_Name' and UserID = u.ID) as 'User Lastname',
	(select Value from [Common]..[Profile] where [Key] = 'Primary_Email' and UserID = u.ID) as 'User Email'
	
	from [Security]..[Group] g
	left join [Security]..[UserGroup] ug on g.ID = ug.GroupID
	left join [Security]..[User] u on ug.UserID = u.ID