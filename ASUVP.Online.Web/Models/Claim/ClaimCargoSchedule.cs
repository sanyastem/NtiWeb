using System;

namespace ASUVP.Online.Web.Models.Claim
{
    /// <summary>
    /// График погрузки
    /// </summary>
    public class ClaimCargoSchedule
    {
        // Версия графика
        public string ScheduleVersion { get; set; }
        // Состояние
        public string State { get; set; }
        // Дата погрузки
        public DateTime CargoDate { get; set; }
        // Тонн
        public string ScheduleWeight { get; set; }
        // Вагонов
        public string ScheduleCarriageCount { get; set; }
    }
}