using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace YCM.CLO.TradeFileImporter
{
    public class StageTradeCsvMap : ClassMap<StageTradeBlotter>
    {
        public StageTradeCsvMap()
        {
            Map(m => m.AssetPrimaryId).Index(0);
            Map(m => m.TotalFees).Index(1);
            Map(m => m.AccruedInterest).Index(2);
            Map(m => m.Trade_Amount).Index(3);
            Map(m => m.TradeAmount).Index(4);
            Map(m => m.TotalAmount).Index(5);
            Map(m => m.CounterParty).Index(6);
            Map(m => m.CounterPartyEntity).Index(7);
            Map(m => m.CounterParty_Display).Index(8);
            Map(m => m.Issuer_DisplayName).Index(9);
            Map(m => m.Portfolio_Name).Index(10);
            Map(m => m.Issuer_Name).Index(11);
            Map(m => m.Asset_Name).Index(12);
            Map(m => m.Asset_SecurityID).Index(13);
            Map(m => m.Asset_MaturityDate).Index(14);
            Map(m => m.Asset_Rate1).Index(15);
            Map(m => m.Asset_AssetDetail_Type).Index(16);
            Map(m => m.Portfolio_EntityId).Index(17);
            Map(m => m.Issuer_EntityId).Index(18);
            Map(m => m.CurrencyType_Identifier).Index(19);
            Map(m => m.TradeTypeDescription).Index(20);
            Map(m => m.Trade_SettleDate).Index(21);
            Map(m => m.Trade_TradeDate).Index(22);
            Map(m => m.Trade_AccruedInterest).Index(23);
            Map(m => m.CounterBank_Name).Index(24);
            Map(m => m.BrokerBank_Name).Index(25);
            Map(m => m.TradeDescription).Index(26);
            Map(m => m.Trade_Commissions).Index(27);
            Map(m => m.Trade_Fees).Index(28);
            Map(m => m.Trade_ID).Index(29);
            Map(m => m.Trade_AccruedPIK).Index(30);
            Map(m => m.Trade_AccruedFee).Index(31);
            Map(m => m.Trade_Accruals).Index(32);
            Map(m => m.CounterBank_EntityId).Index(33);
            Map(m => m.BrokerBank_EntityId).Index(34);
            Map(m => m.Trade_TradeGroup_ID).Index(35);
            Map(m => m.CounterCompanyFundName).Index(36);
            Map(m => m.Trade_OriginalCommitment).Index(37);
            Map(m => m.Trade_OriginalParAmount).Index(38);
            Map(m => m.Position_Id).Index(39);
            Map(m => m.Trade_TradeGroup_DESC).Index(40);

        }
    }
}
