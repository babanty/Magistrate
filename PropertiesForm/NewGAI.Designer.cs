namespace Magistrate.Forms
{
    partial class NewGAI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewGAI));
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxFullRequisites = new System.Windows.Forms.TextBox();
            this.buttonAddGIBDD = new System.Windows.Forms.Button();
            this.buttonDeleteGIBDD = new System.Windows.Forms.Button();
            this.listBoxNameGIBDD = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxNameGIBDD = new System.Windows.Forms.TextBox();
            this.textBoxStandartYIN = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonChangeNameGIBDD = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(465, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(330, 20);
            this.label4.TabIndex = 85;
            this.label4.Text = "Тут список уже имеющихся вариантов";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(391, 20);
            this.label3.TabIndex = 84;
            this.label3.Text = "В это поле ввода писать реквизиты без УИН";
            // 
            // textBoxFullRequisites
            // 
            this.textBoxFullRequisites.Location = new System.Drawing.Point(12, 145);
            this.textBoxFullRequisites.Multiline = true;
            this.textBoxFullRequisites.Name = "textBoxFullRequisites";
            this.textBoxFullRequisites.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxFullRequisites.Size = new System.Drawing.Size(450, 189);
            this.textBoxFullRequisites.TabIndex = 2;
            // 
            // buttonAddGIBDD
            // 
            this.buttonAddGIBDD.Location = new System.Drawing.Point(12, 394);
            this.buttonAddGIBDD.Margin = new System.Windows.Forms.Padding(4);
            this.buttonAddGIBDD.Name = "buttonAddGIBDD";
            this.buttonAddGIBDD.Size = new System.Drawing.Size(451, 29);
            this.buttonAddGIBDD.TabIndex = 4;
            this.buttonAddGIBDD.Text = "Добавить";
            this.buttonAddGIBDD.UseVisualStyleBackColor = true;
            this.buttonAddGIBDD.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonDeleteGIBDD
            // 
            this.buttonDeleteGIBDD.Location = new System.Drawing.Point(469, 394);
            this.buttonDeleteGIBDD.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDeleteGIBDD.Name = "buttonDeleteGIBDD";
            this.buttonDeleteGIBDD.Size = new System.Drawing.Size(225, 29);
            this.buttonDeleteGIBDD.TabIndex = 7;
            this.buttonDeleteGIBDD.Text = "Удалить";
            this.buttonDeleteGIBDD.UseVisualStyleBackColor = true;
            this.buttonDeleteGIBDD.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBoxNameGIBDD
            // 
            this.listBoxNameGIBDD.FormattingEnabled = true;
            this.listBoxNameGIBDD.ItemHeight = 20;
            this.listBoxNameGIBDD.Location = new System.Drawing.Point(469, 42);
            this.listBoxNameGIBDD.Margin = new System.Windows.Forms.Padding(4);
            this.listBoxNameGIBDD.Name = "listBoxNameGIBDD";
            this.listBoxNameGIBDD.Size = new System.Drawing.Size(450, 344);
            this.listBoxNameGIBDD.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(397, 20);
            this.label1.TabIndex = 86;
            this.label1.Text = "Краткое наименование, например: Наш ГБДД";
            // 
            // textBoxNameGIBDD
            // 
            this.textBoxNameGIBDD.Location = new System.Drawing.Point(12, 42);
            this.textBoxNameGIBDD.Multiline = true;
            this.textBoxNameGIBDD.Name = "textBoxNameGIBDD";
            this.textBoxNameGIBDD.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxNameGIBDD.Size = new System.Drawing.Size(450, 68);
            this.textBoxNameGIBDD.TabIndex = 1;
            // 
            // textBoxStandartYIN
            // 
            this.textBoxStandartYIN.Location = new System.Drawing.Point(12, 360);
            this.textBoxStandartYIN.Name = "textBoxStandartYIN";
            this.textBoxStandartYIN.Size = new System.Drawing.Size(450, 27);
            this.textBoxStandartYIN.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 337);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(233, 20);
            this.label2.TabIndex = 88;
            this.label2.Text = "Не изменная часть УИН-а ";
            // 
            // buttonChangeNameGIBDD
            // 
            this.buttonChangeNameGIBDD.Location = new System.Drawing.Point(702, 394);
            this.buttonChangeNameGIBDD.Margin = new System.Windows.Forms.Padding(4);
            this.buttonChangeNameGIBDD.Name = "buttonChangeNameGIBDD";
            this.buttonChangeNameGIBDD.Size = new System.Drawing.Size(220, 29);
            this.buttonChangeNameGIBDD.TabIndex = 6;
            this.buttonChangeNameGIBDD.Text = "Изменить";
            this.buttonChangeNameGIBDD.UseVisualStyleBackColor = true;
            this.buttonChangeNameGIBDD.Click += new System.EventHandler(this.buttonChange_Click);
            // 
            // NewGAI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(942, 453);
            this.Controls.Add(this.buttonChangeNameGIBDD);
            this.Controls.Add(this.textBoxStandartYIN);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxNameGIBDD);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxFullRequisites);
            this.Controls.Add(this.buttonAddGIBDD);
            this.Controls.Add(this.buttonDeleteGIBDD);
            this.Controls.Add(this.listBoxNameGIBDD);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "NewGAI";
            this.Text = "Новые реквизиты получаетелей штрафов, например ГАИ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxFullRequisites;
        private System.Windows.Forms.Button buttonAddGIBDD;
        private System.Windows.Forms.Button buttonDeleteGIBDD;
        private System.Windows.Forms.ListBox listBoxNameGIBDD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxNameGIBDD;
        private System.Windows.Forms.TextBox textBoxStandartYIN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonChangeNameGIBDD;
    }
}