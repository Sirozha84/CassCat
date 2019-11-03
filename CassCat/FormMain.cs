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

        void Refresh()
        {
            library.Sort((o1, o2) => o1.name.CompareTo(o2.name));
            listViewCassetes.BeginUpdate();
            listViewCassetes.Items.Clear();
            int n = 1;
            foreach (Cassete cas in library)
            {
                ListViewItem item = new ListViewItem(n++.ToString());
                item.SubItems.Add(cas.name);
                item.Tag = cas;
                listViewCassetes.Items.Add(item);
            }
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
            //dataGridViewA
            dataGridViewA.Enabled = true;
            //dataGridViewB
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
            if (itsNew) library.Add(edit);
            SaveData();
            Refresh();

            listViewCassetes.Enabled = true;
            textBoxLabel.Text = "";
            textBoxLabel.Enabled = false;
            textBoxPublisher.Text = "";
            textBoxPublisher.Enabled = false;
            textBoxCity.Text = "";
            textBoxCity.Enabled = false;
            textBoxYear.Text = "";
            textBoxYear.Enabled = false;
            //dataGridViewA
            dataGridViewA.Enabled = false;
            //dataGridViewB
            dataGridViewB.Enabled = false;
            buttonAdd.Enabled = true;
            buttonSave.Enabled = false;
            buttonCancel.Enabled = false;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
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
            //dataGridViewA
            dataGridViewA.Enabled = false;
            //dataGridViewB
            dataGridViewB.Enabled = false;
            buttonAdd.Enabled = true;
            buttonSave.Enabled = false;
            buttonCancel.Enabled = false;
        }
    }
}
