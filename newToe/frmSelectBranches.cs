using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace newToe {
/// <summary>Диалог, который отвечет за выбор пользователем нужных ветвей</summary>
public class frmSelectBranches : System.Windows.Forms.Form {

	private ArrayList Branches;
	private ArrayList Elements;

	public ArrayList SelectedBranches;

	#region Controls
	private System.Windows.Forms.ListBox lstSelectedBr;
	private System.Windows.Forms.ListBox lstAvalBr;
	private System.Windows.Forms.Label lblSelected;
	private System.Windows.Forms.Label label1;
	private System.Windows.Forms.Button btnDeselect;
	private System.Windows.Forms.Button btnSelect;
	private System.Windows.Forms.Button btnOK;
	private System.Windows.Forms.Button btnCancel;
	private System.Windows.Forms.Button btnSelectAll;
	private System.Windows.Forms.Button btnDeselectAll;
	private System.Windows.Forms.Label lblComment;
	private System.Windows.Forms.Panel panHead;
	private System.Windows.Forms.Label lblHeadIcon;
	private System.Windows.Forms.GroupBox grpSelectBranches;
	private System.Windows.Forms.GroupBox grpSelectSrc;
	private System.Windows.Forms.ComboBox cmbSrcs;
	private System.ComponentModel.Container components = null;
	#endregion

	#region Windows Form Designer generated code
	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent() {
		System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmSelectBranches));
		this.lstSelectedBr = new System.Windows.Forms.ListBox();
		this.lstAvalBr = new System.Windows.Forms.ListBox();
		this.lblSelected = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.btnDeselect = new System.Windows.Forms.Button();
		this.btnSelect = new System.Windows.Forms.Button();
		this.btnOK = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnSelectAll = new System.Windows.Forms.Button();
		this.btnDeselectAll = new System.Windows.Forms.Button();
		this.lblComment = new System.Windows.Forms.Label();
		this.panHead = new System.Windows.Forms.Panel();
		this.lblHeadIcon = new System.Windows.Forms.Label();
		this.grpSelectBranches = new System.Windows.Forms.GroupBox();
		this.grpSelectSrc = new System.Windows.Forms.GroupBox();
		this.cmbSrcs = new System.Windows.Forms.ComboBox();
		this.panHead.SuspendLayout();
		this.grpSelectBranches.SuspendLayout();
		this.grpSelectSrc.SuspendLayout();
		this.SuspendLayout();
		// 
		// lstSelectedBr
		// 
		this.lstSelectedBr.Location = new System.Drawing.Point(8, 32);
		this.lstSelectedBr.Name = "lstSelectedBr";
		this.lstSelectedBr.Size = new System.Drawing.Size(80, 173);
		this.lstSelectedBr.Sorted = true;
		this.lstSelectedBr.TabIndex = 0;
		// 
		// lstAvalBr
		// 
		this.lstAvalBr.Location = new System.Drawing.Point(152, 32);
		this.lstAvalBr.Name = "lstAvalBr";
		this.lstAvalBr.Size = new System.Drawing.Size(80, 173);
		this.lstAvalBr.Sorted = true;
		this.lstAvalBr.TabIndex = 1;
		// 
		// lblSelected
		// 
		this.lblSelected.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblSelected.Location = new System.Drawing.Point(8, 16);
		this.lblSelected.Name = "lblSelected";
		this.lblSelected.Size = new System.Drawing.Size(80, 16);
		this.lblSelected.TabIndex = 2;
		this.lblSelected.Text = "Выбранные:";
		// 
		// label1
		// 
		this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label1.Location = new System.Drawing.Point(152, 16);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(80, 16);
		this.label1.TabIndex = 3;
		this.label1.Text = "Доступные:";
		// 
		// btnDeselect
		// 
		this.btnDeselect.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnDeselect.Location = new System.Drawing.Point(104, 96);
		this.btnDeselect.Name = "btnDeselect";
		this.btnDeselect.Size = new System.Drawing.Size(32, 24);
		this.btnDeselect.TabIndex = 4;
		this.btnDeselect.Text = ">";
		this.btnDeselect.Click += new System.EventHandler(this.btnDeselect_Click);
		// 
		// btnSelect
		// 
		this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnSelect.Location = new System.Drawing.Point(104, 56);
		this.btnSelect.Name = "btnSelect";
		this.btnSelect.Size = new System.Drawing.Size(32, 24);
		this.btnSelect.TabIndex = 5;
		this.btnSelect.Text = "<";
		this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
		// 
		// btnOK
		// 
		this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnOK.Location = new System.Drawing.Point(17, 392);
		this.btnOK.Name = "btnOK";
		this.btnOK.Size = new System.Drawing.Size(104, 24);
		this.btnOK.TabIndex = 6;
		this.btnOK.Text = "OK";
		this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
		// 
		// btnCancel
		// 
		this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnCancel.Location = new System.Drawing.Point(137, 392);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(104, 24);
		this.btnCancel.TabIndex = 7;
		this.btnCancel.Text = "Отмена";
		// 
		// btnSelectAll
		// 
		this.btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnSelectAll.Location = new System.Drawing.Point(104, 128);
		this.btnSelectAll.Name = "btnSelectAll";
		this.btnSelectAll.Size = new System.Drawing.Size(32, 24);
		this.btnSelectAll.TabIndex = 9;
		this.btnSelectAll.Text = "<<<";
		this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
		// 
		// btnDeselectAll
		// 
		this.btnDeselectAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnDeselectAll.Location = new System.Drawing.Point(104, 168);
		this.btnDeselectAll.Name = "btnDeselectAll";
		this.btnDeselectAll.Size = new System.Drawing.Size(32, 24);
		this.btnDeselectAll.TabIndex = 8;
		this.btnDeselectAll.Text = ">>>";
		this.btnDeselectAll.Click += new System.EventHandler(this.btnDeselectAll_Click);
		// 
		// lblComment
		// 
		this.lblComment.BackColor = System.Drawing.SystemColors.Window;
		this.lblComment.Location = new System.Drawing.Point(64, 8);
		this.lblComment.Name = "lblComment";
		this.lblComment.Size = new System.Drawing.Size(184, 64);
		this.lblComment.TabIndex = 11;
		this.lblComment.Text = "Для ручного упрощения цепи необходимо, во-первых, выбрать номера упрощаемых ветве" +
			"й, во-вторых, выбрать источник, от которого происходит упрощение";
		// 
		// panHead
		// 
		this.panHead.BackColor = System.Drawing.SystemColors.Window;
		this.panHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panHead.Controls.Add(this.lblHeadIcon);
		this.panHead.Controls.Add(this.lblComment);
		this.panHead.Location = new System.Drawing.Point(0, 0);
		this.panHead.Name = "panHead";
		this.panHead.Size = new System.Drawing.Size(264, 88);
		this.panHead.TabIndex = 12;
		// 
		// lblHeadIcon
		// 
		this.lblHeadIcon.Image = ((System.Drawing.Image)(resources.GetObject("lblHeadIcon.Image")));
		this.lblHeadIcon.Location = new System.Drawing.Point(8, 8);
		this.lblHeadIcon.Name = "lblHeadIcon";
		this.lblHeadIcon.Size = new System.Drawing.Size(48, 64);
		this.lblHeadIcon.TabIndex = 13;
		// 
		// grpSelectBranches
		// 
		this.grpSelectBranches.Controls.Add(this.btnDeselect);
		this.grpSelectBranches.Controls.Add(this.label1);
		this.grpSelectBranches.Controls.Add(this.btnSelect);
		this.grpSelectBranches.Controls.Add(this.btnSelectAll);
		this.grpSelectBranches.Controls.Add(this.btnDeselectAll);
		this.grpSelectBranches.Controls.Add(this.lblSelected);
		this.grpSelectBranches.Controls.Add(this.lstAvalBr);
		this.grpSelectBranches.Controls.Add(this.lstSelectedBr);
		this.grpSelectBranches.ForeColor = System.Drawing.SystemColors.Highlight;
		this.grpSelectBranches.Location = new System.Drawing.Point(5, 96);
		this.grpSelectBranches.Name = "grpSelectBranches";
		this.grpSelectBranches.Size = new System.Drawing.Size(248, 216);
		this.grpSelectBranches.TabIndex = 13;
		this.grpSelectBranches.TabStop = false;
		this.grpSelectBranches.Text = "Выберите ветки";
		// 
		// grpSelectSrc
		// 
		this.grpSelectSrc.Controls.Add(this.cmbSrcs);
		this.grpSelectSrc.ForeColor = System.Drawing.SystemColors.Highlight;
		this.grpSelectSrc.Location = new System.Drawing.Point(5, 320);
		this.grpSelectSrc.Name = "grpSelectSrc";
		this.grpSelectSrc.Size = new System.Drawing.Size(248, 64);
		this.grpSelectSrc.TabIndex = 14;
		this.grpSelectSrc.TabStop = false;
		this.grpSelectSrc.Text = "Выберите источник";
		// 
		// cmbSrcs
		// 
		this.cmbSrcs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cmbSrcs.Location = new System.Drawing.Point(40, 26);
		this.cmbSrcs.Name = "cmbSrcs";
		this.cmbSrcs.Size = new System.Drawing.Size(168, 21);
		this.cmbSrcs.TabIndex = 0;
		// 
		// frmSelectBranches
		// 
		this.AcceptButton = this.btnOK;
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		this.CancelButton = this.btnCancel;
		this.ClientSize = new System.Drawing.Size(258, 432);
		this.Controls.Add(this.grpSelectSrc);
		this.Controls.Add(this.grpSelectBranches);
		this.Controls.Add(this.btnCancel);
		this.Controls.Add(this.btnOK);
		this.Controls.Add(this.panHead);
		this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		this.MaximizeBox = false;
		this.MinimizeBox = false;
		this.Name = "frmSelectBranches";
		this.ShowInTaskbar = false;
		this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Ручное упрощение цепи";
		this.Load += new System.EventHandler(this.frmSelectBranches_Load);
		this.panHead.ResumeLayout(false);
		this.grpSelectBranches.ResumeLayout(false);
		this.grpSelectSrc.ResumeLayout(false);
		this.ResumeLayout(false);

	}

	protected override void Dispose( bool disposing ) {
		if( disposing )
			if(components != null)components.Dispose();
		base.Dispose( disposing );
	}
	#endregion

	public frmSelectBranches(ArrayList branches, ArrayList elements) {
		InitializeComponent();
		Branches = branches;
		Elements = elements;
	}

	private void frmSelectBranches_Load(object sender, System.EventArgs e) {
		panHead.Width = ClientSize.Width;

		for(int i = 0, j = 1; i < Branches.Count; i++, j++)
			lstAvalBr.Items.Add((Branches[i] as Branch).Number);

		// составляем список источников
		foreach(Element i in Elements) {
			if(i is CurrentSrc)
				cmbSrcs.Items.Add("E" + i.Number.ToString());
			else if(i is VoltageSrc)
				cmbSrcs.Items.Add("J" + i.Number.ToString());
		}
	}

	private void btnSelect_Click(object sender, System.EventArgs e) {
		if(lstAvalBr.SelectedItem == null) return;
		lstSelectedBr.Items.Add(lstAvalBr.SelectedItem);
		lstAvalBr.Items.Remove(lstAvalBr.SelectedItem);
	}

	private void btnDeselect_Click(object sender, System.EventArgs e) {
		if(lstSelectedBr.SelectedItem == null) return;
		lstAvalBr.Items.Add(lstSelectedBr.SelectedItem);
		lstSelectedBr.Items.Remove(lstSelectedBr.SelectedItem);
	}

	private void btnSelectAll_Click(object sender, System.EventArgs e) {
		lstSelectedBr.Items.AddRange(lstAvalBr.Items);
		lstAvalBr.Items.Clear();
	}

	private void btnDeselectAll_Click(object sender, System.EventArgs e) {
		lstAvalBr.Items.AddRange(lstSelectedBr.Items);
		lstSelectedBr.Items.Clear();
	}

	private void btnOK_Click(object sender, System.EventArgs e) {
		if(lstSelectedBr.Items.Count == 0) {
			MessageBox.Show("Выберите ветки!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return;
		}
		if(cmbSrcs.SelectedItem == null) {
			MessageBox.Show("Выберите источник!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return;
		}

		SelectedBranches = new ArrayList();
		for(int i = 0; i < lstSelectedBr.Items.Count; i++) {
			SelectedBranches.Add(
				Branch.GetBranchFromNo(Branches, (int)lstSelectedBr.Items[i]));
		}
		DialogResult = DialogResult.OK;
		Close();
	}
}
}
