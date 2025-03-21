﻿                using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class formPenjualan: Form
    {
        public formPenjualan()
        {
            InitializeComponent();
            dataGridView1.Columns.Add("kd_produk", "KODE PRODUK");
            dataGridView1.Columns.Add("nama_produk", "NAMA PRODUK");

            dataGridView1.Columns.Add("qty", "QTY");

            dataGridView1.Columns.Add("harga_jual", "HARGA JUAL");
            dataGridView1.Columns.Add("total", "TOTAL");

            dataGridView1.set();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            using (var conn = Properti.conn())
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SqlCommand cmd = new SqlCommand("select kd_produk, nama_produk, harga_jual from [Produk] where kd_produk = @kd_produk", conn);
                    conn.Open();
                    cmd.Parameters.AddWithValue("@kd_produk", textBox1.Text);
                    DataTable dt = new DataTable();
                    SqlDataReader dr = cmd.ExecuteReader();
                    dt.Load(dr);
                    int sum = 0;
                    if (dt.Rows.Count > 0)
                    {
                        string kd_produk = dt.Rows[0][0].ToString();
                        string nama_produk = dt.Rows[0][1].ToString();
                        int qty = 1;
                        int harga_jual = Convert.ToInt32(dt.Rows[0][2]);
                        int total = qty * harga_jual;

                        

                        dataGridView1.Rows.Add(kd_produk, nama_produk, qty, harga_jual, total);



                        for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                        {
                            if(dataGridView1.Rows[i].Cells[3].Value != null)
                            {
                                sum += Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value.ToString());
                                string kodeproduk = dataGridView1.Rows[i].Cells[0].Value.ToString();
                                
                            }
                            
                        }

                        //foreach (DataRow row in dt.Rows)
                        //{
                        //    sum += Convert.ToInt32(row[3].ToString());
                        //    MessageBox.Show(row[3].ToString());
                        //}

                        textBox2.Text = sum.ToString();
                        label1.Text = "TOTAL = " + sum.ToString();
                

                    }
                    else

                    {
                        MessageBox.Show("Data yang anda cari tidak ditemukan!");
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Untuk melihat hasil kembali
            int bayar = Convert.ToInt32(textBox3.Text);
            int total = Convert.ToInt32(textBox2.Text);
            int kembali = bayar - total;
            textBox4.Text = kembali.ToString();


            //untuk meenginsert data
            using (var koneksi = Properti.conn())
            {
                int no_penjualan_awal = 1; 
                koneksi.Open();
                SqlCommand cmd = new SqlCommand("select top 1 no_penjualan from [penjualan] order by no_penjualan desc", koneksi);
                string no_penjualan_db = cmd.ExecuteScalar()?.ToString();

                if (no_penjualan_db == null)
                { 
                    SqlCommand cmdInsert = new SqlCommand("insert into penjualan (no_penjualan, tanggal_jual, kd_pelanggan, total) values (@no_penjualan, @tanggal_jual, @kd_pelanggan, @total )", koneksi);
                    cmdInsert.Parameters.AddWithValue("@no_penjualan", no_penjualan_awal);
                    cmdInsert.Parameters.AddWithValue("@tanggal_jual", DateTime.Now);
                    cmdInsert.Parameters.AddWithValue("@kd_pelanggan", "1");
                    cmdInsert.Parameters.AddWithValue("@total", textBox2.Text);
                    cmdInsert.ExecuteNonQuery();

                    SqlCommand cmdInsert2 = new SqlCommand("insert into penjualan_detail values (@no_penjualan, @kd_produk, @qty)", koneksi);
                    cmdInsert2.Parameters.AddWithValue("@no_penjualan", no_penjualan_awal);
                    cmdInsert2.Parameters.AddWithValue("@kd_produk", textBox1.Text);
                    cmdInsert2.Parameters.AddWithValue("@qty", "1");
                    cmdInsert2.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil disimpan ke penjualan dan penjualan detail!");

                }
                else
                {
                    int no_penjualan_db_int = Convert.ToInt32(no_penjualan_db)+1;
                    string no_penjualan_final = no_penjualan_db_int.ToString();
                    SqlCommand cmdInsert = new SqlCommand("insert into penjualan (no_penjualan, tanggal_jual, kd_pelanggan, total) values (@no_penjualan, @tanggal_jual, @kd_pelanggan, @total)", koneksi);
                    cmdInsert.Parameters.AddWithValue("@no_penjualan", no_penjualan_final);
                    cmdInsert.Parameters.AddWithValue("@tanggal_jual", DateTime.Now);
                    cmdInsert.Parameters.AddWithValue("@kd_pelanggan", "1");
                    cmdInsert.Parameters.AddWithValue("@total", textBox2.Text);
                    cmdInsert.ExecuteNonQuery();

                    SqlCommand cmdInsert2 = new SqlCommand("insert into penjualan_detail (no_penjualan, kd_produk, qty) values (@no_penjualan, @kd_produk, @qty)", koneksi);
                    cmdInsert2.Parameters.AddWithValue("@no_penjualan", no_penjualan_final);
                    cmdInsert2.Parameters.AddWithValue("@kd_produk", textBox1.Text);
                    cmdInsert2.Parameters.AddWithValue("@qty", "1");
                    cmdInsert2.ExecuteNonQuery();

                    MessageBox.Show("Data lain berhasil disimpan ke penjualan dan penjualan detail!");
                }

            }





            //using (var conn = Properti.conn())
            //{
            //    string data = "1";
            //    SqlCommand cek = new SqlCommand("select top 1 no_penjualan from [Penjualan] order by no_penjualan desc", conn);
            //    int cekNo = (int)cek.ExecuteScalar();

            //    if(cekNo == null)
            //    {
            //        SqlCommand tambahpertama = new SqlCommand("insert into [penjualan] values (@no_penjualan, @tanggal_jual, @kd_pelanggan, @total)", conn);
            //        tambahpertama.CommandType = CommandType.Text;
            //        tambahpertama.Parameters.AddWithValue("@kd_produk", data);
            //        tambahpertama.Parameters.AddWithValue("@tanggal_jual", data);
            //        tambahpertama.Parameters.AddWithValue("@kd_pelanggan", "1");

            //    }
            //}
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(e.RowIndex.ToString());
            //MessageBox.Show(e.ColumnIndex.ToString());
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
