Create database Confirmation
go
use Confirmation;
go
CREATE TABLE Account(
    Username NVARCHAR(255) NOT NULL,
    Pass NVARCHAR(255) NOT NULL,
    Id INT NOT NULL IDENTITY(1000,1),
    Email NVARCHAR(255) NOT NULL,
    IsActive BIT NOT NULL default 1
);

ALTER TABLE
    Account ADD CONSTRAINT account_id_primary PRIMARY KEY(Id);
CREATE UNIQUE INDEX account_email_unique ON
    Account(Email);
CREATE TABLE StaffMember(
    Id INT NOT NULL IDENTITY(1000,1),
    MemberName NVARCHAR(255) NOT NULL,
    PositionName INT NOT NULL,
    SchoolId INT NOT NULL
);
ALTER TABLE
    StaffMember ADD CONSTRAINT staff_member_id_primary PRIMARY KEY(id);
CREATE TABLE Forms(
    FormType NVARCHAR(255) NULL,
    topic NVARCHAR(255) NOT NULL,
    massageBody NVARCHAR(255) NULL,
    StatusOfTheMessage INT NOT NULL,
    Sender INT NOT NULL,
    FormId INT NOT NULL IDENTITY(1000,1),
    TimeSend datetime NOT NULL
);
CREATE INDEX forms_formtype_index ON
    Forms(FormType);
ALTER TABLE
    Forms ADD CONSTRAINT forms_formid_primary PRIMARY KEY(FormId);
CREATE TABLE Organizations(
    SchoolId INT NOT NULL,
    ManagerId INT NOT NULL,
    OrganizationName NVARCHAR(255) NOT NULL,
	MashovSchoolId INT NOT NULL,
    MashovPass NVARCHAR(255) NOT NULL
	
);
ALTER TABLE
    Organizations ADD CONSTRAINT organizations_schoolid_primary PRIMARY KEY(SchoolId);
CREATE TABLE Class(
    ClassName NVARCHAR(255) NOT NULL,
    GroupId INT NOT NULL,
    SchoolId INT NOT NULL,
    YearTime INT NOT NULL
);
ALTER TABLE
    Class ADD CONSTRAINT group_groupid_primary PRIMARY KEY(GroupId);
CREATE TABLE Clients_in_Group(
    ClientId INT NOT NULL,
    GroupId INT NOT NULL
);
ALTER TABLE
    Clients_in_Group ADD CONSTRAINT clients_in_group_clientid_primary PRIMARY KEY(ClientId,GroupId);

CREATE TABLE StaffMemberOfGroup(
    StaffMemberId INT NOT NULL,
    GroupId INT NOT NULL
);
ALTER TABLE
    StaffMemberOfGroup ADD CONSTRAINT staffmemberofgroup_staffmemberid_primary PRIMARY KEY(StaffMemberId,GroupId);

CREATE TABLE FormsOfGroups(
    IdOfGroup INT NOT NULL,
    FormId INT NOT NULL IDENTITY(1000,1)
);
ALTER TABLE
    FormsOfGroups ADD CONSTRAINT formsofgroups_idofgroup_primary PRIMARY KEY(IdOfGroup,FormId);

CREATE TABLE Signform(
    IdOfForm INT NOT NULL,
    PerentSignId INT NOT NULL,
    GroupId INT NOT NULL,
    SignTime datetime NOT NULL
);
CREATE TABLE Students(
    Id INT NOT NULL,
    ParentId INT NOT NULL,
    StudentName NVARCHAR(255) NOT NULL
);

ALTER TABLE
    Students ADD CONSTRAINT students_id_primary PRIMARY KEY(Id);
ALTER TABLE
    Signform ADD CONSTRAINT sign_forms_idofform_primary PRIMARY KEY(IdOfForm);
CREATE UNIQUE INDEX sign_forms_perentsignid_unique ON
    Signform(PerentSignId);
CREATE UNIQUE INDEX Signform_groupid_unique ON
    Signform(GroupId);
ALTER TABLE
    StaffMember ADD CONSTRAINT staff_member_schoolid_foreign FOREIGN KEY(SchoolId) REFERENCES Organizations(SchoolId);
ALTER TABLE
    Forms ADD CONSTRAINT forms_sender_foreign FOREIGN KEY(Sender) REFERENCES StaffMember(Id);

ALTER TABLE
    Class ADD CONSTRAINT group_schoolid_foreign FOREIGN KEY(SchoolId) REFERENCES Organizations(SchoolId);
ALTER TABLE
StaffMemberOfGroup ADD CONSTRAINT StaffMemberOfGroup_GroupId_foreign FOREIGN KEY(GroupId) REFERENCES Class(GroupId);
ALTER TABLE
StaffMemberOfGroup ADD CONSTRAINT StaffMemberOfGroup_Id_foreign FOREIGN KEY(StaffMemberId) REFERENCES StaffMember(Id);
ALTER TABLE
Signform ADD CONSTRAINT Sign_Forms_IdOfForm_foreign FOREIGN KEY(IdOfForm) REFERENCES Forms(FormId);

ALTER TABLE
FormsOfGroups ADD CONSTRAINT FormsOfGroups_IdOfForm_foreign FOREIGN KEY(FormId) REFERENCES Forms(FormId);

ALTER TABLE
FormsOfGroups ADD CONSTRAINT FormsOfGroups_IdOfGroup_foreign FOREIGN KEY(IdOfGroup) REFERENCES Class(GroupId);

ALTER TABLE
Students ADD CONSTRAINT Students_familyId_foreign FOREIGN KEY(Id) REFERENCES Account(Id);

ALTER TABLE
Clients_in_Group ADD CONSTRAINT Clients_in_Group_ClientId_foreign FOREIGN KEY(ClientId) REFERENCES Students(Id);

ALTER TABLE
Clients_in_Group ADD CONSTRAINT Clients_in_Group_GroupId_foreign FOREIGN KEY(GroupId) REFERENCES Class(GroupId);

ALTER TABLE
StaffMember ADD CONSTRAINT AccountIdStaffMember_foreign FOREIGN KEY(Id) REFERENCES Account(Id);

ALTER TABLE
Organizations ADD CONSTRAINT StaffMemberId_foreign FOREIGN KEY(ManagerId) REFERENCES StaffMember(Id);




