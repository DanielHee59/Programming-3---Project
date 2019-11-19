using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AT_4;

namespace AT3
{
    //Name: Daniel Hee
    //Student ID: P466436
    //Programming AT 3
    public partial class ClientForm : Form
    {
        private PipeClient PipeClient = new PipeClient();

        string path = "\\\\.\\pipe\\123123"; //path location for client to connect to server

        // Dummy repository class for DB operations
        MockUserRepository userRepo = new MockUserRepository();
        // Let us use the Password manager class to generate the password ans salt
        PasswordManager pwdManager = new PasswordManager();
        public ClientForm()
        {
            InitializeComponent();
            PipeClient.MessageReceived += pipeClient_MessageReceived;
            PipeClient.ServerDisconnected += pipeClient_ServerDisconnected;
            defaultUser();
            tbPath.Text = path;

            //both button will be set as false but once client login succesfully, it will set as true
            btnCsvReader.Enabled = false; 
            btnMusicPlayer.Enabled = false;
        }
        //Add a default user to user database
        private void defaultUser()
        {
            string userid = "abcde";
            string password = "abcde";
            string salt = null;
            string passwordHash = pwdManager.GeneratePasswordHash(password, out salt);
            // save the values in the database
            User user = new User
            {
                UserId = userid,
                PasswordHash = passwordHash,
                Salt = salt
            };
            userRepo.AddUser(user); //Add user information into database
        }

        void pipeClient_ServerDisconnected()
        {
            Invoke(new PipeClient.ServerDisconnectedHandler(EnableStartButton));
        }

        //If client is disconnected from server, connect button will change from false to true
        void EnableStartButton()
        {
            btnConnect.Enabled = true;
        }

        void pipeClient_MessageReceived(byte[] message)
        {
            Invoke(new PipeClient.MessageReceivedHandler(DisplayReceivedMessage),
                new object[] { message });
        }

        //Message sent from Server
        void DisplayReceivedMessage(byte[] message)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            string str = encoder.GetString(message, 0, message.Length); //Decrypt receiving data(Server) from bytes to string

            tbReceiveServer.Text += str;


            //If message receive from client contains "Login Successfully", both button will be true
            if(str.Contains("Login Successfully" + "\r\n"))
            {
                btnCsvReader.Enabled = true;
                btnMusicPlayer.Enabled = true;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            PipeClient.Connect(path); //Connect the path shown at TextBox with the Label Path

            if (!PipeClient.Connected) //if its not connected to server, display an error message
            {
                MessageBox.Show("Connection Error! Please Try Again Later!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Server Connected");
                btnConnect.Enabled = false; //if its connected to server, set button to false 
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            //if both textBox is empty or one of them is empty, display an error "Missing Data"
            //if both textBox is filled with data then encode it into Bytes and send it to the server for validation
            if (!string.IsNullOrEmpty(tbUsername.Text) && !string.IsNullOrEmpty(tbPassword.Text))
            {
                ASCIIEncoding encoder = new ASCIIEncoding();
                PipeClient.SendMessage(encoder.GetBytes(tbUsername.Text + "," + tbPassword.Text));
            }
            else
            {
                MessageBox.Show("Missing Data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // if button is clear, client will be disconnected from server 
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            PipeClient.Disconnect();
            btnConnect.Enabled = true;
        }

        //Clear all items found in the textBox called tbReceiveServer
        private void BtnReset_Click(object sender, EventArgs e)
        {
            tbReceiveServer.Clear();
        }

        private void BtnCsvReader_Click(object sender, EventArgs e)
        {
            CSVReader csv = new CSVReader();
            csv.ShowDialog();
        }

        private void BtnMusicPlayer_Click(object sender, EventArgs e)
        {
            MusicPlayer mp = new MusicPlayer();
            mp.ShowDialog();
        }
    }
}