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
    public partial class Adduser : Form
    {
        private readonly CarRentalDbEntities1 carRentalDbEntities = new CarRentalDbEntities1();

        private ManageUser _manageUser;
        public Adduser()
        {
            InitializeComponent();
        }

        public Adduser(ManageUser manageUser = null)
        {
            InitializeComponent();
            _manageUser = manageUser;
        }

        private void Adduser_Load(object sender, EventArgs e)
        {
            // select * from TypesOfCars
            //var cars = carRentalDbEntities.TypeofCars.ToList();

            var roles = carRentalDbEntities.Roles.ToList();

            cmbrole.DataSource = roles;
            cmbrole.DisplayMember = "role_name";
            cmbrole.ValueMember = "role_id";
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                    
                string username = txtusername.Text;
                var roleId = (int)cmbrole.SelectedValue;
                var password = Utils.DefaultPassword();

                //inserting new user to loginInfo table
                var user = new loginInfo
                {
                    username = username,
                    pass_word = password,
                    isActive = true,
                };
                carRentalDbEntities.loginInfoes.Add(user);
                carRentalDbEntities.SaveChanges();

                
                var userid = user.user_id;
                // adding new user to UserRole table
                var userrole = new userRole
                {
                    userID = userid,
                    roleID = roleId
                    
                };
                carRentalDbEntities.userRoles.Add(userrole);
                carRentalDbEntities.SaveChanges();
                
                MessageBox.Show($"{user.username} had been added successfully", "Add New User");
                _manageUser.PopulateGrid();
                Close();

            }
            catch (Exception)
            {

                MessageBox.Show("There was an error, try again","Error");
            }
            



        }
    }
}
