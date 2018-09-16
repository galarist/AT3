using ArithmeticChallenge.Controllers;
using ArithmeticChallenge.NodeFunctions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        private int PORT = 8888;
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
                    rtbAsked.Clear();
                    //the default print order for the binary tree is in-order
                    //rtbAsked.Text = BinaryTree.PrintInOrder(tree);
                    SendBtn.Enabled = true;
                });

                // Check if answer is incorrect
                if (equation.IsCorrect == false)
                {
                    Invoke((Action)delegate
                    {
                        if (equation.IsCorrect == false)
                        {
                            rtbWrong.Text = InstructorController.PrintLinkList(equationNodeList);
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

        private void Send_Click(object sender, EventArgs e)
        {
            // Set button state to false
            SendBtn.Enabled = false;
            // Create a new equation object from the given data
            Equations equation = new Equations(Convert.ToInt32(textBoxFnumber.Text),
                Convert.ToInt32(textBoxSnumber.Text), comboBoxOperators.Text, Convert.ToInt32(textBoxAnumber.Text), false);

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
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = equations;

            DataGridViewTextBoxColumn columnFirst = new DataGridViewTextBoxColumn();
            columnFirst.DataPropertyName = "FNumber";
            columnFirst.Name = "First Number";
            dataGridView1.Columns.Add(columnFirst);

            DataGridViewTextBoxColumn columnOperator = new DataGridViewTextBoxColumn();
            columnOperator.DataPropertyName = "Operator";
            columnOperator.Name = "Operator";
            dataGridView1.Columns.Add(columnOperator);

            DataGridViewTextBoxColumn columnSecond = new DataGridViewTextBoxColumn();
            columnSecond.DataPropertyName = "SNumber";
            columnSecond.Name = "Second Number";
            dataGridView1.Columns.Add(columnSecond);

            DataGridViewTextBoxColumn columnResult = new DataGridViewTextBoxColumn();
            columnResult.DataPropertyName = "ANumber";
            columnResult.Name = "Answer";
            dataGridView1.Columns.Add(columnResult);
        }

        //refreshes the data grid when a new object is added to the data source
        private void RefreshResultDatagrid()
        {
            dataGridView1.DataSource = null;

            dataGridView1.DataSource = equations;
        }

        #region Update result text box when calculation is changed
        private void ComboBoxOperators_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxAnumber.Text = InstructorController.PerformCalculation(
                textBoxFnumber.Text,
                textBoxSnumber.Text,
                comboBoxOperators.Text).ToString();
        }

        private void TextBoxFnumber_TextChanged(object sender, EventArgs e)
        {
            textBoxAnumber.Text = InstructorController.PerformCalculation(
                textBoxFnumber.Text,
                textBoxSnumber.Text,
                comboBoxOperators.Text).ToString();
        }

        private void TextBoxSnumber_TextChanged(object sender, EventArgs e)
        {
            textBoxAnumber.Text = InstructorController.PerformCalculation(
                textBoxFnumber.Text,
                textBoxSnumber.Text,
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

        #region Sorting Fucntion

        int count = 0;

        private void btn_sortOne_Click(object sender, EventArgs e)
        {
            List<Equations> tempList = new List<Equations>();

            if (count == 0)
            {
                tempList = equations.OrderBy(x => x.Operator).ToList();
                dataGridView1.DataSource = tempList;
                count++;
            }
            else if (count == 1)
            {
                tempList = equations.OrderByDescending(x => x.Operator).ToList();
                dataGridView1.DataSource = tempList;
                count = 0;
            }
        }

        private void btn_sortTwo_Click(object sender, EventArgs e)
        {
            List<Equations> tempList = new List<Equations>();

             if (count == 0)
             {
                 tempList = equations.OrderBy(x => x.Operator).ToList();
                 dataGridView1.DataSource = tempList;
                 count++;
             }
             else if (count == 1)
             {
                 tempList = equations.OrderByDescending(x => x.Operator).ToList();
                 dataGridView1.DataSource = tempList;
                 count = 0;
            }
        }

        private void btn_sortThree_Click(object sender, EventArgs e)
        {
            List<Equations> tempList = new List<Equations>();

            if (count == 0)
            {
                tempList = equations.OrderBy(x => x.Operator).ToList();
                dataGridView1.DataSource = tempList;
                count++;
            }
            else if (count == 1)
            {
                tempList = equations.OrderByDescending(x => x.Operator).ToList();
                dataGridView1.DataSource = tempList;
                count = 0;
            }
        }    

        #endregion

        private void ShowPreOrder_Click(object sender, EventArgs e)
        {
            //lbl_sortOrder.Text = "Binary Tree - Sorted by Pre-Order";
            rtbAsked.Text = "Pre-Order: " + BinaryTree.PrintPreOrder(tree);
        }

        private void ShowInOrder_Click(object sender, EventArgs e)
        {
            //lbl_sortOrder.Text = "Binary Tree - Sorted by In-Order";
            rtbAsked.Text = "In-Order: " + BinaryTree.PrintInOrder(tree);
        }

        private void ShowPostOrder_Click(object sender, EventArgs e)
        {
            //lbl_sortOrder.Text = "Binary Tree - Sorted by Post-Order";
            rtbAsked.Text = "Post-Order: " + BinaryTree.PrintPostOrder(tree);
        }

        private void SavePreOrder_Click(object sender, EventArgs e)
        {
            using (FileStream stream = new FileStream(@"PreOrder.txt", FileMode.Create))
            using (TextWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine("");
            }

            string preOrderDir = @"PreOrder.txt";

            using (StreamWriter writer = new StreamWriter(preOrderDir, true))
            {
                writer.WriteLine("Pre-Order: " + BinaryTree.PrintPreOrder(tree));
                writer.Close();
            }

            var lines = File.ReadAllLines(@"PreOrder.txt").Where(arg => !string.IsNullOrWhiteSpace(arg));
            File.WriteAllLines(@"PreOrder.txt", lines);
        }

        private void SaveInOrder_Click(object sender, EventArgs e)
        {
            using (FileStream stream = new FileStream(@"InOrder.txt", FileMode.Create))
            using (TextWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine("");
            }

            string preOrderDir = @"InOrder.txt";

            using (StreamWriter writer = new StreamWriter(preOrderDir, true))
            {
                writer.WriteLine("In-Order: " + BinaryTree.PrintInOrder(tree));
                writer.Close();
            }

            var lines = File.ReadAllLines(@"InOrder.txt").Where(arg => !string.IsNullOrWhiteSpace(arg));
            File.WriteAllLines(@"InOrder.txt", lines);
        }

        private void SavePostOrder_Click(object sender, EventArgs e)
        {
            using (FileStream stream = new FileStream(@"PostOrder.txt", FileMode.Create))
            using (TextWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine("");
            }

            string preOrderDir = @"PostOrder.txt";

            using (StreamWriter writer = new StreamWriter(preOrderDir, true))
            {
                writer.WriteLine("Post-Order: " + BinaryTree.PrintPostOrder(tree));
                writer.Close();
            }

            var lines = File.ReadAllLines(@"PostOrder.txt").Where(arg => !string.IsNullOrWhiteSpace(arg));
            File.WriteAllLines(@"PostOrder.txt", lines);

        }

        private void btn_numbersOnly(object sender, KeyPressEventArgs e)
        {
          // e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void Instructor_Load(object sender, EventArgs e)
        {
            this.Text = "";
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.RowTemplate.Resizable = DataGridViewTriState.True;
            dataGridView1.RowTemplate.Height = 30;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 13F);
            dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 12F);
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSkyBlue;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView1.BackgroundColor = Color.FromArgb(36, 73, 110);
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromName("Highlight");
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromName("ButtonHighlight");
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromName("AppWorkspace");
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView1.BackColor = Color.FromArgb(/*244, 160, 144*/242, 237, 220);
            DataGridViewColumn column = dataGridView1.Columns[0];
            DataGridViewColumn column1 = dataGridView1.Columns[1];
            DataGridViewColumn column2 = dataGridView1.Columns[2];
            DataGridViewColumn column3 = dataGridView1.Columns[3];
            column.Width = 160;
            column1.Width = 120;
            column2.Width = 140;
            column3.Width = 143;
        }
    }
}
