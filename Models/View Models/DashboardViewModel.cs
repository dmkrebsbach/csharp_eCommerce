using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace projectName.Models{  //change projectName to the name of project

    public class DashboardViewModel
    {
        public Order newOrder {get;set;}  // public "ClassName" should match the class name in the file, not the file name itself
        public List<Order> Orders{get;set;}
        public List<Customer> Customers{get;set;}
        public List<Product> Products{get;set;}
    }
}