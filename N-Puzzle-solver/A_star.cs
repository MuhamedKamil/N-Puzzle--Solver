using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace solve44
{
    class A_star
    {
        priority_queue queue = new priority_queue();

        SortedDictionary<String, node> close_open_set = new SortedDictionary<String, node>();
        SortedDictionary<String, node> to_del_set = new SortedDictionary<String, node>();

        List<node> ll = new List<node>();
        //-------------------------------------------------------------------------------------
        public node solve(node start,bool huristic)//true for hamming , false manhatten
        {
            queue.add_to_priority_queue(start,huristic);
            node new_node = new node(start);
            close_open_set.Add(new_node.puzzle_string, new_node);
            return find_goal(huristic);
        }
        //---------------------------------------------------------------------------------------
        public node find_goal(bool huristic)
        {
            node current_node= new node();

            while (queue.queue_length > 0)
            {
                current_node = queue.remove_from_priority_queue(huristic);

                if(huristic)
                {
                     while (to_del_set.ContainsKey(current_node.puzzle_string) && to_del_set[current_node.puzzle_string].hamming==current_node.hamming)
                     {
                         to_del_set.Remove(current_node.puzzle_string);
                         current_node = queue.remove_from_priority_queue(huristic);
                     }
                }
                else
                {
                    while (to_del_set.ContainsKey(current_node.puzzle_string) && to_del_set[current_node.puzzle_string].manhatten == current_node.manhatten)
                    {
                        to_del_set.Remove(current_node.puzzle_string);
                        current_node = queue.remove_from_priority_queue(huristic);                       
                    }
                }
               

                if (huristic && current_node.hamming == 0)
                {
                    return current_node;
                }
                else if (!(huristic) && current_node.manhatten == 0 )
                {
                    return current_node;
                }

                current_node.nighbors(ref close_open_set, ref queue,huristic, ref to_del_set);

                close_open_set[current_node.puzzle_string].flag_open_or_closed = false;

            }
            return current_node;
        }
    }

}
