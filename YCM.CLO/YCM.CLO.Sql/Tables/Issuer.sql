﻿CREATE TABLE [CLO].[Issuer](
	[IssuerId] [INT] IDENTITY(1,1) NOT NULL,
	[IssuerDesc] [VARCHAR](100) NOT NULL,
	[IssuerCode] [VARCHAR](100) NULL,
	[CreatedOn] [DATETIME] NULL,
	[CreatedBy] [VARCHAR](100) NULL,
	[LastUpdatedOn] [DATETIME] NULL,
	[LastUpdatedBy] [VARCHAR](100) NULL,
	[IsPrivate] [BIT] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IssuerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [CLO].[Issuer] ADD  DEFAULT (GETDATE()) FOR [CreatedOn]
GO

ALTER TABLE [CLO].[Issuer] ADD  DEFAULT (GETDATE()) FOR [LastUpdatedOn]
GO

ALTER TABLE [CLO].[Issuer] ADD  DEFAULT ((0)) FOR [IsPrivate]
GO