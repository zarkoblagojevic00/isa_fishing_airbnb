SET IDENTITY_INSERT Marks ON;

insert into Marks(MarkId, UserId, GivenMark, Description, ServiceId, IsApproved, IsReviewed) values (0, 11, 5.0, 'Great service', 1, 0, 0);
insert into Marks(MarkId, UserId, GivenMark, Description, ServiceId, IsApproved, IsReviewed) values (1, 5, 4.0, 'Solid service', 1, 0, 0);

SET IDENTITY_INSERT Marks OFF;
