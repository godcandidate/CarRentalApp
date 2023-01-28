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
    public partial class AddEditVehicle : Form
    {
        private bool isEditMode;
        private readonly CarRentalDbEntities1 carRentalDbEntities = new CarRentalDbEntities1();

        private ManageVehicleListing _manageVehicleListing;
        public AddEditVehicle(ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Add New Vehicle";
            isEditMode = false;
            btnsave.Text = "Add Vehicle";
           _manageVehicleListing = manageVehicleListing; 
        }

        public AddEditVehicle(TypeofCar carToEdit, ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Vehicle";
            isEditMode = true;
            PopulateFields(carToEdit);
            btnsave.Text = "Save Changes";
            _manageVehicleListing = manageVehicleListing;
        }

        private void PopulateFields(TypeofCar car)
        {
            lblID.Text = car.Car_Id.ToString();
            txtmake.Text = car.Make;
            txtmodel.Text = car.Model;
            txtVIN.Text = car.VIN;
            txtyear.Text = car.Year.ToString();
            txtplateNumber.Text = car.LicensePlateNumber;
        }

  
        private void AddEditVehicle_Load(object sender, EventArgs e)
        {

        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (isEditMode)
                {
                    var id = int.Parse(lblID.Text);
                    var car = carRentalDbEntities.TypeofCars.FirstOrDefault(data => data.Car_Id == id);
                    car.Make = txtmake.Text;
                    car.Model = txtmodel.Text;
                    car.VIN = txtVIN.Text;
                    car.Year = int.Parse(txtyear.Text);
                    car.LicensePlateNumber = txtplateNumber.Text;

                    carRentalDbEntities.SaveChanges();
                    MessageBox.Show("Vehicles has been updated sucessfully ", "Edit Vehicle");
                }

                else
                {
                    var make = txtmake.Text;
                    var model = txtmodel.Text;
                    var VIN = txtVIN.Text;
                    var year = txtyear.Text;
                    var platenumber = txtplateNumber.Text;

                    //validating
                    if (string.IsNullOrEmpty(make) || string.IsNullOrEmpty(model) || string.IsNullOrEmpty(VIN) ||
                        string.IsNullOrEmpty(year) || string.IsNullOrEmpty(platenumber))
                    {
                        MessageBox.Show("Enter missing data");
                    }

                    else
                    {
                        // inserting into the database
                        var car = new TypeofCar();
                        car.Make = make;
                        car.Model = model;
                        car.VIN = VIN;
                        car.Year = Convert.ToInt32(year);
                        car.LicensePlateNumber = platenumber;


                        // adding object to the database
                        carRentalDbEntities.TypeofCars.Add(car);
                        carRentalDbEntities.SaveChanges();

                        MessageBox.Show("Vehicles has been added sucessfully ", "Add Vehicle");
                        
                    }
                }
                _manageVehicleListing.PopulateGrid();
                Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
