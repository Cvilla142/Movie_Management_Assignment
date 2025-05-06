using System.Collections.Generic;
using MovieManagement.Core;

namespace MovieManagement.Core
{
    public static class MovieSorter
    {
        public static IList<Movie> BubbleSortByTitle(IEnumerable<Movie> movies)
        {
            var sortedList = new List<Movie>(movies);
            int n = sortedList.Count;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (string.Compare(sortedList[j].Title, sortedList[j + 1].Title, true) > 0)
                    {
                        (sortedList[j], sortedList[j + 1]) = (sortedList[j + 1], sortedList[j]);
                    }
                }
            }

            return sortedList;
        }

        public static IList<Movie> InsertionSortByYear(IEnumerable<Movie> movies)
        {
            var sortedList = new List<Movie>(movies);

            for (int i = 1; i < sortedList.Count; i++)
            {
                var key = sortedList[i];
                int j = i - 1;

                while (j >= 0 && sortedList[j].ReleaseYear > key.ReleaseYear)
                {
                    sortedList[j + 1] = sortedList[j];
                    j--;
                }

                sortedList[j + 1] = key;
            }

            return sortedList;
        }
    }
}
