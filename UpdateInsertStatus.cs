using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Telphone
{
    public partial class UpdateInsertStatus : Form
    {
        public NpgsqlConnection connection;
        public Tables tables;
        public int id;

        public UpdateInsertStatus(NpgsqlConnection connection, Tables tables, int id)
        {
            InitializeComponent();
            this.connection = connection;
            this.tables = tables;
            this.id = id;
            if(id != 0)
                EnterInfo(id);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (id == 0)
                InsertStatus();
            else
                UpdateStatus(id);

            var enterTables = new EnterTables(connection, tables);
            enterTables.EnterStatus();
            tables = new Tables(connection, Key.Status);
            this.Close();
        }
        private void InsertStatus()
        {
            connection.Open();
            var command = new NpgsqlCommand("add_new_status", connection);
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@new_name", textBox1.Text);
                command.ExecuteNonQuery();
                MessageBox.Show("Статус добавлен!");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private void UpdateStatus(int id)
        {
            connection.Open();
            var command = new NpgsqlCommand("update_status", connection);
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@new_id", id);
                command.Parameters.AddWithValue("@new_name", textBox1.Text);
                command.ExecuteNonQuery();
                MessageBox.Show("Статус изменен!");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private void EnterInfo(int id)
        {
            var str = $"SELECT name from status where id = '{id}'";
            var command = new NpgsqlCommand(str, connection);
            try
            {
                connection.Open();
                var reader = command.ExecuteReader();
                reader.Read();
                textBox1.Text = reader.GetString(0);
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
            var statustable = new EnterTables(connection, tables);
            statustable.EnterStatus();
        }
    }
}
