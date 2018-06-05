namespace ProjectoCompiladoresB
{
    partial class TablaANS
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tablaAS = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // tablaAS
            // 
            this.tablaAS.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tablaAS.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.tablaAS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tablaAS.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.tablaAS.GridLines = true;
            this.tablaAS.Location = new System.Drawing.Point(12, 12);
            this.tablaAS.Name = "tablaAS";
            this.tablaAS.Size = new System.Drawing.Size(1229, 456);
            this.tablaAS.TabIndex = 2;
            this.tablaAS.UseCompatibleStateImageBehavior = false;
            this.tablaAS.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Estado";
            // 
            // TablaANS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1253, 480);
            this.Controls.Add(this.tablaAS);
            this.Name = "TablaANS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "TablaANS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.TablaANS_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView tablaAS;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}