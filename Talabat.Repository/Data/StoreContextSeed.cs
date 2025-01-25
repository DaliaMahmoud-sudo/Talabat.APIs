using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Talabat.Core.Entites;
using Talabat.Core.Entites.OrderAggregate;


namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {
            if (!dbContext.productBrands.Any())
            {
                var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                foreach (var Brand in Brands)

                    await dbContext.Set<ProductBrand>().AddAsync(Brand);

                await dbContext.SaveChangesAsync();
            }
            //seedinng types

            if (!dbContext.productTypes.Any())
            {
                var TypesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                foreach (var Type in Types)
                {
                    await dbContext.Set<ProductType>().AddAsync(Type);
                }
                await dbContext.SaveChangesAsync();
            }
            //seeding product

            if (!dbContext.products.Any())
            {
                var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                foreach (var Product in Products)
                {
                    await dbContext.Set<Product>().AddAsync(Product);
                }
                await dbContext.SaveChangesAsync();

            }
            //seeding delivery
            if (!dbContext.DeliveryMethod.Any())
            {
                var DeliveryMethodData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var DeliveryMethod = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodData);
                foreach (var DeliveryMethods in DeliveryMethod)
                {
                    await dbContext.Set<DeliveryMethod>().AddAsync(DeliveryMethods);
                }
                await dbContext.SaveChangesAsync();

            }





        }
    }
}
