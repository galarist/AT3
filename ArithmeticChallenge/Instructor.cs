/*
 *      Student Number: 451381461
 *      Name:           Mitchell Stone
 *      Date:           14/09/2018
 *      Purpose:        All functions to run the logic for the instructor form
 *      Known Bugs:     nill
 */

using ArithmeticChallenge.Controllers;
using ArithmeticChallenge.NodeFunctions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ArithmeticChallenge
{
    public partial class Instructor : Form
    {
        private Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private Socket clientSocket;
        private int PORT = 3333;
        private byte[] buffer;

        BinaryTree tree = new BinaryTree();

        //list of all current equations to display in the data grid view
        List<Equations> equations = new List<Equations>();

        //an equation object
        Equations equation;

        //A node list of uquation objects
        LinkListNodeList equationNodeList = new LinkListNodeList();

        //symbols used in the dropdown to select for calculationss
        string[] operators = { "+", "-", "x", "/" };

        public Instructor()
        {
            InitializeComponent();

            //set the data source for the data grid view to the equations list
            comboBoxOperators.DataSource = operators;

            //put the columns into the data grid view
            LoadQuestionsDataGridView();

            StartServer();
        }

        private void StartServer()
        {
            try
            {
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT)); //Binds on the specified port
                serverSocket.Listen(10); // listening on a backlog of ten pending connections
                serverSocket.BeginAccept(AcceptCallback, null); // start accepting incoming 
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

        private void AcceptCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket = serverSocket.EndAccept(AR); // set up the clientsocket
                buffer = new byte[clientSocket.ReceiveBufferSize]; // intialise the buffer to the correct buffer size
                Invoke((Action)delegate
                {
                    //lbl_clientCount.Text = "You have 1 client connected";
                });
                // Send a message to the newly connected client.
                var sendData = Encoding.ASCII.GetBytes("{\"server_connection\" : \"connected\"}");
                clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, null);
                // Listen for client data.
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
                // Continue listening for clients.
                serverSocket.BeginAccept(AcceptCallback, null);
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

        private void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                // Socket exception will raise here when client closes
                int received = clientSocket.EndReceive(AR);

                if (received == 0)
                {
                    return;
                }

                // The received data is deserialized in the EquationProperties.
                string message = Encoding.ASCII.GetString(buffer);
                // Any excess characters are stipped of the end
                int index = message.IndexOf("}");
                message = message.Substring(0, index + 1);
                // Deserialize the json string into the equation object
                equation = JsonConvert.DeserializeObject<Equations>(message);

                // Create new node and add to nodelist
                Invoke((Action)delegate
                {
                    LinkListNode node = new LinkListNode(equation);
                    equationNodeList.AddEquationNode(node);
                });

                //add the node to the binary tree
                if (tree.root == null)
                {
                    //if tree is emtpy, set the node as the root
                    tree.root = new BinaryTreeNode(equation);
                }
                else
                {
                    //add the node to the bindary tree
                    tree.Add(equation);
                }

                Invoke((Action)delegate
                {
                    rtb_binaryTree.Clear();
                    //the default print order for the binary tree is in-order
                    rtb_binaryTree.Text = BinaryTree.PrintInOrder(tree);
                    SendBtn.Enabled = true;
                });

                // Check if answer is incorrect
                if (equation.IsCorrect == false)
                {
                    Invoke((Action)delegate
                    {
                        if (equation.IsCorrect == false)
                        {
                            rtb_linkList.Text = InstructorController.PrintLinkList(equationNodeList);
                        }                     
                    });
                }

                // Start receiving data again.
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            // Avoid catching all exceptions handling in cases like these. 
            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);
                Invoke((Action)delegate
                {
                    //lbl_clientCount.Text = "No connected clients";
                });
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);
            }
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            // Set button state to false
            SendBtn.Enabled = false;
            // Create a new equation object from the given data
            Equations equation = new Equations(Convert.ToInt32(tb_firstNumber.Text),
                Convert.ToInt32(tb_secondNumber.Text), comboBoxOperators.Text, Convert.ToInt32(tb_answer.Text), false);

            try
            {
                // Serialize the object into a json string and send the data to the client
                string json = JsonConvert.SerializeObject(equation);
                var sendData = Encoding.ASCII.GetBytes(json);
                clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, null);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Unable to send message, there is no client connected.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SendBtn.Enabled = true;
            }

            // Add equation to list to be displayed
            equations.Insert(0, equation);

            RefreshResultDatagrid();
        }

        //loads the data grid view and allocates the data source values to the correct columns
        public void LoadQuestionsDataGridView()
        {
            dgv_questionsAsked.AutoGenerateColumns = false;
            dgv_questionsAsked.DataSource = equations;

            DataGridViewTextBoxColumn columnFirst = new DataGridViewTextBoxColumn();
            columnFirst.DataPropertyName = "FirstNumber";
            columnFirst.Name = "First";
            dgv_questionsAsked.Columns.Add(columnFirst);

            DataGridViewTextBoxColumn columnOperator = new DataGridViewTextBoxColumn();
            columnOperator.DataPropertyName = "Symbol";
            columnOperator.Name = "Operator";
            dataGridView1.Columns.Add(columnOperator);

            DataGridViewTextBoxColumn columnSecond = new DataGridViewTextBoxColumn();
            columnSecond.DataPropertyName = "SecondNumber";
            columnSecond.Name = "Second";
            dgv_questionsAsked.Columns.Add(columnSecond);

            DataGridViewTextBoxColumn columnResult = new DataGridViewTextBoxColumn();
            columnResult.DataPropertyName = "Result";
            columnResult.Name = "Result";
            dgv_questionsAsked.Columns.Add(columnResult);
        }

        //refreshes the data grid when a new object is added to the data source
        private void RefreshResultDatagrid()
        {
            dgv_questionsAsked.DataSource = null;

            dgv_questionsAsked.DataSource = equations;
        }

        #region Update result text box when calculation is changed
        private void comboBoxOperators_SelectedIndexChanged(object sender, EventArgs e)
        {
            tb_answer.Text = InstructorController.PerformCalculation(
                tb_firstNumber.Text,
                tb_secondNumber.Text,
                comboBoxOperators.Text).ToString();
        }

        private void tb_firstNumber_TextChanged(object sender, EventArgs e)
        {
            tb_answer.Text = InstructorController.PerformCalculation(
                tb_firstNumber.Text,
                tb_secondNumber.Text,
                comboBoxOperators.Text).ToString();
        }

        private void tb_secondNumber_TextChanged(object sender, EventArgs e)
        {
            tb_answer.Text = InstructorController.PerformCalculation(
                tb_firstNumber.Text,
                tb_secondNumber.Text,
                comboBoxOperators.Text).ToString();
        }
        #endregion

        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region List sorting buttons

        int count = 0;

        private void btn_sortOne_Click(object sender, EventArgs e)
        {
            List<Equations> tempList = new List<Equations>();

            if (count == 0)
            {
                tempList = equations.OrderBy(x => x.FirstNumber).ToList();
                dgv_questionsAsked.DataSource = tempList;
                count++;
            }
            else if (count == 1)
            {
                tempList = equations.OrderByDescending(x => x.FirstNumber).ToList();
                dgv_questionsAsked.DataSource = tempList;
                count = 0;
            }           
        }

        private void btn_sortTwo_Click(object sender, EventArgs e)
        {
            List<Equations> tempList = new List<Equations>();

            if (count == 0)
            {
                tempList = equations.OrderBy(x => x.Symbol).ToList();
                dgv_questionsAsked.DataSource = tempList;
                count++;
            }
            else if (count == 1)
            {
                tempList = equations.OrderByDescending(x => x.Symbol).ToList();
                dgv_questionsAsked.DataSource = tempList;
                count = 0;
            }
        }

        private void btn_sortThree_Click(object sender, EventArgs e)
        {
            List<Equations> tempList = new List<Equations>();

            if (count == 0)
            {
                tempList = equations.OrderBy(x => x.SecondNumber).ToList();
                dgv_questionsAsked.DataSource = tempList;
                count++;
            }
            else if (count == 1)
            {
                tempList = equations.OrderByDescending(x => x.SecondNumber).ToList();
                dgv_questionsAsked.DataSource = tempList;
                count = 0;
            }
        }

        private void btn_sortFour_Click(object sender, EventArgs e)
        {
            List<Equations> tempList = new List<Equations>();

            if (count == 0)
            {
                tempList = equations.OrderBy(x => x.Result).ToList();
                dgv_questionsAsked.DataSource = tempList;
                count++;
            }
            else if (count == 1)
            {
                tempList = equations.OrderByDescending(x => x.Result).ToList();
                dgv_questionsAsked.DataSource = tempList;
                count = 0;
            }
        }

        #endregion

        private void btn_printPreOrder_Click(object sender, EventArgs e)
        {
            //lbl_sortOrder.Text = "Binary Tree - Sorted by Pre-Order";
            rtb_binaryTree.Text = BinaryTree.PrintPreOrder(tree);
        }

        private void btn_printInOrder_Click(object sender, EventArgs e)
        {
            //lbl_sortOrder.Text = "Binary Tree - Sorted by In-Order";
            rtb_binaryTree.Text = BinaryTree.PrintInOrder(tree);
        }

        private void btn_printPostOrder_Click(object sender, EventArgs e)
        {
            //lbl_sortOrder.Text = "Binary Tree - Sorted by Post-Order";
            rtb_binaryTree.Text = BinaryTree.PrintPostOrder(tree);
        }

        private void btn_savePreOrder_Click(object sender, EventArgs e)
        {
            string preOrderDir = @"C:\ArithmeticChallenge\pre_order.txt";

            using (StreamWriter writer = new StreamWriter(preOrderDir, true))
            {
                writer.WriteLine(BinaryTree.PrintPreOrder(tree) + DateTime.Now.ToString("yyyy-MM-dd:hh-mm"));
                writer.Close();
            }         
        }

        private void btn_saveInOrder_Click(object sender, EventArgs e)
        {
            string preOrderDir = @"C:\ArithmeticChallenge\in_order.txt";

            using (StreamWriter writer = new StreamWriter(preOrderDir, true))
            {
                writer.WriteLine(BinaryTree.PrintInOrder(tree) + DateTime.Now.ToString("yyyy-MM-dd:hh-mm"));
                writer.Close();
            }
        }

        private void btn_savePostOrder_Click(object sender, EventArgs e)
        {
            string preOrderDir = @"C:\ArithmeticChallenge\post_order.txt";

            using (StreamWriter writer = new StreamWriter(preOrderDir, true))
            {
                writer.WriteLine(BinaryTree.PrintPostOrder(tree) + DateTime.Now.ToString("yyyy-MM-dd:hh-mm"));
                writer.Close();
            }
        }

        private void btn_numbersOnly(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
