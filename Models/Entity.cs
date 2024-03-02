using System;
using System.Collections.Generic;

namespace KYC360.Models
{
    public class Entity
    {
        public string Id { get; set; }
        public string Gender { get; set; }
        public bool Deceased { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Date> Dates {  get; set; }
        public ICollection<Name> Names { get; set; }

    }
}
