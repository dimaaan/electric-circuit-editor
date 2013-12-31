using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace newToe {

public class frmIncidentMatrix : System.Windows.Forms.Form {
	private System.Windows.Forms.ListView lstMatrix;
	private System.ComponentModel.Container components = null;

	private BaseKnot[,] Schema;
	private System.Windows.Forms.Label lblBranch;
	private System.Windows.Forms.Label lblKnot;
	private ArrayList Branches;

	public frmIncidentMatrix(BaseKnot[,] schema, ArrayList branches) {
		InitializeComponent();

		Schema = schema;
		Branches = branches;
	}

	protected override void Dispose( bool disposing ) {
		if( disposing ) if(components != null) components.Dispose();
		base.Dispose( disposing );
	}

	#region Windows Form Designer generated code
	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		this.lstMatrix = new System.Windows.Forms.ListView();
		this.lblBranch = new System.Windows.Forms.Label();
		this.lblKnot = new System.Windows.Forms.Label();
		this.SuspendLayout();
		// 
		// lstMatrix
		// 
		this.lstMatrix.GridLines = true;
		this.lstMatrix.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
		this.lstMatrix.Location = new System.Drawing.Point(16, 16);
		this.lstMatrix.Name = "lstMatrix";
		this.lstMatrix.Size = new System.Drawing.Size(336, 296);
		this.lstMatrix.TabIndex = 0;
		this.lstMatrix.View = System.Windows.Forms.View.Details;
		// 
		// lblBranch
		// 
		this.lblBranch.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblBranch.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
		this.lblBranch.Location = new System.Drawing.Point(0, 0);
		this.lblBranch.Name = "lblBranch";
		this.lblBranch.Size = new System.Drawing.Size(384, 16);
		this.lblBranch.TabIndex = 1;
		this.lblBranch.Text = "В  е  т  в  и";
		this.lblBranch.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		// 
		// lblKnot
		// 
		this.lblKnot.Dock = System.Windows.Forms.DockStyle.Left;
		this.lblKnot.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
		this.lblKnot.Location = new System.Drawing.Point(0, 16);
		this.lblKnot.Name = "lblKnot";
		this.lblKnot.Size = new System.Drawing.Size(16, 342);
		this.lblKnot.TabIndex = 2;
		this.lblKnot.Text = "У   з   л   ы";
		this.lblKnot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		// 
		// frmIncidentMatrix
		// 
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		this.ClientSize = new System.Drawing.Size(384, 358);
		this.Controls.Add(this.lblKnot);
		this.Controls.Add(this.lblBranch);
		this.Controls.Add(this.lstMatrix);
		this.Name = "frmIncidentMatrix";
		this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Матрица инцеденции";
		this.Load += new System.EventHandler(this.frmIncidentMatrix_Load);
		this.ResumeLayout(false);

	}
	#endregion

	private void frmIncidentMatrix_Load(object sender, System.EventArgs e) {
		if(Branches == null || Schema == null) return;

		int i, numKnots = BaseKnot.CountKnots(Schema);
		Branch CurrBranch;

		// создаем таблицу нужного размера
		
		lstMatrix.Width = ClientSize.Width - lblKnot.Width;
		lstMatrix.Height = ClientSize.Height - lblBranch.Height;

		for(i = 0; i < Branches.Count + 1; i++)
			lstMatrix.Columns.Add("", 40,HorizontalAlignment.Center);
		for(i = 1; i < Branches.Count + 1; i++)
			lstMatrix.Columns[i].Text = i.ToString();

		for(i = 1; i <= numKnots; i++) {
			lstMatrix.Items.Add(i.ToString());
			for(int i2 = 0; i2 < Branches.Count + 1; i2++)
				lstMatrix.Items[i - 1].SubItems.Add("0");
		}

		for(int x = 0; x < Schema.GetLength(0); x++) {
			for(int y = 0; y < Schema.GetLength(1); y++) {
				if(!Schema[x,y].IsKnot && !Schema[x,y].IsRemovableKnot) continue;
				for(int i3 = 0; i3 < Branches.Count; i3++) {
					CurrBranch = Branches[i3] as Branch;
					if(CurrBranch.pKnots[0] == Schema[x,y]) {
						lstMatrix.Items[Schema[x,y].KnotNo - 1].SubItems[CurrBranch.Number].Text = "-1";
					}
					else if(CurrBranch.pKnots[CurrBranch.pKnots.Count - 1] == Schema[x,y])
						lstMatrix.Items[Schema[x,y].KnotNo - 1].SubItems[CurrBranch.Number].Text = "+1";
				}
			}
		}
	}
}
}
