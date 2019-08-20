using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace projectName.Models{  //change projectName to the name of project
    
    public class Order{
        [Key]
        public int id{get;set;}
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity{get;set;}

        public int CustomerId{get;set;}
        public Customer Customer{get;set;}
        public int ProductId{get;set;}
        public Product Product{get;set;}

        public DateTime CreatedAt{get;set;} = DateTime.Now;
        public DateTime UpdatedAt{get;set;} = DateTime.Now;
    }

}

