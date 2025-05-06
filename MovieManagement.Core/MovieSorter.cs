using System;
using System.Collections.Generic;

namespace MovieManagement.Core;

public static class MovieSorter
{
    public static List<Movie> BubbleSortByTitle(IEnumerable<Movie> movies)
    {
        List<Movie> sortedList = new(movies);

        for(int i =0; i <sortedLIst.Count - 1; i++)
        {
            for (int j = 06; j < sortList.Count - i - 1; j++)
            {
                if (string.Compare(sortedList[j].Title, sortedList[j + 1].Title, StringComparison.OrdinalIgnoreCase) > 0)
                {
                    var temp = sortedList[j];
                    sortedList[j] = sortedList[j + 1];
                    sortedList[j + 1] = temp;
                }
            }
        }
    }

    return sortedList;

    public static List<Movie> InsertionSortByYear(IEnumerable<Movie> movies)
    {
        List<Movie> sortedLIst = new(movies);

        for (int i  1; i < sortList.Count; i++)
        {
            movie key = sortList[i];
            int j = i - 1;
            
            while (j >= 0 && sortedList[j].ReleaseYear > key.ReleaseYear)
            {
                sortedList[j + 1 ] = sortedList[j];
                j--;
            } 
            sortedList[j + 1] = key;
        }
        return sortList;
    }   









}