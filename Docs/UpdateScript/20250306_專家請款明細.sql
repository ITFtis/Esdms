--�M�a�дک���
alter Table Project add [PrjID] [varchar](9) NULL
alter Table Project add [OwnerA] [nvarchar](50) NULL
alter Table Project add [OwnerB] [nvarchar](50) NULL
alter Table Project add [BriefName] [nvarchar](20) NULL
alter Table Project add [PrjStartDate] [date] NULL
alter Table Project add [PrjEndDate] [date] NULL
alter Table Project add [PjNoM] [varchar](20) NULL
alter Table Project add [PjNameM] [nvarchar](100) NULL
alter Table Project add [Fee] [int] NULL

--�M�צW�٥[�j
alter TABLE Project ALTER COLUMN [Name] [nvarchar](100) NULL
