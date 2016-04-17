﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Restorium
{
    public partial class StokAdd : Form
    {
        public static int countOfItems = 0;
        public static string Aciklama { get; set; }
        public static string ID { get; set; }
        public static string ParaBirimi { get; set; }
        public static decimal BirimFiyat { get; set; }
        public static int Adet { get; set; }
        public static string AdetTuru { get; set; }
        public static bool menuUrunu { get; set; }
        public static bool dinamikStokKontrolu { get; set; }
        AutoCompleteStringCollection col2 = new AutoCompleteStringCollection();
        public StokAdd()
        {
            InitializeComponent();
        }

        private void StokAdd_Load(object sender, EventArgs e)
        {
            countOfItems = 0;
            try
            {
                IniFile INI = new IniFile();
                int stokCount = Convert.ToInt32(INI.Read("stokCount", "Stok"));
                countOfItems = stokCount;
            } catch
            {
                UserLog.WConsole("(!)Stok kayitlarindan stok sayisini okumada hata ! (StokAdd.cs)");
            }
            cbBirim.SelectedItem= "Birim";
            cbParaBirimi.SelectedItem = "TL";
            //----------------------//
            UserLog.WConsole("<<< StokAdd (Form)>>>");
            tbID.AutoCompleteMode = AutoCompleteMode.Suggest;
            tbID.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection col = new AutoCompleteStringCollection();
            
            /*
            for (int i = 0; i < 500; i++)
            {
                if (MainForm.menuID[i] != null)
                {
                    countOfItems++;
                }
                else
                {
                    break;
                }
            }
            */
            UserLog.WConsole("countOfItems :" + countOfItems);
            for (int i = 0; i < countOfItems; i++)
            {
                col.Add(MainForm.menuID[i]);
            }
            tbID.AutoCompleteCustomSource = col;
            //----------------------//
            tbAciklama.AutoCompleteMode = AutoCompleteMode.Suggest;
            tbAciklama.AutoCompleteSource = AutoCompleteSource.CustomSource;
           
            for (int i = 0; i < countOfItems; i++)
            {
                col2.Add(MainForm.menuAciklama[i]);
            }
            tbAciklama.AutoCompleteCustomSource = col2;
            //----------------------//

        }

        private void IDSelectedFromList(object sender, EventArgs e)
        {
            string id = tbID.Text;
            int i = 0;
            foreach (string menuid in MainForm.menuID)
            {
                if (tbID.Text == MainForm.menuID[i])
                {
                    tbAciklama.Text = MainForm.menuAciklama[i];
                    tbBirimFiyat.Text = MainForm.menuPrice[i].ToString();
                    lStatusUrun.Text = "Stokta Mecvut Urun";
                    lStatusUrun.ForeColor = Color.Red;
                    break;
                }
                else
                {
                    i++;
                    lStatusUrun.Text = "Yeni Stok Urunu";
                    lStatusUrun.ForeColor = Color.Green;
                }
            }
        }

        private void AciklamaSelectedFromList(object sender, EventArgs e)
        {
            string aciklama = tbAciklama.Text;
            int i = 0;
            foreach (string menuaciklama in MainForm.menuAciklama)
            {
                if (tbAciklama.Text == MainForm.menuAciklama[i])
                {
                    tbID.Text = MainForm.menuID[i];
                    tbBirimFiyat.Text = MainForm.menuPrice[i].ToString();
                    lStatusUrun.Text = "Stokta Mecvut Urun";
                    lStatusUrun.ForeColor = Color.Red;
                    break;
                }
                else
                {
                    i++;
                    lStatusUrun.Text = "Yeni Stok Urunu";
                    lStatusUrun.ForeColor = Color.Green;
                }
            }
        }

        private void bStokAdd_Click(object sender, EventArgs e)
        {
            CultureInfo culture = new CultureInfo("tr-TR", true);
            if (tbBirimFiyat.Text.LastIndexOf(',') != -1)
                culture.NumberFormat.NumberDecimalSeparator = ",";

            if (tbAciklama.Text != "" && tbAdet.Text != "" && tbBirimFiyat.Text != "" && tbID.Text != "")
            {
                //if(System.Text.RegularExpressions.Regex.IsMatch("^[0-9]", tbAdet.Text) && System.Text.RegularExpressions.Regex.IsMatch("^[0-9]", tbBirimFiyat.Text))
                if (System.Text.RegularExpressions.Regex.IsMatch(tbAdet.Text, "^[0-9]") && System.Text.RegularExpressions.Regex.IsMatch(tbBirimFiyat.Text, "^[0-9]"))
                {
                    //valid item
                    Aciklama = tbAciklama.Text;
                    ID = tbID.Text;
                    BirimFiyat = Convert.ToDecimal(tbBirimFiyat.Text,culture);
                    ParaBirimi = cbParaBirimi.SelectedItem.ToString(); //SORUN BURDA
                    Adet = Convert.ToInt32(tbAdet.Text);
                    AdetTuru = cbBirim.SelectedItem.ToString(); // SORUN BURDA
                    if (cbMenuUrunu.CheckState == CheckState.Checked)
                    {
                        menuUrunu = true;
                    }
                    else
                    {
                        menuUrunu = false;
                    }
                    if (cbDynamicStokCheckEnabled.CheckState == CheckState.Checked)
                    {
                        dinamikStokKontrolu = true;
                    }
                    else
                    {
                        dinamikStokKontrolu = false;
                    }
                    this.DialogResult = DialogResult.OK;
                    col2.Clear();

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lutfen Adet ve Birim Fiyat Alanlarina Gecerli Karakter Giriniz !");
                }
            }
            else
            {
                //invalid item
                MessageBox.Show("Lutfen butun alanlari doldurunuz !");
            }

        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            col2.Clear();
            this.Close();
        }

        private void StokCheckLogic(object sender, EventArgs e)
        {

        }

        private void StokCheckLogicMenuCheckbox(object sender, EventArgs e)
        {
            if (cbMenuUrunu.CheckState == CheckState.Checked)
            {
                cbStokUrunu.CheckState = CheckState.Unchecked;
                cbMenuUrunu.CheckState = CheckState.Checked;
                cbDynamicStokCheckEnabled.Enabled = true;
                cbDynamicStokCheckEnabled.CheckState = CheckState.Unchecked;
            }
            else
            {
                cbDynamicStokCheckEnabled.Enabled = false;
                cbDynamicStokCheckEnabled.CheckState = CheckState.Unchecked;
            }
        }

        private void StokCheckLogicStokUrunuCheckbox(object sender, EventArgs e)
        {
            if (cbStokUrunu.CheckState == CheckState.Checked)
            {
                cbMenuUrunu.CheckState = CheckState.Unchecked;
                cbStokUrunu.CheckState = CheckState.Checked;
                cbDynamicStokCheckEnabled.Enabled = false;
                cbDynamicStokCheckEnabled.CheckState = CheckState.Unchecked;
            }
        }
    }
}
