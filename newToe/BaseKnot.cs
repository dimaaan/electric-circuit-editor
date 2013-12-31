using System;
using System.Drawing;
using System.Drawing.Text;
using System.Collections;

namespace newToe {
/// <summary>
/// ������� ����. �� ����� ����������� � ���� ���������� ����������.
/// </summary>
[Serializable] public class BaseKnot {
	
	/// <summary>������������ �� ������ �����</summary>
	public static bool DrawKnotNumbers = true;

	/// <summary>�������������� �� ������� ���� (��������� ������ �� �������!)</summary>
	public static bool DrawBaseKnots = true;

	private const int RECT_WIDTH = 4, RECT_HEIGHT = 4;
	/// <summary>������ ����������</summary>
	private static Size RectSize;

	/// <summary>��������, ������� �������� ��� ����������</summary>
	private static Pen RectPen;

	/// <summary>����� ������� ���������� ������� ����</summary>
	private static SolidBrush KnotBrush;

	/// <summary>����� ������� ������������� ���������� ����</summary>
	private static SolidBrush RemovableBrush;

	/// <summary>
	/// ��������� �� ��������, ������� ��������� �� ����.
	/// �� ����� ���� ������ ��� 4, �.�. ��� ������. ��������� 
	/// ���-�� ��������� �� ����.
	/// </summary>
	private Element[] pElems = new Element[4];

	/// <summary>���������� ������ ���������� �� �����</summary>
	public Point Coord;

	/// <summary>���� �����������</summary>
	public Color Col;

	/// <summary>�������������� ��������� ��� ���</summary>
	public bool Visible = true;

	/// <summary>����� ����. ���� ������� ���� �� �������� ����� == -1</summary>
	public int KnotNo = -1;

	/// <summary>
	/// �������� �� ���� ����������. 
	/// ������ ���� �� ����� �������� � ������� � ���������� ����� ������������
	/// </summary>
	public bool IsRemovableKnot = false;

	// ------------------------------------------------------------------------

	private int _numElems = -1;
	/// <summary>���-�� ��������� �� ����</summary>
	public int numElems {
		get {
			return _numElems + 1;
		}
	}

	/// <summary>
	/// �������� �� ������� ���� (���������) �����.
	/// ����� ���������� ������� ����, � �������� "�����������" ��� ������� 3 ��������
	/// </summary>
	public bool IsKnot {
		get{
			return _numElems >= 2 ? true : false;
		}
	}

	// ------------------------------------------------------------------------
	static BaseKnot() {
		RectSize = new Size(RECT_WIDTH , RECT_HEIGHT);
		RectPen  = new Pen(Color.FromArgb(0,0,0), 1);
		KnotBrush = new SolidBrush(Color.FromArgb(0,0,0));
		RemovableBrush = new SolidBrush(Color.FromArgb(128,128,128));
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// ���������� ������� ������� ����������� ����� ����� ��������� �������� ������.
	/// ���� ����� ���� ��� �������� ��� ��� �� �������� ������������ null
	/// </summary>
	public static Element GetElementBetweenKnots(BaseKnot pKnot1, BaseKnot pKnot2) {
		if(object.ReferenceEquals(pKnot1, pKnot2)) return null;

		for(int i = 0; i < pKnot1.numElems; i++) {
			for(int i2 = 0; i2 < pKnot2.numElems; i2++) {
				if(pKnot1.GetElement(i) == pKnot2.GetElement(i2))
					return pKnot1.GetElement(i);
			}
		}
		return null;
	}

	// -----------------------------------------------------------------------------
	/// <summary>��������������� ���� �� ����� ���������, ������� ������</summary>
	public static void NumerateKnots(BaseKnot[,] Knots) {
		int CurrNo = 1; // ��������� ���������� � 1

		for(int y = 0; y < Knots.GetLength(1); y++)
			for(int x = 0; x < Knots.GetLength(0); x++)
				if(Knots[x, y].IsKnot || Knots[x, y].IsRemovableKnot) 
					Knots[x, y].KnotNo = CurrNo++;
	}
	
	// -----------------------------------------------------------------------------
	/// <summary>���������� ���-�� ����� (�� �������) �� �����</summary>
	public static int CountKnots(BaseKnot[,] Knots) {
		int res = 0;

		for(int y = 0; y < Knots.GetLength(1); y++)
			for(int x = 0; x < Knots.GetLength(0); x++)
				if(Knots[x,y].IsKnot || Knots[x, y].IsRemovableKnot) res++;
		return res;
	}
	
	// ----------------------------------------------------------------------
	/// <summary>
	/// �����������, � ����� ����� �������������� rect
	/// (������, �������, ����� ��� ������)
	/// ��������� ����� p
	/// </summary>
	//            rect
	// |------------------------|
	// |  l  |  upper      | r  |
	// |  e  |             | i  |
	// |  f  |-------------| g  |
	// |  t  |  lower      | h  |
	// |     |             | t  |
	// |------------------------|
	public static frmMain.Side GetRectSide(Rectangle rect, Point p) {
		int a, b, c, WDiv4;
		
		if(!rect.Contains(p)) return (frmMain.Side) 0;
		WDiv4 = rect.Width / 4;
		a = rect.X + WDiv4;
		b = rect.X + 3 * WDiv4;
		c = rect.Y + rect.Height / 2;
		if(p.X < a) return frmMain.Side.S_LEFT;
		else if(p.X > b) return frmMain.Side.S_RIGHT;
		else if(p.Y < c) return frmMain.Side.S_UPPER;
		else return frmMain.Side.S_LOWER;
	}
	
	// ------------------------------------------------------------------------
	public BaseKnot(Point p) {
		RectSize = new Size(RECT_WIDTH, RECT_HEIGHT);
		RectPen  = new Pen(Color.FromArgb(0,0,0), 1);
		Coord = p;
	}

	// ------------------------------------------------------------------------
	/// <summary>������ </summary>
	public void Draw(Graphics gr) {
		if(!Visible) return;

		if(IsKnot || IsRemovableKnot) {
			const int POINT_INC = 6;

			// ������ ����
			if(IsRemovableKnot) {
				gr.FillEllipse(RemovableBrush, Coord.X - RectSize.Width / 2 - POINT_INC / 2,
					Coord.Y - RectSize.Height / 2 - POINT_INC / 2, 
					RectSize.Width + POINT_INC, RectSize.Height + POINT_INC);
			}
			else {
				gr.FillEllipse(KnotBrush, Coord.X - RectSize.Width / 2 - POINT_INC / 2,
					Coord.Y - RectSize.Height / 2 - POINT_INC / 2, 
					RectSize.Width + POINT_INC, RectSize.Height + POINT_INC);
			}

			if(DrawKnotNumbers) {
				// ������ ����� �����
				Point		NoCoord = new Point();
				FontFamily  fontFamily = new FontFamily("Arial");
				Font        font = new Font(fontFamily, 12, FontStyle.Regular, 
					GraphicsUnit.Pixel);
				SolidBrush  solidBrush = new SolidBrush(Color.FromArgb(41, 45, 182));

				NoCoord.X = Coord.X - RectSize.Width / 2 + RectSize.Width + 3;
				NoCoord.Y = Coord.Y - RectSize.Height / 2 + RectSize.Height + 3;
				gr.DrawString(KnotNo.ToString(), font, solidBrush, NoCoord);
			}
		}
		else {
			if(DrawBaseKnots)
				if(_numElems == -1)
					gr.DrawRectangle(RectPen, Coord.X - RectSize.Width / 2,
						Coord.Y - RectSize.Height / 2, RectSize.Width, RectSize.Height);
		}
	}

	// ------------------------------------------------------------------------
	/// <summary>���������� ����� �������</summary>
	public void AddElement(ref Element Elem) {
		if(_numElems >= 3) return;

		_numElems++;
		pElems[_numElems] = Elem;
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// ������� ��������� ������� �� ��� �������. ����������, ����� ������� ��������� �� �����
	/// </summary>
	public void DeleteElement(int ElemIndex) {
		if(pElems[ElemIndex] == null) return;

		int i;

		pElems[ElemIndex] = null;
		for(i = ElemIndex; i < 3; i++) {
			pElems[i] = pElems[i + 1];
		}
		pElems[i] = null;
		_numElems--;
	}

	/// <summary>
	/// ������� ��������� �������. ����������, ����� ������� ��������� �� �����
	/// </summary>
	public void DeleteElement(Element pElem) {
		for(int i = 0; i < numElems; i++) {
			if(GetElement(i) == pElem) {
				DeleteElement(i);
				return;
			}
		}
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// ������� ������� ����, �� ���� ������������� � ���� ���������
	/// </summary>
	public void ClearElements() {
		pElems = new Element[4];
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// ���������� ������� � �������� ��������. 
	/// �.�. ������������ ���-�� ��������� �� ���� = 4, �� ������ ����� 
	/// ����� �������� ������ � �������� [0;3]
	/// </summary>
	public Element GetElement(int ElemIndex) {
		if(0 <= ElemIndex && ElemIndex <= _numElems) {
			return pElems[ElemIndex];
		}
		else return null;
	}

	// ------------------------------------------------------------------------
	public override string ToString() {
		return String.Format("{0} {1};{2}", IsKnot ? "Knot" : "NotKnot", 
			(Coord.X - frmMain.START_SHIFT) / frmMain.KNOTS_DIST,
			(Coord.Y - frmMain.START_SHIFT) / frmMain.KNOTS_DIST);
	}

}
}