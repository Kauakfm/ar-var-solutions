using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class CancelamentoResponse
    {

        public string Id { get; set; }
        public string Code { get; set; }
        public DateTimeOffset StartAt { get; set; }
        public string Interval { get; set; }
        public long IntervalCount { get; set; }
        public string BillingType { get; set; }
        public CurrentCycle currentCycle { get; set; }
        public DateTimeOffset NextBillingAt { get; set; }
        public string PaymentMethod { get; set; }
        public string Currency { get; set; }
        public string StatementDescriptor { get; set; }
        public long Installments { get; set; }
        public long MinimumPrice { get; set; }
        public string Status { get; set; }
        public long BoletoDueDays { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset CanceledAt { get; set; }
        public Customer customer { get; set; }
        public Card card { get; set; }
        public Plan plan { get; set; }
        public List<Item> Items { get; set; }
        public Boleto boleto { get; set; }
        public Metadata metadata { get; set; }


        public partial class Boleto
        {
        }

        public partial class Card
        {
            public string Id { get; set; }
            public long FirstSixDigits { get; set; }
            public long LastFourDigits { get; set; }
            public string Brand { get; set; }
            public string HolderName { get; set; }
            public long ExpMonth { get; set; }
            public long ExpYear { get; set; }
            public string Status { get; set; }
            public string Type { get; set; }
            public DateTimeOffset CreatedAt { get; set; }
            public DateTimeOffset UpdatedAt { get; set; }
            public BillingAddress BillingAddress { get; set; }
        }

        public partial class BillingAddress
        {
            public string ZipCode { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Country { get; set; }
            public string Line1 { get; set; }
            public long Line2 { get; set; }
        }

        public partial class CurrentCycle
        {
            public string Id { get; set; }
            public DateTimeOffset StartAt { get; set; }
            public DateTimeOffset EndAt { get; set; }
            public DateTimeOffset BillingAt { get; set; }
            public string Status { get; set; }
            public long Cycle { get; set; }
        }

        public partial class Customer
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Code { get; set; }
            public string Document { get; set; }
            public string DocumentType { get; set; }
            public string Type { get; set; }
            public bool Delinquent { get; set; }
            public DateTimeOffset CreatedAt { get; set; }
            public DateTimeOffset UpdatedAt { get; set; }
            public Phones Phones { get; set; }
        }

        public partial class Phones
        {
            public EPhone HomePhone { get; set; }
            public EPhone MobilePhone { get; set; }
        }

        public partial class EPhone
        {
            public long CountryCode { get; set; }
            public long Number { get; set; }
            public long AreaCode { get; set; }
        }

        public partial class Item
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public long Quantity { get; set; }
            public string Status { get; set; }
            public DateTimeOffset CreatedAt { get; set; }
            public DateTimeOffset UpdatedAt { get; set; }
            public PricingScheme PricingScheme { get; set; }
        }

        public partial class PricingScheme
        {
            public long Price { get; set; }
            public string SchemeType { get; set; }
        }

        public partial class Metadata
        {
            public string Id { get; set; }
        }

        public partial class Plan
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Url { get; set; }
            public string StatementDescriptor { get; set; }
            public long MinimumPrice { get; set; }
            public string Interval { get; set; }
            public long IntervalCount { get; set; }
            public string BillingType { get; set; }
            public List<string> PaymentMethods { get; set; }
            public List<long> Installments { get; set; }
            public string Status { get; set; }
            public string Currency { get; set; }
            public DateTimeOffset CreatedAt { get; set; }
            public DateTimeOffset UpdatedAt { get; set; }
            public Metadata Metadata { get; set; }
        }
    }
}
