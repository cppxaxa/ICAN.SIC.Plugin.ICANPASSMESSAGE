namespace ICAN.SIC.Plugin.ICANPASSMESSAGE.Host
{
    partial class ICANPASSMESSAGEHost
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
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnPostMachine = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(12, 12);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(507, 31);
            this.txtMessage.TabIndex = 0;
            this.txtMessage.Text = "Hello";
            // 
            // btnPostMachine
            // 
            this.btnPostMachine.Location = new System.Drawing.Point(391, 67);
            this.btnPostMachine.Name = "btnPostMachine";
            this.btnPostMachine.Size = new System.Drawing.Size(128, 66);
            this.btnPostMachine.TabIndex = 1;
            this.btnPostMachine.Text = "Post";
            this.btnPostMachine.UseVisualStyleBackColor = true;
            this.btnPostMachine.Click += new System.EventHandler(this.btnPostMachine_Click);
            // 
            // ICANPASSMESSAGEHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 567);
            this.Controls.Add(this.btnPostMachine);
            this.Controls.Add(this.txtMessage);
            this.Name = "ICANPASSMESSAGEHost";
            this.Text = "ICANPASSMESSAGEHost";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnPostMachine;
    }
}

