using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ResetPassword : Form
    {
        private readonly CarRentalDbEntities1 carRentalDbEntities = new CarRentalDbEntities1();

        public loginInfo _user;

        public ResetPassword()
        {
            InitializeComponent();
        }

        public ResetPassword(loginInfo user = null)
        {
            InitializeComponent();
            _user = user;
            txtusername.Text = user.username;
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnreset_Click(object sender, EventArgs e)
        {
            if (txtnewpass.Text == txtconfirmpass.Text && string.IsNullOrEmpty(txtnewpass.Text))
            {
                var hashed_password = Utils.HashPassword(txtconfirmpass.Text);
                
                // updating new password to database
                // searching for the user
                var query =
                    from person in carRentalDbEntities.loginInfoes
                    where person.username == _user.username
                    select person;

                //executing the query
                foreach (loginInfo person in query)
                {
                    person.pass_word = hashed_password;
                }
                
                // updating new password to database -- another way
                //_user.pass_word = hashed_password;
                // submit changes to the database
                try
                {
                    carRentalDbEntities.SaveChanges();
                    MessageBox.Show($"{_user.username}'s password has been reset!");
                    this.Close();
                }
                catch (Exception)
                {

                    MessageBox.Show("An error occured, try again", "Update Error");
                }
                
            }
            else
            {
                DialogResult result =MessageBox.Show($"Passwords do not match \n Please try again",
                    "Reset Password", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if ( result == DialogResult.Yes )
                {
                    txtnewpass.Clear();
                    txtconfirmpass.Clear();
                }
            }
            
        }
    }
}
