using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace solve44
{
    class node
    {
        //----------------------------------Data----------------------------------------
        public String puzzle_string;    //to use in map as key 

        public String node_direction = "";

        public int manhatten;
        public int hamming;
        public int moves;

        public int fn;                         //moves + huristic function   F(n)=g(n)+h(n) // g(n):number of moves tree level ,, h(n): hamming value

        public int[,] puzzle2D;
        public int puzzle_size;              //size of array hint size = array.length

        public int[] zero_placeXY;
        public bool flag_open_or_closed;

        public node parent;
        //-----------------------------End Of Data----------------------------------------

        //default constructor
        public node()
        {
        }
        //copy constructor
        public node(node temp)
        {
            parent = new node();
            parent = temp;
            //============================================
            this.manhatten = temp.manhatten;
            hamming = temp.hamming;
            moves = temp.moves;
            //============================================
            this.fn = temp.fn;
            //============================================        
            this.puzzle_string = temp.puzzle_string;
            //============================================
            puzzle_size = temp.puzzle_size;
            puzzle2D = new int[puzzle_size, puzzle_size];
            //============================================
            for (int i = 0; i < puzzle_size; i++)
            {
                for (int j = 0; j < puzzle_size; j++)
                {
                    puzzle2D[i, j] = temp.puzzle2D[i, j];
                }
            }
            //============================================
            zero_placeXY = new int[2];
            zero_placeXY[0] = temp.zero_placeXY[0];
            zero_placeXY[1] = temp.zero_placeXY[1];
            //============================================
            this.flag_open_or_closed = true;
        }
        //with parameters
        public node(int[,] puzzle2D, int size, int moves, int[] zero, String puzzle_string, bool huristic)
        {
            zero_placeXY = new int[2];
            // this.zero_placeXY = zero;
            this.zero_placeXY[0] = zero[0];
            this.zero_placeXY[1] = zero[1];
            puzzle_size = size;
            this.puzzle2D = new int[puzzle_size, puzzle_size];
            for (int i = 0; i < puzzle_size; i++)
            {
                for (int j = 0; j < puzzle_size; j++)
                {
                    this.puzzle2D[i, j] = puzzle2D[i, j];
                }
            }
            this.moves = moves;
            calculate_hamming();
            calculate_manhatten_distance();
            if (huristic)
            {
                fn = this.moves + hamming;
            }
            else
            {
                fn = this.moves + manhatten;
            }
            this.puzzle_string = puzzle_string;
            this.flag_open_or_closed = true;
        }
        //---------------------------------------------------------------------------------
        public bool isequal(ref node dest)
        {
            if (this.puzzle_string.Equals(dest.puzzle_string))
            {
                return true;
            }
            return false;
        }// cmp 2 nodes by thier puzzle string 
        //---------------------------------------------------------------------------------
        public void calculate_fn(bool huristic)//true for hamming , false = manhatten
        {
            if (huristic)
            {
                calculate_hamming();
                fn = moves + hamming;
            }
            else
            {
                calculate_manhatten_distance();
                fn = moves + manhatten;
            }
        }
        //---------------------------------------------------------
        public void calculate_hamming()//hamming_kamil
        {
            int counter = 0;
            for (int k = 0; k < puzzle_size; k++)  // compare elements of the two arrays
            {
                for (int m = 1; m <= puzzle_size; m++)
                {
                    if (puzzle_size * k + m != puzzle2D[k, m - 1] && puzzle2D[k, m - 1] != 0)
                    {
                        counter++;
                    }
                }
            }
            hamming = counter;
            //return counter;
        }
        //----------------------------------------------------------
        public void calculate_manhatten_distance()
        {
            int manhattens = 0;
            for (int k = 0; k < puzzle_size; k++)
            {
                for (int m = 0; m < puzzle_size; m++)
                {
                    int value = this.puzzle2D[k, m];
                    if (value != 0)
                    {
                        int x = (value - 1) / this.puzzle_size;
                        int y = (value - 1) % this.puzzle_size;
                        int dx = (k - x);
                        int dy = (m - y);
                        manhattens += Math.Abs(dx) + Math.Abs(dy);
                    }
                }
            }
            manhatten = manhattens;
            //return counter;
        }
        //---------------------------------------------------------------------------------       
        public void display_node()
        {
            for (int i = 0; i < puzzle_size; i++)
            {
                for (int j = 0; j < puzzle_size; j++)
                {
                    Console.Write(this.puzzle2D[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("manhatten distance = " + manhatten);
            Console.WriteLine("number of moves = " + moves);
        }
        //--------------------------------------------------------------------------------
        public void move_up(bool huristic)
        {
            //move up
            // swap zero 
            int temp_swap = puzzle2D[zero_placeXY[0] - 1, zero_placeXY[1]];
            puzzle2D[zero_placeXY[0] - 1, zero_placeXY[1]] = puzzle2D[zero_placeXY[0], zero_placeXY[1]];
            puzzle2D[zero_placeXY[0], zero_placeXY[1]] = temp_swap;
            //-------------------------------------------------------------------------------------------
            int index_zero = (this.puzzle_size * zero_placeXY[0] + zero_placeXY[1]);
            int index = ((this.puzzle_size * (zero_placeXY[0] - 1)) + zero_placeXY[1]);
            //--------------------------------------------------------------------------------------------
            string[] temp = puzzle_string.Split(' ');
            string xx = temp[index_zero];
            temp[index_zero] = temp[index];
            temp[index] = xx;
            //--------------------------------------------------------------------------------------------
            puzzle_string = "";
            for (int i = 0; i < (this.puzzle_size) * (this.puzzle_size); i++)
            {
                puzzle_string += temp[i] + " ";
            }
            puzzle_string.Remove(((this.puzzle_size) * (this.puzzle_size)));
            //--------------------------------------------------------------------------------------------
            this.moves++;
            //--------------------------------------------------------------------------------------------
            calculate_fn(huristic);
            zero_placeXY[0]--;
        }
        //--------------------------------------------------------------------------------
        public void move_down(bool huristic)
        {
            //move down 
            // swap zero 
            int temp_swap = puzzle2D[zero_placeXY[0] + 1, zero_placeXY[1]];
            puzzle2D[zero_placeXY[0] + 1, zero_placeXY[1]] = puzzle2D[zero_placeXY[0], zero_placeXY[1]];
            puzzle2D[zero_placeXY[0], zero_placeXY[1]] = temp_swap;
            //--------------------------------------------------------------------------------
            int index_zero = (this.puzzle_size * zero_placeXY[0] + zero_placeXY[1]);
            int index = ((this.puzzle_size * (zero_placeXY[0] + 1)) + zero_placeXY[1]);
            //--------------------------------------------------------------------------------
            string[] temp = puzzle_string.Split(' ');
            string xx = temp[index_zero];
            temp[index_zero] = temp[index];
            temp[index] = xx;
            //--------------------------------------------------------------------------------
            puzzle_string = "";
            for (int i = 0; i < (this.puzzle_size) * (this.puzzle_size); i++)
            {
                puzzle_string += temp[i] + " ";
            }
            puzzle_string.Remove(((this.puzzle_size) * (this.puzzle_size)));
            //--------------------------------------------------------------------------------
            this.moves++;
            calculate_fn(huristic);
            zero_placeXY[0]++;
        }
        //--------------------------------------------------------------------------------
        public void move_right(bool huristic)
        {
            //move right 
            int temp_swap = puzzle2D[zero_placeXY[0], zero_placeXY[1] + 1];
            puzzle2D[zero_placeXY[0], zero_placeXY[1] + 1] = puzzle2D[zero_placeXY[0], zero_placeXY[1]];
            puzzle2D[zero_placeXY[0], zero_placeXY[1]] = temp_swap;
            //--------------------------------------------------------------------------------
            int index_zero = (this.puzzle_size * zero_placeXY[0] + zero_placeXY[1]);
            int index = ((this.puzzle_size * (zero_placeXY[0])) + zero_placeXY[1] + 1);
            //--------------------------------------------------------------------------------
            string[] temp = puzzle_string.Split(' ');
            string xx = temp[index_zero];
            temp[index_zero] = temp[index];
            temp[index] = xx;
            //--------------------------------------------------------------------------------
            puzzle_string = "";
            for (int i = 0; i < (this.puzzle_size) * (this.puzzle_size); i++)
            {
                puzzle_string += temp[i] + " ";
            }
            puzzle_string.Remove(((this.puzzle_size) * (this.puzzle_size)));
            //--------------------------------------------------------------------------------
            this.moves++;
            calculate_fn(huristic);
            zero_placeXY[1]++;
        }
        //--------------------------------------------------------------------------------
        public void move_left(bool huristic)
        {
            //move left 
            int temp_swap = puzzle2D[zero_placeXY[0], zero_placeXY[1] - 1];
            puzzle2D[zero_placeXY[0], zero_placeXY[1] - 1] = puzzle2D[zero_placeXY[0], zero_placeXY[1]];
            puzzle2D[zero_placeXY[0], zero_placeXY[1]] = temp_swap;
            //--------------------------------------------------------------------------------
            int index_zero = (this.puzzle_size * zero_placeXY[0] + zero_placeXY[1]);
            int index = ((this.puzzle_size * (zero_placeXY[0])) + zero_placeXY[1] - 1);
            //--------------------------------------------------------------------------------
            string[] temp = puzzle_string.Split(' ');
            string xx = temp[index_zero];
            temp[index_zero] = temp[index];
            temp[index] = xx;
            //--------------------------------------------------------------------------------
            puzzle_string = "";
            for (int i = 0; i < (this.puzzle_size) * (this.puzzle_size); i++)
            {
                puzzle_string += temp[i] + " ";
            }
            puzzle_string.Remove(((this.puzzle_size) * (this.puzzle_size)));
            //--------------------------------------------------------------------------------
            this.moves++;
            calculate_fn(huristic);
            zero_placeXY[1]--;

        }
        //--------------------------------------------------------------------------------
        public void search_exist_befor(ref SortedDictionary<String, node> closed, ref priority_queue queue, bool huristic, String direction, ref SortedDictionary<String, node> to_del)
        {
            if (closed.ContainsKey(this.puzzle_string))
            {
                node temp = new node();
                temp = closed[this.puzzle_string];

                if (temp.flag_open_or_closed == true) ////exit in open list
                {
                    if (temp.fn > this.fn)
                    {    
                        to_del.Add(this.puzzle_string, closed[this.puzzle_string]);
                        closed.Remove(this.puzzle_string);

                        node nody = new node(this);
                        nody.node_direction = direction;

                        queue.add_to_priority_queue(nody, huristic);
                        closed.Add(this.puzzle_string, nody);
                    }
                }
                else          //exist in closed list
                {
                    if (temp.fn > this.fn)
                    {
                        closed.Remove(this.puzzle_string);

                        node nody = new node(this);
                        nody.node_direction = direction;

                        queue.add_to_priority_queue(nody, huristic);
                        closed.Add(this.puzzle_string, nody);
                    }
                }
            }
            else
            {
                node success_node = new node(this);
                success_node.node_direction = direction;

                queue.add_to_priority_queue(success_node, huristic);
                closed.Add(success_node.puzzle_string, success_node);
            }
            //##################################################
            if (direction == "down")
            {
                this.move_up(huristic);
            }
            else if (direction == "up")
            {
                this.move_down(huristic);
            }
            else if (direction == "left")
            {
                this.move_right(huristic);
            }
            else
            {
                this.move_left(huristic);
            }
            //#####################################################
            this.moves--;
            this.moves--;
            this.calculate_fn(huristic);
        }
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        public void nighbors(ref SortedDictionary<String, node> closed, ref priority_queue queue, bool huristic, ref SortedDictionary<String, node> to_del)
        {
            int row = zero_placeXY[0];
            int coloum = zero_placeXY[1];

            if (row == 0 && coloum == 0)
            {
                //-------------------------------- probability (1) in corner top left and will move down -------------------------------------------
                this.move_down(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "down", ref to_del);
                //------------------------------------------------------------------------------------------------------------------------------------
                //-------------------------------//1 probability (1) in corner top left and will move right//---------------------------------------------
                this.move_right(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "right", ref to_del);
            }
            //=================================================================================================================================================================
            // corner top right 
            else if (row == 0 && coloum == puzzle_size - 1)//lo f el corner -> 2 a7tmal
            {
                // 2 probability in corner top right and will move down 
                this.move_down(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "down", ref to_del);
                //----------------------------------------------------------------------------------------------------------------------------
                // 2 probability in corner top right and will move left  
                this.move_left(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "left", ref to_del);
            }
            //=================================================================================================================================================================
            //corner down left 
            else if (row == puzzle_size - 1 && coloum == 0)//lo f el corner -> 2 a7tmal
            {
                //3 probability in corner down left and move up
                this.move_up(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "up", ref to_del);
                //--------------------------------------------------------------------------------------------------------------
                //3 probability in corner down left and move right
                this.move_right(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "right", ref to_del);
            }
            //=================================================================================================================================================================

            //4 probability in corner down right
            //corner down right
            else if (row == puzzle_size - 1 && coloum == puzzle_size - 1)//lo f el corner -> 2 a7tmal
            {
                //4 probability in corner down right and move up
                this.move_up(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "up", ref to_del);
                //------------------------------------------------------------------------------------------------------
                //4 probability in corner down right and move left
                this.move_left(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "left", ref to_del);
            }
            //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
            // edge up
            else if (row == 0 && (coloum != 0 || coloum != puzzle_size - 1))//lo 3la 7arf w m4 f el zoiah -> 3 a7tmlat
            {
                //edge up then -> not move up but left , right and down 
                // move left 
                this.move_left(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "left", ref to_del);
                //********************************************************************************************************************
                // move right 
                this.move_right(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "right", ref to_del);
                //********************************************************************************************************************
                // move down 
                this.move_down(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "down", ref to_del);
            }
            //********************************************************edge down**************************************************************************************
            else if (row == puzzle_size - 1)//lo 3la 7arf w m4 f el zoiah -> 3 a7tmlat
            {
                // edge down -> move up or left or right 
                //move up
                this.move_up(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "up", ref to_del);
                //-----------------------------------------------------------------------------------------------------------------------------
                //move left 
                this.move_left(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "left", ref to_del);
                //-----------------------------------------------------------------------------------------------------------------------------------               
                //move right 
                this.move_right(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "right", ref to_del);
            }
            //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
            //***************************************edge left*******************************************************************
            //edge left
            else if (coloum == 0)//lo 3la 7arf w m4 f el zoiah -> 3 a7tmlat
            {
                //edge is left -> move up or down or right 
                // move up 
                this.move_up(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "up", ref to_del);
                //---------------------------------------------------------------------------------------------------------------------------------
                //move down 
                this.move_down(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "down", ref to_del);
                //-------------------------------------------------------------------------------------------------------------------------------------
                //move right 
                this.move_right(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "right", ref to_del);
            }
            //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
            //**********************edge right*************************************************
            else if (coloum == puzzle_size - 1)//lo 3la 7arf w m4 f el zoiah -> 3 a7tmlat
            {
                //edge is right -> left or down or up
                //--------------------------------------------------------------------------------------------------------------------------------------------------------
                //move left 
                this.move_left(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "left", ref to_del);
                //---------------------------------------------------------------------------------------------------------------------------------------------------------
                //move up 
                this.move_up(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "up", ref to_del);
                //---------------------------------------------------------------------------------------------------------------------
                //move down 
                this.move_down(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "down", ref to_del);
            }
            //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
            //****************************************in middle of puzzle************************************************************  
            else
            {
                //in the middle -> left or right or up or down 
                //-----------------------------------------------------------------------------------------------------------------------------------------------
                //move left 
                this.move_left(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "left", ref to_del);
                //----------------------------------------------------------------------------------------------------------------------------------------------------------------
                //move right 
                this.move_right(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "right", ref to_del);
                //-------------------------------------------------------------------------------------------------------------------------------------------------------------------
                //move up
                this.move_up(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "up", ref to_del);
                //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
                //move down 
                this.move_down(huristic);
                search_exist_befor(ref closed, ref queue, huristic, "down", ref to_del);
            }
        }//end of nighbor function
    }
}