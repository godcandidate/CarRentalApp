using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    
    public partial class login : Form
    {
        private readonly CarRentalDbEntities1 carRentalDbEntities;

        public login()
        {
            InitializeComponent();
            carRentalDbEntities= new CarRentalDbEntities1();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            
            try
            {
                var username = txtusername.Text.Trim();
                var password = txtpassword.Text;

                // create password from encryption from utils class
                var hashed_password = Utils.HashPassword(password);

                
                // select username,password from login table
                var user = carRentalDbEntities.loginInfoes.FirstOrDefault(data1 => data1.username == username &&
                data1.pass_word== hashed_password && data1.isActive == true);

                if (user == null)
                {
                    MessageBox.Show("Please provide valid credentials", "Login Error");
                }
                else
                {
                   var role = user.userRoles.FirstOrDefault();
                    var roleShortName = role.Role.shortname;

                    var mainwindow = new MainForm(this, user);
                    mainwindow.Show();
                    this.Hide();

                }

            }
            catch (Exception)
            {

                MessageBox.Show("Something wnet wrong. Please try again", "Login Error");
            }
        }
    }
}
