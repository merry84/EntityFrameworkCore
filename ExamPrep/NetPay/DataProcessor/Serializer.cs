using NetPay.Data;
using NetPay.Data.Models.Enums;
using NetPay.DataProcessor.ExportDtos;
using ProductShop.Models.Utilities;

namespace NetPay.DataProcessor
{
    public class Serializer
    {
        public static string ExportHouseholdsWhichHaveExpensesToPay(NetPayContext context)
        {
            //Export all households which have at least one expense with a payment status different from "Paid".
            //The households should be exported with all their expenses that are NOT "Paid". 
            // The exported data should be in XML format. 
            // Order the households alphabetically by their contact person. Within each household,
            // order the expenses by payment date in ascending order and by amount in ascending order if dates are the same.
            // Data Fields
            // Household:
            // oContactPerson: Export the contact person of the household
            // oEmail: Export the email of the household
            // oPhoneNumber: Export the phone number of the household
            // oA collection of Expenses
            // Expense:
            // oExpenseName: Export the name of the expense
            // oAmount: Export the amount of the expense, formatted to the second decimal place
            // oPaymentDate: Export the due date of the expense
            // oServiceName: Export the name of the service
            // Expected XML Output:
            // The root element should be <Households>
            // Each household should be represented by a <Household> element
            // Each expense should be represented by an <Expense> element within its associated household

            string result = "";


            var houseHolds = context.Households
                .Where(h => h.Expenses.Any(e => e.PaymentStatus != PaymentStatus.Paid))
                .Select(h => new
                {
                    h.ContactPerson,
                    h.Email,
                    h.PhoneNumber,
                    Expenses = h.Expenses
                        .Where(e => e.PaymentStatus != PaymentStatus.Paid)
                        .Select(e => new
                        {
                            e.ExpenseName,
                            e.Amount,
                            e.DueDate,
                            e.Service.ServiceName
                        })
                        .OrderBy(e => e.DueDate)
                        .ToArray()
                })
                .OrderBy(h => h.ContactPerson)
                .ToArray();
            var householdsResult = houseHolds
                .Select(h => new ExportHouseHoldDto
                {
                    ContactPerson = h.ContactPerson,
                    Email = h.Email!,
                    PhoneNumber = h.PhoneNumber,
                    Expenses = h.Expenses
                        .Select(e => new ExportExpenseDto
                        {
                            ExpenseName = e.ExpenseName,
                            Amount = e.Amount.ToString("F2"),
                            PaymentDate = e.DueDate.ToString("yyyy-MM-dd"),
                            ServiceName = e.ServiceName
                        })
                        .OrderBy(e => e.PaymentDate)
                        .ThenBy(e => e.Amount)
                        .ToArray()
                })
                .OrderBy(h => h.ContactPerson)
                .ToArray();
            result = HelpClass.Serialize(householdsResult, "Households");
            return result;
        }

        //public static string ExportAllServicesWithSuppliers(NetPayContext context)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
