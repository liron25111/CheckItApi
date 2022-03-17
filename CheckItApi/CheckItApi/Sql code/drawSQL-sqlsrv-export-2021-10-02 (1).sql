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
    id INT NOT NULL,
    MemberName NVARCHAR(255) NOT NULL,
    PositionName INT NOT NULL,
    SchoolId INT NOT NULL,
    Pass NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL
);
ALTER TABLE
    StaffMember ADD CONSTRAINT staffmember_id_primary PRIMARY KEY(id);
CREATE UNIQUE INDEX staffmember_email_unique ON
    StaffMember(Email);
CREATE TABLE Forms(
    FormType NVARCHAR(255) NULL,
    topic NVARCHAR(255) NOT NULL,
    massageBody NVARCHAR(255) NULL,
    StatusOfTheMessage INT NOT NULL,
    FormId INT NOT NULL IDENTITY(1,10000),
    GroupId INT NOT NULL,
    Time TIME NOT NULL
);
CREATE INDEX forms_formtype_index ON
    Forms(FormType);
ALTER TABLE
    Forms ADD CONSTRAINT forms_formid_primary PRIMARY KEY(FormId);
CREATE TABLE Organizations(
    SchoolId INT NOT NULL,
    ManagerId INT NOT NULL,
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
CREATE TABLE SignForms(
    IdOfForm INT NOT NULL,
    Account INT NOT NULL,
    SignatureTime TIME NOT NULL
);
ALTER TABLE
    SignForms ADD CONSTRAINT signforms_idofform_primary PRIMARY KEY(IdOfForm,Account);
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

ALTER TABLE
    Class ADD CONSTRAINT group_staffmemberofgroup_foreign FOREIGN KEY(StaffMemberOfGroup) REFERENCES StaffMember(id);
ALTER TABLE
    StaffMember ADD CONSTRAINT staffmember_schoolid_foreign FOREIGN KEY(SchoolId) REFERENCES Organizations(SchoolId);
ALTER TABLE
    Forms ADD CONSTRAINT forms_groupid_foreign FOREIGN KEY(GroupId) REFERENCES Class(GroupId);
ALTER TABLE
    Students ADD CONSTRAINT students_Id_foreign FOREIGN KEY(Id) REFERENCES Account(Id);
ALTER TABLE
    AccountOrganizations ADD CONSTRAINT account_Id_foreign FOREIGN KEY(AccountId) REFERENCES Account(Id);
ALTER TABLE
    SignForms ADD CONSTRAINT account_Idform_foreign FOREIGN KEY(Account) REFERENCES Account(Id);
ALTER TABLE
    ClientsInGroup ADD CONSTRAINT ClientsInGroup_ClientId_foreign FOREIGN KEY(ClientId) REFERENCES Students(id);
ALTER TABLE
    ClientsInGroup ADD CONSTRAINT ClientsInGroup_GroupId_foreign FOREIGN KEY(GroupId) REFERENCES Class(GroupId);

ALTER TABLE
    AccountOrganizations ADD CONSTRAINT AccountOrganizations_OrganizationId_foreign FOREIGN KEY(OrganizationId) REFERENCES Organizations(SchoolId);
ALTER TABLE
    SignForms ADD CONSTRAINT SignForms_IdOfForm_foreign FOREIGN KEY(IdOfForm) REFERENCES Forms(FormId);

	ALTER TABLE
    Organizations ADD CONSTRAINT Organizations_ManagerId_foreign FOREIGN KEY(ManagerId) REFERENCES StaffMember(id);





