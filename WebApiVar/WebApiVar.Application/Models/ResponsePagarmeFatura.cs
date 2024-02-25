using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebApiVar.Application.Models
{
    public class ResponsePagarmeFatura
    {



        [JsonProperty("data")]
        public List<Datum> Data { get; set; }

        [JsonProperty("paging")]
        public paging Paging { get; set; }


        public partial class Datum
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("amount")]
            public long Amount { get; set; }

            [JsonProperty("total_discount")]
            public long TotalDiscount { get; set; }

            [JsonProperty("total_increment")]
            public long TotalIncrement { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("payment_method")]
            public string PaymentMethod { get; set; }

            [JsonProperty("due_at")]
            public DateTimeOffset DueAt { get; set; }

            [JsonProperty("created_at")]
            public DateTimeOffset CreatedAt { get; set; }

            [JsonProperty("items")]
            public List<Item> Items { get; set; }

            [JsonProperty("customer")]
            public Customer Customer { get; set; }

            [JsonProperty("subscription")]
            public Subscription Subscription { get; set; }

            [JsonProperty("cycle")]
            public Cycle Cycle { get; set; }

            [JsonProperty("billing_address")]
            public BillingAddress BillingAddress { get; set; }

            [JsonProperty("charge")]
            public Charge Charge { get; set; }

            [JsonProperty("metadata")]
            public Metadata Metadata { get; set; }
        }

        public partial class BillingAddress
        {
            [JsonProperty("zip_code")]
            public string ZipCode { get; set; }

            [JsonProperty("city")]
            public string City { get; set; }

            [JsonProperty("state")]
            public string State { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }

            [JsonProperty("line_1")]
            public string Line1 { get; set; }

            [JsonProperty("line_2")]
            public long Line2 { get; set; }
        }

        public partial class Charge
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("gateway_id")]
            public string GatewayId { get; set; }

            [JsonProperty("amount")]
            public long Amount { get; set; }

            [JsonProperty("paid_amount")]
            public long PaidAmount { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("currency")]
            public string Currency { get; set; }

            [JsonProperty("payment_method")]
            public string PaymentMethod { get; set; }

            [JsonProperty("due_at")]
            public DateTimeOffset DueAt { get; set; }

            [JsonProperty("paid_at")]
            public DateTimeOffset PaidAt { get; set; }

            [JsonProperty("created_at")]
            public DateTimeOffset CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public DateTimeOffset UpdatedAt { get; set; }

            [JsonProperty("metadata")]
            public Metadata Metadata { get; set; }

            [JsonProperty("recurrence_cycle")]
            public string RecurrenceCycle { get; set; }
        }

        public partial class Metadata
        {
            [JsonProperty("id")]
            public string Id { get; set; }
        }

        public partial class Customer
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("document")]
            public string Document { get; set; }

            [JsonProperty("document_type")]
            public string DocumentType { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("delinquent")]
            public bool Delinquent { get; set; }

            [JsonProperty("created_at")]
            public DateTimeOffset CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public DateTimeOffset UpdatedAt { get; set; }

            [JsonProperty("phones")]
            public Phones Phones { get; set; }
        }

        public partial class Phones
        {
            [JsonProperty("home_phone")]
            public EPhone HomePhone { get; set; }

            [JsonProperty("mobile_phone")]
            public EPhone MobilePhone { get; set; }
        }

        public partial class EPhone
        {
            [JsonProperty("country_code")]
            public long CountryCode { get; set; }

            [JsonProperty("number")]
            public long Number { get; set; }

            [JsonProperty("area_code")]
            public long AreaCode { get; set; }
        }

        public partial class Cycle
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("start_at")]
            public DateTimeOffset StartAt { get; set; }

            [JsonProperty("end_at")]
            public DateTimeOffset EndAt { get; set; }

            [JsonProperty("billing_at")]
            public DateTimeOffset BillingAt { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("cycle")]
            public long CycleCycle { get; set; }
        }

        public partial class Item
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("amount")]
            public long Amount { get; set; }

            [JsonProperty("quantity")]
            public long Quantity { get; set; }
        }

        public partial class Subscription
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("start_at")]
            public DateTimeOffset StartAt { get; set; }

            [JsonProperty("interval")]
            public string Interval { get; set; }

            [JsonProperty("interval_count")]
            public long IntervalCount { get; set; }

            [JsonProperty("billing_type")]
            public string BillingType { get; set; }

            [JsonProperty("next_billing_at")]
            public DateTimeOffset NextBillingAt { get; set; }

            [JsonProperty("payment_method")]
            public string PaymentMethod { get; set; }

            [JsonProperty("currency")]
            public string Currency { get; set; }

            [JsonProperty("statement_descriptor")]
            public string StatementDescriptor { get; set; }

            [JsonProperty("installments")]
            public long Installments { get; set; }

            [JsonProperty("minimum_price")]
            public long MinimumPrice { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("boleto_due_days")]
            public long BoletoDueDays { get; set; }

            [JsonProperty("created_at")]
            public DateTimeOffset CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public DateTimeOffset UpdatedAt { get; set; }

            [JsonProperty("canceled_at")]
            public DateTimeOffset CanceledAt { get; set; }

            [JsonProperty("metadata")]
            public Metadata Metadata { get; set; }
        }

        public partial class paging
        {
            [JsonProperty("total")]
            public long Total { get; set; }
        }
    }
}
