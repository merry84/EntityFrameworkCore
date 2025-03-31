using NetPay.Data;
using NetPay.DataProcessor.ImportDtos;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using NetPay.Data.Models;
using NetPay.Data.Models.Enums;
using Newtonsoft.Json;
using ProductShop.Models.Utilities;

namespace NetPay.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedHousehold = "Successfully imported household. Contact person: {0}";
        private const string SuccessfullyImportedExpense = "Successfully imported expense. {0}, Amount: {1}";

        public static string ImportHouseholds(NetPayContext context, string xmlString)
        {
            StringBuilder output = new();

            var rootDto = HelpClass.Deserialize<ImportHouseholdsDto[]>(xmlString, "Households");

            if (rootDto != null && rootDto.Length > 0)
            {

                ICollection<Household> validHouseholds = new List<Household>();

                foreach (var dto in rootDto)
                {
                    if (!IsValid(dto))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isDuplicateInDb = context.Households.Any(h =>
                        h.ContactPerson == dto.ContactPerson ||
                        h.Email == dto.Email ||
                        h.PhoneNumber == dto.PhoneNumber);

                    bool isDuplicateInBatch = validHouseholds.Any(h =>
                        h.ContactPerson == dto.ContactPerson ||
                        h.Email == dto.Email ||
                        h.PhoneNumber == dto.PhoneNumber);

                    if (isDuplicateInDb || isDuplicateInBatch)
                    {
                        output.AppendLine(DuplicationDataMessage);
                        continue;
                    }

                    Household household = new Household
                    {
                        ContactPerson = dto.ContactPerson,
                        Email = dto.Email,
                        PhoneNumber = dto.PhoneNumber
                    };

                    validHouseholds.Add(household);
                    output.AppendLine(string.Format(SuccessfullyImportedHousehold, dto.ContactPerson));
                }

                context.Households.AddRange(validHouseholds);
                context.SaveChanges();
            }

            return output.ToString().TrimEnd();
        }


        public static string ImportExpenses(NetPayContext context, string jsonString)
        {
           //Using the file "expenses.json", import the data from that file into the database. Print information about each imported object in the format described below.
           // Constraints
           // If any of the required properties is missing, do not import any part of the entity and append.
           // an error message to the method output.
           // If any foreign key leads to an inexisting record valid record, do not import any part
           // of the entity and append an error message to the method output.
           // If any validation error occurs for the expense entity (invalid name, amount, date OR payment status),
           // do not import any part of the entity and append an error message to the method output.
           // oThe DateTime data in the document will be in the following format: "yyyy-MM-dd" 
           // oMake sure you use CultureInfo.InvariantCulture
           // All records in "expenses.json" are guaranteed to be unique
           // To receive the correct Success message, remember to format the Amount value to the 
           // second decimal place.

           var sb = new StringBuilder();
            ImportExpenseDto[]? expenseDtos = JsonConvert.DeserializeObject<ImportExpenseDto[]>(jsonString);
            if (expenseDtos != null && expenseDtos.Length > 0)
            {
                ICollection<Expense> validExpenses = new List<Expense>();

                foreach (var expenseDto in expenseDtos)
                {
                    bool isValidDateDto= DateTime
                        .TryParseExact(expenseDto.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out DateTime dateDto);
                    if(!isValidDateDto || (!IsValid(expenseDto)))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isHouseholdExist = context.Households.Any(h => h.Id == expenseDto.HouseholdId);
                    bool isServiceExist = context.Services.Any(s => s.Id == expenseDto.ServiceId);

                    if (!isHouseholdExist || !isServiceExist)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Expense expense = new Expense
                    {
                        Amount =expenseDto.Amount,
                        DueDate = dateDto,
                        ExpenseName = expenseDto.ExpenseName,
                        PaymentStatus = Enum.Parse<PaymentStatus>(expenseDto.PaymentStatus),
                        HouseholdId = expenseDto.HouseholdId,
                        ServiceId = expenseDto.ServiceId
                    };

                    validExpenses.Add(expense);
                    sb.AppendLine(string.Format(SuccessfullyImportedExpense, expense.ExpenseName, expense.Amount.ToString("F2",CultureInfo.InvariantCulture)));
                }

                context.Expenses.AddRange(validExpenses);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            foreach (var result in validationResults)
            {
                string currvValidationMessage = result.ErrorMessage!;
            }

            return isValid;
        }
    }
}
