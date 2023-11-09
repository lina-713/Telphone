using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Telphone
{
    public partial class Tables : Form
    {
        public readonly NpgsqlConnection connection;
        public readonly Key key;
        public Tables(NpgsqlConnection connection, Key key)
        {
            InitializeComponent();
            this.connection = connection;
            this.key = key;
            EnterTable();
        }
        public Tables(NpgsqlConnection connection)
        {
            InitializeComponent();
            this.connection = connection;
            EnterTable();
        }

        public void EnterTable()
        {
            switch (key)
            {
                case Key.Account:
                    var tables = new EnterTables(connection, this);
                    tables.EnterClients();
                    break;

                case Key.Services:
                    tables = new EnterTables(connection, this);
                    tables.EnterServices();
                    break;

                case Key.Status:
                    tables = new EnterTables(connection, this);
                    tables.EnterStatus();
                    break;

                case Key.IndivAccount:
                    tables = new EnterTables(connection, this);
                    tables.EnterIndivAccount();
                    break;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            switch (key)
            {
                case Key.Account:
                    var insertAccount = new UpdateInsertAccounts(connection, this, 0);
                    insertAccount.Show();
                    break;

                case Key.Services:
                    var insertServices = new UpdateInsertServices(connection, this, 0);
                    insertServices.Show();
                    break;

                case Key.Status:
                    var insertStatus = new UpdateInsertStatus(connection, this, 0);
                    insertStatus.Show();
                    break;

                case Key.IndivAccount:
                    var insertIndivAcc = new UpdateInsertIndivAcc(connection, this, 0);
                    insertIndivAcc.Show();
                    break;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            switch (key)
            {
                case Key.Account:
                    var updateAccount = new UpdateInsertAccounts(connection, this, id);
                    updateAccount.Show();
                    break;

                case Key.Services:
                    var updateServices = new UpdateInsertServices(connection, this, id);
                    updateServices.Show();
                    break;

                case Key.Status:
                    var updateStatus = new UpdateInsertStatus(connection, this, id);
                    updateStatus.Show();
                    break;

                case Key.IndivAccount:
                    var updateIndivAcc = new UpdateInsertIndivAcc(connection, this, id);
                    updateIndivAcc.Show();
                    break;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var tables = new EnterTables(connection, this);
            
            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            switch (key)
            {
                case Key.Account:
                    var result = MessageBox.Show("Вы действительно хотите удалить данного спортсмена?", "Удаление", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        connection.Open();
                        var command = new NpgsqlCommand("delete_subs", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@new_id", id);
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Игрок удален!");
                    }
                    tables.EnterClients();
                    break;

                case Key.Services:
                    result = MessageBox.Show("Вы действительно хотите удалить данного спортсмена?", "Удаление", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        connection.Open();
                        var command = new NpgsqlCommand("delete_service", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@new_id", id);
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Игрок удален!");
                    }
                    tables.EnterServices();
                    break;

                case Key.Status:
                    result = MessageBox.Show("Вы действительно хотите удалить данного спортсмена?", "Удаление", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        connection.Open();
                        var command = new NpgsqlCommand("delete_status", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@new_id", id);
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Игрок удален!");
                    }
                    tables.EnterStatus();
                    break;

                case Key.IndivAccount:
                    result = MessageBox.Show("Вы действительно хотите удалить данного спортсмена?", "Удаление", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        connection.Open();
                        NpgsqlCommand command = new NpgsqlCommand("delete_indacc", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@new_id", id);
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Игрок удален!");
                    }
                    tables.EnterIndivAccount();
                    break;
            }
        }
    }
}
