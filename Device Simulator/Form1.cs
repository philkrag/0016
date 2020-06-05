using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Device_Simulator
{
    public partial class Form1 : Form
    {

        string Incoming_Data = "";


        static int M090D010;
        static int M270D010;
        

        int M090D010_Rotor = 0;
        int M270D010_Rotor = 0;
        



        static int Y_Axis_Movement;
        static int Z_Axis_Movement;




        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }
















        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string Incoming_String;

            var data = new byte[7000];
            IPEndPoint ServerEndPoint = new IPEndPoint(IPAddress.Any, 9051);
            Socket WinSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            WinSocket.Bind(ServerEndPoint);

            do
            {
                //Console.WriteLine("Waiting for client...");
                IPEndPoint sender1 = new IPEndPoint(IPAddress.Any, 0);
                EndPoint Remote = (EndPoint)(sender1);
                int recv = WinSocket.ReceiveFrom(data, ref Remote);
                //Console.Write("Message received from {0}: ", Remote.ToString());

                Incoming_String = Encoding.ASCII.GetString(data, 0, recv);
                Console.WriteLine(Incoming_String);

                Incoming_Data =  Incoming_String + Environment.NewLine + Incoming_Data;

                //listBox1.Items.Add(Incoming_String);

                //Test_Procedure(Incoming_String);

                Test_Procedure(Incoming_String);


            } while (true);

        }


        static void Test_Procedure(string Incoming_String)
        {

            if (Incoming_String.Contains(";"))
            {
                string[] Incoming_String_Array = Incoming_String.Split(';');

                for (int i = 0; i < Incoming_String_Array.Length; i++)
                {

                    string[] Item_Split = Incoming_String_Array[i].Split(':');

                    switch (Item_Split[0])
                    {
                        case "M090D010":
                            M090D010 = Convert.ToInt32(Item_Split[1]);
                            break;

                        case "M270D010":
                            M270D010 = Convert.ToInt32(Item_Split[1]);
                            break;

                            
                        


                        //case "X-AXIS_MOV":
                        //    X_Axis_Movement = Convert.ToInt32(Item_Split[1]);
                        //    break;


                        case "Y-AXIS_MOV":
                            Y_Axis_Movement = Convert.ToInt32(Item_Split[1]);
                            break;
                        case "Z-AXIS_MOV":
                            Z_Axis_Movement = Convert.ToInt32(Item_Split[1]);
                            break;
                        default:

                            break;
                    }

                }

                //Send_UDP("X-AXIS_MOV:" + X_Axis_Movement + ";Y-AXIS_MOV:" + Y_Axis_Movement + ";Z-AXIS_MOV:" + Z_Axis_Movement + ";");
            }
            else
            {
                Console.WriteLine(Incoming_String);

            }

        }











        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = Incoming_Data;
            //label1.Text = M270D010.ToString();
            label2.Text = M090D010_Rotor.ToString();
            label3.Text = M270D010_Rotor.ToString();

            if (M090D010 == 0) { timer3.Enabled = false; } else { timer3.Enabled = true; timer3.Interval = Invert_Rotor_Widget(M090D010); }
            if (M270D010 == 0) { timer2.Enabled = false; } else { timer2.Enabled = true; timer2.Interval = Invert_Rotor_Widget(M270D010); }



        }



        private int Invert_Rotor_Widget(int Set)
        {
            int Return_Value = 0;
            int Multiplier = 4;
            int Protocol_Maximum_Spread = 100; // 
            int Protocol_Functional_Range = (Protocol_Maximum_Spread / 2);
            int Interved_Value = 0;
            if (Set == Protocol_Functional_Range) { Set = Protocol_Functional_Range - 1; }
            if (Set == (Protocol_Functional_Range*-1)) { Set = Protocol_Functional_Range + 1; }
            if (Set > 0){Interved_Value = Protocol_Functional_Range - Set;}
            else{Interved_Value = Protocol_Functional_Range + Set;}
            Return_Value = Interved_Value * Multiplier;
            return Return_Value;
        }




        










    int Rotor_1 = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {

            //M090D010_Rotor = Update_Screen_Components(M090D010, M090D010_Rotor, timer3, pictureBox4, pictureBox6, pictureBox5);
            M270D010_Rotor = Update_Screen_Components(M270D010, M270D010_Rotor, timer2, pictureBox1, pictureBox2, pictureBox3);
            
        }


        private void timer3_Tick(object sender, EventArgs e)
        {
            M090D010_Rotor = Update_Screen_Components(M090D010, M090D010_Rotor, timer3, pictureBox4, pictureBox6, pictureBox5);
        }












        private int Update_Screen_Components(int Input, int Rotor, Timer Timer, PictureBox Upper, PictureBox Middle, PictureBox Lower)
        {
            if (Rotor<0) { Rotor = 0; }

            switch (Rotor)
            {
                case 0:
                    Upper.BackColor = Color.White;
                    Middle.BackColor = Color.White;
                    Lower.BackColor = Color.White;
                    if (Input > 0) { Rotor = 10; }
                    break;

                case 1:
                    Upper.BackColor = Color.White;
                    Middle.BackColor = Color.White;
                    Lower.BackColor = Color.Black;
                    break;

                case 2:
                    Upper.BackColor = Color.White;
                    Middle.BackColor = Color.Black;
                    Lower.BackColor = Color.White;
                    break;

                case 3:
                    Upper.BackColor = Color.Black;
                    Middle.BackColor = Color.White;
                    Lower.BackColor = Color.White;
                    break;

                case 4:
                    Upper.BackColor = Color.White;
                    Middle.BackColor = Color.White;
                    Lower.BackColor = Color.White;
                    break;

                case 10:
                    if (Input < 0) { Rotor = 0; }
                    break;

                default:
                    break;
            }

            if (Input < 0)
            {
                Rotor++;
            }
            else
            {
                Rotor--;
            }
            return Rotor;
        }

        
    }
}
