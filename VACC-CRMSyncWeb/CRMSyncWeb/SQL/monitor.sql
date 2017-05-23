declare @userID uniqueidentifier;
set @userID = 'DC1EA5C7-0D49-4303-BD66-B82A5C65B755';


select * from [Security]..[User] where zUserID = @userID order by zChange desc;
select * from [common]..[Profile] where zUserID = @userID order by zChange desc;
select * from [Security]..[Group] where zUserID = @userID order by zChange desc;
select * from [Security]..[GroupGroup] where zUserID = @userID order by zChange desc;
select * from [Security]..[UserGroup] where zUserID = @userID order by zChange desc;

/*
declare @userID uniqueidentifier;
set @zu = 'DC1EA5C7-0D49-4303-BD66-B82A5C65B755';

delete from [common]..[Profile] where zUserID = @zu;
delete from [Security]..[GroupGroup] where zUserID = @zu;
delete from [Security]..[Group] where zUserID = @zu;
delete from [Security]..[UserGroup] where zUserID = @zu;
delete from [Security]..[User] where zUserID = @zu;
delete from [tSecurity]..[User] where zUserID = @zu;
*/

/*
DELETE A USER
declare @zu uniqueidentifier;
declare @user uniqueidentifier;
set @zu = 'DC1EA5C7-0D49-4303-BD66-B82A5C65B755';
set @user = 'A1986800-CAB0-E411-940B-005056A72AC5';
delete from [common]..[Profile] where zUserID = @zu and UserID = @user;
delete from [Security]..[UserGroup] where zUserID = @zu and UserID = @user;
delete from [Security]..[User] where zUserID = @zu and ID = @user;
*/

/* user setup 
insert into Security..[User] values ('DC1EA5C7-0D49-4303-BD66-B82A5C65B755', 'crmsynctestuser', 'B0-81-DB-E8-5E-1E-C3-FF-C3-D4-E7-D0-22-74-00-CD',
0, '00000000-0000-0000-0000-000000000000', null, 0, '33A951A1-F27C-4358-B0D7-3E086E2F2C08', 'D4-1D-8C-D9-8F-00-B2-04-E9-80-09-98-EC-F8-42-7E', null, 0, null)

insert into [Security]..[UserGroup] values ('DC1EA5C7-0D49-4303-BD66-B82A5C65B755', 'D2695E4E-8DFE-4EB0-99E7-59435CFDDD7C', 0, null, GETUTCDATE(), 0)
*/