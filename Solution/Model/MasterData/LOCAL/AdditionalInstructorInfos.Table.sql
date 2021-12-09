DECLARE @StartDateInstructor AS DATETIME2 = DATEADD(day, 3, GETDATE())
DECLARE @EndDateInstructor AS DATETIME2 = DATEADD(day, 6, GETDATE())
insert into AdditionalInstructorInfos values (4, @StartDateInstructor, @EndDateInstructor, 'Addrenaline addict, ready to give you a good time')