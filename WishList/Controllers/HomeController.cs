using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShopifySharp;
using ShopifySharp.Enums;
using WishList.Data;
using WishList.Models;
using Customer = WishList.Models.Customer;

namespace WishList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShopifyContext _context;
        private readonly IConfiguration _config;
        public HomeController(ShopifyContext context,IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        public IActionResult Install()
        {
          
             return View();

        }

        [HttpPost]
        public IActionResult Install(string store)
        {

            // A URL to redirect the user to after they've confirmed app installation.
            // This URL is required, and must be listed in your app's settings in your Shopify app dashboard.
            // It's case-sensitive too!
            string redirectUrl = "https://4f9ff7c8.ngrok.io/home/oauth";

            //An array of the Shopify access scopes your application needs to run.
            var scopes = new List<AuthorizationScope>()
            {
                AuthorizationScope.ReadCustomers,
                AuthorizationScope.ReadThemes,
                AuthorizationScope.WriteThemes,
                AuthorizationScope.ReadProducts,
                AuthorizationScope.WriteContent,
               
            };


            //All AuthorizationService methods are static.
            Uri appUrl = AuthorizationService.BuildAuthorizationUrl(scopes, store,_config["Keys:ApiKey"], redirectUrl);
            return Redirect(appUrl.ToString());

           
        }


        public async Task<IActionResult> Oauth(string code,string shop)
        {
            string qs = Request.QueryString.ToString();
            if (AuthorizationService.IsAuthenticRequest(qs, _config["Keys:SecretKey"]))
            {
                string accessToken = await AuthorizationService.Authorize(code, shop, _config["Keys:ApiKey"], _config["Keys:SecretKey"]);
                Shopify shops = new Shopify();
                shops.StoreName = shop;
                shops.Token = accessToken;
                _context.Add(shops);
                await _context.SaveChangesAsync();


                await createTemplateAsync(shops.StoreName, shops.Token);

                string a = shop;
                return RedirectToAction("Index", "Home", new

                {
    
                    shop = a,
       
                });
            }
            return View();
        }

        public IActionResult Index(string shop)
        {

            ViewBag.Api = _config["Keys:ApiKey"];
            ViewBag.Address = "https://" + shop;
            ViewBag.Store = shop;

           

            return View(_context.Customer.Where(b => b.StoreName == shop).ToList());
        }

       



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task createTemplateAsync(string shop,string token)
        {
            var service = new ThemeService(shop, token);
            var service1 = new AssetService(shop, token);
            var service2 = new PageService(shop, token);

            var page = new Page()
            {
                CreatedAt = DateTime.UtcNow,
                Title = "Wishlist",
                TemplateSuffix = "wishlist",
            };


            var asset = new Asset()
            {
                ContentType = "page.wishlist/x-liquid",
                Key = "templates/page.wishlist.liquid",
                Value = "<input type=\"hidden\" name = \"customer_id\" value =\"{{ customer.id }}\" id =\"customer_id\">\n<table>\n<tr>\n<th> Product Image </th>\n<th> Product Title </th>\n<th> SKU </th>\n<th> Barcode </th>\n<th> Customer Name </th>\n<th> Customer Email </th>\n</tr>\n<tbody id = \"todos\"></tbody>\n</table>\n " +
                "<script type =\"text/javascript \">\n" +
                "$(document).ready(function() { \n" +
                "$.ajax({" +
                "type: 'GET',\n" +
                "url: 'http://4f9ff7c8.ngrok.io/api/wishlist', \n" +
                "cache: false,\n" +
                "crossDomain: true,\n" +
                "data: { \n" +
                "customer_id:$('#customer_id').val()\n" +
                " },\n" +
                "success: function(data) {\n" +
                 "const tBody = $('#todos');\n" +
                 " $(tBody).empty();\n" +

                "$.each(data, function(key, item) {\n" +
                "const tr = $('<tr></tr>')\n" +
                ".append($('<td></td>').append( $('<img />', {\n" +
                " src: item.variant_image ,\n" +
                "style: 'width: 20%'\n" +
                "})))\n" +
               ".append($('<td></td>').text(item.product_title))\n" +
               ".append($('<td></td>').text(item.variant_sku))\n" +
               ".append($('<td></td>').text(item.variant_barcode))\n" +
               ".append($('<td></td>').text(item.customer_name))\n" +
               ".append($('<td></td>').text(item.customer_email));\n" +
               "tr.appendTo(tBody);\n" +
               "});\n" +

               "},\n" +
               "error: function()\n" +
               "{\n" +
               "alert('Error!');\n" +
               "}\n" +
               "});\n" +
               "});\n" +
               "</script>"
            };

            var themes = await service.ListAsync();


            for (int i = 0; i < themes.Count(); i++)
            {
                if (themes.ElementAt(i).Role == "main")
                {
                    
                    asset = await service1.CreateOrUpdateAsync((long)themes.ElementAt(i).Id, asset);
                    page = await service2.CreateAsync(page);

                 }
            }
        }
    }
}
