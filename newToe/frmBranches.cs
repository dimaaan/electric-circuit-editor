using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace newToe {
	/// <summary>Диалог для вывода списка веток и изменения их направления</summary>
	public class frmBranches : System.Windows.Forms.Form {

		ArrayList Branches;

		//------------------------------------------------------------------------------
		#region Controls
		private System.Windows.Forms.ListBox lstBranches;
		private System.Windows.Forms.Label lblBranchesHead;
		private System.Windows.Forms.GroupBox grpBranchParams;
		private System.Windows.Forms.Label lblBranchNoHead;
		private System.Windows.Forms.Label lblBranchNo;
		private System.Windows.Forms.Label lblDirection;
		private System.Windows.Forms.RadioButton radForward;
		private System.Windows.Forms.RadioButton radBackward;
		private System.Windows.Forms.Label lnlLine;
		private System.Windows.Forms.Panel panHead;
		private System.Windows.Forms.PictureBox picIcon;
		private System.Windows.Forms.Label lblHead;
		private System.Windows.Forms.Label lblComment;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label lblBranchLen;
		private System.Windows.Forms.Label lblBranchLenHead;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		//------------------------------------------------------------------------------
		public frmBranches(ArrayList branches) {
			InitializeComponent();

			Branches = branches;
		}

		//------------------------------------------------------------------------------
		protected override void Dispose( bool disposing ) {
			if( disposing )
				if(components != null) components.Dispose();
			base.Dispose( disposing );
		}

		//------------------------------------------------------------------------------
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmBranches));
			this.lstBranches = new System.Windows.Forms.ListBox();
			this.lblBranchesHead = new System.Windows.Forms.Label();
			this.grpBranchParams = new System.Windows.Forms.GroupBox();
			this.lblBranchNoHead = new System.Windows.Forms.Label();
			this.lblBranchNo = new System.Windows.Forms.Label();
			this.lblDirection = new System.Windows.Forms.Label();
			this.radForward = new System.Windows.Forms.RadioButton();
			this.radBackward = new System.Windows.Forms.RadioButton();
			this.lnlLine = new System.Windows.Forms.Label();
			this.panHead = new System.Windows.Forms.Panel();
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.lblHead = new System.Windows.Forms.Label();
			this.lblComment = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblBranchLen = new System.Windows.Forms.Label();
			this.lblBranchLenHead = new System.Windows.Forms.Label();
			this.grpBranchParams.SuspendLayout();
			this.panHead.SuspendLayout();
			this.SuspendLayout();
			// 
			// lstBranches
			// 
			this.lstBranches.Location = new System.Drawing.Point(8, 96);
			this.lstBranches.Name = "lstBranches";
			this.lstBranches.Size = new System.Drawing.Size(120, 251);
			this.lstBranches.TabIndex = 0;
			this.lstBranches.SelectedIndexChanged += new System.EventHandler(this.lstBranches_SelectedIndexChanged);
			// 
			// lblBranchesHead
			// 
			this.lblBranchesHead.Location = new System.Drawing.Point(8, 80);
			this.lblBranchesHead.Name = "lblBranchesHead";
			this.lblBranchesHead.Size = new System.Drawing.Size(120, 16);
			this.lblBranchesHead.TabIndex = 1;
			this.lblBranchesHead.Text = "Ветви схемы:";
			this.lblBranchesHead.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// grpBranchParams
			// 
			this.grpBranchParams.Controls.Add(this.lblBranchLen);
			this.grpBranchParams.Controls.Add(this.lblBranchLenHead);
			this.grpBranchParams.Controls.Add(this.lnlLine);
			this.grpBranchParams.Controls.Add(this.radBackward);
			this.grpBranchParams.Controls.Add(this.radForward);
			this.grpBranchParams.Controls.Add(this.lblDirection);
			this.grpBranchParams.Controls.Add(this.lblBranchNo);
			this.grpBranchParams.Controls.Add(this.lblBranchNoHead);
			this.grpBranchParams.Location = new System.Drawing.Point(152, 80);
			this.grpBranchParams.Name = "grpBranchParams";
			this.grpBranchParams.Size = new System.Drawing.Size(200, 232);
			this.grpBranchParams.TabIndex = 2;
			this.grpBranchParams.TabStop = false;
			this.grpBranchParams.Text = "Параметры ветви";
			// 
			// lblBranchNoHead
			// 
			this.lblBranchNoHead.Location = new System.Drawing.Point(8, 32);
			this.lblBranchNoHead.Name = "lblBranchNoHead";
			this.lblBranchNoHead.Size = new System.Drawing.Size(100, 16);
			this.lblBranchNoHead.TabIndex = 0;
			this.lblBranchNoHead.Text = "Номер ветви:";
			this.lblBranchNoHead.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblBranchNo
			// 
			this.lblBranchNo.Location = new System.Drawing.Point(128, 32);
			this.lblBranchNo.Name = "lblBranchNo";
			this.lblBranchNo.Size = new System.Drawing.Size(64, 16);
			this.lblBranchNo.TabIndex = 1;
			this.lblBranchNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblDirection
			// 
			this.lblDirection.Location = new System.Drawing.Point(16, 144);
			this.lblDirection.Name = "lblDirection";
			this.lblDirection.Size = new System.Drawing.Size(120, 23);
			this.lblDirection.TabIndex = 2;
			this.lblDirection.Text = "Направление обхода:";
			// 
			// radForward
			// 
			this.radForward.Location = new System.Drawing.Point(40, 176);
			this.radForward.Name = "radForward";
			this.radForward.Size = new System.Drawing.Size(152, 16);
			this.radForward.TabIndex = 3;
			this.radForward.Text = "Вперед";
			this.radForward.CheckedChanged += new System.EventHandler(this.radDirection_CheckedChanged);
			// 
			// radBackward
			// 
			this.radBackward.Location = new System.Drawing.Point(40, 200);
			this.radBackward.Name = "radBackward";
			this.radBackward.Size = new System.Drawing.Size(152, 16);
			this.radBackward.TabIndex = 4;
			this.radBackward.Text = "Назад";
			this.radBackward.CheckedChanged += new System.EventHandler(this.radDirection_CheckedChanged);
			// 
			// lnlLine
			// 
			this.lnlLine.Location = new System.Drawing.Point(8, 112);
			this.lnlLine.Name = "lnlLine";
			this.lnlLine.Size = new System.Drawing.Size(184, 16);
			this.lnlLine.TabIndex = 5;
			this.lnlLine.Text = "_______________________________________";
			this.lnlLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panHead
			// 
			this.panHead.BackColor = System.Drawing.SystemColors.Window;
			this.panHead.Controls.Add(this.lblComment);
			this.panHead.Controls.Add(this.lblHead);
			this.panHead.Controls.Add(this.picIcon);
			this.panHead.Dock = System.Windows.Forms.DockStyle.Top;
			this.panHead.Location = new System.Drawing.Point(0, 0);
			this.panHead.Name = "panHead";
			this.panHead.Size = new System.Drawing.Size(360, 72);
			this.panHead.TabIndex = 3;
			// 
			// picIcon
			// 
			this.picIcon.Image = ((System.Drawing.Image)(resources.GetObject("picIcon.Image")));
			this.picIcon.Location = new System.Drawing.Point(16, 4);
			this.picIcon.Name = "picIcon";
			this.picIcon.Size = new System.Drawing.Size(72, 64);
			this.picIcon.TabIndex = 0;
			this.picIcon.TabStop = false;
			// 
			// lblHead
			// 
			this.lblHead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.lblHead.Location = new System.Drawing.Point(96, 8);
			this.lblHead.Name = "lblHead";
			this.lblHead.Size = new System.Drawing.Size(264, 24);
			this.lblHead.TabIndex = 1;
			this.lblHead.Text = "Выберите необходимое направление обхода";
			// 
			// lblComment
			// 
			this.lblComment.Location = new System.Drawing.Point(96, 32);
			this.lblComment.Name = "lblComment";
			this.lblComment.Size = new System.Drawing.Size(264, 32);
			this.lblComment.TabIndex = 2;
			this.lblComment.Text = "Направление обхода влияет на процесс параллельного упрощения";
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(240, 328);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(112, 24);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// lblBranchLen
			// 
			this.lblBranchLen.Location = new System.Drawing.Point(128, 56);
			this.lblBranchLen.Name = "lblBranchLen";
			this.lblBranchLen.Size = new System.Drawing.Size(64, 16);
			this.lblBranchLen.TabIndex = 7;
			this.lblBranchLen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblBranchLenHead
			// 
			this.lblBranchLenHead.Location = new System.Drawing.Point(8, 56);
			this.lblBranchLenHead.Name = "lblBranchLenHead";
			this.lblBranchLenHead.Size = new System.Drawing.Size(100, 16);
			this.lblBranchLenHead.TabIndex = 6;
			this.lblBranchLenHead.Text = "Длинна:";
			this.lblBranchLenHead.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// frmBranches
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(360, 358);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.panHead);
			this.Controls.Add(this.grpBranchParams);
			this.Controls.Add(this.lblBranchesHead);
			this.Controls.Add(this.lstBranches);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "frmBranches";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Обход ветвей";
			this.Load += new System.EventHandler(this.frmBranches_Load);
			this.grpBranchParams.ResumeLayout(false);
			this.panHead.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		//------------------------------------------------------------------------------
		private void frmBranches_Load(object sender, System.EventArgs e) {
			foreach(Branch i in Branches) {
				lstBranches.BeginUpdate();
				lstBranches.Items.Add(i);
				lstBranches.EndUpdate();
			}
		}

		//------------------------------------------------------------------------------
		private void lstBranches_SelectedIndexChanged(object sender, System.EventArgs e) {
			if(lstBranches.SelectedItem == null) return;

			Branch br = lstBranches.SelectedItem as Branch;
			lblBranchNo.Text = br.Number.ToString();
			lblBranchLen.Text = br.Length.ToString() + " эл.";
			if(br.Direction)
				radForward.Checked = true;
			else
				radBackward.Checked = true;
		}

		//------------------------------------------------------------------------------
		private void btnOK_Click(object sender, System.EventArgs e) {
			DialogResult = DialogResult.OK;
			Close();
		}

		//------------------------------------------------------------------------------
		private void radDirection_CheckedChanged(object sender, System.EventArgs e) {
			Branch br = lstBranches.SelectedItem as Branch;

			if(radForward.Checked)
				br.Direction = true;
			else
				br.Direction = false;
		}
	}
}