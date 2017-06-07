namespace AMSExplorer
{
    partial class ConfigureWebhook
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigureWebhook));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonDeleteConfig = new System.Windows.Forms.Button();
            this.label33 = new System.Windows.Forms.Label();
            this.labelWebhookUI = new System.Windows.Forms.Label();
            this.comboBoxJobState = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxEndpointURL = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.moreinfoNotificationEndPointlink = new System.Windows.Forms.LinkLabel();
            this.comboBoxEndPointType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxTaskProgress = new System.Windows.Forms.CheckBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            resources.ApplyResources(this.buttonOk, "buttonOk");
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.buttonDeleteConfig);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonOk);
            this.panel1.Name = "panel1";
            // 
            // buttonDeleteConfig
            // 
            resources.ApplyResources(this.buttonDeleteConfig, "buttonDeleteConfig");
            this.buttonDeleteConfig.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.buttonDeleteConfig.Name = "buttonDeleteConfig";
            this.buttonDeleteConfig.UseVisualStyleBackColor = true;
            // 
            // label33
            // 
            resources.ApplyResources(this.label33, "label33");
            this.label33.Name = "label33";
            // 
            // labelWebhookUI
            // 
            this.labelWebhookUI.ForeColor = System.Drawing.Color.DarkBlue;
            resources.ApplyResources(this.labelWebhookUI, "labelWebhookUI");
            this.labelWebhookUI.Name = "labelWebhookUI";
            // 
            // comboBoxJobState
            // 
            this.comboBoxJobState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxJobState.FormattingEnabled = true;
            resources.ApplyResources(this.comboBoxJobState, "comboBoxJobState");
            this.comboBoxJobState.Name = "comboBoxJobState";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // textBoxEndpointURL
            // 
            resources.ApplyResources(this.textBoxEndpointURL, "textBoxEndpointURL");
            this.textBoxEndpointURL.Name = "textBoxEndpointURL";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // moreinfoNotificationEndPointlink
            // 
            resources.ApplyResources(this.moreinfoNotificationEndPointlink, "moreinfoNotificationEndPointlink");
            this.moreinfoNotificationEndPointlink.Name = "moreinfoNotificationEndPointlink";
            this.moreinfoNotificationEndPointlink.TabStop = true;
            this.moreinfoNotificationEndPointlink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.moreinfoNotificationEndPointlink_LinkClicked);
            // 
            // comboBoxEndPointType
            // 
            this.comboBoxEndPointType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEndPointType.FormattingEnabled = true;
            resources.ApplyResources(this.comboBoxEndPointType, "comboBoxEndPointType");
            this.comboBoxEndPointType.Name = "comboBoxEndPointType";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // checkBoxTaskProgress
            // 
            resources.ApplyResources(this.checkBoxTaskProgress, "checkBoxTaskProgress");
            this.checkBoxTaskProgress.Name = "checkBoxTaskProgress";
            this.checkBoxTaskProgress.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // ConfigureWebhook
            // 
            this.AcceptButton = this.buttonOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.checkBoxTaskProgress);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxEndPointType);
            this.Controls.Add(this.moreinfoNotificationEndPointlink);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxJobState);
            this.Controls.Add(this.labelWebhookUI);
            this.Controls.Add(this.label33);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBoxEndpointURL);
            this.Name = "ConfigureWebhook";
            this.Load += new System.EventHandler(this.ConfigureWebhook_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button buttonOk;
        public System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label labelWebhookUI;
        private System.Windows.Forms.ComboBox comboBoxJobState;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button buttonDeleteConfig;
        private System.Windows.Forms.TextBox textBoxEndpointURL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel moreinfoNotificationEndPointlink;
        private System.Windows.Forms.ComboBox comboBoxEndPointType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxTaskProgress;
    }
}