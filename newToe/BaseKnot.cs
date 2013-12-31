using System;
using System.Drawing;
using System.Drawing.Text;
using System.Collections;

namespace newToe {
/// <summary>
/// Базовый узел. На схеме представлен в виде маленького квадратика.
/// </summary>
[Serializable] public class BaseKnot {
	
	/// <summary>Отрисовывать ли нумера узлов</summary>
	public static bool DrawKnotNumbers = true;

	/// <summary>Отрисовываются ли базовые узлы (действует только на базовые!)</summary>
	public static bool DrawBaseKnots = true;

	private const int RECT_WIDTH = 4, RECT_HEIGHT = 4;
	/// <summary>Размер квадратика</summary>
	private static Size RectSize;

	/// <summary>Карандаш, которым рисуются все квадратики</summary>
	private static Pen RectPen;

	/// <summary>Кисть которой заливаются базовые узлы</summary>
	private static SolidBrush KnotBrush;

	/// <summary>Кисть которой закрашиваются устранимые узлы</summary>
	private static SolidBrush RemovableBrush;

	/// <summary>
	/// Указатели на элементы, которые находятся на узле.
	/// Не может быть больше чем 4, т.к. это максим. возможное 
	/// кол-во элементов на узле.
	/// </summary>
	private Element[] pElems = new Element[4];

	/// <summary>Координаты центра квадратика на схеме</summary>
	public Point Coord;

	/// <summary>Цвет квардратика</summary>
	public Color Col;

	/// <summary>Отрисовывается квадратик или нет</summary>
	public bool Visible = true;

	/// <summary>Номер узла. Если базовый узел не является узлом == -1</summary>
	public int KnotNo = -1;

	/// <summary>
	/// Является ли узел устранимым. 
	/// Базоый узел не может являться и обычным и устранимым узлом одновременно
	/// </summary>
	public bool IsRemovableKnot = false;

	// ------------------------------------------------------------------------

	private int _numElems = -1;
	/// <summary>Кол-во элементов на узле</summary>
	public int numElems {
		get {
			return _numElems + 1;
		}
	}

	/// <summary>
	/// Является ли базовый узел (квадратик) узлом.
	/// Узлом называется базовый узел, к которому "прикреплены" как минимум 3 элемента
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
	/// Возвращает элемент который содержиться между двумя соседними базовыми узлами.
	/// Если между ними нет элемента или они не соседние возвращается null
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
	/// <summary>Пронумеровывает узлы на схеме построчно, начиная сверху</summary>
	public static void NumerateKnots(BaseKnot[,] Knots) {
		int CurrNo = 1; // нумерация начинается с 1

		for(int y = 0; y < Knots.GetLength(1); y++)
			for(int x = 0; x < Knots.GetLength(0); x++)
				if(Knots[x, y].IsKnot || Knots[x, y].IsRemovableKnot) 
					Knots[x, y].KnotNo = CurrNo++;
	}
	
	// -----------------------------------------------------------------------------
	/// <summary>Возвращает кол-во узлов (не базовых) на схеме</summary>
	public static int CountKnots(BaseKnot[,] Knots) {
		int res = 0;

		for(int y = 0; y < Knots.GetLength(1); y++)
			for(int x = 0; x < Knots.GetLength(0); x++)
				if(Knots[x,y].IsKnot || Knots[x, y].IsRemovableKnot) res++;
		return res;
	}
	
	// ----------------------------------------------------------------------
	/// <summary>
	/// Расчитывает, в какой части прямоугольника rect
	/// (нижний, верхний, левой или правой)
	/// находится точка p
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
	/// <summary>Рисует </summary>
	public void Draw(Graphics gr) {
		if(!Visible) return;

		if(IsKnot || IsRemovableKnot) {
			const int POINT_INC = 6;

			// рисуем узел
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
				// рисуем номер ветви
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
	/// <summary>Прикрепить новый элемент</summary>
	public void AddElement(ref Element Elem) {
		if(_numElems >= 3) return;

		_numElems++;
		pElems[_numElems] = Elem;
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// Удалить указатель элемент по его индексу. Вызывается, когда элемент удаляется со схемы
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
	/// Удалить указатель элемент. Вызывается, когда элемент удаляется со схемы
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
	/// Очищает базовый узел, от всех прикрепленных к нему элементов
	/// </summary>
	public void ClearElements() {
		pElems = new Element[4];
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// Возвращает элемент с заданным индексом. 
	/// Т.к. максимальное кол-во элементов на узле = 4, то индекс может 
	/// иметь значения только в пределах [0;3]
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