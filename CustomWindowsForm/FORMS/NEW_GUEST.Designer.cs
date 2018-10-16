namespace CustomWindowsForm.FORMS
{
    partial class NEW_GUEST
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
            this.TopPanel = new System.Windows.Forms.Panel();
            this._MinButton = new CustomWindowsForm.ButtonZ();
            this._MaxButton = new CustomWindowsForm.MinMaxButton();
            this._CloseButton = new CustomWindowsForm.ButtonZ();
            this.lbl_heder = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.BTN_NEW = new CustomWindowsForm.ShapedButton();
            this.BTN_SAVE = new CustomWindowsForm.ShapedButton();
            this.CMB_COUNTRY = new CustomWindowsForm.hyflexComboBox();
            this.CMB_GENDER = new CustomWindowsForm.hyflexComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.LBL_GUEST = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TXT_LNAME = new CustomWindowsForm.hyflexTextbox();
            this.TXT_EMAIL = new CustomWindowsForm.hyflexTextbox();
            this.TXT_MOBILENO = new CustomWindowsForm.hyflexTextbox();
            this.TXT_PASSPORT = new CustomWindowsForm.hyflexTextbox();
            this.TXT_ID_NUMBER = new CustomWindowsForm.hyflexTextbox();
            this.TXT_ADDRESS = new CustomWindowsForm.hyflexTextbox();
            this.TXT_FNAME = new CustomWindowsForm.hyflexTextbox();
            this.TopPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.TopPanel.Controls.Add(this._MinButton);
            this.TopPanel.Controls.Add(this._MaxButton);
            this.TopPanel.Controls.Add(this._CloseButton);
            this.TopPanel.Controls.Add(this.lbl_heder);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(589, 30);
            this.TopPanel.TabIndex = 5;
            this.TopPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TopPanel_MouseDown);
            this.TopPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TopPanel_MouseMove);
            this.TopPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TopPanel_MouseUp);
            // 
            // _MinButton
            // 
            this._MinButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._MinButton.BZBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this._MinButton.DisplayText = "_";
            this._MinButton.Enabled = false;
            this._MinButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._MinButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._MinButton.ForeColor = System.Drawing.Color.White;
            this._MinButton.Location = new System.Drawing.Point(505, 6);
            this._MinButton.MouseClickColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(160)))));
            this._MinButton.MouseHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this._MinButton.Name = "_MinButton";
            this._MinButton.Size = new System.Drawing.Size(31, 24);
            this._MinButton.TabIndex = 6;
            this._MinButton.Text = "_";
            this._MinButton.TextLocation_X = 6;
            this._MinButton.TextLocation_Y = -20;
            this._MinButton.UseVisualStyleBackColor = true;
            this._MinButton.Visible = false;
            // 
            // _MaxButton
            // 
            this._MaxButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._MaxButton.BZBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this._MaxButton.CFormState = CustomWindowsForm.MinMaxButton.CustomFormState.Normal;
            this._MaxButton.DisplayText = "_";
            this._MaxButton.Enabled = false;
            this._MaxButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._MaxButton.ForeColor = System.Drawing.Color.White;
            this._MaxButton.Location = new System.Drawing.Point(536, 6);
            this._MaxButton.MouseClickColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(160)))));
            this._MaxButton.MouseHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this._MaxButton.Name = "_MaxButton";
            this._MaxButton.Size = new System.Drawing.Size(31, 24);
            this._MaxButton.TabIndex = 5;
            this._MaxButton.Text = "minMaxButton1";
            this._MaxButton.TextLocation_X = 8;
            this._MaxButton.TextLocation_Y = 6;
            this._MaxButton.UseVisualStyleBackColor = true;
            this._MaxButton.Visible = false;
            // 
            // _CloseButton
            // 
            this._CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._CloseButton.BZBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this._CloseButton.DisplayText = "X";
            this._CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._CloseButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._CloseButton.ForeColor = System.Drawing.Color.White;
            this._CloseButton.Location = new System.Drawing.Point(559, 3);
            this._CloseButton.MouseClickColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(160)))));
            this._CloseButton.MouseHoverColor = System.Drawing.Color.Red;
            this._CloseButton.Name = "_CloseButton";
            this._CloseButton.Size = new System.Drawing.Size(27, 29);
            this._CloseButton.TabIndex = 0;
            this._CloseButton.Text = "X";
            this._CloseButton.TextLocation_X = 6;
            this._CloseButton.TextLocation_Y = 1;
            this._CloseButton.UseVisualStyleBackColor = true;
            this._CloseButton.Click += new System.EventHandler(this._CloseButton_Click);
            // 
            // lbl_heder
            // 
            this.lbl_heder.AutoSize = true;
            this.lbl_heder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbl_heder.ForeColor = System.Drawing.Color.White;
            this.lbl_heder.Location = new System.Drawing.Point(3, 8);
            this.lbl_heder.Name = "lbl_heder";
            this.lbl_heder.Size = new System.Drawing.Size(110, 20);
            this.lbl_heder.TabIndex = 1;
            this.lbl_heder.Text = " NEW GUEST";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Location = new System.Drawing.Point(587, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(2, 316);
            this.label1.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(0, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(2, 316);
            this.label2.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Location = new System.Drawing.Point(2, 344);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(585, 2);
            this.label3.TabIndex = 12;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(2, 329);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(585, 15);
            this.panel2.TabIndex = 13;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.BTN_NEW);
            this.groupBox2.Controls.Add(this.BTN_SAVE);
            this.groupBox2.Controls.Add(this.CMB_COUNTRY);
            this.groupBox2.Controls.Add(this.CMB_GENDER);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.LBL_GUEST);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.TXT_LNAME);
            this.groupBox2.Controls.Add(this.TXT_EMAIL);
            this.groupBox2.Controls.Add(this.TXT_MOBILENO);
            this.groupBox2.Controls.Add(this.TXT_PASSPORT);
            this.groupBox2.Controls.Add(this.TXT_ID_NUMBER);
            this.groupBox2.Controls.Add(this.TXT_ADDRESS);
            this.groupBox2.Controls.Add(this.TXT_FNAME);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(7, 36);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(568, 287);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "GUEST INFORMATION";
            // 
            // BTN_NEW
            // 
            this.BTN_NEW.BackColor = System.Drawing.Color.Transparent;
            this.BTN_NEW.BorderColor = System.Drawing.Color.Transparent;
            this.BTN_NEW.BorderWidth = 1;
            this.BTN_NEW.ButtonShape = CustomWindowsForm.ShapedButton.ButtonsShapes.RoundRect;
            this.BTN_NEW.ButtonText = "";
            this.BTN_NEW.EndColor = System.Drawing.Color.Navy;
            this.BTN_NEW.FlatAppearance.BorderSize = 0;
            this.BTN_NEW.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BTN_NEW.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BTN_NEW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_NEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.BTN_NEW.ForeColor = System.Drawing.Color.White;
            this.BTN_NEW.GradientAngle = 90;
            this.BTN_NEW.Location = new System.Drawing.Point(445, 22);
            this.BTN_NEW.MouseClickColor1 = System.Drawing.Color.Yellow;
            this.BTN_NEW.MouseClickColor2 = System.Drawing.Color.Red;
            this.BTN_NEW.MouseHoverColor1 = System.Drawing.Color.Turquoise;
            this.BTN_NEW.MouseHoverColor2 = System.Drawing.Color.DarkSlateGray;
            this.BTN_NEW.Name = "BTN_NEW";
            this.BTN_NEW.ShowButtontext = true;
            this.BTN_NEW.Size = new System.Drawing.Size(108, 41);
            this.BTN_NEW.StartColor = System.Drawing.Color.Fuchsia;
            this.BTN_NEW.TabIndex = 10;
            this.BTN_NEW.Text = "NEW";
            this.BTN_NEW.TextLocation_X = 35;
            this.BTN_NEW.TextLocation_Y = 18;
            this.BTN_NEW.Transparent1 = 25;
            this.BTN_NEW.Transparent2 = 250;
            this.BTN_NEW.UseVisualStyleBackColor = false;
            this.BTN_NEW.Click += new System.EventHandler(this.BTN_NEW_Click);
            // 
            // BTN_SAVE
            // 
            this.BTN_SAVE.BackColor = System.Drawing.Color.Transparent;
            this.BTN_SAVE.BorderColor = System.Drawing.Color.Transparent;
            this.BTN_SAVE.BorderWidth = 1;
            this.BTN_SAVE.ButtonShape = CustomWindowsForm.ShapedButton.ButtonsShapes.RoundRect;
            this.BTN_SAVE.ButtonText = "";
            this.BTN_SAVE.EndColor = System.Drawing.Color.Navy;
            this.BTN_SAVE.FlatAppearance.BorderSize = 0;
            this.BTN_SAVE.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BTN_SAVE.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BTN_SAVE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_SAVE.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_SAVE.ForeColor = System.Drawing.Color.White;
            this.BTN_SAVE.GradientAngle = 90;
            this.BTN_SAVE.Location = new System.Drawing.Point(450, 238);
            this.BTN_SAVE.MouseClickColor1 = System.Drawing.Color.Yellow;
            this.BTN_SAVE.MouseClickColor2 = System.Drawing.Color.Red;
            this.BTN_SAVE.MouseHoverColor1 = System.Drawing.Color.Turquoise;
            this.BTN_SAVE.MouseHoverColor2 = System.Drawing.Color.DarkSlateGray;
            this.BTN_SAVE.Name = "BTN_SAVE";
            this.BTN_SAVE.ShowButtontext = true;
            this.BTN_SAVE.Size = new System.Drawing.Size(103, 40);
            this.BTN_SAVE.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.BTN_SAVE.TabIndex = 9;
            this.BTN_SAVE.Text = "SAVE";
            this.BTN_SAVE.TextLocation_X = 33;
            this.BTN_SAVE.TextLocation_Y = 18;
            this.BTN_SAVE.Transparent1 = 10;
            this.BTN_SAVE.Transparent2 = 250;
            this.BTN_SAVE.UseVisualStyleBackColor = false;
            this.BTN_SAVE.Click += new System.EventHandler(this.BTN_SAVE_Click);
            // 
            // CMB_COUNTRY
            // 
            this.CMB_COUNTRY.F_color = System.Drawing.Color.LightGreen;
            this.CMB_COUNTRY.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.CMB_COUNTRY.FormattingEnabled = true;
            this.CMB_COUNTRY.Items.AddRange(new object[] {
            "MALE",
            "FEMALE"});
            this.CMB_COUNTRY.Location = new System.Drawing.Point(22, 137);
            this.CMB_COUNTRY.Name = "CMB_COUNTRY";
            this.CMB_COUNTRY.Size = new System.Drawing.Size(169, 25);
            this.CMB_COUNTRY.TabIndex = 4;
            this.CMB_COUNTRY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CMB_COUNTRY_KeyDown);
            // 
            // CMB_GENDER
            // 
            this.CMB_GENDER.F_color = System.Drawing.Color.LightGreen;
            this.CMB_GENDER.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.CMB_GENDER.FormattingEnabled = true;
            this.CMB_GENDER.Items.AddRange(new object[] {
            "MALE",
            "FEMALE"});
            this.CMB_GENDER.Location = new System.Drawing.Point(384, 88);
            this.CMB_GENDER.Name = "CMB_GENDER";
            this.CMB_GENDER.Size = new System.Drawing.Size(169, 25);
            this.CMB_GENDER.TabIndex = 3;
            this.CMB_GENDER.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CMB_GENDER_KeyDown);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(201, 121);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "MOBILE NO :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(383, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "GENDER :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 121);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "COUNTRY:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(201, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "LAST NAME :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(22, 170);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "ADDRESS :";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(276, 170);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(45, 13);
            this.label12.TabIndex = 3;
            this.label12.Text = "EMAIL :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(383, 121);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(90, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "PASSPORT NO :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(201, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "ID NUMBER :";
            // 
            // LBL_GUEST
            // 
            this.LBL_GUEST.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.LBL_GUEST.Font = new System.Drawing.Font("Consolas", 10F);
            this.LBL_GUEST.Location = new System.Drawing.Point(21, 41);
            this.LBL_GUEST.Name = "LBL_GUEST";
            this.LBL_GUEST.Size = new System.Drawing.Size(170, 25);
            this.LBL_GUEST.TabIndex = 3;
            this.LBL_GUEST.Text = "N/A";
            this.LBL_GUEST.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(21, 27);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(64, 13);
            this.label13.TabIndex = 3;
            this.label13.Text = "GUEST ID :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "FIRST NAME :";
            // 
            // TXT_LNAME
            // 
            this.TXT_LNAME.F_color = System.Drawing.Color.LightGreen;
            this.TXT_LNAME.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.TXT_LNAME.Location = new System.Drawing.Point(204, 88);
            this.TXT_LNAME.Name = "TXT_LNAME";
            this.TXT_LNAME.Size = new System.Drawing.Size(169, 25);
            this.TXT_LNAME.TabIndex = 2;
            this.TXT_LNAME.User_null_check = CustomWindowsForm.hyflexTextbox.Resust.False;
            this.TXT_LNAME.User_selection = CustomWindowsForm.hyflexTextbox.String_Type.String;
            this.TXT_LNAME.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TXT_LNAME_KeyDown);
            // 
            // TXT_EMAIL
            // 
            this.TXT_EMAIL.F_color = System.Drawing.Color.LightGreen;
            this.TXT_EMAIL.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.TXT_EMAIL.Location = new System.Drawing.Point(279, 186);
            this.TXT_EMAIL.Multiline = true;
            this.TXT_EMAIL.Name = "TXT_EMAIL";
            this.TXT_EMAIL.Size = new System.Drawing.Size(274, 46);
            this.TXT_EMAIL.TabIndex = 8;
            this.TXT_EMAIL.User_null_check = CustomWindowsForm.hyflexTextbox.Resust.False;
            this.TXT_EMAIL.User_selection = CustomWindowsForm.hyflexTextbox.String_Type.String;
            this.TXT_EMAIL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TXT_EMAIL_KeyDown);
            // 
            // TXT_MOBILENO
            // 
            this.TXT_MOBILENO.F_color = System.Drawing.Color.LightGreen;
            this.TXT_MOBILENO.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.TXT_MOBILENO.Location = new System.Drawing.Point(202, 137);
            this.TXT_MOBILENO.Name = "TXT_MOBILENO";
            this.TXT_MOBILENO.Size = new System.Drawing.Size(169, 25);
            this.TXT_MOBILENO.TabIndex = 5;
            this.TXT_MOBILENO.Text = "0";
            this.TXT_MOBILENO.User_null_check = CustomWindowsForm.hyflexTextbox.Resust.False;
            this.TXT_MOBILENO.User_selection = CustomWindowsForm.hyflexTextbox.String_Type.Numeric;
            this.TXT_MOBILENO.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TXT_MOBILENO_KeyDown);
            // 
            // TXT_PASSPORT
            // 
            this.TXT_PASSPORT.F_color = System.Drawing.Color.LightGreen;
            this.TXT_PASSPORT.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.TXT_PASSPORT.Location = new System.Drawing.Point(384, 137);
            this.TXT_PASSPORT.Name = "TXT_PASSPORT";
            this.TXT_PASSPORT.Size = new System.Drawing.Size(169, 25);
            this.TXT_PASSPORT.TabIndex = 6;
            this.TXT_PASSPORT.User_null_check = CustomWindowsForm.hyflexTextbox.Resust.False;
            this.TXT_PASSPORT.User_selection = CustomWindowsForm.hyflexTextbox.String_Type.String;
            this.TXT_PASSPORT.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TXT_PASSPORT_KeyDown);
            // 
            // TXT_ID_NUMBER
            // 
            this.TXT_ID_NUMBER.F_color = System.Drawing.Color.LightGreen;
            this.TXT_ID_NUMBER.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.TXT_ID_NUMBER.Location = new System.Drawing.Point(204, 41);
            this.TXT_ID_NUMBER.Name = "TXT_ID_NUMBER";
            this.TXT_ID_NUMBER.Size = new System.Drawing.Size(169, 25);
            this.TXT_ID_NUMBER.TabIndex = 0;
            this.TXT_ID_NUMBER.User_null_check = CustomWindowsForm.hyflexTextbox.Resust.False;
            this.TXT_ID_NUMBER.User_selection = CustomWindowsForm.hyflexTextbox.String_Type.String;
            this.TXT_ID_NUMBER.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TXT_ID_NUMBER_KeyDown);
            this.TXT_ID_NUMBER.Leave += new System.EventHandler(this.TXT_ID_NUMBER_Leave);
            // 
            // TXT_ADDRESS
            // 
            this.TXT_ADDRESS.F_color = System.Drawing.Color.LightGreen;
            this.TXT_ADDRESS.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.TXT_ADDRESS.Location = new System.Drawing.Point(21, 186);
            this.TXT_ADDRESS.Multiline = true;
            this.TXT_ADDRESS.Name = "TXT_ADDRESS";
            this.TXT_ADDRESS.Size = new System.Drawing.Size(252, 46);
            this.TXT_ADDRESS.TabIndex = 7;
            this.TXT_ADDRESS.User_null_check = CustomWindowsForm.hyflexTextbox.Resust.False;
            this.TXT_ADDRESS.User_selection = CustomWindowsForm.hyflexTextbox.String_Type.String;
            this.TXT_ADDRESS.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TXT_ADDRESS_KeyDown);
            // 
            // TXT_FNAME
            // 
            this.TXT_FNAME.F_color = System.Drawing.Color.LightGreen;
            this.TXT_FNAME.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.TXT_FNAME.Location = new System.Drawing.Point(22, 88);
            this.TXT_FNAME.Name = "TXT_FNAME";
            this.TXT_FNAME.Size = new System.Drawing.Size(169, 25);
            this.TXT_FNAME.TabIndex = 1;
            this.TXT_FNAME.User_null_check = CustomWindowsForm.hyflexTextbox.Resust.False;
            this.TXT_FNAME.User_selection = CustomWindowsForm.hyflexTextbox.String_Type.String;
            this.TXT_FNAME.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TXT_FNAME_KeyDown);
            // 
            // NEW_GUEST
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.ClientSize = new System.Drawing.Size(589, 346);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TopPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "NEW_GUEST";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MsgBox";
            this.Load += new System.EventHandler(this.NEW_GUEST_Load);
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.Label lbl_heder;
        private ButtonZ _CloseButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private ButtonZ _MinButton;
        private MinMaxButton _MaxButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.GroupBox groupBox2;
        private hyflexComboBox CMB_COUNTRY;
        private hyflexComboBox CMB_GENDER;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private hyflexTextbox TXT_LNAME;
        private hyflexTextbox TXT_EMAIL;
        private hyflexTextbox TXT_PASSPORT;
        private hyflexTextbox TXT_ID_NUMBER;
        private hyflexTextbox TXT_ADDRESS;
        private hyflexTextbox TXT_FNAME;
        private ShapedButton BTN_SAVE;
        private ShapedButton BTN_NEW;
        private hyflexTextbox TXT_MOBILENO;
        private System.Windows.Forms.Label LBL_GUEST;
        private System.Windows.Forms.Label label13;
    }
}