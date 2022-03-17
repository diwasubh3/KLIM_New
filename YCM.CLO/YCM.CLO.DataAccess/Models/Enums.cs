namespace YCM.CLO.DataAccess.Models
{
	public enum WatchObjectTypeEnum
    {
        Security =1,
        Issuer = 2
    }

    public enum PaydownObjectTypeEnum
    {
        Security = 1,
        Issuer = 2
    }

    public enum LoadTypeEnum
    {
        Active = 0,
        Historical = 1,
        All =2
    }

    public enum SecuritySource
    {
        WSO = 0,
        YCM = 1
    }

    public enum Op
    {
        Equals =1,
        GreaterThan =2,
        LessThan = 4,
        GreaterThanOrEqual = 3,
        LessThanOrEqual = 5,
        Contains =6,
        StartsWith = 7,
        EndsWith =8
    }

    public enum GroupBy
    {
        Sell = 0,
        Buy = 1
    }
}
