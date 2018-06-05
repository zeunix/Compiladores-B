namespace ProjectoCompiladoresB
{
    partial class Principal
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cadenaCodigo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CompilarBoton = new System.Windows.Forms.Button();
            this.tablaAcciones = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Ejecutar = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.depuradorCuad = new System.Windows.Forms.DataGridView();
            this.tablaSimGrid = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.Variable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Valor_Asignado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operador = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operador_1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operador_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resultado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.depuradorCuad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaSimGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // cadenaCodigo
            // 
            this.cadenaCodigo.AllowDrop = true;
            this.cadenaCodigo.Location = new System.Drawing.Point(12, 57);
            this.cadenaCodigo.Multiline = true;
            this.cadenaCodigo.Name = "cadenaCodigo";
            this.cadenaCodigo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.cadenaCodigo.Size = new System.Drawing.Size(379, 625);
            this.cadenaCodigo.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Codigo:";
            // 
            // CompilarBoton
            // 
            this.CompilarBoton.Location = new System.Drawing.Point(12, 12);
            this.CompilarBoton.Name = "CompilarBoton";
            this.CompilarBoton.Size = new System.Drawing.Size(131, 23);
            this.CompilarBoton.TabIndex = 4;
            this.CompilarBoton.Text = "Compilar";
            this.CompilarBoton.UseVisualStyleBackColor = true;
            this.CompilarBoton.Click += new System.EventHandler(this.CompilarBoton_Click);
            // 
            // tablaAcciones
            // 
            this.tablaAcciones.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.tablaAcciones.Location = new System.Drawing.Point(397, 57);
            this.tablaAcciones.Name = "tablaAcciones";
            this.tablaAcciones.Size = new System.Drawing.Size(479, 625);
            this.tablaAcciones.TabIndex = 5;
            this.tablaAcciones.UseCompatibleStateImageBehavior = false;
            this.tablaAcciones.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Pila";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "token";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Cadena de entrada";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 150;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Accion";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 180;
            // 
            // Ejecutar
            // 
            this.Ejecutar.Enabled = false;
            this.Ejecutar.Location = new System.Drawing.Point(164, 13);
            this.Ejecutar.Name = "Ejecutar";
            this.Ejecutar.Size = new System.Drawing.Size(121, 23);
            this.Ejecutar.TabIndex = 6;
            this.Ejecutar.Text = "Ejecutar";
            this.Ejecutar.UseVisualStyleBackColor = true;
            this.Ejecutar.Click += new System.EventHandler(this.Ejecutar_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(292, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(147, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Tabla ANS";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // depuradorCuad
            // 
            this.depuradorCuad.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.depuradorCuad.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.operador,
            this.operador_1,
            this.operador_2,
            this.resultado});
            this.depuradorCuad.Location = new System.Drawing.Point(882, 57);
            this.depuradorCuad.Name = "depuradorCuad";
            this.depuradorCuad.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.depuradorCuad.Size = new System.Drawing.Size(441, 324);
            this.depuradorCuad.TabIndex = 8;
            // 
            // tablaSimGrid
            // 
            this.tablaSimGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tablaSimGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Variable,
            this.Tipo,
            this.Valor_Asignado});
            this.tablaSimGrid.Location = new System.Drawing.Point(882, 415);
            this.tablaSimGrid.Name = "tablaSimGrid";
            this.tablaSimGrid.Size = new System.Drawing.Size(347, 267);
            this.tablaSimGrid.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(883, 399);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Tabla de Simbolos";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(883, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Depurador";
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(946, 18);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "depurar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Variable
            // 
            this.Variable.HeaderText = "Variable";
            this.Variable.Name = "Variable";
            // 
            // Tipo
            // 
            this.Tipo.HeaderText = "Tipo";
            this.Tipo.Name = "Tipo";
            // 
            // Valor_Asignado
            // 
            this.Valor_Asignado.HeaderText = "Valor Asignado";
            this.Valor_Asignado.Name = "Valor_Asignado";
            // 
            // operador
            // 
            this.operador.HeaderText = "operador";
            this.operador.Name = "operador";
            // 
            // operador_1
            // 
            this.operador_1.HeaderText = "operador 1";
            this.operador_1.Name = "operador_1";
            // 
            // operador_2
            // 
            this.operador_2.HeaderText = "operador 2";
            this.operador_2.Name = "operador_2";
            // 
            // resultado
            // 
            this.resultado.HeaderText = "resultado";
            this.resultado.Name = "resultado";
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(1049, 18);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 13;
            this.button3.Text = "siguiente paso";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1327, 694);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tablaSimGrid);
            this.Controls.Add(this.depuradorCuad);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Ejecutar);
            this.Controls.Add(this.tablaAcciones);
            this.Controls.Add(this.CompilarBoton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cadenaCodigo);
            this.Name = "Principal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Principal";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Principal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.depuradorCuad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaSimGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox cadenaCodigo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CompilarBoton;
        private System.Windows.Forms.ListView tablaAcciones;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button Ejecutar;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView depuradorCuad;
        private System.Windows.Forms.DataGridViewTextBoxColumn operador;
        private System.Windows.Forms.DataGridViewTextBoxColumn operador_1;
        private System.Windows.Forms.DataGridViewTextBoxColumn operador_2;
        private System.Windows.Forms.DataGridViewTextBoxColumn resultado;
        private System.Windows.Forms.DataGridView tablaSimGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Variable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Valor_Asignado;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}

