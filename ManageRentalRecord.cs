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
    public partial class ManageRentalRecord : Form
    {
        private readonly CarRentalDbEntities1 carRentalDbEntities;

        
        public ManageRentalRecord()
        {
            InitializeComponent();
            carRentalDbEntities = new CarRentalDbEntities1();
        }

        public void PopulateGrid()
        {
            var records = carRentalDbEntities.CarRentalRecords
               .Select(data => new
               {
                   Customer = data.CustomerName,
                   Rent_Date = data.RentDate,
                   Received_Date = data.ReceivedDate,
                   data.Cost,
                   Id = data.Rental_Id,

                   //inner join of two tables using LINQ
                   Car = data.TypeofCar.Make + " " + data.TypeofCar.Model
               }
               ).ToList();

            dgvRecordList.DataSource = records;
            dgvRecordList.Columns["Rent_Date"].HeaderText = "Rent Date";
            dgvRecordList.Columns["Received_Date"].HeaderText = "Received Date";

            dgvRecordList.Columns["Id"].Visible = false;

        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            // using MDI form
            var addrentalform = new AddEditRentalForm(this);
            addrentalform.MdiParent = this.MdiParent;
            addrentalform.Show();
        }

        private void ManageRentalRecord_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        private void btnedit_Click(object sender, EventArgs e)
        {

            try
            {
                // get id of selected row
                var id = (int)dgvRecordList.SelectedRows[0].Cells["Id"].Value;

                // query database from record with id
                var record = carRentalDbEntities.CarRentalRecords.FirstOrDefault(data => data.Rental_Id == id);

                // launch  AddEditVehicle form with data
                var addEditRecord = new AddEditRentalForm(record, this);
                addEditRecord.MdiParent = this.MdiParent;
                addEditRecord.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\r" + "Select the entire row to Edit", "Select Error");

            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                // get id of selected row
                var id = (int)dgvRecordList.SelectedRows[0].Cells["Id"].Value;

                // query database from record with id
                var record = carRentalDbEntities.CarRentalRecords.FirstOrDefault(data => data.Rental_Id == id);

                DialogResult result = MessageBox.Show($"Are you sure you want to delete {record.CustomerName} records",
                    "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // delete from database
                    carRentalDbEntities.CarRentalRecords.Remove(record);
                    carRentalDbEntities.SaveChanges();

                    MessageBox.Show("Record data deleted successfully", "Delete Vehicle Record");
                    PopulateGrid();
                }
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\r" + "Select the entire row to Delete", "Select Error");
            }

        }
    }
}
