using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APM_Monitor
{
    public partial class Form1 : Form
    {



      
        public static int[] click_counte = new int[100];
        public static int[] key_counter = new int[100];
      public  static bool running = false;
       public static Stopwatch stopwatch;
        Thread starter,draw;
        static int limit = 100;


      



        public Form1()
        {
            InitializeComponent();
            Shown += Form1_Shown;
            this.FormClosing += Form_FormClosing;
          
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            //    Start();
        }
        private void Form1_Shown(object sender, EventArgs e)
        {

            chart1.Series.Clear();


            Keyboard.start();
            Mouse.start();
            //      Subscribe();
            starter = new Thread(Start);
            starter.Start();
            draw = new Thread(Draw);
            draw.Start();
        }

        void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            starter.Abort();
            draw.Abort();

            if (e.CloseReason == CloseReason.UserClosing) { }
             
                if (e.CloseReason == CloseReason.WindowsShutDown) { }
  
        }

        public void Start()
        {
            MethodInvoker inv = delegate
            {
                label14.Text = "Wait for new Game";
            };
            this.Invoke(inv);

        
            bool found = false;
            while (!found)
            {
                Process[] pname = Process.GetProcessesByName("League of Legends");
                if (pname.Length == 0)

                {
                    Thread.Sleep(5000);
                }
                else
                { found = true; }
            }
            Thread.Sleep(30000);
            MethodInvoker inv2 = delegate
            {
                label14.Text = "recording";
            };
            this.Invoke(inv2);

            stopwatch  = new Stopwatch();
            stopwatch.Start();
            running = true;
         

  
            while (found)
            {
                Process[] pname = Process.GetProcessesByName("League of Legends");
                if (pname.Length == 0)

                { found = false; }
                else
                { Thread.Sleep(10000); }
            }
            MethodInvoker inv3 = delegate
            {
                label14.Text = "Wait for new Game";
            };
            this.Invoke(inv3);
            Stop();

        }

        public void Stop() {




            Display();

        

            stopwatch.Stop();
            running = false;

            click_counte = new int[100];
           key_counter = new int[100];
            limit = 100;


            Start();
        }
        public void Display() {
            //todo
            limit = (int)(stopwatch.ElapsedMilliseconds / 60000);
            MethodInvoker inv4 = delegate
            {
          
            TimeSpan ts = stopwatch.Elapsed;
            label2.Text = ts.ToString(@"m\:ss");
            int clicksTotal = 0;
            int keysTotal = 0;



            chart1.Series.Clear();
               

                chart1.Series.Add("APM");
            chart1.Series.Add("CPM");
            chart1.Series.Add("KPM");


                chart1.ChartAreas[0].AxisX.Maximum = limit;

                chart1.ChartAreas[0].AxisX.Minimum = 0;
                for (int i = 0; i <= limit; i++)
            {
                clicksTotal += click_counte[i];
                keysTotal += key_counter[i];
                chart1.Series["APM"].Points.AddXY(i, click_counte[i] + key_counter[i]);
                chart1.Series["CPM"].Points.AddXY(i, click_counte[i] );
                chart1.Series["KPM"].Points.AddXY(i,  key_counter[i]);
            }

            chart1.Series["CPM"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series["KPM"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series["APM"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            label3.Text = clicksTotal.ToString();
            label5.Text = keysTotal.ToString();

            double apm = (double)(clicksTotal + keysTotal) * 60 / (double)(stopwatch.ElapsedMilliseconds / 1000);
            double cpm = (double)(clicksTotal ) * 60 / (double)(stopwatch.ElapsedMilliseconds / 1000);
            double kpm = (double)(keysTotal) * 60 / (double)(stopwatch.ElapsedMilliseconds / 1000);
            label8.Text = apm.ToString("F");
            label9.Text = cpm.ToString("F");
            label11.Text = kpm.ToString("F");

              
            };
            this.Invoke(inv4);


        }
 
    
        public void Draw()
        {
           
            if(running)    Display();
            Thread.Sleep(60000);
            Draw();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            if (savefile.ShowDialog() == DialogResult.OK)
            {
               chart1.SaveImage(savefile.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
            }
   
        }
    }
}
