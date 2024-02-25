using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class DoacaoResponse
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public bool Closed { get; set; }
        public List<OrderItem> Items { get; set; }
        public Customer Customer { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ClosedAt { get; set; }
        public List<Charge> Charges { get; set; }
        public List<Checkout> Checkouts { get; set; }
    }

    public class OrderItem
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Code { get; set; }
    }

    public class Customer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public string Document { get; set; }
        public string DocumentType { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public bool Delinquent { get; set; }
        public Address Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime Birthdate { get; set; }
        public Phones Phones { get; set; }
    }

    public class Address
    {
        public string Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class Phones
    {
        public Phone HomePhone { get; set; }
        public Phone MobilePhone { get; set; }
    }

    public class Phone
    {
        public string CountryCode { get; set; }
        public string Number { get; set; }
        public string AreaCode { get; set; }
    }

    public class Charge
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string GatewayId { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaidAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Customer Customer { get; set; }
        public Transaction LastTransaction { get; set; }
    }

    public class Transaction
    {
        public string Id { get; set; }
        public string TransactionType { get; set; }
        public string GatewayId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public bool Success { get; set; }
        public int Installments { get; set; }
        public string StatementDescriptor { get; set; }
        public string AcquirerName { get; set; }
        public string AcquirerTid { get; set; }
        public string AcquirerNsu { get; set; }
        public string AcquirerAuthCode { get; set; }
        public string AcquirerMessage { get; set; }
        public string AcquirerReturnCode { get; set; }
        public string OperationType { get; set; }
        public Card Card { get; set; }
        public string FundingSource { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public GatewayResponse GatewayResponse { get; set; }
        public AntifraudResponse AntifraudResponse { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
    }

    public class Card
    {
        public string Id { get; set; }
        public string FirstSixDigits { get; set; }
        public string LastFourDigits { get; set; }
        public string Brand { get; set; }
        public string HolderName { get; set; }
        public string HolderDocument { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Address BillingAddress { get; set; }
    }

    public class GatewayResponse
    {
        public string Code { get; set; }
        public List<string> Errors { get; set; }
    }

    public class AntifraudResponse
    {
        public string Status { get; set; }
        public string Score { get; set; }
        public string ProviderName { get; set; }
    }

    public class Checkout
    {
        // Propriedades da classe
    }

}
