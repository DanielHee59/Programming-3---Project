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
            PipeClient.MessageReceived += PipeClient_MessageReceived;
            PipeClient.ServerDisconnected += PipeClient_ServerDisconnected;
            DefaultUser();
            tbPath.Text = path;

            //both button will be set as false but once client login succesfully, it will set as true
            //so user won't be able to use this button until they login succesfully
            btnCsvReader.Enabled = false; 
            btnMusicPlayer.Enabled = false;
        }

        //Question 3 - Must contain hashing techniques
        //Add a default user to user database
        private void DefaultUser()
        {
            string userid = "abcde";
            string password = "abcde";
            string passwordHash = pwdManager.GeneratePasswordHash(password, out string salt);
            // save the values in the database
            User user = new User
            {
                UserId = userid,
                PasswordHash = passwordHash,
                Salt = salt
            };
            userRepo.AddUser(user); //Add user information into database
        }

        void PipeClient_ServerDisconnected()
        {
            Invoke(new PipeClient.ServerDisconnectedHandler(EnableStartButton));
        }

        //If client is disconnected from server, connect button will change from false to true
        void EnableStartButton()
        {
            btnConnect.Enabled = true;
        }

        void PipeClient_MessageReceived(byte[] message) => Invoke(new PipeClient.MessageReceivedHandler(DisplayReceivedMessage),
                new object[] { message });

        //Message sent from Server
        void DisplayReceivedMessage(byte[] message)
        {

            ASCIIEncoding encoder = new ASCIIEncoding();
            string str = encoder.GetString(message, 0, message.Length); //Decrypt receiving data(Server) from bytes to string

            tbReceiveServer.Text += str;


            //If message receive from client contains "Login Successfully", both button will be true
            if(str.Contains("Login Successfully" + "\r\n"))
            {
                //If button is true, user will be able to click them
                btnCsvReader.Enabled = true;
                btnMusicPlayer.Enabled = true;
            }
        }

        //Connect Button
        private void BtnConnect_Click(object sender, EventArgs e)
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

        //Question 3 - Must contain hashing techniques
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            //if both textBox is empty or one of them is empty, display an error "Missing Data"
            //if both textBox is filled with data then encode it into Bytes and send it to the server for validation
            if (!string.IsNullOrEmpty(tbUsername.Text) && !string.IsNullOrEmpty(tbPassword.Text))
            {
                //The encoding
                ASCIIEncoding encoder = new ASCIIEncoding();
                //Send message to Server
                PipeClient.SendMessage(encoder.GetBytes(tbUsername.Text + "," + tbPassword.Text));
            }
            else
            {
                MessageBox.Show("Missing Data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // if button is clear, client will be disconnected from server 
        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            PipeClient.Disconnect();
            btnConnect.Enabled = true;
        }

        //Clear all items found in the textBox called tbReceiveServer
        private void BtnReset_Click(object sender, EventArgs e)
        {
            tbReceiveServer.Clear();
        }

        //CSVReader button.When this button is clicked, launch CSVReader.cs form
        private void BtnCsvReader_Click(object sender, EventArgs e)
        {
            CSVReader csv = new CSVReader();
            csv.ShowDialog();
        }

        //MusicPlayer button.When this button is clicked, launch MusicPlayer.cs form
        private void BtnMusicPlayer_Click(object sender, EventArgs e)
        {
            MusicPlayer mp = new MusicPlayer();
            mp.ShowDialog();
        }
    }
}