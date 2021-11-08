using System;
using System.Collections.Generic;

namespace Vidly.Models.ViewModels
{
    public class NewCustomerViewModel
    {
        public Customers Customers { get; set; }

        public List<MembershipType> MembershipType { get; set; }

        /* 
         public List<MembershipType> MembershipType { get; set; } - Can do this but in the view we dont need any of the functionality from the LIST type
        like adding an object, etc so can just make it IEnumerable to be able to iterate over it
        
         
         For the Customers we can use the Customer type OR explicity add the Customer Properties like NAme, Birthdate, etc
            - If can reuse the existing domain object that should be the preference

            - But sometimes in large apps how you present an entity on a view can be different from how its defined in the domain model of the application
                - Then you might end up polluting the Model with properites that are only used by one view - in this case you must seperate each DOMAIN and VIEW model
                    and let each develop seperatly 
         
         */

    }
}

