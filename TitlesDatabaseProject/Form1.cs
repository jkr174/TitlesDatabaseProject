/* Jovany Romo
 * 12/1/20
 * Data Binding Branch
 * Summary:     Finally added proper error handling, that way it exits gracefully instead of crashing. 
 *              Also added a dialog method that lets you choose the path of the database file. 
 *          
 *  Inputs:     When the user clicks on Connect, 
 *              it opens a dialog box to select the database file.
 *  
 *  Outputs:    If there is an error, instead of the program crashing, 
 *              it allows you to try and reconnect to the database file.
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
        /// <summary>
        /// btnConnect_Click class
        /// 
        /// Summary:    When the user clicks on the connect button, 
        ///             it allows them to select the database file.
        /// 
        /// Inputs:     User selects the database file.
        /// Outputs:    Should allow the program to access the database, 
        ///             if not, it will give you an error message, 
        ///             and it allows you to try to select the file again.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            bool canAccessDB = false;
            string database = "SQLBooksDB.mdf";

            string fileContent = string.Empty;
            string filePath = string.Empty;
            // Allows for the user to locate the database file.
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "mdf files(*.mdf)|*.mdf";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;

                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            // Try catch block to allow the program to handle errors more gracefully instead of just crashing.
            try
            {
                booksConnection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;"
                                    + "AttachDbFilename=" + filePath + ";"
                                    + "Integrated Security=True;"
                                    + "Connect Timeout=30;");

                booksConnection.Open();

                SqlCommand titlesCommand = new SqlCommand("Select * from Titles", booksConnection);
                SqlDataAdapter titlesAdapter = new SqlDataAdapter();
                titlesAdapter.SelectCommand = titlesCommand;
                DataTable titlesTables = new DataTable();
                titlesAdapter.Fill(titlesTables);

                /*txtTitle.DataBindings.Add("Text", titlesTables, "Title");
                txtYearPublished.DataBindings.Add("Text", titlesTables, "Year_Published");
                txtISBN.DataBindings.Add("Text", titlesTables, "ISBN");
                txtPubID.DataBindings.Add("Text", titlesTables, "PubID");*/

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
                // Error caught
                if (!canAccessDB)
                {
                    string message = "Cannot connect to database. Please check if " + database + " is selected.";
                    string caption = "Cannot connect to " + database + " file!";
                    DialogResult result = MessageBox.Show(message,
                        caption,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                
            }
            if (canAccessDB == true)
            {
                // Success
                string message = "Successfully connected to " + database + " file.";
                string caption = "Success!";

                //btnConnect.Enabled = false;

                MessageBox.Show(message,
                    caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
}
