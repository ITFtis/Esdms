--會議資料修正
--Select * From ActivityCategory Order By Id Desc
--Select * From ActivityCategory_temp_old
--Select * From ActivityCategory_temp_new




--Delete ActivityCategory_temp_new DBCC CHECKIDENT('ActivityCategory_temp_new', RESEED, 0) 

--SELECT  TOP (200) Id, Type, Name, MapNewId 
--FROM     ActivityCategory_temp_old
--WHERE   (Type = 1) AND (Name = '活動辦理')

----------------------------------------------------------------------

--1.新增資料表+資料(ActivityCategory_temp_old) 多一個對應欄位 map_temp_new_Id
--2.新增資料表+資料(ActivityCategory_temp_new)
CREATE TABLE [dbo].[ActivityCategory_temp_new](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[FullName] [nvarchar](200) NULL,
	[DCode] [nvarchar](40) NULL,
	[BDate] [datetime] NULL,
	[BFno] [nvarchar](6) NULL,
	[BName] [nvarchar](50) NULL,
	[UDate] [datetime] NULL,
	[UFno] [nvarchar](6) NULL,
	[UName] [nvarchar](50) NULL,
	[Sort] [int] NULL,
	[IsCount] [nvarchar](1) NULL,
 CONSTRAINT [PK_dbo.ActivityCategory_temp_new] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActivityCategory_temp_old]    Script Date: 2024/7/10 下午 03:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityCategory_temp_old](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DCode] [nvarchar](40) NULL,
	[BDate] [datetime] NULL,
	[BFno] [nvarchar](6) NULL,
	[BName] [nvarchar](50) NULL,
	[UDate] [datetime] NULL,
	[UFno] [nvarchar](6) NULL,
	[UName] [nvarchar](50) NULL,
	[Sort] [int] NULL,
	[IsCount] [nvarchar](1) NULL,
	[MapNewId] [int] NULL,
 CONSTRAINT [PK_dbo.ActivityCategory_temp_old] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ActivityCategory_temp_new] ON 

INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (1, 1, N'計畫審查', N'計畫審查(評選/進度報告/期中/期末/分包評選等)', N'D01', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 1, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (2, 1, N'定期/臨時工作會議', N'定期/臨時工作會議', N'D02', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 2, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (3, 1, N'專案諮詢/學習會議', N'專案諮詢/學習會議(專諮/研商/講習/說明/研討會等)', N'D03', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 3, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (4, 1, N'審查/審議會議', N'審查/審議會議(現勘、評鑑、綠色工廠、工程督導、技審、期刊、手冊、獎項、題庫、石油基金退費等)', N'D04', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 4, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (5, 1, N'輔導/查核/訪談', N'輔導/查核/訪談', N'D05', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 5, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (6, 1, N'活動辦理', N'活動辦理(參訪、記者會、頒獎等各類活動)', N'D06', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 6, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (7, 1, N'訓練/培訓班', N'訓練/培訓班', N'D07', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 7, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (8, 1, N'計畫工作分包', N'計畫工作分包', N'D08', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 8, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (9, 1, N'計畫顧問', N'計畫顧問', N'D09', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 9, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (10, 1, N'期刊撰稿', N'期刊撰稿', N'D10', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 10, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (11, 1, N'業主制定委員名冊', N'業主制定委員名冊', N'D11', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 11, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (12, 2, N'環境部', N'', N'A0', NULL, N'J00007', N'林正祥', NULL, N'J00007', N'林正祥', 1, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (13, 2, N'能源署', N'', N'B0', NULL, N'J00007', N'林正祥', NULL, N'J00007', N'林正祥', 2, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (14, 2, N'產發署', N'', N'C0', NULL, N'J00007', N'林正祥', NULL, N'J00007', N'林正祥', 3, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (15, 2, N'臺北市政府環境保護局', N'', N'E01', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 0, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (16, 2, N'臺南市政府環境保護局', N'', N'E03', NULL, N'F01885', N'許瓊中', NULL, N'F01885', N'許瓊中', 0, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (17, 2, N'臺北市政府工務局', N'', N'E02', NULL, N'F01885', N'許瓊中', NULL, N'F01885', N'許瓊中', 0, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (18, 2, N'教育部', N'', N'E04', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 0, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (19, 2, N'行政院農業委員會', N'', N'E05', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 0, N'Y')
SET IDENTITY_INSERT [dbo].[ActivityCategory_temp_new] OFF
GO

SET IDENTITY_INSERT [dbo].[ActivityCategory_temp_old] ON 

INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (4, 1, N'評選會議', N'D01', NULL, NULL, NULL, CAST(N'2024-06-21T11:21:43.427' AS DateTime), N'F01885', N'許瓊中', 1, N'Y', 1)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (10, 1, N'期中/末審查會議', N'D02', CAST(N'2024-01-10T10:09:47.497' AS DateTime), N'F01879', N'陳宇揚', CAST(N'2024-06-21T11:21:48.067' AS DateTime), N'F01885', N'許瓊中', 2, N'Y', 1)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (11, 1, N'專諮/研商/講習/研討會議', N'D03', CAST(N'2024-01-10T10:10:26.577' AS DateTime), N'F01879', N'陳宇揚', CAST(N'2024-07-04T17:01:13.983' AS DateTime), N'F01879', N'陳宇揚', 3, N'Y', 3)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (12, 1, N'技審會議', N'D04', CAST(N'2024-01-10T10:10:46.367' AS DateTime), N'F01879', N'陳宇揚', CAST(N'2024-06-21T11:22:21.383' AS DateTime), N'F01885', N'許瓊中', 4, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (13, 1, N'輔導/查核', N'D05', CAST(N'2024-01-10T10:11:08.330' AS DateTime), N'F01879', N'陳宇揚', CAST(N'2024-07-04T16:59:02.890' AS DateTime), N'F01879', N'陳宇揚', 5, N'Y', 5)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (14, 1, N'課程講師', N'D06', CAST(N'2024-01-10T10:11:18.077' AS DateTime), N'F01879', N'陳宇揚', CAST(N'2024-06-21T11:22:10.177' AS DateTime), N'F01885', N'許瓊中', 6, N'Y', 7)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (15, 2, N'環境部', N'A0', CAST(N'2024-01-18T09:53:30.417' AS DateTime), N'J00007', N'林正祥', CAST(N'2024-04-03T19:01:40.570' AS DateTime), N'J00007', N'林正祥', 1, N'Y', 12)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (16, 2, N'能源署', N'B0', CAST(N'2024-01-18T09:54:41.953' AS DateTime), N'J00007', N'林正祥', CAST(N'2024-04-03T19:01:48.013' AS DateTime), N'J00007', N'林正祥', 2, N'Y', 13)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (17, 2, N'產發署', N'C0', CAST(N'2024-01-18T09:54:55.500' AS DateTime), N'J00007', N'林正祥', CAST(N'2024-04-03T19:01:54.850' AS DateTime), N'J00007', N'林正祥', 3, N'Y', 14)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (18, 1, N'計畫分包', N'D07', CAST(N'2024-03-11T13:31:59.963' AS DateTime), N'F01879', N'陳宇揚', CAST(N'2024-06-21T11:22:06.710' AS DateTime), N'F01885', N'許瓊中', 7, N'Y', 8)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (19, 1, N'計畫顧問', N'D08', CAST(N'2024-03-11T17:14:00.013' AS DateTime), N'F01879', N'陳宇揚', CAST(N'2024-06-21T11:22:03.537' AS DateTime), N'F01885', N'許瓊中', 8, N'Y', 9)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (20, 1, N'113年產發署計畫委員手冊', N'D09', CAST(N'2024-04-01T16:37:22.870' AS DateTime), N'F01885', N'許瓊中', CAST(N'2024-07-08T10:25:26.060' AS DateTime), N'J00007', N'林正祥', 9, N'N', 11)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (21, 1, N'環境部評等審查小組委員名單', N'D10', CAST(N'2024-04-02T17:31:27.377' AS DateTime), N'F01885', N'許瓊中', CAST(N'2024-07-08T10:25:36.417' AS DateTime), N'J00007', N'林正祥', 10, N'N', 11)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (22, 1, N'分包評選', N'D11', CAST(N'2024-04-02T17:31:33.713' AS DateTime), N'F01885', N'許瓊中', CAST(N'2024-04-03T19:00:11.180' AS DateTime), N'J00007', N'林正祥', 11, N'Y', 1)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (23, 1, N'綠色工廠審議會', N'D12', CAST(N'2024-04-02T17:31:39.343' AS DateTime), N'F01885', N'許瓊中', CAST(N'2024-04-03T19:00:22.560' AS DateTime), N'J00007', N'林正祥', 12, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (24, 1, N'期刊訪問', N'D13', CAST(N'2024-04-02T17:31:44.243' AS DateTime), N'F01885', N'許瓊中', CAST(N'2024-04-03T19:00:32.753' AS DateTime), N'J00007', N'林正祥', 13, N'Y', 5)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (25, 1, N'期刊撰稿人', N'D14', CAST(N'2024-04-02T17:31:49.153' AS DateTime), N'F01885', N'許瓊中', CAST(N'2024-04-03T19:00:41.603' AS DateTime), N'J00007', N'林正祥', 14, N'Y', 10)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (26, 1, N'期刊審查委員(手冊審查)', N'D15', CAST(N'2024-04-02T17:31:55.773' AS DateTime), N'F01885', N'許瓊中', CAST(N'2024-05-24T14:03:09.923' AS DateTime), N'F01885', N'許瓊中', 15, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (27, 1, N'獎項審查委員', N'D16', CAST(N'2024-04-02T17:32:04.440' AS DateTime), N'F01885', N'許瓊中', CAST(N'2024-04-03T19:01:08.330' AS DateTime), N'J00007', N'林正祥', 16, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (28, 1, N'評鑑委員', N'D17', CAST(N'2024-04-02T17:32:10.740' AS DateTime), N'F01885', N'許瓊中', CAST(N'2024-04-03T19:01:16.110' AS DateTime), N'J00007', N'林正祥', 17, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (29, 1, N'環境部碳費費率審議會委員名單', N'D18', CAST(N'2024-04-02T17:32:16.497' AS DateTime), N'F01885', N'許瓊中', CAST(N'2024-07-08T10:25:51.543' AS DateTime), N'J00007', N'林正祥', 18, N'N', 11)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (30, 1, N'訪廠輔導顧問', N'D19', CAST(N'2024-04-22T09:15:26.237' AS DateTime), N'F01885', N'許瓊中', CAST(N'2024-04-22T09:15:50.303' AS DateTime), N'F01885', N'許瓊中', 19, N'Y', 5)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (31, 1, N'專業題庫審查', N'D20', CAST(N'2024-04-25T11:38:09.070' AS DateTime), N'F01885', N'許瓊中', NULL, NULL, NULL, 20, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (32, 1, N'工程督導', N'D21', CAST(N'2024-04-25T11:38:19.307' AS DateTime), N'F01885', N'許瓊中', NULL, NULL, NULL, 21, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (33, 1, N'輔導說明會講師', N'D22', CAST(N'2024-04-25T11:50:23.347' AS DateTime), N'F01885', N'許瓊中', NULL, NULL, NULL, 22, N'Y', 7)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (35, 1, N'審議會議', N'D23', CAST(N'2024-05-13T08:04:02.037' AS DateTime), N'F01885', N'許瓊中', NULL, NULL, NULL, 23, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (36, 1, N'石油基金退費審查', N'D24', CAST(N'2024-05-13T10:59:11.027' AS DateTime), N'F01885', N'許瓊中', NULL, NULL, NULL, 24, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (37, 1, N'活動辦理', N'D25', CAST(N'2024-05-29T10:56:53.713' AS DateTime), N'F01885', N'許瓊中', NULL, NULL, NULL, 0, N'Y', 6)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (38, 1, N'違反個資行政調查審查委員', N'D26', CAST(N'2024-05-31T08:35:42.543' AS DateTime), N'F01885', N'許瓊中', NULL, NULL, NULL, 0, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (39, 2, N'臺北市政府環境保護局', N'E01', CAST(N'2024-05-31T09:48:10.630' AS DateTime), N'F01885', N'許瓊中', NULL, NULL, NULL, 0, N'Y', 15)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (40, 2, N'臺南市政府環境保護局', N'E03', CAST(N'2024-05-31T10:06:39.287' AS DateTime), N'F01885', N'許瓊中', CAST(N'2024-05-31T10:58:18.650' AS DateTime), N'F01885', N'許瓊中', 0, N'Y', 16)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (41, 2, N'臺北市政府工務局', N'E02', CAST(N'2024-05-31T10:59:22.397' AS DateTime), N'F01885', N'許瓊中', CAST(N'2024-05-31T10:59:53.430' AS DateTime), N'F01885', N'許瓊中', 0, N'Y', 17)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (42, 2, N'教育部', N'E04', CAST(N'2024-05-31T11:27:56.413' AS DateTime), N'F01885', N'許瓊中', NULL, NULL, NULL, 0, N'Y', 18)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (43, 1, N'建構智慧低碳校園計畫說明會教育部推薦之講師', N'D27', CAST(N'2024-06-06T13:52:24.717' AS DateTime), N'F01885', N'許瓊中', NULL, NULL, NULL, 0, N'Y', 7)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (44, 1, N'業主指定人選', N'D28', CAST(N'2024-06-13T08:05:44.890' AS DateTime), N'F01885', N'許瓊中', CAST(N'2024-06-21T11:17:53.590' AS DateTime), N'F01885', N'許瓊中', 0, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (45, 1, N'一般例行會議', N'D29', CAST(N'2024-06-13T08:05:56.720' AS DateTime), N'F01885', N'許瓊中', CAST(N'2024-06-21T11:06:12.640' AS DateTime), N'F01885', N'許瓊中', 0, N'Y', 2)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (46, 2, N'行政院農業委員會', N'E05', CAST(N'2024-06-13T08:38:25.133' AS DateTime), N'F01885', N'許瓊中', NULL, NULL, NULL, 0, N'Y', 19)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (47, 1, N'公司內部會議', N'D30', CAST(N'2024-06-21T11:34:54.717' AS DateTime), N'F01885', N'許瓊中', CAST(N'2024-06-26T16:18:09.890' AS DateTime), N'F01885', N'許瓊中', 0, N'Y', 2)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (48, 1, N'性別友善廁所專家諮詢會', N'D31', CAST(N'2024-06-28T14:29:22.870' AS DateTime), N'F01885', N'許瓊中', NULL, NULL, NULL, 0, N'Y', 3)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (51, 1, N'定期/臨時工作會議', N'D34', CAST(N'2024-07-04T14:27:50.023' AS DateTime), N'F01885', N'許瓊中', CAST(N'2024-07-04T17:03:03.177' AS DateTime), N'F01879', N'陳宇揚', 0, N'Y', 2)
SET IDENTITY_INSERT [dbo].[ActivityCategory_temp_old] OFF
GO

--驗證轉換
Select a.Id, a.Name, b.Id, b.Name From ActivityCategory a
Left Join ActivityCategory_temp_old b On a.Id = b.Id

----Select a.Id, a.Name, b.Id, b.Name From ActivityCategory_temp_old a Left Join ActivityCategory_temp_new b On a.MapNewId = b.Id

--4.New 取代
--Select * From ActivityCategory
Delete ActivityCategory DBCC CHECKIDENT('ActivityCategory', RESEED, 0)
GO

SET IDENTITY_INSERT [dbo].[ActivityCategory] ON 

INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (1, 1, N'計畫審查', N'計畫審查(評選/進度報告/期中/期末/分包評選等)', N'D01', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 1, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (2, 1, N'定期/臨時工作會議', N'定期/臨時工作會議', N'D02', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 2, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (3, 1, N'專案諮詢/學習會議', N'專案諮詢/學習會議(專諮/研商/講習/說明/研討會等)', N'D03', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 3, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (4, 1, N'審查/審議會議', N'審查/審議會議(現勘、評鑑、綠色工廠、工程督導、技審、期刊、手冊、獎項、題庫、石油基金退費等)', N'D04', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 4, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (5, 1, N'輔導/查核/訪談', N'輔導/查核/訪談', N'D05', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 5, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (6, 1, N'活動辦理', N'活動辦理(參訪、記者會、頒獎等各類活動)', N'D06', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 6, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (7, 1, N'訓練/培訓班', N'訓練/培訓班', N'D07', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 7, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (8, 1, N'計畫工作分包', N'計畫工作分包', N'D08', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 8, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (9, 1, N'計畫顧問', N'計畫顧問', N'D09', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 9, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (10, 1, N'期刊撰稿', N'期刊撰稿', N'D10', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 10, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (11, 1, N'業主制定委員名冊', N'業主制定委員名冊', N'D11', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 11, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (12, 2, N'環境部', N'', N'A0', NULL, N'J00007', N'林正祥', NULL, N'J00007', N'林正祥', 1, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (13, 2, N'能源署', N'', N'B0', NULL, N'J00007', N'林正祥', NULL, N'J00007', N'林正祥', 2, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (14, 2, N'產發署', N'', N'C0', NULL, N'J00007', N'林正祥', NULL, N'J00007', N'林正祥', 3, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (15, 2, N'臺北市政府環境保護局', N'', N'E01', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 0, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (16, 2, N'臺南市政府環境保護局', N'', N'E03', NULL, N'F01885', N'許瓊中', NULL, N'F01885', N'許瓊中', 0, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (17, 2, N'臺北市政府工務局', N'', N'E02', NULL, N'F01885', N'許瓊中', NULL, N'F01885', N'許瓊中', 0, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (18, 2, N'教育部', N'', N'E04', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 0, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (19, 2, N'行政院農業委員會', N'', N'E05', NULL, N'F01885', N'許瓊中', NULL, NULL, NULL, 0, N'Y')
SET IDENTITY_INSERT [dbo].[ActivityCategory] OFF
GO

--5.轉檔公式(FTISUserHistory 注意：會外Type=2和其他) 需整理沒對應的資料
Select Id, ActivityCategoryType From FTISUserHistory

Update FTISUserHistory
Set ActivityCategoryId = b.MapNewId
From FTISUserHistory a
Left Join ActivityCategory_temp_old b On a.ActivityCategoryId = b.Id

--5.網站驗證(本機資料與正式站)

