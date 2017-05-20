namespace ASUVP.Online.Web.Models.Claim
{
    /// <summary>
    /// Подвижной состав
    /// </summary>
    public class ClaimRollingStock
    {
        // Парк контейнеров
        public string ContainerPark { get; set; }
        // Парк вагонов
        public string ContainerCarriage { get; set; }
        // Вид группы
        public string GroupView { get; set; }
        // Род контейнеров
        public string ContainerKind { get; set; }
        // Род вагонов
        public string CarriageKind { get; set; }
        // Вид отправки
        public string ShipmentKind { get; set; }
        // Тип вагонов
        public string CarriageType { get; set; }
    }
}