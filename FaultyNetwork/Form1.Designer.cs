namespace FaultyNetwork
{
    partial class Form1
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
            this.outputBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.corruptionRateBox = new System.Windows.Forms.NumericUpDown();
            this.dropRateBox = new System.Windows.Forms.NumericUpDown();
            this.printCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.corruptionRateBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dropRateBox)).BeginInit();
            this.SuspendLayout();
            // 
            // outputBox
            // 
            this.outputBox.Location = new System.Drawing.Point(8, 8);
            this.outputBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.outputBox.Multiline = true;
            this.outputBox.Name = "outputBox";
            this.outputBox.Size = new System.Drawing.Size(519, 224);
            this.outputBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 263);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Corruption Rate:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(162, 263);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Drop Rate:";
            // 
            // corruptionRateBox
            // 
            this.corruptionRateBox.Location = new System.Drawing.Point(93, 261);
            this.corruptionRateBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.corruptionRateBox.Name = "corruptionRateBox";
            this.corruptionRateBox.Size = new System.Drawing.Size(55, 20);
            this.corruptionRateBox.TabIndex = 2;
            this.corruptionRateBox.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // dropRateBox
            // 
            this.dropRateBox.Location = new System.Drawing.Point(224, 261);
            this.dropRateBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dropRateBox.Name = "dropRateBox";
            this.dropRateBox.Size = new System.Drawing.Size(55, 20);
            this.dropRateBox.TabIndex = 2;
            this.dropRateBox.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // printCheckBox
            // 
            this.printCheckBox.AutoSize = true;
            this.printCheckBox.Checked = true;
            this.printCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.printCheckBox.Location = new System.Drawing.Point(9, 235);
            this.printCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.printCheckBox.Name = "printCheckBox";
            this.printCheckBox.Size = new System.Drawing.Size(82, 17);
            this.printCheckBox.TabIndex = 3;
            this.printCheckBox.Text = "Print Output";
            this.printCheckBox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 292);
            this.Controls.Add(this.printCheckBox);
            this.Controls.Add(this.dropRateBox);
            this.Controls.Add(this.corruptionRateBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.outputBox);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Faulty Network";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.corruptionRateBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dropRateBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox outputBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown corruptionRateBox;
        private System.Windows.Forms.NumericUpDown dropRateBox;
        private System.Windows.Forms.CheckBox printCheckBox;
    }
}

