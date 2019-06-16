using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace solve44
{
    class priority_queue
    {
        //-----data----------------------------------------------------------
        List<node> queue;// list 
        public int queue_length;
        //----------------------------------------------------------end of data
        //constructor
        public priority_queue()
        {
            queue = new List<node>(); //intialize new list(queue)
            queue_length = 0;
        }
        //---------------------------------------------------------------------
        //clear queue
        public void clear()
        {
            queue.Clear();
            queue_length = 0;
        }
        //----------------------------------------------------------------------
        public void swap(ref List<node> qeueu, int index1, int index2)
        {
            node temp = qeueu[index1];
            qeueu[index1] = qeueu[index2];
            qeueu[index2] = temp;
        }
        //----------------------------------------------------------------------
        //boolen is-empty
        public bool isempty()//isempty check the queue empty or not
        {
            if (queue_length == 0)
                return true;
            else
                return false;
        }
        //------------------------------------------------------------------------
        //************************************************************************
        //add to queue
        public void add_to_priority_queue(node puzzle, bool huristic)  //adding to priority queue using heaping sort complixty O(log-n)
        {
            if (isempty())  //check if empty add without sorting
            {
                queue.Add(puzzle);
                queue_length++;
            }
            else            // if not empty will add then will sort using heap sort
            {
                queue.Add(puzzle);
                queue_length++;
                heap_sort_adding(huristic); //call heap sort function already implmented below
            }
        }
        //------------------------------------------------------------------------
        private void heap_sort_adding(bool huristic)
        {
            //-------helper data------------------------------------------------------------------------
            bool leaf;                        //this boolen var to decide the current leaf is the right child or left child
            int i = queue_length - 1;
            //-------------------------------------------------------------------------end of helper data
            while (i > 1) //compare the child with his parent if child < parent swap else break (target : smallest value is the root for every child)
            {
                //----------------------------------check-------------------------------------------------------------------------------------
                if ((i) % 2 == 0) //so always left child in even index and right child in odd index due to equation
                    leaf = false;//false mean left leaf
                else
                    leaf = true;//true mean right leaf
                //------------------------------------------------end of checking-------------------------------------------------------------
                if (leaf)//right
                {
                    if (queue[i - 1].fn < queue[((i - 1) / 2) - 1].fn)//compare right child with his parent
                    {
                        swap(ref queue, i - 1, ((i - 1) / 2) - 1);
                        i = ((i - 1) / 2);
                    }
                    else if (queue[i - 1].fn == queue[((i - 1) / 2) - 1].fn)
                    {
                        if (huristic)
                        {
                            if (queue[i - 1].hamming < queue[((i - 1) / 2) - 1].hamming)
                            {
                                swap(ref queue, i - 1, ((i - 1) / 2) - 1);
                                i = ((i - 1) / 2);
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (queue[i - 1].manhatten < queue[((i - 1) / 2) - 1].manhatten)
                            {
                                swap(ref queue, i - 1, ((i - 1) / 2) - 1);
                                i = ((i - 1) / 2);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                        break;
                }
                //**************************************************************************************************************************************
                else
                {
                    if (queue[i - 1].fn < queue[(i / 2) - 1].fn)//compare left child with his parent
                    {
                        swap(ref queue, i - 1, (i / 2) - 1);
                        i = (i / 2);
                    }
                    else if (queue[i - 1].fn == queue[(i / 2) - 1].fn)
                    {
                        if(huristic)
                        {
                            if (queue[i - 1].hamming < queue[(i / 2) - 1].hamming)
                            {
                                swap(ref queue, i - 1, (i / 2) - 1);
                                i = (i / 2);
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (queue[i - 1].manhatten < queue[(i / 2) - 1].manhatten)
                            {
                                swap(ref queue, i - 1, (i / 2) - 1);
                                i = (i / 2);
                            }
                            else
                            {
                                break;
                            }
                        }    
                    }
                    else
                        break;
                }
                //****************************************************************************************************************************************
            }//end of while loop

            if (queue_length == 2)
            {
                if (queue[1].fn < queue[0].fn)//compare left child with his parent
                {
                    swap(ref queue,0,1);
                }
                else if (queue[1].fn == queue[0].fn)
                {
                    if(huristic)
                    {
                        if (queue[1].hamming < queue[0].hamming)
                        {
                            swap(ref queue, 0, 1);
                        }
                    }
                    else
                    {
                        if (queue[1].manhatten < queue[0].manhatten)
                        {
                            swap(ref queue, 0, 1);
                        }
                    }
                }
            }
        }//end of heap_sort_function
        //-----------------------------------------------------------------------------------------------------------------------------------
        //**************************************************************************
        public node remove_from_priority_queue(bool huristic)
        {
            //----helper data--------------
            int i = queue_length - 1;
            node temp = queue[0];
            //----------end of helper data
            //--------------------------------------swaping root 1st element with last element
            swap(ref queue, 0, i);
            queue.RemoveAt(i);
            queue_length--;
            //--------------------------------------------------------------swaping done-------
            heap_sort_removing(huristic); //re-heaping
            return temp; //return the root after restore it in temp value then i deleted the root
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void heap_sort_removing(bool huristic)//Hint :! the difference between heaping add and heaping remove is start node ,, here i start from top(root)
        {
            int i = 1;//i=1 to start from the root (actually root at index zero not one but i always make i not equal zero to help me in multiply and division
            //--------------------------------------------end of helper data----
            node left;
            node right;
            node root;
            int left_hn;
            int right_hn;
            int root_hn;
            int left_fn;
            int right_fn;
            int root_fn;

            while ((i * 2) <= queue_length && (i * 2) + 1 <= queue_length)//till not reaching to end Hint :! note that i every time increment by i*2 or i*2+1
            {
                //left exist and right exist && left<right && left<root
                if (i * 2 <= queue_length && (i * 2) + 1 <= queue_length)
                {
                    left = queue[(i * 2) - 1];
                    right = queue[(i * 2)];
                    root = queue[i - 1];
                    //-----------------------------------
                    if(huristic)
                    {
                        left_hn = left.hamming;
                        right_hn = right.hamming;
                        root_hn = root.hamming;
                    }
                    else
                    {
                        left_hn = left.manhatten;
                        right_hn = right.manhatten;
                        root_hn = root.manhatten;
                    }
                    
                    left_fn = left.fn;
                    //-------------------------------------
                    right_fn = right.fn;
                    //--------------------------------------
                    root_fn = root.fn;
                    //************************************************

                    //-----------re-heaping from top to bottom------------------------------------------
                    //if left < right && (left<root || (left==root && leftman<root man)
                    if (left_fn < right_fn && (left_fn < root_fn || (left_fn == root_fn && left_hn < root_hn)))
                    {
                        swap(ref queue, i - 1, (i * 2) - 1);
                        i = i * 2;
                        continue;
                    }

                     //right < left && (right < root || (right=root && right man <root man))
                    else if (right_fn < left_fn && (right_fn < root_fn || (right_fn == root_fn && right_hn < root_hn)))
                    {
                        swap(ref queue, i - 1, (i * 2));
                        i = (i * 2) + 1;
                        continue;
                    }

                       //left == right && left-manming < right-manming && (left< root || (left==root  && left-manming<root-manming)  
                    else if (left_fn == right_fn && (left_hn < right_hn && (left_fn < root_fn || (left_fn == root_fn && left_hn < root_hn))))
                    {
                        swap(ref queue, i - 1, (i * 2) - 1);
                        i = i * 2;
                        continue;
                    }
                    //right == left && right-manming < left-manming && (right< root || (right==root  && right-manming<root-manming) 
                    else if (right_fn == left_fn && (right_hn < left_hn && (right_fn < root_fn || (right_fn == root_fn && right_hn < root_hn))))
                    {
                        swap(ref queue, i - 1, (i * 2));
                        i = (i * 2) + 1;
                        continue;
                    }
                    else if (right_fn == left_fn && (right_hn == left_hn && (right_fn < root_fn || (right_fn == root_fn))))
                    {
                        swap(ref queue, i - 1, (i * 2));
                        i = (i * 2) + 1;
                        continue;
                    }

                    else
                    {
                        break;
                    }
                }


            }
            //right doesn't exist 
            if ((i * 2) + 1 > queue_length && i * 2 <= queue_length)
            {
                left = queue[(i * 2) - 1];
                //right = queue[(i * 2)];
                root = queue[i - 1];
                //----------------------------------------
                //-***************************************
                if (huristic)
                {
                    left_hn = left.hamming;
                    root_hn = root.hamming;
                }
                else
                {
                    left_hn = left.manhatten;
                    root_hn = root.manhatten;
                }
                //-------------------------------------
                left_fn = left.fn;
                //--------------------------------------
                root_fn = root.fn;
                //************************************************
                //-----------re-heaping from top to bottom------------------------------------------
                //if left < right && (left<root || (left==root && leftman<root man)
                if (left_fn < root_fn || (left_fn == root_fn && left_hn < root_hn))
                {
                    swap(ref queue, i - 1, (i * 2) - 1);
                    i = i * 2;
                    //continue;
                }
                else
                {
                    //break;
                }

            }
        }//end of function
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
       /* public void find_delete(node dest,bool huristic)
        {
            for (int i = 0; i < queue_length; i++)
            {
                if (queue[i].isequal(ref dest) && queue[i].fn >= dest.fn)
                {
                    swap(ref queue, i, queue_length - 1);
                    queue.RemoveAt(queue_length - 1);
                    queue_length--;
                    //--------------------------------------------end of helper data----
                    node left;
                    node right;
                    node root;
                    int left_hn;
                    int right_hn;
                    int root_hn;
                    int left_fn;
                    int right_fn;
                    int root_fn;
                    i++;
                    while ((i * 2) <= queue_length && (i * 2) + 1 <= queue_length)//till not reaching to end Hint :! note that i every time increment by i*2 or i*2+1
                    {
                        //left exist and right exist && left<right && left<root
                        if (i * 2 <= queue_length && (i * 2) + 1 <= queue_length)
                        {
                            left = queue[(i * 2) - 1];
                            right = queue[(i * 2)];
                            root = queue[i - 1];
                            //-----------------------------------
                            if (huristic)
                            {
                                left_hn = left.hamming;
                                right_hn = right.hamming;
                                root_hn = root.hamming;
                            }
                            else
                            {
                                left_hn = left.manhatten;
                                right_hn = right.manhatten;
                                root_hn = root.manhatten;
                            }

                            left_fn = left.fn;
                            //-------------------------------------
                            right_fn = right.fn;
                            //--------------------------------------
                            root_fn = root.fn;
                            //************************************************
                            //-----------re-heaping from top to bottom------------------------------------------
                            if (left_fn < right_fn && (left_fn < root_fn || (left_fn == root_fn && left_hn < root_hn)))
                            {
                                swap(ref queue, i - 1, (i * 2) - 1);
                                i = i * 2;
                                continue;
                            }
                             //right < left && (right < root || (right=root && right man <root man))
                            else if (right_fn < left_fn && (right_fn < root_fn || (right_fn == root_fn && right_hn < root_hn)))
                            {
                                swap(ref queue, i - 1, (i * 2));
                                i = (i * 2) + 1;
                                continue;
                            }
                               //left == right && left-manming < right-manming && (left< root || (left==root  && left-manming<root-manming)  
                            else if (left_fn == right_fn && (left_hn < right_hn && (left_fn < root_fn || (left_fn == root_fn && left_hn < root_hn))))
                            {
                                swap(ref queue, i - 1, (i * 2) - 1);
                                i = i * 2;
                                continue;
                            }
                            //right == left && right-manming < left-manming && (right< root || (right==root  && right-manming<root-manming) 
                            else if (right_fn == left_fn && (right_hn < left_hn && (right_fn < root_fn || (right_fn == root_fn && right_hn < root_hn))))
                            {
                                swap(ref queue, i - 1, (i * 2));
                                i = (i * 2) + 1;
                                continue;
                            }
                            else if (right_fn == left_fn && (right_hn == left_hn && (right_fn < root_fn || (right_fn == root_fn))))
                            {
                                swap(ref queue, i - 1, (i * 2));
                                i = (i * 2) + 1;
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }


                    }
                    //right doesn't exist 
                    if ((i * 2) + 1 > queue_length && i * 2 <= queue_length)
                    {
                        left = queue[(i * 2) - 1];
                        //right = queue[(i * 2)];
                        root = queue[i - 1];
                        //-----------------------------------
                        //-***************************************
                        if (huristic)
                        {
                            left_hn = left.hamming;
                            root_hn = root.hamming;
                        }
                        else
                        {
                            left_hn = left.manhatten;
                            root_hn = root.manhatten;
                        }
                        //-------------------------------------
                        left_fn = left.fn;
                        //--------------------------------------
                        root_fn = root.fn;
                        //************************************************
                        //-----------re-heaping from top to bottom------------------------------------------
                        //if left < right && (left<root || (left==root && leftman<root man)
                        if (left_fn < root_fn || (left_fn == root_fn && left_hn < root_hn))
                        {
                            swap(ref queue, i - 1, (i * 2) - 1);
                            i = i * 2;
                            //continue;
                        }
                        else
                        {
                            //break;
                        }

                    }
                }

            }
        }*/
        //**************************************************************************
    }
}
