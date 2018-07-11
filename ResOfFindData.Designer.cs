namespace FileManager
{
    partial class ResOfFindData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResOfFindData));
            this.listViewOfFiles = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // listViewOfFiles
            // 
            this.listViewOfFiles.BackColor = System.Drawing.SystemColors.ControlLight;
            this.listViewOfFiles.Font = new System.Drawing.Font("Segoe Script", 7.25F);
            this.listViewOfFiles.Location = new System.Drawing.Point(12, 12);
            this.listViewOfFiles.Name = "listViewOfFiles";
            this.listViewOfFiles.Size = new System.Drawing.Size(518, 363);
            this.listViewOfFiles.SmallImageList = this.imageList1;
            this.listViewOfFiles.TabIndex = 0;
            this.listViewOfFiles.UseCompatibleStateImageBehavior = false;
            this.listViewOfFiles.View = System.Windows.Forms.View.List;
            this.listViewOfFiles.SelectedIndexChanged += new System.EventHandler(this.listViewOfFiles_SelectedIndexChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder.png");
            this.imageList1.Images.SetKeyName(1, "1491614504_notepad.png");
            // 
            // ResOfFindData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(542, 387);
            this.Controls.Add(this.listViewOfFiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ResOfFindData";
            this.Text = "ResOfFindData";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewOfFiles;
        private System.Windows.Forms.ImageList imageList1;
    }
}