namespace TournamentHostProject
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
            this.startBtn = new System.Windows.Forms.Button();
            this.gameGoalLbl = new System.Windows.Forms.Label();
            this.currentGameLbl = new System.Windows.Forms.Label();
            this.mainPnl = new System.Windows.Forms.Panel();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.pauseBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(496, 134);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(91, 23);
            this.startBtn.TabIndex = 0;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // gameGoalLbl
            // 
            this.gameGoalLbl.AutoSize = true;
            this.gameGoalLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gameGoalLbl.Location = new System.Drawing.Point(12, 34);
            this.gameGoalLbl.Name = "gameGoalLbl";
            this.gameGoalLbl.Size = new System.Drawing.Size(265, 39);
            this.gameGoalLbl.TabIndex = 1;
            this.gameGoalLbl.Text = "Game Goal : {0}";
            // 
            // currentGameLbl
            // 
            this.currentGameLbl.AutoSize = true;
            this.currentGameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentGameLbl.Location = new System.Drawing.Point(12, 88);
            this.currentGameLbl.Name = "currentGameLbl";
            this.currentGameLbl.Size = new System.Drawing.Size(308, 39);
            this.currentGameLbl.TabIndex = 2;
            this.currentGameLbl.Text = "Current Game : {0}";
            // 
            // mainPnl
            // 
            this.mainPnl.Location = new System.Drawing.Point(19, 130);
            this.mainPnl.Name = "mainPnl";
            this.mainPnl.Size = new System.Drawing.Size(471, 469);
            this.mainPnl.TabIndex = 3;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Enabled = false;
            this.cancelBtn.Location = new System.Drawing.Point(496, 192);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(91, 23);
            this.cancelBtn.TabIndex = 2;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // pauseBtn
            // 
            this.pauseBtn.Enabled = false;
            this.pauseBtn.Location = new System.Drawing.Point(496, 163);
            this.pauseBtn.Name = "pauseBtn";
            this.pauseBtn.Size = new System.Drawing.Size(91, 23);
            this.pauseBtn.TabIndex = 1;
            this.pauseBtn.Text = "Pause";
            this.pauseBtn.UseVisualStyleBackColor = true;
            this.pauseBtn.Click += new System.EventHandler(this.pauseBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 611);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.pauseBtn);
            this.Controls.Add(this.mainPnl);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.currentGameLbl);
            this.Controls.Add(this.gameGoalLbl);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.Label gameGoalLbl;
        private System.Windows.Forms.Label currentGameLbl;
        private System.Windows.Forms.Panel mainPnl;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button pauseBtn;
    }
}

