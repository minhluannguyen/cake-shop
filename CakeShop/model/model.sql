USE [master]
GO
/****** Object:  Database [CakeStoreDB]    Script Date: 1/10/2021 3:36:30 PM ******/
CREATE DATABASE [CakeStoreDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CakeStoreDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\CakeStoreDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CakeStoreDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\CakeStoreDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
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
ALTER DATABASE [CakeStoreDB] SET QUERY_STORE = OFF
GO
USE [CakeStoreDB]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = OFF;
GO
USE [CakeStoreDB]
GO
/****** Object:  Table [dbo].[CakeImportOrder]    Script Date: 1/10/2021 3:36:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CakeImportOrder](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ImportOrderName] [nvarchar](50) NULL,
	[ProductID] [int] NULL,
	[ImportDate] [datetime] NULL,
	[ExpirationDate] [datetime] NULL,
	[Quantity] [int] NULL,
	[ImportPrice] [int] NULL,
	[Total] [int] NULL,
 CONSTRAINT [PK_CakeImportOrder] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 1/10/2021 3:36:30 PM ******/
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
/****** Object:  Table [dbo].[Order]    Script Date: 1/10/2021 3:36:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[ID] [int] NOT NULL,
	[PhoneCustomer] [int] NULL,
	[Amount] [int] NULL,
	[Status] [int] NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_Order_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 1/10/2021 3:36:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[ID] [int] NOT NULL,
	[ID_Order] [int] NULL,
	[ID_Cake] [int] NULL,
	[Quantity] [int] NULL,
	[Amount] [int] NULL,
	[No] [int] NULL,
 CONSTRAINT [PK_OD] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 1/10/2021 3:36:30 PM ******/
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
/****** Object:  Table [dbo].[ProductImages]    Script Date: 1/10/2021 3:36:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductImages](
	[ID] [int] NOT NULL,
	[ID_Product] [int] NOT NULL,
	[ImageName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ProductImages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypeCake]    Script Date: 1/10/2021 3:36:30 PM ******/
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

INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (1, 2, N'Red Velvet', N'Bánh sô cô la nhẹ với lớp kem phủ dừa và cây thông Noel', 40000, 20)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (2, 2, N'Yogurt dâu', N'Sữa chua và bánh dâu tây tươi với lớp phủ kem dâu tây trang trí bằng 1 chú tuần lộc nhỏ đáng yêu.', 40000, 20)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (3, 8, N'Mochi Socola', N'Bánh Mochi Nhật Bản hương socola', 36000, 50)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (4, 8, N'Mochi Dâu tây', N'Bánh Mochi Nhật Bản hương dâu tây', 36000, 50)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (5, 8, N'Mochi Khoai môn', N'Bánh Mochi Nhật Bản hương khoai môn', 36000, 50)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (6, 8, N'Mochi Kem cam', N'Bánh Mochi Nhật Bản hương cam', 36000, 50)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (7, 9, N'Tiramisu kem phô mai', N'Bánh được làm bằng pho mát kem, và là sự kết hợp hoàn hảo cho bất kỳ bữa ăn tuyệt vời nào. Kem, dễ chịu và cực kỳ ngon! Bạn biết những gì họ nói: "để lại những gì tốt nhất cho cuối cùng!"', 50000, 15)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (9, 9, N'Tiramisu truyền thống', N'Món tráng miệng cổ điển của Ý với nhiều lớp pho mát kem ngọt ngào, bánh bông lan tinh tế, cà phê và rượu rum.', 45000, 15)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (20, 6, N'Bánh Mỳ Canxi Trẻ Em', N'Đã từ lâu, rất nhiều khách hàng thân quý của La Vita Bakery không ngớt nhắn tin, gọi điện thậm chí chia sẻ những tâm sự dài về một mong muốn có dòng sản phẩm bánh dành cho các bé đang thời kỳ ăn dặm hoặc các mẹ bầu, những người đang nâng niu đứa con yêu quý của mình trong bụng. Họ tìm kiếm một dòng bánh vừa ngon, mượt mà dễ ăn lại cung cấp đầy đủ các dưỡng chất thiết yếu nhất cho trẻ nhỏ và mẹ bầu trong giai đoạn phát triển mạnh mẽ của thể chất lẫn trí lực.', 81000, 0)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (21, 32, N'Carrot Birthday Cake', N'Đây là loại bánh có truyền thống lâu đời, được cho là có nguồn gốc từ Thụy Điển và trở nên nổi tiếng ở Mỹ vào khoảng những năm 1960, cho đến bây giờ nó vẫn là một trong những loại bánh được yêu thích trên thế giới, đặc biệt là ở Anh.', 590000, 0)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (22, 33, N'The Log Cake', N'Nhân bánh có vị đắng đặc trưng của lớp chocolate đen hảo hạng cùng hương thơm nồng rất êm và sâu của bột cacao. Bên ngoài phủ bởi lớp kem cheese chua nhẹ của Pháp. Tổng hoà hương vị dễ dàng khiến bạn mỉm cười ngay từ miếng bánh đầu tiên, kể cả với những ai vốn không phải tín đồ của bánh ngọt.', 680000, 0)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (23, 32, N'Tarzan Cake', N'', 720000, 0)
INSERT [dbo].[Product] ([ID], [IDTypeCake], [Name], [Description], [Price], [Amount]) VALUES (24, 32, N'Red Velvet Birthday Cake', N'Red Velvet thường có nhiều lớp, lớp nhung đỏ thẫm kiêu sa kết hợp cùng lớp kem trắng mịn phủ bên ngoài nổi bật thu hút mọi ánh nhìn. Một chiếc bánh Red Velvet ngon đúng điệu không chỉ mang màu sắc lộng lẫy mà cốt bánh phải đủ ẩm, mềm mượt như nhung, hơi chua dịu và thoang thoảng hương chocolate. Lớp kem phô mai mát lạnh, được đánh vừa độ để bông mềm mà vẫn giữ được độ ngậy béo. Tất cả những điều đó hòa quyện, tạo nên dấu ấn đặc trưng cho Red Velvet.', 550000, 0)
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
INSERT [dbo].[ProductImages] ([ID], [ID_Product], [ImageName]) VALUES (0, 20, N'12b2afa7-8536-49f5-91d1-0fc126754e24.jpeg')
INSERT [dbo].[ProductImages] ([ID], [ID_Product], [ImageName]) VALUES (1, 22, N'b50041b8-4fa6-41ef-b80e-85be73caefa7.jpeg')
INSERT [dbo].[ProductImages] ([ID], [ID_Product], [ImageName]) VALUES (2, 23, N'7b3fc1e9-3610-4599-ace7-3b98e1875b40.jpeg')
INSERT [dbo].[ProductImages] ([ID], [ID_Product], [ImageName]) VALUES (3, 21, N'8a705c85-d2d8-41a9-892e-cd2305446cf3.jpeg')
INSERT [dbo].[ProductImages] ([ID], [ID_Product], [ImageName]) VALUES (4, 24, N'55883f0a-0e20-4f8d-a9c2-89e6d86521d7.jpeg')
INSERT [dbo].[ProductImages] ([ID], [ID_Product], [ImageName]) VALUES (5, 24, N'7d7e23fc-f09f-4164-a5fb-007d1d5b17a7.jpeg')
INSERT [dbo].[ProductImages] ([ID], [ID_Product], [ImageName]) VALUES (6, 24, N'870e61fb-4980-4f6a-a908-36ad889fac58.jpeg')
INSERT [dbo].[ProductImages] ([ID], [ID_Product], [ImageName]) VALUES (8, 2, N'2dbc630a-333d-4135-8a97-4bbe9bdc7c84.png')
INSERT [dbo].[ProductImages] ([ID], [ID_Product], [ImageName]) VALUES (9, 3, N'8af6680a-8da8-4e1d-8a06-42b1738bd6d4.jpg')
INSERT [dbo].[ProductImages] ([ID], [ID_Product], [ImageName]) VALUES (10, 4, N'ee721139-0c2e-42b0-a722-ff193948e345.jpg')
INSERT [dbo].[ProductImages] ([ID], [ID_Product], [ImageName]) VALUES (11, 5, N'a18be794-3eb7-4aa2-9b27-eeadd01ae7f0.jpg')
INSERT [dbo].[ProductImages] ([ID], [ID_Product], [ImageName]) VALUES (12, 6, N'be36c256-ca29-4000-a7f4-22f0f8e1b3f5.jpg')
INSERT [dbo].[ProductImages] ([ID], [ID_Product], [ImageName]) VALUES (13, 7, N'3f570cce-a328-4891-b838-9a33ad647f49.jpg')
INSERT [dbo].[ProductImages] ([ID], [ID_Product], [ImageName]) VALUES (14, 9, N'605f66fe-0b91-4416-9c75-160784dfc213.jpg')
INSERT [dbo].[ProductImages] ([ID], [ID_Product], [ImageName]) VALUES (15, 1, N'cb59a2e6-845c-43ca-a86c-2072ec7accde.jpg')
GO
SET IDENTITY_INSERT [dbo].[TypeCake] ON 

INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (1, N'Bread')
INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (2, N'Cupcake & Muffin')
INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (3, N'Cookies')
INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (4, N'Loaf Cake')
INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (5, N'Puddings')
INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (6, N'Buns')
INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (8, N'Mochi')
INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (9, N'Tiramisu')
INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (11, N'Apple cake')
INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (32, N'Birthday Cake')
INSERT [dbo].[TypeCake] ([ID], [NameTypeCake]) VALUES (33, N'Chocolate Cake')
SET IDENTITY_INSERT [dbo].[TypeCake] OFF
GO
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
