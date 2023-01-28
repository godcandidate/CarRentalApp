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
    
    public partial class ManageUser : Form
    {
        private readonly CarRentalDbEntities1 carRentalDbEntities;

        public ManageUser()
        {
            InitializeComponent();
            carRentalDbEntities = new CarRentalDbEntities1();
        }

        public void PopulateGrid()
        {
            var users = carRentalDbEntities.loginInfoes
               .Select(data => new
               {
                   data.user_id,
                   data.username,
                   data.userRoles.FirstOrDefault().Role.role_name,
                   data.isActive
                   
               }
               ).ToList();

            dgvUserList.DataSource = users;
            dgvUserList.Columns["username"].HeaderText = "Username";
            dgvUserList.Columns["isActive"].HeaderText = "Active";
            dgvUserList.Columns["role_name"].HeaderText = "Role";

            dgvUserList.Columns["user_id"].Visible = false;



        }

        private void ManageUser_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        private void btndeactivate_Click(object sender, EventArgs e)
        {
            try
            {
                // get id of selected row
                var id = (int)dgvUserList.SelectedRows[0].Cells["user_id"].Value;

                // query database from record with id
                var user = carRentalDbEntities.loginInfoes.FirstOrDefault(data => data.user_id == id);

                // change user active status
                user.isActive= user.isActive == true ? false: true;

                carRentalDbEntities.SaveChanges();
                MessageBox.Show($"{user.username}'s status has been changed successfully", "Change Status");
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\r" + "Select the entire row to Edit", "Select Error");

            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            if (!Utils.FormsIsOpen("Adduser"))
            {
                var adduser = new Adduser(this);
                adduser.MdiParent = this.MdiParent;
                adduser.Show();
            }
        }

        private void btnreset_Click(object sender, EventArgs e)
        {

            try
            {
                // get id of selected row
                var id = (int)dgvUserList.SelectedRows[0].Cells["user_id"].Value;

                // query database from record with id
                var user = carRentalDbEntities.loginInfoes.FirstOrDefault(data => data.user_id == id);

                //update password to default
                user.pass_word = Utils.DefaultPassword();

                carRentalDbEntities.SaveChanges();
                MessageBox.Show($"{user.username}'s password has been reset to default password", "Reset Password");


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\r" + "Select the entire row to Edit", "Select Error");

            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}
