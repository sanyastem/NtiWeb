using ASUVP.Core.DataAccess.Model;

namespace ASUVP.Online.Web.Models.Claim
{
    /// <summary>
    /// Полная форма заявки
    /// </summary>
    public class ClaimFullDetails
    {
        // ------------------------------------------------------------------------------------------------- //
        // Страна отправления (dep. - departure)
        public string DepCountry { get; set; }
        // Станция отправления (Код)
        public string DepStationCode { get; set; }
        // Станция отправления (Наименование)
        public string DepStationName { get; set; }
        // Станция отправления (Дорога)
        public string DepStationRoad { get; set; }
        // Вид сообщения
        public string MessageView { get; set; }
        // Страна назначения
        public string DestCountry { get; set; }
        // Станция назначения (Код)
        public string DestStationCode { get; set; }
        // Станция назначения (Наименование)
        public string DestStationName { get; set; }
        // Станция назначения (Дорога)
        public string DestStationRoad { get; set; }
        // Тарифное расстояние (км)
        public string TariffDistance { get; set; }
        // Нормативный срок (дней)
        public string NormalDeliveryTime { get; set; }
        // ------------------------------------------------------------------------------------------------- //
        // Груз ЕТСНГ
        public string CargoETSNG { get; set; }
        // Груз ЕТНГ (Наименование)
        public string CargoETSNGName { get; set; }
        // Груз ГНГ
        public string CargoGNG { get; set; }
        // Груз ГНГ (Наименование)
        public string CargoGNGName { get; set; }
        // Грузоотправитель ТНГЛ
        public string ShipperTGNL { get; set; }
        // Грузоотправитель (Наименование)
        public string ShipperName { get; set; }
        // Грузополучатель ТГНЛ
        public string ConsigneeTGNL { get; set; }
        // Грузополучаель (Наименование)
        public string ConsigneeName { get; set; }
        // Вес груза (тонн)
        public string CargoWeight { get; set; }
        // Колчество контейнеров (шт)
        public string ContainerCount { get; set; }
        // Количество вагонов (шт)
        public string CarriageCount { get; set; }


        public ClaimDetails ClaimDetails { get; set; }
    }
}