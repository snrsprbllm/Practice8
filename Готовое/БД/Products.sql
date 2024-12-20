USE [Practice8]
GO

/****** Object:  Table [dbo].[Products]    Script Date: 20.12.2024 16:15:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Products](
	[id] [int] NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[category_id] [int] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Ñategories] FOREIGN KEY([category_id])
REFERENCES [dbo].[Ñategories] ([id])
GO

ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Ñategories]
GO


