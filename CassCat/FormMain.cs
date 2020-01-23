using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Collections;

namespace CassCat
{
    public partial class FormMain : Form
    {
        List<Cassete> library;
        bool itsNew;
        Cassete edit;
        public FormMain()
        {
            InitializeComponent();
            try
            {
                var serializer = new XmlSerializer(typeof(List<Cassete>));
                using (var reader = new StreamReader("Data.xml"))
                    library = (List<Cassete>)serializer.Deserialize(reader);
            }
            catch
            {
                library = new List<Cassete>();
            }
            Refresh();
        }

        public void SaveData()
        {
            var serializer = new XmlSerializer(typeof(List<Cassete>));
            using (var writer = new StreamWriter("Data.xml"))
                serializer.Serialize(writer, library);
        }

        int Find(Cassete cas, string str)
        {
            string[] names = str.Split(',');
            int s = 0;
            int voids = 0;
            foreach (string name in names)
            {
                string nm = name;
                while (nm.Length > 0 && name[0] == ' ') nm = nm.Substring(1, nm.Length - 1);
                while (nm.Length > 0 && name[nm.Length-1] == ' ') nm = nm.Substring(0, nm.Length-1);
                if (nm.Length < 1) { voids++; break; }
                bool find = false;
                foreach (string game in cas.sideA) if (game.ToLower().Contains(nm.ToLower())) { find = true; break; }
                foreach (string game in cas.sideB) if (game.ToLower().Contains(nm.ToLower())) { find = true; break; }
                if (find)
                    s++;
            }
            float res = 0;
            if (voids < names.Count())
                res = ((float)s / (names.Count() - voids)) * 100;
            if (res > 100) res = 100;
            return (int)res;
        }

        void Refresh()
        {
            library.Sort((o1, o2) => o1.name.CompareTo(o2.name));
            listViewCassetes.BeginUpdate();
            listViewCassetes.Items.Clear();
            int n = 1;
            foreach (Cassete cas in library)
            {
                int p = textBoxSearch.Text == "" ? 101 : Find(cas, textBoxSearch.Text);
                if (p > 0)
                {
                    ListViewItem item;
                    if (p > 100)
                        item = new ListViewItem(n++.ToString());
                    else
                        item = new ListViewItem(p.ToString() + "%");
                    item.SubItems.Add(cas.name);
                    item.Tag = cas;
                    listViewCassetes.Items.Add(item);
                }
            }
            listViewCassetes.Sort();
            listViewCassetes.EndUpdate();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            itsNew = true;
            edit = new Cassete();

            listViewCassetes.Enabled = false;
            textBoxLabel.Text = "";
            textBoxLabel.Enabled = true;
            textBoxPublisher.Text = "";
            textBoxPublisher.Enabled = true;
            textBoxCity.Text = "";
            textBoxCity.Enabled = true;
            textBoxYear.Text = "";
            textBoxYear.Enabled = true;
            dataGridViewA.Rows.Clear();
            dataGridViewA.Enabled = true;
            dataGridViewB.Rows.Clear();
            dataGridViewB.Enabled = true;
            buttonAdd.Enabled = false;
            buttonDel.Enabled = false;
            buttonSave.Enabled = true;
            buttonCancel.Enabled = true;
        }
        private void ButtonDel_Click(object sender, EventArgs e)
        {

        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            edit.name = textBoxLabel.Text;
            edit.publisher = textBoxPublisher.Text;
            edit.city = textBoxCity.Text;
            edit.year = textBoxYear.Text;
            edit.sideA = new List<string>();
            for (int i = 0; dataGridViewA[0, i].Value != null; i++)
                edit.sideA.Add(dataGridViewA[0, i].Value.ToString());
            edit.sideB = new List<string>();
            for (int i = 0; dataGridViewB[0, i].Value != null; i++)
                edit.sideB.Add(dataGridViewB[0, i].Value.ToString());
            if (itsNew) library.Add(edit);
            SaveData();
            Refresh();
            Clear();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            listViewCassetes.Enabled = true;
            textBoxLabel.Text = "";
            textBoxLabel.Enabled = false;
            textBoxPublisher.Text = "";
            textBoxPublisher.Enabled = false;
            textBoxCity.Text = "";
            textBoxCity.Enabled = false;
            textBoxYear.Text = "";
            textBoxYear.Enabled = false;
            dataGridViewA.Rows.Clear();
            dataGridViewA.Enabled = false;
            dataGridViewB.Rows.Clear();
            dataGridViewB.Enabled = false;
            buttonAdd.Enabled = true;
            buttonDel.Enabled = false;
            buttonSave.Enabled = false;
            buttonCancel.Enabled = false;
        }

        private void ListViewCassetes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewCassetes.SelectedItems.Count == 0)
            {
                Clear();
                return;
            }
            itsNew = false;
            edit = (Cassete)listViewCassetes.SelectedItems[0].Tag;

            listViewCassetes.Enabled = true;
            textBoxLabel.Text = edit.name;
            textBoxLabel.Enabled = true;
            textBoxPublisher.Text = edit.publisher;
            textBoxPublisher.Enabled = true;
            textBoxCity.Text = edit.city;
            textBoxCity.Enabled = true;
            textBoxYear.Text = edit.year;
            textBoxYear.Enabled = true;
            dataGridViewA.Rows.Clear();
            foreach(string pr in edit.sideA)
                dataGridViewA.Rows.Add(pr);
            dataGridViewA.Enabled = true;
            dataGridViewB.Rows.Clear();
            foreach (string pr in edit.sideB)
                dataGridViewB.Rows.Add(pr);
            dataGridViewB.Enabled = true;

            buttonDel.Enabled = true;
            buttonSave.Enabled = true;
            buttonCancel.Enabled = true;
        }
        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            Refresh();
        }
        private void buttonReset_Click(object sender, EventArgs e)
        {
            textBoxSearch.Text = "";
        }

        #region Меню
        private void ВыходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ОПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("CassCat\n" +
                "Версия: " + Program.Version +
                "\nАвтор: Сергей Гордеев",
                "О программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        #endregion

    }
}
