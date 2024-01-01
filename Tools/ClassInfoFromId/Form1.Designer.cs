namespace ClassInfoFromId
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TextBoxClassId = new TextBox();
            LabelClassName = new Label();
            ButtonCopyName = new Button();
            SuspendLayout();
            // 
            // TextBoxClassId
            // 
            TextBoxClassId.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 238);
            TextBoxClassId.Location = new Point(12, 12);
            TextBoxClassId.Name = "TextBoxClassId";
            TextBoxClassId.Size = new Size(267, 35);
            TextBoxClassId.TabIndex = 0;
            TextBoxClassId.TextChanged += TextBoxClassId_TextChanged;
            // 
            // LabelClassName
            // 
            LabelClassName.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            LabelClassName.Location = new Point(12, 83);
            LabelClassName.Name = "LabelClassName";
            LabelClassName.Size = new Size(267, 35);
            LabelClassName.TabIndex = 1;
            LabelClassName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ButtonCopyName
            // 
            ButtonCopyName.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 238);
            ButtonCopyName.Location = new Point(12, 151);
            ButtonCopyName.Name = "ButtonCopyName";
            ButtonCopyName.Size = new Size(267, 45);
            ButtonCopyName.TabIndex = 2;
            ButtonCopyName.Text = "Copy";
            ButtonCopyName.UseVisualStyleBackColor = true;
            ButtonCopyName.Click += ButtonCopyName_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(291, 208);
            Controls.Add(ButtonCopyName);
            Controls.Add(LabelClassName);
            Controls.Add(TextBoxClassId);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TextBoxClassId;
        private Label LabelClassName;
        private Button ButtonCopyName;
    }
}
