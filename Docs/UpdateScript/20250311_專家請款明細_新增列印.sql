
--0.�s�W�n�J�̳���
alter Table [User] add [DCode] [nvarchar](2) NULL
Go

--1.�s�W�M�׶O�ά�إN�X(ProjectCostCode)
CREATE TABLE [dbo].[ProjectCostCode](
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Sort] [int] NULL,
	[BDate] [datetime] NULL,
	[BFno] [nvarchar](6) NULL,
	[BName] [nvarchar](50) NULL,
	[UDate] [datetime] NULL,
	[UFno] [nvarchar](6) NULL,
	[UName] [nvarchar](50) NULL,
 CONSTRAINT [PK_ProjectCostCode] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT [dbo].[ProjectCostCode] ([Code], [Name], [Sort], [BDate], [BFno], [BName], [UDate], [UFno], [UName]) VALUES (N'001', N'�X�t�O', 1, CAST(N'2025-03-11T14:22:49.033' AS DateTime), N'J00007', N'�L����', NULL, NULL, NULL)
INSERT [dbo].[ProjectCostCode] ([Code], [Name], [Sort], [BDate], [BFno], [BName], [UDate], [UFno], [UName]) VALUES (N'002', N'�f�d�O', 2, CAST(N'2025-03-11T14:23:24.227' AS DateTime), N'J00007', N'�L����', NULL, NULL, NULL)
INSERT [dbo].[ProjectCostCode] ([Code], [Name], [Sort], [BDate], [BFno], [BName], [UDate], [UFno], [UName]) VALUES (N'003', N'�X�u�O', 3, CAST(N'2025-03-11T14:23:34.977' AS DateTime), N'J00007', N'�L����', NULL, NULL, NULL)
INSERT [dbo].[ProjectCostCode] ([Code], [Name], [Sort], [BDate], [BFno], [BName], [UDate], [UFno], [UName]) VALUES (N'004', N'���I�O', 4, CAST(N'2025-03-11T14:32:47.787' AS DateTime), N'J00007', N'�L����', CAST(N'2025-03-11T14:33:09.343' AS DateTime), N'J00007', N'�L����')
INSERT [dbo].[ProjectCostCode] ([Code], [Name], [Sort], [BDate], [BFno], [BName], [UDate], [UFno], [UName]) VALUES (N'005', N'�Ը߶O', 5, CAST(N'2025-03-11T14:33:18.277' AS DateTime), N'J00007', N'�L����', NULL, NULL, NULL)
INSERT [dbo].[ProjectCostCode] ([Code], [Name], [Sort], [BDate], [BFno], [BName], [UDate], [UFno], [UName]) VALUES (N'006', N'�t���O', 6, CAST(N'2025-03-11T14:33:27.237' AS DateTime), N'J00007', N'�L����', NULL, NULL, NULL)
INSERT [dbo].[ProjectCostCode] ([Code], [Name], [Sort], [BDate], [BFno], [BName], [UDate], [UFno], [UName]) VALUES (N'007', N'�U�ݶO', 7, CAST(N'2025-03-11T14:33:35.600' AS DateTime), N'J00007', N'�L����', NULL, NULL, NULL)
INSERT [dbo].[ProjectCostCode] ([Code], [Name], [Sort], [BDate], [BFno], [BName], [UDate], [UFno], [UName]) VALUES (N'008', N'���Z�O', 8, CAST(N'2025-03-11T14:33:45.643' AS DateTime), N'J00007', N'�L����', NULL, NULL, NULL)
GO


--2.�s�W�M�׾Ǫ̽д�(ProjectInvoice)
CREATE TABLE [dbo].[ProjectInvoice](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrjId] [nvarchar](9) NOT NULL,
	[WorkItem] [nvarchar](100) NOT NULL,
	[CostCode] [nvarchar](20) NOT NULL,
	[Fee] [int] NULL,
	[BDate] [datetime] NULL,
	[BFno] [nvarchar](6) NULL,
	[BName] [nvarchar](50) NULL,
	[UDate] [datetime] NULL,
	[UFno] [nvarchar](6) NULL,
	[UName] [nvarchar](50) NULL,
 CONSTRAINT [PK_ProjectInvoice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--3.�s�W�M�׾Ǫ̽дک���(ProjectInvoiceBasic)
CREATE TABLE [dbo].[ProjectInvoiceBasic](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MId] [int] NOT NULL,
	[BasicName] [nvarchar](40) NOT NULL,
	[CopName] [nvarchar](100) NULL,
	[Amount] [int] NOT NULL,
	[ApplyDate] [datetime] NULL,
	[Note] [nvarchar](300) NULL,
	[BDate] [datetime] NULL,
	[BFno] [nvarchar](6) NULL,
	[BName] [nvarchar](50) NULL,
 CONSTRAINT [PK_ProjectInvoiceBasic] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
