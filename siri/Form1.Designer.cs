namespace siri;

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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        label1 = new System.Windows.Forms.Label();
        onButton = new System.Windows.Forms.Button();
        offButton = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // label1
        // 
        label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(180, 426);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(378, 20);
        label1.TabIndex = 3;
        label1.Text = "f10 to start the program if you don't have that second monitor type shit";
        label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        label1.Click += label1_Click;
        // 
        // onButton
        // 
        onButton.AutoSize = true;
        onButton.Location = new System.Drawing.Point(12, 150);
        onButton.Name = "onButton";
        onButton.Size = new System.Drawing.Size(380, 150);
        onButton.TabIndex = 5;
        onButton.Text = "turn this shit on";
        onButton.UseVisualStyleBackColor = true;
        onButton.Click += onButton_Click;
        // 
        // offButton
        // 
        offButton.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
        offButton.AutoSize = true;
        offButton.Location = new System.Drawing.Point(408, 150);
        offButton.Name = "offButton";
        offButton.Size = new System.Drawing.Size(380, 150);
        offButton.TabIndex = 6;
        offButton.Text = "turn this shit off";
        offButton.UseVisualStyleBackColor = true;
        offButton.Click += offButton_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(offButton);
        Controls.Add(onButton);
        Controls.Add(label1);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        MaximizeBox = false;
        ShowIcon = true;
        Text = "siri.exe controls";
        Icon = new System.Drawing.Icon(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("siri.Resources.app.ico"));
        Load += Form1_Load;
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label label1;

    private System.Windows.Forms.Button offButton;

    private System.Windows.Forms.Button onButton;
    
    #endregion
}