using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BookManagementSystem.Model;

namespace BookManagementSystem.Logic
{
    public class BookLogic
    {
        private readonly string _storagePath;
        private readonly JsonSerializerOptions _serializerOptions;

        public BookLogic()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolder = Path.Combine(appData, "BookManagementSystem");
            Directory.CreateDirectory(appFolder);
            _storagePath = Path.Combine(appFolder, "books.json");

            _serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };

            EnsureStorageInitialized();
        }

        private void EnsureStorageInitialized()
        {
            if (File.Exists(_storagePath))
            {
                return;
            }

            var seedBooks = new List<Book>
            {
                new Book(1, "1984", "Джордж Оруэлл", 1949, "Антиутопия"),
                new Book(2, "Преступление и наказание", "Фёдор Достоевский", 1866, "Роман")
            };

            SaveBooks(seedBooks);
        }

        private List<Book> LoadBooks()
        {
            if (!File.Exists(_storagePath))
            {
                return new List<Book>();
            }

            var json = File.ReadAllText(_storagePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<Book>();
            }

            return JsonSerializer.Deserialize<List<Book>>(json, _serializerOptions) ?? new List<Book>();
        }

        private void SaveBooks(List<Book> books)
        {
            var json = JsonSerializer.Serialize(books, _serializerOptions);
            File.WriteAllText(_storagePath, json);
        }

        public List<Book> GetBooks()
        {
            return LoadBooks();
        }

        public void CreateBook(string title, string author, int year, string genre)
        {
            var books = LoadBooks();
            var id = books.Any() ? books.Max(b => b.Id) + 1 : 1;
            var book = new Book(id, title, author, year, genre);
            books.Add(book);
            SaveBooks(books);
        }

        public bool DeleteBook(int id)
        {
            var books = LoadBooks();
            var removed = books.RemoveAll(b => b.Id == id) > 0;
            if (removed)
            {
                SaveBooks(books);
            }

            return removed;
        }

        public Book GetBookById(int id)
        {
            return LoadBooks().FirstOrDefault(b => b.Id == id);
        }

        public bool UpdateBook(int id, string title, string author, int year, string genre)
        {
            var books = LoadBooks();
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return false;
            }

            book.Title = title;
            book.Author = author;
            book.Year = year;
            book.Genre = genre;
            SaveBooks(books);
            return true;
        }

        public Dictionary<string, List<Book>> GroupBooksByGenre()
        {
            return LoadBooks()
                .GroupBy(b => b.Genre)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public List<Book> FindBooksByAuthor(string author)
        {
            return LoadBooks()
                .Where(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Book> FindBooksByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return new List<Book>();
            }

            return LoadBooks()
                .Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
