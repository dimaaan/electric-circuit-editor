using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections;

namespace newToe {
/// <summary>
/// Абстрактный базовый класс, представляющий элемент цепи.
/// </summary>
[Serializable] public abstract class Element {

	/// <summary>Надо ли отрисовывать номера элементов</summary>
	public static bool DrawNumbers = true;

	public static readonly Color NumerCol = Color.FromArgb(100, 45, 182);

	// -------------------------------------------------------------------------

	/// <summary>
	/// Указатель на базовые узлы. 
	/// Если элемент расположен горизонтально, pKnot1 - левый узел, pKont2 - правый.
	/// Если вертикально: pKnot1 - верхний, pKnot2 - нижний узел
	/// </summary>
	public BaseKnot pKnot1, pKnot2;

	/// <summary>
	/// Истина, если элемент расположен вертивально, ложь если горизонтально
	/// </summary>
	public bool IsVertical;

	/// <summary>
	/// Если истина элемент рисуется зеркально отраженным
	/// </summary>
	public bool IsReversed = false;

	/// <summary>Номер элемента</summary>
	public int Number = 0;

	public readonly Color COLOR_KEY = Color.FromArgb(255,255,255);

	// -------------------------------------------------------------------------

	/// <summary>Отрисовка элемента цепи</summary>
	public abstract void Draw(Graphics gr);

	/// <summary>Возращает изображение элемента цепи</summary>
	public abstract void GetBitmap(out Bitmap bmp);

	/// <summary>
	/// Показывает диалог с параметрами элемента, чтоб юзер ввел/изменил их
	/// </summary>
	public abstract void ShowParamDialog();

	// -------------------------------------------------------------------------
	/// <summary>Пронумеровывает элементы</summary>
	public static void NumerateElements(ArrayList Elements) {
		int RezCount = 1, CurrSrcCount = 1, VolCount = 1;

		for(int i = 0; i < Elements.Count; i++) {
			if(Elements[i].GetType() == typeof(Rezistor))
				(Elements[i] as Element).Number = RezCount++;
			else if(Elements[i].GetType() == typeof(CurrentSrc))
				(Elements[i] as Element).Number = CurrSrcCount++;
			if(Elements[i].GetType() == typeof(VoltageSrc))
				(Elements[i] as Element).Number = VolCount++;
		}
	}

	// -------------------------------------------------------------------------
	/// <summary>
	/// Возвращает точки, которые описывают, как рисовать элемент
	/// </summary>
	protected virtual Point[] GetDestPts(int HalfImageWidth) {
		Point[] DestPts = new Point[3];

		if(IsVertical) {
			if(IsReversed) {
				DestPts[1].X = pKnot1.Coord.X - HalfImageWidth / 2;
				DestPts[1].Y = pKnot1.Coord.Y;
				DestPts[2].X = pKnot2.Coord.X + HalfImageWidth / 2;
				DestPts[2].Y = pKnot2.Coord.Y;
				DestPts[0].X = pKnot2.Coord.X - HalfImageWidth / 2;
				DestPts[0].Y = pKnot2.Coord.Y;
			}
			else {
				DestPts[0].X = pKnot1.Coord.X - HalfImageWidth / 2;
				DestPts[0].Y = pKnot1.Coord.Y;
				DestPts[1].X = DestPts[0].X;
				DestPts[1].Y = pKnot2.Coord.Y;
				DestPts[2].X = pKnot1.Coord.X + HalfImageWidth / 2;
				DestPts[2].Y = pKnot1.Coord.Y;
			}
		}
		else {
			if(IsReversed) {
				DestPts[1].X = pKnot1.Coord.X;
				DestPts[1].Y = pKnot1.Coord.Y - HalfImageWidth / 2;
				DestPts[0].X = pKnot2.Coord.X;
				DestPts[0].Y = pKnot2.Coord.Y - HalfImageWidth / 2;
				DestPts[2].X = pKnot2.Coord.X;
				DestPts[2].Y = pKnot2.Coord.Y + HalfImageWidth / 2;
			}
			else {
				DestPts[0].X = pKnot1.Coord.X;
				DestPts[0].Y = pKnot1.Coord.Y - HalfImageWidth / 2;
				DestPts[1].X = pKnot2.Coord.X;
				DestPts[1].Y = DestPts[0].Y;
				DestPts[2].X = pKnot1.Coord.X;
				DestPts[2].Y = pKnot1.Coord.Y + HalfImageWidth / 2;
			}
		}
		return DestPts;
	}

	// -------------------------------------------------------------------------
	/// <summary>Отрисовывает текст рядом с элементом цепи</summary>
	public virtual void DrawElemNo(string Text, Graphics gr, Size ImgSize) {
		FontFamily  fontFamily = new FontFamily("Times New Roman");
		Font        font = new Font(fontFamily, 12, FontStyle.Regular, 
			GraphicsUnit.Pixel);
		SolidBrush  solidBrush = new SolidBrush(NumerCol);

		if(IsVertical) {
			Point		NoCoord = new Point();

			NoCoord.X = pKnot1.Coord.X + ImgSize.Height / 2;
			NoCoord.Y = pKnot1.Coord.Y + ImgSize.Height / 2;
			gr.DrawString(Text, font, solidBrush, NoCoord);
		}
		else {
			Rectangle rect = new Rectangle();
			StringFormat stringFormat = new StringFormat();
			
			rect.X = pKnot1.Coord.X;
			rect.Y = pKnot1.Coord.Y - ImgSize.Height;
			rect.Width = ImgSize.Width;
			rect.Height = pKnot1.Coord.Y - ImgSize.Height / 2;
			stringFormat.Alignment = StringAlignment.Center;
			gr.DrawString(Text, font, solidBrush, rect, stringFormat);
		}
	}
}


// ===============================================================================
// -------------------------------------------------------------------------------
/// <summary>Элемент цепи - соединение</summary>
[Serializable] public class Connection: Element {
	private const string ImagePath = "Images\\Conn.gif";
	private static Bitmap ElemImage;

	static Connection() {
		ElemImage = new Bitmap(ImagePath);
	}

	public override void Draw(Graphics gr) {
		ImageAttributes atr = new ImageAttributes();

		atr.SetColorKey(COLOR_KEY, COLOR_KEY);
		gr.DrawImage(ElemImage, GetDestPts(ElemImage.Width / 2), 
			new Rectangle(0,0, ElemImage.Width, ElemImage.Height), 
			GraphicsUnit.Pixel, atr);
	}

	public override void GetBitmap(out Bitmap bmp) {
		bmp = ElemImage;
	}

	public override void ShowParamDialog() {
		// т.к. у соединения нет параметров ф-ия пуста
	}


}


// ===============================================================================
// -------------------------------------------------------------------------------
/// <summary>Элемент цепи - резистор</summary>
[Serializable] public class Rezistor: Element {
	private const string ImagePath = "Images\\Rez.gif";
	private static Bitmap ElemImage;

	/// <summary>Сопротивление резистора</summary>
	public float Resistance = 0f;

	static Rezistor() {
		ElemImage = new Bitmap(ImagePath);
	}

	public override void Draw(Graphics gr) {
		ImageAttributes atr = new ImageAttributes();

		atr.SetColorKey(COLOR_KEY, COLOR_KEY);
		gr.DrawImage(ElemImage, GetDestPts(ElemImage.Width / 2), 
			new Rectangle(0,0, ElemImage.Width, ElemImage.Height), 
			GraphicsUnit.Pixel, atr);
		if(DrawNumbers) 
			DrawElemNo(String.Format("R{0}", Number), gr, ElemImage.Size);
	}

	public override void GetBitmap(out Bitmap bmp) {
		bmp = ElemImage;
	}

	public override void ShowParamDialog() {
		frmElemParams dlg = new frmElemParams();
		string strResist = "Сопротивление";

		dlg.ElemParams.Add(strResist, Resistance.ToString());
		dlg.pImage = ElemImage;
		dlg.chkReverse.Checked = IsReversed;
		dlg.ElemIsVertical = IsVertical;
		if(dlg.ShowForm() == DialogResult.OK) {
			Resistance = Convert.ToSingle(dlg.ElemParams[strResist]);
		}
		IsReversed = dlg.chkReverse.Checked;
	}

}


// ===============================================================================
// -------------------------------------------------------------------------------
/// <summary>Элемент цепи - источник напряжения</summary>
[Serializable] public class VoltageSrc: Element {
	private const string ImagePath = "Images\\VolSrc.gif";
	private static Bitmap ElemImage;

	/// <summary>Частота</summary>
	public float Frequency = 0;

	/// <summary>Напряжение</summary>
	public float Voltage = 0;

	static VoltageSrc() {
		ElemImage = new Bitmap(ImagePath);
	}

	public override void Draw(Graphics gr) {
		ImageAttributes atr = new ImageAttributes();

		atr.SetColorKey(COLOR_KEY, COLOR_KEY);
		gr.DrawImage(ElemImage, GetDestPts(ElemImage.Width / 2), 
			new Rectangle(0,0, ElemImage.Width, ElemImage.Height), 
			GraphicsUnit.Pixel, atr);
		if(DrawNumbers) 
			DrawElemNo(String.Format("J{0}", Number), gr, ElemImage.Size);
	}

	public override void GetBitmap(out Bitmap bmp) {
		bmp = ElemImage;
	}

	public override void ShowParamDialog() {
		frmElemParams dlg = new frmElemParams();
		string strFreq = "Частота";
		string strVol = "Напряжение";

		dlg.ElemParams.Add(strFreq, Frequency.ToString());
		dlg.ElemParams.Add(strVol, Voltage.ToString());
		dlg.pImage = ElemImage;
		dlg.chkReverse.Checked = IsReversed;
		dlg.ElemIsVertical = IsVertical;
		if(dlg.ShowForm() == DialogResult.OK) {
			Frequency = Convert.ToSingle(dlg.ElemParams[strFreq]);
			Voltage = Convert.ToSingle(dlg.ElemParams[strVol]);
		}
		IsReversed = dlg.chkReverse.Checked;
	}
}

// ===============================================================================
// -------------------------------------------------------------------------------
/// <summary>Элемент цепи - источник тока</summary>
[Serializable] public class CurrentSrc: Element {
	private const string ImagePath = "Images\\CurrSrc.gif";
	private static Bitmap ElemImage;

	/// <summary>Частота</summary>
	public float Frequency = 0;

	/// <summary>Ток</summary>
	public float Current = 0;

	static CurrentSrc() {
		ElemImage = new Bitmap(ImagePath);
	}

	public override void Draw(Graphics gr) {
		ImageAttributes atr = new ImageAttributes();

		atr.SetColorKey(COLOR_KEY, COLOR_KEY);
		gr.DrawImage(ElemImage, GetDestPts(ElemImage.Width / 2), 
			new Rectangle(0,0, ElemImage.Width, ElemImage.Height), 
			GraphicsUnit.Pixel, atr);
		if(DrawNumbers) 
			DrawElemNo(String.Format("E{0}", Number), gr, ElemImage.Size);
	}

	public override void GetBitmap(out Bitmap bmp) {
		bmp = ElemImage;
	}

	public override void ShowParamDialog() {
		frmElemParams dlg = new frmElemParams();
		string strFreq = "Частота";
		string strCur = "Ток";

		dlg.ElemParams.Add(strFreq, Frequency.ToString());
		dlg.ElemParams.Add(strCur, Current.ToString());
		dlg.pImage = ElemImage;
		dlg.chkReverse.Checked = IsReversed;
		dlg.ElemIsVertical = IsVertical;
		if(dlg.ShowForm() == DialogResult.OK) {
			Frequency = Convert.ToSingle(dlg.ElemParams[strFreq]);
			Current = Convert.ToSingle(dlg.ElemParams[strCur]);
		}
		IsReversed = dlg.chkReverse.Checked;
	}

}
}