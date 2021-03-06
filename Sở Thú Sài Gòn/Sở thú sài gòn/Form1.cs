﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace Sở_thú_sài_gòn
{
    public partial class frmSothusaigon : Form
    {
        public frmSothusaigon()
        {
            InitializeComponent();
        }
        bool isChanged = false;
        bool isSaved = true;
        // Bắt sự kiện khi ấn chuột vào
        private void MouseDown(object sender, MouseEventArgs e)
        {
            ListBox lst = (ListBox)sender;
            int index = lst.IndexFromPoint(e.X, e.Y);
            if (index!=-1)
            lst.DoDragDrop(lst.Items[index].ToString(), DragDropEffects.Copy);

        }
        // Kéo item nào đó đi vào control khác
        private void DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.Move;
        }
        // Thả item xuống
        private void DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                if (!lstDanhSachThu.Items.Contains(lstThuMoi.SelectedItem))
                {
                    int newIndex = lstDanhSachThu.IndexFromPoint(lstDanhSachThu.PointToClient(new Point(e.X, e.Y)));
                    object selectedItem = e.Data.GetData(DataFormats.Text);
                    if (newIndex != -1)
                        lstDanhSachThu.Items.Insert(newIndex, selectedItem);
                    else
                        lstDanhSachThu.Items.Insert(lstDanhSachThu.Items.Count, selectedItem);
                    isChanged = true;
                }
        }

        private void Save(object sender, EventArgs e)
        {
            // Mở tập tin
            StreamWriter write = new StreamWriter("danhsachthu.txt");
            if (write == null)
                return;
            foreach (var item in lstDanhSachThu.Items)
                write.WriteLine(item.ToString());
            write.Close();
            isSaved = false;
        }
        private void mnuClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mnuLoad_Click(object sender, EventArgs e)
        {
            StreamReader reader = new StreamReader("thumoi.txt");
            if (reader == null)
                return;
            string input;
            while ((input = reader.ReadLine()) != null)          
               lstThuMoi.Items.Add(input);          
            reader.Close();
            using (StreamReader rs = new StreamReader("danhsachthu.txt"))
            {
                input = null;
                while ((input = rs.ReadLine()) != null)
                {
                    lstDanhSachThu.Items.Add(input);
                }

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = string.Format("Bây giờ là{0}:{1}:{2} ngày {3} tháng {4} năm {5}",
                                    DateTime.Now.Hour, 
                                    DateTime.Now.Minute,
                                    DateTime.Now.Second,
                                    DateTime.Now.Day,
                                    DateTime.Now.Month,
                                    DateTime.Now.Year);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void Xoa(object sender, EventArgs e)
        {
            while(lstDanhSachThu.SelectedIndex!=-1)
            lstDanhSachThu.Items.RemoveAt(lstDanhSachThu.SelectedIndex);
            isChanged = true;
        }

        private void frmSothusaigon_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isChanged == true)
            {
                if (isSaved)
                {
                    DialogResult result = MessageBox.Show("Do you want to save Changes?", "", MessageBoxButtons.YesNoCancel,
                                                                                        MessageBoxIcon.None
                                                                                        );
                    if (result == DialogResult.Yes)
                    {
                        Save(sender, e);
                        e.Cancel = false;
                    }
                    else if (result == DialogResult.No)
                        e.Cancel = false;
                    else
                        e.Cancel = true;
                }
            }
           

        }
    }

}
