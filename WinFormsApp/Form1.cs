using BookManagementSystem.Logic;
using BookManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        private BookLogic _logic;

        public Form1()
        {
            InitializeComponent();
            _logic = new BookLogic();
            // �������� ��������� �������
            comboBoxGenre.Items.AddRange(new string[] {
                "����������",
                "�����",
                "��������",
                "������� ����������",
                "�����������",
                "������������ �����",
                "���������",
                "������"
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
                MessageBox.Show("��� ������ ���� ������.");
                return;
            }

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author) || string.IsNullOrEmpty(genre))
            {
                MessageBox.Show("��������� ��� ����.");
                return;
            }

            _logic.CreateBook(title, author, year, genre);
            LoadBooks();
            ClearFields();
            MessageBox.Show("����� ���������!");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBoxBooks.SelectedIndex == -1)
            {
                MessageBox.Show("�������� ����� ��� ��������.");
                return;
            }

            var selected = listBoxBooks.SelectedItem.ToString();
            var id = int.Parse(selected.Split(',')[0].Split(':')[1].Trim());

            if (_logic.DeleteBook(id))
            {
                LoadBooks();
                ClearFields();
                MessageBox.Show("����� �������.");
            }
            else
            {
                MessageBox.Show("������ ��������.");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (listBoxBooks.SelectedIndex == -1)
            {
                MessageBox.Show("�������� ����� ��� ��������������.");
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
                MessageBox.Show("������� �������� ����� ��� ��������������.");
                return;
            }

            var title = textBoxTitle.Text;
            var author = textBoxAuthor.Text;
            int year;
            var genre = comboBoxGenre.SelectedItem?.ToString();

            if (!int.TryParse(textBoxYear.Text, out year))
            {
                MessageBox.Show("��� ������ ���� ������.");
                return;
            }

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author) || string.IsNullOrEmpty(genre))
            {
                MessageBox.Show("��������� ��� ����.");
                return;
            }

            int id = int.Parse(textBoxId.Text);
            if (_logic.UpdateBook(id, title, author, year, genre))
            {
                LoadBooks();
                ClearFields();
                MessageBox.Show("����� ���������.");
            }
            else
            {
                MessageBox.Show("������ ����������.");
            }
        }

        private void btnGroup_Click(object sender, EventArgs e)
        {
            var groups = _logic.GroupBooksByGenre();
            var result = new StringBuilder();
            foreach (var group in groups)
            {
                result.AppendLine($"����: {group.Key}");
                foreach (var book in group.Value)
                    result.AppendLine($"  {book}");
                result.AppendLine();
            }
            MessageBox.Show(result.ToString(), "����������� �� ������");
        }

        // ���������� ����� ������ - ���� �� ������ ��� �� ��������
        private void btnFind_Click(object sender, EventArgs e)
        {
            var author = textBoxSearchAuthor.Text;

            List<Book> books = new List<Book>();

            if (!string.IsNullOrWhiteSpace(author))
            {
                // ����� �� ������
                books = _logic.FindBooksByAuthor(author);
            }
            else
            {
                MessageBox.Show("������� ��� ������ ��� ������.");
                return;
            }

            if (books.Any())
            {
                var result = string.Join("\n", books.Select(b => b.ToString()));
                MessageBox.Show(result, "��������� �����");
            }
            else
            {
                MessageBox.Show("����� �� �������.");
            }
        }

        // �������������� ����� - ��������� ������ ��� ������ �� ��������
        private void btnFindTitle_Click(object sender, EventArgs e)
        {
            var title = textBoxSearchTitle.Text;
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("������� �������� ����� ��� ������.");
                return;
            }

            var books = _logic.FindBooksByTitle(title);
            if (books.Any())
            {
                var result = string.Join("\n", books.Select(b => b.ToString()));
                MessageBox.Show(result, "��������� �����");
            }
            else
            {
                MessageBox.Show("����� �� �������.");
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
            textBoxSearchTitle.Clear();
        }

        private void listBoxBooks_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ���������� ��� ���������� � btnUpdate_Click
        }

        private void textBoxSearchTitle_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnFindTitle_Click_1(object sender, EventArgs e)
        {
            var title = textBoxSearchTitle.Text;
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("������� �������� ����� ��� ������.");
                return;
            }

            var books = _logic.FindBooksByTitle(title);
            if (books.Any())
            {
                var result = string.Join("\n", books.Select(b => b.ToString()));
                MessageBox.Show(result, "��������� �����");
            }
            else
            {
                MessageBox.Show("����� �� �������.");
            }
        }
    }
}