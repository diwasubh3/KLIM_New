INSERT INTO CLO.Field
        ( FieldGroupId ,
          FieldName ,
          JsonPropertyName ,
          FieldTitle ,
          JsonFormatString ,
          DisplayWidth ,
          IsPercentage
        )
VALUES  
		( 1 , -- FieldGroupId - smallint
          'CallDate' , -- FieldName - varchar(100)
          'callDate' , -- JsonPropertyName - varchar(100)
          'Call Date' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 1 , -- FieldGroupId - smallint
          'CountryDesc' , -- FieldName - varchar(100)
          'countryDesc' , -- JsonPropertyName - varchar(100)
          'Country' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 1 , -- FieldGroupId - smallint
          'MaturityDate' , -- FieldName - varchar(100)
          'maturityDate' , -- JsonPropertyName - varchar(100)
          'Maturity Date' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 1 , -- FieldGroupId - smallint
          'SnpIndustry' , -- FieldName - varchar(100)
          'snpIndustry' , -- JsonPropertyName - varchar(100)
          'Snp Industry' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 1 , -- FieldGroupId - smallint
          'MoodyIndustry' , -- FieldName - varchar(100)
          'moodyIndustry' , -- JsonPropertyName - varchar(100)
          'MoodyIndustry' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 1 , -- FieldGroupId - smallint
          'IsCovLite' , -- FieldName - varchar(100)
          'isCovLite' , -- JsonPropertyName - varchar(100)
          'IsCovLite' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 1 , -- FieldGroupId - smallint
          'IsFloating' , -- FieldName - varchar(100)
          'isFloating' , -- JsonPropertyName - varchar(100)
          'IsFloating' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 1 , -- FieldGroupId - smallint
          'LienType' , -- FieldName - varchar(100)
          'lienType' , -- JsonPropertyName - varchar(100)
          'LienType' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        )





		GO

INSERT INTO CLO.Field
        ( FieldGroupId ,
          FieldName ,
          JsonPropertyName ,
          FieldTitle ,
          JsonFormatString ,
          DisplayWidth ,
          IsPercentage
        )
VALUES  
		( 2 , -- FieldGroupId - smallint
          'Bid' , -- FieldName - varchar(100)
          'bid' , -- JsonPropertyName - varchar(100)
          'Bid' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 2 , -- FieldGroupId - smallint
          'Offer' , -- FieldName - varchar(100)
          'offer' , -- JsonPropertyName - varchar(100)
          'Offer' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 2 , -- FieldGroupId - smallint
          'Spread' , -- FieldName - varchar(100)
          'spread' , -- JsonPropertyName - varchar(100)
          'Spread' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        )
		,
		( 2 , -- FieldGroupId - smallint
          'LiborFloor' , -- FieldName - varchar(100)
          'liborFloor' , -- JsonPropertyName - varchar(100)
          'Libor Floor' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 2 , -- FieldGroupId - smallint
          'MoodyCashFlowRating' , -- FieldName - varchar(100)
          'moodyCashFlowRating' , -- JsonPropertyName - varchar(100)
          'Moody CashFlow Rating' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 2 , -- FieldGroupId - smallint
          'MoodyCashFlowRatingAdjusted' , -- FieldName - varchar(100)
          'moodyCashFlowRatingAdjusted' , -- JsonPropertyName - varchar(100)
          'MoodyCashFlowRatingAdjusted' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 2 , -- FieldGroupId - smallint
          'MoodyFacilityRating' , -- FieldName - varchar(100)
          'moodyFacilityRating' , -- JsonPropertyName - varchar(100)
          'MoodyFacilityRating' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 2 , -- FieldGroupId - smallint
          'MoodyFacilityRatingAdjusted' , -- FieldName - varchar(100)
          'moodyFacilityRatingAdjusted' , -- JsonPropertyName - varchar(100)
          'Moody Facility Rating Adjusted' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 2 , -- FieldGroupId - smallint
          'MoodyRecovery' , -- FieldName - varchar(100)
          'moodyRecovery' , -- JsonPropertyName - varchar(100)
          'Moody Recovery' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 2 , -- FieldGroupId - smallint
          'SnPIssuerRating' , -- FieldName - varchar(100)
          'snPIssuerRating' , -- JsonPropertyName - varchar(100)
          'S&P Issuer Rating' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 2 , -- FieldGroupId - smallint
          'SnPIssuerRatingAdjusted' , -- FieldName - varchar(100)
          'snPIssuerRatingAdjusted' , -- JsonPropertyName - varchar(100)
          'S&P Issuer Rating Adjusted' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 2 , -- FieldGroupId - smallint
          'SnPFacilityRating' , -- FieldName - varchar(100)
          'snPFacilityRating' , -- JsonPropertyName - varchar(100)
          'S&P Facility Rating' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 2 , -- FieldGroupId - smallint
          'SnPfacilityRatingAdjusted' , -- FieldName - varchar(100)
          'snPfacilityRatingAdjusted' , -- JsonPropertyName - varchar(100)
          'S&P Facility Rating Adjusted' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 2 , -- FieldGroupId - smallint
          'SnPRecoveryRating' , -- FieldName - varchar(100)
          'snPRecoveryRating' , -- JsonPropertyName - varchar(100)
          'S&P Recovery Rating' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 2 , -- FieldGroupId - smallint
          'MoodyOutlook' , -- FieldName - varchar(100)
          'moodyOutlook' , -- JsonPropertyName - varchar(100)
          'Moody Outlook' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 2 , -- FieldGroupId - smallint
          'MoodyWatch' , -- FieldName - varchar(100)
          'moodyWatch' , -- JsonPropertyName - varchar(100)
          'Moody Watch' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 2 , -- FieldGroupId - smallint
          'SnPWatch' , -- FieldName - varchar(100)
          'snPWatch' , -- JsonPropertyName - varchar(100)
          'S&P Watch' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 2 , -- FieldGroupId - smallint
          'NextReportingDate' , -- FieldName - varchar(100)
          'nextReportingDate' , -- JsonPropertyName - varchar(100)
          'Next Reporting Date' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 2 , -- FieldGroupId - smallint
          'FiscalYearEndDate' , -- FieldName - varchar(100)
          'fiscalYearEndDate' , -- JsonPropertyName - varchar(100)
          'Fiscal Year End Date' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 2 , -- FieldGroupId - smallint
          'IsAgentBankStable' , -- FieldName - varchar(100)
          'isAgentBankStable' , -- JsonPropertyName - varchar(100)
          'IsAgentBankStable' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        )

		GO
		INSERT INTO CLO.Field
        ( FieldGroupId ,
          FieldName ,
          JsonPropertyName ,
          FieldTitle ,
          JsonFormatString ,
          DisplayWidth ,
          IsPercentage
        )
