# EpubDownloader

EpubDownloader is a simple command-line tool built with C# for searching and downloading eBooks from the Anna's Archive API. Users can search for books by title, author, or file type, and download the desired eBook in their preferred format.

## Features

- **Search for Books**: Search for eBooks by title, author, and other filters.
- **Download eBooks**: Select and download eBooks in different formats such as PDF, EPUB, MOBI, etc.
- **Customizable Options**: User-provided API key and file format for search.
- **Cross-Platform**: Can be built and used on multiple platforms.

## Prerequisites

Before you begin, ensure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download) 6.0 or later (for building from source).
- [Anna's Archive API key](https://rapidapi.com/annas-archive-api.p.rapidapi.com/) (required for making API requests).

## Installation

### Clone the Repository

```bash
git clone https://github.com/PoorDoomer/EpubDownloader.git
cd EpubDownloader
```
### Building the Project

If you'd like to build the project from source, run the following commands:

```bash
dotnet build
```
### Running the Application

You can run the application with:

```bash

dotnet run
```

## Using the Released .exe (Windows)

For Windows users, you can download the pre-built executable from the releases section.

Download the .zip file from the latest release.
Unzip it and then click on the executable or run it via command line:

```bash
EpubDownloader.exe
```

## Configuration

You can customize the download folder path in the EnvironmentVariables.cs file. By default, books are downloaded to the DownloadedBooks folder inside the project directory.

```csharp

public static string downloadPath = Path.Combine(AppContext.BaseDirectory, "DownloadedBooks");
```
The API key is entered at runtime by the user and used for authentication in API requests.


