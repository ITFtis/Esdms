--�|ĳ��ƭץ�
--Select * From ActivityCategory Order By Id Desc
--Select * From ActivityCategory_temp_old
--Select * From ActivityCategory_temp_new




--Delete ActivityCategory_temp_new DBCC CHECKIDENT('ActivityCategory_temp_new', RESEED, 0) 

--SELECT  TOP (200) Id, Type, Name, MapNewId 
--FROM     ActivityCategory_temp_old
--WHERE   (Type = 1) AND (Name = '���ʿ�z')

----------------------------------------------------------------------

--1.�s�W��ƪ�+���(ActivityCategory_temp_old) �h�@�ӹ������ map_temp_new_Id
--2.�s�W��ƪ�+���(ActivityCategory_temp_new)
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
/****** Object:  Table [dbo].[ActivityCategory_temp_old]    Script Date: 2024/7/10 �U�� 03:38:43 ******/
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

INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (1, 1, N'�p�e�f�d', N'�p�e�f�d(����/�i�׳��i/����/����/���]���ﵥ)', N'D01', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 1, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (2, 1, N'�w��/�{�ɤu�@�|ĳ', N'�w��/�{�ɤu�@�|ĳ', N'D02', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 2, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (3, 1, N'�M�׿Ը�/�ǲ߷|ĳ', N'�M�׿Ը�/�ǲ߷|ĳ(�M��/���/����/����/��Q�|��)', N'D03', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 3, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (4, 1, N'�f�d/�fĳ�|ĳ', N'�f�d/�fĳ�|ĳ(�{�ɡB��Ų�B���u�t�B�u�{���ɡB�޼f�B���Z�B��U�B�����B�D�w�B�۪o����h�O��)', N'D04', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 4, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (5, 1, N'����/�d��/�X��', N'����/�d��/�X��', N'D05', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 5, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (6, 1, N'���ʿ�z', N'���ʿ�z(�ѳX�B�O�̷|�B�{�����U������)', N'D06', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 6, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (7, 1, N'�V�m/���V�Z', N'�V�m/���V�Z', N'D07', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 7, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (8, 1, N'�p�e�u�@���]', N'�p�e�u�@���]', N'D08', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 8, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (9, 1, N'�p�e�U��', N'�p�e�U��', N'D09', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 9, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (10, 1, N'���Z���Z', N'���Z���Z', N'D10', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 10, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (11, 1, N'�~�D��w�e���W�U', N'�~�D��w�e���W�U', N'D11', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 11, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (12, 2, N'���ҳ�', N'', N'A0', NULL, N'J00007', N'�L����', NULL, N'J00007', N'�L����', 1, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (13, 2, N'�෽�p', N'', N'B0', NULL, N'J00007', N'�L����', NULL, N'J00007', N'�L����', 2, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (14, 2, N'���o�p', N'', N'C0', NULL, N'J00007', N'�L����', NULL, N'J00007', N'�L����', 3, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (15, 2, N'�O�_���F�����ҫO�@��', N'', N'E01', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 0, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (16, 2, N'�O�n���F�����ҫO�@��', N'', N'E03', NULL, N'F01885', N'�\ã��', NULL, N'F01885', N'�\ã��', 0, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (17, 2, N'�O�_���F���u�ȧ�', N'', N'E02', NULL, N'F01885', N'�\ã��', NULL, N'F01885', N'�\ã��', 0, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (18, 2, N'�Ш|��', N'', N'E04', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 0, N'Y')
INSERT [dbo].[ActivityCategory_temp_new] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (19, 2, N'��F�|�A�~�e���|', N'', N'E05', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 0, N'Y')
SET IDENTITY_INSERT [dbo].[ActivityCategory_temp_new] OFF
GO

SET IDENTITY_INSERT [dbo].[ActivityCategory_temp_old] ON 

INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (4, 1, N'����|ĳ', N'D01', NULL, NULL, NULL, CAST(N'2024-06-21T11:21:43.427' AS DateTime), N'F01885', N'�\ã��', 1, N'Y', 1)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (10, 1, N'����/���f�d�|ĳ', N'D02', CAST(N'2024-01-10T10:09:47.497' AS DateTime), N'F01879', N'���t��', CAST(N'2024-06-21T11:21:48.067' AS DateTime), N'F01885', N'�\ã��', 2, N'Y', 1)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (11, 1, N'�M��/���/����/��Q�|ĳ', N'D03', CAST(N'2024-01-10T10:10:26.577' AS DateTime), N'F01879', N'���t��', CAST(N'2024-07-04T17:01:13.983' AS DateTime), N'F01879', N'���t��', 3, N'Y', 3)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (12, 1, N'�޼f�|ĳ', N'D04', CAST(N'2024-01-10T10:10:46.367' AS DateTime), N'F01879', N'���t��', CAST(N'2024-06-21T11:22:21.383' AS DateTime), N'F01885', N'�\ã��', 4, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (13, 1, N'����/�d��', N'D05', CAST(N'2024-01-10T10:11:08.330' AS DateTime), N'F01879', N'���t��', CAST(N'2024-07-04T16:59:02.890' AS DateTime), N'F01879', N'���t��', 5, N'Y', 5)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (14, 1, N'�ҵ{���v', N'D06', CAST(N'2024-01-10T10:11:18.077' AS DateTime), N'F01879', N'���t��', CAST(N'2024-06-21T11:22:10.177' AS DateTime), N'F01885', N'�\ã��', 6, N'Y', 7)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (15, 2, N'���ҳ�', N'A0', CAST(N'2024-01-18T09:53:30.417' AS DateTime), N'J00007', N'�L����', CAST(N'2024-04-03T19:01:40.570' AS DateTime), N'J00007', N'�L����', 1, N'Y', 12)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (16, 2, N'�෽�p', N'B0', CAST(N'2024-01-18T09:54:41.953' AS DateTime), N'J00007', N'�L����', CAST(N'2024-04-03T19:01:48.013' AS DateTime), N'J00007', N'�L����', 2, N'Y', 13)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (17, 2, N'���o�p', N'C0', CAST(N'2024-01-18T09:54:55.500' AS DateTime), N'J00007', N'�L����', CAST(N'2024-04-03T19:01:54.850' AS DateTime), N'J00007', N'�L����', 3, N'Y', 14)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (18, 1, N'�p�e���]', N'D07', CAST(N'2024-03-11T13:31:59.963' AS DateTime), N'F01879', N'���t��', CAST(N'2024-06-21T11:22:06.710' AS DateTime), N'F01885', N'�\ã��', 7, N'Y', 8)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (19, 1, N'�p�e�U��', N'D08', CAST(N'2024-03-11T17:14:00.013' AS DateTime), N'F01879', N'���t��', CAST(N'2024-06-21T11:22:03.537' AS DateTime), N'F01885', N'�\ã��', 8, N'Y', 9)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (20, 1, N'113�~���o�p�p�e�e����U', N'D09', CAST(N'2024-04-01T16:37:22.870' AS DateTime), N'F01885', N'�\ã��', CAST(N'2024-07-08T10:25:26.060' AS DateTime), N'J00007', N'�L����', 9, N'N', 11)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (21, 1, N'���ҳ������f�d�p�թe���W��', N'D10', CAST(N'2024-04-02T17:31:27.377' AS DateTime), N'F01885', N'�\ã��', CAST(N'2024-07-08T10:25:36.417' AS DateTime), N'J00007', N'�L����', 10, N'N', 11)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (22, 1, N'���]����', N'D11', CAST(N'2024-04-02T17:31:33.713' AS DateTime), N'F01885', N'�\ã��', CAST(N'2024-04-03T19:00:11.180' AS DateTime), N'J00007', N'�L����', 11, N'Y', 1)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (23, 1, N'���u�t�fĳ�|', N'D12', CAST(N'2024-04-02T17:31:39.343' AS DateTime), N'F01885', N'�\ã��', CAST(N'2024-04-03T19:00:22.560' AS DateTime), N'J00007', N'�L����', 12, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (24, 1, N'���Z�X��', N'D13', CAST(N'2024-04-02T17:31:44.243' AS DateTime), N'F01885', N'�\ã��', CAST(N'2024-04-03T19:00:32.753' AS DateTime), N'J00007', N'�L����', 13, N'Y', 5)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (25, 1, N'���Z���Z�H', N'D14', CAST(N'2024-04-02T17:31:49.153' AS DateTime), N'F01885', N'�\ã��', CAST(N'2024-04-03T19:00:41.603' AS DateTime), N'J00007', N'�L����', 14, N'Y', 10)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (26, 1, N'���Z�f�d�e��(��U�f�d)', N'D15', CAST(N'2024-04-02T17:31:55.773' AS DateTime), N'F01885', N'�\ã��', CAST(N'2024-05-24T14:03:09.923' AS DateTime), N'F01885', N'�\ã��', 15, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (27, 1, N'�����f�d�e��', N'D16', CAST(N'2024-04-02T17:32:04.440' AS DateTime), N'F01885', N'�\ã��', CAST(N'2024-04-03T19:01:08.330' AS DateTime), N'J00007', N'�L����', 16, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (28, 1, N'��Ų�e��', N'D17', CAST(N'2024-04-02T17:32:10.740' AS DateTime), N'F01885', N'�\ã��', CAST(N'2024-04-03T19:01:16.110' AS DateTime), N'J00007', N'�L����', 17, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (29, 1, N'���ҳ��ҶO�O�v�fĳ�|�e���W��', N'D18', CAST(N'2024-04-02T17:32:16.497' AS DateTime), N'F01885', N'�\ã��', CAST(N'2024-07-08T10:25:51.543' AS DateTime), N'J00007', N'�L����', 18, N'N', 11)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (30, 1, N'�X�t�����U��', N'D19', CAST(N'2024-04-22T09:15:26.237' AS DateTime), N'F01885', N'�\ã��', CAST(N'2024-04-22T09:15:50.303' AS DateTime), N'F01885', N'�\ã��', 19, N'Y', 5)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (31, 1, N'�M�~�D�w�f�d', N'D20', CAST(N'2024-04-25T11:38:09.070' AS DateTime), N'F01885', N'�\ã��', NULL, NULL, NULL, 20, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (32, 1, N'�u�{����', N'D21', CAST(N'2024-04-25T11:38:19.307' AS DateTime), N'F01885', N'�\ã��', NULL, NULL, NULL, 21, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (33, 1, N'���ɻ����|���v', N'D22', CAST(N'2024-04-25T11:50:23.347' AS DateTime), N'F01885', N'�\ã��', NULL, NULL, NULL, 22, N'Y', 7)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (35, 1, N'�fĳ�|ĳ', N'D23', CAST(N'2024-05-13T08:04:02.037' AS DateTime), N'F01885', N'�\ã��', NULL, NULL, NULL, 23, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (36, 1, N'�۪o����h�O�f�d', N'D24', CAST(N'2024-05-13T10:59:11.027' AS DateTime), N'F01885', N'�\ã��', NULL, NULL, NULL, 24, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (37, 1, N'���ʿ�z', N'D25', CAST(N'2024-05-29T10:56:53.713' AS DateTime), N'F01885', N'�\ã��', NULL, NULL, NULL, 0, N'Y', 6)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (38, 1, N'�H�ϭӸ��F�լd�f�d�e��', N'D26', CAST(N'2024-05-31T08:35:42.543' AS DateTime), N'F01885', N'�\ã��', NULL, NULL, NULL, 0, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (39, 2, N'�O�_���F�����ҫO�@��', N'E01', CAST(N'2024-05-31T09:48:10.630' AS DateTime), N'F01885', N'�\ã��', NULL, NULL, NULL, 0, N'Y', 15)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (40, 2, N'�O�n���F�����ҫO�@��', N'E03', CAST(N'2024-05-31T10:06:39.287' AS DateTime), N'F01885', N'�\ã��', CAST(N'2024-05-31T10:58:18.650' AS DateTime), N'F01885', N'�\ã��', 0, N'Y', 16)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (41, 2, N'�O�_���F���u�ȧ�', N'E02', CAST(N'2024-05-31T10:59:22.397' AS DateTime), N'F01885', N'�\ã��', CAST(N'2024-05-31T10:59:53.430' AS DateTime), N'F01885', N'�\ã��', 0, N'Y', 17)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (42, 2, N'�Ш|��', N'E04', CAST(N'2024-05-31T11:27:56.413' AS DateTime), N'F01885', N'�\ã��', NULL, NULL, NULL, 0, N'Y', 18)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (43, 1, N'�غc���z�C�Үն�p�e�����|�Ш|�����ˤ����v', N'D27', CAST(N'2024-06-06T13:52:24.717' AS DateTime), N'F01885', N'�\ã��', NULL, NULL, NULL, 0, N'Y', 7)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (44, 1, N'�~�D���w�H��', N'D28', CAST(N'2024-06-13T08:05:44.890' AS DateTime), N'F01885', N'�\ã��', CAST(N'2024-06-21T11:17:53.590' AS DateTime), N'F01885', N'�\ã��', 0, N'Y', 4)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (45, 1, N'�@��Ҧ�|ĳ', N'D29', CAST(N'2024-06-13T08:05:56.720' AS DateTime), N'F01885', N'�\ã��', CAST(N'2024-06-21T11:06:12.640' AS DateTime), N'F01885', N'�\ã��', 0, N'Y', 2)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (46, 2, N'��F�|�A�~�e���|', N'E05', CAST(N'2024-06-13T08:38:25.133' AS DateTime), N'F01885', N'�\ã��', NULL, NULL, NULL, 0, N'Y', 19)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (47, 1, N'���q�����|ĳ', N'D30', CAST(N'2024-06-21T11:34:54.717' AS DateTime), N'F01885', N'�\ã��', CAST(N'2024-06-26T16:18:09.890' AS DateTime), N'F01885', N'�\ã��', 0, N'Y', 2)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (48, 1, N'�ʧO�͵��Z�ұM�a�Ը߷|', N'D31', CAST(N'2024-06-28T14:29:22.870' AS DateTime), N'F01885', N'�\ã��', NULL, NULL, NULL, 0, N'Y', 3)
INSERT [dbo].[ActivityCategory_temp_old] ([Id], [Type], [Name], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount], [MapNewId]) VALUES (51, 1, N'�w��/�{�ɤu�@�|ĳ', N'D34', CAST(N'2024-07-04T14:27:50.023' AS DateTime), N'F01885', N'�\ã��', CAST(N'2024-07-04T17:03:03.177' AS DateTime), N'F01879', N'���t��', 0, N'Y', 2)
SET IDENTITY_INSERT [dbo].[ActivityCategory_temp_old] OFF
GO

--�����ഫ
Select a.Id, a.Name, b.Id, b.Name From ActivityCategory a
Left Join ActivityCategory_temp_old b On a.Id = b.Id

----Select a.Id, a.Name, b.Id, b.Name From ActivityCategory_temp_old a Left Join ActivityCategory_temp_new b On a.MapNewId = b.Id

--4.New ���N
--Select * From ActivityCategory
Delete ActivityCategory DBCC CHECKIDENT('ActivityCategory', RESEED, 0)
GO

SET IDENTITY_INSERT [dbo].[ActivityCategory] ON 

INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (1, 1, N'�p�e�f�d', N'�p�e�f�d(����/�i�׳��i/����/����/���]���ﵥ)', N'D01', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 1, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (2, 1, N'�w��/�{�ɤu�@�|ĳ', N'�w��/�{�ɤu�@�|ĳ', N'D02', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 2, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (3, 1, N'�M�׿Ը�/�ǲ߷|ĳ', N'�M�׿Ը�/�ǲ߷|ĳ(�M��/���/����/����/��Q�|��)', N'D03', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 3, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (4, 1, N'�f�d/�fĳ�|ĳ', N'�f�d/�fĳ�|ĳ(�{�ɡB��Ų�B���u�t�B�u�{���ɡB�޼f�B���Z�B��U�B�����B�D�w�B�۪o����h�O��)', N'D04', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 4, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (5, 1, N'����/�d��/�X��', N'����/�d��/�X��', N'D05', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 5, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (6, 1, N'���ʿ�z', N'���ʿ�z(�ѳX�B�O�̷|�B�{�����U������)', N'D06', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 6, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (7, 1, N'�V�m/���V�Z', N'�V�m/���V�Z', N'D07', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 7, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (8, 1, N'�p�e�u�@���]', N'�p�e�u�@���]', N'D08', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 8, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (9, 1, N'�p�e�U��', N'�p�e�U��', N'D09', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 9, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (10, 1, N'���Z���Z', N'���Z���Z', N'D10', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 10, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (11, 1, N'�~�D��w�e���W�U', N'�~�D��w�e���W�U', N'D11', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 11, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (12, 2, N'���ҳ�', N'', N'A0', NULL, N'J00007', N'�L����', NULL, N'J00007', N'�L����', 1, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (13, 2, N'�෽�p', N'', N'B0', NULL, N'J00007', N'�L����', NULL, N'J00007', N'�L����', 2, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (14, 2, N'���o�p', N'', N'C0', NULL, N'J00007', N'�L����', NULL, N'J00007', N'�L����', 3, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (15, 2, N'�O�_���F�����ҫO�@��', N'', N'E01', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 0, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (16, 2, N'�O�n���F�����ҫO�@��', N'', N'E03', NULL, N'F01885', N'�\ã��', NULL, N'F01885', N'�\ã��', 0, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (17, 2, N'�O�_���F���u�ȧ�', N'', N'E02', NULL, N'F01885', N'�\ã��', NULL, N'F01885', N'�\ã��', 0, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (18, 2, N'�Ш|��', N'', N'E04', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 0, N'Y')
INSERT [dbo].[ActivityCategory] ([Id], [Type], [Name], [FullName], [DCode], [BDate], [BFno], [BName], [UDate], [UFno], [UName], [Sort], [IsCount]) VALUES (19, 2, N'��F�|�A�~�e���|', N'', N'E05', NULL, N'F01885', N'�\ã��', NULL, NULL, NULL, 0, N'Y')
SET IDENTITY_INSERT [dbo].[ActivityCategory] OFF
GO

--5.���ɤ���(FTISUserHistory �`�N�G�|�~Type=2�M��L) �ݾ�z�S���������
Select Id, ActivityCategoryType From FTISUserHistory

Update FTISUserHistory
Set ActivityCategoryId = b.MapNewId
From FTISUserHistory a
Left Join ActivityCategory_temp_old b On a.ActivityCategoryId = b.Id

--5.��������(������ƻP������)

