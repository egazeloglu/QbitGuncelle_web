﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.Net;
using System.Diagnostics;

namespace QbitGuncelle_web
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void CompressButton_Click(object sender, EventArgs e)
        {
            string sourceFile = "";
            string destinationFile = "";

            // OpenFileDialog nesnesi oluşturuluyor
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            // Kullanıcı dosya seçtiyse
            if (result == DialogResult.OK)
            {
                // Seçilen dosyanın yolu alınıyor
                sourceFile = openFileDialog.FileName;
                destinationFile = Application.StartupPath + "\\QbitKazan_v2.zip";
            }

            try
            {
                // Check if source file exists
                if (!File.Exists(sourceFile))
                {
                    MessageBox.Show("Kaynak dosya bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Check if destination file already exists
                if (File.Exists(destinationFile))
                {
                    MessageBox.Show("Hedef dosya zaten mevcut. Lütfen farklı bir dosya adı belirtin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Compress the file
                ZipFile.CreateFromDirectory(Path.GetDirectoryName(sourceFile), destinationFile);


                // Upload the compressed file via FTP
                UploadFileViaFTP(destinationFile, $"ftp://ftp.qbitproje.com/", "qbitkazanv2@qbitproje.com", "_zN6sV_5StP_", progressBar1);

                MessageBox.Show("Yükleme Dosyası başarıyla oluşturuldu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DecompressButton_Click(object sender, EventArgs e)
        {

        }
        //private void UploadFileViaFTP(string fileToUpload, string ftpServerPath, string ftpUsername, string ftpPassword)
        //{
        //    using (WebClient client = new WebClient())
        //    {
        //        client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
        //        client.UploadFile(ftpServerPath + Path.GetFileName(fileToUpload), WebRequestMethods.Ftp.UploadFile, fileToUpload);
        //    }
        //}

        private void UploadFileViaFTP(string fileToUpload, string ftpServerPath, string ftpUsername, string ftpPassword, ProgressBar progressBar)
        {
            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                // Dosya boyutunu al
                FileInfo fileInfo = new FileInfo(fileToUpload);
                long fileSize = fileInfo.Length;

                // İlerleme olaylarına abone ol
                client.UploadProgressChanged += (sender, e) =>
                {
                    // İlerleme çubuğunu güncelle
                    progressBar.Value = (int)((e.BytesSent * 100) / fileSize);
                };

                // Dosyayı yükle
                //client.UploadFileAsync(new Uri(ftpServerPath + "/" + Path.GetFileName(fileToUpload)), WebRequestMethods.Ftp.UploadFile, fileToUpload);
                client.UploadFile(ftpServerPath + Path.GetFileName(fileToUpload), WebRequestMethods.Ftp.UploadFile, fileToUpload);
            }
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            string destinationFile="";

            // OpenFileDialog nesnesi oluşturuluyor
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            // Kullanıcı dosya seçtiyse
            if (result == DialogResult.OK)
            {
                // Seçilen dosyanın yolu alınıyor 
                destinationFile = openFileDialog.FileName;
            }
            // Upload the compressed file via FTP
            UploadFileViaFTP(destinationFile, $"ftp://ftp.qbitproje.com/", "qbitkazanv2@qbitproje.com", "_zN6sV_5StP_", progressBar1);
            MessageBox.Show("Karşıya Yükleme Tamamlandı.", "Güncel dosyayı web yükle ", MessageBoxButtons.OK);
        }
    }
}