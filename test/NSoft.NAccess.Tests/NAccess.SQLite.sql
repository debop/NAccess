
    PRAGMA foreign_keys = OFF;

    drop table if exists "Calendar";

    drop table if exists CalendarLoc;

    drop table if exists CALENDAR_META;

    drop table if exists "CalendarRule";

    drop table if exists CALENDAR_RULE_LOC;

    drop table if exists CALENDAR_RULE_META;

    drop table if exists "CalendarRuleOfUser";

    drop table if exists "Code";

    drop table if exists CodeLoc;

    drop table if exists "Company";

    drop table if exists CompanyLoc;

    drop table if exists CompanyMeta;

    drop table if exists "Department";

    drop table if exists DepartmentLoc;

    drop table if exists DepartmentMeta;

    drop table if exists "DepartmentMember";

    drop table if exists "EmployeeCodeBase";

    drop table if exists EmployeeCodeLoc;

    drop table if exists "Favorite";

    drop table if exists "File";

    drop table if exists "FileMapping";

    drop table if exists "GroupActor";

    drop table if exists "Group";

    drop table if exists GroupLoc;

    drop table if exists GroupMeta;

    drop table if exists "MasterCodeItem";

    drop table if exists MASTER_CODE_ITEM_LOC;

    drop table if exists "MasterCode";

    drop table if exists MasterCodeLoc;

    drop table if exists "Menu";

    drop table if exists "MenuTemplate";

    drop table if exists MenuTemplateLoc;

    drop table if exists MenuTemplateMeta;

    drop table if exists "Product";

    drop table if exists ProductLoc;

    drop table if exists ProductMeta;

    drop table if exists "ResourceActor";

    drop table if exists "Resource";

    drop table if exists ResouceLoc;

    drop table if exists ResourceMeta;

    drop table if exists "UserConfig";

    drop table if exists "UserLoginLog";

    drop table if exists "User";

    drop table if exists UserLoc;

    drop table if exists UserMeta;

    drop table if exists "WorkTimeByDay";

    drop table if exists "WorkTimeByHour";

    drop table if exists "WorkTimeByMinute";

    drop table if exists "WorkTimeByRange";

    PRAGMA foreign_keys = ON;

    create table "Calendar" (
        CalendarId  integer primary key autoincrement,
       CalendarCode TEXT not null,
       CalendarName TEXT not null,
       ProjectId TEXT,
       ResourceId TEXT,
       TimeZone TEXT,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME,
       ParentId BIGINT,
       TreeOrder INT,
       TreeLevel INT,
       constraint FKD2C41674478FF1D8 foreign key (ParentId) references "Calendar"
    );

    create table CalendarLoc (
        CalendarId BIGINT not null,
       CalendarName TEXT not null,
       Description TEXT,
       ExAttr TEXT,
       LocaleKey TEXT not null,
       primary key (CalendarId, LocaleKey),
       constraint FK9B2E17B3A9659A9D foreign key (CalendarId) references "Calendar"
    );

    create table CALENDAR_META (
        CAL_ID BIGINT not null,
       MetaValue TEXT,
       MetaValueType TEXT,
       MetaKey TEXT not null,
       primary key (CAL_ID, MetaKey),
       constraint FK_CAL_META_CAL foreign key (CAL_ID) references "Calendar"
    );

    create table "CalendarRule" (
        CalendarRuleId  integer primary key autoincrement,
       CAL_RULE_NAME TEXT,
       DayOrException INT,
       ExceptionType INT,
       ExceptionPattern TEXT,
       ExceptionClassName TEXT,
       IsWorking INT not null,
       FromTime DATETIME,
       ToTime DATETIME,
       FromTime1 DATETIME,
       ToTime1 DATETIME,
       FromTime2 DATETIME,
       ToTime2 DATETIME,
       FromTime3 DATETIME,
       ToTime3 DATETIME,
       FromTime4 DATETIME,
       ToTime4 DATETIME,
       FromTime5 DATETIME,
       ToTime5 DATETIME,
       ViewOrder INT,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME,
       CalendarId BIGINT not null,
       constraint FK13480398A9659A9D foreign key (CalendarId) references "Calendar"
    );

    create table CALENDAR_RULE_LOC (
        CAL_RULE_ID BIGINT not null,
       CalendarRuleLocaleName TEXT not null,
       Description TEXT,
       ExAttr TEXT,
       LocaleKey TEXT not null,
       primary key (CAL_RULE_ID, LocaleKey),
       constraint FK_CAL_RULE_LOC_CAL_RULE foreign key (CAL_RULE_ID) references "CalendarRule"
    );

    create table CALENDAR_RULE_META (
        CAL_RULE_ID BIGINT not null,
       MetaValue TEXT,
       MetaValueType TEXT,
       MetaKey TEXT not null,
       primary key (CAL_RULE_ID, MetaKey),
       constraint FK65DB3EDD4F858EE2 foreign key (CAL_RULE_ID) references "CalendarRule"
    );

    create table "CalendarRuleOfUser" (
        CalendarRuleOfUserId  integer primary key autoincrement,
       CompanyCode TEXT not null,
       UserCode TEXT not null,
       DayOrException INT default 0 ,
       ExceptionType INT,
       ExceptionPattern TEXT,
       ExceptionClassName TEXT,
       IS_WORKING INT not null,
       FromTime DATETIME,
       ToTime DATETIME,
       FromTime1 DATETIME,
       ToTime1 DATETIME,
       FromTime2 DATETIME,
       ToTime2 DATETIME,
       FromTime3 DATETIME,
       ToTime3 DATETIME,
       FromTime4 DATETIME,
       ToTime4 DATETIME,
       FromTime5 DATETIME,
       ToTime5 DATETIME,
       IsActive BOOL,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME,
       CalendarRuleId BIGINT not null,
       constraint FK1D6732E4C25799BD foreign key (CalendarRuleId) references "CalendarRule"
    );

    create table "Code" (
        CodeId  integer primary key autoincrement,
       ItemCode TEXT not null,
       IsActive BOOL,
       IsSysDefined BOOL,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME,
       GroupCompanyCode TEXT not null,
       GroupCode TEXT not null,
       GroupName TEXT not null
    );

    create table CodeLoc (
        CodeId BIGINT not null,
       GroupName TEXT not null,
       ItemName TEXT not null,
       Description TEXT,
       ExAttr TEXT,
       LocaleKey TEXT not null,
       primary key (CodeId, LocaleKey),
       constraint FKF9A54638C9F2A1EB foreign key (CodeId) references "Code"
    );

    create table "Company" (
        CompanyId  integer primary key autoincrement,
       CompanyCode TEXT not null,
       CompanyName TEXT not null,
       IsActive BOOL,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME
    );

    create table CompanyLoc (
        CompanyId INT not null,
       CompanyName TEXT not null,
       Description TEXT,
       ExAttr TEXT,
       LocaleKey TEXT not null,
       primary key (CompanyId, LocaleKey),
       constraint FKC151678438749D06 foreign key (CompanyId) references "Company"
    );

    create table CompanyMeta (
        CompanyId INT not null,
       MetaValue TEXT,
       MetaValueType TEXT,
       MetaKey TEXT not null,
       primary key (CompanyId, MetaKey),
       constraint FKB46B9FEB38749D06 foreign key (CompanyId) references "Company"
    );

    create table "Department" (
        DepartmentId  integer primary key autoincrement,
       DepartmentCode TEXT not null,
       DepartmentName TEXT not null,
       EName TEXT,
       Kind TEXT,
       IsActive BOOL,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME,
       CompanyId INT not null,
       ParentId INT,
       TreeOrder INT,
       TreeLevel INT,
       constraint FK6BAC779638749D06 foreign key (CompanyId) references "Company",
       constraint FK6BAC7796A286EC9C foreign key (ParentId) references "Department",
       constraint FK6BAC7796CB816B7B foreign key (DepartmentId) references "Department"
    );

    create table DepartmentLoc (
        DepartmentId INT not null,
       DepartmentName TEXT,
       Description TEXT,
       ExAttr TEXT,
       LocaleKey TEXT not null,
       primary key (DepartmentId, LocaleKey),
       constraint FKD0801CE3CB816B7B foreign key (DepartmentId) references "Department"
    );

    create table DepartmentMeta (
        DepartmentId INT not null,
       MetaValue TEXT,
       MetaValueType TEXT,
       MetaKey TEXT not null,
       primary key (DepartmentId, MetaKey),
       constraint FK2E689169CB816B7B foreign key (DepartmentId) references "Department"
    );

    create table "DepartmentMember" (
        DepartmentMemberId  integer primary key autoincrement,
       IsLeader BOOL,
       IsActive BOOL,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME,
       DepartmentId INT not null,
       UserId INT not null,
       EmployeeTitleId UNIQUEIDENTIFIER,
       constraint FKFE7B00CCB816B7B foreign key (DepartmentId) references "Department",
       constraint FKFE7B00C52E9DA29 foreign key (UserId) references "User",
       constraint FKFE7B00C62D67FD2 foreign key (EmployeeTitleId) references "EmployeeCodeBase"
    );

    create table "EmployeeCodeBase" (
        EmployeeCodeBaseId UNIQUEIDENTIFIER not null,
       CodeKind TEXT not null,
       EmployeeCodeBaseCode TEXT not null,
       EmployeeCodeBaseName TEXT not null,
       EName TEXT,
       ViewOrder INT,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME,
       CompanyId INT not null,
       ParentCode TEXT,
       HourlyWages NUMERIC,
       primary key (EmployeeCodeBaseId),
       constraint FK3D30BB9838749D06 foreign key (CompanyId) references "Company"
    );

    create table EmployeeCodeLoc (
        EmployeeCodeBaseId UNIQUEIDENTIFIER not null,
       EmployeeCodeLocaleName TEXT,
       Description TEXT,
       ExAttr TEXT,
       LocaleKey TEXT not null,
       primary key (EmployeeCodeBaseId, LocaleKey),
       constraint FK2AB871F6AA6728FD foreign key (EmployeeCodeBaseId) references "EmployeeCodeBase"
    );

    create table "Favorite" (
        FavoriteId  integer primary key autoincrement,
       ProductCode TEXT not null,
       CompanyCode TEXT not null,
       OwnerCode TEXT not null,
       OwnerKind INT,
       OwnerName TEXT,
       Content TEXT,
       RegisterCode TEXT,
       RegistDate DATETIME,
       Preference INT,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME
    );

    create table "File" (
        FileId UNIQUEIDENTIFIER not null,
       Category TEXT,
       FileName TEXT not null,
       ResourceId TEXT,
       ResourceKind TEXT,
       OwnerCode TEXT,
       OwnerKind INT,
       StoredFileName TEXT,
       StoredFilePath TEXT,
       FileSize BIGINT,
       FileType TEXT,
       LinkUrl TEXT,
       State INT,
       StateId TEXT,
       IsRecentVersion BOOL,
       Version TEXT,
       VersionDesc TEXT,
       FileGroup INT,
       FileFloor INT,
       CreateDate DATETIME,
       DeleteDate DATETIME,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME,
       FileMappingId BIGINT,
       primary key (FileId),
       constraint FK611DA7F8B60B2B49 foreign key (FileMappingId) references "FileMapping"
    );

    create table "FileMapping" (
        FileMappingId  integer primary key autoincrement,
       ProductCode TEXT not null,
       SystemId TEXT not null,
       SubId TEXT,
       Key1 TEXT,
       Key2 TEXT,
       Key3 TEXT,
       Key4 TEXT,
       Key5 TEXT,
       State INT,
       CreateDate DATETIME,
       DeleteDate DATETIME,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME
    );

    create table "GroupActor" (
        CompanyCode TEXT not null,
       GroupCode TEXT not null,
       ActorCode TEXT not null,
       ActorKind INT not null,
       Descriptoin TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME,
       primary key (CompanyCode, GroupCode, ActorCode, ActorKind)
    );

    create table "Group" (
        GroupId  integer primary key autoincrement,
       GroupCode TEXT not null,
       GroupName TEXT not null,
       Kind INT,
       IsActive BOOL,
       SqlStatement TEXT,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME,
       CompanyId INT,
       constraint FKBA21C18E38749D06 foreign key (CompanyId) references "Company"
    );

    create table GroupLoc (
        GroupId BIGINT not null,
       GroupName TEXT,
       SqlStatement TEXT,
       Description TEXT,
       ExAttr TEXT,
       LocaleKey TEXT not null,
       primary key (GroupId, LocaleKey),
       constraint FK2D21A51BEFDEDD0C foreign key (GroupId) references "Group"
    );

    create table GroupMeta (
        GroupId BIGINT not null,
       MetaValue TEXT,
       MetaValueType TEXT,
       MetaKey TEXT not null,
       primary key (GroupId, MetaKey),
       constraint FK43454B7CEFDEDD0C foreign key (GroupId) references "Group"
    );

    create table "MasterCodeItem" (
        MasterCodeItemId UNIQUEIDENTIFIER not null,
       ITEM_CODE TEXT not null,
       ITEM_NAME TEXT not null,
       ITEM_VALUE TEXT not null,
       VIEW_ORDER INT,
       IsActive BOOL,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME,
       CODE_ID UNIQUEIDENTIFIER not null,
       MasterCodeId UNIQUEIDENTIFIER,
       primary key (MasterCodeItemId),
       constraint FK_MC_ITEM_MC foreign key (CODE_ID) references "MasterCode",
       constraint FK9DB87EF07826474F foreign key (MasterCodeId) references "MasterCode"
    );

    create table MASTER_CODE_ITEM_LOC (
        ITEM_ID UNIQUEIDENTIFIER not null,
       ITEM_NAME TEXT not null,
       Description TEXT,
       ExAttr TEXT,
       LocaleKey TEXT not null,
       primary key (ITEM_ID, LocaleKey),
       constraint FK_MC_ITEM_LOC_MC_ITEM foreign key (ITEM_ID) references "MasterCodeItem"
    );

    create table "MasterCode" (
        MasterCodeId UNIQUEIDENTIFIER not null,
       MasterCodeCode TEXT not null,
       MasterCodeName TEXT not null,
       IsActive BOOL,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME,
       ProductId BIGINT not null,
       primary key (MasterCodeId),
       constraint FKA76AF223715A1EFD foreign key (ProductId) references "Product"
    );

    create table MasterCodeLoc (
        MasterCodeId UNIQUEIDENTIFIER not null,
       CodeName TEXT not null,
       Description TEXT,
       ExAttr TEXT,
       LocaleKey TEXT not null,
       primary key (MasterCodeId, LocaleKey),
       constraint FKF512DCC37826474F foreign key (MasterCodeId) references "MasterCode"
    );

    create table "Menu" (
        MenuId  integer primary key autoincrement,
       MenuCode TEXT not null,
       IsActive BOOL,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME,
       MenuTemplateId BIGINT not null,
       ParentId BIGINT,
       TreeOrder INT,
       TreeLevel INT,
       constraint FK5CB0913794F3DEB9 foreign key (MenuTemplateId) references "MenuTemplate",
       constraint FK5CB0913750F6ED7A foreign key (ParentId) references "Menu",
       constraint FK5CB09137197A1041 foreign key (MenuId) references "Menu"
    );

    create table "MenuTemplate" (
        MenuTemplateId  integer primary key autoincrement,
       MenuTemplateCode TEXT not null,
       MenuTemplateName TEXT not null,
       MenuUrl TEXT,
       TreePath TEXT,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME,
       ProductId BIGINT not null,
       ParentId BIGINT,
       TreeOrder INT,
       TreeLevel INT,
       constraint FKFE474531715A1EFD foreign key (ProductId) references "Product",
       constraint FKFE47453139AE675 foreign key (ParentId) references "MenuTemplate",
       constraint FKFE47453194F3DEB9 foreign key (MenuTemplateId) references "MenuTemplate"
    );

    create table MenuTemplateLoc (
        MenuTemplateId BIGINT not null,
       MenuTemplateLocaleName TEXT not null,
       MenuUrl TEXT,
       Description TEXT,
       ExAttr TEXT,
       LocaleKey TEXT not null,
       primary key (MenuTemplateId, LocaleKey),
       constraint FK2829BA3894F3DEB9 foreign key (MenuTemplateId) references "MenuTemplate"
    );

    create table MenuTemplateMeta (
        MenuTemplateId BIGINT not null,
       MetaValue TEXT,
       MetaValueType TEXT,
       MetaKey TEXT not null,
       primary key (MenuTemplateId, MetaKey),
       constraint FK8329404C94F3DEB9 foreign key (MenuTemplateId) references "MenuTemplate"
    );

    create table "Product" (
        ProductId  integer primary key autoincrement,
       ProductCode TEXT not null,
       ProductName TEXT not null,
       AbbrName TEXT,
       IsActive BOOL,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME
    );

    create table ProductLoc (
        ProductId BIGINT not null,
       ProductLocaleName TEXT,
       Description TEXT,
       ExAttr TEXT,
       LocaleKey TEXT not null,
       primary key (ProductId, LocaleKey),
       constraint FKE90E6F5B715A1EFD foreign key (ProductId) references "Product"
    );

    create table ProductMeta (
        ProductId BIGINT not null,
       MetaValue TEXT,
       MetaValueType TEXT,
       MetaKey TEXT not null,
       primary key (ProductId, MetaKey),
       constraint FK8C87B431715A1EFD foreign key (ProductId) references "Product"
    );

    create table "ResourceActor" (
        ProductCode TEXT not null,
       ResourceCode TEXT not null,
       ResourceInstanceId TEXT not null,
       CompanyCode TEXT not null,
       ActorCode TEXT not null,
       ActorKind INT not null,
       AuthorityKind INT,
       primary key (ProductCode, ResourceCode, ResourceInstanceId, CompanyCode, ActorCode, ActorKind)
    );

    create table "Resource" (
        ResourceId  integer primary key autoincrement,
       ProductCode TEXT not null,
       ResourceCode TEXT not null,
       ResourceName TEXT not null,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME
    );

    create table ResouceLoc (
        ResourceId BIGINT not null,
       RESOURCE_NAME TEXT,
       Description TEXT,
       ExAttr TEXT,
       LocaleKey TEXT not null,
       primary key (ResourceId, LocaleKey),
       constraint FK8EE6DC2F6264A118 foreign key (ResourceId) references "Resource"
    );

    create table ResourceMeta (
        ResourceId BIGINT not null,
       MetaValue TEXT,
       MetaValueType TEXT,
       MetaKey TEXT not null,
       primary key (ResourceId, MetaKey),
       constraint FKE3A3C0F06264A118 foreign key (ResourceId) references "Resource"
    );

    create table "UserConfig" (
        ProductCode TEXT not null,
       CompanyCode TEXT not null,
       UserCode TEXT not null,
       ConfigKey TEXT not null,
       Value TEXT,
       DefaultValue TEXT,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME,
       primary key (ProductCode, CompanyCode, UserCode, ConfigKey)
    );

    create table "UserLoginLog" (
        UserLoginLogId  integer primary key autoincrement,
       ProductCode TEXT not null,
       CompanyCode TEXT not null,
       DepartmentCode TEXT,
       UserCode TEXT not null,
       LoginId TEXT not null,
       LoginTime DATETIME not null,
       LocaleKey TEXT,
       ProductName TEXT,
       CompanyName TEXT,
       DepartmentName TEXT,
       UserName TEXT,
       ExAttr TEXT
    );

    create table "User" (
        UserId  integer primary key autoincrement,
       UserCode TEXT not null,
       UserName TEXT not null,
       EName TEXT,
       EmpNo TEXT not null,
       Kind INT,
       LoginId TEXT,
       Password TEXT,
       IdentityNumber TEXT,
       RoleCode TEXT,
       Email TEXT,
       Telephone TEXT,
       MobilePhone TEXT,
       IsActive BOOL,
       StatusFlag TEXT,
       Description TEXT,
       ExAttr TEXT,
       UpdateTimestamp DATETIME,
       CompanyId INT not null,
       PositionId UNIQUEIDENTIFIER,
       GradeId UNIQUEIDENTIFIER,
       constraint FK7185C17C38749D06 foreign key (CompanyId) references "Company",
       constraint FK7185C17C14885ED8 foreign key (PositionId) references "EmployeeCodeBase",
       constraint FK7185C17C353A2411 foreign key (GradeId) references "EmployeeCodeBase"
    );

    create table UserLoc (
        UserId INT not null,
       UserName TEXT not null,
       Description TEXT,
       ExAttr TEXT,
       LocaleKey TEXT not null,
       primary key (UserId, LocaleKey),
       constraint FK1336DB8B52E9DA29 foreign key (UserId) references "User"
    );

    create table UserMeta (
        UserId INT not null,
       MetaValue TEXT,
       MetaValueType TEXT,
       MetaKey TEXT not null,
       primary key (UserId, MetaKey),
       constraint FK6C55B0A152E9DA29 foreign key (UserId) references "User"
    );

    create table "WorkTimeByDay" (
        WorkTimeByTimeBaseId UNIQUEIDENTIFIER not null,
       CalendarCode TEXT not null,
       WorkTime DATETIME not null,
       IsWork BOOL not null,
       WorkInMinute INT,
       CumulatedInMinute BIGINT,
       UpdateTimestamp DATETIME,
       primary key (WorkTimeByTimeBaseId)
    );

    create table "WorkTimeByHour" (
        WorkTimeByTimeBaseId UNIQUEIDENTIFIER not null,
       CalendarCode TEXT not null,
       WorkTime DATETIME not null,
       IsWork BOOL not null,
       WorkInMinute INT,
       CumulatedInMinute BIGINT,
       UpdateTimestamp DATETIME,
       primary key (WorkTimeByTimeBaseId)
    );

    create table "WorkTimeByMinute" (
        WorkTimeByTimeBaseId UNIQUEIDENTIFIER not null,
       CalendarCode TEXT not null,
       WorkTime DATETIME not null,
       IsWork BOOL not null,
       WorkInMinute INT,
       CumulatedInMinute BIGINT,
       UpdateTimestamp DATETIME,
       primary key (WorkTimeByTimeBaseId)
    );

    create table "WorkTimeByRange" (
        WorkTimeByTimeBaseId UNIQUEIDENTIFIER not null,
       CalendarCode TEXT not null,
       WorkTime DATETIME not null,
       IsWork BOOL not null,
       WorkInMinute INT,
       CumulatedInMinute BIGINT,
       UpdateTimestamp DATETIME,
       WorkTime1 DATETIME,
       WorkTime2 DATETIME,
       primary key (WorkTimeByTimeBaseId)
    );

    create index IX_CalRule_Cal on "CalendarRule" (FromTime, ToTime, CalendarId);

    create index IX_CAL_USER_RULE_USER on "CalendarRuleOfUser" (CompanyCode, UserCode, FromTime, ToTime, CalendarRuleId);

    create index IX_FILE_RESOURCE on "File" (ResourceId, ResourceKind);

    create index IX_FILE_MAP_PRD on "FileMapping" (ProductCode, SystemId, SubId);

    create index IX_MASTERCODE_PRD on "MasterCode" (ProductId);

    create index IX_USER_EMP_NO on "User" (EmpNo);

    create index IX_USER_LOGIN on "User" (LoginId, Password);
