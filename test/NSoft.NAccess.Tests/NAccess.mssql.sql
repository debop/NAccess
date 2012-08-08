
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKD2C41674478FF1D8]') AND parent_object_id = OBJECT_ID('[Calendar]'))
alter table NAccessTest.dbo.[Calendar]  drop constraint FKD2C41674478FF1D8
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK9B2E17B3A9659A9D]') AND parent_object_id = OBJECT_ID('CalendarLoc'))
alter table NAccessTest.dbo.CalendarLoc  drop constraint FK9B2E17B3A9659A9D
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_CAL_META_CAL]') AND parent_object_id = OBJECT_ID('CALENDAR_META'))
alter table NAccessTest.dbo.CALENDAR_META  drop constraint FK_CAL_META_CAL
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK13480398A9659A9D]') AND parent_object_id = OBJECT_ID('[CalendarRule]'))
alter table NAccessTest.dbo.[CalendarRule]  drop constraint FK13480398A9659A9D
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_CAL_RULE_LOC_CAL_RULE]') AND parent_object_id = OBJECT_ID('CALENDAR_RULE_LOC'))
alter table NAccessTest.dbo.CALENDAR_RULE_LOC  drop constraint FK_CAL_RULE_LOC_CAL_RULE
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK65DB3EDD4F858EE2]') AND parent_object_id = OBJECT_ID('CALENDAR_RULE_META'))
alter table NAccessTest.dbo.CALENDAR_RULE_META  drop constraint FK65DB3EDD4F858EE2
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK1D6732E4C25799BD]') AND parent_object_id = OBJECT_ID('[CalendarRuleOfUser]'))
alter table NAccessTest.dbo.[CalendarRuleOfUser]  drop constraint FK1D6732E4C25799BD
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKF9A54638C9F2A1EB]') AND parent_object_id = OBJECT_ID('CodeLoc'))
alter table NAccessTest.dbo.CodeLoc  drop constraint FKF9A54638C9F2A1EB
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKC151678438749D06]') AND parent_object_id = OBJECT_ID('CompanyLoc'))
alter table NAccessTest.dbo.CompanyLoc  drop constraint FKC151678438749D06
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKB46B9FEB38749D06]') AND parent_object_id = OBJECT_ID('CompanyMeta'))
alter table NAccessTest.dbo.CompanyMeta  drop constraint FKB46B9FEB38749D06
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK6BAC779638749D06]') AND parent_object_id = OBJECT_ID('[Department]'))
alter table NAccessTest.dbo.[Department]  drop constraint FK6BAC779638749D06
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK6BAC7796A286EC9C]') AND parent_object_id = OBJECT_ID('[Department]'))
alter table NAccessTest.dbo.[Department]  drop constraint FK6BAC7796A286EC9C
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK6BAC7796CB816B7B]') AND parent_object_id = OBJECT_ID('[Department]'))
alter table NAccessTest.dbo.[Department]  drop constraint FK6BAC7796CB816B7B
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKD0801CE3CB816B7B]') AND parent_object_id = OBJECT_ID('DepartmentLoc'))
alter table NAccessTest.dbo.DepartmentLoc  drop constraint FKD0801CE3CB816B7B
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK2E689169CB816B7B]') AND parent_object_id = OBJECT_ID('DepartmentMeta'))
alter table NAccessTest.dbo.DepartmentMeta  drop constraint FK2E689169CB816B7B
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKFE7B00CCB816B7B]') AND parent_object_id = OBJECT_ID('[DepartmentMember]'))
alter table NAccessTest.dbo.[DepartmentMember]  drop constraint FKFE7B00CCB816B7B
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKFE7B00C52E9DA29]') AND parent_object_id = OBJECT_ID('[DepartmentMember]'))
alter table NAccessTest.dbo.[DepartmentMember]  drop constraint FKFE7B00C52E9DA29
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKFE7B00C62D67FD2]') AND parent_object_id = OBJECT_ID('[DepartmentMember]'))
alter table NAccessTest.dbo.[DepartmentMember]  drop constraint FKFE7B00C62D67FD2
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK3D30BB9838749D06]') AND parent_object_id = OBJECT_ID('[EmployeeCodeBase]'))
alter table NAccessTest.dbo.[EmployeeCodeBase]  drop constraint FK3D30BB9838749D06
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK2AB871F6AA6728FD]') AND parent_object_id = OBJECT_ID('EmployeeCodeLoc'))
alter table NAccessTest.dbo.EmployeeCodeLoc  drop constraint FK2AB871F6AA6728FD
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK611DA7F8B60B2B49]') AND parent_object_id = OBJECT_ID('[File]'))
alter table NAccessTest.dbo.[File]  drop constraint FK611DA7F8B60B2B49
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKBA21C18E38749D06]') AND parent_object_id = OBJECT_ID('[Group]'))
alter table NAccessTest.dbo.[Group]  drop constraint FKBA21C18E38749D06
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK2D21A51BEFDEDD0C]') AND parent_object_id = OBJECT_ID('GroupLoc'))
alter table NAccessTest.dbo.GroupLoc  drop constraint FK2D21A51BEFDEDD0C
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK43454B7CEFDEDD0C]') AND parent_object_id = OBJECT_ID('GroupMeta'))
alter table NAccessTest.dbo.GroupMeta  drop constraint FK43454B7CEFDEDD0C
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_MC_ITEM_MC]') AND parent_object_id = OBJECT_ID('[MasterCodeItem]'))
alter table NAccessTest.dbo.[MasterCodeItem]  drop constraint FK_MC_ITEM_MC
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK9DB87EF07826474F]') AND parent_object_id = OBJECT_ID('[MasterCodeItem]'))
alter table NAccessTest.dbo.[MasterCodeItem]  drop constraint FK9DB87EF07826474F
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_MC_ITEM_LOC_MC_ITEM]') AND parent_object_id = OBJECT_ID('MASTER_CODE_ITEM_LOC'))
alter table NAccessTest.dbo.MASTER_CODE_ITEM_LOC  drop constraint FK_MC_ITEM_LOC_MC_ITEM
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKA76AF223715A1EFD]') AND parent_object_id = OBJECT_ID('[MasterCode]'))
alter table NAccessTest.dbo.[MasterCode]  drop constraint FKA76AF223715A1EFD
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKF512DCC37826474F]') AND parent_object_id = OBJECT_ID('MasterCodeLoc'))
alter table NAccessTest.dbo.MasterCodeLoc  drop constraint FKF512DCC37826474F
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK5CB0913794F3DEB9]') AND parent_object_id = OBJECT_ID('[Menu]'))
alter table NAccessTest.dbo.[Menu]  drop constraint FK5CB0913794F3DEB9
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK5CB0913750F6ED7A]') AND parent_object_id = OBJECT_ID('[Menu]'))
alter table NAccessTest.dbo.[Menu]  drop constraint FK5CB0913750F6ED7A
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK5CB09137197A1041]') AND parent_object_id = OBJECT_ID('[Menu]'))
alter table NAccessTest.dbo.[Menu]  drop constraint FK5CB09137197A1041
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKFE474531715A1EFD]') AND parent_object_id = OBJECT_ID('[MenuTemplate]'))
alter table NAccessTest.dbo.[MenuTemplate]  drop constraint FKFE474531715A1EFD
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKFE47453139AE675]') AND parent_object_id = OBJECT_ID('[MenuTemplate]'))
alter table NAccessTest.dbo.[MenuTemplate]  drop constraint FKFE47453139AE675
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKFE47453194F3DEB9]') AND parent_object_id = OBJECT_ID('[MenuTemplate]'))
alter table NAccessTest.dbo.[MenuTemplate]  drop constraint FKFE47453194F3DEB9
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK2829BA3894F3DEB9]') AND parent_object_id = OBJECT_ID('MenuTemplateLoc'))
alter table NAccessTest.dbo.MenuTemplateLoc  drop constraint FK2829BA3894F3DEB9
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK8329404C94F3DEB9]') AND parent_object_id = OBJECT_ID('MenuTemplateMeta'))
alter table NAccessTest.dbo.MenuTemplateMeta  drop constraint FK8329404C94F3DEB9
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKE90E6F5B715A1EFD]') AND parent_object_id = OBJECT_ID('ProductLoc'))
alter table NAccessTest.dbo.ProductLoc  drop constraint FKE90E6F5B715A1EFD
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK8C87B431715A1EFD]') AND parent_object_id = OBJECT_ID('ProductMeta'))
alter table NAccessTest.dbo.ProductMeta  drop constraint FK8C87B431715A1EFD
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK8EE6DC2F6264A118]') AND parent_object_id = OBJECT_ID('ResouceLoc'))
alter table NAccessTest.dbo.ResouceLoc  drop constraint FK8EE6DC2F6264A118
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKE3A3C0F06264A118]') AND parent_object_id = OBJECT_ID('ResourceMeta'))
alter table NAccessTest.dbo.ResourceMeta  drop constraint FKE3A3C0F06264A118
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK7185C17C38749D06]') AND parent_object_id = OBJECT_ID('[User]'))
alter table NAccessTest.dbo.[User]  drop constraint FK7185C17C38749D06
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK7185C17C14885ED8]') AND parent_object_id = OBJECT_ID('[User]'))
alter table NAccessTest.dbo.[User]  drop constraint FK7185C17C14885ED8
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK7185C17C353A2411]') AND parent_object_id = OBJECT_ID('[User]'))
alter table NAccessTest.dbo.[User]  drop constraint FK7185C17C353A2411
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK1336DB8B52E9DA29]') AND parent_object_id = OBJECT_ID('UserLoc'))
alter table NAccessTest.dbo.UserLoc  drop constraint FK1336DB8B52E9DA29
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK6C55B0A152E9DA29]') AND parent_object_id = OBJECT_ID('UserMeta'))
alter table NAccessTest.dbo.UserMeta  drop constraint FK6C55B0A152E9DA29
;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[Calendar]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[Calendar];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.CalendarLoc') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.CalendarLoc;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.CALENDAR_META') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.CALENDAR_META;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[CalendarRule]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[CalendarRule];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.CALENDAR_RULE_LOC') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.CALENDAR_RULE_LOC;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.CALENDAR_RULE_META') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.CALENDAR_RULE_META;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[CalendarRuleOfUser]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[CalendarRuleOfUser];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[Code]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[Code];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.CodeLoc') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.CodeLoc;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[Company]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[Company];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.CompanyLoc') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.CompanyLoc;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.CompanyMeta') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.CompanyMeta;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[Department]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[Department];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.DepartmentLoc') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.DepartmentLoc;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.DepartmentMeta') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.DepartmentMeta;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[DepartmentMember]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[DepartmentMember];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[EmployeeCodeBase]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[EmployeeCodeBase];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.EmployeeCodeLoc') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.EmployeeCodeLoc;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[Favorite]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[Favorite];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[File]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[File];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[FileMapping]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[FileMapping];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[GroupActor]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[GroupActor];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[Group]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[Group];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.GroupLoc') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.GroupLoc;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.GroupMeta') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.GroupMeta;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[MasterCodeItem]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[MasterCodeItem];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.MASTER_CODE_ITEM_LOC') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.MASTER_CODE_ITEM_LOC;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[MasterCode]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[MasterCode];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.MasterCodeLoc') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.MasterCodeLoc;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[Menu]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[Menu];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[MenuTemplate]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[MenuTemplate];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.MenuTemplateLoc') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.MenuTemplateLoc;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.MenuTemplateMeta') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.MenuTemplateMeta;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[Product]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[Product];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.ProductLoc') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.ProductLoc;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.ProductMeta') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.ProductMeta;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[ResourceActor]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[ResourceActor];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[Resource]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[Resource];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.ResouceLoc') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.ResouceLoc;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.ResourceMeta') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.ResourceMeta;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[UserConfig]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[UserConfig];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[UserLoginLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[UserLoginLog];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[User]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[User];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.UserLoc') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.UserLoc;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.UserMeta') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.UserMeta;

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[WorkTimeByDay]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[WorkTimeByDay];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[WorkTimeByHour]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[WorkTimeByHour];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[WorkTimeByMinute]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[WorkTimeByMinute];

    if exists (select * from dbo.sysobjects where id = object_id(N'NAccessTest.dbo.[WorkTimeByRange]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NAccessTest.dbo.[WorkTimeByRange];

    create table NAccessTest.dbo.[Calendar] (
        CalendarId BIGINT IDENTITY NOT NULL,
       CalendarCode VARCHAR(128) not null,
       CalendarName NVARCHAR(255) not null,
       ProjectId VARCHAR(50) null,
       ResourceId VARCHAR(50) null,
       TimeZone VARCHAR(50) null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       ParentId BIGINT null,
       TreeOrder INT null,
       TreeLevel INT null,
       primary key (CalendarId)
    );

    create table NAccessTest.dbo.CalendarLoc (
        CalendarId BIGINT not null,
       CalendarName NVARCHAR(255) not null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       LocaleKey NVARCHAR(128) not null,
       primary key (CalendarId, LocaleKey)
    );

    create table NAccessTest.dbo.CALENDAR_META (
        CAL_ID BIGINT not null,
       MetaValue NVARCHAR(MAX) null,
       MetaValueType NVARCHAR(255) null,
       MetaKey NVARCHAR(255) not null,
       primary key (CAL_ID, MetaKey)
    );

    create table NAccessTest.dbo.[CalendarRule] (
        CalendarRuleId BIGINT IDENTITY NOT NULL,
       CAL_RULE_NAME NVARCHAR(255) null,
       DayOrException INT null,
       ExceptionType INT null,
       ExceptionPattern VARCHAR(1024) null,
       ExceptionClassName VARCHAR(1024) null,
       IsWorking INT not null,
       FromTime DATETIME null,
       ToTime DATETIME null,
       FromTime1 DATETIME null,
       ToTime1 DATETIME null,
       FromTime2 DATETIME null,
       ToTime2 DATETIME null,
       FromTime3 DATETIME null,
       ToTime3 DATETIME null,
       FromTime4 DATETIME null,
       ToTime4 DATETIME null,
       FromTime5 DATETIME null,
       ToTime5 DATETIME null,
       ViewOrder INT null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       CalendarId BIGINT not null,
       primary key (CalendarRuleId)
    );

    create table NAccessTest.dbo.CALENDAR_RULE_LOC (
        CAL_RULE_ID BIGINT not null,
       CalendarRuleLocaleName NVARCHAR(255) not null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       LocaleKey NVARCHAR(128) not null,
       primary key (CAL_RULE_ID, LocaleKey)
    );

    create table NAccessTest.dbo.CALENDAR_RULE_META (
        CAL_RULE_ID BIGINT not null,
       MetaValue NVARCHAR(MAX) null,
       MetaValueType NVARCHAR(1024) null,
       MetaKey NVARCHAR(255) not null,
       primary key (CAL_RULE_ID, MetaKey)
    );

    create table NAccessTest.dbo.[CalendarRuleOfUser] (
        CalendarRuleOfUserId BIGINT IDENTITY NOT NULL,
       CompanyCode NVARCHAR(50) not null,
       UserCode NVARCHAR(50) not null,
       DayOrException INT default 0  null,
       ExceptionType INT null,
       ExceptionPattern VARCHAR(1024) null,
       ExceptionClassName VARCHAR(1024) null,
       IS_WORKING INT not null,
       FromTime DATETIME null,
       ToTime DATETIME null,
       FromTime1 DATETIME null,
       ToTime1 DATETIME null,
       FromTime2 DATETIME null,
       ToTime2 DATETIME null,
       FromTime3 DATETIME null,
       ToTime3 DATETIME null,
       FromTime4 DATETIME null,
       ToTime4 DATETIME null,
       FromTime5 DATETIME null,
       ToTime5 DATETIME null,
       IsActive BIT null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       CalendarRuleId BIGINT not null,
       primary key (CalendarRuleOfUserId)
    );

    create table NAccessTest.dbo.[Code] (
        CodeId BIGINT IDENTITY NOT NULL,
       ItemCode VARCHAR(128) not null,
       IsActive BIT null,
       IsSysDefined BIT null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       GroupCompanyCode VARCHAR(128) not null,
       GroupCode VARCHAR(128) not null,
       GroupName VARCHAR(128) not null,
       primary key (CodeId)
    );

    create table NAccessTest.dbo.CodeLoc (
        CodeId BIGINT not null,
       GroupName NVARCHAR(255) not null,
       ItemName NVARCHAR(255) not null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       LocaleKey NVARCHAR(128) not null,
       primary key (CodeId, LocaleKey)
    );

    create table NAccessTest.dbo.[Company] (
        CompanyId INT IDENTITY NOT NULL,
       CompanyCode VARCHAR(128) not null,
       CompanyName NVARCHAR(255) not null,
       IsActive BIT null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       primary key (CompanyId)
    );

    create table NAccessTest.dbo.CompanyLoc (
        CompanyId INT not null,
       CompanyName NVARCHAR(255) not null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       LocaleKey NVARCHAR(128) not null,
       primary key (CompanyId, LocaleKey)
    );

    create table NAccessTest.dbo.CompanyMeta (
        CompanyId INT not null,
       MetaValue NVARCHAR(MAX) null,
       MetaValueType NVARCHAR(1024) null,
       MetaKey NVARCHAR(255) not null,
       primary key (CompanyId, MetaKey)
    );

    create table NAccessTest.dbo.[Department] (
        DepartmentId INT IDENTITY NOT NULL,
       DepartmentCode VARCHAR(128) not null,
       DepartmentName NVARCHAR(255) not null,
       EName NVARCHAR(255) null,
       Kind VARCHAR(128) null,
       IsActive BIT null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       CompanyId INT not null,
       ParentId INT null,
       TreeOrder INT null,
       TreeLevel INT null,
       primary key (DepartmentId)
    );

    create table NAccessTest.dbo.DepartmentLoc (
        DepartmentId INT not null,
       DepartmentName NVARCHAR(255) null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       LocaleKey NVARCHAR(128) not null,
       primary key (DepartmentId, LocaleKey)
    );

    create table NAccessTest.dbo.DepartmentMeta (
        DepartmentId INT not null,
       MetaValue NVARCHAR(MAX) null,
       MetaValueType NVARCHAR(1024) null,
       MetaKey NVARCHAR(255) not null,
       primary key (DepartmentId, MetaKey)
    );

    create table NAccessTest.dbo.[DepartmentMember] (
        DepartmentMemberId INT IDENTITY NOT NULL,
       IsLeader BIT null,
       IsActive BIT null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       DepartmentId INT not null,
       UserId INT not null,
       EmployeeTitleId UNIQUEIDENTIFIER null,
       primary key (DepartmentMemberId)
    );

    create table NAccessTest.dbo.[EmployeeCodeBase] (
        EmployeeCodeBaseId UNIQUEIDENTIFIER not null,
       CodeKind NVARCHAR(32) not null,
       EmployeeCodeBaseCode VARCHAR(128) not null,
       EmployeeCodeBaseName NVARCHAR(128) not null,
       EName NVARCHAR(128) null,
       ViewOrder INT null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       CompanyId INT not null,
       ParentCode VARCHAR(128) null,
       HourlyWages DECIMAL(19,5) null,
       primary key (EmployeeCodeBaseId)
    );

    create table NAccessTest.dbo.EmployeeCodeLoc (
        EmployeeCodeBaseId UNIQUEIDENTIFIER not null,
       EmployeeCodeLocaleName NVARCHAR(128) null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       LocaleKey NVARCHAR(128) not null,
       primary key (EmployeeCodeBaseId, LocaleKey)
    );

    create table NAccessTest.dbo.[Favorite] (
        FavoriteId BIGINT IDENTITY NOT NULL,
       ProductCode VARCHAR(128) not null,
       CompanyCode VARCHAR(128) not null,
       OwnerCode VARCHAR(128) not null,
       OwnerKind INT null,
       OwnerName NVARCHAR(255) null,
       Content NVARCHAR(MAX) null,
       RegisterCode VARCHAR(128) null,
       RegistDate DATETIME null,
       Preference INT null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       primary key (FavoriteId)
    );

    create table NAccessTest.dbo.[File] (
        FileId UNIQUEIDENTIFIER not null,
       Category NVARCHAR(50) null,
       FileName NVARCHAR(1024) not null,
       ResourceId NVARCHAR(255) null,
       ResourceKind NVARCHAR(255) null,
       OwnerCode VARCHAR(128) null,
       OwnerKind INT null,
       StoredFileName NVARCHAR(1024) null,
       StoredFilePath NVARCHAR(1024) null,
       FileSize BIGINT null,
       FileType VARCHAR(128) null,
       LinkUrl NVARCHAR(1024) null,
       State INT null,
       StateId VARCHAR(50) null,
       IsRecentVersion BIT null,
       Version VARCHAR(32) null,
       VersionDesc NVARCHAR(MAX) null,
       FileGroup INT null,
       FileFloor INT null,
       CreateDate DATETIME null,
       DeleteDate DATETIME null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       FileMappingId BIGINT null,
       primary key (FileId)
    );

    create table NAccessTest.dbo.[FileMapping] (
        FileMappingId BIGINT IDENTITY NOT NULL,
       ProductCode VARCHAR(128) not null,
       SystemId NVARCHAR(128) not null,
       SubId NVARCHAR(128) null,
       Key1 NVARCHAR(MAX) null,
       Key2 NVARCHAR(MAX) null,
       Key3 NVARCHAR(MAX) null,
       Key4 NVARCHAR(MAX) null,
       Key5 NVARCHAR(MAX) null,
       State INT null,
       CreateDate DATETIME null,
       DeleteDate DATETIME null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       primary key (FileMappingId)
    );

    create table NAccessTest.dbo.[GroupActor] (
        CompanyCode NVARCHAR(255) not null,
       GroupCode NVARCHAR(255) not null,
       ActorCode NVARCHAR(255) not null,
       ActorKind INT not null,
       Descriptoin NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       primary key (CompanyCode, GroupCode, ActorCode, ActorKind)
    );

    create table NAccessTest.dbo.[Group] (
        GroupId BIGINT IDENTITY NOT NULL,
       GroupCode VARCHAR(255) not null,
       GroupName NVARCHAR(255) not null,
       Kind INT null,
       IsActive BIT null,
       SqlStatement NVARCHAR(MAX) null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       CompanyId INT null,
       primary key (GroupId)
    );

    create table NAccessTest.dbo.GroupLoc (
        GroupId BIGINT not null,
       GroupName NVARCHAR(255) null,
       SqlStatement NVARCHAR(MAX) null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       LocaleKey NVARCHAR(128) not null,
       primary key (GroupId, LocaleKey)
    );

    create table NAccessTest.dbo.GroupMeta (
        GroupId BIGINT not null,
       MetaValue NVARCHAR(MAX) null,
       MetaValueType NVARCHAR(1024) null,
       MetaKey NVARCHAR(255) not null,
       primary key (GroupId, MetaKey)
    );

    create table NAccessTest.dbo.[MasterCodeItem] (
        MasterCodeItemId UNIQUEIDENTIFIER not null,
       ITEM_CODE VARCHAR(128) not null,
       ITEM_NAME NVARCHAR(255) not null,
       ITEM_VALUE NVARCHAR(MAX) not null,
       VIEW_ORDER INT null,
       IsActive BIT null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       CODE_ID UNIQUEIDENTIFIER not null,
       MasterCodeId UNIQUEIDENTIFIER null,
       primary key (MasterCodeItemId)
    );

    create table NAccessTest.dbo.MASTER_CODE_ITEM_LOC (
        ITEM_ID UNIQUEIDENTIFIER not null,
       ITEM_NAME NVARCHAR(255) not null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       LocaleKey NVARCHAR(128) not null,
       primary key (ITEM_ID, LocaleKey)
    );

    create table NAccessTest.dbo.[MasterCode] (
        MasterCodeId UNIQUEIDENTIFIER not null,
       MasterCodeCode VARCHAR(128) not null,
       MasterCodeName NVARCHAR(128) not null,
       IsActive BIT null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       ProductId BIGINT not null,
       primary key (MasterCodeId)
    );

    create table NAccessTest.dbo.MasterCodeLoc (
        MasterCodeId UNIQUEIDENTIFIER not null,
       CodeName NVARCHAR(128) not null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       LocaleKey NVARCHAR(128) not null,
       primary key (MasterCodeId, LocaleKey)
    );

    create table NAccessTest.dbo.[Menu] (
        MenuId BIGINT IDENTITY NOT NULL,
       MenuCode VARCHAR(128) not null,
       IsActive BIT null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       MenuTemplateId BIGINT not null,
       ParentId BIGINT null,
       TreeOrder INT null,
       TreeLevel INT null,
       primary key (MenuId)
    );

    create table NAccessTest.dbo.[MenuTemplate] (
        MenuTemplateId BIGINT IDENTITY NOT NULL,
       MenuTemplateCode VARCHAR(128) not null,
       MenuTemplateName NVARCHAR(128) not null,
       MenuUrl NVARCHAR(MAX) null,
       TreePath VARCHAR(MAX) null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       ProductId BIGINT not null,
       ParentId BIGINT null,
       TreeOrder INT null,
       TreeLevel INT null,
       primary key (MenuTemplateId)
    );

    create table NAccessTest.dbo.MenuTemplateLoc (
        MenuTemplateId BIGINT not null,
       MenuTemplateLocaleName NVARCHAR(255) not null,
       MenuUrl NVARCHAR(MAX) null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       LocaleKey NVARCHAR(128) not null,
       primary key (MenuTemplateId, LocaleKey)
    );

    create table NAccessTest.dbo.MenuTemplateMeta (
        MenuTemplateId BIGINT not null,
       MetaValue NVARCHAR(MAX) null,
       MetaValueType NVARCHAR(1024) null,
       MetaKey NVARCHAR(255) not null,
       primary key (MenuTemplateId, MetaKey)
    );

    create table NAccessTest.dbo.[Product] (
        ProductId BIGINT IDENTITY NOT NULL,
       ProductCode VARCHAR(128) not null,
       ProductName NVARCHAR(128) not null,
       AbbrName NVARCHAR(128) null,
       IsActive BIT null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       primary key (ProductId)
    );

    create table NAccessTest.dbo.ProductLoc (
        ProductId BIGINT not null,
       ProductLocaleName NVARCHAR(255) null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       LocaleKey NVARCHAR(128) not null,
       primary key (ProductId, LocaleKey)
    );

    create table NAccessTest.dbo.ProductMeta (
        ProductId BIGINT not null,
       MetaValue NVARCHAR(MAX) null,
       MetaValueType NVARCHAR(1024) null,
       MetaKey NVARCHAR(255) not null,
       primary key (ProductId, MetaKey)
    );

    create table NAccessTest.dbo.[ResourceActor] (
        ProductCode NVARCHAR(255) not null,
       ResourceCode NVARCHAR(255) not null,
       ResourceInstanceId NVARCHAR(255) not null,
       CompanyCode NVARCHAR(255) not null,
       ActorCode NVARCHAR(255) not null,
       ActorKind INT not null,
       AuthorityKind INT null,
       primary key (ProductCode, ResourceCode, ResourceInstanceId, CompanyCode, ActorCode, ActorKind)
    );

    create table NAccessTest.dbo.[Resource] (
        ResourceId BIGINT IDENTITY NOT NULL,
       ProductCode VARCHAR(128) not null,
       ResourceCode VARCHAR(128) not null,
       ResourceName NVARCHAR(255) not null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       primary key (ResourceId)
    );

    create table NAccessTest.dbo.ResouceLoc (
        ResourceId BIGINT not null,
       RESOURCE_NAME NVARCHAR(255) null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       LocaleKey NVARCHAR(128) not null,
       primary key (ResourceId, LocaleKey)
    );

    create table NAccessTest.dbo.ResourceMeta (
        ResourceId BIGINT not null,
       MetaValue NVARCHAR(MAX) null,
       MetaValueType NVARCHAR(1024) null,
       MetaKey NVARCHAR(255) not null,
       primary key (ResourceId, MetaKey)
    );

    create table NAccessTest.dbo.[UserConfig] (
        ProductCode NVARCHAR(255) not null,
       CompanyCode NVARCHAR(255) not null,
       UserCode NVARCHAR(255) not null,
       ConfigKey NVARCHAR(255) not null,
       Value NVARCHAR(MAX) null,
       DefaultValue NVARCHAR(MAX) null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       primary key (ProductCode, CompanyCode, UserCode, ConfigKey)
    );

    create table NAccessTest.dbo.[UserLoginLog] (
        UserLoginLogId BIGINT IDENTITY NOT NULL,
       ProductCode VARCHAR(128) not null,
       CompanyCode VARCHAR(128) not null,
       DepartmentCode VARCHAR(128) null,
       UserCode VARCHAR(128) not null,
       LoginId NVARCHAR(50) not null,
       LoginTime DATETIME not null,
       LocaleKey VARCHAR(64) null,
       ProductName NVARCHAR(255) null,
       CompanyName NVARCHAR(255) null,
       DepartmentName NVARCHAR(255) null,
       UserName NVARCHAR(255) null,
       ExAttr NVARCHAR(MAX) null,
       primary key (UserLoginLogId)
    );

    create table NAccessTest.dbo.[User] (
        UserId INT IDENTITY NOT NULL,
       UserCode VARCHAR(128) not null,
       UserName NVARCHAR(255) not null,
       EName NVARCHAR(255) null,
       EmpNo NVARCHAR(50) not null,
       Kind INT null,
       LoginId NVARCHAR(128) null,
       Password NVARCHAR(128) null,
       IdentityNumber NVARCHAR(50) null,
       RoleCode NVARCHAR(50) null,
       Email VARCHAR(50) null,
       Telephone VARCHAR(50) null,
       MobilePhone VARCHAR(50) null,
       IsActive BIT null,
       StatusFlag VARCHAR(255) null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       UpdateTimestamp DATETIME null,
       CompanyId INT not null,
       PositionId UNIQUEIDENTIFIER null,
       GradeId UNIQUEIDENTIFIER null,
       primary key (UserId)
    );

    create table NAccessTest.dbo.UserLoc (
        UserId INT not null,
       UserName NVARCHAR(50) not null,
       Description NVARCHAR(MAX) null,
       ExAttr NVARCHAR(MAX) null,
       LocaleKey NVARCHAR(128) not null,
       primary key (UserId, LocaleKey)
    );

    create table NAccessTest.dbo.UserMeta (
        UserId INT not null,
       MetaValue NVARCHAR(MAX) null,
       MetaValueType NVARCHAR(1024) null,
       MetaKey NVARCHAR(255) not null,
       primary key (UserId, MetaKey)
    );

    create table NAccessTest.dbo.[WorkTimeByDay] (
        WorkTimeByTimeBaseId UNIQUEIDENTIFIER not null,
       CalendarCode VARCHAR(128) not null,
       WorkTime DATETIME not null,
       IsWork BIT not null,
       WorkInMinute INT null,
       CumulatedInMinute BIGINT null,
       UpdateTimestamp DATETIME null,
       primary key (WorkTimeByTimeBaseId)
    );

    create table NAccessTest.dbo.[WorkTimeByHour] (
        WorkTimeByTimeBaseId UNIQUEIDENTIFIER not null,
       CalendarCode VARCHAR(128) not null,
       WorkTime DATETIME not null,
       IsWork BIT not null,
       WorkInMinute INT null,
       CumulatedInMinute BIGINT null,
       UpdateTimestamp DATETIME null,
       primary key (WorkTimeByTimeBaseId)
    );

    create table NAccessTest.dbo.[WorkTimeByMinute] (
        WorkTimeByTimeBaseId UNIQUEIDENTIFIER not null,
       CalendarCode VARCHAR(128) not null,
       WorkTime DATETIME not null,
       IsWork BIT not null,
       WorkInMinute INT null,
       CumulatedInMinute BIGINT null,
       UpdateTimestamp DATETIME null,
       primary key (WorkTimeByTimeBaseId)
    );

    create table NAccessTest.dbo.[WorkTimeByRange] (
        WorkTimeByTimeBaseId UNIQUEIDENTIFIER not null,
       CalendarCode VARCHAR(128) not null,
       WorkTime DATETIME not null,
       IsWork BIT not null,
       WorkInMinute INT null,
       CumulatedInMinute BIGINT null,
       UpdateTimestamp DATETIME null,
       WorkTime1 DATETIME null,
       WorkTime2 DATETIME null,
       primary key (WorkTimeByTimeBaseId)
    );

    alter table NAccessTest.dbo.[Calendar] 
        add constraint FKD2C41674478FF1D8 
        foreign key (ParentId) 
        references NAccessTest.dbo.[Calendar];

    alter table NAccessTest.dbo.CalendarLoc 
        add constraint FK9B2E17B3A9659A9D 
        foreign key (CalendarId) 
        references NAccessTest.dbo.[Calendar];

    alter table NAccessTest.dbo.CALENDAR_META 
        add constraint FK_CAL_META_CAL 
        foreign key (CAL_ID) 
        references NAccessTest.dbo.[Calendar];

    create index IX_CalRule_Cal on NAccessTest.dbo.[CalendarRule] (FromTime, ToTime, CalendarId);

    alter table NAccessTest.dbo.[CalendarRule] 
        add constraint FK13480398A9659A9D 
        foreign key (CalendarId) 
        references NAccessTest.dbo.[Calendar];

    alter table NAccessTest.dbo.CALENDAR_RULE_LOC 
        add constraint FK_CAL_RULE_LOC_CAL_RULE 
        foreign key (CAL_RULE_ID) 
        references NAccessTest.dbo.[CalendarRule];

    alter table NAccessTest.dbo.CALENDAR_RULE_META 
        add constraint FK65DB3EDD4F858EE2 
        foreign key (CAL_RULE_ID) 
        references NAccessTest.dbo.[CalendarRule];

    create index IX_CAL_USER_RULE_USER on NAccessTest.dbo.[CalendarRuleOfUser] (CompanyCode, UserCode, FromTime, ToTime, CalendarRuleId);

    alter table NAccessTest.dbo.[CalendarRuleOfUser] 
        add constraint FK1D6732E4C25799BD 
        foreign key (CalendarRuleId) 
        references NAccessTest.dbo.[CalendarRule];

    alter table NAccessTest.dbo.CodeLoc 
        add constraint FKF9A54638C9F2A1EB 
        foreign key (CodeId) 
        references NAccessTest.dbo.[Code];

    alter table NAccessTest.dbo.CompanyLoc 
        add constraint FKC151678438749D06 
        foreign key (CompanyId) 
        references NAccessTest.dbo.[Company];

    alter table NAccessTest.dbo.CompanyMeta 
        add constraint FKB46B9FEB38749D06 
        foreign key (CompanyId) 
        references NAccessTest.dbo.[Company];

    alter table NAccessTest.dbo.[Department] 
        add constraint FK6BAC779638749D06 
        foreign key (CompanyId) 
        references NAccessTest.dbo.[Company];

    alter table NAccessTest.dbo.[Department] 
        add constraint FK6BAC7796A286EC9C 
        foreign key (ParentId) 
        references NAccessTest.dbo.[Department];

    alter table NAccessTest.dbo.[Department] 
        add constraint FK6BAC7796CB816B7B 
        foreign key (DepartmentId) 
        references NAccessTest.dbo.[Department];

    alter table NAccessTest.dbo.DepartmentLoc 
        add constraint FKD0801CE3CB816B7B 
        foreign key (DepartmentId) 
        references NAccessTest.dbo.[Department];

    alter table NAccessTest.dbo.DepartmentMeta 
        add constraint FK2E689169CB816B7B 
        foreign key (DepartmentId) 
        references NAccessTest.dbo.[Department];

    alter table NAccessTest.dbo.[DepartmentMember] 
        add constraint FKFE7B00CCB816B7B 
        foreign key (DepartmentId) 
        references NAccessTest.dbo.[Department];

    alter table NAccessTest.dbo.[DepartmentMember] 
        add constraint FKFE7B00C52E9DA29 
        foreign key (UserId) 
        references NAccessTest.dbo.[User];

    alter table NAccessTest.dbo.[DepartmentMember] 
        add constraint FKFE7B00C62D67FD2 
        foreign key (EmployeeTitleId) 
        references NAccessTest.dbo.[EmployeeCodeBase];

    alter table NAccessTest.dbo.[EmployeeCodeBase] 
        add constraint FK3D30BB9838749D06 
        foreign key (CompanyId) 
        references NAccessTest.dbo.[Company];

    alter table NAccessTest.dbo.EmployeeCodeLoc 
        add constraint FK2AB871F6AA6728FD 
        foreign key (EmployeeCodeBaseId) 
        references NAccessTest.dbo.[EmployeeCodeBase];

    create index IX_FILE_RESOURCE on NAccessTest.dbo.[File] (ResourceId, ResourceKind);

    alter table NAccessTest.dbo.[File] 
        add constraint FK611DA7F8B60B2B49 
        foreign key (FileMappingId) 
        references NAccessTest.dbo.[FileMapping];

    create index IX_FILE_MAP_PRD on NAccessTest.dbo.[FileMapping] (ProductCode, SystemId, SubId);

    alter table NAccessTest.dbo.[Group] 
        add constraint FKBA21C18E38749D06 
        foreign key (CompanyId) 
        references NAccessTest.dbo.[Company];

    alter table NAccessTest.dbo.GroupLoc 
        add constraint FK2D21A51BEFDEDD0C 
        foreign key (GroupId) 
        references NAccessTest.dbo.[Group];

    alter table NAccessTest.dbo.GroupMeta 
        add constraint FK43454B7CEFDEDD0C 
        foreign key (GroupId) 
        references NAccessTest.dbo.[Group];

    alter table NAccessTest.dbo.[MasterCodeItem] 
        add constraint FK_MC_ITEM_MC 
        foreign key (CODE_ID) 
        references NAccessTest.dbo.[MasterCode];

    alter table NAccessTest.dbo.[MasterCodeItem] 
        add constraint FK9DB87EF07826474F 
        foreign key (MasterCodeId) 
        references NAccessTest.dbo.[MasterCode];

    alter table NAccessTest.dbo.MASTER_CODE_ITEM_LOC 
        add constraint FK_MC_ITEM_LOC_MC_ITEM 
        foreign key (ITEM_ID) 
        references NAccessTest.dbo.[MasterCodeItem];

    create index IX_MASTERCODE_PRD on NAccessTest.dbo.[MasterCode] (ProductId);

    alter table NAccessTest.dbo.[MasterCode] 
        add constraint FKA76AF223715A1EFD 
        foreign key (ProductId) 
        references NAccessTest.dbo.[Product];

    alter table NAccessTest.dbo.MasterCodeLoc 
        add constraint FKF512DCC37826474F 
        foreign key (MasterCodeId) 
        references NAccessTest.dbo.[MasterCode];

    alter table NAccessTest.dbo.[Menu] 
        add constraint FK5CB0913794F3DEB9 
        foreign key (MenuTemplateId) 
        references NAccessTest.dbo.[MenuTemplate];

    alter table NAccessTest.dbo.[Menu] 
        add constraint FK5CB0913750F6ED7A 
        foreign key (ParentId) 
        references NAccessTest.dbo.[Menu];

    alter table NAccessTest.dbo.[Menu] 
        add constraint FK5CB09137197A1041 
        foreign key (MenuId) 
        references NAccessTest.dbo.[Menu];

    alter table NAccessTest.dbo.[MenuTemplate] 
        add constraint FKFE474531715A1EFD 
        foreign key (ProductId) 
        references NAccessTest.dbo.[Product];

    alter table NAccessTest.dbo.[MenuTemplate] 
        add constraint FKFE47453139AE675 
        foreign key (ParentId) 
        references NAccessTest.dbo.[MenuTemplate];

    alter table NAccessTest.dbo.[MenuTemplate] 
        add constraint FKFE47453194F3DEB9 
        foreign key (MenuTemplateId) 
        references NAccessTest.dbo.[MenuTemplate];

    alter table NAccessTest.dbo.MenuTemplateLoc 
        add constraint FK2829BA3894F3DEB9 
        foreign key (MenuTemplateId) 
        references NAccessTest.dbo.[MenuTemplate];

    alter table NAccessTest.dbo.MenuTemplateMeta 
        add constraint FK8329404C94F3DEB9 
        foreign key (MenuTemplateId) 
        references NAccessTest.dbo.[MenuTemplate];

    alter table NAccessTest.dbo.ProductLoc 
        add constraint FKE90E6F5B715A1EFD 
        foreign key (ProductId) 
        references NAccessTest.dbo.[Product];

    alter table NAccessTest.dbo.ProductMeta 
        add constraint FK8C87B431715A1EFD 
        foreign key (ProductId) 
        references NAccessTest.dbo.[Product];

    alter table NAccessTest.dbo.ResouceLoc 
        add constraint FK8EE6DC2F6264A118 
        foreign key (ResourceId) 
        references NAccessTest.dbo.[Resource];

    alter table NAccessTest.dbo.ResourceMeta 
        add constraint FKE3A3C0F06264A118 
        foreign key (ResourceId) 
        references NAccessTest.dbo.[Resource];

    create index IX_USER_EMP_NO on NAccessTest.dbo.[User] (EmpNo);

    create index IX_USER_LOGIN on NAccessTest.dbo.[User] (LoginId, Password);

    alter table NAccessTest.dbo.[User] 
        add constraint FK7185C17C38749D06 
        foreign key (CompanyId) 
        references NAccessTest.dbo.[Company];

    alter table NAccessTest.dbo.[User] 
        add constraint FK7185C17C14885ED8 
        foreign key (PositionId) 
        references NAccessTest.dbo.[EmployeeCodeBase];

    alter table NAccessTest.dbo.[User] 
        add constraint FK7185C17C353A2411 
        foreign key (GradeId) 
        references NAccessTest.dbo.[EmployeeCodeBase];

    alter table NAccessTest.dbo.UserLoc 
        add constraint FK1336DB8B52E9DA29 
        foreign key (UserId) 
        references NAccessTest.dbo.[User];

    alter table NAccessTest.dbo.UserMeta 
        add constraint FK6C55B0A152E9DA29 
        foreign key (UserId) 
        references NAccessTest.dbo.[User];
