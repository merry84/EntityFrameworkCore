
using Newtonsoft.Json.Serialization;

namespace ProductShop
{
    using Newtonsoft.Json;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    using Data;
    using Models;
    using DTOs;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext dbContext = new();
            dbContext.Database.Migrate();

            Console.WriteLine("ProductShop database created successfully.");
            string categoriesProducts = File.ReadAllText("../../../Datasets/categories-products.json");
            const string outputPath = "../../../Results/output.json";

            string result = GetUsersWithProducts(dbContext);
            File.WriteAllText(outputPath, result, Encoding.Unicode);
            Console.WriteLine(result);

            //dbContext.Products.RemoveRange(dbContext.Products);
            //dbContext.SaveChanges();
            //dbContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Products', RESEED, 0)");

        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {

            UserDto[]? usersDto = JsonConvert.DeserializeObject<UserDto[]>(inputJson);
            string result = string.Empty;

            if (usersDto != null)
            {
                ICollection<User> userToAdd = new List<User>();
                foreach (var userDto in usersDto)
                {
                    if (!IsValid(userDto))
                    {
                        continue;
                    }

                    int? userAge = null;
                    if (userDto.Age != null)
                    {
                        bool isAgeValid = int.TryParse(userDto.Age.ToString(), out int parsedAge);
                        if (!isAgeValid)
                        {
                            continue;
                        }
                        userAge = parsedAge;
                    }
                    var user = new User
                    {
                        FirstName = userDto.FirstName,
                        LastName = userDto.LastName,
                        Age = userAge
                    };
                    userToAdd.Add(user);
                }
                context.Users.AddRange(userToAdd);
                context.SaveChanges();
                return $"Successfully imported {userToAdd.Count}";
            }

            return result;

        }

        // Problem 02
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var productDtos = JsonConvert.DeserializeObject<List<ProductDto>>(inputJson);

            var validProducts = new List<Product>();
            string result = string.Empty;
            foreach (var dto in productDtos)
            {
                if (!IsValid(dto))
                    continue;

                var product = new Product
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    SellerId = dto.SellerId,
                    BuyerId = dto.BuyerId // OK if null
                };

