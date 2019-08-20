using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace projectName.Models{  //change projectName to the name of project
    
    public class Product{
        [Key]
        public int id{get;set;}
        [Required(ErrorMessage = "Name is Required")]
        [MinLength(3)]
        public string Name{get;set;}
        [Url]
        public string Url{get;set;}
        [MinLength(10)]
        public string Description{get;set;}
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity{get;set;}

        public List<Order> Orders{get;set;}

        public DateTime CreatedAt{get;set;} = DateTime.Now;
        public DateTime UpdatedAt{get;set;} = DateTime.Now;
    } 

}