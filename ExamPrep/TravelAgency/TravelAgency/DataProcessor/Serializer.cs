using Newtonsoft.Json;
using ProductShop.Models.Utilities;
using TravelAgency.Data;
using TravelAgency.Data.Models.Enums;
using TravelAgency.DataProcessor.ExportDtos;

namespace TravelAgency.DataProcessor
{
    public class Serializer
    {
        public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext context)
        {
            
            // 
            string result = string.Empty;

            var validGuide = context.Guides
                .Where(g => g.Language == Language.Spanish)
                .Select(g => new GuideDto()
                {
                    FullName = g.FullName,
                    TourPackages = g.TourPackagesGuides
                        .Select(e =>new ExportTourPackageDto()
                        {
                            Name = e.TourPackage.PackageName,
                            Description = e.TourPackage.Description!,
                            Price = e.TourPackage.Price
                        })
                        .OrderByDescending(e=> e.Price)
                        .ToArray()
                        })
                .OrderByDescending(g => g.TourPackages.Length)
                .ThenBy(g => g.FullName)
                .ToArray();
            result = HelpClass.Serialize(validGuide, "Guides");
            return result;
        }
              
       

        public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext context)
        {
            
            string result = string.Empty;
            var customerValid= context.Customers
                .Where(c=>c.Bookings.Any(b=>b.TourPackage.PackageName== "Horse Riding Tour"))
                .Select(c=> new ExportCustomerDto()
                {
                    FullName= c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Bookings = c.Bookings
                        .Where(b => b.TourPackage.PackageName == "Horse Riding Tour")
                        .OrderBy(b => b.BookingDate)
                        .Select(b => new ExportBookingDto()
                        {
                            TourPackageName = b.TourPackage.PackageName,
                            Date = b.BookingDate.ToString("yyyy-MM-dd")
                        })
                        .ToArray()
                })
                .OrderByDescending(c => c.Bookings.Length)
                .ThenBy(c => c.FullName)
                .ToArray();

            result = JsonConvert.SerializeObject(customerValid, Formatting.Indented);
            return result;


        }
    }
}
