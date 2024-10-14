
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using ApiFunctions;
using Book;
using Loaders;
namespace EpubDownloader
{
    class Program
    {
        private static List<Book.BookModel> books = new List<Book.BookModel>();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to the Epub Downloader!");

            while (true)
            {
                Console.WriteLine("Please choose a command:");
                Console.WriteLine("1. Search for books");
                Console.WriteLine("0. Exit");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "1":
                        await HandleSearchCommand();
                        break;
                    case "0":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid command. Please try again.");
                        break;
                }
            }
        }
public static string SanitizeFileName(string fileName)
{
    // Define invalid characters in file names
    char[] invalidChars = Path.GetInvalidFileNameChars();
    
    // Replace invalid characters with an underscore
    foreach (char c in invalidChars)
    {
        fileName = fileName.Replace(c, '_');
    }

    return fileName;
}
private static async Task HandleSearchCommand()
{
    Console.WriteLine("Let's search for some books!");

    // Prompt the user for the search term
    Console.WriteLine("Enter a search term (e.g., title, author, etc.):");
    string searchTerm = Console.ReadLine();

    // Ask the user for optional author
    Console.WriteLine("Enter author name (optional):");
    string author = Console.ReadLine();

    // Ask the user for optional file type filter
    Console.WriteLine("Enter file type to filter (optional, e.g., pdf, epub, mobi, azw3):");
    string fileType = Console.ReadLine();

    // List supported languages and let the user pick one
    Console.WriteLine("Choose a language (optional, default is 'en'):");
    Console.WriteLine("[en] English, [fr] French, [it] Italian, [de] German, [es] Spanish");
    string lang = Console.ReadLine();
    lang = string.IsNullOrEmpty(lang) ? "en" : lang;

    // List categories for the user to choose from
    Console.WriteLine("Choose a category (optional, default is all):");
    Console.WriteLine("[fiction], [nonfiction], [comic], [magazine], [musicalscore], [other], [unknown]");
    string category = Console.ReadLine();

    // Ask the user for sorting preference
    Console.WriteLine("Choose a sorting option (optional, default is 'mostRelevant'):");
    Console.WriteLine("[newest], [largest], [oldest], [smallest], [mostRelevant]");
    string sorting = Console.ReadLine();
    sorting = string.IsNullOrEmpty(sorting) ? "mostRelevant" : sorting;

    // Define the limit for the number of results per page
    Console.WriteLine("Enter the number of results per page (optional, default is 10):");
    string limitInput = Console.ReadLine();
    int limit = string.IsNullOrEmpty(limitInput) ? 10 : int.Parse(limitInput);

    int currentPage = 1;
    books.Clear();

    while (true)
    {
        // Show a loading spinner while waiting for the response
        Loader.Start();

        AnnaArchiveApiClient apiClient = new AnnaArchiveApiClient();
        try
        {
            // Call the API with the user's input
            string searchResult = await apiClient.SearchBookAsync(
                searchTerm: searchTerm,
                author: author,
                limit: limit,
                lang: lang,
                category: category,
                fileType: fileType,
                sorting: sorting,
                skip: (currentPage - 1) * limit
            );

            Loader.Stop();

            // Deserialize the JSON response
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var response = JsonSerializer.Deserialize<BookResponse>(searchResult, options);

            // Display only the titles and years
            Console.WriteLine($"Page {currentPage} - Books:");
            for (int i = 0; i < response.Books.Count; i++)
            {
                books.Add(response.Books[i]);
                Console.WriteLine($"{i + 1}. {response.Books[i].Title} ({response.Books[i].Year})");
            }

            // Ask the user if they want to see more results
            Console.WriteLine("Do you want to see the next page, previous page, see details of a book (-id <number>), download a book (-dl <number>), or exit?");
            Console.WriteLine("[n] Next page");
            Console.WriteLine("[p] Previous page");
            Console.WriteLine("[-id <number>] Show details of a book");
            Console.WriteLine("[-dl <number>] Download a book");
            Console.WriteLine("[e] Exit");

            string action = Console.ReadLine();

            if (action.ToLower() == "n")
            {
                currentPage++;
            }
            else if (action.ToLower() == "p" && currentPage > 1)
            {
                currentPage--;
            }
            else if (action.ToLower().StartsWith("-id"))
            {
                int bookId;
                if (int.TryParse(action.Split(' ')[1], out bookId) && bookId > 0 && bookId <= books.Count)
                {
                    var selectedBook = books[bookId - 1];
                    Console.WriteLine("Book Details:");
                    Console.WriteLine($"Title: {selectedBook.Title}");
                    Console.WriteLine($"Author: {selectedBook.Author}");
                    Console.WriteLine($"Year: {selectedBook.Year}");
                    Console.WriteLine($"Format: {selectedBook.Format}");
                    Console.WriteLine($"Size: {selectedBook.Size}");
                    Console.WriteLine($"MD5: {selectedBook.Md5}");
                    Console.WriteLine($"Genre: {selectedBook.Genre}");
                    Console.WriteLine($"Image URL: {selectedBook.ImgUrl}");
                }
                else
                {
                    Console.WriteLine("Invalid book ID. Try again.");
                }
            }

else if (action.ToLower().StartsWith("-dl"))
{
    int bookId;
    if (int.TryParse(action.Split(' ')[1], out bookId) && bookId > 0 && bookId <= books.Count)
    {
        var selectedBook = books[bookId - 1];

        // Show a loader while retrieving download links
        Loader.Start();
        var downloadLinks = await apiClient.DownloadBookAsync(selectedBook.Md5);
        Loader.Stop();

        // Deserialize the download links as a list of strings
        var downloadLinksList = JsonSerializer.Deserialize<List<string>>(downloadLinks, options);

        // Display the download links with indices
        Console.WriteLine("Download Links:");
        for (int i = 0; i < downloadLinksList.Count; i++)
        {
            Console.WriteLine($"{i + 1}: {downloadLinksList[i]}");
        }

        // Ask the user to select a link to download
        Console.WriteLine("Select the link number to download:");
        int selectedLinkIndex = int.Parse(Console.ReadLine()) - 1; // Convert user input to zero-based index

        if (selectedLinkIndex >= 0 && selectedLinkIndex < downloadLinksList.Count)
        {
            string selectedDownloadLink = downloadLinksList[selectedLinkIndex];

            // Use the book title as the file name and sanitize it
            string fileName = SanitizeFileName(selectedBook.Title.Replace(" ", "_") + ".pdf"); // Adjust file extension based on the format

            // Show the loader during the download process
            Loader.Start();

            // Download the selected file
            await apiClient.DownloadFileAsync(selectedDownloadLink, fileName);

            // Stop the loader and notify the user when the download is finished
            Loader.Stop();
            Console.WriteLine($"Finished downloading '{selectedBook.Title}' to your downloads folder.");
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }
    }
    else
    {
        Console.WriteLine("Invalid book ID. Try again.");
    }
}
           else if (action.ToLower() == "e")
            {
                break;
            }
        }
        catch (Exception ex)
        {
            Loader.Stop();
            Console.WriteLine($"Error occurred: {ex.Message}");
            break;
        }
    }
}

   }
}

