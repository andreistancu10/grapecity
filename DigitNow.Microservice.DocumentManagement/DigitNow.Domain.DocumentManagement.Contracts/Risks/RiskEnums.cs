namespace DigitNow.Domain.DocumentManagement.Contracts.Risks.Enums
{
    public enum RiskProbability
    {
        None = 0,
        Low = 1,
        Medium = 2,
        High = 3,
    } 
    
    public enum HeadOfDepartmentDecision
    {
        None = 0,
        Classified = 1,
        Escalated = 2,
        Retained = 3,
    } 
    
    public enum AdoptedStrategy
    {
        None = 0,
        Acceptation = 1,
        Monitorization = 2,
        Evitation = 3,
        Transferation= 4,
        Treatation = 5,
    }

    public static class RiskExposure
    {
        public const string SR = "SR";
        public const string SM = "SM";
        public const string SS = "SS";
        public const string MR = "MR";
        public const string MM = "MM";
        public const string MS = "MS";
        public const string RR = "RR";
        public const string RM = "RM";
        public const string RS = "RS";
    }
}