                validProducts.Add(product);
            }

            context.Products.AddRange(validProducts);
            context.SaveChanges();
            result = $"Successfully imported {validProducts.Count}";
            return result;
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            CategoryDto[]? categoryDtos = JsonConvert.DeserializeObject<CategoryDto[]>(inputJson);


            string result = string.Empty;

            if (categoryDtos != null)
            {
                ICollection<Category> validCategories = new List<Category>();
                foreach (CategoryDto category in categoryDtos)
                {
                    if (!IsValid(category))
                    {
                        continue;
                    }

                    Category categoryToAdd = new Category()
                    {
                        Name = category.Name!,
                    };
                    validCategories.Add(categoryToAdd);
                }
                context.Categories.AddRange(validCategories);
                context.SaveChanges();
                result = $"Successfully imported {validCategories.Count}";
            }

            return result;
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            string result = string.Empty;
            CategoriesProductsDto[]? categoryProductDtos
                = JsonConvert.DeserializeObject<CategoriesProductsDto[]>(inputJson);


            if (categoryProductDtos != null)
            {
                ICollection<int> validCategoryIds = context.Categories
                    .Select(c => c.Id).ToList();
                ICollection<int> validProductIds = context.Products
                    .Select(p => p.Id).ToList();

                ICollection<CategoryProduct> categoryProductsToAdd = new List<CategoryProduct>();
                foreach (CategoriesProductsDto categoryProductDto in categoryProductDtos)
                {
                    if (!IsValid(categoryProductDto))
                        continue;

                    int categoryId = categoryProductDto.CategoryId;
                    int productId = categoryProductDto.ProductId;

                    if (!validCategoryIds.Contains(categoryId) || !validProductIds.Contains(productId))
                        continue;

                    //if (categoryProductsToAdd.Any(cp => cp.CategoryId == categoryId && cp.ProductId == productId))
                    //    continue;


                    ////if (context.CategoriesProducts.Any(cp => cp.CategoryId == categoryId && cp.ProductId == productId))
                    //continue;

                    CategoryProduct categoryProduct = new CategoryProduct()
                    {
                        CategoryId = categoryId,
                        ProductId = productId
                    };
                    categoryProductsToAdd.Add(categoryProduct);
                }

                context.CategoriesProducts.AddRange(categoryProductsToAdd);
                context.SaveChanges();
                result = $"Successfully imported {categoryProductsToAdd.Count}";
            }
            return result;

        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            //Get all products in a specified price range:  500 to 1000 (inclusive). Order them by price (from lowest to highest). Select only the product name, price and the full name of the seller. Export the result to JSON.

            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    Name = p.Name,
                    Price = p.Price,
                    Seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .ToList();

            DefaultContractResolver camelCaseResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            string result = JsonConvert
                .SerializeObject(products, Formatting.Indented, new JsonSerializerSettings()
                {
                    ContractResolver = camelCaseResolver
                });


            return result;
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            var usersWithSoldProducts = context
                .Users
                .Where(u => u.ProductsSold
                    .Any(p => p.BuyerId.HasValue))
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    SoldProducts = u.ProductsSold
                        .Where(p => p.BuyerId.HasValue)
                        .Select(p => new
                        {
                            p.Name,
                            p.Price,
                            BuyerFirstName = p.Buyer!.FirstName,
                            BuyerLastName = p.Buyer.LastName
                        })
                        .ToArray()
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToArray();

            DefaultContractResolver camelCaseResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            string jsonResult = JsonConvert
                .SerializeObject(usersWithSoldProducts, Formatting.Indented, new JsonSerializerSettings()
                {
                    ContractResolver = camelCaseResolver
                });

            return jsonResult;
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {

            //Get all categories. Order them in descending order by the category's products count. For each category select its name, the number of products, the average price of those products (rounded to the second digit after the decimal separator) and the total revenue (total price sum and rounded to the second digit after the decimal separator) of those products (regardless if they have a buyer or not).

            var categories = context.Categories
                .Select(c => new
                {
                    Category = c.Name,
                    ProductsCount = c.CategoriesProducts.Count,
                    AveragePrice = c.CategoriesProducts
                        .Average(cp => cp.Product.Price)
                        .ToString("F2"),
                    TotalRevenue = c.CategoriesProducts
                        .Sum(cp => cp.Product.Price)
                        .ToString("F2")
                })
                .OrderByDescending(c => c.ProductsCount)
                .ToList();
            DefaultContractResolver camelCaseResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            string result = JsonConvert
                .SerializeObject(categories, Formatting.Indented, new JsonSerializerSettings()
                {
                    ContractResolver = camelCaseResolver,

                });
            return result;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            //Get all users who have at least 1 sold product with a buyer. Order them in descending order by the number of sold products to a buyer. Select only their first and last name and age and for each product – name and price. Ignore all null values.
            // Export the results to JSON. Follow the format below to better understand how to structure your data. 

            var users = context.Users
                .Where(u => u.ProductsSold
                    .Any(p => p.BuyerId != null && p.Price != null))
                .OrderByDescending(u => u.ProductsSold.Count(p => p.BuyerId != null))
                .Select(u => new
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = u.ProductsSold
                        .Where(p => p.BuyerId != null && p.Price != null)
                        .Select(p => new
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .ToArray()
                })
                .OrderByDescending(u => u.SoldProducts.Length)
                .ToArray();


            var output = new
            {
                UsersCount = users.Length,
                Users = users.Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = new
                    {
                        Count = u.SoldProducts.Length,
                        Products = u.SoldProducts
                    }
                })
            };
            DefaultContractResolver camelCaseResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            string result = JsonConvert
                .SerializeObject(output, Formatting.Indented, new JsonSerializerSettings()
                {
                    ContractResolver = camelCaseResolver,
                    NullValueHandling = NullValueHandling.Ignore

                });
            return result;
        }




        private static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator
                .TryValidateObject(dto, validateContext, validationResults, true);

            return isValid;
        }


    }
}