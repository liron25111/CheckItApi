Create database Confirmation
go
use Confirmation;
go
CREATE TABLE Account(
    Username NVARCHAR(255) NOT NULL,
    Pass NVARCHAR(255) NOT NULL,
    Id INT NOT NULL IDENTITY(1,1000),
    Email NVARCHAR(255) NOT NULL,
    IsActiveStudent BIT NOT NULL
);
ALTER TABLE
    Account ADD CONSTRAINT account_id_primary PRIMARY KEY(Id);
CREATE UNIQUE INDEX account_email_unique ON
    Account(Email);
CREATE TABLE StaffMember(
    id INT NOT NULL IDENTITY(1,1000),
    MemberName NVARCHAR(255) NOT NULL,
    SchoolId INT ,
    Pass NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL
);
ALTER TABLE
    StaffMember ADD CONSTRAINT staffmember_id_primary PRIMARY KEY(id);
CREATE UNIQUE INDEX staffmember_email_unique ON
    StaffMember(Email);
CREATE TABLE Form(
    FormType NVARCHAR(255) NULL,
    topic NVARCHAR(255) NOT NULL,
    massageBody NVARCHAR(255) NULL,
    StatusOfTheMessage INT NOT NULL,
    FormId INT NOT NULL IDENTITY(1,10000),
--    GroupId INT NOT NULL,
    TripDate DATETIME NOT NULL,
    SentByStaffMemebr INT NOT NULL
);
CREATE INDEX Form_formtype_index ON
    Form(FormType);
ALTER TABLE
    Form ADD CONSTRAINT Form_formid_primary PRIMARY KEY(FormId);
CREATE TABLE Organizations(
    SchoolId INT NOT NULL IDENTITY(1,1000),
    ManagerId INT,
    OrganizationName NVARCHAR(255) NOT NULL,
    MashovSchoolId INT NOT NULL
);
ALTER TABLE
    Organizations ADD CONSTRAINT organizations_schoolid_primary PRIMARY KEY(SchoolId);
CREATE TABLE Class(
    ClassName NVARCHAR(255) NOT NULL,
    StaffMemberOfGroup INT NOT NULL,
    GroupId INT NOT NULL IDENTITY(1,1000),
    ClassYear NVARCHAR(255) NOT NULL
);
ALTER TABLE
    Class ADD CONSTRAINT group_groupid_primary PRIMARY KEY(GroupId);
CREATE TABLE ClientsInGroup(
    ClientId INT NOT NULL,
    GroupId INT NOT NULL
);
ALTER TABLE
    ClientsInGroup ADD CONSTRAINT clientsingroup_clientid_primary PRIMARY KEY(ClientId,GroupId);
CREATE TABLE SignForm(
    IdOfForm INT NOT NULL,
    Account INT NOT NULL,
    SignatureTime TIME NOT NULL
);
ALTER TABLE
    SignForm ADD CONSTRAINT signForm_idofform_primary PRIMARY KEY(IdOfForm,Account);
CREATE TABLE Students(
    id INT NOT NULL,
    Name NVARCHAR(255) NOT NULL
);
ALTER TABLE
    Students ADD CONSTRAINT students_id_primary PRIMARY KEY(id);
CREATE TABLE AccountOrganizations(
    AccountId INT NOT NULL,
    OrganizationId INT NOT NULL
);
ALTER TABLE
    AccountOrganizations ADD CONSTRAINT accountorganizations_accountid_primary PRIMARY KEY(AccountId,OrganizationId);

CREATE TABLE GroupsInForm(
    GroupId INT NOT NULL,
    FormId INT NOT NULL
);
ALTER TABLE
    GroupsInForm ADD CONSTRAINT groupsInForm_primary PRIMARY KEY(GroupId,FormId);
ALTER TABLE
    Class ADD CONSTRAINT group_staffmemberofgroup_foreign FOREIGN KEY(StaffMemberOfGroup) REFERENCES StaffMember(id);
ALTER TABLE
    StaffMember ADD CONSTRAINT staffmember_schoolid_foreign FOREIGN KEY(SchoolId) REFERENCES Organizations(SchoolId);
--ALTER TABLE
--    Form ADD CONSTRAINT Form_groupid_foreign FOREIGN KEY(GroupId) REFERENCES Class(GroupId);
ALTER TABLE
    Students ADD CONSTRAINT students_Id_foreign FOREIGN KEY(Id) REFERENCES Account(Id);
ALTER TABLE
    AccountOrganizations ADD CONSTRAINT account_Id_foreign FOREIGN KEY(AccountId) REFERENCES Account(Id);
ALTER TABLE
    SignForm ADD CONSTRAINT account_Idform_foreign FOREIGN KEY(Account) REFERENCES Account(Id);
ALTER TABLE
    ClientsInGroup ADD CONSTRAINT ClientsInGroup_ClientId_foreign FOREIGN KEY(ClientId) REFERENCES Students(id);
ALTER TABLE
    ClientsInGroup ADD CONSTRAINT ClientsInGroup_GroupId_foreign FOREIGN KEY(GroupId) REFERENCES Class(GroupId);

ALTER TABLE
    AccountOrganizations ADD CONSTRAINT AccountOrganizations_OrganizationId_foreign FOREIGN KEY(OrganizationId) REFERENCES Organizations(SchoolId);
ALTER TABLE
    SignForm ADD CONSTRAINT SignForm_IdOfForm_foreign FOREIGN KEY(IdOfForm) REFERENCES Form(FormId);

ALTER TABLE
    Organizations ADD CONSTRAINT Organizations_ManagerId_foreign FOREIGN KEY(ManagerId) REFERENCES StaffMember(id);

ALTER TABLE
    Form ADD CONSTRAINT Form_SentByStaffMember FOREIGN KEY(SentByStaffMemebr) REFERENCES StaffMember(id);

ALTER TABLE
    GroupsInForm ADD CONSTRAINT GroupId_foreign FOREIGN KEY(GroupId) REFERENCES Class(GroupId);

ALTER TABLE
    GroupsInForm ADD CONSTRAINT FormId_foreign FOREIGN KEY(FormId) REFERENCES Form(FormId);



