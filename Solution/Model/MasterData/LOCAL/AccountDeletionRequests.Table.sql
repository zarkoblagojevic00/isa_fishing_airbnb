DECLARE @creationDate AS DATETIME2 = DATEADD(day, -3, GETDATE())
insert into AccountDeletionRequests(UserId, RequestedDateTime, Reason, IsReviewed, IsApproved) values (10, @creationDate, 'Deletion reason', 0, 0);
insert into AccountDeletionRequests(UserId, RequestedDateTime, Reason, IsReviewed, IsApproved) values (12, @creationDate, 'Deletion reason', 0, 0);