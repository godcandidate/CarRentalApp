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
    public partial class ManageVehicleListing : Form
    {
        private readonly CarRentalDbEntities1 carRentalDbEntities;
        public ManageVehicleListing()
        {
            InitializeComponent();
            carRentalDbEntities= new CarRentalDbEntities1();

        }

        private void ManageVehicleListing_Load(object sender, EventArgs e)
        {
            PopulateGrid();
            // select * from TypeofCars
            //var cars = carRentalDbEntities.TypeofCars.ToList();

            // select Car_Id as ID, Car_name as Name from TypeofCars
            /*
            dgvCarList.Columns[0].HeaderText = "CarID";
            dgvCarList.Columns[1].HeaderText = "Car Name";*/
        }

        public void PopulateGrid()
        {
            var cars = carRentalDbEntities.TypeofCars
               .Select(data => new
               {
                   Make = data.Make,
                   Model = data.Model,
                   VIN = data.VIN,
                   Year = data.Year,
                   licensePlateNumber = data.LicensePlateNumber,
                   data.Car_Id
               }
               ).ToList();

            dgvCarList.DataSource = cars;
            dgvCarList.Columns[4].HeaderText = "License Plate Number ";
            dgvCarList.Columns[5].Visible = false;

        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        private void btnsubmit_Click(object sender, EventArgs e)
        {
            // using MDI form
            var addVehicleform = new AddEditVehicle(this);
            addVehicleform.MdiParent = this.MdiParent;
            addVehicleform.Show();
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            try
            {
                // get id of selected row
                var id = (int)dgvCarList.SelectedRows[0].Cells["Car_Id"].Value;

                // query database from record with id
                var car = carRentalDbEntities.TypeofCars.FirstOrDefault(data => data.Car_Id == id);

                // launch  AddEditVehicle form with data
                var addVehicleform = new AddEditVehicle(car, this);
                addVehicleform.MdiParent = this.MdiParent;
                addVehicleform.Show();
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
                var id = (int)dgvCarList.SelectedRows[0].Cells["Car_Id"].Value;

                // query database from record with id
                var car = carRentalDbEntities.TypeofCars.FirstOrDefault(data => data.Car_Id == id);
     
                DialogResult result = MessageBox.Show($"Are you sure you want to delete {car.Make} listings",
                    "Delete Listing", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                
                if (result == DialogResult.Yes) 
                {
                    // delete from database
                    carRentalDbEntities.TypeofCars.Remove(car);
                    carRentalDbEntities.SaveChanges();

                    MessageBox.Show("Vehicle data deleted successfully", "Delete Vehicle Record");
                    PopulateGrid();
                }
                
            }
            catch (Exception ex )
            {
                MessageBox.Show(ex.Message + "\n\r" + "Select the entire row to Delete", "Select Error");
            }

        }
    }
}
