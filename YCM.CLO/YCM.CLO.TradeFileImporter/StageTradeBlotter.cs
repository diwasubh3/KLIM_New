using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YCM.CLO.TradeFileImporter
{
    [Table("StageTradeBlotter", Schema = "CLO")]
    public class StageTradeBlotter
    {
        public int TradeBlotterJobId { get; set; }
        public string AssetPrimaryId { get; set; }
        public string Portfolio_Name { get; set; }
        public string Issuer_Name { get; set; }
        public string Asset_Name { get; set; }
        public string Asset_SecurityID { get; set; }
        public string Asset_MaturityDate { get; set; }
        public decimal? Asset_Rate1 { get; set; }
        public int? Asset_AssetDetail_Type { get; set; }
        public string Portfolio_EntityId { get; set; }
        public string Issuer_EntityId { get; set; }
        public string CurrencyType_Identifier { get; set; }
        public string TradeTypeDescription { get; set; }
        public DateTime? Trade_SettleDate { get; set; }
        public DateTime? Trade_TradeDate { get; set; }
        public double? Trade_AccruedInterest { get; set; }
        public string CounterBank_Name { get; set; }
        public string BrokerBank_Name { get; set; }
        public string TradeDescription { get; set; }
        public double? Trade_Commissions { get; set; }
        public double? Trade_Fees { get; set; }
        public double? Trade_Amount { get; set; }
        public int Trade_ID { get; set; }
        public double? Trade_AccruedPIK { get; set; }
        public double? Trade_AccruedFee { get; set; }
        public double? Trade_Accruals { get; set; }
        public string CounterBank_EntityId { get; set; }
        public string BrokerBank_EntityId { get; set; }
        public int? Trade_TradeGroup_ID { get; set; }
        public string CounterCompanyFundName { get; set; }
        public int? Position_Id { get; set; }
        public string Trade_TradeGroup_DESC { get; set; }
        public double? TotalFees { get; set; }
        public double? AccruedInterest { get; set; }
        public double? TradeAmount { get; set; }
        public double? TotalAmount { get; set; }
        public string CounterParty { get; set; }
        public string CounterPartyEntity { get; set; }
        public string CounterParty_Display { get; set; }
        public string Issuer_DisplayName { get; set; }
        public double? Trade_OriginalCommitment { get; set; }
        public double? Trade_OriginalParAmount { get; set; }     
        public TradeBlotterJob TradeBlotterJob { get; set; }

    }
}
