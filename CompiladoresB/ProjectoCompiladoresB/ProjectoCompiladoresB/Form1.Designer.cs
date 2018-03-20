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
            this.cargarTabla = new System.Windows.Forms.Button();
            this.tablaAS = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cadenaCodigo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CompilarBoton = new System.Windows.Forms.Button();
            this.tablaAcciones = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // cargarTabla
            // 
            this.cargarTabla.Location = new System.Drawing.Point(12, 12);
            this.cargarTabla.Name = "cargarTabla";
            this.cargarTabla.Size = new System.Drawing.Size(130, 23);
            this.cargarTabla.TabIndex = 0;
            this.cargarTabla.Text = "Cargar tabla AS";
            this.cargarTabla.UseVisualStyleBackColor = true;
            this.cargarTabla.Click += new System.EventHandler(this.cargarTabla_Click);
            // 
            // tablaAS
            // 
            this.tablaAS.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tablaAS.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.tablaAS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tablaAS.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.tablaAS.GridLines = true;
            this.tablaAS.Location = new System.Drawing.Point(12, 253);
            this.tablaAS.Name = "tablaAS";
            this.tablaAS.Size = new System.Drawing.Size(427, 238);
            this.tablaAS.TabIndex = 1;
            this.tablaAS.UseCompatibleStateImageBehavior = false;
            this.tablaAS.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Estado";
            // 
            // cadenaCodigo
            // 
            this.cadenaCodigo.Location = new System.Drawing.Point(12, 57);
            this.cadenaCodigo.Multiline = true;
            this.cadenaCodigo.Name = "cadenaCodigo";
            this.cadenaCodigo.Size = new System.Drawing.Size(427, 190);
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
            this.CompilarBoton.Location = new System.Drawing.Point(148, 12);
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
            this.tablaAcciones.Location = new System.Drawing.Point(455, 57);
            this.tablaAcciones.Name = "tablaAcciones";
            this.tablaAcciones.Size = new System.Drawing.Size(445, 434);
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
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 503);
            this.Controls.Add(this.tablaAcciones);
            this.Controls.Add(this.CompilarBoton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cadenaCodigo);
            this.Controls.Add(this.tablaAS);
            this.Controls.Add(this.cargarTabla);
            this.Name = "Principal";
            this.Text = "Principal";
            this.Load += new System.EventHandler(this.Principal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button cargarTabla;
        private System.Windows.Forms.ListView tablaAS;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TextBox cadenaCodigo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CompilarBoton;
        private System.Windows.Forms.ListView tablaAcciones;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}

