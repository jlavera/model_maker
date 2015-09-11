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

namespace ModelMaker {
    public partial class Main : Form {

        private List<Column> columnas;
        private List<Table> tablas;

        public Main() {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e) {
            cmbMotor.SelectedIndex = 0;

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
            
            if (cmbMotor.SelectedItem == null || tbDireccion.Text == "" || tbDb.Text == "" || tbUsuario.Text == "" || tbPassword.Text == "") {
                MessageBox.Show("Falta completar campos");
                return;
            }

            //--Asignar datos de la db
            DB.setData(cmbMotor.SelectedItem.ToString(), tbDireccion.Text, tbDb.Text, tbUsuario.Text, tbPassword.Text);

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

        private void dgvColumnas_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex == -1)
                return;

            //--lol
            (
                (Column)
                ((Table)clbTablas.SelectedItem).columnas.Where(c => c.posicion == (int)dgvColumnas["col_posicion", e.RowIndex].Value).ElementAt(0))
                .nombreAtributo = dgvColumnas[e.ColumnIndex, e.RowIndex].Value.ToString();
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

            FileHandler.Proyecto = tbProyecto.Text;
            FileHandler.OutPutFolder = tbOutput.Text + "/Output";

            if (Directory.Exists(FileHandler.OutPutFolder))
                Directory.Delete(FileHandler.OutPutFolder, true);

            //--Crea las carpetas necesarias si no existen
            if (!Directory.Exists(FileHandler.OutPutFolder))
                Directory.CreateDirectory(FileHandler.OutPutFolder);
            if (!Directory.Exists(FileHandler.OutPutDAOFolder))
                Directory.CreateDirectory(FileHandler.OutPutDAOFolder);
            if (!Directory.Exists(FileHandler.OutPut_DAOFolder))
                Directory.CreateDirectory(FileHandler.OutPut_DAOFolder);
            if (!Directory.Exists(FileHandler.OutPutBOFolder))
                Directory.CreateDirectory(FileHandler.OutPutBOFolder);
            if (!Directory.Exists(FileHandler.OutPut_BOFolder))
                Directory.CreateDirectory(FileHandler.OutPut_BOFolder);
            if (!Directory.Exists(FileHandler.OutPutUtilsFolder))
                Directory.CreateDirectory(FileHandler.OutPutUtilsFolder);

            var ctrs = new Controles() { Direccion = tbDireccion.Text, Database = tbDb.Text, User = tbUsuario.Text, Password = tbPassword.Text };

            //--Crear DAOBase
            FileHandler.copyFile(FileHandler.Archivos.DAOBase, FileHandler.OutPutDAOFolder + "/DAOBase.cs", tbProyecto.Text, ctrs);

            //--Crear IBO
            FileHandler.copyFile(FileHandler.Archivos.IBO, FileHandler.OutPutBOFolder + "/IBO.cs", tbProyecto.Text, ctrs);

            //--Crear DB
            FileHandler.copyFile(FileHandler.Archivos.DB, FileHandler.OutPutUtilsFolder + "/DB.cs", tbProyecto.Text, ctrs);

            //--Crear MyException
            FileHandler.copyFile(FileHandler.Archivos.EXCEPTION, FileHandler.OutPutUtilsFolder + "/MyException.cs", tbProyecto.Text, ctrs);

            //--Crear los DAOs
            foreach (Table tlb in clbTablas.CheckedItems)
                FileHandler.createClassFiles(tlb);

            MessageBox.Show("Done", "Well", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void bElegirOutput_Click(object sender, EventArgs e) {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK){
                tbOutput.Text = folderBrowserDialog1.SelectedPath;
            }
                
        }
    }
}