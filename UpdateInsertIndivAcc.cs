using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class UpdateInsertIndivAcc : Form
    {
        public NpgsqlConnection connection;
        public Tables tables;
        public int id;
        public UpdateInsertIndivAcc(NpgsqlConnection connection, Tables tables, int id)
        {
            InitializeComponent();
            this.connection = connection;
            this.tables = tables;
            this.id = id;
            SubsDictionary();
            ServiceDictionary();
            if(id != 0)
                EnterInfo(id);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (id == 0)
                InsertIndivAcc();
            else
                UpdateIndivAcc(id);

            var enterTables = new EnterTables(connection, tables);
            enterTables.EnterIndivAccount();
            tables = new Tables(connection, Key.IndivAccount);
            this.Close();
        }

        private void EnterInfo(int id)
        {
            var str = $"select id_sub, serv from individual_account where id = {id}";
            var command = new NpgsqlCommand(str, connection);
            try
            {
                connection.Open();
                var reader = command.ExecuteReader();
                reader.Read();
                comboBox1.SelectedIndex = reader.GetInt32(0) - 1;
                comboBox2.SelectedIndex = reader.GetInt32(1) - 1;
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
            var indacc = new EnterTables(connection, tables);
            indacc.EnterIndivAccount();
        }
        private void InsertIndivAcc()
        {
            connection.Open();
            var command = new NpgsqlCommand("add_new_indacc", connection);
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@new_id_sub", comboBox1.SelectedIndex + 1);
                command.Parameters.AddWithValue("@new_serv", comboBox2.SelectedIndex + 1);
                command.ExecuteNonQuery();
                MessageBox.Show("Аккаунт изменен!");
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
        private void UpdateIndivAcc(int id)
        {
            connection.Open();
            var command = new NpgsqlCommand("update_indacc", connection);
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@new_id", id);
                command.Parameters.AddWithValue("@new_id_sub", comboBox1.SelectedIndex + 1);
                command.Parameters.AddWithValue("@new_serv", comboBox2.SelectedIndex + 1);
                command.ExecuteNonQuery();
                MessageBox.Show("Аккаунт изменен!");
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

        public void SubsDictionary()
        {
            string str = "SELECT account_number FROM subscribers";
            var accNumberList = Accounts.ComboBigInt(connection, str);
            var dictionaries = new ObservableCollection<StatusDictionary>();
            accNumberList.ForEach(Name => dictionaries.Add(new StatusDictionary() { IKey = String.Empty, IValue = Name }));
            comboBox1.DataSource = dictionaries.ToList();
        }


        public void ServiceDictionary()
        {
            string str = "SELECT name_service FROM services";
            var serviceList = Accounts.ComboString(connection, str);
            var dictionaries = new ObservableCollection<StatusDictionary>();
            serviceList.ForEach(Name => dictionaries.Add(new StatusDictionary() { IKey = String.Empty, IValue = Name }));
            comboBox2.DataSource = dictionaries.ToList();
        }
    }
}
