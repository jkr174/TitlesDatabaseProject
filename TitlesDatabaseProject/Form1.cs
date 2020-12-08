/* Jovany Romo
 * 12/1/20
 * Data Binding Branch
 * 
 */

using System;
using System.IO;
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

namespace TitlesDatabaseProject
{
    public partial class frmTitles : Form
    {
        SqlConnection booksConnection;
        CurrencyManager titlesManager;

        public frmTitles()
        {
            InitializeComponent();
        }

        private void frmTitles_Load(object sender, EventArgs e)
        {
            
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            titlesManager.Position = 0;
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            titlesManager.Position--;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            titlesManager.Position++;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            titlesManager.Position = titlesManager.Count - 1;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            bool canAccessDB = false;
            var database = "SQLBooksDB.mdf";

            try
            {
                booksConnection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;"
                                    + "AttachDbFilename=|DataDirectory|\\" + database + ";"
                                    + "Integrated Security=True;"
                                    + "Connect Timeout=30;");

                booksConnection.Open();

                SqlCommand titlesCommand = new SqlCommand("Select * from Titles", booksConnection);
                SqlDataAdapter titlesAdapter = new SqlDataAdapter();
                titlesAdapter.SelectCommand = titlesCommand;
                DataTable titlesTables = new DataTable();
                titlesAdapter.Fill(titlesTables);

                txtTitle.DataBindings.Add("Text", titlesTables, "Title");
                txtYearPublished.DataBindings.Add("Text", titlesTables, "Year_Published");
                txtISBN.DataBindings.Add("Text", titlesTables, "ISBN");
                txtPubID.DataBindings.Add("Text", titlesTables, "PubID");

                titlesManager = (CurrencyManager)
                    BindingContext[titlesTables];

                booksConnection.Close();

                booksConnection.Dispose();
                titlesCommand.Dispose();
                titlesAdapter.Dispose();
                titlesTables.Dispose();

                canAccessDB = true;
            }
            catch
            {
                if (!canAccessDB)
                {
                    string message = "Cannot connect to database. Please check if " + database + " is in the bin folder.";
                    string caption = "Cannot find " + database + " file!";
                    DialogResult result = MessageBox.Show(message,
                        caption,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                
            }
            if (canAccessDB == true)
            {
                string message = "Successfully connected to " + database + " file.";
                string caption = "Success!";
                btnConnect.Enabled = false;

                MessageBox.Show(message,
                    caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
}
