--�|ĳ���p���隸��(Y/N)
--Select * From ActivityCategory

alter Table ActivityCategory add IsCount [nvarchar](1) NULL
Go


Update ActivityCategory
Set IsCount = 'Y'
Go
