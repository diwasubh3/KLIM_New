CREATE TABLE [CLO].[ParameterValue]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	[ParameterTypeId] SMALLINT NOT NULL references CLO.ParameterType([ParameterTypeId]),
	ParameterValueNumber numeric(28,4),
	ParameterValueText varchar(100),
	ParameterMinValueNumber numeric(28,4),
	ParameterMaxValueNumber numeric(28,4),
)
