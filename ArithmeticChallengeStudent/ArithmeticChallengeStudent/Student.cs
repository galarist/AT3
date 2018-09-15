/*
 *      Student Number: 451381461
 *      Name:           Mitchell Stone
 *      Date:           14/09/2018
 *      Purpose:        All functions to run the logic for the student window
 *      Known Bugs:     nill
 */

using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ArithmeticChallengeStudent
{
    public partial class Student : Form
    {
        //create a client socket to store socket data
        private static Socket clientSocket;

        //create a buffer to store the messages
        private byte[] buffer;

        //open on port 3333
        private int PORT = 3333;

        //IP address only works for local computer
        private string LocalIP = "127.0.0.1";

        string message = null;

        static Equations equation;

        public Student()
        {
            InitializeComponent();
            btn_submit.Enabled = false;

            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //connect to the specified host.
                var endPoint = new IPEndPoint(IPAddress.Parse(LocalIP), PORT);
                clientSocket.BeginConnect(endPoint, ConnectCallback, null);
            }
            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);
            }
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                int received = clientSocket.EndReceive(AR);
                if (received == 0)
                {
                    return;
                }

                message = Encoding.ASCII.GetString(buffer);

                int index = message.IndexOf("}");
                message = message.Substring(0, index + 1);

                //Check for intial connection reply
                if (message.Contains("server_connection"))
                {
                    //ServerMessages serverMessage = JsonConvert.DeserializeObject<ServerMessages>(message);
                   // Console.WriteLine("Message from the server: " + serverMessage.Message);
                    buffer = new byte[clientSocket.ReceiveBufferSize];
                }
                else if (DeserializeJson(message) != null)
                {
                    //deserialize json string into an object
                    Console.WriteLine(message);
                    equation = DeserializeJson(message);

                    Invoke((Action)delegate
                    {
                        //update the question text box to show the equation
                        tb_question.Text = equation.FirstNumber.ToString() + equation.Symbol + equation.SecondNumber.ToString() + "=";
                        btn_submit.Enabled = true;
                    });
                    buffer = new byte[clientSocket.ReceiveBufferSize];
                }

                // Start receiving data again.
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            // Avoid catching all exceptions handling in cases like these.
            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);
            }
        }

        private void ConnectCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket.EndConnect(AR);
                buffer = new byte[clientSocket.ReceiveBufferSize];
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);
            }
        }

        private void SendCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket.EndSend(AR);
            }
            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);
            }
        }

        private Equations DeserializeJson(string json)
        {
            //basic function to deserialise a json string
            try
            {
                Equations eq = JsonConvert.DeserializeObject<Equations>(json);
                return eq;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        private void btn_submit_Click(object sender, EventArgs e)
        {
            if (tb_answer.Text == equation.Result.ToString())
            {
                //submit correct answer
                MessageBox.Show("That is correct!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                equation.IsCorrect = true;
                string json = JsonConvert.SerializeObject(equation);
                SendMessage(json);
            }
            else
            {
                //submit incorrect answer
                MessageBox.Show("That is incorrect!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                equation.IsCorrect = false;
                string json = JsonConvert.SerializeObject(equation);
                SendMessage(json);
            }
            //toggle button and clear text boxes
            btn_submit.Enabled = false;
            tb_answer.Clear();
            tb_question.Clear();
        }

        private void SendMessage(string json)
        {
            //function that sends the reply to the server
            var sendData = Encoding.ASCII.GetBytes(json);
            clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, null);
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
