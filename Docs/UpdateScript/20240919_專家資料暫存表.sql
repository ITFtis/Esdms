--�M�a��ƼȦs��
--1.�s�W��ƪ�(z_DataMisBasicUser)
CREATE TABLE [dbo].[z_DataMisBasicUser](
	[BasicName] [nvarchar](40) NOT NULL,
	[sno] [nvarchar](50) NULL,
	[PrivatePhone] [nvarchar](30) NULL,
	[OfficeEmail] [nvarchar](50) NULL,
	[BDate] [datetime] NULL,
	[UDate] [datetime] NULL,
	[UMno] [nvarchar](30) NULL,
	[UName] [nvarchar](50) NULL,
	[IsUpdate] [bit] NOT NULL,
 CONSTRAINT [PK_z_DataMisBasicUser_1] PRIMARY KEY CLUSTERED 
(
	[BasicName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--2.sp
CREATE PROCEDURE [dbo].[sp_update_z_DataMisBasicUser]
AS
BEGIN
	--1.�R���L���ʡA��s�M�a��ƼȦs��
	Delete z_DataMisBasicUser
	Where IsUpdate = 0

	Insert Into z_DataMisBasicUser(BasicName, sno, PrivatePhone, OfficeEmail, BDate, UDate, UMno, UName, IsUpdate)
	Select Name AS BasicName, '' AS sno, IsNull(PrivatePhone, '') AS PrivatePhone, IsNull(OfficeEmail, '') AS OfficeEmail, 
		   GetDate() AS BDate, Null AS UDate, Null AS UMno, Null AS UName, 0 AS IsUpdate 
	From BasicUser a
	Where Exists
	(
		--(BasicUser)�M�a��Ʃe���m�W���ơA���᭱���
		Select *
		From
		(
			Select Name, Max(BDate) AS BDate
			From BasicUser
			Group By Name
		)b 
		Where a.Name = b.Name And a.BDate = b.BDate
	)
	And Not Exists
	(
		--(z_DataMisBasicUser)�O�s���ʪ��e�����
		Select *
		From z_DataMisBasicUser c
		Where a.Name = c.BasicName
	)
	
END
GO
