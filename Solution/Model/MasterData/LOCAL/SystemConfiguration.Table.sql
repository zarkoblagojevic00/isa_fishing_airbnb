DELETE SystemConfigurations

DBCC CHECKIDENT ('[dbo].[SystemConfigurations]', RESEED, 1)

INSERT INTO SystemConfigurations VALUES ('MailAddress', 'zamisr.isa@gmail.com')
INSERT INTO SystemConfigurations VALUES ('MailPassword', 'isamail123')
INSERT INTO SystemConfigurations VALUES ('MailPort', '587')
INSERT INTO SystemConfigurations VALUES ('MailHost', 'smtp.gmail.com')