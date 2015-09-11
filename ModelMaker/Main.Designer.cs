namespace ModelMaker {
    partial class Main {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.gbConfig = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.bCargar = new System.Windows.Forms.Button();
            this.tbDireccion = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDb = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbUsuario = new System.Windows.Forms.TextBox();
            this.bGenerar = new System.Windows.Forms.Button();
            this.gbEstructura = new System.Windows.Forms.GroupBox();
            this.clbTablas = new System.Windows.Forms.CheckedListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dgvColumnas = new System.Windows.Forms.DataGridView();
            this.col_posicion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_tipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_max = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_precision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_default = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_nullable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.col_pk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.col_uq = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.col_index = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.col_fk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.col_fktabla = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_fkcol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_fkreferenced = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.tbProyecto = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.bElegirOutput = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.gbConfig.SuspendLayout();
            this.gbEstructura.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumnas)).BeginInit();
            this.SuspendLayout();
            // 
            // gbConfig
            // 
            this.gbConfig.Controls.Add(this.label5);
            this.gbConfig.Controls.Add(this.bCargar);
            this.gbConfig.Controls.Add(this.tbDireccion);
            this.gbConfig.Controls.Add(this.label4);
            this.gbConfig.Controls.Add(this.label3);
            this.gbConfig.Controls.Add(this.label2);
            this.gbConfig.Controls.Add(this.tbDb);
            this.gbConfig.Controls.Add(this.tbPassword);
            this.gbConfig.Controls.Add(this.tbUsuario);
            this.gbConfig.Location = new System.Drawing.Point(12, 12);
            this.gbConfig.Name = "gbConfig";
            this.gbConfig.Size = new System.Drawing.Size(810, 78);
            this.gbConfig.TabIndex = 0;
            this.gbConfig.TabStop = false;
            this.gbConfig.Text = "Configuración de la base de datos:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Dirección:";
            // 
            // bCargar
            // 
            this.bCargar.Location = new System.Drawing.Point(532, 27);
            this.bCargar.Name = "bCargar";
            this.bCargar.Size = new System.Drawing.Size(259, 37);
            this.bCargar.TabIndex = 5;
            this.bCargar.Text = "Cargar";
            this.bCargar.UseVisualStyleBackColor = true;
            this.bCargar.Click += new System.EventHandler(this.bCargar_Click);
            // 
            // tbDireccion
            // 
            this.tbDireccion.Location = new System.Drawing.Point(23, 43);
            this.tbDireccion.Name = "tbDireccion";
            this.tbDireccion.Size = new System.Drawing.Size(100, 20);
            this.tbDireccion.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(399, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Base de datos:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(272, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Contraseña:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(139, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Usuario:";
            // 
            // tbDb
            // 
            this.tbDb.Location = new System.Drawing.Point(402, 44);
            this.tbDb.Name = "tbDb";
            this.tbDb.Size = new System.Drawing.Size(100, 20);
            this.tbDb.TabIndex = 4;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(275, 44);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(100, 20);
            this.tbPassword.TabIndex = 3;
            // 
            // tbUsuario
            // 
            this.tbUsuario.Location = new System.Drawing.Point(142, 43);
            this.tbUsuario.Name = "tbUsuario";
            this.tbUsuario.Size = new System.Drawing.Size(100, 20);
            this.tbUsuario.TabIndex = 2;
            // 
            // bGenerar
            // 
            this.bGenerar.Location = new System.Drawing.Point(1123, 38);
            this.bGenerar.Name = "bGenerar";
            this.bGenerar.Size = new System.Drawing.Size(139, 37);
            this.bGenerar.TabIndex = 2;
            this.bGenerar.Text = "Generar >>";
            this.bGenerar.UseVisualStyleBackColor = true;
            this.bGenerar.Click += new System.EventHandler(this.bGenerar_Click);
            // 
            // gbEstructura
            // 
            this.gbEstructura.Controls.Add(this.clbTablas);
            this.gbEstructura.Controls.Add(this.label7);
            this.gbEstructura.Controls.Add(this.dgvColumnas);
            this.gbEstructura.Controls.Add(this.label6);
            this.gbEstructura.Location = new System.Drawing.Point(12, 96);
            this.gbEstructura.Name = "gbEstructura";
            this.gbEstructura.Size = new System.Drawing.Size(1252, 331);
            this.gbEstructura.TabIndex = 4;
            this.gbEstructura.TabStop = false;
            this.gbEstructura.Text = "Estructura de la base de datos:";
            // 
            // clbTablas
            // 
            this.clbTablas.FormattingEnabled = true;
            this.clbTablas.Location = new System.Drawing.Point(22, 44);
            this.clbTablas.Name = "clbTablas";
            this.clbTablas.Size = new System.Drawing.Size(187, 274);
            this.clbTablas.TabIndex = 0;
            this.clbTablas.SelectedIndexChanged += new System.EventHandler(this.clbTablas_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(212, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Columnas:";
            // 
            // dgvColumnas
            // 
            this.dgvColumnas.AllowUserToAddRows = false;
            this.dgvColumnas.AllowUserToDeleteRows = false;
            this.dgvColumnas.AllowUserToResizeRows = false;
            this.dgvColumnas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvColumnas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvColumnas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_posicion,
            this.col_name,
            this.col_tipo,
            this.col_max,
            this.col_precision,
            this.col_default,
            this.col_nullable,
            this.col_pk,
            this.col_uq,
            this.col_index,
            this.col_fk,
            this.col_fktabla,
            this.col_fkcol,
            this.col_fkreferenced});
            this.dgvColumnas.Location = new System.Drawing.Point(226, 44);
            this.dgvColumnas.Name = "dgvColumnas";
            this.dgvColumnas.Size = new System.Drawing.Size(1009, 274);
            this.dgvColumnas.TabIndex = 2;
            // 
            // col_posicion
            // 
            this.col_posicion.HeaderText = "Posición";
            this.col_posicion.Name = "col_posicion";
            this.col_posicion.ReadOnly = true;
            this.col_posicion.Width = 72;
            // 
            // col_name
            // 
            this.col_name.HeaderText = "Nombre";
            this.col_name.Name = "col_name";
            this.col_name.ReadOnly = true;
            this.col_name.Width = 69;
            // 
            // col_tipo
            // 
            this.col_tipo.HeaderText = "Tipo";
            this.col_tipo.Name = "col_tipo";
            this.col_tipo.ReadOnly = true;
            this.col_tipo.Width = 53;
            // 
            // col_max
            // 
            this.col_max.HeaderText = "Maximo";
            this.col_max.Name = "col_max";
            this.col_max.ReadOnly = true;
            this.col_max.Width = 68;
            // 
            // col_precision
            // 
            this.col_precision.HeaderText = "Presicion";
            this.col_precision.Name = "col_precision";
            this.col_precision.ReadOnly = true;
            this.col_precision.Width = 75;
            // 
            // col_default
            // 
            this.col_default.HeaderText = "Default";
            this.col_default.Name = "col_default";
            this.col_default.ReadOnly = true;
            this.col_default.Width = 66;
            // 
            // col_nullable
            // 
            this.col_nullable.HeaderText = "Nullable";
            this.col_nullable.Name = "col_nullable";
            this.col_nullable.ReadOnly = true;
            this.col_nullable.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.col_nullable.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.col_nullable.Width = 70;
            // 
            // col_pk
            // 
            this.col_pk.HeaderText = "PK";
            this.col_pk.Name = "col_pk";
            this.col_pk.ReadOnly = true;
            this.col_pk.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.col_pk.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.col_pk.Width = 46;
            // 
            // col_uq
            // 
            this.col_uq.HeaderText = "UQ";
            this.col_uq.Name = "col_uq";
            this.col_uq.ReadOnly = true;
            this.col_uq.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.col_uq.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.col_uq.Width = 48;
            // 
            // col_index
            // 
            this.col_index.HeaderText = "IX";
            this.col_index.Name = "col_index";
            this.col_index.ReadOnly = true;
            this.col_index.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.col_index.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.col_index.Width = 42;
            // 
            // col_fk
            // 
            this.col_fk.HeaderText = "Foreign key";
            this.col_fk.Name = "col_fk";
            this.col_fk.ReadOnly = true;
            this.col_fk.Width = 61;
            // 
            // col_fktabla
            // 
            this.col_fktabla.HeaderText = "FK Tabla";
            this.col_fktabla.Name = "col_fktabla";
            this.col_fktabla.ReadOnly = true;
            this.col_fktabla.Width = 69;
            // 
            // col_fkcol
            // 
            this.col_fkcol.HeaderText = "FK Columna";
            this.col_fkcol.Name = "col_fkcol";
            this.col_fkcol.ReadOnly = true;
            this.col_fkcol.Width = 82;
            // 
            // col_fkreferenced
            // 
            this.col_fkreferenced.HeaderText = "FK Referenced";
            this.col_fkreferenced.Name = "col_fkreferenced";
            this.col_fkreferenced.ReadOnly = true;
            this.col_fkreferenced.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.col_fkreferenced.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.col_fkreferenced.Width = 96;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Tablas:";
            // 
            // tbProyecto
            // 
            this.tbProyecto.Location = new System.Drawing.Point(845, 29);
            this.tbProyecto.Name = "tbProyecto";
            this.tbProyecto.Size = new System.Drawing.Size(216, 20);
            this.tbProyecto.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(842, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Proyecto:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(842, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Output folder:";
            // 
            // tbOutput
            // 
            this.tbOutput.Enabled = false;
            this.tbOutput.Location = new System.Drawing.Point(845, 73);
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.Size = new System.Drawing.Size(101, 20);
            this.tbOutput.TabIndex = 11;
            // 
            // bElegirOutput
            // 
            this.bElegirOutput.Location = new System.Drawing.Point(952, 71);
            this.bElegirOutput.Name = "bElegirOutput";
            this.bElegirOutput.Size = new System.Drawing.Size(109, 23);
            this.bElegirOutput.TabIndex = 13;
            this.bElegirOutput.Text = "Elegir output";
            this.bElegirOutput.UseVisualStyleBackColor = true;
            this.bElegirOutput.Click += new System.EventHandler(this.bElegirOutput_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1274, 439);
            this.Controls.Add(this.bElegirOutput);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbOutput);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbProyecto);
            this.Controls.Add(this.bGenerar);
            this.Controls.Add(this.gbEstructura);
            this.Controls.Add(this.gbConfig);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.gbConfig.ResumeLayout(false);
            this.gbConfig.PerformLayout();
            this.gbEstructura.ResumeLayout(false);
            this.gbEstructura.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumnas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbConfig;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDb;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbUsuario;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbDireccion;
        private System.Windows.Forms.Button bCargar;
        private System.Windows.Forms.GroupBox gbEstructura;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dgvColumnas;
        private System.Windows.Forms.Button bGenerar;
        private System.Windows.Forms.CheckedListBox clbTablas;
        private System.Windows.Forms.TextBox tbProyecto;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_posicion;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_tipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_max;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_precision;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_default;
        private System.Windows.Forms.DataGridViewCheckBoxColumn col_nullable;
        private System.Windows.Forms.DataGridViewCheckBoxColumn col_pk;
        private System.Windows.Forms.DataGridViewCheckBoxColumn col_uq;
        private System.Windows.Forms.DataGridViewCheckBoxColumn col_index;
        private System.Windows.Forms.DataGridViewCheckBoxColumn col_fk;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_fktabla;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_fkcol;
        private System.Windows.Forms.DataGridViewCheckBoxColumn col_fkreferenced;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.Button bElegirOutput;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}