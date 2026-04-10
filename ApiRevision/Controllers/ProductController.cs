using ApiRevision.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ApiRevision.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        // GET: ProductController

        private readonly IConfiguration _configuration;

        

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = GetConnection())
            {
                SqlCommand sqlCommand = new SqlCommand("Select * from Product", connection);

                connection.Open();

                SqlDataReader dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    products.Add(new Product
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = dr["Name"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"])
                    });

                }
            }
            return Ok(products);
        }
        
        // GET: ProductController/Details/5
        [HttpGet("{id}")]
        public ActionResult Details(int id)
        {
            Product product = null;
            using (SqlConnection connection = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("Select * from Product where Id = @id", connection);
                cmd.Parameters.AddWithValue("@Id", id);
                connection.Open();
                
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.Read())
                {
                    product = new Product
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = dr["Name"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"])
                    };
                }

            }
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // GET: ProductController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: ProductController/Create
        [HttpPost("create")]
        public ActionResult Create(Product collection)
        {
            try
            {
                

                using (SqlConnection connection = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("Insert into Product(Name,Price) Values(@Name,@Price)", connection);

                    cmd.Parameters.AddWithValue("Name", collection.Name);
                    cmd.Parameters.AddWithValue("Price", collection.Price);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                return Ok("Product Created Successfully");
            }
            catch
            {
                return Ok("Product Not created");
            }
        }

        //// GET: ProductController/Edit/5
        //[HttpGet("{id}")]
        //public ActionResult Edit(int id)
        //{
        //    Product product = null;
        //    using(SqlConnection con = GetConnection())
        //    {
        //        SqlCommand cmd = new SqlCommand("select * from Product where Id = @id", con);
        //        cmd.Parameters.AddWithValue("Id", id);
                
        //        SqlDataReader dr = cmd.ExecuteReader();

        //        product.Add(new Product
        //        {
        //            Name = @
        //        })

                
        //    }
        //    return View();
        //}

        // POST: ProductController/Edit/5

        [HttpPost("edit")]
        public ActionResult Edit(Product product)
        {
            try
            {
                int id = product.Id;
                using (SqlConnection connection = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("Update Product set Name = @Name,Price = @Price Where Id = @id",connection);
                    cmd.Parameters.AddWithValue("Id", id);
                    cmd.Parameters.AddWithValue("Name", product.Name);
                    cmd.Parameters.AddWithValue("Price", product.Price);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }

                return Ok("Product Updated Successfully");
            }
            catch
            {
                return BadRequest("Product Not Updated");
            }
        }

        // GET: ProductController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: ProductController/Delete/5
        [HttpPost("delete/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                using(SqlConnection connection = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("Delete from Product where Id = @id", connection);
                    cmd.Parameters.AddWithValue("Id", id);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                return Ok("Product Deleted Successfully");
            }
            catch
            {
                return BadRequest("Error to delete Product");
            }
        }
    }
}
