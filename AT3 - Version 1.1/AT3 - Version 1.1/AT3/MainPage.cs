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
    public partial class MainPage : Form
    {
        public MainPage()
        {
            InitializeComponent();
            creatingNewAdmin();
        }

        // Dummy repository class for DB operations
        MockUserRepository userRepo = new MockUserRepository();
        // Let us use the Password manager class to generate the password and salt
        PasswordManager pwdManager = new PasswordManager();

        private void btnLogin_Click(object sender, EventArgs e)
        {

            string userId = tbName.Text;
            string password = tbPassword.Text;

            User loginAdmin = userRepo.GetUser(userId); //get userID

            bool result = pwdManager.IsPasswordMatch(password, loginAdmin.Salt, loginAdmin.PasswordHash); //check if inputted password is matched with the password saved for this User

            if (result == true) //if password matched
            {
                MessageBox.Show("Password matched! Server has been logon");
                new MultiFormContext(new ServerForm(), new ClientForm()); //serverform and clientform popup
            }
            else //if password is incorrect
            {
                MessageBox.Show("Incorrect Password! Please try again!");
            }
        }

        private string creatingNewAdmin()
        {
            string userid = "user"; //creating userID for admin 
            string password = "user"; //creating password for admin
            string salt = null;
            string passwordHash = pwdManager.GeneratePasswordHash(password, out salt);
            User createAdmin = new User
            {
                UserId = userid,
                PasswordHash = passwordHash,
                Salt = salt
            };
            userRepo.AddUser(createAdmin); //add admin details into database
            return salt;
        }
        
    }
}