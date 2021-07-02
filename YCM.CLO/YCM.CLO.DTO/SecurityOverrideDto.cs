using System;

namespace YCM.CLO.DTO
{
    public class SecurityOverrideDto
    {
        public int SecurityOverrideId { get; set; }
        public int SecurityId { get; set; }
        public string SecurityCode { get; set; }
        public short FieldId { get; set; }
        public string OverrideValue { get; set; }
        public string EffectiveFrom { get; set; }
        public string EffectiveTo { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual FieldDto Field { get; set; }
        public string FacilityDesc { get; set; }
        public string IssuerDesc { get; set; }
        public string Attribute { get; set; }
        public string Comments { get; set; }
        public bool IsVisible => true;
        public string ExistingValue { get; set; }

        public bool IsCurrent=> 
            (!string.IsNullOrEmpty(EffectiveTo) && DateTime.Parse(EffectiveTo) >= DateTime.Today || string.IsNullOrEmpty(EffectiveTo))
            && DateTime.Parse(EffectiveFrom) <= DateTime.Today;

        public bool IsFuture =>
            !string.IsNullOrEmpty(EffectiveTo) && DateTime.Parse(EffectiveTo) > DateTime.Today
                                               && DateTime.Parse(EffectiveFrom) > DateTime.Today;

        public bool IsHistorical =>
            !string.IsNullOrEmpty(EffectiveTo) && DateTime.Parse(EffectiveTo) < DateTime.Today
                                               && DateTime.Parse(EffectiveFrom) < DateTime.Today;

        public string SearchText
            => (SecurityCode +"|" + FacilityDesc + "|" + IssuerDesc + "|" + OverrideValue + "|" + EffectiveFrom?.ToString() + "|" + EffectiveTo?.ToString() + "|" + Field.FieldTitle + Comments?.ToString()).ToLower();
    }
}
