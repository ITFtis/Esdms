--會議不計評選次數(Y/N)
--Select * From ActivityCategory

--alter Table ActivityCategory add IsNoCount [nvarchar](1) NULL
--Go

--Update ActivityCategory
--Set IsNoCount = 'N'
--Go

Update ActivityCategory
Set IsCount = 'Y'
Go
