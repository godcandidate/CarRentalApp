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

    public partial class AddEditRentalForm : Form
    {
        private readonly CarRentalDbEntities1 carRentalDbEntities = new CarRentalDbEntities1();
        private bool isEditMode;

        private ManageRentalRecord _manageRentalRecord;

        public AddEditRentalForm(ManageRentalRecord manageRentalRecord = null)
        {
            InitializeComponent();
            lblTitle.Text = "Add New Rental";
            isEditMode = false;
            btnsave.Text = "Add Rental";
            _manageRentalRecord = manageRentalRecord;
        }

        public void PopulateFields(CarRentalRecord record)
        {
            lblID.Text = record.Rental_Id.ToString();
            txtcustomername.Text = record.CustomerName;
            dtpdateRented.Value = (DateTime)record.RentDate;
            dtpReturned.Value = (DateTime)record.ReceivedDate;
            txtcost.Text = record.Cost.ToString();
            
           

        }
        public AddEditRentalForm(CarRentalRecord recordToEdit, ManageRentalRecord manageRentalRecord = null)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Rental Record";
            isEditMode = true;
            PopulateFields(recordToEdit);
            btnsave.Text = "Save Changes";
            _manageRentalRecord = manageRentalRecord;
        }

        private void btnsubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var rent_date = dtpdateRented.Value;
                var return_date = dtpReturned.Value;

                var cartype = cmbcartype.Text;
                double cost = Convert.ToDouble(txtcost.Text);

                // Edit mode 
                if (isEditMode)
                {
                    var id = int.Parse(lblID.Text);
                    var record = carRentalDbEntities.CarRentalRecords.FirstOrDefault(data => data.Rental_Id == id);
                    record.CustomerName = txtcustomername.Text;
                    record.RentDate = rent_date;
                    record.ReceivedDate = return_date;
                    record.Cost = (decimal)cost;
                    record.CarType = (int)cmbcartype.SelectedIndex;
                    

                    carRentalDbEntities.SaveChanges();
                    MessageBox.Show("Record has been updated sucessfully ", "Edit Record");
                }
                else 
                {
                    //validating
                    if (string.IsNullOrEmpty(txtcustomername.Text) || string.IsNullOrEmpty(cartype))
                    {
                        MessageBox.Show("Enter missing data");
                    }
                    else if (rent_date > return_date)
                    {
                        MessageBox.Show("Illegal date selection");
                    }
                    else
                    {
                        // inserting into the database
                        var RentalRecord = new CarRentalRecord();
                        RentalRecord.CustomerName = txtcustomername.Text;
                        RentalRecord.RentDate = rent_date;
                        RentalRecord.ReceivedDate = return_date;
                        RentalRecord.Cost = decimal.Parse(txtcost.Text);
                        RentalRecord.CarType = (int)cmbcartype.SelectedValue;

                        // adding object to the database
                        carRentalDbEntities.CarRentalRecords.Add(RentalRecord);
                        carRentalDbEntities.SaveChanges();

                        MessageBox.Show("Records submited sucessfully ", "Submit Records");
                    }
                }
                if (_manageRentalRecord != null)
                {
                    _manageRentalRecord.PopulateGrid();
                }
                Close();
                
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message);
            }

            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // select * from TypesOfCars
            //var cars = carRentalDbEntities.TypeofCars.ToList();

            var cars = carRentalDbEntities.TypeofCars
                .Select(data => new
                {
                    Id = data.Car_Id,
                    name = data.Make + " " + data.Model
                })
                .ToList();
            
            cmbcartype.DisplayMember = "name";
            cmbcartype.ValueMember = "Id";
            cmbcartype.DataSource = cars;

        }
    }
}
