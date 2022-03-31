﻿using System;
using System.Data.OleDb;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=D:\\Documents\\Desktop\\Plan.mdb";
        public int act_table = 1;
        public Form1()
        {
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            string CommandText = "SELECT " + "[Перевозка].[Номер], " + "[Маршрут].[Номер маршрута], " + "[Маршрут].[Пункт назначения], " + "[Маршрут].[Время отправки], " + "[Маршрут].[Время прибытия], " + "[Билет].[Место], " + "[Билет].[Ф_И_О], " + "[Билет].[Стоимость], " + "[Водитель].[Ф_И_О] " + "FROM " + "[Перевозка], " + "[Маршрут], " + "[Билет], " + "[Водитель] " + "WHERE " + "([Перевозка].[ID_Marshrut]=[Маршрут].[ID_Marshrut]) AND " + "([Перевозка].[ID_Bilet] = [Билет].[ID_Bilet]) AND " + "([Перевозка].[ID_Voditel] = [Водитель].[ID_Voditel]) ";
            if (textBox1.Text != "")
            {
                if (comboBox1.SelectedIndex == 0)
                    CommandText = CommandText + " AND ([Перевозка].[Номер] = '" + textBox1.Text + "')";
                if (comboBox1.SelectedIndex == 1)
                    CommandText = CommandText + " AND (Маршрут.[Номер маршрута] = '" + textBox1.Text + "') ";

                if (comboBox1.SelectedIndex == 2)
                    CommandText = CommandText + " AND (Маршрут.[Пункт назначения] LIKE '" + textBox1.Text + "%') ";
                if (comboBox1.SelectedIndex == 3)
                    CommandText = CommandText + " AND (Билет.[Ф_И_О] LIKE '" + textBox1.Text + "%') ";
                if (comboBox1.SelectedIndex == 4)
                    CommandText = CommandText + " AND ([Водитель].[Ф_И_О] LIKE '" + textBox1.Text + "%') ";
            }

            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(CommandText, ConnectionString);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "[Перевозка]");
            dataGridView1.DataSource = ds.Tables["[Перевозка]"].DefaultView;
        }
        public void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            button1_Click(sender, e);
            Get_Bilets();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string CommandText;
            string num_per, ID_M, ID_B, ID_D, ID_A, ID_V;
            int row;
            Form2 f = new Form2();

            if (f.ShowDialog() == DialogResult.OK)
            {
                if (f.textBox1.Text == "") num_per = "0";
                else num_per = f.textBox1.Text;
                row = f.dataGridView1.CurrentCell.RowIndex;
                ID_M = Convert.ToString(f.dataGridView1[0, row].Value);
                row = f.dataGridView2.CurrentCell.RowIndex;
                ID_B = Convert.ToString(f.dataGridView2[0, row].Value);
                row = f.dataGridView3.CurrentCell.RowIndex;
                ID_D = Convert.ToString(f.dataGridView3[0, row].Value);
                row = f.dataGridView4.CurrentCell.RowIndex;
                ID_A = Convert.ToString(f.dataGridView4[0, row].Value);
                row = f.dataGridView5.CurrentCell.RowIndex;
                ID_V = Convert.ToString(f.dataGridView5[0, row].Value);
                CommandText = "INSERT INTO [Перевозка] (Номер, ID_Marshrut, ID_Bilet, ID_Dispetcher, ID_Avtobus, ID_Voditel) " + "VALUES (" + num_per + ", " + ID_M + ", " + ID_B + ", " + ID_D + ", " + ID_A + ", " + ID_V + ")";
                My_Execute_Non_Query(CommandText);
                button1_Click(sender, e);
            }
        }
        public void My_Execute_Non_Query(string CommandText)
        {
            OleDbConnection conn = new OleDbConnection(ConnectionString);
            conn.Open();
            OleDbCommand myCommand = conn.CreateCommand();
            myCommand.CommandText = CommandText;
            myCommand.ExecuteNonQuery();
            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();

            if (f.ShowDialog() == DialogResult.OK)
            {
                int index, index_old;
                string ID;
                string CommandText = "DELETE FROM ";
                index = dataGridView1.CurrentRow.Index; 
                index_old = index;
                ID = Convert.ToString(dataGridView1[0, index].Value); 
                CommandText = "DELETE FROM [Перевозка] WHERE [Перевозка].[Номер] = '" + ID + "'";
                My_Execute_Non_Query(CommandText);
                button1_Click(sender, e);

                if (index_old >= 0)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1[0, index_old].Selected = true;
                }
            }
        }
        private void Get_Bilets()  
        {
            string CommandText = "SELECT ID_Bilet, [Место], [Стоимость], [Время], [Ф_И_О], [Паспорт], [Льготы] FROM [Билет]";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(CommandText, ConnectionString);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "[Билет]");
            dataGridView2.DataSource = ds.Tables[0].DefaultView;
            dataGridView2.Columns[0].Visible = false; 
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Get_Bilets();
            act_table = 1;
        }
        private void Get_Marshruts()  
        {
            string CommandText = "SELECT * FROM [Маршрут]";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(CommandText, ConnectionString);
            DataSet ds = new DataSet();  
            dataAdapter.Fill(ds, "[Маршрут]");
            dataGridView2.DataSource = ds.Tables[0].DefaultView;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Get_Marshruts();
            act_table = 2;
        }
        private void Get_Avtobus()  
        {
            string CommandText = "SELECT * FROM Автобус";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(CommandText, ConnectionString);
            DataSet ds = new DataSet(); 
            dataAdapter.Fill(ds, "Автобус"); 
            dataGridView2.DataSource = ds.Tables[0].DefaultView;
            dataGridView2.Columns[0].Visible = false; 
        }
        private void button6_Click(object sender, EventArgs e)
        {
            Get_Avtobus();
            act_table = 3;
        }
        private void Get_Voditel() 
        {
            string CommandText = "SELECT * FROM Водитель";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(CommandText, ConnectionString);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Водитель");
            dataGridView2.DataSource = ds.Tables[0].DefaultView;
            dataGridView2.Columns[0].Visible = false;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            Get_Voditel();
            act_table = 4;
        }
        private void Get_Dispetcher()
        {
            string CommandText = "SELECT * FROM [Диспетчер]";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(CommandText, ConnectionString);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Диспетчер");
            dataGridView2.DataSource = ds.Tables[0].DefaultView;
            dataGridView2.Columns[0].Visible = false;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            Get_Dispetcher();
            act_table = 5;
        }
        private void Add_Bilet(string mesto, string stoimost, DateTime vremja, string name, string passport, bool lgota)
        {
            string CommandText;
            string s_vremja;
            string s_stoimost;
            s_vremja = Convert.ToString(vremja); 
            s_stoimost = stoimost.Replace(',', '.'); 
            CommandText = "INSERT INTO [Билет] (Место, Стоимость, [Время], [Ф_И_О], Паспорт, Льготы) " + "VALUES ('" + mesto + "', " + s_stoimost + ", '" + s_vremja + "', '"
            + name + "', '" + passport + "', " + lgota + ")";
            My_Execute_Non_Query(CommandText);
        }
        private void Add_Marshrut(string num_marsh, string punkt, double rasst, DateTime vremja_otpr, DateTime vremja_prib)
        {
            string CommandText;
            string s_otpr, s_prib;
            string s_ves, s_rasst;
            s_otpr = Convert.ToString(vremja_otpr); 
            s_prib = Convert.ToString(vremja_prib); 
            
           
            s_rasst = Convert.ToString(rasst); 
            s_rasst = s_rasst.Replace(',', '.'); 

            CommandText = "INSERT INTO [Маршрут] ([Номер маршрута], [Пункт назначения], Район, Область, Расстояние, Вес, [Время отправки], [Время прибытия])"
            + " VALUES ('" + num_marsh + "', '" + punkt + "', '"  
            + s_rasst + ", '" + s_otpr + "', '" + s_prib + "')";
            My_Execute_Non_Query(CommandText);
        }
        void Add_Avtobus(string num, string model, string znak, string k_mest)
        {
            string CommandText;
            CommandText = "INSERT INTO [Автобус] ([Номер], [Модель], [Номерной знак], [Количество мест])"
            + " VALUES ('" + num + "', '" + model + "', '" + znak + "', " + k_mest + ")";
            My_Execute_Non_Query(CommandText);
        }
        void Add_Voditel(string f_i_o, string d_r, string passport) 
        {
            string CommandText;
            CommandText = "INSERT INTO [Водитель] ([Ф_И_О], [Дата рождения], [Паспорт])"
            + " VALUES ('" + f_i_o + "', '" + d_r + "', '" + passport + "')";
            My_Execute_Non_Query(CommandText);
        }
        void Add_Dispetcher(string f_i_o, string d_r, string adres) 
        {
            string CommandText;
            CommandText = "INSERT INTO [Диспетчер] ([Ф_И_О], [Дата рождения], [Адрес])"
            + " VALUES ('" + f_i_o + "', '" + d_r + "', '" + adres + "')";
            My_Execute_Non_Query(CommandText);
        }
        private void button10_Click(object sender, EventArgs e)
        {
            if (act_table == 1) 
            {
                Form4 f = new Form4();

                if (f.ShowDialog() == DialogResult.OK)
                {
                    Add_Bilet(f.textBox1.Text, f.textBox2.Text, Convert.ToDateTime(f.textBox3.Text),
         f.textBox4.Text, f.textBox5.Text, f.checkBox1.Checked);
                    Get_Bilets();
                }
            }
            else
            if (act_table == 2) 
            {
                Form5 f = new Form5();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    Add_Marshrut(f.textBox1.Text, f.textBox2.Text, 
                      Convert.ToDouble(f.textBox5.Text),
         f.dateTimePicker1.Value, f.dateTimePicker2.Value);
                    Get_Marshruts();
                }
            }
            else
            if (act_table == 3)
            {
                Form6 f = new Form6();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    Add_Avtobus(f.textBox1.Text, f.textBox2.Text, f.textBox3.Text, f.textBox4.Text);
                    Get_Avtobus();
                }
            }
            else
            if (act_table == 4) 
            {
                Form7 f = new Form7();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    Add_Voditel(f.textBox1.Text, Convert.ToString(f.dateTimePicker1.Value), f.textBox2.Text);
                    Get_Voditel();
                }
            }
            else
            if (act_table == 5) 
            {
                Form8 f = new Form8();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    Add_Dispetcher(f.textBox1.Text, Convert.ToString(f.dateTimePicker1.Value), f.textBox2.Text);
                    Get_Dispetcher();
                }
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.Text = " ";
            if (f.ShowDialog() == DialogResult.OK)
            {
                int index, index_old;
                string ID;
                string CommandText = "DELETE FROM ";
                index = dataGridView2.CurrentRow.Index; 
                index_old = index;
                ID = Convert.ToString(dataGridView2[0, index].Value); 
                if (act_table == 1) 
                    CommandText = "DELETE FROM Билет WHERE Билет.ID_Bilet = " + ID;
                if (act_table == 2)
                    CommandText = "DELETE FROM Маршрут WHERE Маршрут.ID_Marshrut = " + ID;
                if (act_table == 3)
                    CommandText = "DELETE FROM Автобус WHERE Автобус.ID_Avtobus = " + ID;
                if (act_table == 4)
                    CommandText = "DELETE FROM Водитель WHERE Водитель.ID_Voditel = " + ID;
                if (act_table == 5) 
                    CommandText = "DELETE FROM Диспетчер WHERE Диспетчер.ID_Dispetcher = " + ID;
                My_Execute_Non_Query(CommandText);
                if (act_table == 1) Get_Bilets();
                else
                if (act_table == 2) Get_Marshruts();
                else
                if (act_table == 3) Get_Avtobus();
                else
                if (act_table == 4) Get_Voditel();
                else
                if (act_table == 5) Get_Dispetcher();
                if (index_old >= 0)
                {
                    dataGridView2.ClearSelection();
                    dataGridView2[0, index_old].Selected = true;
                }
            }
        }
    }
}