
    drop table "Calendar" cascade constraints;

    drop table CalendarLoc cascade constraints;

    drop table CALENDAR_META cascade constraints;

    drop table "CalendarRule" cascade constraints;

    drop table CALENDAR_RULE_LOC cascade constraints;

    drop table CALENDAR_RULE_META cascade constraints;

    drop table "CalendarRuleOfUser" cascade constraints;

    drop table "Code" cascade constraints;

    drop table CodeLoc cascade constraints;

    drop table "Company" cascade constraints;

    drop table CompanyLoc cascade constraints;

    drop table CompanyMeta cascade constraints;

    drop table "Department" cascade constraints;

    drop table DepartmentLoc cascade constraints;

    drop table DepartmentMeta cascade constraints;

    drop table "DepartmentMember" cascade constraints;

    drop table "EmployeeCodeBase" cascade constraints;

    drop table EmployeeCodeLoc cascade constraints;

    drop table "Favorite" cascade constraints;

    drop table "File" cascade constraints;

    drop table "FileMapping" cascade constraints;

    drop table "GroupActor" cascade constraints;

    drop table "Group" cascade constraints;

    drop table GroupLoc cascade constraints;

    drop table GroupMeta cascade constraints;

    drop table "MasterCodeItem" cascade constraints;

    drop table MASTER_CODE_ITEM_LOC cascade constraints;

    drop table "MasterCode" cascade constraints;

    drop table MasterCodeLoc cascade constraints;

    drop table "Menu" cascade constraints;

    drop table "MenuTemplate" cascade constraints;

    drop table MenuTemplateLoc cascade constraints;

    drop table MenuTemplateMeta cascade constraints;

    drop table "Product" cascade constraints;

    drop table ProductLoc cascade constraints;

    drop table ProductMeta cascade constraints;

    drop table "ResourceActor" cascade constraints;

    drop table "Resource" cascade constraints;

    drop table ResouceLoc cascade constraints;

    drop table ResourceMeta cascade constraints;

    drop table "UserConfig" cascade constraints;

    drop table "UserLoginLog" cascade constraints;

    drop table "User" cascade constraints;

    drop table UserLoc cascade constraints;

    drop table UserMeta cascade constraints;

    drop table "WorkTimeByDay" cascade constraints;

    drop table "WorkTimeByHour" cascade constraints;

    drop table "WorkTimeByMinute" cascade constraints;

    drop table "WorkTimeByRange" cascade constraints;

    drop sequence hibernate_sequence;

    create table "Calendar" (
        CalendarId NUMBER(20,0) not null,
       CalendarCode VARCHAR2(128) not null,
       CalendarName NVARCHAR2(255) not null,
       ProjectId VARCHAR2(50),
       ResourceId VARCHAR2(50),
       TimeZone VARCHAR2(50),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       ParentId NUMBER(20,0),
       TreeOrder NUMBER(10,0),
       TreeLevel NUMBER(10,0),
       primary key (CalendarId)
    );

    create table CalendarLoc (
        CalendarId NUMBER(20,0) not null,
       CalendarName NVARCHAR2(255) not null,
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       LocaleKey NVARCHAR2(128) not null,
       primary key (CalendarId, LocaleKey)
    );

    create table CALENDAR_META (
        CAL_ID NUMBER(20,0) not null,
       MetaValue NVARCHAR2(2000),
       MetaValueType NVARCHAR2(255),
       MetaKey NVARCHAR2(255) not null,
       primary key (CAL_ID, MetaKey)
    );

    create table "CalendarRule" (
        CalendarRuleId NUMBER(20,0) not null,
       CAL_RULE_NAME NVARCHAR2(255),
       DayOrException NUMBER(10,0),
       ExceptionType NUMBER(10,0),
       ExceptionPattern VARCHAR2(1024),
       ExceptionClassName VARCHAR2(1024),
       IsWorking NUMBER(10,0) not null,
       FromTime TIMESTAMP(4),
       ToTime TIMESTAMP(4),
       FromTime1 TIMESTAMP(4),
       ToTime1 TIMESTAMP(4),
       FromTime2 TIMESTAMP(4),
       ToTime2 TIMESTAMP(4),
       FromTime3 TIMESTAMP(4),
       ToTime3 TIMESTAMP(4),
       FromTime4 TIMESTAMP(4),
       ToTime4 TIMESTAMP(4),
       FromTime5 TIMESTAMP(4),
       ToTime5 TIMESTAMP(4),
       ViewOrder NUMBER(10,0),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       CalendarId NUMBER(20,0) not null,
       primary key (CalendarRuleId)
    );

    create table CALENDAR_RULE_LOC (
        CAL_RULE_ID NUMBER(20,0) not null,
       CalendarRuleLocaleName NVARCHAR2(255) not null,
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       LocaleKey NVARCHAR2(128) not null,
       primary key (CAL_RULE_ID, LocaleKey)
    );

    create table CALENDAR_RULE_META (
        CAL_RULE_ID NUMBER(20,0) not null,
       MetaValue NVARCHAR2(2000),
       MetaValueType NVARCHAR2(1024),
       MetaKey NVARCHAR2(255) not null,
       primary key (CAL_RULE_ID, MetaKey)
    );

    create table "CalendarRuleOfUser" (
        CalendarRuleOfUserId NUMBER(20,0) not null,
       CompanyCode NVARCHAR2(50) not null,
       UserCode NVARCHAR2(50) not null,
       DayOrException NUMBER(10,0) default 0 ,
       ExceptionType NUMBER(10,0),
       ExceptionPattern VARCHAR2(1024),
       ExceptionClassName VARCHAR2(1024),
       IS_WORKING NUMBER(10,0) not null,
       FromTime TIMESTAMP(4),
       ToTime TIMESTAMP(4),
       FromTime1 TIMESTAMP(4),
       ToTime1 TIMESTAMP(4),
       FromTime2 TIMESTAMP(4),
       ToTime2 TIMESTAMP(4),
       FromTime3 TIMESTAMP(4),
       ToTime3 TIMESTAMP(4),
       FromTime4 TIMESTAMP(4),
       ToTime4 TIMESTAMP(4),
       FromTime5 TIMESTAMP(4),
       ToTime5 TIMESTAMP(4),
       IsActive NUMBER(1,0),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       CalendarRuleId NUMBER(20,0) not null,
       primary key (CalendarRuleOfUserId)
    );

    create table "Code" (
        CodeId NUMBER(20,0) not null,
       ItemCode VARCHAR2(128) not null,
       IsActive NUMBER(1,0),
       IsSysDefined NUMBER(1,0),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       GroupCompanyCode VARCHAR2(128) not null,
       GroupCode VARCHAR2(128) not null,
       GroupName VARCHAR2(128) not null,
       primary key (CodeId)
    );

    create table CodeLoc (
        CodeId NUMBER(20,0) not null,
       GroupName NVARCHAR2(255) not null,
       ItemName NVARCHAR2(255) not null,
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       LocaleKey NVARCHAR2(128) not null,
       primary key (CodeId, LocaleKey)
    );

    create table "Company" (
        CompanyId NUMBER(10,0) not null,
       CompanyCode VARCHAR2(128) not null,
       CompanyName NVARCHAR2(255) not null,
       IsActive NUMBER(1,0),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       primary key (CompanyId)
    );

    create table CompanyLoc (
        CompanyId NUMBER(10,0) not null,
       CompanyName NVARCHAR2(255) not null,
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       LocaleKey NVARCHAR2(128) not null,
       primary key (CompanyId, LocaleKey)
    );

    create table CompanyMeta (
        CompanyId NUMBER(10,0) not null,
       MetaValue NVARCHAR2(2000),
       MetaValueType NVARCHAR2(1024),
       MetaKey NVARCHAR2(255) not null,
       primary key (CompanyId, MetaKey)
    );

    create table "Department" (
        DepartmentId NUMBER(10,0) not null,
       DepartmentCode VARCHAR2(128) not null,
       DepartmentName NVARCHAR2(255) not null,
       EName NVARCHAR2(255),
       Kind VARCHAR2(128),
       IsActive NUMBER(1,0),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       CompanyId NUMBER(10,0) not null,
       ParentId NUMBER(10,0),
       TreeOrder NUMBER(10,0),
       TreeLevel NUMBER(10,0),
       primary key (DepartmentId)
    );

    create table DepartmentLoc (
        DepartmentId NUMBER(10,0) not null,
       DepartmentName NVARCHAR2(255),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       LocaleKey NVARCHAR2(128) not null,
       primary key (DepartmentId, LocaleKey)
    );

    create table DepartmentMeta (
        DepartmentId NUMBER(10,0) not null,
       MetaValue NVARCHAR2(2000),
       MetaValueType NVARCHAR2(1024),
       MetaKey NVARCHAR2(255) not null,
       primary key (DepartmentId, MetaKey)
    );

    create table "DepartmentMember" (
        DepartmentMemberId NUMBER(10,0) not null,
       IsLeader NUMBER(1,0),
       IsActive NUMBER(1,0),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       DepartmentId NUMBER(10,0) not null,
       UserId NUMBER(10,0) not null,
       EmployeeTitleId RAW(16),
       primary key (DepartmentMemberId)
    );

    create table "EmployeeCodeBase" (
        EmployeeCodeBaseId RAW(16) not null,
       CodeKind NVARCHAR2(32) not null,
       EmployeeCodeBaseCode VARCHAR2(128) not null,
       EmployeeCodeBaseName NVARCHAR2(128) not null,
       EName NVARCHAR2(128),
       ViewOrder NUMBER(10,0),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       CompanyId NUMBER(10,0) not null,
       ParentCode VARCHAR2(128),
       HourlyWages NUMBER(19,5),
       primary key (EmployeeCodeBaseId)
    );

    create table EmployeeCodeLoc (
        EmployeeCodeBaseId RAW(16) not null,
       EmployeeCodeLocaleName NVARCHAR2(128),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       LocaleKey NVARCHAR2(128) not null,
       primary key (EmployeeCodeBaseId, LocaleKey)
    );

    create table "Favorite" (
        FavoriteId NUMBER(20,0) not null,
       ProductCode VARCHAR2(128) not null,
       CompanyCode VARCHAR2(128) not null,
       OwnerCode VARCHAR2(128) not null,
       OwnerKind NUMBER(10,0),
       OwnerName NVARCHAR2(255),
       Content NVARCHAR2(2000),
       RegisterCode VARCHAR2(128),
       RegistDate TIMESTAMP(4),
       Preference NUMBER(10,0),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       primary key (FavoriteId)
    );

    create table "File" (
        FileId RAW(16) not null,
       Category NVARCHAR2(50),
       FileName NVARCHAR2(1024) not null,
       ResourceId NVARCHAR2(255),
       ResourceKind NVARCHAR2(255),
       OwnerCode VARCHAR2(128),
       OwnerKind NUMBER(10,0),
       StoredFileName NVARCHAR2(1024),
       StoredFilePath NVARCHAR2(1024),
       FileSize NUMBER(20,0),
       FileType VARCHAR2(128),
       LinkUrl NVARCHAR2(1024),
       State NUMBER(10,0),
       StateId VARCHAR2(50),
       IsRecentVersion NUMBER(1,0),
       Version VARCHAR2(32),
       VersionDesc NVARCHAR2(2000),
       FileGroup NUMBER(10,0),
       FileFloor NUMBER(10,0),
       CreateDate TIMESTAMP(4),
       DeleteDate TIMESTAMP(4),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       FileMappingId NUMBER(20,0),
       primary key (FileId)
    );

    create table "FileMapping" (
        FileMappingId NUMBER(20,0) not null,
       ProductCode VARCHAR2(128) not null,
       SystemId NVARCHAR2(128) not null,
       SubId NVARCHAR2(128),
       Key1 NVARCHAR2(2000),
       Key2 NVARCHAR2(2000),
       Key3 NVARCHAR2(2000),
       Key4 NVARCHAR2(2000),
       Key5 NVARCHAR2(2000),
       State NUMBER(10,0),
       CreateDate TIMESTAMP(4),
       DeleteDate TIMESTAMP(4),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       primary key (FileMappingId)
    );

    create table "GroupActor" (
        CompanyCode NVARCHAR2(255) not null,
       GroupCode NVARCHAR2(255) not null,
       ActorCode NVARCHAR2(255) not null,
       ActorKind NUMBER(10,0) not null,
       Descriptoin NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       primary key (CompanyCode, GroupCode, ActorCode, ActorKind)
    );

    create table "Group" (
        GroupId NUMBER(20,0) not null,
       GroupCode VARCHAR2(255) not null,
       GroupName NVARCHAR2(255) not null,
       Kind NUMBER(10,0),
       IsActive NUMBER(1,0),
       SqlStatement NVARCHAR2(2000),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       CompanyId NUMBER(10,0),
       primary key (GroupId)
    );

    create table GroupLoc (
        GroupId NUMBER(20,0) not null,
       GroupName NVARCHAR2(255),
       SqlStatement NVARCHAR2(2000),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       LocaleKey NVARCHAR2(128) not null,
       primary key (GroupId, LocaleKey)
    );

    create table GroupMeta (
        GroupId NUMBER(20,0) not null,
       MetaValue NVARCHAR2(2000),
       MetaValueType NVARCHAR2(1024),
       MetaKey NVARCHAR2(255) not null,
       primary key (GroupId, MetaKey)
    );

    create table "MasterCodeItem" (
        MasterCodeItemId RAW(16) not null,
       ITEM_CODE VARCHAR2(128) not null,
       ITEM_NAME NVARCHAR2(255) not null,
       ITEM_VALUE NVARCHAR2(2000) not null,
       VIEW_ORDER NUMBER(10,0),
       IsActive NUMBER(1,0),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       CODE_ID RAW(16) not null,
       MasterCodeId RAW(16),
       primary key (MasterCodeItemId)
    );

    create table MASTER_CODE_ITEM_LOC (
        ITEM_ID RAW(16) not null,
       ITEM_NAME NVARCHAR2(255) not null,
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       LocaleKey NVARCHAR2(128) not null,
       primary key (ITEM_ID, LocaleKey)
    );

    create table "MasterCode" (
        MasterCodeId RAW(16) not null,
       MasterCodeCode VARCHAR2(128) not null,
       MasterCodeName NVARCHAR2(128) not null,
       IsActive NUMBER(1,0),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       ProductId NUMBER(20,0) not null,
       primary key (MasterCodeId)
    );

    create table MasterCodeLoc (
        MasterCodeId RAW(16) not null,
       CodeName NVARCHAR2(128) not null,
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       LocaleKey NVARCHAR2(128) not null,
       primary key (MasterCodeId, LocaleKey)
    );

    create table "Menu" (
        MenuId NUMBER(20,0) not null,
       MenuCode VARCHAR2(128) not null,
       IsActive NUMBER(1,0),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       MenuTemplateId NUMBER(20,0) not null,
       ParentId NUMBER(20,0),
       TreeOrder NUMBER(10,0),
       TreeLevel NUMBER(10,0),
       primary key (MenuId)
    );

    create table "MenuTemplate" (
        MenuTemplateId NUMBER(20,0) not null,
       MenuTemplateCode VARCHAR2(128) not null,
       MenuTemplateName NVARCHAR2(128) not null,
       MenuUrl NVARCHAR2(2000),
       TreePath VARCHAR2(4000),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       ProductId NUMBER(20,0) not null,
       ParentId NUMBER(20,0),
       TreeOrder NUMBER(10,0),
       TreeLevel NUMBER(10,0),
       primary key (MenuTemplateId)
    );

    create table MenuTemplateLoc (
        MenuTemplateId NUMBER(20,0) not null,
       MenuTemplateLocaleName NVARCHAR2(255) not null,
       MenuUrl NVARCHAR2(2000),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       LocaleKey NVARCHAR2(128) not null,
       primary key (MenuTemplateId, LocaleKey)
    );

    create table MenuTemplateMeta (
        MenuTemplateId NUMBER(20,0) not null,
       MetaValue NVARCHAR2(2000),
       MetaValueType NVARCHAR2(1024),
       MetaKey NVARCHAR2(255) not null,
       primary key (MenuTemplateId, MetaKey)
    );

    create table "Product" (
        ProductId NUMBER(20,0) not null,
       ProductCode VARCHAR2(128) not null,
       ProductName NVARCHAR2(128) not null,
       AbbrName NVARCHAR2(128),
       IsActive NUMBER(1,0),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       primary key (ProductId)
    );

    create table ProductLoc (
        ProductId NUMBER(20,0) not null,
       ProductLocaleName NVARCHAR2(255),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       LocaleKey NVARCHAR2(128) not null,
       primary key (ProductId, LocaleKey)
    );

    create table ProductMeta (
        ProductId NUMBER(20,0) not null,
       MetaValue NVARCHAR2(2000),
       MetaValueType NVARCHAR2(1024),
       MetaKey NVARCHAR2(255) not null,
       primary key (ProductId, MetaKey)
    );

    create table "ResourceActor" (
        ProductCode NVARCHAR2(255) not null,
       ResourceCode NVARCHAR2(255) not null,
       ResourceInstanceId NVARCHAR2(255) not null,
       CompanyCode NVARCHAR2(255) not null,
       ActorCode NVARCHAR2(255) not null,
       ActorKind NUMBER(10,0) not null,
       AuthorityKind NUMBER(10,0),
       primary key (ProductCode, ResourceCode, ResourceInstanceId, CompanyCode, ActorCode, ActorKind)
    );

    create table "Resource" (
        ResourceId NUMBER(20,0) not null,
       ProductCode VARCHAR2(128) not null,
       ResourceCode VARCHAR2(128) not null,
       ResourceName NVARCHAR2(255) not null,
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       primary key (ResourceId)
    );

    create table ResouceLoc (
        ResourceId NUMBER(20,0) not null,
       RESOURCE_NAME NVARCHAR2(255),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       LocaleKey NVARCHAR2(128) not null,
       primary key (ResourceId, LocaleKey)
    );

    create table ResourceMeta (
        ResourceId NUMBER(20,0) not null,
       MetaValue NVARCHAR2(2000),
       MetaValueType NVARCHAR2(1024),
       MetaKey NVARCHAR2(255) not null,
       primary key (ResourceId, MetaKey)
    );

    create table "UserConfig" (
        ProductCode NVARCHAR2(255) not null,
       CompanyCode NVARCHAR2(255) not null,
       UserCode NVARCHAR2(255) not null,
       ConfigKey NVARCHAR2(255) not null,
       Value NVARCHAR2(2000),
       DefaultValue NVARCHAR2(2000),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       primary key (ProductCode, CompanyCode, UserCode, ConfigKey)
    );

    create table "UserLoginLog" (
        UserLoginLogId NUMBER(20,0) not null,
       ProductCode VARCHAR2(128) not null,
       CompanyCode VARCHAR2(128) not null,
       DepartmentCode VARCHAR2(128),
       UserCode VARCHAR2(128) not null,
       LoginId NVARCHAR2(50) not null,
       LoginTime TIMESTAMP(4) not null,
       LocaleKey VARCHAR2(64),
       ProductName NVARCHAR2(255),
       CompanyName NVARCHAR2(255),
       DepartmentName NVARCHAR2(255),
       UserName NVARCHAR2(255),
       ExAttr NVARCHAR2(2000),
       primary key (UserLoginLogId)
    );

    create table "User" (
        UserId NUMBER(10,0) not null,
       UserCode VARCHAR2(128) not null,
       UserName NVARCHAR2(255) not null,
       EName NVARCHAR2(255),
       EmpNo NVARCHAR2(50) not null,
       Kind NUMBER(10,0),
       LoginId NVARCHAR2(128),
       Password NVARCHAR2(128),
       IdentityNumber NVARCHAR2(50),
       RoleCode NVARCHAR2(50),
       Email VARCHAR2(50),
       Telephone VARCHAR2(50),
       MobilePhone VARCHAR2(50),
       IsActive NUMBER(1,0),
       StatusFlag VARCHAR2(255),
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       UpdateTimestamp TIMESTAMP(4),
       CompanyId NUMBER(10,0) not null,
       PositionId RAW(16),
       GradeId RAW(16),
       primary key (UserId)
    );

    create table UserLoc (
        UserId NUMBER(10,0) not null,
       UserName NVARCHAR2(50) not null,
       Description NVARCHAR2(2000),
       ExAttr NVARCHAR2(2000),
       LocaleKey NVARCHAR2(128) not null,
       primary key (UserId, LocaleKey)
    );

    create table UserMeta (
        UserId NUMBER(10,0) not null,
       MetaValue NVARCHAR2(2000),
       MetaValueType NVARCHAR2(1024),
       MetaKey NVARCHAR2(255) not null,
       primary key (UserId, MetaKey)
    );

    create table "WorkTimeByDay" (
        WorkTimeByTimeBaseId RAW(16) not null,
       CalendarCode VARCHAR2(128) not null,
       WorkTime TIMESTAMP(4) not null,
       IsWork NUMBER(1,0) not null,
       WorkInMinute NUMBER(10,0),
       CumulatedInMinute NUMBER(20,0),
       UpdateTimestamp TIMESTAMP(4),
       primary key (WorkTimeByTimeBaseId)
    );

    create table "WorkTimeByHour" (
        WorkTimeByTimeBaseId RAW(16) not null,
       CalendarCode VARCHAR2(128) not null,
       WorkTime TIMESTAMP(4) not null,
       IsWork NUMBER(1,0) not null,
       WorkInMinute NUMBER(10,0),
       CumulatedInMinute NUMBER(20,0),
       UpdateTimestamp TIMESTAMP(4),
       primary key (WorkTimeByTimeBaseId)
    );

    create table "WorkTimeByMinute" (
        WorkTimeByTimeBaseId RAW(16) not null,
       CalendarCode VARCHAR2(128) not null,
       WorkTime TIMESTAMP(4) not null,
       IsWork NUMBER(1,0) not null,
       WorkInMinute NUMBER(10,0),
       CumulatedInMinute NUMBER(20,0),
       UpdateTimestamp TIMESTAMP(4),
       primary key (WorkTimeByTimeBaseId)
    );

    create table "WorkTimeByRange" (
        WorkTimeByTimeBaseId RAW(16) not null,
       CalendarCode VARCHAR2(128) not null,
       WorkTime TIMESTAMP(4) not null,
       IsWork NUMBER(1,0) not null,
       WorkInMinute NUMBER(10,0),
       CumulatedInMinute NUMBER(20,0),
       UpdateTimestamp TIMESTAMP(4),
       WorkTime1 TIMESTAMP(4),
       WorkTime2 TIMESTAMP(4),
       primary key (WorkTimeByTimeBaseId)
    );

    alter table "Calendar" 
        add constraint FKD2C41674478FF1D8 
        foreign key (ParentId) 
        references "Calendar";

    alter table CalendarLoc 
        add constraint FK9B2E17B3A9659A9D 
        foreign key (CalendarId) 
        references "Calendar";

    alter table CALENDAR_META 
        add constraint FK_CAL_META_CAL 
        foreign key (CAL_ID) 
        references "Calendar";

    create index IX_CalRule_Cal on "CalendarRule" (FromTime, ToTime, CalendarId);

    alter table "CalendarRule" 
        add constraint FK13480398A9659A9D 
        foreign key (CalendarId) 
        references "Calendar";

    alter table CALENDAR_RULE_LOC 
        add constraint FK_CAL_RULE_LOC_CAL_RULE 
        foreign key (CAL_RULE_ID) 
        references "CalendarRule";

    alter table CALENDAR_RULE_META 
        add constraint FK65DB3EDD4F858EE2 
        foreign key (CAL_RULE_ID) 
        references "CalendarRule";

    create index IX_CAL_USER_RULE_USER on "CalendarRuleOfUser" (CompanyCode, UserCode, FromTime, ToTime, CalendarRuleId);

    alter table "CalendarRuleOfUser" 
        add constraint FK1D6732E4C25799BD 
        foreign key (CalendarRuleId) 
        references "CalendarRule";

    alter table CodeLoc 
        add constraint FKF9A54638C9F2A1EB 
        foreign key (CodeId) 
        references "Code";

    alter table CompanyLoc 
        add constraint FKC151678438749D06 
        foreign key (CompanyId) 
        references "Company";

    alter table CompanyMeta 
        add constraint FKB46B9FEB38749D06 
        foreign key (CompanyId) 
        references "Company";

    alter table "Department" 
        add constraint FK6BAC779638749D06 
        foreign key (CompanyId) 
        references "Company";

    alter table "Department" 
        add constraint FK6BAC7796A286EC9C 
        foreign key (ParentId) 
        references "Department";

    alter table "Department" 
        add constraint FK6BAC7796CB816B7B 
        foreign key (DepartmentId) 
        references "Department";

    alter table DepartmentLoc 
        add constraint FKD0801CE3CB816B7B 
        foreign key (DepartmentId) 
        references "Department";

    alter table DepartmentMeta 
        add constraint FK2E689169CB816B7B 
        foreign key (DepartmentId) 
        references "Department";

    alter table "DepartmentMember" 
        add constraint FKFE7B00CCB816B7B 
        foreign key (DepartmentId) 
        references "Department";

    alter table "DepartmentMember" 
        add constraint FKFE7B00C52E9DA29 
        foreign key (UserId) 
        references "User";

    alter table "DepartmentMember" 
        add constraint FKFE7B00C62D67FD2 
        foreign key (EmployeeTitleId) 
        references "EmployeeCodeBase";

    alter table "EmployeeCodeBase" 
        add constraint FK3D30BB9838749D06 
        foreign key (CompanyId) 
        references "Company";

    alter table EmployeeCodeLoc 
        add constraint FK2AB871F6AA6728FD 
        foreign key (EmployeeCodeBaseId) 
        references "EmployeeCodeBase";

    create index IX_FILE_RESOURCE on "File" (ResourceId, ResourceKind);

    alter table "File" 
        add constraint FK611DA7F8B60B2B49 
        foreign key (FileMappingId) 
        references "FileMapping";

    create index IX_FILE_MAP_PRD on "FileMapping" (ProductCode, SystemId, SubId);

    alter table "Group" 
        add constraint FKBA21C18E38749D06 
        foreign key (CompanyId) 
        references "Company";

    alter table GroupLoc 
        add constraint FK2D21A51BEFDEDD0C 
        foreign key (GroupId) 
        references "Group";

    alter table GroupMeta 
        add constraint FK43454B7CEFDEDD0C 
        foreign key (GroupId) 
        references "Group";

    alter table "MasterCodeItem" 
        add constraint FK_MC_ITEM_MC 
        foreign key (CODE_ID) 
        references "MasterCode";

    alter table "MasterCodeItem" 
        add constraint FK9DB87EF07826474F 
        foreign key (MasterCodeId) 
        references "MasterCode";

    alter table MASTER_CODE_ITEM_LOC 
        add constraint FK_MC_ITEM_LOC_MC_ITEM 
        foreign key (ITEM_ID) 
        references "MasterCodeItem";

    create index IX_MASTERCODE_PRD on "MasterCode" (ProductId);

    alter table "MasterCode" 
        add constraint FKA76AF223715A1EFD 
        foreign key (ProductId) 
        references "Product";

    alter table MasterCodeLoc 
        add constraint FKF512DCC37826474F 
        foreign key (MasterCodeId) 
        references "MasterCode";

    alter table "Menu" 
        add constraint FK5CB0913794F3DEB9 
        foreign key (MenuTemplateId) 
        references "MenuTemplate";

    alter table "Menu" 
        add constraint FK5CB0913750F6ED7A 
        foreign key (ParentId) 
        references "Menu";

    alter table "Menu" 
        add constraint FK5CB09137197A1041 
        foreign key (MenuId) 
        references "Menu";

    alter table "MenuTemplate" 
        add constraint FKFE474531715A1EFD 
        foreign key (ProductId) 
        references "Product";

    alter table "MenuTemplate" 
        add constraint FKFE47453139AE675 
        foreign key (ParentId) 
        references "MenuTemplate";

    alter table "MenuTemplate" 
        add constraint FKFE47453194F3DEB9 
        foreign key (MenuTemplateId) 
        references "MenuTemplate";

    alter table MenuTemplateLoc 
        add constraint FK2829BA3894F3DEB9 
        foreign key (MenuTemplateId) 
        references "MenuTemplate";

    alter table MenuTemplateMeta 
        add constraint FK8329404C94F3DEB9 
        foreign key (MenuTemplateId) 
        references "MenuTemplate";

    alter table ProductLoc 
        add constraint FKE90E6F5B715A1EFD 
        foreign key (ProductId) 
        references "Product";

    alter table ProductMeta 
        add constraint FK8C87B431715A1EFD 
        foreign key (ProductId) 
        references "Product";

    alter table ResouceLoc 
        add constraint FK8EE6DC2F6264A118 
        foreign key (ResourceId) 
        references "Resource";

    alter table ResourceMeta 
        add constraint FKE3A3C0F06264A118 
        foreign key (ResourceId) 
        references "Resource";

    create index IX_USER_EMP_NO on "User" (EmpNo);

    create index IX_USER_LOGIN on "User" (LoginId, Password);

    alter table "User" 
        add constraint FK7185C17C38749D06 
        foreign key (CompanyId) 
        references "Company";

    alter table "User" 
        add constraint FK7185C17C14885ED8 
        foreign key (PositionId) 
        references "EmployeeCodeBase";

    alter table "User" 
        add constraint FK7185C17C353A2411 
        foreign key (GradeId) 
        references "EmployeeCodeBase";

    alter table UserLoc 
        add constraint FK1336DB8B52E9DA29 
        foreign key (UserId) 
        references "User";

    alter table UserMeta 
        add constraint FK6C55B0A152E9DA29 
        foreign key (UserId) 
        references "User";

    create sequence hibernate_sequence;
