using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        string connectionString;
        string queryString;
        string parentTable;//Supplier or Departament
        string childTable;//Products or Employees
        string parentPk;
        string childPk;
        string relationFk;
        string columnFk;
        int textBoxCount;
        int childPkIndex;
        int parentPkIndex;
        List<TextBox> textBoxList = new List<TextBox>();
        List<Label> labelList = new List<Label>();
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connectionString = ConfigurationManager.AppSettings.Get("connectionString");

            //MessageBox.Show(connectionString);
            childTable = ConfigurationManager.AppSettings.Get("childTable");
            parentTable = ConfigurationManager.AppSettings.Get("parentTable");

            queryString = "select C.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS T JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE C ON C.CONSTRAINT_NAME=T.CONSTRAINT_NAME WHERE C.TABLE_NAME='"+parentTable+"' and T.CONSTRAINT_TYPE='PRIMARY KEY' ";
            
            //MessageBox.Show(queryString);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    
                    connection.Open();

                    SqlCommand command = new SqlCommand(queryString, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    
                    reader.Read();

                    parentPk = reader.GetString(0);



                    
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            //MessageBox.Show(parentPk);
            queryString = "select C.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS T JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE C ON C.CONSTRAINT_NAME=T.CONSTRAINT_NAME WHERE C.TABLE_NAME='"+childTable+"' and T.CONSTRAINT_TYPE='PRIMARY KEY' ";
            //MessageBox.Show(queryString);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {

                    connection.Open();

                    SqlCommand command = new SqlCommand(queryString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    reader.Read();

                    childPk = reader.GetString(0);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            //MessageBox.Show(childPk);

            queryString = "SELECT COL_NAME(parent_object_id, parent_column_id) FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID( '" + childTable + "') AND OBJECT_NAME(referenced_object_id) = '" + parentTable + "'";
            //MessageBox.Show(queryString);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {

                    connection.Open();

                    SqlCommand command = new SqlCommand(queryString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    reader.Read();

                    relationFk = reader.GetString(0);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            //MessageBox.Show(relationFk);

            queryString = "select COLUMN_NAME from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE WHERE CONSTRAINT_NAME in(select CONSTRAINT_NAME from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE UNIQUE_CONSTRAINT_NAME in (select CONSTRAINT_NAME from INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE Table_Name = '" + parentTable + "' and constraint_type = 'primary key'))";
            //MessageBox.Show(queryString);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {

                    connection.Open();

                    SqlCommand command = new SqlCommand(queryString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    reader.Read();

                    columnFk = reader.GetString(0);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            //MessageBox.Show(columnFk);

            queryString = "SELECT count(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '"+childTable+"'";
            //MessageBox.Show(queryString);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    
                    connection.Open();

                    SqlCommand command = new SqlCommand(queryString, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    reader.Read();

                    textBoxCount = reader.GetInt32(0);
                    //MessageBox.Show("here");
                    //MessageBox.Show(textBoxCountTemp);
                    //System.Console.WriteLine(textBoxCount);

                    //textBoxCount = Int32.Parse(textBoxCountTemp);
                    //System.Console.WriteLine(textBoxCount);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            //MessageBox.Show(textBoxCount.ToString());



            foreach (int i in Enumerable.Range(0,textBoxCount))
            {
                Label label = new Label();
                label.Text = i.ToString();
                labelList.Add(label);
                labelList[i].Location = new Point(520, 300 + 30 * i);
                if (i % 2 == 0)
                    labelList[i].Location = new Point(459, 334 + 26 * ((int)i / 2));
                else
                    labelList[i].Location = new Point(706, 334 + 26 * ((int)i / 2));
                labelList[i].Visible = true;
                Controls.Add(labelList[i]);
                TextBox tb = new TextBox();
                textBoxList.Add(tb);
                if (i % 2 == 0)
                    textBoxList[i].Location = new Point(352, 334 + 26 * ((int)i/2));
                else
                    textBoxList[i].Location = new Point(600, 334 + 26 * ((int)i/2));

                textBoxList[i].Visible = true;
                textBoxList[i].Width = 100;
                textBoxList[i].Height = 20;
                labelList[i].Width = 140;
                labelList[i].Height = 13;
                Controls.Add(textBoxList[i]);
            }


            queryString = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + childTable + "' ORDER BY ORDINAL_POSITION";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {

                    connection.Open();

                    SqlCommand command = new SqlCommand(queryString, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    int i = 0;
                    while (reader.Read())
                    {

                        string colName = reader.GetString(0);
                        labelList[i].Text = colName;

                        if (colName.Equals(childPk))
                        {
                            //MessageBox.Show(colName);
                            childPkIndex = i; 
                        }
                        i++;
                    }
                    //MessageBox.Show(childPkIndex.ToString());
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }

            queryString = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + parentTable + "' ORDER BY ORDINAL_POSITION";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {

                    connection.Open();

                    SqlCommand command = new SqlCommand(queryString, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    int i = 0;
                    while (reader.Read())
                    {

                        string colName = reader.GetString(0);
                        //labelList[i].Text = colName;

                        if (colName.Equals(parentPk))
                        {
                            //MessageBox.Show(colName);
                            parentPkIndex = i;
                        }
                        i++;
                    }
                    //MessageBox.Show(parentPkIndex.ToString());
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            queryString = "select * from "+parentTable;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                //command.Parameters.AddWithValue("@p", paramValue);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        dataGridView1.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            queryString = "select * from "+childTable+" WHERE "+columnFk+"=@p";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    int paramValue = Int32.Parse(dataGridView1.Rows[e.RowIndex].Cells[parentPkIndex].Value.ToString());
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@p", paramValue);
                
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        dataGridView2.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                foreach (int i in Enumerable.Range(0, textBoxCount))
                {
                    textBoxList[i].Text = dataGridView2.Rows[e.RowIndex].Cells[i].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            queryString = "INSERT INTO "+childTable+" (";
            foreach (int i in Enumerable.Range(0, textBoxCount))
                if (i != childPkIndex)//&& textBoxList[i].Text.Length>0
                {
                    queryString = queryString + " "+labelList[i].Text+" ";
                    if (i != textBoxCount - 1)
                        queryString = queryString + " , ";
                }
            queryString = queryString + " ) VALUES ( ";
            
            //queryString = "INSERT INTO " + childTable + " VALUES (";
            foreach (int i in Enumerable.Range(0, textBoxCount))
                if (i != childPkIndex)
                {
                    queryString = queryString + "@b" + i + " ";
                    if (i != textBoxCount - 1)
                        queryString = queryString + ", ";
                }
            queryString = queryString + " )";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                //MessageBox.Show(queryString);
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    foreach (int i in Enumerable.Range(0, textBoxCount))
                        if(i!=childPkIndex)
                        {
                            if (textBoxList[i].Text.Length > 0)
                                command.Parameters.AddWithValue("@b" + i, textBoxList[i].Text);
                            else
                                command.Parameters.AddWithValue("@b"+i, DBNull.Value);
                            //command.Parameters.AddWithValue("@a" + i, labelList[i].Text);
                            
                        }
                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            queryString = "DELETE FROM " + childTable + " WHERE "+childPk+"=@childPk";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@childPk", textBoxList[childPkIndex].Text);
                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            queryString = "UPDATE "+childTable+" SET ";
            foreach (int i in Enumerable.Range(0, textBoxCount))
                if(i!=childPkIndex)
                {
                    //queryString = queryString + "@a"+i+"='@b"+i+"'";
                    queryString = queryString + " "+labelList[i].Text+"=@b"+i+" ";
                    if (i != textBoxCount - 1)
                        queryString = queryString + ", ";
                }

            queryString = queryString + " WHERE "+childPk+"=@b"+childPkIndex+" ";
            //MessageBox.Show(queryString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();

                    foreach (int i in Enumerable.Range(0, textBoxCount))
                    {
                        if(textBoxList[i].Text.Length > 0)
                            command.Parameters.AddWithValue("@b" + i, textBoxList[i].Text);
                        else
                            command.Parameters.AddWithValue("@b" + i, DBNull.Value);
                        //command.Parameters.AddWithValue("@a" + i, labelList[i].Text);
                    }
                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
