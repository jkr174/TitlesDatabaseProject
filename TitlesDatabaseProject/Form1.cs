using System;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TitlesDatabaseProject
{
    public partial class frmTitles : Form
    {
        SqlConnection NWindConnection;
        SqlCommand customersCommand;
        SqlDataAdapter customersAdapter;
        DataTable customersTable;
        CurrencyManager customersManager;


        public frmTitles()
        {
            InitializeComponent();
        }

        private void frmTitles_Load(object sender, EventArgs e)
        {
            bool canAccessDB = false;
            //string fileNameDB = "SQLNWindDB.mdf";

            string fileContent = string.Empty;
            string filePath = string.Empty;
            using(OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                string CombinedPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..\\..");

                openFileDialog.InitialDirectory = System.IO.Path.GetFullPath(CombinedPath);
                openFileDialog.Filter = "mdf files(*.mdf)|*.mdf";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = false;
                openFileDialog.Multiselect = false;

                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;

                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            try
            {
                NWindConnection = new SqlConnection("Data Source=.\\SQLEXPRESS;" +
                    "AttachDbFilename=" + filePath + ";" +
                    "integrated Security=True;" +
                    "Connect Timeout=30;" +
                    "User Instance=True");

                NWindConnection.Open();

                customersCommand = new SqlCommand("SELECT * " +
                    "FROM Customers",
                    NWindConnection);

                customersAdapter = new SqlDataAdapter();
                customersAdapter.SelectCommand = customersCommand;
                customersTable = new DataTable();
                customersAdapter.Fill(customersTable);

                txtCustomerID.DataBindings.Add("Text", customersTable, "CustomerID");
                txtCompanyName.DataBindings.Add("Text", customersTable, "CompanyName");
                txtContactName.DataBindings.Add("Text", customersTable, "ContactName");
                txtContactTitle.DataBindings.Add("Text", customersTable, "ContactTitle");

                customersManager = (CurrencyManager)
                    BindingContext[customersTable];

                NWindConnection.Close();

                NWindConnection.Dispose();
                customersCommand.Dispose();
                customersAdapter.Dispose();
                customersTable.Dispose();
                canAccessDB = true;
            }
            catch
            {
                if (!canAccessDB)
                {
                    string message = "Cannot connect to database. Please check if the database was selected.";
                    string caption = "Cannot connect to file!";
                    DialogResult result = MessageBox.Show(message,
                        caption,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnFirst_Click(object sender, EventArgs e)
        {
            customersManager.Position = 0;
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            customersManager.Position--;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            customersManager.Position++;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            customersManager.Position = customersManager.Count - 1;
        }
    }
}
