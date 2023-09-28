using System.Text.Json.Serialization;

namespace MlSuite.DTOs
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    //public class Address
    //{
    //    [JsonPropertyName("address")]
    //    public string Address { get; set; }

    //    [JsonPropertyName("city")]
    //    public string City { get; set; }

    //    [JsonPropertyName("state")]
    //    public string State { get; set; }

    //    [JsonPropertyName("zip_code")]
    //    public string ZipCode { get; set; }
    //}

    //public class AlternativePhone
    //{
    //    [JsonPropertyName("area_code")]
    //    public string AreaCode { get; set; }

    //    [JsonPropertyName("extension")]
    //    public string Extension { get; set; }

    //    [JsonPropertyName("number")]
    //    public string Number { get; set; }
    //}

    //public class BillData
    //{
    //    [JsonPropertyName("accept_credit_note")]
    //    public string AcceptCreditNote { get; set; }
    //}

    //public class Billing
    //{
    //    [JsonPropertyName("allow")]
    //    public bool? Allow { get; set; }

    //    [JsonPropertyName("codes")]
    //    public List<object> Codes { get; set; }
    //}

    //public class Buy
    //{
    //    [JsonPropertyName("allow")]
    //    public bool? Allow { get; set; }

    //    [JsonPropertyName("codes")]
    //    public List<object> Codes { get; set; }

    //    [JsonPropertyName("immediate_payment")]
    //    public ImmediatePayment ImmediatePayment { get; set; }
    //}

    //public class BuyerReputation
    //{
    //    [JsonPropertyName("canceled_transactions")]
    //    public int? CanceledTransactions { get; set; }

    //    [JsonPropertyName("tags")]
    //    public List<object> Tags { get; set; }

    //    [JsonPropertyName("transactions")]
    //    public Transactions Transactions { get; set; }
    //}

    //public class Canceled
    //{
    //    [JsonPropertyName("paid")]
    //    public object Paid { get; set; }

    //    [JsonPropertyName("total")]
    //    public object Total { get; set; }
    //}

    //public class Cancellations
    //{
    //    [JsonPropertyName("period")]
    //    public string Period { get; set; }

    //    [JsonPropertyName("rate")]
    //    public double? Rate { get; set; }

    //    [JsonPropertyName("value")]
    //    public int? Value { get; set; }
    //}

    //public class Claims
    //{
    //    [JsonPropertyName("period")]
    //    public string Period { get; set; }

    //    [JsonPropertyName("rate")]
    //    public double? Rate { get; set; }

    //    [JsonPropertyName("value")]
    //    public int? Value { get; set; }
    //}

    //public class Company
    //{
    //    [JsonPropertyName("brand_name")]
    //    public string BrandName { get; set; }

    //    [JsonPropertyName("city_tax_id")]
    //    public string CityTaxId { get; set; }

    //    [JsonPropertyName("corporate_name")]
    //    public string CorporateName { get; set; }

    //    [JsonPropertyName("identification")]
    //    public string Identification { get; set; }

    //    [JsonPropertyName("state_tax_id")]
    //    public string StateTaxId { get; set; }

    //    [JsonPropertyName("cust_type_id")]
    //    public string CustTypeId { get; set; }

    //    [JsonPropertyName("soft_descriptor")]
    //    public string SoftDescriptor { get; set; }
    //}

    //public class Context
    //{
    //    [JsonPropertyName("device")]
    //    public object Device { get; set; }

    //    [JsonPropertyName("flow")]
    //    public object Flow { get; set; }

    //    [JsonPropertyName("ip_address")]
    //    public string IpAddress { get; set; }

    //    [JsonPropertyName("source")]
    //    public string Source { get; set; }
    //}

    //public class Credit
    //{
    //    [JsonPropertyName("consumed")]
    //    public double? Consumed { get; set; }

    //    [JsonPropertyName("credit_level_id")]
    //    public string CreditLevelId { get; set; }

    //    [JsonPropertyName("rank")]
    //    public string Rank { get; set; }
    //}

    //public class DelayedHandlingTime
    //{
    //    [JsonPropertyName("period")]
    //    public string Period { get; set; }

    //    [JsonPropertyName("rate")]
    //    public double? Rate { get; set; }

    //    [JsonPropertyName("value")]
    //    public int? Value { get; set; }
    //}

    public class Identification
    {
        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    //public class ImmediatePayment
    //{
    //    [JsonPropertyName("reasons")]
    //    public List<object> Reasons { get; set; }

    //    [JsonPropertyName("required")]
    //    public bool? Required { get; set; }
    //}

    //public class List
    //{
    //    [JsonPropertyName("allow")]
    //    public bool? Allow { get; set; }

    //    [JsonPropertyName("codes")]
    //    public List<object> Codes { get; set; }

    //    [JsonPropertyName("immediate_payment")]
    //    public ImmediatePayment ImmediatePayment { get; set; }
    //}

    //public class Metrics
    //{
    //    [JsonPropertyName("sales")]
    //    public Sales Sales { get; set; }

    //    [JsonPropertyName("claims")]
    //    public Claims Claims { get; set; }

    //    [JsonPropertyName("delayed_handling_time")]
    //    public DelayedHandlingTime DelayedHandlingTime { get; set; }

    //    [JsonPropertyName("cancellations")]
    //    public Cancellations Cancellations { get; set; }
    //}

    //public class NotYetRated
    //{
    //    [JsonPropertyName("paid")]
    //    public object Paid { get; set; }

    //    [JsonPropertyName("total")]
    //    public object Total { get; set; }

    //    [JsonPropertyName("units")]
    //    public object Units { get; set; }
    //}

    //public class Phone
    //{
    //    [JsonPropertyName("area_code")]
    //    public string AreaCode { get; set; }

    //    [JsonPropertyName("extension")]
    //    public string Extension { get; set; }

    //    [JsonPropertyName("number")]
    //    public string Number { get; set; }

    //    [JsonPropertyName("verified")]
    //    public bool? Verified { get; set; }
    //}

    //public class Ratings
    //{
    //    [JsonPropertyName("negative")]
    //    public double? Negative { get; set; }

    //    [JsonPropertyName("neutral")]
    //    public double? Neutral { get; set; }

    //    [JsonPropertyName("positive")]
    //    public double? Positive { get; set; }
    //}

    //public class RestrictionsColiving
    //{
    //    [JsonPropertyName("verification_status")]
    //    public string VerificationStatus { get; set; }

    //    [JsonPropertyName("user_tags")]
    //    public List<object> UserTags { get; set; }

    //    [JsonPropertyName("user_internal_tags")]
    //    public List<object> UserInternalTags { get; set; }

    //    [JsonPropertyName("user_status_attributes")]
    //    public List<object> UserStatusAttributes { get; set; }
    //}

    public class MeDto
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; }

        [JsonPropertyName("registration_date")]
        public DateTime? RegistrationDate { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("country_id")]
        public string CountryId { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("identification")]
        public Identification Identification { get; set; }

        //[JsonPropertyName("address")]
        //public Address Address { get; set; }

        //[JsonPropertyName("phone")]
        //public Phone Phone { get; set; }

        //[JsonPropertyName("alternative_phone")]
        //public AlternativePhone AlternativePhone { get; set; }

        [JsonPropertyName("user_type")]
        public string UserType { get; set; }

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }

        [JsonPropertyName("logo")]
        public object Logo { get; set; }

        [JsonPropertyName("points")]
        public int? Points { get; set; }

        [JsonPropertyName("site_id")]
        public string SiteId { get; set; }

        [JsonPropertyName("permalink")]
        public string Permalink { get; set; }

        [JsonPropertyName("seller_experience")]
        public string SellerExperience { get; set; }

        //[JsonPropertyName("bill_data")]
        //public BillData BillData { get; set; }

        //[JsonPropertyName("seller_reputation")]
        //public SellerReputation SellerReputation { get; set; }

        //[JsonPropertyName("buyer_reputation")]
        //public BuyerReputation BuyerReputation { get; set; }

        //[JsonPropertyName("status")]
        //public Status Status { get; set; }

        [JsonPropertyName("secure_email")]
        public string SecureEmail { get; set; }

        //[JsonPropertyName("company")]
        //public Company Company { get; set; }

        //[JsonPropertyName("credit")]
        //public Credit Credit { get; set; }

        //[JsonPropertyName("context")]
        //public Context Context { get; set; }

        //[JsonPropertyName("thumbnail")]
        //public Thumbnail Thumbnail { get; set; }

        [JsonPropertyName("registration_identifiers")]
        public List<object> RegistrationIdentifiers { get; set; }
    }

    //public class Sales
    //{
    //    [JsonPropertyName("period")]
    //    public string Period { get; set; }

    //    [JsonPropertyName("completed")]
    //    public int? Completed { get; set; }
    //}

    //public class Sell
    //{
    //    [JsonPropertyName("allow")]
    //    public bool? Allow { get; set; }

    //    [JsonPropertyName("codes")]
    //    public List<object> Codes { get; set; }

    //    [JsonPropertyName("immediate_payment")]
    //    public ImmediatePayment ImmediatePayment { get; set; }
    //}

    //public class SellerReputation
    //{
    //    [JsonPropertyName("level_id")]
    //    public string LevelId { get; set; }

    //    [JsonPropertyName("power_seller_status")]
    //    public string PowerSellerStatus { get; set; }

    //    [JsonPropertyName("transactions")]
    //    public Transactions Transactions { get; set; }

    //    [JsonPropertyName("metrics")]
    //    public Metrics Metrics { get; set; }
    //}

    //public class ShoppingCart
    //{
    //    [JsonPropertyName("buy")]
    //    public string Buy { get; set; }

    //    [JsonPropertyName("sell")]
    //    public string Sell { get; set; }
    //}

    //public class Status
    //{
    //    [JsonPropertyName("billing")]
    //    public Billing Billing { get; set; }

    //    [JsonPropertyName("buy")]
    //    public Buy Buy { get; set; }

    //    [JsonPropertyName("confirmed_email")]
    //    public bool? ConfirmedEmail { get; set; }

    //    [JsonPropertyName("shopping_cart")]
    //    public ShoppingCart ShoppingCart { get; set; }

    //    [JsonPropertyName("immediate_payment")]
    //    public bool? ImmediatePayment { get; set; }

    //    [JsonPropertyName("list")]
    //    public List List { get; set; }

    //    [JsonPropertyName("mercadoenvios")]
    //    public string Mercadoenvios { get; set; }

    //    [JsonPropertyName("mercadopago_account_type")]
    //    public string MercadopagoAccountType { get; set; }

    //    [JsonPropertyName("mercadopago_tc_accepted")]
    //    public bool? MercadopagoTcAccepted { get; set; }

    //    [JsonPropertyName("required_action")]
    //    public object RequiredAction { get; set; }

    //    [JsonPropertyName("sell")]
    //    public Sell Sell { get; set; }

    //    [JsonPropertyName("site_status")]
    //    public string SiteStatus { get; set; }

    //    [JsonPropertyName("user_type")]
    //    public string UserType { get; set; }

    //    [JsonPropertyName("restrictions_coliving")]
    //    public RestrictionsColiving RestrictionsColiving { get; set; }
    //}

    //public class Thumbnail
    //{
    //    [JsonPropertyName("picture_id")]
    //    public string PictureId { get; set; }

    //    [JsonPropertyName("picture_url")]
    //    public string PictureUrl { get; set; }
    //}

    //public class Transactions
    //{
    //    [JsonPropertyName("canceled")]
    //    public int? Canceled { get; set; }

    //    [JsonPropertyName("completed")]
    //    public int? Completed { get; set; }

    //    [JsonPropertyName("period")]
    //    public string Period { get; set; }

    //    [JsonPropertyName("ratings")]
    //    public Ratings Ratings { get; set; }

    //    [JsonPropertyName("total")]
    //    public int? Total { get; set; }

    //    [JsonPropertyName("not_yet_rated")]
    //    public NotYetRated NotYetRated { get; set; }

    //    [JsonPropertyName("unrated")]
    //    public Unrated Unrated { get; set; }
    //}

    //public class Unrated
    //{
    //    [JsonPropertyName("paid")]
    //    public object Paid { get; set; }

    //    [JsonPropertyName("total")]
    //    public object Total { get; set; }
    //}


}
