using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
    public class Customers
    {
        public int Id { get; set; }

        [NotEmpty]
        //[Required]
        public string Name { get; set; }

        public bool IsSubscribedToNewsletter { get; set; }

        // This is a navigation property b/c allow to navigate from one model to another - useful when want to load an object and its related objects together from a DB 
        public MembershipType MembershipType { get; set; }

        [Display(Name = "Membership Type")]
        //Sometimes for optimization dont want to load the entire membership object just the FK so EF recognizes this convention and treats this prop name and the FK 
        public byte MembershipTypeId { get; set; }

        [Min18YearsIfMember]
        [Display(Name="Date Of Birth")] // - This works but have to recompile code everytime to change the label in HTML
        public DateTime Birthdate { get; set; }

    }

}

