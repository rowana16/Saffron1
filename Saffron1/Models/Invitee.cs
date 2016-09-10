using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saffron1.Models
{
    public class Invitee
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool BeenUsed { get; set; }

        public int HouseholdId { get; set; }

        public virtual Household Household { get; set; }
    }
}