using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MiniLibrary
{
    public partial class MainWindow : Window
    {
        private List<Book> books = new List<Book>();

        public MainWindow()
        {
            InitializeComponent();
            books.Add(new Book
            {
                Title = "Война и мир",
                Author = "Лев Толстой",
                Genre = "Классика",
                Rating = 5,
                IsRead = true
            });

            books.Add(new Book
            {
                Title = "1984",
                Author = "Джордж Оруэлл",
                Genre = "Антиутопия",
                Rating = 4,
                IsRead = false
            });

            RefreshBookList();
        }

        public class Book
        {
            public string Title { get; set; }
            public string Author { get; set; }
            public string Genre { get; set; }
            public int Rating { get; set; }
            public bool IsRead { get; set; }
        }

        private void RefreshBookList()
        {
            BooksListView.Items.Clear();

            foreach (var book in books)
            {
                var item = new ListViewItem();
                item.Content = $"{book.Title} | {book.Author} | {book.Genre} | ★{book.Rating} | " +
                             (book.IsRead ? "Прочитано" : "Не прочитано");
                item.Tag = book;
                BooksListView.Items.Add(item);
            }

            UpdateStats();
        }

        private void UpdateStats()
        {
            TotalBooksText.Text = $"Всего книг: {books.Count}";
            ReadBooksText.Text = $"Прочитано: {books.Count(b => b.IsRead)}";
            AvgRatingText.Text = $"Средний рейтинг: {(books.Any() ? books.Average(b => b.Rating).ToString("0.0") : "0.0")}";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newBook = new Book
            {
                Title = TitleTextBox.Text,
                Author = AuthorTextBox.Text,
                Genre = GenreComboBox.Text,
                Rating = int.Parse(RatingComboBox.Text.Split(' ')[0]),
                IsRead = false
            };

            books.Add(newBook);
            RefreshBookList();

            TitleTextBox.Text = "";
            AuthorTextBox.Text = "";
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (BooksListView.SelectedItem is ListViewItem item && item.Tag is Book book)
            {
                books.Remove(book);
                RefreshBookList();
            }
        }

        private void ToggleReadButton_Click(object sender, RoutedEventArgs e)
        {
            if (BooksListView.SelectedItem is ListViewItem item && item.Tag is Book book)
            {
                book.IsRead = !book.IsRead;
                RefreshBookList();
            }
        }

        private void SortByAuthor_Click(object sender, RoutedEventArgs e)
        {
            books = books.OrderBy(b => b.Author).ToList();
            RefreshBookList();
        }

        private void SortByRating_Click(object sender, RoutedEventArgs e)
        {
            books = books.OrderByDescending(b => b.Rating).ToList();
            RefreshBookList();
        }

        private void BooksListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BooksListView.SelectedItem is ListViewItem item && item.Tag is Book book)
            {
                TitleTextBox.Text = book.Title;
                AuthorTextBox.Text = book.Author;
                GenreComboBox.Text = book.Genre;
                RatingComboBox.SelectedIndex = book.Rating - 1;
                books.Remove(book);
                RefreshBookList();
            }
        }
    }
}