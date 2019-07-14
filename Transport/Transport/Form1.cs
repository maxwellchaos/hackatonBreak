using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Transport
{
    public partial class Form1 : Form
    {
        int typebt = 0;
        bool bt = false, roadbt = false;
        int temp = 1, temp2 = 1, id = 0, x1 = 0, y1 = 0, x2 = 0, y2 = 0;
        RegionGraph rg = new RegionGraph();
        PictureBox road;
        Bitmap car = new Bitmap(Application.StartupPath + "\\car.png");
        Bitmap Mainzn = new Bitmap(Application.StartupPath + "\\mainroad.jpg");
        Bitmap prk = new Bitmap(Application.StartupPath + "\\prk.jpg");
        Bitmap exit = new Bitmap(Application.StartupPath + "\\exit.jpg");
        


        public Form1()
        {
            InitializeComponent();
        }
        
        private void MouseMovePic(PictureBox home)
        {
            home.Image = Image.FromFile(Application.StartupPath + "\\HomeBigTop.jpg");
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            do
            {
                PictureBox home = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.AutoSize
                };
                Controls.Add(home);
                temp++;
                if (temp > 3)
                {
                    if(temp2 == 2)
                    {
                        home.Image = Image.FromFile(Application.StartupPath + "\\Frame-1.jpg");
                        home.Left = 280;
                        home.Top = 280;
                    }
                    else
                    {
                        home.Image = Image.FromFile(Application.StartupPath + "\\Frame-1.jpg");
                        home.Left = 40;
                        home.Top = 280;
                    }
                    temp2++;
                }
                else
                {
                    if (temp == 2)
                    {
                        home.Image = Image.FromFile(Application.StartupPath + "\\Frame.jpg");
                        home.Left = 280;
                        home.Top = 50;
                    }
                    else
                    {
                        home.Image = Image.FromFile(Application.StartupPath + "\\Frame.jpg");
                        home.Left = 40;
                        home.Top = 50;
                    }
                }
                i++;
            }
            while (i != 4);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            bt = true;
            typebt = 2;
            this.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            this.DoubleBuffered = true;
            for (int i = 0; i < rg.Nodes.Count; i++)
            {
                for (int j = 0; j < rg.Nodes[i].outRoads.Count; j++)
                {
                    Pen pen = new Pen(Color.Gray, 10);
                    e.Graphics.DrawLine(pen, rg.Nodes[rg.Nodes[i].outRoads[j].inNode].x + 25, rg.Nodes[rg.Nodes[i].outRoads[j].inNode].y + 30, rg.Nodes[rg.Nodes[i].outRoads[j].outNode].x + 25, rg.Nodes[rg.Nodes[i].outRoads[j].outNode].y + 30);
                }
            }
            for (int i = 0; i < rg.Nodes.Count; i++)
            {
                if(rg.Nodes[i].NodeType == 2)
                {
                    e.Graphics.DrawImage(Mainzn, rg.Nodes[i].x, rg.Nodes[i].y);
                    
                }
                if (rg.Nodes[i].NodeType == 3)
                {
                    e.Graphics.DrawImage(exit, rg.Nodes[i].x, rg.Nodes[i].y);
                }
                if (rg.Nodes[i].NodeType == 1)
                {
                    e.Graphics.DrawImage(prk, rg.Nodes[i].x, rg.Nodes[i].y);
                }

            }
           
            if(roadbt == false)
            {
                int prov1 = -2;
                int prov2 = -2;
                for(int i = 0; i < rg.Nodes.Count; i++)
                {
                    if(x1 <= rg.Nodes[i].x + 50 && x1 >= rg.Nodes[i].x - 50)
                    {
                        if(y1 <= rg.Nodes[i].y + 50 && y1 >= rg.Nodes[i].y - 50)
                        {
                            prov1 = i;
                        }
                    }
                    if (x2 <= rg.Nodes[i].x + 50 && x2 >= rg.Nodes[i].x - 50)
                    {
                        if (y2 <= rg.Nodes[i].y + 50 && y2 >= rg.Nodes[i].y - 50)
                        {
                            prov2 = i;
                        }
                    }
                    if(prov1 >= 0 && prov2 >= 0)
                    {
                        e.Graphics.DrawLine(Pens.Black, x1, y1, x2, y2);
                        rg.AddRoad(prov1, prov2);
                        prov1 = 0;
                        prov2 = 0;
                        break;
                    }
                }
            }
            if (timer1.Enabled)
            {
                foreach (Auto a in rg.autos)
                {
                    int deltaX = (rg.Nodes[a.rr.inNode].x - rg.Nodes[a.rr.outNode].x) / a.rr.Length;
                    int deltaY = (rg.Nodes[a.rr.inNode].y - rg.Nodes[a.rr.outNode].y) / a.rr.Length;
                    int autoX = rg.Nodes[a.rr.inNode].x - deltaX * a.Point;
                    int autoY = rg.Nodes[a.rr.inNode].y - deltaY * a.Point;
                    e.Graphics.DrawImage(car, autoX, autoY);

                }
            }
        }

        private void Button7_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (roadbt == true)
            {
                x2 = e.X;
                y2 = e.Y;
            }
            roadbt = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            rg.GenerateAuto();
            rg.moveAllAuto();
            this.Invalidate();
            label1.Text = (rg.sumTime / rg.countAuto).ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            car.MakeTransparent(Color.White);
            Mainzn.MakeTransparent(Color.White);
            prk.MakeTransparent(Color.White);
            exit.MakeTransparent(Color.White);
        }
        
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if(bt == true)
            {
                rg.AddNode(id, typebt, e.X, e.Y);
                id++;
                bt = false;
            }
            if(roadbt == true)
            {
                x1 = e.X;
                y1 = e.Y;
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            bt = true;
            typebt = 1;
            this.Invalidate();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Invalidate();
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            roadbt = true;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            bt = true;
            typebt = 3;
            this.Invalidate();
        }
    }
}
