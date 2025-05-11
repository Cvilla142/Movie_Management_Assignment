using System;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

            var query = SearchBox.Text.Trim();
            var selectedItem = SearchTypeBox.SelectedItem as ComboBoxItem;
            var mode = selectedItem?.Content?.ToString();

            if (mode == "ID")
            {
                var movie = _manager.SearchByID(query);

                MoviesGrid.ItemsSource = movie != null
                    ? new List<Movie> { movie }
                    : new List<Movie>();
            }
            else
            {
                var results = _manager.SearchByTitle(query);
                MoviesGrid.ItemsSource = results;
            }
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
            var dlg = new OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json"
            };

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    _manager.ImportFromJson(dlg.FileName);
                    RefreshGrid();
                    MessageBox.Show(
                        "Movies imported successfully.",
                        "Import Complete",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Import Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
                catch (JsonException ex)
                {
                    MessageBox.Show(
                        $"Invalid JSON format:\n{ex.Message}",
                        "Import Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error importing movies:\n{ex.Message}",
                        "Import Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }


        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Filter = "JSON Files (*.json)|*.json",
                FileName = "movies.json"
            };

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    _manager.ExportToJson(dlg.FileName);
                    MessageBox.Show(
                        "Movies exported successfully.",
                        "Export Complete",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error exporting movies:\n{ex.Message}",
                        "Export Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        private void Borrow_Click(object sender, RoutedEventArgs e)
        {
            var movieId = BorrowIdBox.Text.Trim();
            var userId  = BorrowUserBox.Text.Trim();

            if (_manager.SearchByID(movieId) is null)
            {
                MessageBox.Show(
                    "Movie ID not found. Please make sure it’s correct (case‐sensitive).",
                    "Invalid Movie ID",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            var user = new User(userId, userId, DateTime.Now);

            try
            {
                _borrower.BorrowMovie(movieId, user);
                RefreshGrid();
            }
            catch (KeyNotFoundException)
            {

                MessageBox.Show(
                    $"Movie '{movieId}' not found.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }

            BorrowIdBox.Clear();
            BorrowUserBox.Clear();
        }

        private void HandleReturn(string movieId)
        {

            var movie = _manager.SearchByID(movieId)
                        ?? throw new KeyNotFoundException($"Movie with ID '{movieId}' not found.");


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



        private void MoviesGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && MoviesGrid.SelectedItem is Movie selected)
            {
                HandleReturn(selected.MovieId);
            }
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            var rawId = BorrowIdBox.Text.Trim();
            string movieId;

            if (!string.IsNullOrEmpty(rawId))
            {
                movieId = rawId;
            }
            else if (MoviesGrid.SelectedItem is Movie sel)
            {
                movieId = sel.MovieId;
            }
            else
            {
                MessageBox.Show(
                    "Please select a movie from the list or enter its ID.",
                    "No Movie Selected",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }
            if (_manager.SearchByID(movieId) is null)
            {
                MessageBox.Show(
                    $"Movie ID '{movieId}' not found. Please make sure it’s correct.",
                    "Invalid Movie ID",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }
            HandleReturn(movieId);
            
            BorrowIdBox.Clear();
            BorrowUserBox.Clear();
        }



            private void MoviesGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && MoviesGrid.SelectedItem is Movie selected)
            {
                HandleReturn(selected.MovieId);

                e.Handled = true;
            }
        }
    }
}
