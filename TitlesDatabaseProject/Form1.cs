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
            
            booksConnection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;"
                                    + "AttachDbFilename=C:\\Users\\jromo021627\\Documents\\TitlesDatabaseProject\\Databases;"
                                    + "Integrated Security=True;"
                                    + "Connect Timeout=30;");
            booksConnection.Open();
            lblState.Text = booksConnection.State.ToString();
            booksConnection.Close();
            lblState.Text += booksConnection.State.ToString();
            booksConnection.Dispose();
        }
    }
}
