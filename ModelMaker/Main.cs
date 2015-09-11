using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;

using ModelMaker.Clases;
using ModelMaker.Utils;

namespace ModelMaker {
    public partial class Main : Form {

        private List<Column> columnas;
        private List<Table> tablas;
        private Controles ctrs;

        public Main() {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e) {
            //--Levantar config file
            XmlDocument doc = new XmlDocument();
            try {
                doc.Load("App.config");
                
                foreach (XmlNode node in doc.ChildNodes[0].ChildNodes) {
                    switch (node.Name) {
                        case "direccion":
                            tbDireccion.Text = node.InnerText;
                            break;
                        case "usuario":
                            tbUsuario.Text = node.InnerText;
                            break;
                        case "contrasenia":
                            tbPassword.Text = node.InnerText;
                            break;
                        case "base":
                            tbDb.Text = node.InnerText;
                            break;
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void bCargar_Click(object sender, EventArgs e) {
            ctrs = new Controles() {
                Direccion = tbDireccion.Text, 
                Database = tbDb.Text, 
                User = tbUsuario.Text,
                Password = tbPassword.Text 
            };

            try {
                ctrs.Vaidate();
            } catch (MyException ex) {
                MessageBox.Show(ex.Message);
            }

            //--Asignar datos de la db
            DB.setData(ctrs);

            //--Levantar y ejecutar query
            StreamReader sr = null;
            try {
                sr = new StreamReader("Levantar.sql");
            } catch (FileNotFoundException) {
                MessageBox.Show("No se encontró el archivo Levantar.sql");
                return;
            }

            string query = sr.ReadToEnd();

            try {
                //--Armar columnas
                columnas = DB.ExecuteReader<Column>(query);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
                return;
            }

            tablas = columnas.GroupBy(c => c.tabla).Select(item => new Table(item.Key, item.ToList())).ToList();

            refreshTables();
        }

        private void clbTablas_SelectedIndexChanged(object sender, EventArgs e) {
            if (clbTablas.SelectedIndex == -1)
                return;

            refreshColumns();
        }

        private void refreshTables() {

            //--Llenar clb de tablas y tildarlas
            clbTablas.Items.Clear();
            clbTablas.Items.AddRange(tablas.ToArray());
            for (int i = 0; i < clbTablas.Items.Count; i++)
                clbTablas.SetItemChecked(i, true);

        }

        private void refreshColumns() {
            Table _tabla = ((Table)clbTablas.SelectedItem);

            //--Llenar dgv según la tabla seleccionada
            dgvColumnas.Rows.Clear();
            foreach (Column col in _tabla.columnas) {
                dgvColumnas.Rows.Add(col.ToArray());
            }
        }

        private void bGenerar_Click(object sender, EventArgs e) {
            if (tbProyecto.Text == "") {
                MessageBox.Show("Falta nombre del proyecto.");
                return;
            }

            if (String.IsNullOrWhiteSpace(tbOutput.Text)) {
                MessageBox.Show("Falta seleccionar la carpeta de salida.");
                return;
            }

            //--Procesar las tablas checkeadas
            new FileHandler(tbProyecto.Text, tbOutput.Text + "/Output", ctrs).Work(clbTablas.CheckedItems.Cast<Table>());

            MessageBox.Show("Done", "Well", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void bElegirOutput_Click(object sender, EventArgs e) {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) tbOutput.Text = folderBrowserDialog1.SelectedPath;                
        }
    }
}