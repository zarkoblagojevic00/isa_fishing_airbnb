DECLARE @creationDateIssue AS DATETIME2 = DATEADD(day, -3, GETDATE())
SET IDENTITY_INSERT Issues ON;
insert into Issues(IssueId, UserId, IssuedEntityId, Reason, CreatedDateTime, IsReviewed) values (0, 11, 1, 'Issue reason', @creationDateIssue, 0);
insert into Issues(IssueId, UserId, IssuedEntityId, Reason, CreatedDateTime, IsReviewed) values (1, 11, 2, 'Issue reason', @creationDateIssue, 0);
SET IDENTITY_INSERT Issues OFF;
