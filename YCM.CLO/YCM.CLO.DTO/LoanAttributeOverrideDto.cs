namespace YCM.CLO.DTO
{
	public class LoanAttributeOverrideDto
    {
        public int SecurityOverrideId { get; set; }
        public int SecurityId { get; set; }
        public short FieldId { get; set; }
        public string OverrideValue { get; set; }
        public string EffectiveFrom { get; set; }
        public string EffectiveTo { get; set; }
        public virtual FieldDto Field { get; set; }
        public string ExistingValue { get; set; }
        public void SetSearchText(VwSecurityDto vwSecurity)
        {
            this.SearchText =
                (OverrideValue + "|" + EffectiveFrom?.ToString() + "|" + EffectiveTo?.ToString() + "|" +
                 Field.FieldTitle
                 + "|" + (ExistingValue ?? "") + "|" +
                 vwSecurity.SecurityCode + "|" + "|YCM|WSO|effective|throught|current|" +
                 vwSecurity.SecurityLastUpdatedOn).ToLower();
        }

        public string SearchText { get; set; }
            
    }
}
