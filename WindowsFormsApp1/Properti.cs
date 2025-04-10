using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class Properti
    {

        public static SqlConnection conn()
        {
            return new SqlConnection("Data Source=DESKTOP-DHE9K9C;Initial Catalog=LatihanLKS;Integrated Security=True;TrustServerCertificate=True");
        }

        public static bool validasi(Control.ControlCollection container, TextBox kosong = null)
        {
            foreach (Control c in container)
            {
                if (c is TextBoxBase textBox && string.IsNullOrWhiteSpace(textBox.Text) && textBox != kosong)
                {
                    return true;
                }
            }
            return false;
        }

        public static string enkripsi(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));

            }
            return sb.ToString();
        }

    }
}
