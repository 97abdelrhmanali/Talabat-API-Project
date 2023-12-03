using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Data;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Entity;

namespace Talabat.Repository.Data
{
    public class SeedData
    {
        public static async Task Seedasync(StoreContext context , ILoggerFactory loggerFactory)
        {
			try
			{
				if(context?.Brands.Count() == 0)
				{
					var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
					var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);

					foreach (var brand in Brands)
						await context.Brands.AddAsync(brand);

					await context.SaveChangesAsync();
                }
                if (context?.Categories.Count() == 0)
                {
                    var CategoriesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");
                    var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoriesData);

                    foreach (var Cat in Categories)
                        await context.Categories.AddAsync(Cat);

                    await context.SaveChangesAsync();
                }


                if (context?.Products.Count() == 0)
                {
                    var productsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    foreach (var product in products)
                        await context.Products.AddAsync(product);

                    await context.SaveChangesAsync();
                }

                if (context?.DeliveryMethods.Count() == 0)
                {
                    var DeliveryMethodData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                    var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodData);

                    foreach (var DeliveryMethod in DeliveryMethods)
                        await context.DeliveryMethods.AddAsync(DeliveryMethod);

                    await context.SaveChangesAsync();
                }
            }
			catch (Exception ex)
			{

                var logger = loggerFactory.CreateLogger<SeedData>();
                logger.LogError(ex, "Error Happen while adding Data");
			}
        } 
    }
}
