using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using MsdnMag;

namespace newToe {
public class frmMain : System.Windows.Forms.Form {

	//========================================================================
	// -------------------------- � � � � � � � � �

	/// <summary>
	/// ���������� ������� ����� �� ��������� � �����������. 
	/// ����� ���-�� ������� ����� �� ����� = KNOTS_X_NUM * KNOTS_Y_NUM
	/// </summary>
	public const int KNOTS_X_NUM = 20, KNOTS_Y_NUM = 20;

	/// <summary>
	/// ����� ����� �� �����
	/// </summary>
	public const int START_SHIFT = 20;

	/// <summary>���������� ����� �������� ������</summary>
	public const int KNOTS_DIST = 50;

	public enum Side {S_UPPER = 1, S_LOWER, S_LEFT, S_RIGHT}

	//========================================================================
	// -------------------------- � � � �

	/// <summary>
	/// ��������������� ������� (����� ������ �� ��������������� = 0)
	/// </summary>
	private Element DragElem;

	private BaseKnot DragKnot;

	/// <summary>
	/// �������, ��� ����������� ���� ������ �������. ���� ���� ������� == null
	/// </summary>
	private Element MenuElem;

	private BaseKnot MenuKnot;
	
	/// <summary>������ � ������� �� �����</summary>
	BaseKnot[,] Knots;

	/// <summary>�������� ����</summary>
	ArrayList Elements = new ArrayList();

	/// <summary>
	/// ������� ��������. ��������� ������: 0 - �������������� �����.
	/// 1 - ������������
	/// </summary>
	bool[,,] BuzyPos = new bool[KNOTS_X_NUM + 1, KNOTS_Y_NUM + 1,2];

	/// <summary>������ ����� �����</summary>
	ArrayList Branches;

	/// <summary>
	/// ���� ���� ��������� �����. ���� �� ��� �� ��������� == null
	/// </summary>
	string SavePath;

	const string WndCap = " - �������� ����";

	// ������ ��� �������
	string _SchemaName;
	bool _IsModifed = false;
	bool _BaseKnotVisible = true;
	bool _SchemaClosed = false;
	bool _DrawBranchNumbers = true;
	
	//========================================================================
	// -------------------------- � � � � � � � �

	// ----------------------------------------------------------------------
	/// <summary>������������ �� �� ����� ������� ���� (����������)</summary>
	bool BaseKnotsVisible {
		get {return _BaseKnotVisible;}
		set {
			_BaseKnotVisible = value;
			BaseKnot.DrawBaseKnots = value;
			picDrawBox.Invalidate();
		}
	}

	// ----------------------------------------------------------------------
	private enum ProgState {PS_NORMAL,PS_DRAG}
	private ProgState _State = ProgState.PS_NORMAL;
	private ProgState State {
		get{return _State;}
		set{
			_State = value;
			if(_State == ProgState.PS_DRAG) {
				tlbMain.Enabled = false;
				picDrawBox.Cursor = Cursors.Cross;
			}
			else {
				DragElem = null;
				DragKnot = null;
				picDrawBox.Invalidate();
				tlbMain.Enabled = true;
				picDrawBox.Cursor = Cursors.Default;
			}
		}
	}

	// ----------------------------------------------------------------------
	/// <summary>�������� ����� �����. </summary>
	private string SchemaName {
		get{return _SchemaName;}
		set {
			_SchemaName = value;
			if(IsModifed)
				this.Text = SchemaName + "*" + WndCap;
			else
				this.Text = SchemaName + WndCap;
		}
	}

	// ----------------------------------------------------------------------
	/// <summary>���� �� ����� �������� ����� ����������</summary>
	private bool IsModifed {
		get{return _IsModifed;}
		set {
			if(value == _IsModifed) return;
			if(value)
				this.Text = SchemaName + "*" + WndCap;
			else
				this.Text = SchemaName + WndCap;
			_IsModifed = value;
		}
	}

	// ----------------------------------------------------------------------
	/// <summary>�������� �� ����� ��� ���</summary>
	private bool SchemaClosed {
		get{return _SchemaClosed;}
		set {
			tbbMatrix.Enabled = value;
			tbbSimplify.Enabled = value;
			_SchemaClosed = value;
		}
	}

	// ----------------------------------------------------------------------
	private bool DrawBranchNumbers {
		get{return _DrawBranchNumbers;}
		set {
			if(_DrawBranchNumbers == value) return;
			_DrawBranchNumbers = value;
			picDrawBox.Invalidate();
		}
	}

	//========================================================================
	// -------------------------- � � � � � �

	#region Controls
	private System.Windows.Forms.ToolBarButton tbbElements;
	private System.Windows.Forms.ToolBar tlbMain;
	private System.Windows.Forms.ContextMenu cmnuElements;
	private System.Windows.Forms.ContextMenu cmnuNumeration;
	private System.Windows.Forms.ContextMenu cmnuMatrix;
	private System.Windows.Forms.ToolBarButton tbbEnumeration;
	private System.Windows.Forms.ToolBarButton tbbMatrix;
	private System.Windows.Forms.ImageList imglstImages;
	private System.Windows.Forms.MenuItem mnuHeadElements;
	private System.Windows.Forms.MenuItem mnuLine1;
	private System.Windows.Forms.MenuItem mnuConstCurrent;
	private System.Windows.Forms.MenuItem mnuCCResistance;
	private System.Windows.Forms.MenuItem mnuCCCurrentSrc;
	private System.Windows.Forms.MenuItem mnuCCVoltageSrc;
	private System.Windows.Forms.MenuItem mnuVarCurrent;
	private System.Windows.Forms.MenuItem mnuVCResistance;
	private System.Windows.Forms.MenuItem mnuVCCondenser;
	private System.Windows.Forms.MenuItem mnuVCCoil;
	private System.Windows.Forms.MenuItem mnuVCVoltageSrc;
	private System.Windows.Forms.MenuItem mnuVCCurrentSrc;
	private System.Windows.Forms.MenuItem mnuManageSrc;
	private System.Windows.Forms.MenuItem mnuMSResistance;
	private System.Windows.Forms.MenuItem mnuMSCurrentSrc;
	private System.Windows.Forms.MenuItem mnuMSVoltageSrc;
	private System.Windows.Forms.MenuItem mnuMSS;
	private System.Windows.Forms.MenuItem mnuMS_ITUT;
	private System.Windows.Forms.MenuItem mnuMS_ITUN;
	private System.Windows.Forms.MenuItem mnuMS_INUN;
	private System.Windows.Forms.MenuItem mnuMS_INUT;
	private System.Windows.Forms.MenuItem mnuCommonElements;
	private System.Windows.Forms.MenuItem mnuCEConnection;
	private System.Windows.Forms.MenuItem mnuHeadNumeration;
	private System.Windows.Forms.MenuItem mnuN_Line1;
	private System.Windows.Forms.MenuItem mnuEnumKnots;
	private System.Windows.Forms.MenuItem mnuEnumBranch;
	private System.Windows.Forms.MenuItem mnuHeadMatrix;
	private System.Windows.Forms.MenuItem mnuM_Line1;
	private System.Windows.Forms.MenuItem mnuIncidencMatrix;
	private System.Windows.Forms.MenuItem mnuParamMatrix;
	private System.Windows.Forms.Panel panWorkSpace;
	private System.Windows.Forms.PictureBox picDrawBox;
	private System.Windows.Forms.ToolBarButton tbbSeparator1;
	private System.Windows.Forms.ToolBarButton tbbShowBaseKnots;
	private System.Windows.Forms.MainMenu MainMenu;
	private System.Windows.Forms.MenuItem mnuFile;
	private System.Windows.Forms.MenuItem mnuNewFile;
	private System.Windows.Forms.MenuItem mnuLine2;
	private System.Windows.Forms.MenuItem mnuSaveFile;
	private System.Windows.Forms.MenuItem mnuOpenFile;
	private System.Windows.Forms.MenuItem mnuLine3;
	private System.Windows.Forms.MenuItem mnuExit;
	private System.Windows.Forms.MenuItem mnuSaveAs;
	private System.Windows.Forms.MenuItem mnuAbout;
	private System.Windows.Forms.ContextMenu cmnuElemMenu;
	private System.Windows.Forms.MenuItem mnuDeleteElem;
	private System.Windows.Forms.ToolBarButton tbbSimplify;
	private System.Windows.Forms.ContextMenu cmnuSimplify;
	private System.Windows.Forms.MenuItem mnuAutoSimplify;
	private System.Windows.Forms.MenuItem mnuHandSimplify;
	private System.Windows.Forms.MenuItem mnuTransforms;
	private System.Windows.Forms.MenuItem mnuLine4;
	private GraphicMenu GrapMnu;
	public System.Windows.Forms.ImageList imglstMenu;
	private System.Windows.Forms.ToolBar tlbMenu;
	private System.Windows.Forms.ToolBarButton tbbNewFile;
	private System.Windows.Forms.ToolBarButton tbbOpenFile;
	private System.Windows.Forms.ToolBarButton tbbSaveFile;
	private System.Windows.Forms.ToolBarButton tbbSaveAs;
	private System.Windows.Forms.MenuItem mnuElemParams;
	private System.Windows.Forms.MenuItem mnuCERemovableKnot;
	private System.Windows.Forms.MenuItem mnuElemHead;
	private System.Windows.Forms.MenuItem mnuElemLine;
	private System.Windows.Forms.MenuItem mnuLine5;
	private System.Windows.Forms.MenuItem mnuBranches;
	private System.ComponentModel.IContainer components;
	#endregion

