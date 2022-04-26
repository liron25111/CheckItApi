Use Confirmation
go


INSERT INTO StaffMember (MemberName,Pass ,Email)
VALUES ('Ofer','12345','Ofer@gmail.com')

INSERT INTO Organizations (OrganizationName,MashovSchoolId)
VALUES ('Ramon',1234)

INSERT INTO Account (Username, Pass,Email,IsActiveStudent)
VALUES ('liron','2511','liron.abovich@gmail.com',1);

INSERT INTO Class (ClassName, StaffMemberOfGroup, ClassYear) Values ('Computer Science', 1, '2020');

INSERT INTO Class (ClassName, StaffMemberOfGroup, ClassYear) Values ('Web Services', 1, '2020');

INSERT INTO Class (ClassName, StaffMemberOfGroup, ClassYear) Values ('Homeroom', 1, '2020');

INSERT INTO Students (id, [Name]) Values (1, 'Liron');
INSERT INTO ClientsInGroup (GroupId, ClientId) Values (1,1);
INSERT INTO ClientsInGroup (GroupId, ClientId) Values (1001,1);
INSERT INTO ClientsInGroup (GroupId, ClientId) Values (2001,1);






