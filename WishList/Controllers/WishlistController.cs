using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ShopifySharp;
using WishList.Data;
using WishList.Models;
using Customer = WishList.Models.Customer;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WishList.Controllers
{
   

    [Route("api/[controller]")]
    //[EnableCors("CorsPolicy")]
    public class WishlistController : Controller
    {
        private readonly ShopifyContext _context;
        public WishlistController(ShopifyContext context)
        {
            _context = context;
          
        }

        // GET: api/<controller>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        [HttpGet]
        public  ActionResult<IEnumerable<Customer>> Get(string customer_id)
        {
            var cust = _context.Customer.Where(b => b.customer_id == customer_id).ToList();
            return cust;
        }


        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        [HttpPost]
        public async Task Post(Customer obj)
        {
           
            var shop = _context.Shopify.Single(b => b.StoreName == obj.StoreName);
            string Token = shop.Token;
            string store = shop.StoreName;
            var service = new ProductService(store, Token);
            var service1 = new CustomerService(store, Token);
            var service2 = new ProductVariantService(store, Token);
            var service3 = new ProductImageService(store, Token);


            var product = await service.GetAsync(long.Parse(obj.product_id));
            var customer = await service1.GetAsync(long.Parse(obj.customer_id));

            obj.customer_name = customer.FirstName;
            obj.customer_email = customer.Email;

            obj.product_title = product.Title;


            var variant = await service2.GetAsync(long.Parse(obj.variant_id));

            obj.variant_sku = variant.SKU;
            obj.variant_barcode = variant.Barcode;

            // var image = await service3.GetAsync(long.Parse(obj.product_id), (long)variant.ImageId);

            IEnumerable<ProductImage> obj1 = product.Images;
            obj.variant_image = obj1.ElementAt(0).Src;
                
            

            _context.Add(obj);
            await _context.SaveChangesAsync();

           
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
