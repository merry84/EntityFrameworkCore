using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using ProductShop.Models.Utilities;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ImportDtos;

namespace TravelAgency.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
        private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

        public static string ImportCustomers(TravelAgencyContext context, string xmlString)
        {
            //Using the file "customers.xml", import the data from the file into the database. 
            // Each imported customer should be validated and added to the database if it meets the specified criteria.
            // The method should return a string containing information about each import attempt, formatted as described.
            // Constraints
            // Validation of Customer Entities - Each customer entity must be validated against the following criteria:
            // oFullName – Must meet the constraints for the property, described above
            // oEmail – Must meet the constraints for the property, described above
            // oPhoneNumber - Must meet the constraints for the property, described above
            // Duplication Check - Before adding a customer to the database, 
            // ensure there are no existing records with the same:
            // oFullName OR Email OR PhoneNumber
            // If any validation error occurs for a customer entity or any of the fields match an existing record, the customer entity should not be imported, and the appropriate error message or duplication message should be appended to the method's output
            // Success Messages
            // oFor each successfully imported customer, append a success message to the output, formatted as Successfully imported customer - {FullName}
            // Data Persistence
            // oAfter processing all customers from the XML file, 
            // add the valid customer entities to the proper collection
            // oSave the changes to the database
            // Success message
            // Successfully imported customer - {customerFullName}
            // Example
            // customers.xml
            // <?xml version='1.0' encoding='UTF-8'?>
            // <Customers>
            // 	<Customer phoneNumber="+357683444233">
            // 		<FullName>Robert Simons</FullName>
            // 		<Email>robert.simons@mail.dm</Email>
            // 	</Customer>
            // <Customer phoneNumber="+357600444236">
            // 		<FullName>Emma Brown</FullName>
            // 		<Email>emma.brown@mail.dm</Email>
            // 	</Customer> 
            // <Customers>


            StringBuilder sb = new StringBuilder();
            ImportCustomerDto[]? customerDtos = HelpClass.Deserialize<ImportCustomerDto[]>(xmlString, "Customers");

            if (customerDtos != null)
            {
                ICollection<Customer> dbCustomers = new List<Customer>();
                foreach (ImportCustomerDto customerDto in customerDtos)
                {
                    if (!IsValid(customerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (ExistInDb(customerDto, dbCustomers))
                    {
                        sb.AppendLine(DuplicationDataMessage);
                        continue;
                    }

                    Customer customer = new Customer
                    {
                        FullName = customerDto.FullName,
                        Email = customerDto.Email,
                        PhoneNumber = customerDto.PhoneNumber
                    };

                    dbCustomers.Add(customer);
                    sb.AppendLine(string.Format(SuccessfullyImportedCustomer, customer.FullName));
                }

                context.Customers.AddRange(dbCustomers);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportBookings(TravelAgencyContext context, string jsonString)
        {
            //Using the file "bookings.json", import the data from that file into the database. Print information about each imported object in the format described below.
            // Constraints
            // If any validation error occurs for the booking entity (invalid date),
            // do not import any part of the entity and append an error message to the method output.
            // oThe DateTime data in the document will be in the following format: "yyyy-MM-dd" 
            // oMake sure you use CultureInfo.InvariantCulture
            // The Customers and TourPackages associated with every single Booking will be string values,
            // which could be matched to already existing records in the database. 
            // Success message
            // Successfully imported booking – TourPackage: {tourPackageName}, Date: {date.ToString("yyyy-MM-dd")}
            // Example
            // bookings.json
            // [
            //   {
            //     "BookingDate": "2024-09-21",
            //     "CustomerName": "Donald Sanders",
            //     "TourPackageName": "Horse Riding Tour"
            //   },
            //   {
            //     "BookingDate": "2024-09-22",
            //     "CustomerName": "Donald Sanders",
            //     "TourPackageName": "Sightseeing Tour"
            //   },
        

            StringBuilder sb = new StringBuilder();

            ImportBookingDto[]? bookingDtos =JsonConvert.DeserializeObject<ImportBookingDto[]>(jsonString);
            if (bookingDtos != null)
            {
                ICollection<Booking> dbBookings = new List<Booking>();

                foreach (var booking in bookingDtos)
                {
                    bool isValidDate = DateTime
                        .TryParseExact(booking.BookingDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);

                    if (!IsValid(booking) || !isValidDate)
                    {
                        sb.AppendLine(ErrorMessage); continue;
                    }

                    var customerExist = context.Customers.FirstOrDefault(c => c.FullName == booking.CustomerName);
                    var tourPackageExist = context.TourPackages.FirstOrDefault(tp => tp.PackageName == booking.TourPackageName);
                    if (customerExist != null || tourPackageExist != null)
                    {
                        Booking dbBooking = new Booking
                        {
                            BookingDate = date,
                            Customer = customerExist!,
                            TourPackage = tourPackageExist!

                        };
                        dbBookings.Add(dbBooking);
                        sb.AppendLine(string.Format(SuccessfullyImportedBooking, dbBooking.TourPackage.PackageName, dbBooking.BookingDate.ToString("yyyy-MM-dd")));
                    }
                    else
                    {
                        sb.AppendLine(ErrorMessage);
                    }

                }

                context.Bookings.AddRange(dbBookings);
                context.SaveChanges();
            }
            return sb.ToString().TrimEnd();

        }

        private static bool ExistInDb(ImportCustomerDto customerDto, ICollection<Customer> dbCustomers)
        {
            bool result = dbCustomers.Any(c => c.FullName == customerDto.FullName) ||
                          dbCustomers.Any(c => c.Email == customerDto.Email) ||
                          dbCustomers.Any(c => c.PhoneNumber == customerDto.PhoneNumber);

            return result;
        }
        public static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string currValidationMessage = validationResult.ErrorMessage;
            }

            return isValid;
        }
    }
}
