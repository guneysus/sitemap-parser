using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polly;
using sample_ecommerce_poco;
using ServiceStack.Redis;
using sitemap_reader;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sample_ecommerce_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IRedisClientsManager redisClientsManager;

        public ProductController(IProductService productService, IRedisClientsManager redisClientsManager)
        {
            this.productService = productService;
            this.redisClientsManager = redisClientsManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            Policy.Handle<ServiceStack.Redis.RedisException>(ex => true)
            .WaitAndRetry(4, retry =>
            {
                double value = Math.Pow(2, retry - 2);
                TimeSpan waitingTime = TimeSpan.FromSeconds(value);
                Console.WriteLine($"trial: {retry} | waiting for {waitingTime}");
                return waitingTime;
            })

            .Execute(() =>
            {
                var redisClient = redisClientsManager.GetClient();
                var nativeClient = (IRedisNativeClient)redisClient;

                redisClient.Increment("visit:api:product", 1);
                nativeClient.Incr("visit:api:product");
            });


            IRedisClient client = redisClientsManager.GetClient();

            //if (client.ContainsKey("products"))
            //{
            //    return Ok(client.Get<IList<Product>>("products"));
            //}

            var products = productService.GetAll(100, 0);

            foreach (var item in products)
            {
                //client.SetEntryInHash($"product:{item.Id}", "Id", item.Id.ToString());
                //client.SetEntryInHash($"product:{item.Id}","Link", item.Link);
                //client.SetEntryInHash($"product:{item.Id}", "Image", item.Image);
                //client.SetEntryInHash($"product:{item.Id}", "Price", item.Price.ToString());
                //client.SetEntryInHash($"product:{item.Id}", "SalesPrice", item.SalesPrice.ToString());
                //client.SetEntryInHash($"product:{item.Id}", "Title", item.Title);

                client.SetRangeInHash($"product:{item.Id}", JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(item)));
            }

            client.Add("products", products);

            return base.Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            IRedisClient redisClient = redisClientsManager.GetClient();
            var nativeClient = (IRedisNativeClient)redisClient;
            //return Ok(nativeClient.HGetAll("product:1"));
            Dictionary<string, string> result = redisClient.GetAllEntriesFromHash($"product:{id}");
            return base.Ok(result);
        }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IRedisClientsManager redisClientsManager;

        public ShoppingCartController(IRedisClientsManager redisClientsManager)
        {
            this.redisClientsManager = redisClientsManager;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id, int userId = 1)
        {
            IRedisClient redisClient = redisClientsManager.GetClient();
            redisClient.IncrementValueInHash($"user:{userId}:basket", $"{id}", 1);
            return Ok();
        }
    }

    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IRedisClientsManager redisClientsManager;
        private readonly IProductService productService;

        public BasketController(IRedisClientsManager redisClientsManager, IProductService productService)
        {
            this.redisClientsManager = redisClientsManager;
            this.productService = productService;
        }

        [HttpGet("{id}/detail")]
        public IActionResult Get(int id)
        {
            IRedisClient redisClient = redisClientsManager.GetClient();
            //var keys = redisClient.GetHashKeys($"user:{id}:basket");

            var basketSummary = redisClient.GetAllEntriesFromHash($"user:{id}:basket");

            var items = new List<ShoppingCartItem>();


            foreach (var kv in basketSummary)
            {
                var productId = int.Parse(kv.Key);
                var quantity = int.Parse(kv.Value);
                var product = productService.Get(productId);
                items.Add(new ShoppingCartItem() {
                    Product = product,
                    Quantity = quantity
                });

            }

            return base.Ok(new
            {
                objects = items
            });
        }
    }

    public class ShoppingCartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }


    public class ProductService : IProductService
    {
        private readonly IRedisClientsManager redisClientsManager;

        public ProductService(IRedisClientsManager redisClientsManager)
        {
            this.redisClientsManager = redisClientsManager;
        }

        Product IProductService.Get(int id)
        {
            var redisClient = redisClientsManager.GetClient();
            string hashId = $"product:{id}";
            //var keys = redisClient.GetHashKeys(hashId);

            //var product = redisClient.GetFromHash<Product>($"product:{id}");

            var allEntries = redisClient.GetAllEntriesFromHash(hashId);

            var product = JsonConvert.DeserializeObject<Product>(JsonConvert.SerializeObject(allEntries));

            //throw new NotImplementedException();
            return product;
        }

        IList<Product> IProductService.GetAll(int limit, int skip)
        {
            return SitemapParser
                .Parse<Urlset>(@"C:\Users\ahmed.guneysu\workspace\repos\sample-ecommerce\data\sitemap_244.xml")
                .Url
                .Skip(skip)
                .Take(limit)
                .Select(x => new Product()
                {
                    Id = Faker.RandomNumber.Next(1, 100),
                    SalesPrice = Faker.RandomNumber.Next(6, 999) + Math.Round(Convert.ToDecimal(new Random().NextDouble() * 10), 2),
                    Price = Faker.RandomNumber.Next(6, 999) + Math.Round(Convert.ToDecimal(new Random().NextDouble() * 10), 2),
                    Title = x.Image?.Title,
                    Image = x.Image?.Loc,
                    Link = x?.Loc,
                }).ToList();
        }
    }

    public interface IProductService
    {
        IList<Product> GetAll(int limit, int skip = 0);
        Product Get(int id);
    }
}
