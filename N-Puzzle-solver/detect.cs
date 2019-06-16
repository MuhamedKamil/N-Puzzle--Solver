using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace solve44
{
    class detect
    {
        //----------------------data----------------------------------------------------------------------------------------------------------------
        public static int swaps = 0;//using to find swaps
        public static int sizeL = 0;//using to find swaps
        /***********************how many swaps***************************/
        //********************************divide step***********************************************/
        public static void calc_swaps(int[] arr, int start, int end)
        {
            if (start < end)
            {
                int mid = (end + start) / 2;
                calc_swaps(arr, start, mid);
                calc_swaps(arr, mid + 1, end);
                merge_swap(arr, start, mid, end);
            }
        }
        //********************************** conqer step ***********************************************///
        static void merge_swap(int[] arr, int start, int mid, int end)
        {
            sizeL = (mid - start) + 1;       //size of left branch
            int[] temp = new int[end - start + 1];//merging in temp array
            int i = start, j = mid + 1, k = 0;
            //merge and calculate swaps
            while (i <= mid && j <= end)
            {
                if (arr[i] < arr[j])
                {
                    temp[k] = arr[i];
                    sizeL--;
                    k++;
                    i++;
                }
                else
                {
                    temp[k] = arr[j];
                    swaps += sizeL;
                    k++;
                    j++;
                }
            }
            //push remaind elements in left branch
            while (i <= mid)
            {
                temp[k] = arr[i];
                k++;
                i++;
            }
            // push remaind elements in right branch
            while (j <= end)
            {
                temp[k] = arr[j];
                k++;
                j++;
                sizeL--;
            }
            // reback elements to original array after merged
            k = 0;
            i = start;
            while (k < temp.Length && i <= end)
            {
                arr[i] = temp[k];
                i++;
                k++;
            }
        }
        //*****************************************************************//
        /************************end of how many swaps above************/
        public static bool detect_puzzle(int[,] puzzle2D, int puzzle_size, int zero_row)//recieve 1D array and decied is solved or not
        {
            int[] n_puzzle = new int[puzzle_size * puzzle_size];
            int pointer = 0;
            int zero_place = 0;
            for (int i = 0; i < puzzle_size; i++)
            {
                for (int k = 0; k < puzzle_size; k++)
                {
                    if (puzzle2D[i, k] == 0)
                    {
                        zero_place = pointer;
                    }
                    n_puzzle[pointer] = puzzle2D[i, k];
                    pointer++;
                }
            }
            //***********************set data/intialize *******************************************************************************

            swaps = 0; //number of swaps 
            sizeL = 0;//this var will use it in calculate swap when merging the array
            //**************************************************************************************************************************
            if (puzzle_size % 2 == 0)// check if puzzle even or odd 
            {//if even [the puzzle's swaps number + zero row position must equal odd number to be solved , else the puzzle is unsolved

                calc_swaps(n_puzzle, 0, puzzle_size * puzzle_size - 1);  //n-puzzl // calculte number of swaps in O(nlogn) in worth case
                swaps = (swaps - zero_place) /*+ 1*/;                // this equation to calculate the actually number of swaps if zero is not exist in the array
                swaps += zero_row;                         //here to check the puzzle is solved or not i add the zero raw place to swaps actualyy number if zero didn't exist and if swaps after add row number is odd so it's solved else unsolved
                //--------------------------check-------------------------------------------------------------------------------------------
                if (swaps % 2 != 0)
                {
                   // Console.WriteLine("Solved");
                    return true;
                }
                else
                {
                    //Console.WriteLine("Un-Solved");
                    return false;
                }
                //-------------------------------------------------------------------------------------------------------------end of checking-
            }
//********************************************************************************************************************************************
            else // here if the puzzle size is odd will repeat the above statement except add the row number to actually swap number if zero doesn't exist 
            // and swaps number must be even to be solved
            {
                calc_swaps(n_puzzle, 0, puzzle_size * puzzle_size - 1);//n-puzzl 
                swaps = (swaps - zero_place) /*+ 1*/;

                if (swaps % 2 == 0)
                {
                    //Console.WriteLine("Solved");
                    return true;
                }
                else
                {
                   // Console.WriteLine("Un-Solved");
                    return false;
                }
            }

        }
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

    }
}
