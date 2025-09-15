using System;
using BookManagementSystem.Logic;
using BookManagementSystem.Model;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var logic = new BookLogic();

            while (true)
            {
                Console.WriteLine("\n--- Управление книгами ---");
                Console.WriteLine("1. Показать все книги");
                Console.WriteLine("2. Добавить книгу");
                Console.WriteLine("3. Удалить книгу");
                Console.WriteLine("4. Обновить книгу");
                Console.WriteLine("5. Найти по автору");
                Console.WriteLine("6. Группировка по жанрам");
                Console.WriteLine("7. Выход");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        foreach (var book in logic.GetBooks())
                            Console.WriteLine(book);
                        break;

                    case "2":
                        Console.Write("Название: ");
                        var title = Console.ReadLine();
                        Console.Write("Автор: ");
                        var author = Console.ReadLine();
                        Console.Write("Год: ");
                        var year = int.Parse(Console.ReadLine());
                        Console.Write("Жанр: ");
                        var genre = Console.ReadLine();
                        logic.CreateBook(title, author, year, genre);
                        Console.WriteLine("Книга добавлена!");
                        break;

                    case "3":
                        Console.Write("ID для удаления: ");
                        var id = int.Parse(Console.ReadLine());
                        if (logic.DeleteBook(id))
                            Console.WriteLine("Книга удалена.");
                        else
                            Console.WriteLine("Книга не найдена.");
                        break;

                    case "4":
                        Console.Write("ID для обновления: ");
                        id = int.Parse(Console.ReadLine());
                        Console.Write("Новое название: ");
                        title = Console.ReadLine();
                        Console.Write("Новый автор: ");
                        author = Console.ReadLine();
                        Console.Write("Новый год: ");
                        year = int.Parse(Console.ReadLine());
                        Console.Write("Новый жанр: ");
                        genre = Console.ReadLine();
                        if (logic.UpdateBook(id, title, author, year, genre))
                            Console.WriteLine("Книга обновлена.");
                        else
                            Console.WriteLine("Книга не найдена.");
                        break;

                    case "5":
                        Console.Write("Введите имя автора: ");
                        var searchAuthor = Console.ReadLine();
                        var books = logic.FindBooksByAuthor(searchAuthor);
                        if (books.Any())
                        {
                            foreach (var book in books)
                                Console.WriteLine(book);
                        }
                        else
                            Console.WriteLine("Книги не найдены.");
                        break;

                    case "6":
                        var groups = logic.GroupBooksByGenre();
                        foreach (var group in groups)
                        {
                            Console.WriteLine($"\nЖанр: {group.Key}");
                            foreach (var book in group.Value)
                                Console.WriteLine($"  {book}");
                        }
                        break;

                    case "7":
                        return;

                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }
    }
}