using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Telphone
{
    public partial class UpdateInsertServices : Form
    {
        public NpgsqlConnection connection;
        public Tables tables;
        public int id;
        public UpdateInsertServices(NpgsqlConnection connection, Tables tables, int id)
        {
            InitializeComponent();
            this.connection = connection;
            this.tables = tables;
            this.id = id;
            if (id != 0)
                EnterInfo(id);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (id == 0)
                InsertService();
            else
                UpdateService(id);

            var service = new EnterTables(connection, tables);
            service.EnterServices();
            tables = new Tables(connection, Key.Services);
            this.Close();
        }
        public void InsertService()
        {
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("add_new_service", connection);
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@new_name", textBox1.Text);
                command.Parameters.AddWithValue("@new_price", Convert.ToInt32(textBox2.Text));
                command.Parameters.AddWithValue("@new_explain", textBox3.Text);
                command.ExecuteNonQuery();
                MessageBox.Show("Услуга добавлена!");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            finally
            {
                connection.Close();
            }
            
        }

        public void UpdateService(int id)
        {
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("update_service", connection);
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@new_id", id);
                command.Parameters.AddWithValue("@new_name", textBox1.Text);
                command.Parameters.AddWithValue("@new_price", Convert.ToInt32(textBox2.Text));
                command.Parameters.AddWithValue("@new_explain", textBox3.Text);
                command.ExecuteNonQuery();
                MessageBox.Show("Услуга обновлена!");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            finally
            {
                connection.Close();
            }
            
        }
        public void EnterInfo(int id)
        {
            var str = $"SELECT name_service, price, explain from services where id = '{id}'";
            var command = new NpgsqlCommand(str, connection);
            try
            {
                connection.Open();
                var reader = command.ExecuteReader();
                reader.Read();
                textBox1.Text = reader.GetString(0);
                textBox2.Text = reader.GetInt32(1).ToString();
                textBox3.Text = reader.GetString(2);
                reader.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            finally
            {
                connection.Close();
            }
            var serviceTable  = new EnterTables(connection, tables);
            serviceTable.EnterServices();
        }
    }
}
