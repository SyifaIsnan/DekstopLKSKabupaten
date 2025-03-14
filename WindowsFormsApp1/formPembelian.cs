using System;
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
    public partial class formPembelian: Form
    {
        public formPembelian()
        {
            InitializeComponent();
            dataGridView1.Columns.Add("kd_produk", "KODE PRODUK");
            dataGridView1.Columns.Add("nama_produk", "NAMA PRODUK");

            dataGridView1.Columns.Add("qty", "QTY");

            dataGridView1.Columns.Add("harga_modal", "HARGA BELI");
            dataGridView1.Columns.Add("total", "TOTAL");

            dataGridView1.set();
             
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
                //int no_pembelian_awal = 1;
                koneksi.Open();
                SqlCommand cmd = new SqlCommand("select top 1 no_pembelian from [pembelian] order by no_pembelian desc", koneksi);
                string no_pembelian_db = cmd.ExecuteScalar()?.ToString();

                if (no_pembelian_db == null)
                {
                  
                    SqlCommand cmdInsert = new SqlCommand("insert into pembelian values (@kd_produk, @qty, @tanggal)", koneksi);
                    //cmdInsert.Parameters.AddWithValue("@no_pembelian", no_pembelian_awal);
                    cmdInsert.Parameters.AddWithValue("@kd_produk", textBox1.Text);
                    cmdInsert.Parameters.AddWithValue("@qty", "1");
                    cmdInsert.Parameters.AddWithValue("@tanggal", DateTime.Now);
                    cmdInsert.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil disimpan ke pembelian!");

                }
                else
                {
                    SqlCommand cmdInsert = new SqlCommand("insert into pembelian values (@kd_produk, @qty, @tanggal)", koneksi);
                    //cmdInsert.Parameters.AddWithValue("@no_pembelian", no_pembelian_awal);
                    cmdInsert.Parameters.AddWithValue("@kd_produk", textBox1.Text);
                    cmdInsert.Parameters.AddWithValue("@qty", "1");
                    cmdInsert.Parameters.AddWithValue("@tanggal", DateTime.Now);
                    cmdInsert.ExecuteNonQuery();
                    MessageBox.Show("Data lain berhasil disimpan ke pembelian!");
                }

            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
           

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
                    SqlCommand cmd = new SqlCommand("select kd_produk, nama_produk, harga_modal from [Produk] where kd_produk = @kd_produk", conn);
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
                        int harga_modal = Convert.ToInt32(dt.Rows[0][2]);
                        int total = qty * harga_modal;



                        dataGridView1.Rows.Add(kd_produk, nama_produk, qty, harga_modal, total);



                        for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                        {
                            if (dataGridView1.Rows[i].Cells[3].Value != null)
                            {
                                sum += Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value.ToString());
                                string kodeproduk = dataGridView1.Rows[i].Cells[0].Value.ToString();

                            }

                        }

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

