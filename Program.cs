
using System;
using System.Threading.Tasks;
using ApiFunctions;
using Book;

namespace EpubDownloader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting the application...");

            // Create Book object from the Book namespace
            BookModel book = new BookModel("filePath", "Harry Potter", true, "120", DateTime.Now);

            // Display Book state
            book.State();

            // Call the API using the ApiFunctions namespace
            AnnaArchiveApiClient apiClient = new AnnaArchiveApiClient();
            try
            {
                string searchResult = await apiClient.SearchBookAsync("Harry Potter");
                Console.WriteLine("Search Results:");
                Console.WriteLine(searchResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
    }
}

