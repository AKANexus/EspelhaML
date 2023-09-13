using System.Text.Json.Serialization;

namespace MlSuite.MlSynch.DTO
{
    public class Agency
    {
        [JsonPropertyName("carrier_id")]
        public ulong CarrierId { get; set; }
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }
        [JsonPropertyName("agency_id")]
        public string AgencyId { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("open_hours")]
        public string OpenHours { get; set; }
    }
    public class Buffering
    {
        [JsonPropertyName("date")]
        public object Date { get; set; }
    }

    public class CostComponents
    {
        [JsonPropertyName("loyal_discount")]
        public double? LoyalDiscount { get; set; }

        [JsonPropertyName("special_discount")]
        public double? SpecialDiscount { get; set; }

        [JsonPropertyName("compensation")]
        public double? Compensation { get; set; }

        [JsonPropertyName("gap_discount")]
        public double? GapDiscount { get; set; }

        [JsonPropertyName("ratio")]
        public double? Ratio { get; set; }
    }


    public class DimensionsSource
    {
        [JsonPropertyName("origin")]
        public string Origin { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class EstimatedDeliveryExtended
    {
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("offset")]
        public int? Offset { get; set; }
    }

    public class EstimatedDeliveryFinal
    {
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("offset")]
        public int? Offset { get; set; }
    }

    public class EstimatedDeliveryLimit
    {
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("offset")]
        public int? Offset { get; set; }
    }

    public class EstimatedDeliveryTime
    {
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("pay_before")]
        public DateTime? PayBefore { get; set; }

        [JsonPropertyName("schedule")]
        public object Schedule { get; set; }

        [JsonPropertyName("unit")]
        public string Unit { get; set; }

        [JsonPropertyName("offset")]
        public Offset Offset { get; set; }

        [JsonPropertyName("shipping")]
        public int? Shipping { get; set; }

        [JsonPropertyName("time_frame")]
        public TimeFrame TimeFrame { get; set; }

        [JsonPropertyName("handling")]
        public int? Handling { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class EstimatedHandlingLimit
    {
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }
    }

    public class EstimatedScheduleLimit
    {
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }
    }

    public class Municipality
    {
        [JsonPropertyName("id")]
        public ulong? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class Offset
    {
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("shipping")]
        public object Shipping { get; set; }
    }

    public class PriorityClass
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class ReceiverAddress
    {
        [JsonPropertyName("country")]
        public Country Country { get; set; }

        [JsonPropertyName("address_line")]
        public string AddressLine { get; set; }

        [JsonPropertyName("types")]
        public List<string> Types { get; set; }

        [JsonPropertyName("scoring")]
        public double? Scoring { get; set; }

        [JsonPropertyName("agency")]
        public Agency? Agency { get; set; }

        [JsonPropertyName("city")]
        public City City { get; set; }

        [JsonPropertyName("geolocation_type")]
        public string GeolocationType { get; set; }

        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }

        [JsonPropertyName("municipality")]
        public Municipality Municipality { get; set; }

        [JsonPropertyName("location_id")]
        public ulong? LocationId { get; set; }

        [JsonPropertyName("street_name")]
        public string StreetName { get; set; }

        [JsonPropertyName("zip_code")]
        public string ZipCode { get; set; }

        [JsonPropertyName("geolocation_source")]
        public string GeolocationSource { get; set; }

        [JsonPropertyName("delivery_preference")]
        public string DeliveryPreference { get; set; }

        [JsonPropertyName("intersection")]
        public string Intersection { get; set; }

        [JsonPropertyName("street_number")]
        public string StreetNumber { get; set; }

        [JsonPropertyName("receiver_name")]
        public string? ReceiverName { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("id")]
        public ulong? Id { get; set; }

        [JsonPropertyName("state")]
        public State State { get; set; }

        [JsonPropertyName("neighborhood")]
        public Neighborhood Neighborhood { get; set; }

        [JsonPropertyName("geolocation_last_updated")]
        public DateTime? GeolocationLastUpdated { get; set; }

        [JsonPropertyName("receiver_phone")]
        public string? ReceiverPhone { get; set; }

        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }
    }

    public class ShipmentDto : ErrorDto
    {
        [JsonPropertyName("substatus_history")]
        public List<SubstatusHistory> SubstatusHistory { get; set; }

        [JsonPropertyName("snapshot_packing")]
        public SnapshotPacking SnapshotPacking { get; set; }

        [JsonPropertyName("receiver_id")]
        public ulong? ReceiverId { get; set; }

        [JsonPropertyName("base_cost")]
        public double? BaseCost { get; set; }

        [JsonPropertyName("status_history")]
        public StatusHistory StatusHistory { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("return_details")]
        public object ReturnDetails { get; set; }

        [JsonPropertyName("sender_id")]
        public ulong? SenderId { get; set; }

        [JsonPropertyName("mode")]
        public string Mode { get; set; }

        [JsonPropertyName("order_cost")]
        public double? OrderCost { get; set; }

        [JsonPropertyName("priority_class")]
        public PriorityClass PriorityClass { get; set; }

        [JsonPropertyName("service_id")]
        public ulong? ServiceId { get; set; }

        [JsonPropertyName("shipping_items")]
        public List<ShippingItem> ShippingItems { get; set; }

        [JsonPropertyName("tracking_number")]
        public string TrackingNumber { get; set; }

        [JsonPropertyName("cost_components")]
        public CostComponents CostComponents { get; set; }

        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("tracking_method")]
        public string TrackingMethod { get; set; }

        [JsonPropertyName("last_updated")]
        public DateTime? LastUpdated { get; set; }

        [JsonPropertyName("items_types")]
        public List<string> ItemsTypes { get; set; }

        [JsonPropertyName("comments")]
        public object Comments { get; set; }

        [JsonPropertyName("substatus")]
        public string? Substatus { get; set; }

        [JsonPropertyName("date_created")]
        public DateTime? DateCreated { get; set; }

        [JsonPropertyName("date_first_printed")]
        public DateTime? DateFirstPrinted { get; set; }

        [JsonPropertyName("created_by")]
        public string CreatedBy { get; set; }

        [JsonPropertyName("application_id")]
        public object ApplicationId { get; set; }

        [JsonPropertyName("shipping_option")]
        public ShippingOption ShippingOption { get; set; }

        [JsonPropertyName("tags")]
        public List<object> Tags { get; set; }

        [JsonPropertyName("sender_address")]
        public SenderAddress SenderAddress { get; set; }

        [JsonPropertyName("sibling")]
        public Sibling Sibling { get; set; }

        [JsonPropertyName("return_tracking_number")]
        public object ReturnTrackingNumber { get; set; }

        [JsonPropertyName("site_id")]
        public string SiteId { get; set; }

        [JsonPropertyName("carrier_info")]
        public object CarrierInfo { get; set; }

        [JsonPropertyName("market_place")]
        public string MarketPlace { get; set; }

        [JsonPropertyName("receiver_address")]
        public ReceiverAddress ReceiverAddress { get; set; }

        [JsonPropertyName("customer_id")]
        public object CustomerId { get; set; }

        [JsonPropertyName("order_id")]
        public long? OrderId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("logistic_type")]
        public string LogisticType { get; set; }
    }

    public class SenderAddress
    {
        [JsonPropertyName("country")]
        public Country Country { get; set; }

        [JsonPropertyName("address_line")]
        public string AddressLine { get; set; }

        [JsonPropertyName("types")]
        public List<string> Types { get; set; }

        [JsonPropertyName("scoring")]
        public object Scoring { get; set; }

        [JsonPropertyName("agency")]
        public Agency? Agency { get; set; }

        [JsonPropertyName("city")]
        public City City { get; set; }

        [JsonPropertyName("geolocation_type")]
        public object GeolocationType { get; set; }

        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }

        [JsonPropertyName("municipality")]
        public Municipality Municipality { get; set; }

        [JsonPropertyName("location_id")]
        public object LocationId { get; set; }

        [JsonPropertyName("street_name")]
        public string StreetName { get; set; }

        [JsonPropertyName("zip_code")]
        public string ZipCode { get; set; }

        [JsonPropertyName("geolocation_source")]
        public object GeolocationSource { get; set; }

        [JsonPropertyName("intersection")]
        public object Intersection { get; set; }

        [JsonPropertyName("street_number")]
        public string StreetNumber { get; set; }

        [JsonPropertyName("comment")]
        public object Comment { get; set; }

        [JsonPropertyName("id")]
        public object Id { get; set; }

        [JsonPropertyName("state")]
        public State State { get; set; }

        [JsonPropertyName("neighborhood")]
        public Neighborhood Neighborhood { get; set; }

        [JsonPropertyName("geolocation_last_updated")]
        public object GeolocationLastUpdated { get; set; }

        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }
    }

    public class ShippingItem
    {
        [JsonPropertyName("domain_id")]
        public object DomainId { get; set; }

        [JsonPropertyName("quantity")]
        public int? Quantity { get; set; }

        [JsonPropertyName("dimensions_source")]
        public DimensionsSource DimensionsSource { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("user_product_id")]
        public object UserProductId { get; set; }

        [JsonPropertyName("sender_id")]
        public ulong? SenderId { get; set; }

        [JsonPropertyName("dimensions")]
        public string Dimensions { get; set; }
    }

    public class ShippingOption
    {
        [JsonPropertyName("processing_time")]
        public int? ProcessingTime { get; set; }

        [JsonPropertyName("cost")]
        public decimal? Cost { get; set; }

        [JsonPropertyName("estimated_schedule_limit")]
        public EstimatedScheduleLimit EstimatedScheduleLimit { get; set; }

        [JsonPropertyName("shipping_method_id")]
        public ulong? ShippingMethodId { get; set; }

        [JsonPropertyName("estimated_delivery_final")]
        public EstimatedDeliveryFinal EstimatedDeliveryFinal { get; set; }

        [JsonPropertyName("buffering")]
        public Buffering Buffering { get; set; }

        [JsonPropertyName("list_cost")]
        public double? ListCost { get; set; }

        [JsonPropertyName("estimated_delivery_limit")]
        public EstimatedDeliveryLimit EstimatedDeliveryLimit { get; set; }

        [JsonPropertyName("priority_class")]
        public PriorityClass PriorityClass { get; set; }

        [JsonPropertyName("delivery_promise")]
        public string DeliveryPromise { get; set; }

        [JsonPropertyName("delivery_type")]
        public string DeliveryType { get; set; }

        [JsonPropertyName("estimated_handling_limit")]
        public EstimatedHandlingLimit EstimatedHandlingLimit { get; set; }

        [JsonPropertyName("estimated_delivery_time")]
        public EstimatedDeliveryTime EstimatedDeliveryTime { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public ulong? Id { get; set; }

        [JsonPropertyName("estimated_delivery_extended")]
        public EstimatedDeliveryExtended EstimatedDeliveryExtended { get; set; }

        [JsonPropertyName("currency_id")]
        public string CurrencyId { get; set; }
    }

    public class Sibling
    {
        [JsonPropertyName("reason")]
        public object Reason { get; set; }

        [JsonPropertyName("sibling_id")]
        public object SiblingId { get; set; }

        [JsonPropertyName("description")]
        public object Description { get; set; }

        [JsonPropertyName("source")]
        public object Source { get; set; }

        [JsonPropertyName("date_created")]
        public object DateCreated { get; set; }

        [JsonPropertyName("last_updated")]
        public object LastUpdated { get; set; }
    }

    public class SnapshotPacking
    {
        [JsonPropertyName("snapshot_id")]
        public string SnapshotId { get; set; }

        [JsonPropertyName("pack_hash")]
        public string PackHash { get; set; }
    }

    public class StatusHistory
    {
        [JsonPropertyName("date_shipped")]
        public DateTime? DateShipped { get; set; }

        [JsonPropertyName("date_returned")]
        public object DateReturned { get; set; }

        [JsonPropertyName("date_delivered")]
        public object DateDelivered { get; set; }

        [JsonPropertyName("date_first_visit")]
        public object DateFirstVisit { get; set; }

        [JsonPropertyName("date_not_delivered")]
        public object DateNotDelivered { get; set; }

        [JsonPropertyName("date_cancelled")]
        public object DateCancelled { get; set; }

        [JsonPropertyName("date_handling")]
        public DateTime? DateHandling { get; set; }

        [JsonPropertyName("date_ready_to_ship")]
        public DateTime? DateReadyToShip { get; set; }
    }

    public class SubstatusHistory
    {
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("substatus")]
        public string Substatus { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class TimeFrame
    {
        [JsonPropertyName("from")]
        public object From { get; set; }

        [JsonPropertyName("to")]
        public object To { get; set; }
    }

}
