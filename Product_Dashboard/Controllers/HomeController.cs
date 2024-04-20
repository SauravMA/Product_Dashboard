using Dapper;
using Microsoft.AspNetCore.Mvc;
using Product_Dashboard.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;


namespace Product_Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("YourConnectionString");
        }


        public IActionResult Index()
        {
            IEnumerable<YourModel> data;

            using (var db = new SqlConnection(_connectionString))
            {
                db.Open();
                data = db.Query<YourModel>("SELECT * FROM Product_List");
            }

            return View(data);

        }

        [HttpPost]

        public IActionResult Insert(string pname, string desc, string price, string pdate)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                db.Open();

              
                var insertQuery = "INSERT INTO Product_List (Product_Name, Discription, Product_Price, Product_Date) VALUES (@ProductName, @ProductDescription, @ProductPrice, @ProductDate)";

                db.Execute(insertQuery, new { ProductName = pname, ProductDescription = desc, ProductPrice = price, ProductDate = pdate });
            }

            return Json("Successful");
        }

        [HttpPost]

        public IActionResult Update(int sn, string pname, string disc, string price, string pdate)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                db.Open();

                var updateQuery = @"
                UPDATE Product_List 
                SET Product_Name = @ProductName, 
                    Discription = @ProductDescription, 
                    Product_Price = @ProductPrice, 
                    Product_Date = @ProductDate 
                WHERE SN_Id= @ProductId";

                db.Execute(updateQuery, new { ProductId = sn, ProductName = pname, ProductDescription = disc, ProductPrice = price, ProductDate = pdate });
            }

            return Json("Successful");
        }

        [HttpPost]

        public IActionResult Delete(int sn)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                db.Open();

                var deleteQuery = @"
            DELETE FROM Product_List 
            WHERE SN_Id = @ProductId";

                db.Execute(deleteQuery, new { ProductId = sn });
            }

            return Json("Successful");
        }

        public class YourModel
        {
            public int SN_Id { get; set; }
            public string Product_Name { get; set; }

            public string Discription { get; set;}

            public string Product_Price { get; set;}

            public string Product_Date { get; set; }
        }

        


    }
}
