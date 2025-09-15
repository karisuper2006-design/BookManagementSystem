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
            // Инициализация тестовыми данными
            _books.Add(new Book(1, "1984", "Джордж Оруэлл", 1949, "Фантастика"));
            _books.Add(new Book(2, "Преступление и наказание", "Федор Достоевский", 1866, "Роман"));
        }

        // 1. Создание книги
        public void CreateBook(string title, string author, int year, string genre)
        {
            var id = _books.Any() ? _books.Max(b => b.Id) + 1 : 1;
            var book = new Book(id, title, author, year, genre);
            _books.Add(book);
        }

        // 2. Удаление книги по ID
        public bool DeleteBook(int id)
        {
            return _books.RemoveAll(b => b.Id == id) > 0;
        }

        // 3. Чтение всех книг
        public List<Book> GetBooks()
        {
            return _books.ToList();
        }

        // 3. Чтение книги по ID
        public Book GetBookById(int id)
        {
            return _books.FirstOrDefault(b => b.Id == id);
        }

        // 4. Изменение книги
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

        // 5. Бизнес-функция 1: Группировка по жанрам
        public Dictionary<string, List<Book>> GroupBooksByGenre()
        {
            return _books.GroupBy(b => b.Genre)
                         .ToDictionary(g => g.Key, g => g.ToList());
        }

        // 5. Бизнес-функция 2: Поиск книг по автору
        public List<Book> FindBooksByAuthor(string author)
        {
            return _books.Where(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}