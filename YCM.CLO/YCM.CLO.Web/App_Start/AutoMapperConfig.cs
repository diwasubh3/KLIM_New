using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DTO;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.Web.App_Start
{
	public static class AutoMapperConfig
	{
		private static readonly Dictionary<decimal, string> _creditScores;

		static AutoMapperConfig()
		{
			using (IRepository repo = new Repository())
			{
				var scores = repo.GetCreditScores();
				_creditScores = scores.ToDictionary(k => (decimal)k.Id, v => v.ScoreDescription);
			}
		}

        public static void RegisterMappings()
        {
			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<Field, CustomViewFieldDto>()
					.ForMember(m => m.FieldId, m => m.MapFrom(s => s.FieldId))
					.ForMember(m => m.FieldTitle, m => m.MapFrom(s => s.FieldTitle))
					.ForMember(m => m.FieldGroupId, m => m.MapFrom(s => s.FieldGroupId))
					.ForMember(m => m.IsHidden, m => m.MapFrom(s => s.Hidden))
					.ForMember(m => m.CustomViewFieldId, m => m.Ignore())
					.ForMember(m => m.ViewId, m => m.Ignore())
					.ForMember(m => m.SortOrder, m => m.Ignore());

				cfg.CreateMap<CustomViewField, CustomViewFieldDto>()
					.ForMember(m => m.FieldId, m => m.MapFrom(s => s.FieldId))
					.ForMember(m => m.FieldTitle, m => m.Ignore())
					.ForMember(m => m.CustomViewFieldId, m => m.Ignore())
					.ForMember(m => m.ViewId, m => m.Ignore())
					.ForMember(m => m.SortOrder, m => m.Ignore());

				cfg.CreateMap<Field, CustomViewField>()
					.ForMember(m => m.FieldId, m => m.MapFrom(s => s.FieldId))
					.ForMember(m => m.CustomViewFieldId, m => m.Ignore())
					.ForMember(m => m.ViewId, m => m.Ignore())
					.ForMember(m => m.SortOrder, m => m.Ignore());

				cfg.CreateMap<Field, FieldDto>().ForMember(f=>f.FieldGroupName, f=>f.MapFrom(fg=>fg.FieldGroup.FieldGroupName));
                cfg.CreateMap<FieldGroup, FieldGroupDto>();

                cfg.CreateMap<RuleField, RuleFieldDto>();
                cfg.CreateMap<Fund, FundDto>().ForMember(f=>f.WALDate, f=>f.MapFrom(fd=>fd.WALDate.HasValue?fd.WALDate.Value.ToString("MM/dd/yyyy"):string.Empty))
                .ForMember(f => f.ReInvestEndDate, f => f.MapFrom(fd => fd.ReInvestEndDate.HasValue ? fd.ReInvestEndDate.Value.ToString("MM/dd/yyyy") : string.Empty))
                .ForMember(f => f.PricingDate, f => f.MapFrom(fd => fd.PricingDate.HasValue ? fd.PricingDate.Value.ToString("MM/dd/yyyy") : string.Empty))
                .ForMember(f => f.ClosingDate, f => f.MapFrom(fd => fd.ClosingDate.HasValue ? fd.ClosingDate.Value.ToString("MM/dd/yyyy") : string.Empty))
                .ForMember(f => f.NonCallEndsDate, f => f.MapFrom(fd => fd.NonCallEndsDate.HasValue ? fd.NonCallEndsDate.Value.ToString("MM/dd/yyyy") : string.Empty))
                .ForMember(f => f.FinalMaturity, f => f.MapFrom(fd => fd.FinalMaturity.HasValue ? fd.FinalMaturity.Value.ToString("MM/dd/yyyy") : string.Empty))
                ;

                cfg.CreateMap<FundDto, Fund>();

                cfg.CreateMap<Rule, RuleDto>();
	            cfg.CreateMap<AnalystResearchHeader, AnalystResearchHeaderDto>()
		            .ForMember(x => x.Issuer, x => x.MapFrom(y => y.Issuer.IssuerCode));
	            cfg.CreateMap<AnalystResearchDetail, AnalystResearchDetailDto>()
		            .ForMember(x => x.AsOfDate, x => x.MapFrom(y => y.AsOfDate.ToString("MM/dd/yyyy")))
					.ForMember(x => x.SeniorLeverage, x => x.MapFrom(y => y.SeniorLeverage.HasValue ? $"{y.SeniorLeverage:F1}x" : string.Empty))
					.ForMember(x => x.TotalLeverage, x => x.MapFrom(y => y.TotalLeverage.HasValue ? $"{y.TotalLeverage:F1}x" : string.Empty))
					.ForMember(x => x.NetTotalLeverage, x => x.MapFrom(y => y.NetTotalLeverage.HasValue ? $"{y.NetTotalLeverage:F1}x" : string.Empty))
					.ForMember(x => x.FCFDebt, x => x.MapFrom(y => y.FCFDebt.HasValue ? $"{y.FCFDebt:P1}" : string.Empty))
					.ForMember(x => x.EnterpriseValue, x => x.MapFrom(y => y.EnterpriseValue.HasValue ? $"{y.EnterpriseValue:#,##0.0;(#,##0.0)}x" : string.Empty))
					.ForMember(x => x.LTMRevenues, x => x.MapFrom(y => y.LTMRevenues.HasValue ? $"{y.LTMRevenues:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.LTMEBITDA, x => x.MapFrom(y => y.LTMEBITDA.HasValue ? $"{y.LTMEBITDA:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.LTMFCF, x => x.MapFrom(y => y.LTMFCF.HasValue ? $"{y.LTMFCF:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.Revenues, x => x.MapFrom(y => y.Revenues.HasValue ? $"{y.Revenues:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.YoYGrowth, x => x.MapFrom(y => y.YoYGrowth.HasValue ? $"{y.YoYGrowth:P1}" : string.Empty))
					.ForMember(x => x.OrganicGrowth, x => x.MapFrom(y => y.OrganicGrowth.HasValue ? $"{y.OrganicGrowth:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.CashEBITDA, x => x.MapFrom(y => y.CashEBITDA.HasValue ? $"{y.CashEBITDA:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.Margin, x => x.MapFrom(y => y.Margin.HasValue ? $"{y.Margin:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.TransactionExpenses, x => x.MapFrom(y => y.TransactionExpenses.HasValue ? $"{y.TransactionExpenses:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.RestructuringAndIntegration, x => x.MapFrom(y => y.RestructuringAndIntegration.HasValue ? $"{y.RestructuringAndIntegration:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.Other1, x => x.MapFrom(y => y.Other1.HasValue ? $"{y.Other1:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.PFEBITDA, x => x.MapFrom(y => y.PFEBITDA.HasValue ? $"{y.PFEBITDA:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.LTMPFEBITDA, x => x.MapFrom(y => y.LTMPFEBITDA.HasValue ? $"{y.LTMPFEBITDA:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.PFCostSaves, x => x.MapFrom(y => y.PFCostSaves.HasValue ? $"{y.PFCostSaves:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.PFAcquisitionAdjustment, x => x.MapFrom(y => y.PFAcquisitionAdjustment.HasValue ? $"{y.PFAcquisitionAdjustment:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.CovenantEBITDA, x => x.MapFrom(y => y.CovenantEBITDA.HasValue ? $"{y.CovenantEBITDA:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.Interest, x => x.MapFrom(y => y.Interest.HasValue ? $"{y.Interest:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.CashTaxes, x => x.MapFrom(y => y.CashTaxes.HasValue ? $"{y.CashTaxes:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.WorkingCapital, x => x.MapFrom(y => y.WorkingCapital.HasValue ? $"{y.WorkingCapital:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.RestructuringOneTime, x => x.MapFrom(y => y.RestructuringOneTime.HasValue ? $"{y.RestructuringOneTime:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.Other2, x => x.MapFrom(y => y.Other2.HasValue ? $"{y.Other2:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.OCF, x => x.MapFrom(y => y.OCF.HasValue ? $"{y.OCF:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.CapitalExpenditures, x => x.MapFrom(y => y.CapitalExpenditures.HasValue ? $"{y.CapitalExpenditures:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.FCF, x => x.MapFrom(y => y.FCF.HasValue ? $"{y.FCF:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.ABLRCF, x => x.MapFrom(y => y.ABLRCF.HasValue ? $"{y.ABLRCF:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.FirstLienDebt, x => x.MapFrom(y => y.FirstLienDebt.HasValue ? $"{y.FirstLienDebt:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.TotalDebt, x => x.MapFrom(y => y.TotalDebt.HasValue ? $"{y.TotalDebt:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.EquityMarketCap, x => x.MapFrom(y => y.EquityMarketCap.HasValue ? $"{y.EquityMarketCap:#,##0.0;(#,##0.0)}" : string.Empty))
					.ForMember(x => x.Cash, x => x.MapFrom(y => y.Cash.HasValue ? $"{y.Cash:#,##0.0;(#,##0.0)}" : string.Empty))
					;

				cfg.CreateMap<vw_Position, PositionDto>()
					.ForMember(m => m.EnterpriseValue, x => x.MapFrom(y => y.EnterpriseValue.HasValue
						? $"{y.EnterpriseValue:N}"
						: string.Empty))
					.ForMember(m => m.LTMFCF, x => x.MapFrom(y => y.LTMFCF.HasValue
						? $"{y.LTMFCF:N}"
						: string.Empty))
					.ForMember(m => m.SeniorLeverage, x => x.MapFrom(y => y.SeniorLeverage.HasValue
						? $"{y.SeniorLeverage:N}"
						: string.Empty))
					//.ForMember(m => m.IsPrivate, x => x.MapFrom(y => y.IsPrivate.GetValueOrDefault()))
					.ForMember(m => m.IsSellCandidate, x => x.Ignore());
                cfg.CreateMap<vw_AggregatePosition, PositionDto>()
					.ForMember(m => m.CreditScore, x => x.MapFrom(y => y.CreditScore.HasValue && y.LienTypeId != 1
					? Math.Min(8, y.CreditScore.Value + 2)
							: y.CreditScore))
					.ForMember(m => m.ScoreDescription, x => x.ResolveUsing(y =>
	                {
						var score = y.CreditScore.HasValue && y.LienTypeId != 1
							? Math.Min(8, y.CreditScore.Value + 2)
							: y.CreditScore;
		                var desc = _creditScores.ContainsKey(score.GetValueOrDefault())
			                ? _creditScores[score.GetValueOrDefault()] : string.Empty;
		                return desc;
	                }))
	                .ForMember(m => m.IsPrivate, x => x.MapFrom(y => y.IsPrivate.GetValueOrDefault()))
					.ForMember(m => m.EnterpriseValue, x => x.MapFrom(y => y.EnterpriseValue.HasValue
			            ? $"{y.EnterpriseValue:N}" : string.Empty))
		            .ForMember(m => m.LTMFCF, x => x.MapFrom(y => y.LTMFCF.HasValue
			            ? $"{y.LTMFCF:N}" : string.Empty))
		            .ForMember(m => m.SeniorLeverage, x => x.MapFrom(y => y.SeniorLeverage.HasValue
			            ? $"{y.SeniorLeverage:N}" : string.Empty))
					.ForMember(m => m.IsSellCandidate, x => x.Ignore());
                cfg.CreateMap<RuleResult, RuleRestultDto>();
                cfg.CreateMap<RuleSectionType, RuleSectionTypeDto>();

                cfg.CreateMap<ParameterTypeDto, ParameterType>();
                cfg.CreateMap<ParameterType, ParameterTypeDto>();
                cfg.CreateMap<ParameterValue, ParameterValueDto>();
                cfg.CreateMap<ParameterValueDto, ParameterValue>();

                cfg.CreateMap<AnalystResearch, AnalystResearchDto>().ForMember(a=>a.Issuer, f=>f.MapFrom(fa=>fa.Issuer.IssuerDesc))
                .ForMember(a => a.CLOAnalyst, f => f.MapFrom(fa => fa.User.FullName))
                .ForMember(a => a.AsOfDate, f => f.MapFrom(fa => fa.AsOfDate.HasValue? fa.AsOfDate.Value.ToString("MM/dd/yyyy"):""))
                .ForMember(a => a.HFAnalyst, f => f.MapFrom(fa => fa.User1.FullName));

                cfg.CreateMap<vw_AnalystResearch, AnalystResearchDto>().ForMember(a => a.Issuer, f => f.MapFrom(fa => fa.IssuerCode)).ForMember(a => a.AsOfDate, f => f.MapFrom(fa => fa.AsOfDate.HasValue ? fa.AsOfDate.Value.ToString("MM/dd/yyyy") : ""));
                
                cfg.CreateMap<Facility, FacilityDto>();
				cfg.CreateMap<TradeType, TradeTypeDto>();
				cfg.CreateMap<AllocationRule, AllocationRuleDto>();
				cfg.CreateMap<SettleMethods, SettleMethodsDto>();
				cfg.CreateMap<InterestTreatment, InterestTreatmentDto>();
				cfg.CreateMap<Trader, TraderDto>();
				cfg.CreateMap<CounterParty, CounterPartyDto>();
				cfg.CreateMap<TradeBooking, TradeBookingDto>();
				cfg.CreateMap<TradeBookingDetail, TradeBookingDetailDto>();
				cfg.CreateMap<Issuer, IssuerDto>();
                cfg.CreateMap<Security, SecurityDto>()
                .ForMember(s=>s.Issuer,f=>f.MapFrom(s=>s.Issuer.IssuerDesc))
                .ForMember(s => s.Facility, f => f.MapFrom(s => s.Facility.FacilityDesc))
                .ForMember(s => s.CallDate, f => f.MapFrom(s => s.CallDate.HasValue? s.CallDate.Value.ToString("MM/dd/yyyy"):""))
                .ForMember(s => s.MaturityDate, f => f.MapFrom(s => s.MaturityDate.HasValue ? s.MaturityDate.Value.ToString("MM/dd/yyyy") : ""))
                .ForMember(s => s.SnpIndustry, f => f.MapFrom(s => s.Industry1.IndustryDesc))
                .ForMember(s => s.MoodyIndustry, f => f.MapFrom(s => s.Industry.IndustryDesc))
                .ForMember(s => s.IsFloating, f => f.MapFrom(s => s.IsFloating? "Floating" : "Fixed"))
                .ForMember(s => s.SecurityName, f => f.MapFrom(s => s.SecurityCode + " | " + s.Issuer.IssuerDesc + " | " + s.Facility.FacilityDesc))
                .ForMember(s => s.LienType, f => f.MapFrom(s => s.LienType.LienTypeDesc));

                cfg.CreateMap<Security, SecurityReconDto>()
              .ForMember(s => s.Issuer, f => f.MapFrom(s => s.Issuer.IssuerDesc))
              .ForMember(s => s.Code, f => f.MapFrom(s  => s.SecurityCode))
              .ForMember(s => s.Source, f => f.MapFrom(s => s.SourceId.HasValue && s.SourceId.Value==1 ? "YCM" : "WSO"))
              .ForMember(s => s.LoanDesc, f => f.MapFrom(s => s.SecurityDesc))
              .ForMember(s => s.Facility, f => f.MapFrom(s => s.Facility.FacilityDesc))
              .ForMember(s => s.MaturityDate, f => f.MapFrom(s => s.MaturityDate.HasValue ? s.MaturityDate.Value.ToString("MM/dd/yyyy") : ""));

                cfg.CreateMap<SecurityOverride, SecurityOverrideDto>()
                .ForMember(f=>f.SecurityCode, f=>f.MapFrom(sg=>sg.Security.SecurityCode))
                .ForMember(f => f.OverrideValue, f => f.MapFrom(sg => sg.Field.FieldType.HasValue && sg.Field.FieldType.Value == 3? DateTime.Parse(sg.OverrideValue).ToString("MM/dd/yyyy")
                :(sg.Field.FieldType.HasValue && sg.Field.FieldType.Value == 5 ?(sg.OverrideValue=="0"?"N":"Y"): sg.OverrideValue)))
                .ForMember(f => f.FacilityDesc, f => f.MapFrom(sg => sg.Security.Facility.FacilityDesc))
                .ForMember(f => f.IssuerDesc, f => f.MapFrom(sg => sg.Security.Issuer.IssuerDesc))
                .ForMember(f => f.EffectiveFrom, f => f.MapFrom(sg => sg.EffectiveFrom.HasValue ? sg.EffectiveFrom.Value.ToString("MM/dd/yyyy"):""))
                .ForMember(f => f.EffectiveTo, f => f.MapFrom(sg => sg.EffectiveTo.HasValue ? sg.EffectiveTo.Value.ToString("MM/dd/yyyy") : ""));

                cfg.CreateMap<SecurityOverride, LoanAttributeOverrideDto>()
                .ForMember(f => f.OverrideValue, f => f.MapFrom(sg => sg.Field.FieldType.HasValue && sg.Field.FieldType.Value == 3 ? DateTime.Parse(sg.OverrideValue).ToString("MM/dd/yyyy")
                : (sg.Field.FieldType.HasValue && sg.Field.FieldType.Value == 5 ? (sg.OverrideValue == "0" ? "N" : "Y") : sg.OverrideValue)))
                .ForMember(f => f.EffectiveFrom, f => f.MapFrom(sg => sg.EffectiveFrom.HasValue ? sg.EffectiveFrom.Value.ToString("MM/dd/yyyy") : ""))
                .ForMember(f => f.EffectiveTo, f => f.MapFrom(sg => sg.EffectiveTo.HasValue ? sg.EffectiveTo.Value.ToString("MM/dd/yyyy") : ""));

                cfg.CreateMap<User, UserDto>();

                cfg.CreateMap<vw_Security, VwSecurityDto>()
                .ForMember(f => f.SecurityLastUpdatedOn, f => f.MapFrom(sg => sg.SecurityLastUpdatedOn.HasValue ? sg.SecurityLastUpdatedOn.Value.ToString("MM/dd/yyyy h:m tt") : ""));

                cfg.CreateMap<vw_SecurityMarketCalculation, VwSecurityDto>()
                .ForMember(f => f.SecurityLastUpdatedOn, f => f.MapFrom(sg => sg.SecurityLastUpdatedOn.HasValue ? sg.SecurityLastUpdatedOn.Value.ToString("MM/dd/yyyy h:m tt") : ""));

                cfg.CreateMap<TradeAllocationDto, TradeAllocation>();
                cfg.CreateMap<TradeDto, Trade>();

                cfg.CreateMap<Trade, TradeDto>()
                .ForMember(f => f.IssuerId, f => f.MapFrom(sg => sg.Security.IssuerId));

                cfg.CreateMap<TradeAllocation, TradeAllocationDto>();

                cfg.CreateMap<PricingDto, Pricing>();

                cfg.CreateMap<LienType, LienTypeDto>();
                cfg.CreateMap<Industry, IndustryDto>();
                cfg.CreateMap<Rating, RatingDto>();

                cfg.CreateMap<TempSecurityDto, Security>();
                cfg.CreateMap<Security, TempSecurityDto>();
                cfg.CreateMap<FacilityDto, Facility>();
                cfg.CreateMap<IssuerDto, Issuer>();
                cfg.CreateMap<LienTypeDto, LienType>();

                cfg.CreateMap<TradeSwap, TradeSwapDto>();
				cfg.CreateMap<TradeHistory, TradeHistoryDto>();
			});
            

        }
    }
}