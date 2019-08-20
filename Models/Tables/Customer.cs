using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace projectName.Models{  //change projectName to the name of project
    
public class Customer{
    [Key]
    public int CustomerId{get;set;}
    [Required]
    [MinLength(2)]
    public string FirstName{get;set;}
    [Required]
    [MinLength(3)]
    public string LastName{get;set;}
    [Required]
    [EmailAddress]
    public string Email{get;set;}

    public List<Order> Orders{get;set;}

    public DateTime CreatedAt{get;set;} = DateTime.Now;
    public DateTime UpdatedAt{get;set;} = DateTime.Now;
}

}