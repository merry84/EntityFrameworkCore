using Microsoft.EntityFrameworkCore;
using ProductShop.Data;

using ProductShop.Models;
using ProductShop.Models.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ProductShop.DTOs.Import;
using System.Text;
using ProductShop.DTOs.Export;
using System.Xml.Serialization;

namespace ProductShop
{

    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext context = new ProductShopContext();
           // context.Database.Migrate();
            Console.WriteLine("Database migrated to the latest version successfully!");

            const string xmlFilePath = "../../../Datasets/categories-products.xml";
            string inputXml = File.ReadAllText(xmlFilePath);

            //const string outputFilePath = "../../../Results/userSoldProducts.xml";
            //string result = GetSoldProducts(context);

            //File.WriteAllText(outputFilePath, result, Encoding.Unicode);
            //Console.WriteLine(result);
            string result = ImportCategoryProducts(context, inputXml);
            Console.WriteLine(result);


        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
           
            ProductDto[] productDtos = HelpClass.Deserialize<ProductDto[]>(inputXml, "Products");
            var validProducts = new List<Product>();

            // Взимаме валидните UserId-та, за да сравняваме
            var userIds = context.Users
                .Select(u => u.Id).ToHashSet();  

            foreach (var dto in productDtos)
            {
                bool isValidPrice = decimal.TryParse(dto.Price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedPrice);

                if (!isValidPrice)
                    continue;

                if (!userIds.Contains(dto.SellerId))
                    continue;

                // Ако BuyerId е 0 или не съществува, го задаваме като null
                int? buyerId = userIds.Contains(dto.BuyerId) ? dto.BuyerId : null;

                var product = new Product
                {
                    Name = dto.Name,
                    Price = parsedPrice,
                    SellerId = dto.SellerId,
                    BuyerId = buyerId
                };

                validProducts.Add(product);
            }
           
            context.Products.AddRange(validProducts);
            context.SaveChanges();

            return $"Successfully imported {validProducts.Count}";
        }


        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            //Import the categories from the provided file "categories.xml". 
            // Some of the names will be null, so you don't have to add them to the database. Just skip the record and continue.
            // Your method should return a string with the following message:
            // $"Successfully imported {categories.Count}";

            string result = string.Empty;


            CategoriesDto[]? categoryDtos = HelpClass.Deserialize<CategoriesDto[]>(inputXml, "Categories");
            if (categoryDtos != null)
            {
                ICollection<Category> validCategories = new List<Category>();
                foreach (CategoriesDto categoryDto in categoryDtos)
                {
                    if (!IsValid(categoryDto))
                    {
                        continue;
                    }

                    if (!IsValid(categoryDto.Name))
                    {
                        continue;
                    }
                    Category category = new Category()
                    {
                        Name = categoryDto.Name
                    };
                    validCategories.Add(category);
                }
                context.Categories.AddRange(validCategories);
                context.SaveChanges();
                result = $"Successfully imported {validCategories.Count}";
            }
            return result;
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            string result = string.Empty;

            CategoryProductsDto[]? catProdDtos
                = HelpClass.Deserialize<CategoryProductsDto[]>(inputXml, "CategoryProducts");

            if (catProdDtos != null)
            {
                ICollection<CategoryProduct> validCatProd = new List<CategoryProduct>();

                foreach (CategoryProductsDto catProdDto in catProdDtos)
                {
                    bool isProductIdValid = int.TryParse(catProdDto.ProductId, out int productId);
                    bool isCategoryIdValid = int.TryParse(catProdDto.CategoryId, out int categoryId);
                   // Console.WriteLine($"Processing: ProductId = '{catProdDto.ProductId}', CategoryId = '{catProdDto.CategoryId}'");

                    if (!IsValid(catProdDto))
                    {
                        Console.WriteLine("invalid dto");

                        continue;
                    }
                    if (!isProductIdValid || !isCategoryIdValid)
                    {
                        Console.WriteLine("invalid");
                        continue;
                    }

                    // Проверяваме дали продуктът и категорията съществуват в базата
                    bool productExists = context.Products.Any(p => p.Id == productId);
                    bool categoryExists = context.Categories.Any(c => c.Id == categoryId);

                    if (!productExists || !categoryExists)
                    {
                        Console.WriteLine("exist product");
                        continue;
                    }

                    // Ако и двете проверки минат, добавяме обекта
                    CategoryProduct categoryProduct = new CategoryProduct
                    {
                        CategoryId = categoryId,
                        ProductId = productId
                    };

                    validCatProd.Add(categoryProduct);
                }

                context.CategoryProducts.AddRange(validCatProd);
                context.SaveChanges();

                result = $"Successfully imported {validCatProd.Count}";
            }

            return result;
        }


        public static string GetProductsInRange(ProductShopContext context)
        {
            //Get all products in a specified price range between 500 and 1000 (inclusive). Order them by price (from lowest to highest).
            //Select only the product name, price and the full name of the buyer. Take top 10 records.
            // Return the list of suppliers to XML in the format provided below.
            //<?xml version="1.0" encoding="utf-16"?>
            // <Products>
            //   <Product>
            //     <name>TRAMADOL HYDROCHLORIDE</name>
            //     <price>516.48</price>
            //   </Product>
            //   <Product>
            //     <name>Allopurinol</name>
            //     <price>518.5</price>
            //     <buyer>Wallas Duffyn</buyer>
            //   </Product>
            //   <Product>
            //     <name>Parsley</name>
            //     <price>519.06</price>
            //     <buyer>Brendin Predohl</buyer>
            // …
            //   </Product>
            // </Products>


            ProductInRangeDto[] products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new ProductInRangeDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = p.Buyer.FirstName + " " + p.Buyer.LastName
                })
                .OrderBy(p => p.Price)
                .Take(10)
                .ToArray();

            string xmlResult = HelpClass.Serialize(products, "Products");
            return xmlResult;

        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.BuyerId != null)) // взимаме само продадени
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .Select(u => new UserDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold
                        .Where(p => p.BuyerId != null)
                        .Select(p => new ProductDtoExport
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .ToArray()
                })
                .ToArray();

            return HelpClass.Serialize(users, "Users");
        }

        private static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validateResults = new List<ValidationResult>();

            bool isValid = Validator
                .TryValidateObject(dto, validateContext, validateResults, true);

            return isValid;
        }
    }
}