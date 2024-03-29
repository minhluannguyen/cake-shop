USE [master]
GO
/****** Object:  Database [CakeStoreDB]    Script Date: 05-Jan-21 11:47:32 PM ******/
CREATE DATABASE [CakeStoreDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CakeStoreDB', FILENAME = N'F:\Software\SQL Server 2019\MSSQL15.MSSQLSERVER\MSSQL\DATA\CakeStoreDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CakeStoreDB_log', FILENAME = N'F:\Software\SQL Server 2019\MSSQL15.MSSQLSERVER\MSSQL\DATA\CakeStoreDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [CakeStoreDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CakeStoreDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CakeStoreDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CakeStoreDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CakeStoreDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CakeStoreDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CakeStoreDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [CakeStoreDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [CakeStoreDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CakeStoreDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CakeStoreDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CakeStoreDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CakeStoreDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CakeStoreDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CakeStoreDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CakeStoreDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CakeStoreDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [CakeStoreDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CakeStoreDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CakeStoreDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CakeStoreDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CakeStoreDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CakeStoreDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CakeStoreDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CakeStoreDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CakeStoreDB] SET  MULTI_USER 
GO
ALTER DATABASE [CakeStoreDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CakeStoreDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CakeStoreDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CakeStoreDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CakeStoreDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'CakeStoreDB', N'ON'
GO
ALTER DATABASE [CakeStoreDB] SET QUERY_STORE = OFF
GO
USE [CakeStoreDB]
GO
/****** Object:  Table [dbo].[CakeImportOrder]    Script Date: 05-Jan-21 11:47:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CakeImportOrder](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ImportOrderName] [nvarchar](50) NULL,
	[ProductID] [int] NULL,
	[ImageOrder] [nvarchar](50) NULL,
	[ImportDate] [datetime] NULL,
	[ExpirationDate] [datetime] NULL,
	[Quantity] [int] NULL,
	[GoodsAmount] [int] NULL,
	[Remainder] [int] NULL,
 CONSTRAINT [PK_CakeImportOrder] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 05-Jan-21 11:47:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Name] [nvarchar](10) NULL,
	[PhoneNumber] [int] NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[PhoneNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 05-Jan-21 11:47:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PhoneCustomer] [int] NULL,
	[Amount] [int] NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 05-Jan-21 11:47:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[ID_Order] [int] NOT NULL,
	[ID_Cake] [int] NOT NULL,
	[UnitPrice] [int] NULL,
	[Quantity] [int] NULL,
	[Amount] [int] NULL,
	[No] [int] NULL,
 CONSTRAINT [PK_OrderDetail] PRIMARY KEY CLUSTERED 
(
	[ID_Order] ASC,
	[ID_Cake] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 05-Jan-21 11:47:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDTypeCake] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](500) NULL,
	[Price] [int] NULL,
	[Amount] [int] NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductImages]    Script Date: 05-Jan-21 11:47:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductImages](
	[ID_Product] [int] NOT NULL,
	[ImageName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_ProductImages] PRIMARY KEY CLUSTERED 
(
	[ID_Product] ASC,
	[ImageName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypeCake]    Script Date: 05-Jan-21 11:47:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeCake](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NameTypeCake] [nvarchar](50) NULL,
 CONSTRAINT [PK_TypeCake] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (1, 2, N'Nhung đỏ và cây thông Noel', N'
Bánh sô cô la nhẹ với lớp kem phủ dừa và cây thông Noel', 40000, 20)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (2, 2, N'Yogurt dâu trang trí tuần lộc', N'Sữa chua và bánh dâu tây tươi với lớp phủ kem dâu tây trang trí bằng 1 chú tuần lộc nhỏ đáng yêu.', 40000, 20)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (3, 8, N'Mochi Socola', N'Bánh Mochi Nhật Bản hương socola', 36000, 50)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (4, 8, N'Mochi Dâu tây', N'Bánh Mochi Nhật Bản hương dâu tây', 36000, 50)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (5, 8, N'Mochi Khoai môn', N'Bánh Mochi Nhật Bản hương khoai môn', 36000, 50)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (6, 8, N'Mochi Kem cam', N'Bánh Mochi Nhật Bản hương cam', 36000, 50)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (7, 9, N'Tiramisu kem phô mai', N'Bánh được làm bằng pho mát kem, và là sự kết hợp hoàn hảo cho bất kỳ bữa ăn tuyệt vời nào. Kem, dễ chịu và cực kỳ ngon! Bạn biết những gì họ nói: "để lại những gì tốt nhất cho cuối cùng!"', 50000, 15)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (9, 9, N'Tiramisu truyền thống', N'Món tráng miệng cổ điển của Ý với nhiều lớp pho mát kem ngọt ngào, bánh bông lan tinh tế, cà phê và rượu rum.', 45000, 15)
SET IDENTITY_INSERT [dbo].[Product] OFF
INSERT [dbo].[ProductImages] ([ID_Product], [ImageName]) VALUES (3, N'13-1200x676-1.jpg')
INSERT [dbo].[ProductImages] ([ID_Product], [ImageName]) VALUES (3, N'cach-lam-banh-mochi-socola-6.jpg')
INSERT [dbo].[ProductImages] ([ID_Product], [ImageName]) VALUES (3, N'cach-lam-banh-mochi-socola-mem-thom-ngot-nhe-khong-gay-beo-1391.jpg')
INSERT [dbo].[ProductImages] ([ID_Product], [ImageName]) VALUES (7, N'2dc58c2e-3d2d-4797-8866-a21964b55921.jpg')
INSERT [dbo].[ProductImages] ([ID_Product], [ImageName]) VALUES (9, N'3e45767b-9f7f-4818-b2b7-575e40c496cls.jpg')
SET IDENTITY_INSERT [dbo].[TypeCake] ON 

INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (1, N'Bread')
INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (2, N'Cupcake & Muffin')
INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (3, N'Cookies')
INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (4, N'Loaf Cake')
INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (5, N'Puddings')
INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (6, N'Buns')
INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (8, N'Mochi')
INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (9, N'Tiramisu')
SET IDENTITY_INSERT [dbo].[TypeCake] OFF
ALTER TABLE [dbo].[CakeImportOrder]  WITH CHECK ADD  CONSTRAINT [FK_CakeImportOrder_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ID])
GO
ALTER TABLE [dbo].[CakeImportOrder] CHECK CONSTRAINT [FK_CakeImportOrder_Product]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer] FOREIGN KEY([PhoneCustomer])
REFERENCES [dbo].[Customer] ([PhoneNumber])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Customer]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Order] FOREIGN KEY([ID_Order])
REFERENCES [dbo].[Order] ([ID])
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Order]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Product] FOREIGN KEY([ID_Cake])
REFERENCES [dbo].[Product] ([ID])
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Product]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_TypeCake] FOREIGN KEY([IDTypeCake])
REFERENCES [dbo].[TypeCake] ([ID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_TypeCake]
GO
ALTER TABLE [dbo].[ProductImages]  WITH CHECK ADD  CONSTRAINT [FK_ProductImages_Product] FOREIGN KEY([ID_Product])
REFERENCES [dbo].[Product] ([ID])
GO
ALTER TABLE [dbo].[ProductImages] CHECK CONSTRAINT [FK_ProductImages_Product]
GO
USE [master]
GO
ALTER DATABASE [CakeStoreDB] SET  READ_WRITE 
GO
