
USE [DigNDB_Smittestop]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
-- To skip initial migration on production because migration mechanism have been enabled after first releaseon production. Initial migration describes db from first release (tables are already created)
INSERT INTO [dbo].[__EFMigrationsHistory]
           ([MigrationId] ,[ProductVersion])
     VALUES
           (N'20200903125821_InitialMigration', N'3.1.7')
GO
