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


        public frmTitles()
        {
            InitializeComponent();
        }

        private void frmTitles_Load(object sender, EventArgs e)
        {
            bool canAccessDB = false;

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

                txtTitle.DataBindings.Add("Text", titlesTables, "Title");
                txtYearPublished.DataBindings.Add("Text", titlesTables, "Year_Published");
                txtISBN.DataBindings.Add("Text", titlesTables, "ISBN");
                txtPubID.DataBindings.Add("Text", titlesTables, "PubID");

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
                    const string message = "Cannot connect to database. Please check if SQLBooksDB.mdf is in the bin folder!";
                    const string caption = "Cannot find database!";

                    MessageBox.Show(message,
                        caption,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void txtPubID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
