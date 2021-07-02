CREATE TABLE [CLO].[AlertProcessor]
(
	[AlertId] INT NOT NULL PRIMARY KEY identity(1,1),
	[AlertProcessorClassName] varchar(100),
	[ParameterTypeId] SMALLINT NOT NULL references CLO.ParameterType([ParameterTypeId]),
	[IsActive] bit default(1)
)
