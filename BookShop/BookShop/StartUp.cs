using System.Globalization;
using System.Text;
using BookShop.Models.Enums;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BookShop
{
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);
            //string input = Console.ReadLine();
            var result = RemoveBooks(db);
            //var result = CountBooks(db, 12);
            Console.WriteLine(result);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            //Return in a single string all book titles, each on a new line, that have an age restriction, equal to the given command. Order the titles alphabetically.
            // Read input from the console in your main method and call your method with the necessary arguments. Print the returned string to the console. Ignore the casing of the input.
            string result = string.Empty;

            if (!Enum.TryParse(command, true, out AgeRestriction ageRestriction))
            {
                return result;
            }
            var books = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToList();
            return string.Join(Environment.NewLine, books);
        }
        public static string GetGoldenBooks(BookShopContext context)
        {

            //Return in a single string the titles of the golden edition books that have less than 5000 copies,
            //each on a new line. Order them by BookId ascending.
            var goldenBooks = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();
            return string.Join(Environment.NewLine, goldenBooks);

        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            //Return in a single string all titles and prices of books with a price higher than 40,
            //each on a new row in the format given below. Order them by price descending.
            var books = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => $"{b.Title} - ${b.Price:F2}")
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            //Return in a single string with all titles of books that are NOT released in a given year.
            //Order them by bookId ascending.

            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();
            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            //Return in a single string the titles of books by a given list of categories.
            //The list of categories will be given in a single line separated by one or more spaces.
            //Ignore casing. Order by title alphabetically.

            var categories= input
                .ToLower()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var books = context.Books
                .Where(b=>b.BookCategories
                    .Any(bc=>categories.Contains(bc.Category.Name.ToLower())))
                .Select(b=>b.Title)
                .OrderBy(b => b)
                .ToList();
            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            //Return the title, edition type and price of all books that are released before a given date.
            //The date will be a string in the format "dd-MM-yyyy".
            // Return all of the rows in a single string, ordered by release date (descending).

            //DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture) конвертира входния низ в DateTime.
            var releasedBooks = context.Books
                .Where(b => b.ReleaseDate < DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}")
                .ToList();
            return string.Join(Environment.NewLine, releasedBooks);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            //Return the full names of authors, whose first name ends with a given string.
            // Return all names in a single string, each on a new row, ordered alphabetically.
            var  authorNameEndsIn= context.Authors
                .Where(a=>a.FirstName.EndsWith(input))
                .Select(a=>a.FirstName + " " + a.LastName)
                .OrderBy(a => a)
                .ToList();
            return string.Join(Environment.NewLine, authorNameEndsIn);

        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            //Return the titles of the book, which contain a given string. Ignore casing.
            // Return all titles in a single string, each on a new row, ordered alphabetically.
            var bookTitles= context.Books
                .Where(b=>b.Title.ToLower().Contains(input.ToLower()))
                .Select(b=>b.Title)
                .OrderBy(b => b)
                .ToList();
            return string.Join(Environment.NewLine, bookTitles);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            //Return all titles of books and their authors' names for books, which are written by authors
            //whose last names start with the given string.
            // Return a single string with each title on a new row. Ignore casing. Order by BookId ascending.

            var booksByAuthor= context.Books
                .Where(b=>b.Author.LastName.ToLower()
                    .StartsWith(input.ToLower()))
                .OrderBy(b=>b.BookId)
                .Select(b=>$"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                .ToList();
            return string.Join(Environment.NewLine, booksByAuthor);

        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            //Return the number of books, which have a title longer than the number given as an input.
            return context.Books
                .Count(b => b.Title.Length > lengthCheck);
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
           //Return the total number of book copies for each author.
           //Order the results descending by total book copies.
             // Return all results in a single string, each on a new line.

             var countCopiesByAuthor = context.Authors
                 .Select(a => new
                 {
                     FullName = a.FirstName + " " + a.LastName,
                     Copies = a.Books.Sum(b => b.Copies)
                 })
                 .OrderByDescending(a => a.Copies)
                 .Select(a => $"{a.FullName} - {a.Copies}")
                 .ToList();

             return string.Join(Environment.NewLine, countCopiesByAuthor);
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            //Return the total profit of all books by category.
            //Profit for a book can be calculated by multiplying its number of copies by the price per single book.
            //Order the results by descending by total profit for a category and ascending by category name.
            //Print the total profit formatted to the second digit.


            var totalProfit= context.Categories
                .OrderByDescending(c=>c.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price))
                .ThenBy(c => c.Name)
                .Select(c=> $"{c.Name} ${c.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price):f2}")
                .ToList();
            return string.Join(Environment.NewLine, totalProfit);
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            //Get the most recent books by categories. The categories should be ordered by name alphabetically.
            //Only take the top 3 most recent books from each category – ordered by release date (descending).
            //Select and print the category name and for each book – its title and release year

            var mostRecentBooks = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    BookTitle = c.CategoryBooks
                        .Select(cb => cb)
                        .OrderByDescending(b => b.Book.ReleaseDate)
                        .Take(3)
                        .Select(b => new
                        {
                            b.Book.Title,
                            Year = b.Book.ReleaseDate.Value.Year
                        })
                        .ToList()

                })
                .ToList();

            var sb = new StringBuilder();
            foreach (var categoryBook in mostRecentBooks)
            {
                //--Action
                sb.AppendLine($"--{categoryBook.CategoryName}");
                foreach (var book in categoryBook.BookTitle)
                {
                     sb.AppendLine($"{book.Title} ({book.Year})");
                    
                }
                // Brandy ofthe Damned (2015)

            }
            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var increasePricesBooks = context.Books
                .Where(b => b.ReleaseDate.HasValue
                            && b.ReleaseDate.Value.Year < 2010)
                .ToArray();
            foreach (var book in increasePricesBooks)
            {
                book.Price += 5;
            }

            context.SaveChanges();

        }

        public static int RemoveBooks(BookShopContext context)
        {
            //Remove all books, which have less than 4200 copies. Return an int - the number of books that were deleted from the database.

            var removedBooks = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();
            int countToRemove = removedBooks.Count;

            context.RemoveRange(removedBooks);
            context.SaveChanges();
            return countToRemove;
        }
    }
}


