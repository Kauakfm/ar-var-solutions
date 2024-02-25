using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVar.Application.Models
{
    public class PlanoPagarMeRequest
    {
        public Costumer customer { get; set; }
        public Card card { get; set; }
        public string plan_id { get; set; }
        public string payment_method { get; set; }
        public int boleto_due_days { get; set; }
        public Metadata metadata { get; set; }

        public class Metadata
        {
            public string id { get; set; }
        }
        public class Costumer
        {
            public Phones phones { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string email { get; set; }
            public string code { get; set; }
            public string document { get; set; }
            public string document_type { get; set; }
            public string gender { get; set; }
            public string birthdate { get; set; }
            public class Phones
            {
                public Mobile_phone mobile_phone { get; set; }
                public Home_phone home_phone { get; set; }

                public class Mobile_phone
                {
                    public string country_code { get; set; }
                    public string area_code { get; set; }
                    public string number { get; set; }
                }
                public class Home_phone
                {
                    public string country_code { get; set; }
                    public string area_code { get; set; }
                    public string number { get; set; }
                }
            }
        }
        public class Card
        {
            public Billing_address billing_address { get; set; }
            public string number { get; set; }
            public string holder_name { get; set; }
            public int exp_month { get; set; }
            public int exp_year { get; set; }
            public string cvv { get; set; }
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
