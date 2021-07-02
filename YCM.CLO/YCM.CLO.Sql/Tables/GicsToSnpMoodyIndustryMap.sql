CREATE TABLE [CLO].[GicsToSnpMoodyIndustryMap]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[SectorId] SMALLINT,
	[GICSIndustryGroup] VARCHAR(100) NOT NULL,
	[GICSIndustry] VARCHAR(100) NOT NULL,
	[GICSIndustryGrpDesc] VARCHAR(500) NOT NULL,
	[GICSIndustryDesc] VARCHAR(100) NOT NULL,


	[MappedSnPIndustryId] SMALLINT,
	[MappedMoodyIndustryId] SMALLINT,

	[CreatedOn] datetime default (getdate()),
	[CreatedBy] varchar(100) null,
	[LastUpdatedOn] datetime default (getdate()),
	[LastUpdatedBy] varchar(100) null,

)
