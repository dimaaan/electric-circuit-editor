using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;

namespace newToe {
/// <summary>
/// Форма для ввода параметров элемента цепи
/// </summary>
public class frmElemParams : System.Windows.Forms.Form {
	#region Controls
private System.Windows.Forms.PictureBox picElemPic;
	public System.Windows.Forms.Label lblHead;
	private System.Windows.Forms.Panel panParams;
	private System.Windows.Forms.Button btnOK;
	public System.Windows.Forms.CheckBox chkReverse;
	private System.ComponentModel.Container components = null;
	#endregion

	//-----------------------------------------------------------------

	public ListDictionary ElemParams;

	public bool ElemIsVertical;
	private System.Windows.Forms.Panel panHead;
	private System.Windows.Forms.PictureBox picIcon;
	private System.Windows.Forms.Label label1;

	public Bitmap pImage;

	//-----------------------------------------------------------------

	#region Windows Form Designer generated code
	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmElemParams));
		this.picElemPic = new System.Windows.Forms.PictureBox();
		this.lblHead = new System.Windows.Forms.Label();
		this.panParams = new System.Windows.Forms.Panel();
		this.btnOK = new System.Windows.Forms.Button();
		this.chkReverse = new System.Windows.Forms.CheckBox();
		this.panHead = new System.Windows.Forms.Panel();
		this.picIcon = new System.Windows.Forms.PictureBox();
		this.label1 = new System.Windows.Forms.Label();
		this.panHead.SuspendLayout();
		this.SuspendLayout();
		// 
		// picElemPic
		// 
		this.picElemPic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.picElemPic.Location = new System.Drawing.Point(16, 64);
		this.picElemPic.Name = "picElemPic";
		this.picElemPic.Size = new System.Drawing.Size(64, 50);
		this.picElemPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.picElemPic.TabIndex = 0;
		this.picElemPic.TabStop = false;
		// 
		// lblHead
		// 
		this.lblHead.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.lblHead.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
		this.lblHead.Location = new System.Drawing.Point(56, 0);
		this.lblHead.Name = "lblHead";
		this.lblHead.Size = new System.Drawing.Size(216, 16);
		this.lblHead.TabIndex = 1;
		this.lblHead.Text = "Введите параметры элемента";
		// 
		// panParams
		// 
		this.panParams.Location = new System.Drawing.Point(88, 72);
		this.panParams.Name = "panParams";
		this.panParams.Size = new System.Drawing.Size(192, 56);
		this.panParams.TabIndex = 2;
		// 
		// btnOK
		// 
		this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnOK.Location = new System.Drawing.Point(96, 144);
		this.btnOK.Name = "btnOK";
		this.btnOK.Size = new System.Drawing.Size(108, 24);
		this.btnOK.TabIndex = 3;
		this.btnOK.Text = "OK";
		this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
		// 
		// chkReverse
		// 
		this.chkReverse.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.chkReverse.Location = new System.Drawing.Point(16, 120);
		this.chkReverse.Name = "chkReverse";
		this.chkReverse.Size = new System.Drawing.Size(80, 24);
		this.chkReverse.TabIndex = 4;
		this.chkReverse.Text = "Повернуть";
		this.chkReverse.CheckedChanged += new System.EventHandler(this.chkReverse_CheckedChanged);
		// 
		// panHead
		// 
		this.panHead.BackColor = System.Drawing.Color.White;
		this.panHead.Controls.Add(this.label1);
		this.panHead.Controls.Add(this.lblHead);
		this.panHead.Controls.Add(this.picIcon);
		this.panHead.Dock = System.Windows.Forms.DockStyle.Top;
		this.panHead.Location = new System.Drawing.Point(0, 0);
		this.panHead.Name = "panHead";
		this.panHead.Size = new System.Drawing.Size(290, 56);
		this.panHead.TabIndex = 5;
		// 
		// picIcon
		// 
		this.picIcon.Image = ((System.Drawing.Image)(resources.GetObject("picIcon.Image")));
		this.picIcon.Location = new System.Drawing.Point(16, 12);
		this.picIcon.Name = "picIcon";
		this.picIcon.Size = new System.Drawing.Size(24, 24);
		this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.picIcon.TabIndex = 6;
		this.picIcon.TabStop = false;
		// 
		// label1
		// 
		this.label1.Location = new System.Drawing.Point(56, 24);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(224, 32);
		this.label1.TabIndex = 7;
		this.label1.Text = "Возможна экспотенциальная запись. Например 3.1e-4";
		// 
		// frmElemParams
		// 
		this.AcceptButton = this.btnOK;
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		this.AutoScroll = true;
		this.ClientSize = new System.Drawing.Size(290, 176);
		this.Controls.Add(this.panHead);
		this.Controls.Add(this.chkReverse);
		this.Controls.Add(this.btnOK);
		this.Controls.Add(this.panParams);
		this.Controls.Add(this.picElemPic);
		this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		this.MaximizeBox = false;
		this.MinimizeBox = false;
		this.Name = "frmElemParams";
		this.ShowInTaskbar = false;
		this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Параметры элемента цепи";
		this.Load += new System.EventHandler(this.frmElemParams_Load);
		this.panHead.ResumeLayout(false);
		this.ResumeLayout(false);

	}
	#endregion

	//-----------------------------------------------------------------
	public frmElemParams() {
		InitializeComponent();

		ElemParams = new ListDictionary();
	}

	//-----------------------------------------------------------------
	protected override void Dispose( bool disposing ) {
		if( disposing ) 
			if(components != null) components.Dispose();
		base.Dispose( disposing );
	}

	//-----------------------------------------------------------------
	/// <summary>Добавляет необходимые контролы и показывает форму модально</summary>
	public DialogResult ShowForm() {
		if(ElemParams.Count == 0) return DialogResult.Abort;

		Label pLbl;
		TextBox pTxt;
		int CtrlY = 0, 
			CtrlHeight = 20,
			TxtBoxWidth = 60,
			TxtBoxX = panParams.Width - TxtBoxWidth,
			LblWidth = panParams.Width - TxtBoxWidth - 5,
			Count = 1;

		picElemPic.Image = new Bitmap(pImage);
		if(chkReverse.Checked) 
			picElemPic.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);

		foreach(DictionaryEntry i in ElemParams) {
			pLbl = new Label();
			pTxt = new TextBox();

			pLbl.Location	= new Point(0, CtrlY);
			pLbl.Size		= new Size(LblWidth, CtrlHeight);

			pTxt.Location	= new Point(TxtBoxX, CtrlY);
			pTxt.Size		= new Size(TxtBoxWidth, CtrlHeight);

			pLbl.Text		= i.Key.ToString();
			pTxt.Text		= i.Value.ToString();
			pLbl.Tag		= Count;
			pTxt.Tag		= Count;
			pTxt.TextChanged += new EventHandler(txtBoxes_TextChanged_Handler);

			panParams.Controls.Add(pLbl);
			panParams.Controls.Add(pTxt);

			CtrlY += pLbl.Size.Height + 5;
			Count++;
		}

		if(ElemIsVertical)
			picElemPic.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);

		return ShowDialog();
	}

	//-----------------------------------------------------------------
	private void txtBoxes_TextChanged_Handler(object sender, EventArgs e) {
		TextBox txt = sender as TextBox;

		for(int i = 0; i < txt.Text.Length; i++) {
			if(!char.IsLetter(txt.Text[i])) txt.Text.Remove(i, 1);
		}
	}

	//-----------------------------------------------------------------
	private void btnOK_Click(object sender, System.EventArgs e) {
		IEnumerator i = panParams.Controls.GetEnumerator();
		Label plbl;
		TextBox ptxt;

		while(i.MoveNext()) {
			plbl = i.Current as Label;
			i.MoveNext();
			ptxt = i.Current as TextBox;
			try{
				ptxt.Text = ptxt.Text.Replace(".", ",");
				ElemParams[plbl.Text] = ptxt.Text != "" ? Convert.ToSingle(ptxt.Text) : 0;
			}
			catch {
				MessageBox.Show("Введены неправильные данные", "", 
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
		}

		DialogResult = DialogResult.OK;
		Close();
	}

	//-----------------------------------------------------------------
	private void chkReverse_CheckedChanged(object sender, System.EventArgs e) {
		if(picElemPic == null || picElemPic.Image == null) return;
		picElemPic.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
		picElemPic.Invalidate();
	}

	//-----------------------------------------------------------------
	private void frmElemParams_Load(object sender, System.EventArgs e) {
	
	}
}
}