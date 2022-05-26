using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YCM.CLO.Web.Models
{
    public class TradeXML
    {
    }


    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class WSOXML
    {

        private TRADE_IN_9x tRADE_IN_9xField;

        /// <remarks/>
        public TRADE_IN_9x TRADE_IN_9x
        {
            get
            {
                return this.tRADE_IN_9xField;
            }
            set
            {
                this.tRADE_IN_9xField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TRADE_IN_9x
    {

        private PARAMETERS pARAMETERSField;

        private DATA dATAField;

        /// <remarks/>
        public PARAMETERS PARAMETERS
        {
            get
            {
                return this.pARAMETERSField;
            }
            set
            {
                this.pARAMETERSField = value;
            }
        }

        /// <remarks/>
        public DATA DATA
        {
            get
            {
                return this.dATAField;
            }
            set
            {
                this.dATAField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class PARAMETERS
    {

        private string dateformatField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dateformat
        {
            get
            {
                return this.dateformatField;
            }
            set
            {
                this.dateformatField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DATA
    {

        private ASSET aSSETField;

        /// <remarks/>
        public ASSET ASSET
        {
            get
            {
                return this.aSSETField;
            }
            set
            {
                this.aSSETField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ASSET
    {

        private IDENTIFIER iDENTIFIERField;

        private TRADEGROUP tRADEGROUPField;

        private TRADE tRADEField;

        private string tidField;

        /// <remarks/>
        public IDENTIFIER IDENTIFIER
        {
            get
            {
                return this.iDENTIFIERField;
            }
            set
            {
                this.iDENTIFIERField = value;
            }
        }

        /// <remarks/>
        public TRADEGROUP TRADEGROUP
        {
            get
            {
                return this.tRADEGROUPField;
            }
            set
            {
                this.tRADEGROUPField = value;
            }
        }

        /// <remarks/>
        public TRADE TRADE
        {
            get
            {
                return this.tRADEField;
            }
            set
            {
                this.tRADEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tid
        {
            get
            {
                return this.tidField;
            }
            set
            {
                this.tidField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class IDENTIFIER
    {

        private string tdesField;

        private string idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tdes
        {
            get
            {
                return this.tdesField;
            }
            set
            {
                this.tdesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TRADEGROUP
    {

        private string cancelField;

        private string cancelreferenceticketidField;

        private string interesttreatmentField;

        private string readyforsettlementField;

        private string referenceticketidField;

        private string settlementplatformField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cancel
        {
            get
            {
                return this.cancelField;
            }
            set
            {
                this.cancelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cancelreferenceticketid
        {
            get
            {
                return this.cancelreferenceticketidField;
            }
            set
            {
                this.cancelreferenceticketidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string interesttreatment
        {
            get
            {
                return this.interesttreatmentField;
            }
            set
            {
                this.interesttreatmentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string readyforsettlement
        {
            get
            {
                return this.readyforsettlementField;
            }
            set
            {
                this.readyforsettlementField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string referenceticketid
        {
            get
            {
                return this.referenceticketidField;
            }
            set
            {
                this.referenceticketidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string settlementplatform
        {
            get
            {
                return this.settlementplatformField;
            }
            set
            {
                this.settlementplatformField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TRADE
    {

        private UDF uDFField;

        private NOTE nOTEField;

        private List<PORTFOLIOALLOCATION> pORTFOLIOALLOCATIONField;

        private string cancelField;

        private string cancelidField;

        private string counterpartyidField;

        private string quantityField;

        private string priceField;

        private string reasonfortradeField;

        private string settlementmethodField;

        private string tradedateField;

        private string tradeidField;

        private string traderidField;

        private string typeField;

        private string updateField;

        private string updateidField;

        /// <remarks/>
        public UDF UDF
        {
            get
            {
                return this.uDFField;
            }
            set
            {
                this.uDFField = value;
            }
        }

        /// <remarks/>
        public NOTE NOTE
        {
            get
            {
                return this.nOTEField;
            }
            set
            {
                this.nOTEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PORTFOLIOALLOCATION")]
        public List<PORTFOLIOALLOCATION> PORTFOLIOALLOCATION
        {
            get
            {
                return this.pORTFOLIOALLOCATIONField;
            }
            set
            {
                this.pORTFOLIOALLOCATIONField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cancel
        {
            get
            {
                return this.cancelField;
            }
            set
            {
                this.cancelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cancelid
        {
            get
            {
                return this.cancelidField;
            }
            set
            {
                this.cancelidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string counterpartyid
        {
            get
            {
                return this.counterpartyidField;
            }
            set
            {
                this.counterpartyidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string reasonfortrade
        {
            get
            {
                return this.reasonfortradeField;
            }
            set
            {
                this.reasonfortradeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string settlementmethod
        {
            get
            {
                return this.settlementmethodField;
            }
            set
            {
                this.settlementmethodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tradedate
        {
            get
            {
                return this.tradedateField;
            }
            set
            {
                this.tradedateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tradeid
        {
            get
            {
                return this.tradeidField;
            }
            set
            {
                this.tradeidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string traderid
        {
            get
            {
                return this.traderidField;
            }
            set
            {
                this.traderidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string update
        {
            get
            {
                return this.updateField;
            }
            set
            {
                this.updateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string updateid
        {
            get
            {
                return this.updateidField;
            }
            set
            {
                this.updateidField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class UDF
    {

        private string valueField;

        private string fieldnameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string fieldname
        {
            get
            {
                return this.fieldnameField;
            }
            set
            {
                this.fieldnameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class NOTE
    {

        private string notetypeField;

        private string noteField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string notetype
        {
            get
            {
                return this.notetypeField;
            }
            set
            {
                this.notetypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string note
        {
            get
            {
                return this.noteField;
            }
            set
            {
                this.noteField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class PORTFOLIOALLOCATION
    {

        private string portfolioidField;

        private string amountallocationField;

        private string nameField;

        private string tradeidField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string portfolioid
        {
            get
            {
                return this.portfolioidField;
            }
            set
            {
                this.portfolioidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string amountallocation
        {
            get
            {
                return this.amountallocationField;
            }
            set
            {
                this.amountallocationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tradeid
        {
            get
            {
                return this.tradeidField;
            }
            set
            {
                this.tradeidField = value;
            }
        }
    }


}