	#region Windows Form Designer generated code
	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmMain));
		this.tlbMain = new System.Windows.Forms.ToolBar();
		this.tbbElements = new System.Windows.Forms.ToolBarButton();
		this.cmnuElements = new System.Windows.Forms.ContextMenu();
		this.mnuHeadElements = new System.Windows.Forms.MenuItem();
		this.mnuLine1 = new System.Windows.Forms.MenuItem();
		this.mnuConstCurrent = new System.Windows.Forms.MenuItem();
		this.mnuCCResistance = new System.Windows.Forms.MenuItem();
		this.mnuCCCurrentSrc = new System.Windows.Forms.MenuItem();
		this.mnuCCVoltageSrc = new System.Windows.Forms.MenuItem();
		this.mnuVarCurrent = new System.Windows.Forms.MenuItem();
		this.mnuVCResistance = new System.Windows.Forms.MenuItem();
		this.mnuVCCondenser = new System.Windows.Forms.MenuItem();
		this.mnuVCCoil = new System.Windows.Forms.MenuItem();
		this.mnuVCVoltageSrc = new System.Windows.Forms.MenuItem();
		this.mnuVCCurrentSrc = new System.Windows.Forms.MenuItem();
		this.mnuManageSrc = new System.Windows.Forms.MenuItem();
		this.mnuMSResistance = new System.Windows.Forms.MenuItem();
		this.mnuMSCurrentSrc = new System.Windows.Forms.MenuItem();
		this.mnuMSVoltageSrc = new System.Windows.Forms.MenuItem();
		this.mnuMSS = new System.Windows.Forms.MenuItem();
		this.mnuMS_ITUT = new System.Windows.Forms.MenuItem();
		this.mnuMS_ITUN = new System.Windows.Forms.MenuItem();
		this.mnuMS_INUN = new System.Windows.Forms.MenuItem();
		this.mnuMS_INUT = new System.Windows.Forms.MenuItem();
		this.mnuCommonElements = new System.Windows.Forms.MenuItem();
		this.mnuCEConnection = new System.Windows.Forms.MenuItem();
		this.mnuCERemovableKnot = new System.Windows.Forms.MenuItem();
		this.tbbEnumeration = new System.Windows.Forms.ToolBarButton();
		this.cmnuNumeration = new System.Windows.Forms.ContextMenu();
		this.mnuHeadNumeration = new System.Windows.Forms.MenuItem();
		this.mnuN_Line1 = new System.Windows.Forms.MenuItem();
		this.mnuEnumKnots = new System.Windows.Forms.MenuItem();
		this.mnuEnumBranch = new System.Windows.Forms.MenuItem();
		this.tbbSimplify = new System.Windows.Forms.ToolBarButton();
		this.cmnuSimplify = new System.Windows.Forms.ContextMenu();
		this.mnuTransforms = new System.Windows.Forms.MenuItem();
		this.mnuLine4 = new System.Windows.Forms.MenuItem();
		this.mnuAutoSimplify = new System.Windows.Forms.MenuItem();
		this.mnuHandSimplify = new System.Windows.Forms.MenuItem();
		this.tbbMatrix = new System.Windows.Forms.ToolBarButton();
		this.cmnuMatrix = new System.Windows.Forms.ContextMenu();
		this.mnuHeadMatrix = new System.Windows.Forms.MenuItem();
		this.mnuM_Line1 = new System.Windows.Forms.MenuItem();
		this.mnuIncidencMatrix = new System.Windows.Forms.MenuItem();
		this.mnuParamMatrix = new System.Windows.Forms.MenuItem();
		this.tbbSeparator1 = new System.Windows.Forms.ToolBarButton();
		this.tbbShowBaseKnots = new System.Windows.Forms.ToolBarButton();
		this.imglstImages = new System.Windows.Forms.ImageList(this.components);
		this.panWorkSpace = new System.Windows.Forms.Panel();
		this.picDrawBox = new System.Windows.Forms.PictureBox();
		this.MainMenu = new System.Windows.Forms.MainMenu();
		this.mnuFile = new System.Windows.Forms.MenuItem();
		this.mnuNewFile = new System.Windows.Forms.MenuItem();
		this.mnuLine2 = new System.Windows.Forms.MenuItem();
		this.mnuOpenFile = new System.Windows.Forms.MenuItem();
		this.mnuSaveFile = new System.Windows.Forms.MenuItem();
		this.mnuSaveAs = new System.Windows.Forms.MenuItem();
		this.mnuLine3 = new System.Windows.Forms.MenuItem();
		this.mnuExit = new System.Windows.Forms.MenuItem();
		this.mnuAbout = new System.Windows.Forms.MenuItem();
		this.cmnuElemMenu = new System.Windows.Forms.ContextMenu();
		this.mnuElemHead = new System.Windows.Forms.MenuItem();
		this.mnuElemLine = new System.Windows.Forms.MenuItem();
		this.mnuElemParams = new System.Windows.Forms.MenuItem();
		this.mnuDeleteElem = new System.Windows.Forms.MenuItem();
		this.GrapMnu = new MsdnMag.GraphicMenu();
		this.tlbMenu = new System.Windows.Forms.ToolBar();
		this.tbbNewFile = new System.Windows.Forms.ToolBarButton();
		this.tbbOpenFile = new System.Windows.Forms.ToolBarButton();
		this.tbbSaveFile = new System.Windows.Forms.ToolBarButton();
		this.tbbSaveAs = new System.Windows.Forms.ToolBarButton();
		this.imglstMenu = new System.Windows.Forms.ImageList(this.components);
		this.mnuLine5 = new System.Windows.Forms.MenuItem();
		this.mnuBranches = new System.Windows.Forms.MenuItem();
		this.panWorkSpace.SuspendLayout();
		this.SuspendLayout();
		// 
		// tlbMain
		// 
		this.tlbMain.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
		this.tlbMain.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																				   this.tbbElements,
																				   this.tbbEnumeration,
																				   this.tbbSimplify,
																				   this.tbbMatrix,
																				   this.tbbSeparator1,
																				   this.tbbShowBaseKnots});
		this.tlbMain.ButtonSize = new System.Drawing.Size(30, 30);
		this.tlbMain.DropDownArrows = true;
		this.tlbMain.ImageList = this.imglstImages;
		this.tlbMain.Location = new System.Drawing.Point(0, 28);
		this.tlbMain.Name = "tlbMain";
		this.tlbMain.ShowToolTips = true;
		this.tlbMain.Size = new System.Drawing.Size(640, 44);
		this.tlbMain.TabIndex = 0;
		this.tlbMain.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tlbMain_ButtonClick);
		// 
		// tbbElements
		// 
		this.tbbElements.DropDownMenu = this.cmnuElements;
		this.tbbElements.ImageIndex = 0;
		this.tbbElements.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
		this.tbbElements.Tag = "1";
		// 
		// cmnuElements
		// 
		this.cmnuElements.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuHeadElements,
																					 this.mnuLine1,
																					 this.mnuConstCurrent,
																					 this.mnuVarCurrent,
																					 this.mnuManageSrc,
																					 this.mnuCommonElements});
		// 
		// mnuHeadElements
		// 
		this.mnuHeadElements.DefaultItem = true;
		this.mnuHeadElements.Enabled = false;
		this.mnuHeadElements.Index = 0;
		this.mnuHeadElements.Text = "���������� ����";
		// 
		// mnuLine1
		// 
		this.mnuLine1.Index = 1;
		this.mnuLine1.Text = "-";
		// 
		// mnuConstCurrent
		// 
		this.mnuConstCurrent.Index = 2;
		this.mnuConstCurrent.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.mnuCCResistance,
																						this.mnuCCCurrentSrc,
																						this.mnuCCVoltageSrc});
		this.mnuConstCurrent.Text = "����� � ���������� �����";
		// 
		// mnuCCResistance
		// 
		this.mnuCCResistance.Index = 0;
		this.mnuCCResistance.Text = "�������������";
		this.mnuCCResistance.Click += new System.EventHandler(this.mnuCCResistance_Click);
		// 
		// mnuCCCurrentSrc
		// 
		this.mnuCCCurrentSrc.Index = 1;
		this.mnuCCCurrentSrc.Text = "�������� ����";
		this.mnuCCCurrentSrc.Click += new System.EventHandler(this.mnuCCCurrentSrc_Click);
		// 
		// mnuCCVoltageSrc
		// 
		this.mnuCCVoltageSrc.Index = 2;
		this.mnuCCVoltageSrc.Text = "�������� ����������";
		this.mnuCCVoltageSrc.Click += new System.EventHandler(this.mnuCCVoltageSrc_Click);
		// 
		// mnuVarCurrent
		// 
		this.mnuVarCurrent.Enabled = false;
		this.mnuVarCurrent.Index = 3;
		this.mnuVarCurrent.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuVCResistance,
																					  this.mnuVCCondenser,
																					  this.mnuVCCoil,
																					  this.mnuVCVoltageSrc,
																					  this.mnuVCCurrentSrc});
		this.mnuVarCurrent.Text = "����� � ���������� �����";
		// 
		// mnuVCResistance
		// 
		this.mnuVCResistance.Index = 0;
		this.mnuVCResistance.Text = "�������������";
		// 
		// mnuVCCondenser
		// 
		this.mnuVCCondenser.Index = 1;
		this.mnuVCCondenser.Text = "�����������";
		// 
		// mnuVCCoil
		// 
		this.mnuVCCoil.Index = 2;
		this.mnuVCCoil.Text = "������� �������������";
		// 
		// mnuVCVoltageSrc
		// 
		this.mnuVCVoltageSrc.Index = 3;
		this.mnuVCVoltageSrc.Text = "�������� ����������";
		// 
		// mnuVCCurrentSrc
		// 
		this.mnuVCCurrentSrc.Index = 4;
		this.mnuVCCurrentSrc.Text = "�������� ����";
		// 
		// mnuManageSrc
		// 
		this.mnuManageSrc.Enabled = false;
		this.mnuManageSrc.Index = 4;
		this.mnuManageSrc.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuMSResistance,
																					 this.mnuMSCurrentSrc,
																					 this.mnuMSVoltageSrc,
																					 this.mnuMSS});
		this.mnuManageSrc.Text = "����� � ������������ �����������";
		// 
		// mnuMSResistance
		// 
		this.mnuMSResistance.Index = 0;
		this.mnuMSResistance.Text = "�������������";
		// 
		// mnuMSCurrentSrc
		// 
		this.mnuMSCurrentSrc.Index = 1;
		this.mnuMSCurrentSrc.Text = "�������� ����";
		// 
		// mnuMSVoltageSrc
		// 
		this.mnuMSVoltageSrc.Index = 2;
		this.mnuMSVoltageSrc.Text = "�������� ����������";
		// 
		// mnuMSS
		// 
		this.mnuMSS.Index = 3;
		this.mnuMSS.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																			   this.mnuMS_ITUT,
																			   this.mnuMS_ITUN,
																			   this.mnuMS_INUN,
																			   this.mnuMS_INUT});
		this.mnuMSS.Text = "����������� ���������";
		// 
		// mnuMS_ITUT
		// 
		this.mnuMS_ITUT.Index = 0;
		this.mnuMS_ITUT.Text = "����";
		// 
		// mnuMS_ITUN
		// 
		this.mnuMS_ITUN.Index = 1;
		this.mnuMS_ITUN.Text = "����";
		// 
		// mnuMS_INUN
		// 
		this.mnuMS_INUN.Index = 2;
		this.mnuMS_INUN.Text = "����";
		// 
		// mnuMS_INUT
		// 
		this.mnuMS_INUT.Index = 3;
		this.mnuMS_INUT.Text = "����";
		// 
		// mnuCommonElements
		// 
		this.mnuCommonElements.Index = 5;
		this.mnuCommonElements.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.mnuCEConnection,
																						  this.mnuCERemovableKnot});
		this.mnuCommonElements.Text = "����� ����������";
		// 
		// mnuCEConnection
		// 
		this.mnuCEConnection.Index = 0;
		this.mnuCEConnection.Text = "����������";
		this.mnuCEConnection.Click += new System.EventHandler(this.mnuCEConnection_Click);
		// 
		// mnuCERemovableKnot
		// 
		this.mnuCERemovableKnot.Index = 1;
		this.mnuCERemovableKnot.Text = "���������� ����";
		this.mnuCERemovableKnot.Click += new System.EventHandler(this.mnuCERemovableKnot_Click);
		// 
		// tbbEnumeration
		// 
		this.tbbEnumeration.DropDownMenu = this.cmnuNumeration;
		this.tbbEnumeration.ImageIndex = 1;
		this.tbbEnumeration.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
		this.tbbEnumeration.Tag = "2";
		// 
		// cmnuNumeration
		// 
		this.cmnuNumeration.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mnuHeadNumeration,
																					   this.mnuN_Line1,
																					   this.mnuEnumKnots,
																					   this.mnuEnumBranch});
		// 
		// mnuHeadNumeration
		// 
		this.mnuHeadNumeration.DefaultItem = true;
		this.mnuHeadNumeration.Enabled = false;
		this.mnuHeadNumeration.Index = 0;
		this.mnuHeadNumeration.Text = "��������� ���������";
		// 
		// mnuN_Line1
		// 
		this.mnuN_Line1.Index = 1;
		this.mnuN_Line1.Text = "-";
		// 
		// mnuEnumKnots
		// 
		this.mnuEnumKnots.Checked = true;
		this.mnuEnumKnots.Index = 2;
		this.mnuEnumKnots.Text = "���������� ����";
		this.mnuEnumKnots.Click += new System.EventHandler(this.mnuEnumKnots_Click);
		// 
		// mnuEnumBranch
		// 
		this.mnuEnumBranch.Checked = true;
		this.mnuEnumBranch.Index = 3;
		this.mnuEnumBranch.Text = "���������� �����";
		this.mnuEnumBranch.Click += new System.EventHandler(this.mnuEnumBranch_Click);
		// 
		// tbbSimplify
		// 
		this.tbbSimplify.DropDownMenu = this.cmnuSimplify;
		this.tbbSimplify.ImageIndex = 4;
		this.tbbSimplify.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
		// 
		// cmnuSimplify
		// 
		this.cmnuSimplify.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuTransforms,
																					 this.mnuLine4,
																					 this.mnuAutoSimplify,
																					 this.mnuHandSimplify,
																					 this.mnuLine5,
																					 this.mnuBranches});
		// 
		// mnuTransforms
		// 
		this.mnuTransforms.DefaultItem = true;
		this.mnuTransforms.Enabled = false;
		this.mnuTransforms.Index = 0;
		this.mnuTransforms.Text = "��������������";
		// 
		// mnuLine4
		// 
		this.mnuLine4.Index = 1;
		this.mnuLine4.Text = "-";
		// 
		// mnuAutoSimplify
		// 
		this.mnuAutoSimplify.Index = 2;
		this.mnuAutoSimplify.Text = "��������������";
		this.mnuAutoSimplify.Click += new System.EventHandler(this.mnuAutoSimplify_Click);
		// 
		// mnuHandSimplify
		// 
		this.mnuHandSimplify.Index = 3;
		this.mnuHandSimplify.Text = "������ ...";
		this.mnuHandSimplify.Click += new System.EventHandler(this.mnuHandSimplify_Click);
		// 
		// tbbMatrix
		// 
		this.tbbMatrix.DropDownMenu = this.cmnuMatrix;
		this.tbbMatrix.ImageIndex = 2;
		this.tbbMatrix.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
		this.tbbMatrix.Tag = "3";
		// 
		// cmnuMatrix
		// 
		this.cmnuMatrix.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				   this.mnuHeadMatrix,
																				   this.mnuM_Line1,
																				   this.mnuIncidencMatrix,
																				   this.mnuParamMatrix});
		// 
		// mnuHeadMatrix
		// 
		this.mnuHeadMatrix.DefaultItem = true;
		this.mnuHeadMatrix.Enabled = false;
		this.mnuHeadMatrix.Index = 0;
		this.mnuHeadMatrix.Text = "�������";
		// 
		// mnuM_Line1
		// 
		this.mnuM_Line1.Index = 1;
		this.mnuM_Line1.Text = "-";
		// 
		// mnuIncidencMatrix
		// 
		this.mnuIncidencMatrix.Index = 2;
		this.mnuIncidencMatrix.Text = "����������";
		this.mnuIncidencMatrix.Click += new System.EventHandler(this.mnuIncidencMatrix_Click);
		// 
		// mnuParamMatrix
		// 
		this.mnuParamMatrix.Index = 3;
		this.mnuParamMatrix.Text = "����������";
		// 
		// tbbSeparator1
		// 
		this.tbbSeparator1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
		// 
		// tbbShowBaseKnots
		// 
		this.tbbShowBaseKnots.ImageIndex = 3;
		this.tbbShowBaseKnots.Pushed = true;
		this.tbbShowBaseKnots.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
		this.tbbShowBaseKnots.Tag = "4";
		// 
		// imglstImages
		// 
		this.imglstImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
		this.imglstImages.ImageSize = new System.Drawing.Size(32, 32);
		this.imglstImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglstImages.ImageStream")));
		this.imglstImages.TransparentColor = System.Drawing.Color.Transparent;
		// 
		// panWorkSpace
		// 
		this.panWorkSpace.AutoScroll = true;
		this.panWorkSpace.BackColor = System.Drawing.SystemColors.AppWorkspace;
		this.panWorkSpace.Controls.Add(this.picDrawBox);
		this.panWorkSpace.Location = new System.Drawing.Point(0, 72);
		this.panWorkSpace.Name = "panWorkSpace";
		this.panWorkSpace.Size = new System.Drawing.Size(696, 408);
		this.panWorkSpace.TabIndex = 0;
		this.panWorkSpace.Paint += new System.Windows.Forms.PaintEventHandler(this.panWorkSpace_Paint);
		// 
		// picDrawBox
		// 
		this.picDrawBox.BackColor = System.Drawing.SystemColors.Window;
		this.picDrawBox.Location = new System.Drawing.Point(8, 8);
		this.picDrawBox.Name = "picDrawBox";
		this.picDrawBox.Size = new System.Drawing.Size(592, 408);
		this.picDrawBox.TabIndex = 0;
		this.picDrawBox.TabStop = false;
		this.picDrawBox.Paint += new System.Windows.Forms.PaintEventHandler(this.picDrawBox_Paint);
		this.picDrawBox.DoubleClick += new System.EventHandler(this.picDrawBox_DoubleClick);
		this.picDrawBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picDrawBox_MouseMove);
		this.picDrawBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picDrawBox_MouseDown);
		// 
		// MainMenu
		// 
		this.MainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				 this.mnuFile,
																				 this.mnuAbout});
		// 
		// mnuFile
		// 
		this.mnuFile.Index = 0;
		this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				this.mnuNewFile,
																				this.mnuLine2,
																				this.mnuOpenFile,
																				this.mnuSaveFile,
																				this.mnuSaveAs,
																				this.mnuLine3,
																				this.mnuExit});
		this.mnuFile.Text = "����";
		// 
		// mnuNewFile
		// 
		this.mnuNewFile.Index = 0;
		this.mnuNewFile.OwnerDraw = true;
		this.mnuNewFile.Text = "�����";
		this.mnuNewFile.Click += new System.EventHandler(this.mnuNewFile_Click);
		// 
		// mnuLine2
		// 
		this.mnuLine2.Index = 1;
		this.mnuLine2.OwnerDraw = true;
		this.mnuLine2.Text = "-";
		// 
		// mnuOpenFile
		// 
		this.mnuOpenFile.Index = 2;
		this.mnuOpenFile.OwnerDraw = true;
		this.mnuOpenFile.Text = "������� ...";
		this.mnuOpenFile.Click += new System.EventHandler(this.mnuOpenFile_Click);
		// 
		// mnuSaveFile
		// 
		this.mnuSaveFile.Index = 3;
		this.mnuSaveFile.OwnerDraw = true;
		this.mnuSaveFile.Text = "���������";
		this.mnuSaveFile.Click += new System.EventHandler(this.mnuSaveFile_Click);
		// 
		// mnuSaveAs
		// 
		this.mnuSaveAs.Index = 4;
		this.mnuSaveAs.OwnerDraw = true;
		this.mnuSaveAs.Text = "��������� ��� ...";
		this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
		// 
		// mnuLine3
		// 
		this.mnuLine3.Index = 5;
		this.mnuLine3.OwnerDraw = true;
		this.mnuLine3.Text = "-";
		// 
		// mnuExit
		// 
		this.mnuExit.Index = 6;
		this.mnuExit.OwnerDraw = true;
		this.mnuExit.Text = "�����";
		this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
		// 
		// mnuAbout
		// 
		this.mnuAbout.Index = 1;
		this.mnuAbout.Text = "?";
		this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
		// 
		// cmnuElemMenu
		// 
		this.cmnuElemMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuElemHead,
																					 this.mnuElemLine,
																					 this.mnuElemParams,
																					 this.mnuDeleteElem});
		// 
		// mnuElemHead
		// 
		this.mnuElemHead.DefaultItem = true;
		this.mnuElemHead.Enabled = false;
		this.mnuElemHead.Index = 0;
		this.mnuElemHead.Text = "���� ...";
		// 
		// mnuElemLine
		// 
		this.mnuElemLine.Index = 1;
		this.mnuElemLine.Text = "-";
		// 
		// mnuElemParams
		// 
		this.mnuElemParams.Index = 2;
		this.mnuElemParams.Text = "��������� ...";
		this.mnuElemParams.Click += new System.EventHandler(this.mnuElemParams_Click);
		// 
		// mnuDeleteElem
		// 
		this.mnuDeleteElem.Index = 3;
		this.mnuDeleteElem.Text = "�������";
		this.mnuDeleteElem.Click += new System.EventHandler(this.mnuDeleteElem_Click);
		// 
		// GrapMnu
		// 
		this.GrapMnu.AutoBind = true;
		this.GrapMnu.BitmapBackColor = System.Drawing.Color.Gainsboro;
		this.GrapMnu.Font = null;
		this.GrapMnu.MenuItemBackColorEnd = System.Drawing.Color.Gainsboro;
		this.GrapMnu.MenuItemBackColorSelected = System.Drawing.Color.FromArgb(((System.Byte)(182)), ((System.Byte)(189)), ((System.Byte)(210)));
		this.GrapMnu.MenuItemBackColorStart = System.Drawing.Color.Snow;
		this.GrapMnu.MenuItemBorderSelected = System.Drawing.Color.Indigo;
		this.GrapMnu.MenuItemDithered = true;
		this.GrapMnu.MenuItemForeColor = System.Drawing.Color.Navy;
		this.GrapMnu.MenuItemForeColorDisabled = System.Drawing.Color.Gray;
		// 
		// tlbMenu
		// 
		this.tlbMenu.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
		this.tlbMenu.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																				   this.tbbNewFile,
																				   this.tbbOpenFile,
																				   this.tbbSaveFile,
																				   this.tbbSaveAs});
		this.tlbMenu.ButtonSize = new System.Drawing.Size(30, 30);
		this.tlbMenu.DropDownArrows = true;
		this.tlbMenu.ImageList = this.imglstMenu;
		this.tlbMenu.Location = new System.Drawing.Point(0, 0);
		this.tlbMenu.Name = "tlbMenu";
		this.tlbMenu.ShowToolTips = true;
		this.tlbMenu.Size = new System.Drawing.Size(640, 28);
		this.tlbMenu.TabIndex = 0;
		this.tlbMenu.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tlbMenu_ButtonClick);
		// 
		// tbbNewFile
		// 
		this.tbbNewFile.ImageIndex = 0;
		// 
		// tbbOpenFile
		// 
		this.tbbOpenFile.ImageIndex = 1;
		// 
		// tbbSaveFile
		// 
		this.tbbSaveFile.ImageIndex = 2;
		// 
		// tbbSaveAs
		// 
		this.tbbSaveAs.ImageIndex = 3;
		// 
		// imglstMenu
		// 
		this.imglstMenu.ImageSize = new System.Drawing.Size(16, 16);
		this.imglstMenu.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglstMenu.ImageStream")));
		this.imglstMenu.TransparentColor = System.Drawing.Color.Gainsboro;
		// 
		// mnuLine5
		// 
		this.mnuLine5.Index = 4;
		this.mnuLine5.Text = "-";
		// 
		// mnuBranches
		// 
		this.mnuBranches.Index = 5;
		this.mnuBranches.Text = "����� ������ ...";
		this.mnuBranches.Click += new System.EventHandler(this.mnuBranches_Click);
		// 
		// frmMain
		// 
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		this.ClientSize = new System.Drawing.Size(640, 513);
		this.Controls.Add(this.tlbMain);
		this.Controls.Add(this.tlbMenu);
		this.Controls.Add(this.panWorkSpace);
		this.Menu = this.MainMenu;
		this.Name = "frmMain";
		this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Closing += new System.ComponentModel.CancelEventHandler(this.frmMain_Closing);
		this.SizeChanged += new System.EventHandler(this.frmMain_SizeChanged);
		this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmMain_KeyPress);
		this.Load += new System.EventHandler(this.frmMain_Load);
		this.panWorkSpace.ResumeLayout(false);
		this.ResumeLayout(false);

	}
	#endregion

	[STAThread] static void Main() {
		Application.Run(new frmMain());
	}

	// ----------------------------------------------------------------------
	public frmMain() {
		InitializeComponent();
	}

	// ----------------------------------------------------------------------
	protected override void Dispose( bool disposing ) {
		if( disposing )
			if (components != null)  components.Dispose();
		base.Dispose( disposing );
	}

	// ----------------------------------------------------------------------
	private void frmMain_Load(object sender, System.EventArgs e) {
		GrapMnu.Init(MainMenu);
		GrapMnu.Init(cmnuElements);
		GrapMnu.Init(cmnuElemMenu);
		GrapMnu.Init(cmnuMatrix);
		GrapMnu.Init(cmnuNumeration);
		GrapMnu.Init(cmnuSimplify);
		
		GrapMnu.AddIcon(mnuNewFile, imglstMenu.Images[0] as Bitmap);
		GrapMnu.AddIcon(mnuOpenFile, imglstMenu.Images[1] as Bitmap);
		GrapMnu.AddIcon(mnuSaveFile, imglstMenu.Images[2] as Bitmap);
		GrapMnu.AddIcon(mnuSaveAs, imglstMenu.Images[3] as Bitmap);
		GrapMnu.AddIcon(mnuDeleteElem, imglstMenu.Images[4] as Bitmap);
		GrapMnu.AddIcon(mnuElemParams, imglstMenu.Images[5] as Bitmap);
		GrapMnu.AddIcon(mnuCCResistance, imglstMenu.Images[6] as Bitmap);
		GrapMnu.AddIcon(mnuCCCurrentSrc, imglstMenu.Images[7] as Bitmap);
		GrapMnu.AddIcon(mnuCCVoltageSrc, imglstMenu.Images[8] as Bitmap);
		GrapMnu.AddIcon(mnuCEConnection, imglstMenu.Images[9] as Bitmap);
		GrapMnu.AddIcon(mnuCERemovableKnot, imglstMenu.Images[10] as Bitmap);

		mnuNewFile_Click(null, null);

		// ��������� ������ picDrawBox, ���� �� ���� ����������� ��� ������� ����
		picDrawBox.Size = new Size(START_SHIFT + KNOTS_DIST * KNOTS_X_NUM,
			START_SHIFT + KNOTS_DIST * KNOTS_Y_NUM);

		tlbMain.DropDownArrows = false;
		Branches = new ArrayList();
	}

	// ----------------------------------------------------------------------
	private void frmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
		DialogResult res = ShowWarning();

		if(res == DialogResult.Yes)
			mnuSaveFile_Click(null, null);
		else if(res == DialogResult.Cancel)
			e.Cancel = false;
	}

	// ----------------------------------------------------------------------
	private void frmMain_SizeChanged(object sender, System.EventArgs e) {
		panWorkSpace.Width = ClientSize.Width;
		panWorkSpace.Height = ClientSize.Height - tlbMain.Height - tlbMenu.Height;
	}

	// ----------------------------------------------------------------------
	/// <summary>������ ����, �� ����� �� ������</summary>
	private void panWorkSpace_Paint(object sender, System.Windows.Forms.PaintEventArgs e) {
		const int SHADOW_SHIFT = 5; // �� ������� �������� "����������" ������ ����
		const int SHADOW_SIZE = 5;	// ������ �������������� ����
		SolidBrush brShadow = new SolidBrush(Color.FromArgb(0,0,0)); // ���� ����
		Rectangle[] Rects = new Rectangle[2]; // "����" ������������ �� ���� 2 �������������� ��� � ������ �� ����� �� ������

		Rects[0].X = picDrawBox.Location.X + SHADOW_SHIFT;
		Rects[0].Y = picDrawBox.Location.Y + picDrawBox.Size.Height + 1;
		Rects[0].Width = picDrawBox.Size.Width;
		Rects[0].Height = SHADOW_SIZE;
		Rects[1].X = picDrawBox.Location.X + picDrawBox.Size.Width + 1;
		Rects[1].Y = picDrawBox.Location.Y + SHADOW_SHIFT;
		Rects[1].Width = SHADOW_SIZE;
		Rects[1].Height = picDrawBox.Size.Height;
		e.Graphics.FillRectangles(brShadow, Rects);
	}

	// ----------------------------------------------------------------------
	/// <summary>������ �����</summary>
	private void picDrawBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e) {
		int i;
		Element pCurrEl;
		BaseKnot KnotOver = null; // ����, ��� ������� ������ ��������������� ����
		Point CursorPos = Cursor.Position;

		// ������ ��������
		for(i = 0; i < Elements.Count; i++) {
			pCurrEl = (Element) Elements[i];
			pCurrEl.Draw(e.Graphics);
		}

		// ������ ����
		if(DragKnot != null) {
			KnotOver = BaseKnotFromCoord(CursorPos);
			if(KnotOver != null) {
				KnotOver.Visible = false;
				if(KnotOver.IsRemovableKnot || KnotOver.IsKnot) {
					KnotOver.Visible = true;
					KnotOver = null;
					picDrawBox.Cursor = Cursors.No;
				}
				else if(picDrawBox.Cursor == Cursors.No) picDrawBox.Cursor = Cursors.Cross;
			}
		}
		for(i = 0; i < KNOTS_X_NUM; i++)
			for(int i2 = 0; i2 < KNOTS_Y_NUM; i2++)
				Knots[i, i2].Draw(e.Graphics);
		if(KnotOver != null) KnotOver.Visible = true;

		// ������ ������ ������
		if(SchemaClosed && DrawBranchNumbers)
			for(i = 0; i < Branches.Count; i++)
				(Branches[i] as Branch).DrawNumbers(e.Graphics);

		// ���� ������ ���-�� ��������������� ...
		if(State == ProgState.PS_DRAG && ActiveForm == this) {
			if(DragElem != null) { // ��������������� �������
				int KnotIndexX, KnotIndexY;
				Side side;
				Rectangle KnotRect = new Rectangle();

				if(!GetPointInKnots(CursorPos ,out KnotIndexX, out KnotIndexY)) return;
				if(KnotIndexX == KNOTS_X_NUM - 1 || KnotIndexY == KNOTS_Y_NUM - 1) return;
				KnotRect.X = Knots[KnotIndexX, KnotIndexY].Coord.X;
				KnotRect.Y = Knots[KnotIndexX, KnotIndexY].Coord.Y;
				KnotRect.Width = KnotRect.Height = KNOTS_DIST;
				side = BaseKnot.GetRectSide(KnotRect, picDrawBox.PointToClient(Cursor.Position));
				if(GetBuzy(KnotIndexX, KnotIndexY, side)) {
					picDrawBox.Cursor = Cursors.No;
					return;
				}
				else
					if(picDrawBox.Cursor == Cursors.No) picDrawBox.Cursor = Cursors.Cross;
			
				switch(side) {
					case Side.S_LEFT:
						DragElem.pKnot1 = Knots[KnotIndexX, KnotIndexY];
						DragElem.pKnot2 = Knots[KnotIndexX, KnotIndexY + 1];
						DragElem.IsVertical = true;
						break;
					case Side.S_RIGHT:
						DragElem.pKnot1 = Knots[KnotIndexX + 1, KnotIndexY];
						DragElem.pKnot2 = Knots[KnotIndexX + 1, KnotIndexY + 1];
						DragElem.IsVertical = true;
						break;
					case Side.S_UPPER:
						DragElem.pKnot1 = Knots[KnotIndexX, KnotIndexY];
						DragElem.pKnot2 = Knots[KnotIndexX + 1, KnotIndexY];
						DragElem.IsVertical = false;
						break;
					case Side.S_LOWER:
						DragElem.pKnot1 = Knots[KnotIndexX, KnotIndexY + 1];
						DragElem.pKnot2 = Knots[KnotIndexX + 1, KnotIndexY + 1];
						DragElem.IsVertical = false;
						break;
				}
				DragElem.Draw(e.Graphics);
			}
			else if(KnotOver != null) { //��������������� ����
				DragKnot.Coord = KnotOver.Coord;
				BaseKnot.DrawKnotNumbers = false;
				DragKnot.Draw(e.Graphics);
				BaseKnot.DrawKnotNumbers = true;
			}
		}
	}



	#region MenuHandlers
	// ======================================================================
	// ------------------------- T o o l   B a r
	private void mnuCEConnection_Click(object sender, System.EventArgs e) {
		DragElem = new Connection();
		State = ProgState.PS_DRAG;
	}

	// ----------------------------------------------------------------------
	private void mnuCERemovableKnot_Click(object sender, System.EventArgs e) {
		DragKnot = new BaseKnot(new Point(0,0));
		DragKnot.IsRemovableKnot = true;
		State = ProgState.PS_DRAG;
	}

	// ----------------------------------------------------------------------
	private void mnuCCResistance_Click(object sender, System.EventArgs e) {
		DragElem = new Rezistor();
		State = ProgState.PS_DRAG;
	}

	// ----------------------------------------------------------------------
	private void mnuCCVoltageSrc_Click(object sender, System.EventArgs e) {
		DragElem = new VoltageSrc();
		State = ProgState.PS_DRAG;
	}

	// ----------------------------------------------------------------------
	private void mnuCCCurrentSrc_Click(object sender, System.EventArgs e) {
		DragElem = new CurrentSrc();
		State = ProgState.PS_DRAG;
	}

	// ----------------------------------------------------------------------
	private void mnuEnumKnots_Click(object sender, System.EventArgs e) {
		MenuItem a = sender as MenuItem;

		a.Checked = !a.Checked;
		BaseKnot.DrawKnotNumbers = a.Checked;
		picDrawBox.Invalidate();
	}

	// ----------------------------------------------------------------------
	private void mnuEnumBranch_Click(object sender, System.EventArgs e) {
		MenuItem mi = sender as MenuItem;

		mi.Checked = !mi.Checked;
		DrawBranchNumbers = mi.Checked;
	}

	// ----------------------------------------------------------------------
	private void mnuAutoSimplify_Click(object sender, System.EventArgs e) {
		bool Ser = true, Par = true;
		
		while(Ser || Par) {
			Ser = SerialSimplify(Branches);
			Par = ParallelSimplify(Branches);
		}
	}

	// ----------------------------------------------------------------------
	private void mnuHandSimplify_Click(object sender, System.EventArgs e) {
		frmSelectBranches dlg = new frmSelectBranches(Branches, Elements);

		if(dlg.ShowDialog() != DialogResult.OK) return;

		bool Ser = true, Par = true;
		
		while(Ser || Par) {
			Ser = SerialSimplify(dlg.SelectedBranches);
			Par = ParallelSimplify(dlg.SelectedBranches);
		}
	}

	// ----------------------------------------------------------------------
	private void mnuBranches_Click(object sender, System.EventArgs e) {
		frmBranches dlg = new frmBranches(Branches);

		dlg.ShowDialog();
	}

	// ----------------------------------------------------------------------
	private void mnuIncidencMatrix_Click(object sender, System.EventArgs e) {
		frmIncidentMatrix dlg = new frmIncidentMatrix(Knots, Branches);

		dlg.ShowDialog();
	}

	// ----------------------------------------------------------------------
	private void tlbMenu_ButtonClick(object sender, 
		System.Windows.Forms.ToolBarButtonClickEventArgs e) 
	{
		switch(e.Button.ImageIndex) {
			case 0:		// NewFile
				mnuNewFile_Click(null, null);
				break;
			case 1:		// OpenFile
				mnuOpenFile_Click(null, null);
				break;
			case 2:		// SaveFile
				mnuSaveFile_Click(null, null);
				break;
			case 3:		// Save As
				mnuSaveAs_Click(null, null);
				break;
		}
	}

	// ======================================================================
	private void mnuDeleteElem_Click(object sender, System.EventArgs e) {
		if(MenuElem != null) {
			DeleteElement(MenuElem, true, true);
			MenuElem = null;
		}
		else if(MenuKnot != null) {
			if(MenuKnot.IsRemovableKnot) {
				MenuKnot.IsRemovableKnot = false;
				BaseKnot.NumerateKnots(Knots);
				Branch.CreateBranches(Knots, out Branches);
				picDrawBox.Invalidate();
				MenuKnot = null;
			}
		}
	}

	// ----------------------------------------------------------------------
	private void mnuElemParams_Click(object sender, System.EventArgs e) {
		if(MenuElem == null) return;
		MenuElem.ShowParamDialog();
		MenuElem = null;
	}

	
	// ======================================================================
	// ------------------------- � � � � � � �   � � � � 

	private void mnuExit_Click(object sender, System.EventArgs e) {
		Application.Exit();
	}

	// ----------------------------------------------------------------------
	private void mnuNewFile_Click(object sender, System.EventArgs e) {
		DialogResult res = ShowWarning();

		if(res == DialogResult.Cancel) return;
		else if(res == DialogResult.Yes)
			mnuSaveFile_Click(null, null);

		int x = START_SHIFT, y = START_SHIFT;

		if(Elements != null) Elements.Clear();
		if(Branches != null) Branches.Clear();
		BuzyPos = new bool[KNOTS_X_NUM + 1, KNOTS_Y_NUM + 1,2];

		Knots = new BaseKnot[KNOTS_X_NUM, KNOTS_Y_NUM];
		for(int i = 0; i < KNOTS_X_NUM; i++) {
			for(int i2 = 0; i2 < KNOTS_Y_NUM; i2++) {
				Knots[i, i2] = new BaseKnot(new Point(x, y));
				y += KNOTS_DIST;
			}
			x += KNOTS_DIST;
			y = START_SHIFT;
		}
		picDrawBox.Invalidate();
		IsModifed = false;
		SchemaName = "����� �����";
		SchemaClosed = false;
	}

	// ----------------------------------------------------------------------
	private void mnuSaveFile_Click(object sender, System.EventArgs e) {
		if(SavePath != null) {
			SaveSchema(SavePath);
		}
		else {
			SaveFileDialog dlg = new SaveFileDialog();

			dlg.OverwritePrompt = true;
			dlg.RestoreDirectory = true;
			dlg.Title = "��������� �����";
			dlg.Filter = "����� ���� (*.toe)|*.toe|��� ����� (*.*)|*.*";
			dlg.FileName = SchemaName;
			if(dlg.ShowDialog() == DialogResult.OK)
				SaveSchema(dlg.FileName);
		}
	}

	// ----------------------------------------------------------------------
	private void mnuSaveAs_Click(object sender, System.EventArgs e) {
		SaveFileDialog dlg = new SaveFileDialog();

		dlg.OverwritePrompt = true;
		dlg.RestoreDirectory = true;
		dlg.Title = "��������� �����";
		dlg.Filter = "����� ���� (*.toe)|*.toe|��� ����� (*.*)|*.*";
		dlg.FileName = SchemaName;
		if(dlg.ShowDialog() == DialogResult.OK)
			SaveSchema(dlg.FileName);
	}

	// ----------------------------------------------------------------------
	private void mnuOpenFile_Click(object sender, System.EventArgs e) {
		DialogResult res = ShowWarning();

		if(res == DialogResult.Cancel)	return;
		if(res == DialogResult.Yes)		mnuSaveFile_Click(null, null);

		OpenFileDialog dlg = new OpenFileDialog();

		dlg.RestoreDirectory = true;
		dlg.Title = "������� �����";
		dlg.Filter = "����� ���� (*.toe)|*.toe|��� ����� (*.*)|*.*";
		if(dlg.ShowDialog() == DialogResult.OK) {
			LoadSchema(dlg.FileName);
			SchemaName = Path.GetFileNameWithoutExtension(dlg.FileName);
		}
	}

	// ----------------------------------------------------------------------
	private void mnuAbout_Click(object sender, System.EventArgs e) {
		frmAbout dlg = new frmAbout();

		dlg.ShowDialog();
	}
	#endregion


	// ======================================================================
	// ----------------------------------------------------------------------
	private void frmMain_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
		if(State == ProgState.PS_DRAG && e.KeyChar == ' ') ReverseElem();
	}

	// ----------------------------------------------------------------------
	private void picDrawBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
		if(State == ProgState.PS_DRAG) {
			picDrawBox.Invalidate();
		}
	}

	// ----------------------------------------------------------------------
	private void tlbMain_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e) {
		ToolBar pTlb = sender as ToolBar;

		switch(Convert.ToInt32(e.Button.Tag)) {
			case 4:
				BaseKnotsVisible = e.Button.Pushed ? true : false;
				break;
		}
	}

	// ----------------------------------------------------------------------
	private void picDrawBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
		Point	CurPos = new Point(e.X, e.Y),
				CurScreenPos = picDrawBox.PointToScreen(CurPos);

		if(e.Button == MouseButtons.Right) { // ������ ������ ������ ����
			int KnotX, KnotY;
			Side side;
			Rectangle rect = new Rectangle();

			if(State == ProgState.PS_DRAG) {	// ������ ��������������
				State = ProgState.PS_NORMAL;
				Cursor = Cursors.Default;
			}
			else if(BaseKnotFromCoordExact(CurScreenPos) != null) { 
				// �������� ����������� ���� �������� ����
				BaseKnot Knot = BaseKnotFromCoordExact(CurScreenPos);
				if(Knot.IsRemovableKnot) {
					MenuKnot = Knot;
					mnuElemParams.Enabled = false;
					mnuElemHead.Text = "���� ����������� ����";
					picDrawBox.ContextMenu = cmnuElemMenu;
					picDrawBox.ContextMenu.Show(picDrawBox, CurPos);
					picDrawBox.ContextMenu = null;
					mnuElemParams.Enabled = true;
				}
			}
			else { // �������� ����������� ���� ��������
				if(!GetPointInKnots(CurScreenPos, out KnotX, out KnotY)) 
					return;
				rect.X = Knots[KnotX, KnotY].Coord.X;
				rect.Y = Knots[KnotX, KnotY].Coord.Y;
				rect.Width = rect.Height = KNOTS_DIST;
				side = BaseKnot.GetRectSide(rect, CurPos);
				if(GetBuzy(KnotX, KnotY, side)) {	// �������� ����������� ����
					MenuElem = GetElement(KnotX, KnotY, side);
					if(MenuElem == null) return;
					mnuElemHead.Text = "* ���� �������� ���� *";
					if(MenuElem is Connection) mnuElemParams.Enabled = false;
					picDrawBox.ContextMenu = cmnuElemMenu;
					picDrawBox.ContextMenu.Show(picDrawBox, CurPos);
					picDrawBox.ContextMenu = null;
					mnuElemParams.Enabled = true;
				}
			}
		}
		else if(State == ProgState.PS_DRAG && e.Button == MouseButtons.Left) { // drop 
			if(DragElem != null) { // drop element
				if(e.X < START_SHIFT || e.Y < START_SHIFT || // ���� drop ����� �� ���� - �������
					e.X > Knots[KNOTS_X_NUM - 1, KNOTS_Y_NUM - 1].Coord.X ||
					e.Y > Knots[KNOTS_X_NUM - 1, KNOTS_Y_NUM - 1].Coord.Y) return;

				int Knx, Kny;
				Rectangle rect = new Rectangle();
				Side side;
			
				GetPointInKnots(CurScreenPos, out Knx, out Kny);
				rect.X = Knots[Knx, Kny].Coord.X;
				rect.Y = Knots[Knx, Kny].Coord.Y;
				rect.Width = rect.Height = KNOTS_DIST;
				side = BaseKnot.GetRectSide(rect, CurPos);
				CreateElement(Knx, Kny, side, ref DragElem, true, true, true);
				if(DragElem is Connection)
					DragElem = new Connection();
				else if(DragElem is Rezistor)
					DragElem = new Rezistor();
				else if(DragElem is VoltageSrc)
					DragElem = new VoltageSrc();
				else if(DragElem is CurrentSrc)
					DragElem = new CurrentSrc();
				else
					MessageBox.Show("������ � picDrawBox_MouseDown");
			}
			else if(DragKnot != null) { // drop baseknot
				if(picDrawBox.Cursor == Cursors.No) return;

				BaseKnot Kn = BaseKnotFromCoord(CurScreenPos);
				
				if(Kn == null) return;
				CreateRemovableKnot(Kn);
				DragKnot = new BaseKnot(DragKnot.Coord);
				DragKnot.IsRemovableKnot = true;
			}
			else if(State == ProgState.PS_DRAG && 
				e.Button == MouseButtons.Middle) // reverse element
				ReverseElem();
		}
	}

	// -----------------------------------------------------------------------------
	private void picDrawBox_DoubleClick(object sender, System.EventArgs e) {
		if(State != ProgState.PS_NORMAL) return;

		int KnotX, KnotY;
		Side side;
		Element pElem;
		Rectangle rect = new Rectangle();

		if(!GetPointInKnots(Cursor.Position ,out KnotX, out KnotY)) return;
		rect.X = Knots[KnotX, KnotY].Coord.X;
		rect.Y = Knots[KnotX, KnotY].Coord.Y;
		rect.Width = rect.Height = KNOTS_DIST;
		side = BaseKnot.GetRectSide(rect, picDrawBox.PointToClient(Cursor.Position));
		pElem = GetElement(KnotX, KnotY, side);
		if(pElem == null) return;
		pElem.ShowParamDialog();
		picDrawBox.Invalidate();
	}

	// ==============================================================================

	// -----------------------------------------------------------------------------
	/// <summary>
	/// ���������� ������ � �������� "��������� ���������?".
	/// ���������� ���� ��: Yes, No, Cancel
	/// </summary>
	private DialogResult ShowWarning() {
		if(IsModifed) {
			DialogResult res = MessageBox.Show("��������� ���������?", "", MessageBoxButtons.YesNoCancel,
				MessageBoxIcon.Question);
			if(res == DialogResult.Yes) IsModifed = false;
			return res;
		}
		else return DialogResult.No;
	}

	// -----------------------------------------------------------------------------
	private void SetBuzy(int KnotX, int KnotY, Side side, bool Val) {
		switch(side) {
			case Side.S_LEFT:
				BuzyPos[KnotX, KnotY, 1] = Val;
				break;
			case Side.S_RIGHT:
				BuzyPos[KnotX + 1, KnotY, 1] = Val;
				break;
			case Side.S_UPPER:
				BuzyPos[KnotX, KnotY, 0] = Val;
				break;
			case Side.S_LOWER:
				BuzyPos[KnotX, KnotY + 1, 0] = Val;
				break;
		}
	}

	// -----------------------------------------------------------------------------
	private bool GetBuzy(int KnotX, int KnotY, Side side) {
		switch(side) {
			case Side.S_LEFT:
				return BuzyPos[KnotX, KnotY, 1];
			case Side.S_RIGHT:
				return BuzyPos[KnotX + 1, KnotY, 1];
			case Side.S_UPPER:
				return BuzyPos[KnotX, KnotY, 0];
			case Side.S_LOWER:
				return BuzyPos[KnotX, KnotY + 1, 0];
		}
		return true;
	}

	// -----------------------------------------------------------------------------
	private void ReverseElem() {
		DragElem.IsReversed = !DragElem.IsReversed;
		picDrawBox.Invalidate();
	}

	// -----------------------------------------------------------------------------
	/// <summary>
	/// ���� �������� ������ ���, ����� ���-�� ��������� ���� ��������
	/// </summary>
	private void ElementsChanged(bool Redraw, bool RecreateBranches) {
		IsModifed = true;
		BaseKnot.NumerateKnots(Knots);
		if(RecreateBranches) {
			if(BaseKnot.CountKnots(Knots) != 0) {
				SchemaClosed = Branch.CreateBranches(Knots, out Branches);
				Branch.NumerateBranches(Branches);
			}
		}
		Element.NumerateElements(Elements);
		if(SchemeIsContour()) MakeBranchFromContour();
		if(Redraw) picDrawBox.Invalidate();
	}

	// -----------------------------------------------------------------------------
	/// <summary>������� ����� ������� �� �����</summary>
	private void CreateElement(int KnotX, int KnotY, Side side, ref Element NewEl, 
		bool Redraw, bool ShowParamDlg, bool RecreateBranches)
	{
		if(GetBuzy(KnotX, KnotY, side)) return; // ���� ����� ������ �������

		switch(side) {
				/* �������� ����� pKnot1 � pKnot2 ������� NewEl ����������� ���:
				 * ���� ������� ����� �����������: pKnot1 - ��� ������� ����, pKnot2 - ������
				 * ���� �������������: pKnot1 - ����� ����, pKnot2 - ������ */
			case Side.S_LEFT:
				NewEl.pKnot1 = Knots[KnotX, KnotY];
				NewEl.pKnot2 = Knots[KnotX, KnotY + 1];
				NewEl.IsVertical = true;
				Knots[KnotX, KnotY].AddElement(ref NewEl);
				Knots[KnotX, KnotY + 1].AddElement(ref NewEl);
				break;
			case Side.S_RIGHT:
				NewEl.pKnot1 = Knots[KnotX + 1, KnotY];
				NewEl.pKnot2 = Knots[KnotX + 1, KnotY + 1];
				NewEl.IsVertical = true;
				Knots[KnotX + 1, KnotY].AddElement(ref NewEl);
				Knots[KnotX + 1, KnotY + 1].AddElement(ref NewEl);
				break;
			case Side.S_UPPER:
				NewEl.pKnot1 = Knots[KnotX, KnotY];
				NewEl.pKnot2 = Knots[KnotX + 1, KnotY];
				NewEl.IsVertical = false;
				Knots[KnotX, KnotY].AddElement(ref NewEl);
				Knots[KnotX + 1, KnotY].AddElement(ref NewEl);
				break;
			case Side.S_LOWER:
				NewEl.pKnot1 = Knots[KnotX, KnotY + 1];
				NewEl.pKnot2 = Knots[KnotX + 1, KnotY + 1];
				NewEl.IsVertical = false;
				Knots[KnotX, KnotY + 1].AddElement(ref NewEl);
				Knots[KnotX + 1, KnotY + 1].AddElement(ref NewEl);
				break;
		}
		
		if(ShowParamDlg) NewEl.ShowParamDialog();
		NewEl.Number = Elements.Count + 1;
		Elements.Add(NewEl);
		SetBuzy(KnotX, KnotY, side, true); // ������ ��� ������� ������
		ElementsChanged(Redraw, RecreateBranches);
	}

	// ----------------------------------------------------------------------
	/// <summary>
	/// ������ ������� ���� � ��������� ��������� ���������� (���� ��� �� ����)
	/// </summary>
	private void CreateRemovableKnot(BaseKnot kn) {		
		if(kn.IsKnot) return;
		kn.IsRemovableKnot = true;
		IsModifed = true;
		ElementsChanged(false, true);
	}

	// ----------------------------------------------------------------------
	private void DeleteElement(Element DelEl, bool Redraw, bool RecreateBranches) {
		int KnotX, KnotY;
		Side side;

		GetElementPos(DelEl, out KnotX, out KnotY, out side);
		if(KnotX == -1) return;
		SetBuzy(KnotX, KnotY, side, false);
		DelEl.pKnot1.DeleteElement(DelEl);
		DelEl.pKnot2.DeleteElement(DelEl);
		DelEl.pKnot1.Visible = DelEl.pKnot2.Visible = true;
		if(Elements.IndexOf(DelEl) == -1) return;
		Elements.Remove(DelEl);
		ElementsChanged(Redraw, RecreateBranches);
	}

	// ----------------------------------------------------------------------
	/// <summary>������� �������� ����� �� �����</summary>
	private void DeleteBranch(Branch DelBr) {
		for(int i = 0; i < DelBr.Length - 1; i++) 
			DeleteElement(DelBr.GetElement(i), false, false);
		Branches.Remove(DelBr);
		DeleteElement(DelBr.GetElement(DelBr.Length - 1), true, true);
	}

	// ----------------------------------------------------------------------
	/// <summary>�������� �� ����� ���� ������� ������</summary>
	private void ChangeElement(Element OldEl, Element NewEl, bool Redraw, bool ShowParamDlg) {
		int Knx, Kny;
		Side side;

		if(!GetElementPos(OldEl, out Knx, out Kny, out side)) return;
		DeleteElement(OldEl, false, false);
		if(NewEl.pKnot1 != null) NewEl.pKnot1.DeleteElement(NewEl);
		if(NewEl.pKnot2 != null) NewEl.pKnot2.DeleteElement(NewEl);
		NewEl.pKnot1 = NewEl.pKnot1 = null;
		CreateElement(Knx, Kny, side, ref NewEl, Redraw, ShowParamDlg, false);
	}

	// ----------------------------------------------------------------------
	/// <summary>
	/// ����������, ��������� �� ����� ��� ������ (����� � �������� �����.).
	/// ���� �� - ���������� true � ����������(�������) ������ �������� 
	/// �������� ����, ���� ��������, � ������� �� ���������, 
	/// ���� ��� - ���������� false.
	/// </summary>
	private bool GetPointInKnots(Point pt, out int KnotX, out int KnotY) {
		Point CurPos =  picDrawBox.PointToClient(pt);

		CurPos.X -= START_SHIFT;
		CurPos.Y -= START_SHIFT;
		KnotX = KnotY = -1;
		
		// ���� ������ �� �������� �� picDrawBox ��� ��������� �� ������� �� �����
		if(!PointInDrawBox(pt)) return false;

		// ������ �������� �� �����
		KnotX = CurPos.X / KNOTS_DIST;
		KnotY = CurPos.Y / KNOTS_DIST;
		return true;
	}


	// -----------------------------------------------------------------------------
	/// <summary>���������� ������� ����� � ��������� �������� �� ��������� �� ����</summary>
	private bool GetElementPos(Element Elem, out int KnotX, out int KnotY, out Side side) {
		if(Elem == null) {
			KnotX = KnotY = -1;
			side = Side.S_LEFT;
			return false;
		}
		Rectangle rect = new Rectangle();
		Point pt = new Point();

		KnotX = (Elem.pKnot1.Coord.X - START_SHIFT) / KNOTS_DIST;
		KnotY = (Elem.pKnot1.Coord.Y - START_SHIFT) / KNOTS_DIST;
		rect.X = Elem.pKnot1.Coord.X;
		rect.Y = Elem.pKnot1.Coord.Y;
		rect.Width = rect.Height = KNOTS_DIST;
		pt.X = (Elem.pKnot1.Coord.X + Elem.pKnot2.Coord.X) / 2 + 1;
		pt.Y = (Elem.pKnot1.Coord.Y + Elem.pKnot2.Coord.Y) / 2 + 1;
		side = BaseKnot.GetRectSide(rect, pt);
		return GetBuzy(KnotX, KnotY, side);
	}

	// -----------------------------------------------------------------------------
	/// <summary>
	/// ���� �� �������� ������� ��������� ������� ���������� ���. ���� ��� - null
	/// </summary>
	private Element GetElement(int KnotX, int KnotY, Side side) {
		BaseKnot pKnot1 = null, pKnot2 = null;

		if(GetBuzy(KnotX, KnotY, side)) {
			switch(side) {
				case Side.S_LEFT:
					pKnot1 = Knots[KnotX, KnotY];
					pKnot2 = Knots[KnotX, KnotY + 1];
					break;
				case Side.S_RIGHT:
					pKnot1 = Knots[KnotX + 1, KnotY];
					pKnot2 = Knots[KnotX + 1, KnotY + 1];
					break;
				case Side.S_UPPER:
					pKnot1 = Knots[KnotX, KnotY];
					pKnot2 = Knots[KnotX + 1, KnotY];
					break;
				case Side.S_LOWER:
					pKnot1 = Knots[KnotX, KnotY + 1];
					pKnot2 = Knots[KnotX + 1, KnotY + 1];
					break;
			}

			for(int i = 0; i < pKnot1.numElems; i++)
				for(int i2 = 0; i2 < pKnot2.numElems; i2++)
					if(pKnot1.GetElement(i) == pKnot2.GetElement(i2) &&
						pKnot1.GetElement(i) != null)
						return pKnot1.GetElement(i);
			return null;
		}
		else return null;
	}

	// -----------------------------------------------------------------------------
	/// <summary>
	/// ���������� �� �������� ����� �� �����?
	/// ���������� ����� � �������� ������� ���������.
	/// ������� �� ����� �����������
	/// </summary>
	private bool PointInDrawBox(Point pt) {
		pt = picDrawBox.PointToClient(pt);
		if(pt.X < START_SHIFT || pt.Y < START_SHIFT) return false;
		if(pt.X > picDrawBox.Width - START_SHIFT || pt.Y > picDrawBox.Width - START_SHIFT)
			return false;
		return true;
	}

	// -----------------------------------------------------------------------------
	/// <summary>
	/// ���������� ����� ������� ������� ����, � �������� ����� �� �����.
	/// ���������� ����� � �������� ������� ���������
	/// ���� ����� ��� ����� ���������� null
	/// </summary>
	private BaseKnot BaseKnotFromCoord(Point pt) {
		if(!PointInDrawBox(pt)) return null;
		pt = picDrawBox.PointToClient(pt);

		Point KnotCoord = new Point();
		double x;

		pt.X += START_SHIFT;
		pt.Y += START_SHIFT;
		x = Convert.ToDouble(pt.X) / Convert.ToDouble(KNOTS_DIST);
		KnotCoord.X = (int) Math.Round(x) - 1;
		x = Convert.ToDouble(pt.Y) / Convert.ToDouble(KNOTS_DIST);
		KnotCoord.Y = (int) Math.Round(x) - 1;
		if(KnotCoord.X > Knots.GetLength(0) || KnotCoord.Y > Knots.GetLength(1)) 
			return null;
		return Knots[KnotCoord.X, KnotCoord.Y];
	}

	// -----------------------------------------------------------------------------
	/// <summary>
	/// ���������� ������� ����, ���� ����� ���������� �� ���
	/// ���������� ����� � �������� ������� ���������
	/// ���� ����� �� ����� �� ������� ���� ���������� null
	/// </summary>
	private BaseKnot BaseKnotFromCoordExact(Point pt) {
		if(!PointInDrawBox(pt)) return null;

		Size Precistion = new Size(10, 10);
		Point KnotCoord = new Point();
		double x;
		int a, b;

		pt = picDrawBox.PointToClient(pt);
		pt.X += START_SHIFT;
		pt.Y += START_SHIFT;
		x = Convert.ToDouble(pt.X) / Convert.ToDouble(KNOTS_DIST);
		a = (int) Math.Floor(x) - 1;
		b = (int) Math.Ceiling(x) - 1;
		if(Knots[a, 0].Coord.X + Precistion.Width / 2 >= pt.X)
			KnotCoord.X = a;
		else if(Knots[b, 0].Coord.X - Precistion.Width / 2 <= pt.X)
			KnotCoord.X = b;
		else
			return null;
		x = Convert.ToDouble(pt.Y) / Convert.ToDouble(KNOTS_DIST);
		a = (int) Math.Floor(x) - 1;
		b = (int) Math.Ceiling(x) - 1;
		if(Knots[0, a].Coord.Y + Precistion.Height / 2 >= pt.Y)
			KnotCoord.Y = a;
		else if(Knots[0, b].Coord.Y - Precistion.Height / 2 <= pt.Y)
			KnotCoord.Y = b;
		else
			return null;
		return Knots[KnotCoord.X, KnotCoord.Y];
	}

	// -----------------------------------------------------------------------------
	/// <summary>
	/// ��������� ����� � ����, ��������� ������������
	/// </summary>
	private void SaveSchema(string FilePath) {
		IFormatter formatter = new BinaryFormatter();
		Stream stream = new FileStream(FilePath, FileMode.Create, 
			FileAccess.Write, FileShare.None);

		formatter.Serialize(stream, Knots);
		formatter.Serialize(stream, BuzyPos);
		stream.Close();
		IsModifed = false;
		SavePath = FilePath;
	}

	// -----------------------------------------------------------------------------
	/// <summary>��������� ����� �� �����, ��������� ������������</summary>
	private void LoadSchema(string FilePath) {
		IFormatter formatter = new BinaryFormatter();
		Stream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);

		Knots = (BaseKnot[,]) formatter.Deserialize(stream);
		BuzyPos = (bool[,,]) formatter.Deserialize(stream);
		Elements = new ArrayList();
		for(int x = 0; x < KNOTS_X_NUM; x++)
			for(int y = 0; y < KNOTS_Y_NUM; y++) {
				for(int i3 = 0; i3 < Knots[x,y].numElems; i3++) {
					if(Elements.IndexOf(Knots[x,y].GetElement(i3)) == -1)
						Elements.Add(Knots[x,y].GetElement(i3));
				}
			}
		stream.Close();
		SavePath = FilePath;
		ElementsChanged(true, true);
		IsModifed = false;
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// ���������� ���������������� ��������� �����
	/// ���� ��� �� ������� ��������� ���������� true
	/// </summary>
	private bool SerialSimplify(ArrayList BrahcnesToSimp) {
		if(BrahcnesToSimp == null) return false;
		if(BrahcnesToSimp.Count == 0) return false;

		bool res = false;
	
		foreach(Branch i in BrahcnesToSimp) {
			res = MakeSerialRezisorSimplify(i);
			res = res || MakeSerialCurrentSrcSimplify(i);
			res = res || MakeSerialVoltageSrcSimplify(i);
			res = res || MakeSerialVoltageAndCurrentSimplify(i);
		}
		return res;
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// ������������ ��������� �����. 
	/// �������� ������ ����� ����������������� ���������.
	/// ���� ���-�� ������� ��������� ���������� true
	/// </summary>
	private bool ParallelSimplify(ArrayList BranchesToSimp) {
		if(BranchesToSimp == null) return false;
		if(BranchesToSimp.Count == 0) return false;
		if(SchemeIsContour()) return false;

		BaseKnot br1kn1, br1kn2, br2kn1, br2kn2;
		Branch ShortBr, LongBr;
		Rezistor Rez;
		float ShortBrRes, LongBrRes, ResultRes;
		int j;
		bool res = false;
		Element CurrEl;
		ArrayList ReplacesElems;

		foreach(Branch i in BranchesToSimp) {
			foreach(Branch i2 in BranchesToSimp) {
				br1kn1 = i.pKnots[0] as BaseKnot;
				br1kn2 = i.pKnots[i.pKnots.Count - 1] as BaseKnot;
				br2kn1 = i2.pKnots[0] as BaseKnot;
				br2kn2 = i2.pKnots[i2.pKnots.Count - 1] as BaseKnot;
				if(!((br1kn1.Equals(br2kn1) && br1kn2.Equals(br2kn2)) ||
					(br1kn1.Equals(br2kn2) && br1kn2.Equals(br2kn1))) ||
					i == i2) continue;
				if(i.Length > i2.Length) {
					LongBr = i;
					ShortBr = i2;
				}
				else {
					LongBr = i2;
					ShortBr = i;
				}
				Rez = (Rezistor) LongBr.FindElement(typeof(Rezistor));
				if(Rez == null) continue;
				LongBrRes = Rez.Resistance;
				Rez = (Rezistor) ShortBr.FindElement(typeof(Rezistor));
				if(Rez == null) continue;
				ShortBrRes = Rez.Resistance;
				if(Branches.Count <= 2) return false; // ���� �������� ����� 2 ����� �������� ������ ������
				if(LongBrRes == 0 || ShortBrRes == 0)
					ResultRes = 0;
				else
					ResultRes = (LongBrRes * ShortBrRes) 
						/ (LongBrRes + ShortBrRes);

				// ������� ���-�� ������������ ��������� �� ������� �����
				ReplacesElems = new ArrayList();
				for(j = 0; j < LongBr.Length; j++) {
					CurrEl = LongBr.GetElement(j);
					if((!(CurrEl is Connection)) && (!(CurrEl is Rezistor))) {
						ReplacesElems.Add(CurrEl);
					}
				}

				/* ������� ���������� ��������� ��� �������� �� �������� �����.
				 * ���� �� ��� ��� ����� - ��������� �� ������� */
				if(ReplacesElems.Count <= ShortBr.CountFreePlaces()) { // �� �������� ����� ������� �����
					for(j = 0; j < ReplacesElems.Count; j++) {
						ChangeElement(ShortBr.GetElement(ShortBr.FindFreePlace()),
									ReplacesElems[j] as Element, false, false);
					}
					CurrEl = ShortBr.FindElement(typeof(Rezistor));
					(CurrEl as Rezistor).Resistance = ResultRes;
					DeleteBranch(LongBr);
					return true;
				}
				else { // �� �������� ����� �� ������� �����. ��������� �� �������
					ReplacesElems.Clear();
					for(j = 0; j < ShortBr.Length; j++) {  // ������� ���-�� ������������ ��������� �� �������� �����
						CurrEl = ShortBr.GetElement(j);
						if(!(CurrEl is Connection) && !(CurrEl is Rezistor))
							ReplacesElems.Add(CurrEl);
					}
					if(ReplacesElems.Count > ShortBr.CountFreePlaces()) // �� ������� ����� ���� ��� ����� - ��������� ����� ������������ ���������� ������
						return false;
					for(j = 0; j < ReplacesElems.Count; j++) {
						ChangeElement(LongBr.GetElement(LongBr.FindFreePlace()),
							ReplacesElems[j] as Element, false, false);
					}
					CurrEl = LongBr.FindElement(typeof(Rezistor));
					(CurrEl as Rezistor).Resistance = ResultRes;
					DeleteBranch(ShortBr);
					return true;
				}
			}
		}
		picDrawBox.Invalidate();
#if(DEBUG)
		if(!Branch.ChekBranches(Branches))
			System.Windows.Forms.MessageBox.Show("����� ����������� �����������!", "ParallelSimplify->Branch.CreateBranches");
#endif
		return res;
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// �������� �� ����� �������� � ��������� ��������� �� NewEl.
	/// </summary>
	private void SimplifyElements(Branch Br, ArrayList ElemIndexes, Element NewEl) {
		Element CurrEl;
		int Pos;

		for(int i = 0; i < ElemIndexes.Count; i++) {
			CurrEl = Br.GetElement((int) ElemIndexes[i]);
			ChangeElement(CurrEl, new Connection(), false, false);
		}
		Pos = Br.FindFreePlace();
		if(Pos == -1) return;
		ChangeElement(Br.GetElement(Pos), NewEl, true, false);
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// ��������� ���������������� ��������� ���������� �� �������� �����.
	/// ���� ���-������ ������� ��������� ���������� true
	/// </summary>
	private bool MakeSerialRezisorSimplify(Branch Br) {
		if(Br == null) return false;

		Element CurrEl;
		ArrayList ElIndexes = new ArrayList();
		float Summ = 0f;
		
		ElIndexes = Br.FindElements(typeof(Rezistor));
		if(ElIndexes.Count < 2) return false;
		for(int i = 0; i < ElIndexes.Count; i++) { // c�������� ������������� ���� ����������
			CurrEl = Br.GetElement((int)ElIndexes[i]);
			Summ += (CurrEl as Rezistor).Resistance;
		}
		// ������ ����� �������� �� �����
		Rezistor NewRez = new Rezistor();
		NewRez.Resistance = Summ;
		NewRez.IsReversed = Br.Direction;
		SimplifyElements(Br, ElIndexes, NewRez);
		return true;
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// ��������� ���������������� ��������� ���������� ���� �� �������� �����. 
	/// ���� ���-������ ������� ��������� ���������� true
	/// </summary>
	private bool MakeSerialCurrentSrcSimplify(Branch Br) {
		if(Br == null) return false;

		CurrentSrc NewCurSrc;
		ArrayList ElIndexes = Br.FindElements(typeof(CurrentSrc));

		if(ElIndexes.Count < 2) return false;
		NewCurSrc = new CurrentSrc();
		NewCurSrc.Frequency = (Br.GetElement((int) ElIndexes[0]) as CurrentSrc).Frequency;
		NewCurSrc.Current = (Br.GetElement((int) ElIndexes[0]) as CurrentSrc).Current;
		NewCurSrc.IsReversed = Br.Direction;
		SimplifyElements(Br, ElIndexes, NewCurSrc);
		return true;
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// ��������� ���������������� ��������� ���������� ���������� �� �������� �����. 
	/// ���� ���-������ ������� ��������� ���������� true
	/// </summary>
	private bool MakeSerialVoltageSrcSimplify(Branch Br) {
		if(Br == null) return false;

		VoltageSrc CurrVS;
		VoltageSrc NewVolSrc;
		float SummVol = 0f, SummFreq = 0f;
		ArrayList ElIndexes = Br.FindElements(typeof(VoltageSrc));

		if(ElIndexes.Count < 2) return false;
		for(int i = 0; i < ElIndexes.Count; i++) {
			CurrVS = Br.GetElement((int) ElIndexes[i]) as VoltageSrc;
			if(CurrVS.IsReversed == Br.Direction) {
				SummVol += CurrVS.Voltage;
				SummFreq += CurrVS.Frequency;
			}
			else {
				SummVol -= CurrVS.Voltage;
				SummFreq -= CurrVS.Frequency;
			}
		}
		NewVolSrc = new VoltageSrc();
		NewVolSrc.Voltage = SummVol;
		NewVolSrc.Frequency = SummFreq;
		NewVolSrc.IsReversed = Br.Direction;
		SimplifyElements(Br, ElIndexes, NewVolSrc);
		return true;
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// ��������� ���������������� ��������� ���������� ����������  �
	/// ���������� ���� �� �������� �����. 
	/// ���� ���-������ ������� ��������� ���������� true
	/// </summary>
	private bool MakeSerialVoltageAndCurrentSimplify(Branch Br) {
		if(Br == null) return false;

		CurrentSrc CurSrc = Br.FindElement(typeof(CurrentSrc)) as CurrentSrc;
		VoltageSrc VolSrc = Br.FindElement(typeof(VoltageSrc)) as VoltageSrc;
		CurrentSrc NewCurSrc;
		ArrayList ElIndexes;

		if(CurSrc == null || VolSrc == null) return false;
		NewCurSrc = new CurrentSrc();
		NewCurSrc.Current = CurSrc.Current;
		NewCurSrc.Frequency = CurSrc.Frequency;
		NewCurSrc.IsReversed = CurSrc.IsReversed;
		ElIndexes = new ArrayList();
		ElIndexes.Add(Br.IndexOf(CurSrc));
		ElIndexes.Add(Br.IndexOf(VolSrc));
		SimplifyElements(Br, ElIndexes, NewCurSrc);
		return true;
	}

	// ------------------------------------------------------------------------
	/// <summary>����������, �������� �� ����� ����� ��������� ��������?</summary>
	private bool SchemeIsContour() {
		if(Branches.Count != 0) return false;
		if(BaseKnot.CountKnots(Knots) != 0) return false;
		
		for(int x = 0; x < Knots.GetLength(0); x++) {
			for(int y = 0; y < Knots.GetLength(1); y++) {
				if(Knots[x,y].numElems == 2) {
					BaseKnot	StartKnot = Knots[x,y],
								CurrKnot = Knots[x,y];
					Element CurrEl = StartKnot.GetElement(0);
					
					do { // ���� �� �������
						CurrKnot = CurrEl.pKnot1 != CurrKnot ? CurrEl.pKnot1 : CurrEl.pKnot2;
						if(CurrKnot.numElems != 2) return false;
						if(CurrKnot.Equals(StartKnot)) return true; // ���� �� ������ � ������, ������ ��� ������
						CurrEl = CurrKnot.GetElement(0) != CurrEl 
							? CurrKnot.GetElement(0) : CurrKnot.GetElement(1);
					} while(true);
				}
			}
		}
		return false;
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// ����� ����� ����� ��� �������, �� ��� ��� ������, �.� ��� �����.
	/// �-�� ������ ���������� ���� � ������ ������� (����� ������� ��� �����), 
	/// ����� ��� ����� ���� �������� � ������� �������
	/// </summary>
	public void MakeBranchFromContour() {
		if(BaseKnot.CountKnots(Knots) != 0) return;

		for(int x = 0; x < KNOTS_X_NUM; x++) {
			for(int y = 0; y < KNOTS_Y_NUM; y++) {
				if(Knots[x,y].numElems == 2) {
					CreateRemovableKnot(Knots[x,y]);
					return;
				}
			}
		}
	}
}
}