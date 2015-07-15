USE [LearningWords]
GO

/****** Object:  Table [dbo].[Language]    Script Date: 07/14/2015 22:48:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Language](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LanguageCode] [varchar](25) NULL,
	[Description] [text] NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) 
) 

GO

SET ANSI_PADDING OFF
GO


/****** Object:  Table [dbo].[WordLearned]    Script Date: 07/14/2015 22:48:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WordLearned](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[WordID] [uniqueidentifier] NOT NULL,
	[Word] [nvarchar](150) NOT NULL,
	[LanguageID] [int] NOT NULL,
	[Description] [text] NULL,
 CONSTRAINT [PK_Word] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ,
 CONSTRAINT [IX_unique] UNIQUE NONCLUSTERED 
(
	[Word] ASC,
	[WordID] ASC,
	[LanguageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
) 

GO

ALTER TABLE [dbo].[WordLearned]  WITH CHECK ADD  CONSTRAINT [FK_WordLearned_Language] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[Language] ([ID])
GO

ALTER TABLE [dbo].[WordLearned] CHECK CONSTRAINT [FK_WordLearned_Language]
GO



