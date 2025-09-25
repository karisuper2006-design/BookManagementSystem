using System;
using System.Collections.Generic;
using System.Linq;
using BookManagementSystem.Model;

namespace BookManagementSystem.Logic
{
    public class BookLogic
    {
        private List<Book> _books;

        public BookLogic()
        {
            _books = new List<Book>();
            
            _books.Add(new Book(1, "1984", "Джордж Оруэлл", 1949, "Фантастика"));
            _books.Add(new Book(2, "Преступление и наказание", "Федор Достоевский", 1866, "Роман"));
        }

        
        public void CreateBook(string title, string author, int year, string genre)
        {
            var id = _books.Any() ? _books.Max(b => b.Id) + 1 : 1;
            var book = new Book(id, title, author, year, genre);
            _books.Add(book);
        }

        
        public bool DeleteBook(int id)
        {
            return _books.RemoveAll(b => b.Id == id) > 0;
        }

        
        public List<Book> GetBooks()
        {
            return _books.ToList();
        }

        
        public Book GetBookById(int id)
        {
            return _books.FirstOrDefault(b => b.Id == id);
        }

        
        public bool UpdateBook(int id, string title, string author, int year, string genre)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                book.Title = title;
                book.Author = author;
                book.Year = year;
                book.Genre = genre;
                return true;
            }
            return false;
        }

        
        public Dictionary<string, List<Book>> GroupBooksByGenre()
        {
            return _books.GroupBy(b => b.Genre)
                         .ToDictionary(g => g.Key, g => g.ToList());
        }

        
        public List<Book> FindBooksByAuthor(string author)
        {
            return _books.Where(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        
        public List<Book> FindBooksByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return new List<Book>();

            return _books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}