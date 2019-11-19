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
    public partial class ServerForm : Form
    {
        private PipeServer PipeServer = new PipeServer();

        string userID; //Global Variable for userID, userID information sent from client for verification will be stored here
        string Password; //Global Variable for Password, password information sent from client for verification will be stored here
        string path = "\\\\.\\pipe\\123123"; //Path location of Server

        // Dummy repository class for DB operations
        static MockUserRepository userRepo = new MockUserRepository();
        // Let us use the Password manager class to generate the password ans salt
        static PasswordManager pwdManager = new PasswordManager();
        public ServerForm()
        {
            InitializeComponent();

            PipeServer.MessageReceived += pipeServer_MessageReceived;
            PipeServer.ClientDisconnected += pipeServer_ClientDisconnected;

            tbPath.Text = path;
        }
        void pipeServer_ClientDisconnected()
        {
            Invoke(new PipeServer.ClientDisconnectedHandler(ClientDisconnected));
        }

        void ClientDisconnected()
        {
            MessageBox.Show("Total connected clients: " + PipeServer.TotalConnectedClients);
        }

        void pipeServer_MessageReceived(byte[] message)
        {
            Invoke(new PipeServer.MessageReceivedHandler(DisplayMessageReceived),
                new object[] { message });
        }

        //Received message sent from server
        void DisplayMessageReceived(byte[] message)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            string str = encoder.GetString(message, 0, message.Length);

            //When user pressed login on client side, information will be sent here via this format(userID, Password) 
            //information will be split using .Split Method.
            string[] details = str.Split(',');

            userID = details[0]; //userID information will be stored in Global Variable
            Password = details[1];//Password information will be stored in Global Variable

            TestLogin(); //Implementing this method when client pressed login on client side
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            //Display Message when there already a running server
            if (PipeServer.Running)
            {
                MessageBox.Show("Server is running", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else //Else, start a server and set start button to false
            {
                PipeServer.Start(path);
                btnStart.Enabled = false;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            lstOutput.Items.Clear(); //refresh the listBox

            string userId = tbUser.Text; //get userID from textBox 
            string password = tbPassword.Text; //get password from textBox
            lstOutput.Items.Add("User " + userId + " has been created"); //Add it to listBox
            lstOutput.Items.Add("userid " + userId); //Add it to listBox
            lstOutput.Items.Add("password " + password); //Add it to listBox
            string salt = null;
            string passwordHash = pwdManager.GeneratePasswordHash(password, out salt);  //Save password into pwdManager

            //Save new user to user database
            User newUser = new User
            {
                UserId = userId,
                PasswordHash = passwordHash,
                Salt = salt
            };

            userRepo.AddUser(newUser); //Add new user into database
            lstOutput.Items.Add("Salt " + salt);
            //Send Message to client informing them user has been created
            PipeServer.SendMessage(Encoding.ASCII.GetBytes("User Created: UserID:" + "," + userId + "," + "Password:" + "," + password + "\r\n"));

            tbUser.Clear();
            tbPassword.Clear();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userId = tbLogin.Text; //get userID from textBox 
            string passwordLogin = tbPasswordLogin.Text; //get password from textBox 

            User user1 = userRepo.GetUser(userId); //Get user details based on inputted userID

            bool result = pwdManager.IsPasswordMatch(passwordLogin, user1.Salt, user1.PasswordHash); //check if inputted password is matched with the password saved for this User

            if (result == true)
            { //if password is matched
                lstOutput.Items.Add("Test User Login Successfully");//Display on listBox
            }
            else
            { //if password is incorrect
                lstOutput.Items.Add("Test User Login Failed");//Display on listBox
            }

            tbLogin.Clear();
            tbPasswordLogin.Clear();

        }

        public void TestLogin()
        {

            User user2 = userRepo.GetUser(userID); //Get user details based on inputted userID

            bool result = pwdManager.IsPasswordMatch(Password, user2.Salt, user2.PasswordHash); //check if inputted password is matched with the password saved for this User

            if (result == true)
            { //if password is matched
                PipeServer.SendMessage(Encoding.ASCII.GetBytes("Login Successfully" + "\r\n"));  //Display on client side
               
            }
            else
            { //if password is incorrect
                PipeServer.SendMessage(Encoding.ASCII.GetBytes("Incorrect Password!" + "," + " Please try again!" + "\r\n")); //Display on client side
            }

            tbLogin.Clear();
            tbPasswordLogin.Clear();


        }
        private void BtnSend_Click(object sender, EventArgs e)
        {
            PipeServer.SendMessage(Encoding.ASCII.GetBytes(tbPublished.Text + "\r\n")); //Send information from server to client
        }
    }
}


