using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using solve44;

namespace N_Puzzle_solver
{
    public partial class Form1 : Form
    {
        bool solver = false;
        bool loader = false;
        bool player= false;
        int numberofmoves;
        long pos = 0;
        file myfile;

        node start;
        node path;

        Stack<node> stepsstack = new Stack<node>();
        String puzzle_string = "";
      //  A_star A;

        ListBox stepslist;
        RadioButton manhattenradio;
        RadioButton hammingradio;

        Button load, solveb, play;

        int[,] _2D;
        int puzzle_size;
        int[] zeroplace;
        public int[] arr;

        public int tile_width = 100;
        public int tile_hight = 100;
        public int location_x = 200;
        public int temp_loc;
        public int location_y = 300;
        int size = 0;
        int zero = 0;
        public Label[] tiles;




        public Form1()
        {
            InitializeComponent();
            IntializeData();
            add_gui();
        }
        //-----------------------------
        public void IntializeData()
        {
            myfile = new file("test.txt");
            pos = myfile.read_from_specific_pos(pos);

            puzzle_size = myfile.get_puzzle_size();
            size = puzzle_size;

            _2D = new int[puzzle_size, puzzle_size];
            _2D = myfile.get_puzzle2D();

            zeroplace = new int[2];
            zeroplace = myfile.get_zero_place();
        
            for (int i = 0; i < myfile.get_puzzle_size(); i++)
            {
                for (int j = 0; j < myfile.get_puzzle_size(); j++)
                {
                    puzzle_string += _2D[i, j].ToString() + " ";
                }
            }
            //-----------------------------------------------------------------------------------------
            puzzle_string.Remove(myfile.get_puzzle_size() * myfile.get_puzzle_size());
            //-----------------------------------------------------------------------------------------
        }
        //-----------------------------
        public void add_gui()
        {
            int x, y;
            x = this.Width;
            y = this.Height;
          
            //********************************************
            stepslist = new ListBox();
            stepslist.Font=new Font("Arial",20);

            stepslist.Size = new System.Drawing.Size(x / 3, y-(y /3)+100);
            stepslist.Location = new System.Drawing.Point(x-(x/6),10);
            stepslist.Items.Add(" Steps of Moving ");
            stepslist.Items.Add("-------------------------------------------");
            this.Controls.Add(stepslist);
            //********************************************
            hammingradio = new RadioButton();
            hammingradio.Text = "Hamming";
            hammingradio.Font = new Font("Arial", 25);
            hammingradio.Location = new System.Drawing.Point(stepslist.Location.X, stepslist.Location.Y + stepslist.Height + 10);
            hammingradio.Size = new System.Drawing.Size(stepslist.Width / 2, 50);
            this.Controls.Add(hammingradio);
            //********************************************
            //********************************************
            manhattenradio = new RadioButton();
            manhattenradio.Text = "Manhatten";
            manhattenradio.Font = new Font("Arial", 25);
            manhattenradio.Location = new System.Drawing.Point(stepslist.Location.X+stepslist.Width/2+5, stepslist.Location.Y + stepslist.Height + 10);
            manhattenradio.Size = new System.Drawing.Size(200, 50);
            this.Controls.Add(manhattenradio);
            //********************************************
            load = new Button();
            load.Text = "Load";
            load.BackColor = Color.LightGray;
            load.Font = new Font("Arial", 15);
            load.TextAlign = ContentAlignment.MiddleCenter;
            load.Location = new System.Drawing.Point(stepslist.Location.X, hammingradio.Location.Y+hammingradio.Height+20);
            load.Size = new System.Drawing.Size(120, 30);
             load.Click += new System.EventHandler(this.load_Click);
            this.Controls.Add(load);
            //********************************************
            solveb = new Button();
            solveb.Text = "Solve";
            solveb.BackColor = Color.LightGray;
            solveb.Font = new Font("Arial", 15);
            solveb.TextAlign = ContentAlignment.MiddleCenter;
            solveb.Location = new System.Drawing.Point(stepslist.Location.X+load.Width+10, hammingradio.Location.Y + hammingradio.Height + 20);
            solveb.Size = new System.Drawing.Size(120, 30);
            solveb.Click += new System.EventHandler(this.solve_Click);
            this.Controls.Add(solveb);
            //********************************************
            play = new Button();
            play.Text = "Play Result";
            play.BackColor = Color.LightGray;
            play.Font = new Font("Arial", 15);
            play.TextAlign = ContentAlignment.MiddleCenter;
            play.Location = new System.Drawing.Point(solveb.Location.X + solveb.Width+10, hammingradio.Location.Y + hammingradio.Height + 20);
            play.Size = new System.Drawing.Size(140, 30);
            play.Click += new System.EventHandler(this.play_Click);
            this.Controls.Add(play);
            //********************************************
            manhattenradio.Checked = true;
        }
        //-----------------------------
        private void load_Click(object sender, EventArgs e)
        {
            if (!loader)
            {
                loader = true;
                creat_puzzle();
            }
        }
        //****************************************************
        public void creat_puzzle()
        {
            tiles = new Label[puzzle_size * puzzle_size];

            location_x = ((this.Width / 2) / (puzzle_size + 2))*2;
            location_y = ((this.Height / 2) / (puzzle_size + 2))*2;
            tile_width = location_x/2;
            tile_hight = location_x/2;
            
            temp_loc = location_x;
            int c = 0;

            for (int i = 0; i < puzzle_size; i++)
            {
                for (int j = 0; j < puzzle_size; j++)
                {
                    if (_2D[i, j] != 0)
                    {
                        tiles[c] = new Label();
                        tiles[c].Location = new System.Drawing.Point(location_x, location_y);
                        tiles[c].Size = new System.Drawing.Size(tile_width, tile_hight);
                        tiles[c].TabStop = false;
                        tiles[c].BackColor = Color.White;                   
                        tiles[c].Text = (_2D[i, j]).ToString();
                        tiles[c].Font = new Font("Arial", 100/(puzzle_size));
                        tiles[c].TextAlign = ContentAlignment.MiddleCenter;
                        this.Controls.Add(tiles[c]);
                        c++;
                    }
                    else
                    {
                        zero = c;
                        tiles[c] = new Label();
                        tiles[c].Location = new System.Drawing.Point(0, 0);
                        tiles[c].Size = new System.Drawing.Size(tile_width, tile_hight);
                        tiles[c].TabStop = false;
                        tiles[c].BackColor = this.BackColor;
                        c++;
                    }

                    location_x += tile_width + 5;
                }
                location_x = temp_loc;
                location_y += tile_hight + 5;
            }

            arr = new int[puzzle_size * puzzle_size];
            for (int i = 0; i < puzzle_size * puzzle_size; i++)
            {
                arr[i] = i;
            }

        }
        //*****************************************************
        private void solve_Click(object sender, EventArgs e)
        {

            if(!(solver) && loader)
            {
                solver = true;
               // this.Enabled = false;
               // new Task(Astar_solving).Start();
                solveb.Enabled = false;
                play.Enabled = false;
                load.Enabled = false;
                Astar_solving();
                play.Enabled = true;
                
            }
            else
            {
                MessageBox.Show("Check All Data Befor Running");
            }
            
        }
        //***************************************************
        public void Astar_solving()
        {
            if (detect.detect_puzzle(_2D, puzzle_size, zeroplace[0]))
            {
                A_star A = new A_star();
                start = new node(_2D, puzzle_size, 0, zeroplace, puzzle_string, false);
                path = new node();
                if (manhattenradio.Enabled)
                {
                    path = A.solve(start, false);
                }
                else
                {
                    path = A.solve(start, true);
                }
                numberofmoves = path.moves;
                stepslist.Items.Clear();
                stepslist.Items.Add("Number Of Moves = " + numberofmoves.ToString());
                while (path.parent != null)
                {
                    stepsstack.Push(path);
                    path = path.parent;
                }
                stepsstack.Push(start);
                //*******************************************
                int s = stepsstack.Count;
                String steps = "";
                int x, y;
                for (int i = 1; i < s; i++)
                {
                    if (stepsstack.ElementAt(i).node_direction == "right")
                    {
                        x = stepsstack.ElementAt(i).zero_placeXY[0];
                        y = stepsstack.ElementAt(i).zero_placeXY[1];
                        steps = (i).ToString() + "- Move " + stepsstack.ElementAt(i).puzzle2D[x, y - 1].ToString() + " left";
                        stepslist.Items.Add(steps);
                    }
                    else if (stepsstack.ElementAt(i).node_direction == "left")
                    {
                        x = stepsstack.ElementAt(i).zero_placeXY[0];
                        y = stepsstack.ElementAt(i).zero_placeXY[1];
                        steps = (i).ToString() + "- Move " + stepsstack.ElementAt(i).puzzle2D[x, y + 1].ToString() + " right";
                        stepslist.Items.Add(steps);
                        //----------------------------------------------------
                    }
                    else if (stepsstack.ElementAt(i).node_direction == "up")
                    {
                        x = stepsstack.ElementAt(i).zero_placeXY[0];
                        y = stepsstack.ElementAt(i).zero_placeXY[1];
                        steps = (i).ToString() + "- Move " + stepsstack.ElementAt(i).puzzle2D[x + 1, y].ToString() + " Down";
                        stepslist.Items.Add(steps);
                        //----------------------------------------------------
                    }
                    else if (stepsstack.ElementAt(i).node_direction == "down")
                    {
                        x = stepsstack.ElementAt(i).zero_placeXY[0];
                        y = stepsstack.ElementAt(i).zero_placeXY[1];
                        steps = (i).ToString() + "- Move " + stepsstack.ElementAt(i).puzzle2D[x - 1, y].ToString() + " Up";
                        stepslist.Items.Add(steps);
                        //------------------------------------------
                    }
                }
            }
            else
            {
                MessageBox.Show("Unsolvable");
            }
            this.Enabled = true;
        }
        //***************************************************
        public void solve()
        {
            //String steps = "";
            int z_down = 0;
            int z_right = 0;
            int z_left = 0;
            int z_up = 0;
            if (size * size > zero + 1)
            {
                z_right = arr[zero + 1];
            }
            if (zero - 1 >= 0)
            {
                z_left = arr[zero - 1];
            }
            if (zero - size >= 0)
            {
                z_up = arr[zero - size];
            }
            if (arr.Length - 1 >= zero + size)
            {
                z_down = arr[zero + size];
            }
            stepsstack.Pop();
            //var t = Task.Delay(1000);//1 second/1000 ms
            for (int i = 0; i < stepsstack.Count; i++)
            {

                //------------------------------------------------------------------------
                if (stepsstack.ElementAt(i).node_direction == "right")
                {
                  //  steps = (i + 1).ToString() + "- Move " + tiles[z_right].Text + " left";
                    //stepslist.Items.Add(steps);
                    tiles[z_right].Location = new Point(tiles[z_right].Location.X - (tile_width + 5), tiles[z_right].Location.Y);
                    //------------------------------------------------
                    int tempo = arr[zero];
                    arr[zero] = arr[zero + 1];
                    arr[zero + 1] = tempo;
                    zero++;
                    if (size * size > zero + 1)
                    {
                        z_right = arr[zero + 1];
                    }
                    if (zero - 1 >= 0)
                    {
                        z_left = arr[zero - 1];
                    }
                    if (zero - size >= 0)
                    {
                        z_up = arr[zero - size];
                    }
                    if (arr.Length - 1 >= zero + size)
                    {
                        z_down = arr[zero + size];
                    }
                    // zero = arr[zero];
                    //----------------------------------------------------
                }
                else if (stepsstack.ElementAt(i).node_direction == "left")
                {
                  //  steps = (i + 1).ToString() + "- Move " + tiles[z_left].Text + " right";
                  //  stepslist.Items.Add(steps);
                    tiles[z_left].Location = new Point(tiles[z_left].Location.X + (tile_width + 5), tiles[z_left].Location.Y);
                    //-----------------------------------------------------
                    int tempo = arr[zero];
                    arr[zero] = arr[zero - 1];
                    arr[zero - 1] = tempo;

                    zero--;

                    if (size * size > zero + 1)
                    {
                        z_right = arr[zero + 1];
                    }
                    if (zero - 1 >= 0)
                    {
                        z_left = arr[zero - 1];
                    }
                    if (zero - size >= 0)
                    {
                        z_up = arr[zero - size];
                    }
                    if (arr.Length - 1 >= zero + size)
                    {
                        z_down = arr[zero + size];
                    }
                    //----------------------------------------------------
                }
                else if (stepsstack.ElementAt(i).node_direction == "up")
                {
                   // steps = (i + 1).ToString() + "- Move " + tiles[z_up].Text + " Down";
                   // stepslist.Items.Add(steps);
                    tiles[z_up].Location = new Point(tiles[z_up].Location.X, tiles[z_up].Location.Y + ((tile_hight + 5)));
                    int tempo = arr[zero];
                    arr[zero] = arr[zero - size];
                    arr[zero - size] = tempo;
                    zero -= size;
                    if (size * size > zero + 1)
                    {
                        z_right = arr[zero + 1];
                    }
                    if (zero - 1 >= 0)
                    {
                        z_left = arr[zero - 1];
                    }
                    if (zero - size >= 0)
                    {
                        z_up = arr[zero - size];
                    }
                    if (arr.Length - 1 >= zero + size)
                    {
                        z_down = arr[zero + size];
                    }
                    //----------------------------------------------------
                }
                else if (stepsstack.ElementAt(i).node_direction == "down")
                {
                  //  steps = (i + 1).ToString() + "- Move " + tiles[z_down].Text + " Up";
                   // stepslist.Items.Add(steps);
                    tiles[z_down].Location = new Point(tiles[z_down].Location.X, tiles[z_down].Location.Y - ((tile_hight + 5)));
                    int tempo = arr[zero];
                    arr[zero] = arr[zero + size];
                    arr[zero + size] = tempo;
                    zero += size;

                    if (size * size > zero + 1)
                    {
                        z_right = arr[zero + 1];
                    }
                    if (zero - 1 >= 0)
                    {
                        z_left = arr[zero - 1];
                    }
                    if (zero - size >= 0)
                    {
                        z_up = arr[zero - size];
                    }
                    if (arr.Length - 1 >= zero + size)
                    {
                        z_down = arr[zero + size];
                    }
                    //------------------------------------------
                    //  zero = arr[zero];

                }
                var t = Task.Delay(1000);
                t.Wait();
            }

        }
        //*****************************************************
        private void play_Click(object sender, EventArgs e)
        {
            if (!(player)&&solver)
            {
                player = true;
                solveb.Enabled = false;
                play.Enabled = false;
                load.Enabled = false;
                solve();
            }
                
            else
                MessageBox.Show("Check All Data Befor Running");
        }
    }
}
