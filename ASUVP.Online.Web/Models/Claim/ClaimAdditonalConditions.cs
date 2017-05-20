namespace ASUVP.Online.Web.Models.Claim
{
    /// <summary>
    /// Дополнительные условия
    /// </summary>
    public class ClaimAdditonalConditions
    {
        // Условие
        public string Condition { get; set; }
        // Значение
        public string ConditionValue { get; set; }
        // Ограничения
        public string ConditionLimitations { get; set; }
    }
}