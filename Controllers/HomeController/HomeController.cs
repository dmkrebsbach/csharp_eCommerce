using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Http; // FOR USE OF SESSIONS
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using projectName.Models; //change projectName to the name of project

namespace projectName.Controllers  //change projectName to the name of project
{
    public class HomeController : Controller{
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]               // GETS Main Registration and Login Page
        public IActionResult Index(){
            return View("Login");
        }

        [HttpPost("register")]
        public IActionResult CreateUser(LoginViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == viewModel.newUser.Email))
                {
                    ModelState.AddModelError("newUser.Email", "Email already in use!");
                    return View("Login");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                viewModel.newUser.Password = Hasher.HashPassword(viewModel.newUser, viewModel.newUser.Password);

                dbContext.Users.Add(viewModel.newUser);
                dbContext.SaveChanges();

                HttpContext.Session.SetInt32("userInSess", viewModel.newUser.UserId);

                return RedirectToAction("Success");
            }
            else
            {
                return View("Login");
            }
        }


        [HttpPost("login")]
        public IActionResult LoginUser(LoginViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                var dbUser = dbContext.Users.FirstOrDefault(u => u.Email == viewModel.newLogin.loginEmail);
                if(dbUser == null)
                {
                    ModelState.AddModelError("newLogin.loginEmail", "Email does not exist; please create account");
                    return View("Login");
                }

                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(viewModel.newLogin, dbUser.Password, viewModel.newLogin.loginPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("newLogin.loginPassword", "Password does not match Account on File");
                    return View("Login");
                }
                HttpContext.Session.SetInt32("userInSess", dbUser.UserId);

                return RedirectToAction("Success");
            }
            else
            {
                return View("Login");
            }
        }

        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("/success")]
        public IActionResult Success()
        {
            return View("Dashboard"); // RETURN REDIRECT TO FIRST RENDERED PAGE UPON SUCCESSFUL LOGIN/REG
        }

        [HttpGet("/dashboard")]
        public IActionResult Dashboard()
        {
            DashboardViewModel viewModel= new DashboardViewModel();

            viewModel.Products = dbContext.Products
                .Where(p => p.Quantity > 0)
                .Take(3)
                .ToList();
            viewModel.Orders = dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .OrderBy(o => o.CreatedAt)
                .Take(3)
                .ToList();
            viewModel.Customers = dbContext.Customers
                .OrderBy(u => u.CreatedAt)
                .Take(3)
                .ToList();

            return View("Dashboard", viewModel); // RETURN REDIRECT TO FIRST RENDERED PAGE UPON SUCCESSFUL LOGIN/REG
        }

        // The rest of the Controller Code goes here (routes, Posts, Gets, Linq, etc)

        [HttpGet("/customers")]
        public IActionResult ViewCustomers()
        {
            CustomerViewModel viewModel = new CustomerViewModel();

            viewModel.Customers = dbContext.Customers.ToList();
            return View("Customers", viewModel); // RETURN REDIRECT TO FIRST RENDERED PAGE UPON SUCCESSFUL LOGIN/REG
        }

        [HttpPost("customer/create")]
        public IActionResult CreateCustomer(CustomerViewModel viewModel){
            if(ModelState.IsValid){
                dbContext.Customers.Add(viewModel.newCustomer);
                dbContext.SaveChanges();

                return RedirectToAction("ViewCustomers");
            }

            CustomerViewModel newViewModel = new CustomerViewModel();

            return View("customers", newViewModel);
        }

        [HttpPost("product/create")]
        public IActionResult CreateProduct(ProductViewModel viewModel){
            if(ModelState.IsValid){
                dbContext.Products.Add(viewModel.newProduct);
                dbContext.SaveChanges();
                System.Console.WriteLine("THIS SHIT WAS CREATED THIS SHIT WAS CREATED SHIT ASLDK FJADSLKF JASDLKF JASDLKFJ ASDLKJ F");
            }

            return RedirectToAction("Products");
        }

        [HttpGet("/products")]
        public IActionResult Products(){
            ProductViewModel viewModel = new ProductViewModel();

            viewModel.Products = dbContext.Products
                .Where(p => p.Quantity > 0)
                .ToList();

            return View("Products", viewModel);
        }

        [HttpGet("orders")]
        public IActionResult Orders(){

            OrderViewModel viewModel = new OrderViewModel();

            viewModel.Orders = dbContext.Orders
            .Include(o => o.Customer)
            .Include(o => o.Product)
            .ToList();

            viewModel.Customers = dbContext.Customers.ToList();
            viewModel.Products = dbContext.Products.ToList();

            return View("Orders", viewModel);
        }

        [HttpPost("order/create")]
        public IActionResult CreateOrder(OrderViewModel viewModel){
            if(ModelState.IsValid){
                Product product = dbContext.Products
                    .FirstOrDefault(p => p.id == viewModel.newOrder.ProductId);
                if(product.Quantity < viewModel.newOrder.Quantity){
                    ModelState.AddModelError("Quantity", "You have tried to order more than are available");

                    OrderViewModel otherOrderView = new OrderViewModel();
                    return View("orders", otherOrderView);
                }

                product.Quantity -=  viewModel.newOrder.Quantity;

                dbContext.Orders.Add(viewModel.newOrder);

                dbContext.SaveChanges();

                return RedirectToAction("Orders");
            }

            OrderViewModel orderView = new OrderViewModel();
                viewModel.Orders = dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .ToList();

                viewModel.Customers = dbContext.Customers.ToList();
                viewModel.Products = dbContext.Products.ToList();

            return View("Orders", orderView);
        }


    }
}