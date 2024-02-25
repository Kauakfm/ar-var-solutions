using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVar.Repository.Models;
using static WebApiVar.Application.Models.PlanoPagarMeRequest.Card;

namespace WebApiVar.Application.Models
{
    public class DoacaoRequest
    {
        public Costumer customer { get; set; }
        public List<Items> items { get; set; }
        public List<Payments> payments { get; set; }
        public bool antifraud_enabled { get; set; }
        public class Costumer
        {
            public Address address { get; set; }
            public Phones phones { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string email { get; set; }
            public string code { get; set; }
            public string document { get; set; }
            public string document_type { get; set; }
            public string gender { get; set; }
            public string birthdate { get; set; }
            public class Address
            {
                public string country { get; set; }
                public string state { get; set; }
                public string city { get; set; }
                public string zip_code { get; set; }
                public string line_1 { get; set; }
                public string line_2 { get; set; }
            }
            public class Phones
            {
                public Mobile_phone mobile_phone { get; set; }

                public class Mobile_phone
                {
                    public string country_code { get; set; }
                    public string area_code { get; set; }
                    public string number { get; set; }
                }
            }
        }


        public class Items
        {
            public int amount { get; set; }
            public string description { get; set; }
            public int quantity { get; set; }
            public string code { get; set; }
        }
        public class Payments
        {
            public Credit_card credit_card { get; set; }
            public string payment_method { get; set; }
            public class Credit_card
            {
                public Card card { get; set; }
                public string operation_type { get; set; }
                public int installments { get; set; }
                public string statement_descriptor { get; set; }
                public class Card
                {
                    public string number { get; set; }
                    public string holder_name { get; set; }
                    public int exp_month { get; set; }
                    public int exp_year { get; set; }
                    public string cvv { get; set; }
                    public Billing_address billing_address { get; set; }
                    public class Billing_address
                    {
                        public string country { get; set; }
                        public string state { get; set; }
                        public string city { get; set; }
                        public string zip_code { get; set; }
                        public string line_1 { get; set; }
                        public string line_2 { get; set; }
                    }
                }
            }

        }

    }

}
