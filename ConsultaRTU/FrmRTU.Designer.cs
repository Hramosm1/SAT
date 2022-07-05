
namespace ConsultaRTU
{
    partial class FrmRTU
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRTU));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dataGrid = new Bunifu.UI.WinForms.BunifuDataGridView();
            this.ePERSONASRTUBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.bunifuCustomLabel1 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.btnDescargar = new Bunifu.Framework.UI.BunifuImageButton();
            this.btnProcesar = new Bunifu.Framework.UI.BunifuImageButton();
            this.btnConsultaLocal = new Bunifu.Framework.UI.BunifuImageButton();
            this.bunifuToolTip1 = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            this.nitDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dpiDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.primernombreDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.segundonombreDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.primerapellidoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.segundoapellidoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.apellidocasadaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cedulaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fechaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sexoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nacionalidadDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.estadocivilDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ultimaactualizacionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ePERSONASRTUBindingSource)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnDescargar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnProcesar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConsultaLocal)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1451, 722);
            this.panel1.TabIndex = 0;
            this.bunifuToolTip1.SetToolTip(this.panel1, "");
            this.bunifuToolTip1.SetToolTipIcon(this.panel1, null);
            this.bunifuToolTip1.SetToolTipTitle(this.panel1, "");
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.dataGrid);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 70);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1451, 652);
            this.panel3.TabIndex = 1;
            this.bunifuToolTip1.SetToolTip(this.panel3, "");
            this.bunifuToolTip1.SetToolTipIcon(this.panel3, null);
            this.bunifuToolTip1.SetToolTipTitle(this.panel3, "");
            // 
            // dataGrid
            // 
            this.dataGrid.AllowCustomTheming = false;
            this.dataGrid.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            this.dataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGrid.AutoGenerateColumns = false;
            this.dataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGrid.BackgroundColor = System.Drawing.Color.White;
            this.dataGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGrid.ColumnHeadersHeight = 40;
            this.dataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nitDataGridViewTextBoxColumn,
            this.dpiDataGridViewTextBoxColumn,
            this.nombre,
            this.primernombreDataGridViewTextBoxColumn,
            this.segundonombreDataGridViewTextBoxColumn,
            this.primerapellidoDataGridViewTextBoxColumn,
            this.segundoapellidoDataGridViewTextBoxColumn,
            this.apellidocasadaDataGridViewTextBoxColumn,
            this.cedulaDataGridViewTextBoxColumn,
            this.fechaDataGridViewTextBoxColumn,
            this.sexoDataGridViewTextBoxColumn,
            this.nacionalidadDataGridViewTextBoxColumn,
            this.estadocivilDataGridViewTextBoxColumn,
            this.ultimaactualizacionDataGridViewTextBoxColumn});
            this.dataGrid.CurrentTheme.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.dataGrid.CurrentTheme.AlternatingRowsStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.dataGrid.CurrentTheme.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGrid.CurrentTheme.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
            this.dataGrid.CurrentTheme.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGrid.CurrentTheme.BackColor = System.Drawing.Color.White;
            this.dataGrid.CurrentTheme.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
            this.dataGrid.CurrentTheme.HeaderStyle.BackColor = System.Drawing.Color.DodgerBlue;
            this.dataGrid.CurrentTheme.HeaderStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.75F, System.Drawing.FontStyle.Bold);
            this.dataGrid.CurrentTheme.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dataGrid.CurrentTheme.Name = null;
            this.dataGrid.CurrentTheme.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dataGrid.CurrentTheme.RowsStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.dataGrid.CurrentTheme.RowsStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGrid.CurrentTheme.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
            this.dataGrid.CurrentTheme.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGrid.DataSource = this.ePERSONASRTUBindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGrid.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid.EnableHeadersVisualStyles = false;
            this.dataGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
            this.dataGrid.HeaderBackColor = System.Drawing.Color.DodgerBlue;
            this.dataGrid.HeaderBgColor = System.Drawing.Color.Empty;
            this.dataGrid.HeaderForeColor = System.Drawing.Color.White;
            this.dataGrid.Location = new System.Drawing.Point(0, 0);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.RowHeadersVisible = false;
            this.dataGrid.RowHeadersWidth = 51;
            this.dataGrid.RowTemplate.Height = 40;
            this.dataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid.Size = new System.Drawing.Size(1451, 652);
            this.dataGrid.TabIndex = 0;
            this.dataGrid.Theme = Bunifu.UI.WinForms.BunifuDataGridView.PresetThemes.Light;
            this.bunifuToolTip1.SetToolTip(this.dataGrid, "");
            this.bunifuToolTip1.SetToolTipIcon(this.dataGrid, null);
            this.bunifuToolTip1.SetToolTipTitle(this.dataGrid, "");
            // 
            // ePERSONASRTUBindingSource
            // 
            this.ePERSONASRTUBindingSource.DataSource = typeof(ConsultaRTU.Entidades.E_PERSONAS_RTU);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(128)))), ((int)(((byte)(159)))));
            this.panel2.Controls.Add(this.bunifuCustomLabel1);
            this.panel2.Controls.Add(this.btnDescargar);
            this.panel2.Controls.Add(this.btnProcesar);
            this.panel2.Controls.Add(this.btnConsultaLocal);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1451, 70);
            this.panel2.TabIndex = 0;
            this.bunifuToolTip1.SetToolTip(this.panel2, "");
            this.bunifuToolTip1.SetToolTipIcon(this.panel2, null);
            this.bunifuToolTip1.SetToolTipTitle(this.panel2, "");
            // 
            // bunifuCustomLabel1
            // 
            this.bunifuCustomLabel1.AutoSize = true;
            this.bunifuCustomLabel1.Font = new System.Drawing.Font("Comic Sans MS", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuCustomLabel1.ForeColor = System.Drawing.Color.White;
            this.bunifuCustomLabel1.Location = new System.Drawing.Point(33, 12);
            this.bunifuCustomLabel1.Name = "bunifuCustomLabel1";
            this.bunifuCustomLabel1.Size = new System.Drawing.Size(214, 48);
            this.bunifuCustomLabel1.TabIndex = 5;
            this.bunifuCustomLabel1.Text = "Buscar RTU";
            this.bunifuToolTip1.SetToolTip(this.bunifuCustomLabel1, "");
            this.bunifuToolTip1.SetToolTipIcon(this.bunifuCustomLabel1, null);
            this.bunifuToolTip1.SetToolTipTitle(this.bunifuCustomLabel1, "");
            // 
            // btnDescargar
            // 
            this.btnDescargar.Enabled = false;
            this.btnDescargar.Image = ((System.Drawing.Image)(resources.GetObject("btnDescargar.Image")));
            this.btnDescargar.ImageActive = null;
            this.btnDescargar.Location = new System.Drawing.Point(1357, 12);
            this.btnDescargar.Name = "btnDescargar";
            this.btnDescargar.Size = new System.Drawing.Size(56, 45);
            this.btnDescargar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnDescargar.TabIndex = 4;
            this.btnDescargar.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.btnDescargar, "Descargar Informacion");
            this.bunifuToolTip1.SetToolTipIcon(this.btnDescargar, null);
            this.bunifuToolTip1.SetToolTipTitle(this.btnDescargar, "Descargar Informacion");
            this.btnDescargar.Zoom = 10;
            this.btnDescargar.Click += new System.EventHandler(this.bunifuImageButton2_Click);
            // 
            // btnProcesar
            // 
            this.btnProcesar.Enabled = false;
            this.btnProcesar.Image = ((System.Drawing.Image)(resources.GetObject("btnProcesar.Image")));
            this.btnProcesar.ImageActive = null;
            this.btnProcesar.Location = new System.Drawing.Point(1278, 12);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(56, 45);
            this.btnProcesar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnProcesar.TabIndex = 3;
            this.btnProcesar.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.btnProcesar, "Procesar Archivo");
            this.bunifuToolTip1.SetToolTipIcon(this.btnProcesar, null);
            this.bunifuToolTip1.SetToolTipTitle(this.btnProcesar, "Procesar Archivo");
            this.btnProcesar.Zoom = 10;
            this.btnProcesar.Click += new System.EventHandler(this.bunifuImageButton1_Click);
            // 
            // btnConsultaLocal
            // 
            this.btnConsultaLocal.Image = ((System.Drawing.Image)(resources.GetObject("btnConsultaLocal.Image")));
            this.btnConsultaLocal.ImageActive = null;
            this.btnConsultaLocal.Location = new System.Drawing.Point(1194, 12);
            this.btnConsultaLocal.Name = "btnConsultaLocal";
            this.btnConsultaLocal.Size = new System.Drawing.Size(56, 45);
            this.btnConsultaLocal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnConsultaLocal.TabIndex = 2;
            this.btnConsultaLocal.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.btnConsultaLocal, "Cargar NITs");
            this.bunifuToolTip1.SetToolTipIcon(this.btnConsultaLocal, null);
            this.bunifuToolTip1.SetToolTipTitle(this.btnConsultaLocal, "Cargar NITs");
            this.btnConsultaLocal.Zoom = 10;
            this.btnConsultaLocal.Click += new System.EventHandler(this.btnConsultaLocal_Click);
            // 
            // bunifuToolTip1
            // 
            this.bunifuToolTip1.Active = true;
            this.bunifuToolTip1.AlignTextWithTitle = false;
            this.bunifuToolTip1.AllowAutoClose = false;
            this.bunifuToolTip1.AllowFading = true;
            this.bunifuToolTip1.AutoCloseDuration = 5000;
            this.bunifuToolTip1.BackColor = System.Drawing.SystemColors.Control;
            this.bunifuToolTip1.BorderColor = System.Drawing.Color.Gainsboro;
            this.bunifuToolTip1.ClickToShowDisplayControl = false;
            this.bunifuToolTip1.ConvertNewlinesToBreakTags = true;
            this.bunifuToolTip1.DisplayControl = null;
            this.bunifuToolTip1.EntryAnimationSpeed = 350;
            this.bunifuToolTip1.ExitAnimationSpeed = 200;
            this.bunifuToolTip1.GenerateAutoCloseDuration = false;
            this.bunifuToolTip1.IconMargin = 6;
            this.bunifuToolTip1.InitialDelay = 0;
            this.bunifuToolTip1.Name = "bunifuToolTip1";
            this.bunifuToolTip1.Opacity = 1D;
            this.bunifuToolTip1.OverrideToolTipTitles = false;
            this.bunifuToolTip1.Padding = new System.Windows.Forms.Padding(10);
            this.bunifuToolTip1.ReshowDelay = 100;
            this.bunifuToolTip1.ShowAlways = true;
            this.bunifuToolTip1.ShowBorders = false;
            this.bunifuToolTip1.ShowIcons = true;
            this.bunifuToolTip1.ShowShadows = true;
            this.bunifuToolTip1.Tag = null;
            this.bunifuToolTip1.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            this.bunifuToolTip1.TextForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.bunifuToolTip1.TextMargin = 2;
            this.bunifuToolTip1.TitleFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.bunifuToolTip1.TitleForeColor = System.Drawing.Color.Black;
            this.bunifuToolTip1.ToolTipPosition = new System.Drawing.Point(0, 0);
            this.bunifuToolTip1.ToolTipTitle = null;
            // 
            // nitDataGridViewTextBoxColumn
            // 
            this.nitDataGridViewTextBoxColumn.DataPropertyName = "nit";
            this.nitDataGridViewTextBoxColumn.FillWeight = 75F;
            this.nitDataGridViewTextBoxColumn.HeaderText = "NIT";
            this.nitDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.nitDataGridViewTextBoxColumn.Name = "nitDataGridViewTextBoxColumn";
            // 
            // dpiDataGridViewTextBoxColumn
            // 
            this.dpiDataGridViewTextBoxColumn.DataPropertyName = "dpi";
            this.dpiDataGridViewTextBoxColumn.FillWeight = 75F;
            this.dpiDataGridViewTextBoxColumn.HeaderText = "DPI";
            this.dpiDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.dpiDataGridViewTextBoxColumn.Name = "dpiDataGridViewTextBoxColumn";
            // 
            // nombre
            // 
            this.nombre.DataPropertyName = "nombre";
            this.nombre.FillWeight = 250F;
            this.nombre.HeaderText = "Nombre";
            this.nombre.MinimumWidth = 6;
            this.nombre.Name = "nombre";
            this.nombre.ReadOnly = true;
            // 
            // primernombreDataGridViewTextBoxColumn
            // 
            this.primernombreDataGridViewTextBoxColumn.DataPropertyName = "primer_nombre";
            this.primernombreDataGridViewTextBoxColumn.HeaderText = "primer_nombre";
            this.primernombreDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.primernombreDataGridViewTextBoxColumn.Name = "primernombreDataGridViewTextBoxColumn";
            this.primernombreDataGridViewTextBoxColumn.Visible = false;
            // 
            // segundonombreDataGridViewTextBoxColumn
            // 
            this.segundonombreDataGridViewTextBoxColumn.DataPropertyName = "segundo_nombre";
            this.segundonombreDataGridViewTextBoxColumn.HeaderText = "segundo_nombre";
            this.segundonombreDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.segundonombreDataGridViewTextBoxColumn.Name = "segundonombreDataGridViewTextBoxColumn";
            this.segundonombreDataGridViewTextBoxColumn.Visible = false;
            // 
            // primerapellidoDataGridViewTextBoxColumn
            // 
            this.primerapellidoDataGridViewTextBoxColumn.DataPropertyName = "primer_apellido";
            this.primerapellidoDataGridViewTextBoxColumn.HeaderText = "primer_apellido";
            this.primerapellidoDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.primerapellidoDataGridViewTextBoxColumn.Name = "primerapellidoDataGridViewTextBoxColumn";
            this.primerapellidoDataGridViewTextBoxColumn.Visible = false;
            // 
            // segundoapellidoDataGridViewTextBoxColumn
            // 
            this.segundoapellidoDataGridViewTextBoxColumn.DataPropertyName = "segundo_apellido";
            this.segundoapellidoDataGridViewTextBoxColumn.HeaderText = "segundo_apellido";
            this.segundoapellidoDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.segundoapellidoDataGridViewTextBoxColumn.Name = "segundoapellidoDataGridViewTextBoxColumn";
            this.segundoapellidoDataGridViewTextBoxColumn.Visible = false;
            // 
            // apellidocasadaDataGridViewTextBoxColumn
            // 
            this.apellidocasadaDataGridViewTextBoxColumn.DataPropertyName = "apellido_casada";
            this.apellidocasadaDataGridViewTextBoxColumn.HeaderText = "apellido_casada";
            this.apellidocasadaDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.apellidocasadaDataGridViewTextBoxColumn.Name = "apellidocasadaDataGridViewTextBoxColumn";
            this.apellidocasadaDataGridViewTextBoxColumn.Visible = false;
            // 
            // cedulaDataGridViewTextBoxColumn
            // 
            this.cedulaDataGridViewTextBoxColumn.DataPropertyName = "cedula";
            this.cedulaDataGridViewTextBoxColumn.FillWeight = 75F;
            this.cedulaDataGridViewTextBoxColumn.HeaderText = "CEDULA";
            this.cedulaDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.cedulaDataGridViewTextBoxColumn.Name = "cedulaDataGridViewTextBoxColumn";
            this.cedulaDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fechaDataGridViewTextBoxColumn
            // 
            this.fechaDataGridViewTextBoxColumn.DataPropertyName = "fecha";
            this.fechaDataGridViewTextBoxColumn.HeaderText = "Fecha Nacimiento";
            this.fechaDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.fechaDataGridViewTextBoxColumn.Name = "fechaDataGridViewTextBoxColumn";
            this.fechaDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sexoDataGridViewTextBoxColumn
            // 
            this.sexoDataGridViewTextBoxColumn.DataPropertyName = "sexo";
            this.sexoDataGridViewTextBoxColumn.HeaderText = "GENERO";
            this.sexoDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.sexoDataGridViewTextBoxColumn.Name = "sexoDataGridViewTextBoxColumn";
            this.sexoDataGridViewTextBoxColumn.ReadOnly = true;
            this.sexoDataGridViewTextBoxColumn.Visible = false;
            // 
            // nacionalidadDataGridViewTextBoxColumn
            // 
            this.nacionalidadDataGridViewTextBoxColumn.DataPropertyName = "nacionalidad";
            this.nacionalidadDataGridViewTextBoxColumn.HeaderText = "NACIONALIDAD";
            this.nacionalidadDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.nacionalidadDataGridViewTextBoxColumn.Name = "nacionalidadDataGridViewTextBoxColumn";
            this.nacionalidadDataGridViewTextBoxColumn.ReadOnly = true;
            this.nacionalidadDataGridViewTextBoxColumn.Visible = false;
            // 
            // estadocivilDataGridViewTextBoxColumn
            // 
            this.estadocivilDataGridViewTextBoxColumn.DataPropertyName = "estado_civil";
            this.estadocivilDataGridViewTextBoxColumn.HeaderText = "ESTADO CIVIL";
            this.estadocivilDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.estadocivilDataGridViewTextBoxColumn.Name = "estadocivilDataGridViewTextBoxColumn";
            this.estadocivilDataGridViewTextBoxColumn.ReadOnly = true;
            this.estadocivilDataGridViewTextBoxColumn.Visible = false;
            // 
            // ultimaactualizacionDataGridViewTextBoxColumn
            // 
            this.ultimaactualizacionDataGridViewTextBoxColumn.DataPropertyName = "ultima_actualizacion";
            this.ultimaactualizacionDataGridViewTextBoxColumn.FillWeight = 175F;
            this.ultimaactualizacionDataGridViewTextBoxColumn.HeaderText = "Ultima Actualizacion";
            this.ultimaactualizacionDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.ultimaactualizacionDataGridViewTextBoxColumn.Name = "ultimaactualizacionDataGridViewTextBoxColumn";
            this.ultimaactualizacionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // FrmRTU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1451, 722);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmRTU";
            this.Text = "Consulta  por DPI";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ePERSONASRTUBindingSource)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnDescargar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnProcesar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConsultaLocal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private Bunifu.UI.WinForms.BunifuDataGridView dataGrid;
        private Bunifu.Framework.UI.BunifuImageButton btnProcesar;
        private Bunifu.Framework.UI.BunifuImageButton btnConsultaLocal;
        private Bunifu.Framework.UI.BunifuImageButton btnDescargar;
        private Bunifu.UI.WinForms.BunifuToolTip bunifuToolTip1;
        private Bunifu.Framework.UI.BunifuCustomLabel bunifuCustomLabel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombreDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource ePERSONASRTUBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn nitDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dpiDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn primernombreDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn segundonombreDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn primerapellidoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn segundoapellidoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn apellidocasadaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cedulaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fechaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sexoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nacionalidadDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn estadocivilDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ultimaactualizacionDataGridViewTextBoxColumn;
    }
}