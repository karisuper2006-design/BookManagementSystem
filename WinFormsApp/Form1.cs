using BookManagementSystem.Logic;
using BookManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        private BookLogic _logic;

        public Form1()
        {
            InitializeComponent();
            _logic = new BookLogic();
            // Заполним комбобокс жанрами
            comboBoxGenre.Items.AddRange(new string[] {
                "Фантастика",
                "Роман",
                "Детектив",
                "Научная фантастика",
                "Приключения",
                "Исторический роман",
                "Биография",
                "Поэзия"
            });
            LoadBooks();
        }

        private void LoadBooks()
        {
            listBoxBooks.Items.Clear();
            foreach (var book in _logic.GetBooks())
            {
                listBoxBooks.Items.Add(book.ToString());
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var title = textBoxTitle.Text;
            var author = textBoxAuthor.Text;
            int year;
            var genre = comboBoxGenre.SelectedItem?.ToString();

            if (!int.TryParse(textBoxYear.Text, out year))
            {
                MessageBox.Show("Год должен быть числом.");
                return;
            }

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author) || string.IsNullOrEmpty(genre))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            _logic.CreateBook(title, author, year, genre);
            LoadBooks();
            ClearFields();
            MessageBox.Show("Книга добавлена!");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBoxBooks.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите книгу для удаления.");
                return;
            }

            var selected = listBoxBooks.SelectedItem.ToString();
            var id = int.Parse(selected.Split(',')[0].Split(':')[1].Trim());

            if (_logic.DeleteBook(id))
            {
                LoadBooks();
                ClearFields();
                MessageBox.Show("Книга удалена.");
            }
            else
            {
                MessageBox.Show("Ошибка удаления.");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (listBoxBooks.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите книгу для редактирования.");
                return;
            }

            var selected = listBoxBooks.SelectedItem.ToString();
            var id = int.Parse(selected.Split(',')[0].Split(':')[1].Trim());

            var book = _logic.GetBookById(id);
            if (book != null)
            {
                textBoxId.Text = book.Id.ToString();
                textBoxTitle.Text = book.Title;
                textBoxAuthor.Text = book.Author;
                textBoxYear.Text = book.Year.ToString();
                comboBoxGenre.SelectedItem = book.Genre;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxId.Text))
            {
                MessageBox.Show("Сначала выберите книгу для редактирования.");
                return;
            }

            var title = textBoxTitle.Text;
            var author = textBoxAuthor.Text;
            int year;
            var genre = comboBoxGenre.SelectedItem?.ToString();

            if (!int.TryParse(textBoxYear.Text, out year))
            {
                MessageBox.Show("Год должен быть числом.");
                return;
            }

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author) || string.IsNullOrEmpty(genre))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            int id = int.Parse(textBoxId.Text);
            if (_logic.UpdateBook(id, title, author, year, genre))
            {
                LoadBooks();
                ClearFields();
                MessageBox.Show("Книга обновлена.");
            }
            else
            {
                MessageBox.Show("Ошибка обновления.");
            }
        }

        private void btnGroup_Click(object sender, EventArgs e)
        {
            var groups = _logic.GroupBooksByGenre();
            var result = new StringBuilder();
            foreach (var group in groups)
            {
                result.AppendLine($"Жанр: {group.Key}");
                foreach (var book in group.Value)
                    result.AppendLine($"  {book}");
                result.AppendLine();
            }
            MessageBox.Show(result.ToString(), "Группировка по жанрам");
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            var author = textBoxSearchAuthor.Text;
            if (string.IsNullOrEmpty(author))
            {
                MessageBox.Show("Введите имя автора для поиска.");
                return;
            }

            var books = _logic.FindBooksByAuthor(author);
            if (books.Any())
            {
                var result = string.Join("\n", books.Select(b => b.ToString()));
                MessageBox.Show(result, "Найденные книги");
            }
            else
            {
                MessageBox.Show("Книги не найдены.");
            }
        }

        private void ClearFields()
        {
            textBoxId.Clear();
            textBoxTitle.Clear();
            textBoxAuthor.Clear();
            textBoxYear.Clear();
            comboBoxGenre.SelectedIndex = -1;
            textBoxSearchAuthor.Clear();
        }

        private void listBoxBooks_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Обработчик уже реализован в btnUpdate_Click
        }
    }
}