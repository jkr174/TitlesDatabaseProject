/* Jovany Romo
 * 12/1/20
 * Data Binding Branch
 * Summary:     Finally finished the 3-2 SQL assignment. 
 *              Now uses the connect button so that the program can error handle gracefully.
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

        public frmTitles()
        {
            InitializeComponent();
        }

        private void frmTitles_Load(object sender, EventArgs e)
        {
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// btnConnect_Click class
        /// 
        /// Summary:    Should allow the program to exit gracefully if there is an error.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            bool success = false;
            try
            {
                this.titlesTableAdapter.Fill(this.sQLBooksDBDataSet.Titles);
                success = true;
            }
            catch
            {
                if (!success)
                {
                    string message = "The program was not able to connect to SQL, please check if SQL Express is installed, along with MSSM Studio. Ensure that SQLBooksDB.mdf is located at the Working folder.";
                    string caption = "Cannot connect to SQL!";
                    DialogResult result = MessageBox.Show(message,
                        caption,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void titlesBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.titlesBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.sQLBooksDBDataSet);

        }
    }
}
