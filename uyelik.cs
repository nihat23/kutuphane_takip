using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.OleDb;//Acces kutuphanemızı cagırıyoruz..

namespace KütüpHane_Program
{
    public partial class uyelik : Form
    {
        public uyelik()
        {
            InitializeComponent();
        }

        OleDbConnection baglan = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Kayit_Bilgiler.accdb");
        OleDbCommand komutver;
        OleDbDataAdapter liste;
        OleDbDataReader sorgu_oku;
        DataTable tablo;

        //**Kitap iade**
        void Kitap_iade_Sayfasi()
        {
            baglan.Open();
            liste = new OleDbDataAdapter("select tc AS[TC NO],AdiSoyadi AS[ADI SOYADI],Cinsiyet AS[CİNSIYET],Telefon AS[TELEFON],Adres AS[ADRESİ],KitapAdi AS[KITAP ADI],YazarAdi AS[YAZAR ADI],AlisTarih AS[ALIŞ TARİHİ],iadeTarih AS[İADE TARİHİ],Teslim AS[TESLİM DURUMU] from Kayit_Bilgiler", baglan);
            tablo = new DataTable();
            liste.Fill(tablo);

            dataGridView1Kitap_iade.DataSource = tablo;
            baglan.Close();

        }
        void Kitap_iade_Sayfasi_temizle()
        {
            textBox1Tc.Clear();
            textBox2AdiSoyadi.Clear();
            textBox4Telefon.Clear();
            textBox5Adres.Clear();
            textBox2Y_Adi.Clear();

            /*
         for (int i = 0; i <groupBox1.Controls.Count; i++)
         {
             if(groupBox1.Controls[i] is TextBox)
             {
                 groupBox1.Controls[i].Text = "";
             }
         }
            */
        }

        //**kitap kayıt**
        void Kitap_Kayit_Sayfasi()
        {
            baglan.Open();
            liste = new OleDbDataAdapter("select id AS[SIRA NO], K_Adi AS[KİTAP ADI],K_Yazar AS[YAZAR ADI],K_Nerede AS[KİTAP NEREDE] from KitapKayitYap", baglan);
            tablo = new DataTable();
            liste.Fill(tablo);

            dataGridView2Kitap_Kayit.DataSource = tablo;
            baglan.Close();
        }
        void Kitap_Kayit_Sayfasi_temizle()
        {
            textBox1KitapAdi.Clear();
            textBox3KitapNerede.Clear();
            textBox2KitapYazari.Clear();
        }

        void iade_topla()
        {
            baglan.Open();
            komutver = new OleDbCommand("select count(tc) from Kayit_Bilgiler ", baglan);
            label16iadeTopla.Text = komutver.ExecuteScalar().ToString();
            baglan.Close();
        }

