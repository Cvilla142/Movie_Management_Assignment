using System.Windows;
using Microsoft.Win32;
using MovieManagement.Core;

namespace MovieManagement.UI
{
    public partial class MainWindow : Window
    {
        private readonly MovieManager _manager = new MovieManager();

        public MainWindow()
        {
            InitializeComponent();
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            MoviesGrid.ItemsSource = null;
            MoviesGrid.ItemsSource = _manager.GetAllMovies();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var results = _manager.SearchByTitle(SearchBox.Text);
            MoviesGrid.ItemsSource = results;
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
            var dlg = new OpenFileDialog { Filter = "JSON Files (*.json)|*.json" };
            if (dlg.ShowDialog() == true)
            {
                _manager.ImportFromJson(dlg.FileName);
                RefreshGrid();
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "JSON Files (*.json)|*.json" };
            if (dlg.ShowDialog() == true)
            {
                _manager.ExportToJson(dlg.FileName);
            }
        }
    }
}
