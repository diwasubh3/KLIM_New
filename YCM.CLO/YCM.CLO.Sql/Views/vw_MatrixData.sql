CREATE VIEW [clo].[vw_MatrixData]
	AS SELECT 
	m.id,
	f.FundCode,
	m.Spread,
	m.Diversity,
	m.WARF,
	m.WARFModifier,
	m.DataPointType,
	DataPoint = case when m.DataPointType =1 then 'Major' else 'Minor' end,
	f.FundId,
	Interpolation= case when m.InterpolationType is null then '' when m.InterpolationType = 1 then 'Spread' when m.InterpolationType = 2 then 'Diversity' when m.InterpolationType = 3 then 'SpreadWithDiversity' end,
	[FromMajorMatrixDataId],
	[ToMajorMatrixDataId]
	FROM CLO.MatrixData m with(nolock) 
	JOIN CLO.Fund f with(nolock) on m.FundId = f.FundId