VALUES  
		( 3 , -- FieldGroupId - smallint
          'CLOAnalyst' , -- FieldName - varchar(100)
          'cLOAnalyst' , -- JsonPropertyName - varchar(100)
          'CLO Analyst' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 3 , -- FieldGroupId - smallint
          'HFAnalyst' , -- FieldName - varchar(100)
          'hFAnalyst' , -- JsonPropertyName - varchar(100)
          'HF Analyst' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 3 , -- FieldGroupId - smallint
          'AsOfDate' , -- FieldName - varchar(100)
          'asOfDate' , -- JsonPropertyName - varchar(100)
          'As Of Date' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 3 , -- FieldGroupId - smallint
          'LiquidityScore' , -- FieldName - varchar(100)
          'liquidityScore' , -- JsonPropertyName - varchar(100)
          'Liquidity Score' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 3 , -- FieldGroupId - smallint
          'OneLLeverage' , -- FieldName - varchar(100)
          'oneLLeverage' , -- JsonPropertyName - varchar(100)
          '1L Leverage' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 3 , -- FieldGroupId - smallint
          'TotalLeverage' , -- FieldName - varchar(100)
          'totalLeverage' , -- JsonPropertyName - varchar(100)
          'Total Leverage' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - i
          NULL  -- IsPercentage - bit
        ),
		( 3 , -- FieldGroupId - smallint
          'EVMultiple' , -- FieldName - varchar(100)
          'eVMultiple' , -- JsonPropertyName - varchar(100)
          'EV Multiple' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 3 , -- FieldGroupId - smallint
          'LTMRevenues' , -- FieldName - varchar(100)
          'lTMRevenues' , -- JsonPropertyName - varchar(100)
          'LTM Revenues' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 3 , -- FieldGroupId - smallint
          'LTMEBITDA' , -- FieldName - varchar(100)
          'lTMEBITDA' , -- JsonPropertyName - varchar(100)
          'LTM EBITDA' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
		( 3 , -- FieldGroupId - smallint
          'FCF' , -- FieldName - varchar(100)
          'fCF' , -- JsonPropertyName - varchar(100)
          'FCF' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        ),
			( 3 , -- FieldGroupId - smallint
          'Comments' , -- FieldName - varchar(100)
          'comments' , -- JsonPropertyName - varchar(100)
          'Comments' , -- FieldTitle - varchar(100)
          '' , -- JsonFormatString - varchar(200)
          140 , -- DisplayWidth - int
          NULL  -- IsPercentage - bit
        )
		
		GO