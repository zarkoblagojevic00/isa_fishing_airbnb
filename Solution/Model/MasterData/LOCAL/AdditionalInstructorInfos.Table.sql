DECLARE @StartDateInstructor AS DATETIME2 = DATEADD(day, 3, GETDATE())
DECLARE @EndDateInstructor AS DATETIME2 = DATEADD(day, 6, GETDATE())
insert into AdditionalInstructorInfos values (3, @StartDateInstructor, @EndDateInstructor, 'Addrenaline addict, ready to give you a good time')
insert into AdditionalInstructorInfos values (10, @StartDateInstructor, @EndDateInstructor, 'Addrenaline addict, always ready for new adventures')