        private void button1Ekle_Click(object sender, EventArgs e)
        {
            bool kayit_varmi = false;

            baglan.Open();
            komutver = new OleDbCommand("select *from Kayit_Bilgiler where tc='" + textBox1Tc.Text + "'", baglan);
            sorgu_oku = komutver.ExecuteReader();
            while (sorgu_oku.Read())
            {
                kayit_varmi = true;
                MessageBox.Show("Girdiginiz TC Numarası kayıt var..", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                break;
            }
            baglan.Close();

            if (kayit_varmi == false)
            {
                if (textBox1Tc.Text != "" && textBox2AdiSoyadi.Text != "" && textBox5Adres.Text != "" && comboBox1Cinsiyet.Text != "" && textBox4Telefon.Text != "" && comboBox1K_Adi.Text != "" && textBox2Y_Adi.Text != "" && dateTimePicker1Alis.Text != "" && dateTimePicker2iade.Text != "" && comboBox1Teslim_Durumu.Text != "")
                {
                    if (textBox1Tc.Text.Length == 11)
                    {
                        baglan.Open();
                        komutver = new OleDbCommand("insert into Kayit_Bilgiler(tc,AdiSoyadi,Cinsiyet,Telefon,Adres,KitapAdi,YazarAdi,AlisTarih,iadeTarih,Teslim) values(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10)", baglan);
                        komutver.Parameters.AddWithValue("@p1", textBox1Tc.Text);
                        komutver.Parameters.AddWithValue("@p2", textBox2AdiSoyadi.Text);
                        komutver.Parameters.AddWithValue("@p3", comboBox1Cinsiyet.Text);
                        komutver.Parameters.AddWithValue("@p4", textBox4Telefon.Text);
                        komutver.Parameters.AddWithValue("@p5", textBox5Adres.Text);
                        komutver.Parameters.AddWithValue("@p6", comboBox1K_Adi.Text);
                        komutver.Parameters.AddWithValue("@p7", textBox2Y_Adi.Text);
                        komutver.Parameters.AddWithValue("@p8", dateTimePicker1Alis.Value.ToShortDateString());
                        komutver.Parameters.AddWithValue("@p9", dateTimePicker2iade.Value.ToShortDateString());
                        komutver.Parameters.AddWithValue("@p10", comboBox1Teslim_Durumu.Text);
                        komutver.ExecuteNonQuery();
                        baglan.Close();

                        MessageBox.Show("Kayıt Listeye Eklendi..", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Kitap_iade_Sayfasi_temizle();
                        Kitap_iade_Sayfasi();
                        iade_topla();
                    }
                    else
                    {
                        MessageBox.Show("Tc Numarası 11 Rakam olmalıdır..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }

                }
                else
                {
                    MessageBox.Show("Alanları Boş Geçmeyiniz..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }


            }


        }

        private void uyelik_Load(object sender, EventArgs e)
        {

            Kitap_iade_Sayfasi();
            Kitap_Kayit_Sayfasi();

            baglan.Open();
            komutver = new OleDbCommand("select K_Adi from KitapKayitYap", baglan);
            sorgu_oku = komutver.ExecuteReader();
            while (sorgu_oku.Read())
            {

                comboBox1K_Adi.Items.Add(sorgu_oku["K_Adi"].ToString());

            }
            baglan.Close();

            iade_topla();

            /*
            dataGridView1Kitap_Cikis.Columns[0].ReadOnly = true;
            dataGridView1Kitap_Cikis.Columns[1].ReadOnly = true;
            dataGridView1Kitap_Cikis.Columns[2].ReadOnly = true;
            dataGridView1Kitap_Cikis.Columns[3].ReadOnly = true;
            dataGridView1Kitap_Cikis.Columns[4].ReadOnly = true;
            dataGridView1Kitap_Cikis.Columns[5].ReadOnly = true;
            dataGridView1Kitap_Cikis.Columns[6].ReadOnly = true;
            dataGridView1Kitap_Cikis.Columns[7].ReadOnly = true;
            dataGridView1Kitap_Cikis.Columns[8].ReadOnly = true;
            */
        }

        private void button2Sil_Click(object sender, EventArgs e)
        {
            DialogResult cvp = MessageBox.Show("Silmek istediginizden eminmisiniz..?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (cvp == DialogResult.Yes)
            {
                if (textBox1Tc.Text != "")
                {
                    baglan.Open();
                    komutver = new OleDbCommand("delete from Kayit_Bilgiler where tc='" + textBox1Tc.Text + "' ", baglan);
                    komutver.ExecuteNonQuery();
                    baglan.Close();
                    MessageBox.Show("Belirtilen Tc Numarası Silindi..", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Kitap_iade_Sayfasi();
                    Kitap_iade_Sayfasi_temizle();
                    iade_topla();
                }
                else
                {
                    MessageBox.Show("Silinecek TC Numarasını Giriniz..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            if (cvp == DialogResult.No)
            {
                Kitap_iade_Sayfasi();
                Kitap_iade_Sayfasi_temizle();

            }

        }

        private void dataGridView1Kitap_Cikis_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1Kitap_iade.SelectedCells[0].RowIndex;
            textBox1Tc.Text = dataGridView1Kitap_iade.Rows[secilen].Cells[0].Value.ToString();
            textBox2AdiSoyadi.Text = dataGridView1Kitap_iade.Rows[secilen].Cells[1].Value.ToString();
            comboBox1Cinsiyet.Text = dataGridView1Kitap_iade.Rows[secilen].Cells[2].Value.ToString();
            textBox4Telefon.Text = dataGridView1Kitap_iade.Rows[secilen].Cells[3].Value.ToString();
            textBox5Adres.Text = dataGridView1Kitap_iade.Rows[secilen].Cells[4].Value.ToString();
            comboBox1K_Adi.Text = dataGridView1Kitap_iade.Rows[secilen].Cells[5].Value.ToString();
            textBox2Y_Adi.Text = dataGridView1Kitap_iade.Rows[secilen].Cells[6].Value.ToString();
            dateTimePicker1Alis.Text = dataGridView1Kitap_iade.Rows[secilen].Cells[7].Value.ToString();
            dateTimePicker2iade.Text = dataGridView1Kitap_iade.Rows[secilen].Cells[8].Value.ToString();
            comboBox1Teslim_Durumu.Text = dataGridView1Kitap_iade.Rows[secilen].Cells[9].Value.ToString();
        }

        private void button3Duzelt_Click(object sender, EventArgs e)
        {
            if (textBox1Tc.Text != "" && textBox2AdiSoyadi.Text != "" && textBox5Adres.Text != "" && comboBox1Cinsiyet.Text != "" && textBox4Telefon.Text != "" && comboBox1K_Adi.Text != "" && textBox2Y_Adi.Text != "" && dateTimePicker1Alis.Text != "" && dateTimePicker2iade.Text != "" && comboBox1Teslim_Durumu.Text != "")
            {
                baglan.Open();
                komutver = new OleDbCommand("update Kayit_Bilgiler set AdiSoyadi=@p1,Cinsiyet=@p2,Telefon=@p3,Adres=@p4,KitapAdi=@p5,YazarAdi=@p6,AlisTarih=@p7,iadeTarih=@p8,Teslim=@p10 where tc=@p9", baglan);
                komutver.Parameters.AddWithValue("@p1", textBox2AdiSoyadi.Text);
                komutver.Parameters.AddWithValue("@p2", comboBox1Cinsiyet.Text);
                komutver.Parameters.AddWithValue("@p3", textBox4Telefon.Text);
                komutver.Parameters.AddWithValue("@p4", textBox5Adres.Text);
                komutver.Parameters.AddWithValue("@p5", comboBox1K_Adi.Text);
                komutver.Parameters.AddWithValue("@p6", textBox2Y_Adi.Text);
                komutver.Parameters.AddWithValue("@p7", dateTimePicker1Alis.Value.ToShortDateString());
                komutver.Parameters.AddWithValue("@p8", dateTimePicker2iade.Value.ToShortDateString());
                komutver.Parameters.AddWithValue("@p10", comboBox1Teslim_Durumu.Text);
                komutver.Parameters.AddWithValue("@p9", textBox1Tc.Text);//where tc
                komutver.ExecuteNonQuery();
                baglan.Close();

                MessageBox.Show("Bilgiler GünCellendi..", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Kitap_iade_Sayfasi();

            }
            else
            {
                MessageBox.Show("Lütfen Boş Alan Bırakmayınız..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void textBox1Tc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > 126 || e.KeyChar < 58)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void button5Temizle_Click(object sender, EventArgs e)
        {
            Kitap_iade_Sayfasi_temizle();

        }

        private void textBox4Telefon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > 126 || e.KeyChar < 58)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void button1AramaYap_Click(object sender, EventArgs e)
        {
            if (textBox1Tc.Text != "")
            {
                if (textBox1Tc.Text != "" || comboBox1K_Adi.Text != "")
                {
                    baglan.Open();                                                                                                                                                                                                                                                                            //      where KitapAdi like '" + comboBox1K_Adi.Text + "%'", baglan);  
                    liste = new OleDbDataAdapter("select  tc AS[TC NO],AdiSoyadi AS[ADI SOYADI],Cinsiyet AS[CİNSIYET],Telefon AS[TELEFON],Adres AS[ADRESİ],KitapAdi AS[KITAP ADI],YazarAdi AS[YAZAR ADI],AlisTarih AS[ALIŞ TARİHİ],iadeTarih AS[İADE TARİHİ],Teslim AS[TESLİM DURUMU] from Kayit_Bilgiler where tc like '" + textBox1Tc.Text + "%'", baglan);
                    tablo = new DataTable();
                    liste.Fill(tablo);

                    dataGridView1Kitap_iade.DataSource = tablo;
                    baglan.Close();
                }
                else
                {
                    MessageBox.Show("Lütfen aramak istediginiz TC numarasını giriniz..", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            else
            {
                MessageBox.Show("Aramak istediginiz TC numarasını yazınız..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }


        }

        private void button1Yenile_Click(object sender, EventArgs e)
        {
            Kitap_iade_Sayfasi();
        }


        private void button1KitapEkle_Click(object sender, EventArgs e)
        {
            bool varmi = false;

            baglan.Open();
            komutver = new OleDbCommand("select *from KitapKayitYap where K_Adi='" + textBox1KitapAdi.Text + "' ", baglan);
            sorgu_oku = komutver.ExecuteReader();
            while (sorgu_oku.Read())
            {
                varmi = true;
                MessageBox.Show("Kayıt listesinde var..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                break;
            }
            baglan.Close();

            if (varmi == false)
            {
                if (textBox1KitapAdi.Text != "" && textBox2KitapYazari.Text != "" && textBox3KitapNerede.Text != "")
                {

                    baglan.Open();
                    komutver = new OleDbCommand("insert into KitapKayitYap(K_Adi,K_Yazar,K_Nerede) values('" + textBox1KitapAdi.Text + "','" + textBox2KitapYazari.Text + "','" + textBox3KitapNerede.Text + "') ", baglan);
                    komutver.ExecuteNonQuery();

                    comboBox1K_Adi.Items.Clear();
                    komutver = new OleDbCommand("select K_Adi from KitapKayitYap", baglan);
                    sorgu_oku = komutver.ExecuteReader();
                    while (sorgu_oku.Read())
                    {

                        comboBox1K_Adi.Items.Add(sorgu_oku["K_Adi"].ToString());

                    }

                    baglan.Close();
                    MessageBox.Show("Kayıt listeye eklendi..", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Kitap_Kayit_Sayfasi();
                    Kitap_Kayit_Sayfasi_temizle();


                }
                else
                {
                    MessageBox.Show("Boş Olan Alanları oldurunuz..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void button2KitapSil_Click(object sender, EventArgs e)
        {
            DialogResult cvp = MessageBox.Show("Silmek istediginizden eminmisiniz..?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);


            if (cvp == DialogResult.Yes)
            {

                if (label14.Text != "")
                {
                    if ((int)dataGridView2Kitap_Kayit.CurrentRow.Cells[0].Value > -1)
                {
                   

                        baglan.Open();
                        komutver = new OleDbCommand("delete from KitapKayitYap where id=@aydi", baglan);
                        komutver.Parameters.AddWithValue("@aydi", dataGridView2Kitap_Kayit.CurrentRow.Cells[0].Value);
                        komutver.ExecuteNonQuery();

                        comboBox1K_Adi.Items.Clear();
                        komutver = new OleDbCommand("select K_Adi from KitapKayitYap", baglan);
                        sorgu_oku = komutver.ExecuteReader();
                        while (sorgu_oku.Read())
                        {

                            comboBox1K_Adi.Items.Add(sorgu_oku["K_Adi"].ToString());

                        }

                        baglan.Close();
                        MessageBox.Show("Kayıt Silindi..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Kitap_Kayit_Sayfasi();
                        Kitap_Kayit_Sayfasi_temizle();

                    }
                   

                }
                else
                {
                    MessageBox.Show("Silinecek deger yok..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }


            }
            if (cvp == DialogResult.No)
            {
                Kitap_Kayit_Sayfasi();
                baglan.Close();
            }




        }

        private void dataGridView2Kitap_Kayit_DoubleClick(object sender, EventArgs e)
        {
            int secilen = dataGridView2Kitap_Kayit.SelectedCells[0].RowIndex;
            label14.Text = dataGridView2Kitap_Kayit.Rows[secilen].Cells[0].Value.ToString();
            textBox1KitapAdi.Text = dataGridView2Kitap_Kayit.Rows[secilen].Cells[1].Value.ToString();
            textBox2KitapYazari.Text = dataGridView2Kitap_Kayit.Rows[secilen].Cells[2].Value.ToString();
            textBox3KitapNerede.Text = dataGridView2Kitap_Kayit.Rows[secilen].Cells[3].Value.ToString();
        }

        private void button3Kitap_Duzenle_Click(object sender, EventArgs e)
        {
            if (textBox1KitapAdi.Text != "" && textBox2KitapYazari.Text != "" && textBox3KitapNerede.Text != "")
            {

                baglan.Open();
                komutver = new OleDbCommand("update KitapKayitYap set K_Adi=@p1,K_Yazar=@p2,K_Nerede=@p3 where id=@p4", baglan);
                komutver.Parameters.AddWithValue("@p1", textBox1KitapAdi.Text);
                komutver.Parameters.AddWithValue("@p2", textBox2KitapYazari.Text);
                komutver.Parameters.AddWithValue("@p3", textBox3KitapNerede.Text);
                komutver.Parameters.AddWithValue("@p4", label14.Text);
                komutver.ExecuteNonQuery();

                comboBox1K_Adi.Items.Clear();
                komutver = new OleDbCommand("select K_Adi from KitapKayitYap", baglan);
                sorgu_oku = komutver.ExecuteReader();
                while (sorgu_oku.Read())
                {

                    comboBox1K_Adi.Items.Add(sorgu_oku["K_Adi"].ToString());

                }

                baglan.Close();
                MessageBox.Show("Kayıtlar Düzenlendi..", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Kitap_Kayit_Sayfasi();
                Kitap_Kayit_Sayfasi_temizle();



            }
            else
            {
                MessageBox.Show("Alanlar Boş Olamaz..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void button4KitapArama_Click_1(object sender, EventArgs e)
        {
            if (textBox1KitapAdi.Text != "" || textBox2KitapYazari.Text != "")
            {

                baglan.Open();                                                                                                               //      where KitapAdi like '" + comboBox1K_Adi.Text + "%'", baglan);  
                liste = new OleDbDataAdapter("select id AS[SIRA NO], K_Adi AS[KİTAP ADI],K_Yazar AS[YAZAR ADI],K_Nerede AS[KİTAP NEREDE] from KitapKayitYap where K_Adi like '" + textBox1KitapAdi.Text + "%'", baglan);
                tablo = new DataTable();
                liste.Fill(tablo);

                dataGridView2Kitap_Kayit.DataSource = tablo;
                baglan.Close();
            }
            else
            {
                MessageBox.Show("Aramak istediginiz Kitap adı yazınız..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

        }

        private void button1Kayit_Yenile_Click(object sender, EventArgs e)
        {

            Kitap_Kayit_Sayfasi();
        }

        private void comboBox1K_Adi_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglan.Open();
            komutver = new OleDbCommand("select *from KitapKayitYap where K_Adi='" + comboBox1K_Adi.Text + "' ", baglan);
            sorgu_oku = komutver.ExecuteReader();
            while (sorgu_oku.Read())
            {
                textBox2Y_Adi.Text = sorgu_oku["K_Yazar"].ToString();
            }
            baglan.Close();

            Kitap_iade_Sayfasi();
            Kitap_Kayit_Sayfasi();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/n.beyi");
        }
         
        private void button1Yazdir_Click_1(object sender, EventArgs e)
        {

        }
    }
}
