--�O�_mis�ﶵ
alter Table ActivityCategory add IsMis [nvarchar](1) NULL
Go

Update ActivityCategory
Set IsMis = 'Y'
