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
            /*try
            {
                if (!File.Exists("SQLBooksDB.mdf"))
                    throw new FileNotFoundException();
            }
            catch(Exception e)
            {
                MessageBox.Show("The file does not exist!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                WriteException(e);
            }*/

            booksConnection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;"
                                    + "AttachDbFilename='|DataDirectory|\\SQLBooksDB.mdf';"
                                    + "Integrated Security=True;"
                                    + "Connect Timeout=30;");
            booksConnection.Open();

            SqlCommand titlesCommand = new SqlCommand("Select * from Titles", booksConnection);
            SqlDataAdapter titlesAdapter = new SqlDataAdapter();
            titlesAdapter.SelectCommand = titlesCommand;
            DataTable titlesTable = new DataTable();
            titlesAdapter.Fill(titlesTable);

            txtTitle.DataBindings.Add("Text", titlesTable, "Title");
            txtYearPublished.DataBindings.Add("Text", titlesTable, "Year_Published");
            txtISBN.DataBindings.Add("Text", titlesTable, "ISBN");
            txtPubID.DataBindings.Add("Text", titlesTable, "PubID");
            
            booksConnection.Close();
            booksConnection.Dispose();
            titlesCommand.Dispose();
            titlesAdapter.Dispose();
            titlesTable.Dispose();
        }
        public static void WriteException(Exception exception)
        {
            string filePath = @"|DataDirectory|//Error.txt";

            using (StreamWriter logWriter = new StreamWriter(filePath, true))
            {
                logWriter.WriteLine("Message :" + exception.Message + "<br/>" + Environment.NewLine + "StackTrace :" + exception.StackTrace +
                    "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                logWriter.WriteLine(Environment.NewLine + "----------------------");
            }
        }
    }
}
