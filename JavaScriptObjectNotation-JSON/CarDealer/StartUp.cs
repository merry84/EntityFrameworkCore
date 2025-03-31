using Microsoft.EntityFrameworkCore;
namespace CarDealer
{
    using Data;
    using Models;
    using DTOs.Import;

    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext db = new();
            string suppliersJson = File.ReadAllText("../../../Datasets/sales.json");
            //string result = ImportSales(db, suppliersJson);
            //Console.WriteLine(result);

            string result = GetSalesWithAppliedDiscount(db);

            Console.WriteLine(result);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}.";
        }


        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            PartDto[]? partDtos = JsonConvert.DeserializeObject<PartDto[]>(inputJson);

            var validSupplierIds = context.Suppliers
                .Select(s => s.Id)
                .ToHashSet();

            var validParts = new List<Part>();

            foreach (var dto in partDtos)
            {
                if (!validSupplierIds.Contains(dto.SupplierId))
                {
                    continue;
                }

                var part = new Part
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    Quantity = dto.Quantity,
                    SupplierId = dto.SupplierId
                };

                validParts.Add(part);
            }

            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            //Import the cars from the provided file "cars.json".
            // Your method should return string with the following message:
            // $"Successfully imported {cars.Count}.";
            CarDto[]? carDtos = JsonConvert.DeserializeObject<CarDto[]>(inputJson);
            ICollection<Car> cars = new List<Car>();
            ICollection<PartCar> partCars = new List<PartCar>();


            if (carDtos != null)
            {
                foreach (var dto in carDtos)
                {
                    if (!IsValid(dto))
                    {
                        continue;
                    }

                    var car = new Car
                    {
                        Make = dto.Make,
                        Model = dto.Model,
                        TraveledDistance = dto.TraveledDistance
                    };
                    cars.Add(car);
                    foreach (var partId in dto.partsId.Distinct())
                    {
                        var partCar = new PartCar
                        {
                            Car = car,
                            PartId = partId
                        };
                        partCars.Add(partCar);
                    }
                }

                context.Cars.AddRange(cars);
                context.PartsCars.AddRange(partCars);
                context.SaveChanges();


            }

            return $"Successfully imported {cars.Count}.";

        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            //Import the customers from the provided file "customers.json".
            // Your method should return a string with the following message:
            // $"Successfully imported {customers.Count}.";
            CustomerDto[]? customerDtos = JsonConvert.DeserializeObject<CustomerDto[]>(inputJson);
            ICollection<Customer> validCostumers = new List<Customer>();
            if (validCostumers != null)
            {
                foreach (CustomerDto customerDto in customerDtos)
                {
                    if (!IsValid(customerDto))
                    {
                        continue;
                    }

                    Customer customer = new Customer()
                    {
                        Name = customerDto.Name,
                        BirthDate = customerDto.BirthDate,
                        IsYoungDriver = customerDto.IsYoungDriver
                    };
                    validCostumers.Add(customer);

                }

                context.Customers.AddRange(validCostumers);
                context.SaveChanges();
            }

            return $"Successfully imported {validCostumers.Count}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            //Import the sales from the provided file "sales.json".
            // Your method should return a string with the following message:
            // $"Successfully imported {sales.Count}.";

            SaleDto[]? saleDtos = JsonConvert.DeserializeObject<SaleDto[]>(inputJson);
            ICollection<Sale> validSales = new List<Sale>();

            if (saleDtos != null)
            {
                foreach (SaleDto saleDto in saleDtos)
                {
                    if (!IsValid(saleDto))
                    {
                        continue;
                    }

                    Sale sale = new Sale()
                    {
                        CarId = saleDto.CarId,
                        CustomerId = saleDto.CustomerId,
                        Discount = saleDto.Discount
                    };
                    validSales.Add(sale);
                }

                context.Sales.AddRange(validSales);
                context.SaveChanges();
            }

            return $"Successfully imported {validSales.Count}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            //Get all customers ordered by their birth date ascending. If two customers are born on the same date first print those who are not young drivers (e.g., print experienced drivers first). Export the list of customers to JSON in the format provided below.
            // Your method should return a string with the following JSON format:
            // [
            // {
            //   "Name": "Louann Holzworth",
            //   "BirthDate": " 01/10/1960",
            //   "IsYoungDriver": false
            // },
            // {
            //   "Name": "Donnetta Soliz",
            //   "BirthDate": "01/10/1963",
            //   "IsYoungDriver": true
            // }
            //
            // ]
            var customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = c.IsYoungDriver
                })
                .ToArray();

            string json = JsonConvert.SerializeObject(customers, Formatting.Indented);
            return json;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            //Get all cars with Toyota make and order them by model alphabetically and by traveled distance descending. Export the list of cars to JSON in the format provided below.

            // Your method should return a string with the following JSON format:
            // [
            //    {
            //        "Id": 134,
            //        "Make": "Toyota",
            //        "Model": "Camry Hybrid",
            //        "TraveledDistance": 486872832,
            //    },
            //    {
            //        "Id": 139,
            //        "Make": "Toyota",
            //        "Model": "Camry Hybrid",
            //        "TraveledDistance": 397831570,
            //    },
            //]

            var cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c => new
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .ToArray();

            string json = JsonConvert.SerializeObject(cars, Formatting.Indented);
            return json;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            //Get all suppliers that do not import parts from abroad. Get their id, name and the number of parts they can offer to supply. Export the list of suppliers to JSON in the format provided below.
            // Your method should return a string with the following JSON format:
            // [{
            //     "Id": 2,
            //     "Name": "Agway Inc.",
            //     "PartsCount": 3
            //   },
            //   {
            //     "Id": 4,
            //     "Name": "Airgas, Inc.",
            //     "PartsCount": 2
            //   }, 
            // ...
            // ]
            var suppliers = context.Suppliers
                .Where(s => !s.IsImporter)
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToArray();

            string json = JsonConvert.SerializeObject(suppliers, Formatting.Indented);
            return json;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            //Get all cars along with their list of parts. For the car get only make, model and traveled distance and for the parts get only name and price (formatted to 2nd digit after the decimal point). Export the list of cars and their parts to JSON in the format provided below.
            // Your method should return a string with the following JSON format:
            // [{
            //   "car": {
            //     "Make": "Opel",
            //     "Model": "Omega",
            //     "TraveledDistance": 176664996
            //   },
            //   "parts": [
            //     {
            //       "Name": "Rear Right Side Inner door handle",
            //       "Price": "79.99"
            //     },
            //     {
            //       "Name": "Door water-shield",
            //       "Price": "123.99"
            //     },
            //     {
            //       "Name": "Front Right Side Door Glass",
            //       "Price": "100.91"
            //     },
            //     {
            //       "Name": "Window motor",
            //       "Price": "123.49"
            //     },
            //     {
            //       "Name": "Rocker arm",
            //       "Price": "98.99"
            //     },
            //     {
            //       "Name": "Turbocharger",
            //       "Price": "0.40"
            //     },
            //     {
            //       "Name": "Water pipe",
            //       "Price": "44.94"
            //     },
            //     {
            //       "Name": "Muffler",
            //       "Price": "106.90"
            //     },
            //     {
            //       "Name": "Heat shield",
            //       "Price": "10.99"
            //     },
            //     {
            //       "Name": "Speed reducer",
            //       "Price": "14.99"
            //     },
            //     {
            //       "Name": "Differential seal",
            //       "Price": "109.99"
            //     }
            //   ]
            // },
            // {
            //   "car": {
            //     "Make": "Opel",
            //     "Model": "Astra",
            //     "TraveledDistance": 516628215
            //   },
            //   "parts": [
            //     {
            //       "Name": "Front Left Side Door Glass",
            //       "Price": "100.92"
            //     },
            //     {
            //       "Name": "Fan belt",
            //       "Price": "10.99"
            //     },
            //     {
            //       "Name": "Tappet",
            //       "Price": "300.29"
            //     }
            //   ]
            // },
            // {
            //   "car": {
            //     "Make": "Opel",
            //     "Model": "Astra",
            //     "TraveledDistance": 156191509
            //   },
            //   "parts": [
            //     {
            //       "Name": "Sunroof Rail",
            //       "Price": "100.25"
            //     },
            //     {
            //       "Name": "Window seal",
            //       "Price": "100.99"
            //     },
            //     {
            //       "Name": "Steering box",
            //       "Price": "103.99"
            //     }
            //   ]

            //    }, 
            // ...
            // ]

            var cars = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TraveledDistance = c.TraveledDistance
                    },
                    parts = c.PartsCars
                        .Select(pc => new
                        {
                            Name = pc.Part.Name,
                            Price = pc.Part.Price.ToString("F2")
                        })
                        .ToArray()
                })
                .ToArray();

            string json = JsonConvert.SerializeObject(cars, Formatting.Indented);
            return json;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            //Get all customers that have bought at least 1 car and get their names, bought cars count and total spent money on cars. Order the result list by total spent money descending, then by total bought cars again in descending order. Export the list of customers to JSON in the format provided below.
            // Your method should return a string with the following JSON format:
            // [{
            //     "fullName": " Faustina Burgher",
            //     "boughtCars": 4,
            //     "spentMoney": 12585.89
            //   },
            //   {
            //     "fullName": " Garret Capron",
            //     "boughtCars": 3,
            //     "spentMoney": 11743.59
            //  },
            //   {
            //     "fullName": " Carri Knapik",
            //     "boughtCars": 4,
            //     "spentMoney": 11550.63
            //   }, 
            // ...
            // ]

            var customers = context.Customers
                .Where(c => c.Sales.Any())
                .Select(cr => new
                {
                    fullName = cr.Name,
                    boughtCars = cr.Sales.Count,
                    spentMoney = cr.Sales
                        .SelectMany(s => s.Car.PartsCars)
                        .Sum(pc => pc.Part.Price)
                })
                .OrderByDescending(c => c.spentMoney)
                .ThenByDescending(c => c.boughtCars)
                .Take(10)
                .ToArray();

            string json = JsonConvert.SerializeObject(customers, Formatting.Indented);
            return json;


        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            //Get first 10 sales with information about the car, customer and price of the sale with and without discount. Export the list of sales to JSON in the format provided below.


            // Your method should return a string with the following JSON format:   
            // [{
            //     "car": {
            //       "Make": "Toyota",
            //       "Model": "Tacoma",
            //       "TraveledDistance": 431663130
            //     },
            //     "customerName": "Ann Mcenaney",
            //     "discount": "30.00",
            //     "price": "195.97",
            //     "priceWithDiscount": "137.18"
            //   },
            //   {
            //     "car": {
            //       "Make": "Ferrari",
            //       "Model": "275",
            //       "TraveledDistance": 448008546
            //     },
            //     "customerName": "Faustina Burgher",
            //     "discount": "50.00",
            //     "price": "2547.57",
            //     "priceWithDiscount": "1273.79"
            //   }, 
            // ...
            // ]

            var sales = context.Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        s.Car.Make,
                        s.Car.Model,
                        s.Car.TraveledDistance
                    },
                    customerName = s.Customer.Name,
                    discount = s.Discount.ToString("F2"),
                    price = s.Car.PartsCars.Sum(p => p.Part.Price).ToString("F2"),
                    priceWithDiscount =
                        (s.Car.PartsCars.Sum(pc => pc.Part.Price) * (1 - s.Discount / 100)).ToString("F2"),

                })
                .ToArray();

            string json = JsonConvert.SerializeObject(sales, Formatting.Indented);

            return json;

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