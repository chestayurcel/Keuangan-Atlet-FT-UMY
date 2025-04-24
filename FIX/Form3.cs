using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ucp1
{
    public partial class Form3 : Form
    {
        private string connectionString = "Data Source=LAPTOP-I0H7METT\\CHESTAYURCEL;Initial Catalog=keuangan;Integrated Security=True";

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ClearForm()
        {
            txtid_Prestasi.Clear(); // hanya untuk display
            txtNama_Prestasi.Clear();
            txttingkat_Prestasi.Clear();
            txttahun_Prestasi.Clear();
            txtNama_Prestasi.Focus();
        }

        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT id_Prestasi, Nama_Prestasi, tingkat_Prestasi, tahun_Prestasi FROM Prestasi";
                    SqlDataAdapter da = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvPrestasi.AutoGenerateColumns = true;
                    dgvPrestasi.DataSource = dt;

                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool IsFormValid()
        {
            return !(string.IsNullOrWhiteSpace(txtNama_Prestasi.Text) ||
                     string.IsNullOrWhiteSpace(txttingkat_Prestasi.Text) ||
                     string.IsNullOrWhiteSpace(txttahun_Prestasi.Text));
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!IsFormValid())
            {
                MessageBox.Show("Harap isi semua data!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txttahun_Prestasi.Text.Trim(), out int tahun))
            {
                MessageBox.Show("Tahun prestasi harus berupa angka.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Prestasi (Nama_Prestasi, tingkat_Prestasi, tahun_Prestasi) " +
                                   "VALUES (@nama, @tingkat, @tahun)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nama", txtNama_Prestasi.Text.Trim());
                        cmd.Parameters.AddWithValue("@tingkat", txttingkat_Prestasi.Text.Trim());
                        cmd.Parameters.AddWithValue("@tahun", tahun);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Data berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Data gagal ditambahkan!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPrestasi.SelectedRows.Count > 0)
            {
                DialogResult confirm = MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        try
                        {
                            string id = dgvPrestasi.SelectedRows[0].Cells["id_Prestasi"].Value.ToString();
                            conn.Open();
                            string query = "DELETE FROM Prestasi WHERE id_Prestasi = @id";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@id", id);

                            int rows = cmd.ExecuteNonQuery();
                            if (rows > 0)
                            {
                                MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Data gagal dihapus!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Harap pilih data yang akan dihapus.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            MessageBox.Show($"Jumlah Kolom: {dgvPrestasi.ColumnCount}\nJumlah Baris: {dgvPrestasi.RowCount}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvPrestasi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPrestasi.Rows[e.RowIndex];

                txtid_Prestasi.Text = row.Cells["id_Prestasi"].Value?.ToString();
                txtNama_Prestasi.Text = row.Cells["Nama_Prestasi"].Value?.ToString();
                txttingkat_Prestasi.Text = row.Cells["tingkat_Prestasi"].Value?.ToString();
                txttahun_Prestasi.Text = row.Cells["tahun_Prestasi"].Value?.ToString();
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Pastikan ada data yang dipilih di DataGridView
            if (string.IsNullOrWhiteSpace(txtid_Prestasi.Text))
            {
                MessageBox.Show("Harap pilih data yang akan diperbarui dari tabel.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi form
            if (!IsFormValid())
            {
                MessageBox.Show("Harap isi semua data!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi Tahun harus berupa angka
            if (!int.TryParse(txttahun_Prestasi.Text.Trim(), out int tahun))
            {
                MessageBox.Show("Tahun prestasi harus berupa angka.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Proses Update ke database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Prestasi SET Nama_Prestasi = @nama, tingkat_Prestasi = @tingkat, tahun_Prestasi = @tahun WHERE id_Prestasi = @id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nama", txtNama_Prestasi.Text.Trim());
                        cmd.Parameters.AddWithValue("@tingkat", txttingkat_Prestasi.Text.Trim());
                        cmd.Parameters.AddWithValue("@tahun", tahun);
                        cmd.Parameters.AddWithValue("@id", txtid_Prestasi.Text.Trim()); // ID yang digunakan untuk update

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Data berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData(); // Reload data setelah update
                        }
                        else
                        {
                            MessageBox.Show("Data gagal diperbarui!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
