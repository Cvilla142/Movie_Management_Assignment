using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using MovieManagement.Core;

namespace MovieManagement.UI
{
    public partial class MainWindow : Window
    {
        private readonly MovieManager _manager = new MovieManager();
        private readonly MovieBorrower _borrower;

        public MainWindow()
        {
            InitializeComponent();
            _borrower = new MovieBorrower(_manager);

            RefreshGrid();
        }

        private void RefreshGrid()
        {
            MoviesGrid.ItemsSource = null;
            MoviesGrid.ItemsSource = _manager.GetAllMovies().ToList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(AddYearBox.Text, out var year))
            {
                MessageBox.Show("Please enter a valid year.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var movie = new Movie(
                AddIdBox.Text.Trim(),
                AddTitleBox.Text.Trim(),
                AddDirectorBox.Text.Trim(),
                AddGenreBox.Text.Trim(),
                year
            );

            try
            {
                _manager.AddMovie(movie);
                RefreshGrid();
                MessageBox.Show("Movie added.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show($"Duplicate ID '{movie.MovieId}'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            AddIdBox.Clear();
            AddTitleBox.Clear();
            AddDirectorBox.Clear();
            AddGenreBox.Clear();
            AddYearBox.Clear();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            MoviesGrid.ItemsSource = _manager.SearchByTitle(SearchBox.Text.Trim());
        }

        private void SortTitle_Click(object sender, RoutedEventArgs e)
        {
            _manager.SortByTitle();
            RefreshGrid();
        }

        private void SortYear_Click(object sender, RoutedEventArgs e)
        {
            _manager.SortByReleaseYear();
            RefreshGrid();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "JSON Files|*.json" };
            if (dlg.ShowDialog() == true)
            {
                _manager.ImportFromJson(dlg.FileName);
                RefreshGrid();
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "JSON Files|*.json" };
            if (dlg.ShowDialog() == true)
            {
                _manager.ExportToJson(dlg.FileName);
            }
        }

        private void Borrow_Click(object sender, RoutedEventArgs e)
        {
            var movieId = BorrowIdBox.Text.Trim();
            var userId  = BorrowUserBox.Text.Trim();
            var user    = new User(userId, userId, DateTime.Now);

            try
            {
                _borrower.BorrowMovie(movieId, user);
                RefreshGrid();
            }
        catch (KeyNotFoundException)
        {
            MessageBox.Show($"Movie '{movieId}' not found.");
        }
            BorrowIdBox.Clear();
            BorrowUserBox.Clear();
        }

        // This method centralizes the return logic for both button click and Enter key.
        private void HandleReturn(string movieId)
        {
            // Lookup the movie so we can show its Title
            var movie = _manager.SearchByID(movieId)
                        ?? throw new KeyNotFoundException($"Movie with ID '{movieId}' not found.");

            // Now carry on using `movie` instead of raw id…
            var nextUser = _borrower.ReturnMovie(movieId);
            RefreshGrid();

            if (nextUser != null)
            {
                MessageBox.Show(
                    $"\"{movie.Title}\" has been auto-borrowed to {nextUser.Name}.",
                    "Movie Reassigned",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            else
            {
                MessageBox.Show(
                    $"\"{movie.Title}\" has been returned and is now available.",
                    "Movie Returned",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }


        // Wire this up in XAML: KeyDown="MoviesGrid_KeyDown"
        private void MoviesGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && MoviesGrid.SelectedItem is Movie selected)
            {
                HandleReturn(selected.MovieId);
            }
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            // 1) Determine which Movie ID to use:
            var movieId = BorrowIdBox.Text.Trim();
            if (string.IsNullOrEmpty(movieId))
            {
                if (MoviesGrid.SelectedItem is Movie sel)
                {
                    movieId = sel.MovieId;
                }
                else
                {
                    MessageBox.Show(
                        "Please select a movie in the list or type its ID.",
                        "No Movie Selected",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }
            }

            // 2) Call the shared logic
            HandleReturn(movieId);

            // 3) Clear the input box
            BorrowIdBox.Clear();
            BorrowUserBox.Clear();
        }

    }
}
