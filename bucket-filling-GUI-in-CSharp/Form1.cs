using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5a
{
   
    public partial class Form1 : Form
    {
        private Color color; //store brush color 
        private int counter; //tracking variable that indicates how much water should be poured in bucket
        private bool stopped; //indicates if Faucet is stopped 
        private bool filled; //indicates if bucket is filled
        int[] speedTracker = { 0,180, 160, 140, 120, 100, 80, 60, 40, 20, 10 }; // array containing water falling speed for different levels on trackbar
        private Graphics graphics;

        public Form1()
        {
            // color stored as light blue by default
            color = Color.LightBlue;

            // counter set to 1 as the bucket doesn't contain water
            counter = 1;

            //initially faucet is stopped
            stopped = true;

            //bucket is not filled
            filled = false; 

            InitializeComponent();

            // Keeps declaring Paint object and passes "paintingForm" as method argument
            Paint += new PaintEventHandler(paintForm);

            // set to 0 initially
            trackBar1.Value = 0;
        }
        
        // Method that invoked by Paint Class which draws lines of the bucket
        private void paintForm(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.White), 100, 290, 100, 400);
            e.Graphics.DrawLine(new Pen(Color.White), 100, 400, 300, 400);
            e.Graphics.DrawLine(new Pen(Color.White), 300, 400, 300, 290);
        }

        //show color dialog when color button is clicked
        private void colorButon_click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                color = colorDialog1.Color;
        }
        
        //close program when close button is clicked
        private void close_Program(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        // method invoked evrytime the trackbar is scrolled in the program
        private void TrackBarScroll(object sender, EventArgs e)
        {
            // create Graphics object
            graphics = CreateGraphics();

            // if bucket if filled then clears bucket then sets bool filled to false
            if (filled)
            {
                graphics.FillRectangle(new System.Drawing.SolidBrush(BackColor), 101, 200, 199, 200);
                filled = false;
            }

            // start timer and set stopped to false if Faucet is stopped
            if (stopped)
            {
                timer1.Start();
                stopped = false;
            }

            // setting interval from the speedTrackar array using trackbar value as index if the trackbar value is greater than 0 
            if (trackBar1.Value > 0)
            {
                timer1.Interval = speedTracker[trackBar1.Value];
            }

            // stop Faucet if trackbar value is 0
            if (trackBar1.Value == 0)
            {
                timer1.Stop();
                stopped = true;
                graphics.FillRectangle(new SolidBrush(BackColor), 110, 200, 15, 200 - counter + 1);
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            graphics = CreateGraphics();

            // genreate rectangle using FillRectangle using brush object as first argument, and coordinates as second argument
            graphics.FillRectangle(new SolidBrush(color), 101, 400 - counter, 199, 1);
            graphics.FillRectangle(new SolidBrush(color), 110, 200, 15, 200 - counter + 1);
            counter += 1;

            // if bucket is filled
            if ((400 - counter) == 295)
            {
                //set filled and stopped to true since bucket is filled
                filled = true;
                stopped = true;

                //stop timer
                timer1.Stop();

                //no need to fill water anymore so set counter to 0
                counter = 0;

                //set trackbar to initial value
                trackBar1.Value = 0;

                // create black rectangle to make future rectangle visible
                graphics.FillRectangle(new SolidBrush(BackColor), 110, 200, 15, 96);
            }
        }
    }
}

