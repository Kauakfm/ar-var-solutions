using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class AdoteumAlunoResponse
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public DateTime StartAt { get; set; }
        public string Interval { get; set; }
        public int IntervalCount { get; set; }
        public string BillingType { get; set; }
        public CurrentCycle currentCycle { get; set; }
        public DateTime NextBillingAt { get; set; }
        public string PaymentMethod { get; set; }
        public string Currency { get; set; }
        public string StatementDescriptor { get; set; }
        public int Installments { get; set; }
        public int MinimumPrice { get; set; }
        public string Status { get; set; }
        public int BoletoDueDays { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Customer customer { get; set; }
        public Card card { get; set; }
        public Plan plan { get; set; }
        public List<Item> items { get; set; }
        public Boleto boleto { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
        public class Customer
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Document { get; set; }
            public string DocumentType { get; set; }
            public string Type { get; set; }
            public bool Delinquent { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public Phones phones { get; set; }
            public class Phones
            {
                public Phone HomePhone { get; set; }
                public Phone MobilePhone { get; set; }
                public class Phone
                {
                    public string CountryCode { get; set; }
                    public string Number { get; set; }
                    public string AreaCode { get; set; }
                }
            }

        }
        public class CurrentCycle
        {
            public string Id { get; set; }
            public DateTime StartAt { get; set; }
            public DateTime EndAt { get; set; }
            public DateTime BillingAt { get; set; }
            public string Status { get; set; }
            public int Cycle { get; set; }
        }
        public class Plan
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Url { get; set; }
            public string StatementDescriptor { get; set; }
            public int MinimumPrice { get; set; }
            public string Interval { get; set; }
            public int IntervalCount { get; set; }
            public string BillingType { get; set; }
            public List<string> PaymentMethods { get; set; }
            public List<int> Installments { get; set; }
            public string Status { get; set; }
            public string Currency { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public Dictionary<string, object> Metadata { get; set; }
        }
        public class Item
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int Quantity { get; set; }
            public string Status { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public PricingScheme PricingScheme { get; set; }
        }

        public class PricingScheme
        {
            public int Price { get; set; }
            public string SchemeType { get; set; }
        }
        public class Card
        {
            public string Id { get; set; }
            public string FirstSixDigits { get; set; }
            public string LastFourDigits { get; set; }
            public string Brand { get; set; }
            public string HolderName { get; set; }
            public int ExpMonth { get; set; }
            public int ExpYear { get; set; }
            public string Status { get; set; }
            public string Type { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public Address BillingAddress { get; set; }
            public class Address
            {
                public string Street { get; set; }
                public string Complement { get; set; }
                public string ZipCode { get; set; }
                public string City { get; set; }
                public string State { get; set; }
                public string Country { get; set; }
                public string Line1 { get; set; }
                public string Line2 { get; set; }
            }
        }
        public class Boleto
        {
            // Propriedades da classe
        }
    }
}

