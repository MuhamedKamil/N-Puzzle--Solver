using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace solve44
{
    class file
    {
        //**********data******************************
        String file_path;                        //file path or name
        String line;                             //will read line and save the line in this string to split it and store it in array
        long bytesRead = 0;                      //this variable is declared to store the position of last puzzle had readed 
        int puzzle_size;                         //number of line in the puzzle will read 
        int[,] puzzle_arr_2D;
        int arr_pointer = 0;
        int zero_place;
        int[] zero_placeXY;
        //********************************************
        //*****constructor take the file name or path************
        public file(String file_path)
        {
            this.file_path = file_path;
            zero_placeXY = new int[2];
        }
        //-------------------------------------------------
        //getter
        public int get_puzzle_size()
        {
            return puzzle_size;
        }
        //------------------------------------------------
        public int[,] get_puzzle2D()
        {
            return puzzle_arr_2D;
        }
        //------------------------------------------------
        //*******the below function will read puzzle by puzzle " take the position of the puzzle to read and return the position of next puzzle (end of the puzzle is already readed)
        public long read_from_specific_pos(long pos)
        {
            bytesRead = 0;
            int newLineBytes = System.Environment.NewLine.Length;//i don't know actually what is really this line doing !
            //----------------------------------------------------------------------------------------------------------------
            using (var sr = new StreamReader(file_path))       //and also what is word "Using" doing 
            {
                sr.BaseStream.Seek(pos, SeekOrigin.Begin);     //seek the reader to the position
                //------------------------------------------------------------------------------------
                line = sr.ReadLine();                          //read line
                bytesRead += line.Length + newLineBytes;       //calculate the current position
                //------------------------------------------------------------------------------------
                puzzle_size = int.Parse(line);                               //after reading the first line for the current puzzle will convert it to get the size off puzzle
               // puzzle_arr_1D = new int[(puzzle_size * puzzle_size)];       //intializing of 1D array of (n*n)-1 Hint !: zero in puzzle don't calculated 
                puzzle_arr_2D = new int[puzzle_size, puzzle_size];             //intializing of 2D array of n*n Hint !:zero calculated
                arr_pointer = 0;                                           //pointer of the 1D array 
                //---outer loop---------------------------------------------------------------------------------------------------------------
                for (int i = 0; i < puzzle_size; i++)                         //this loop to fill 1D array
                {
                    line = sr.ReadLine();                        //read line  
                    bytesRead += line.Length + newLineBytes;     //calculate the current position 
                    string[] splite = line.Split(' ');           //splite the line which has readed to fill the values in 1D array
                    //inner loop****************************************************************************--------------------------------
                    for (int l = 0; l < puzzle_size; l++)         //fill 1D array
                    {
                        if (int.Parse(splite[l]) == 0)
                        {
                            zero_place = arr_pointer;
                            zero_placeXY[0] = i;
                            zero_placeXY[1] = l;
                        }
                    //    puzzle_arr_1D[arr_pointer] = int.Parse(splite[l]);
                        puzzle_arr_2D[i, l] = int.Parse(splite[l]); //zero calculated
                        arr_pointer++;

                    }
                    //***************************************************************end of inner loop
                }
                //---------------------------------------------------------------------------------------------end of outter loop
                pos += bytesRead;
                sr.Close();
            }//end of using function
            return pos;
        }
        //---------------------------------------------------------------------------
        public bool not_end(long pos)
        {
            StreamReader sr = new StreamReader(file_path);
            sr.BaseStream.Seek(pos, SeekOrigin.Begin);
            if (sr.Peek() == -1)
            {
                sr.Close();
                return false;
            }
            else
            {
                sr.Close();
                return true;
            }

        }
        //-------------------------------
        public int[] get_zero_place()
        {
            return zero_placeXY;
        }
    }
}
