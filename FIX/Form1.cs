using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FIX
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=LAPTOP-I0H7METT\\CHESTAYURCEL;Initial Catalog=keuangan;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void ClearForm()
        {
            txtNIM.Clear();
            txtNama.Clear();
            txtProdi.Clear();
            txtCabor.Clear();
            txtAngkatan.Clear();
            txtNIM.Focus();
        }

        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT NIM, Nama, Prodi, Angkatan, Cabor FROM Atlit";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvMahasiswa.AutoGenerateColumns = true;
                    dgvMahasiswa.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memuat data: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO Atlit (NIM, Nama, Prodi, Angkatan, Cabor) VALUES (@NIM, @Nama, @Prodi, @Angkatan, @Cabor)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                    cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                    cmd.Parameters.AddWithValue("@Prodi", txtProdi.Text);
                    cmd.Parameters.AddWithValue("@Angkatan", txtAngkatan.Text);
                    cmd.Parameters.AddWithValue("@Cabor", txtCabor.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil ditambahkan.");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menambahkan data: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE Atlit SET Nama = @Nama, Prodi = @Prodi, Angkatan = @Angkatan, Cabor = @Cabor WHERE NIM = @NIM";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                    cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                    cmd.Parameters.AddWithValue("@Prodi", txtProdi.Text);
                    cmd.Parameters.AddWithValue("@Angkatan", txtAngkatan.Text);
                    cmd.Parameters.AddWithValue("@Cabor", txtCabor.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil diperbarui.");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memperbarui data: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Atlit WHERE NIM = @NIM";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil dihapus.");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menghapus data: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void dgvMahasiswa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvMahasiswa.Rows[e.RowIndex].Cells.Count >= 5)
            {
                DataGridViewRow row = dgvMahasiswa.Rows[e.RowIndex];
                txtNIM.Text = row.Cells[0].Value?.ToString();
                txtNama.Text = row.Cells[1].Value?.ToString();
                txtProdi.Text = row.Cells[2].Value?.ToString();
                txtAngkatan.Text = row.Cells[3].Value?.ToString();
                txtCabor.Text = row.Cells[4].Value?.ToString();
            }
        }

        private void Nama_Click(object sender, EventArgs e)
        {

        }

    }
}
