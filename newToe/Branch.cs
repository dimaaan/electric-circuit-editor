using System;
using System.Collections;
using System.Drawing;

namespace newToe {
/// <summary>Класс для разбивки схемы на ветки</summary>
public class Branch {

	/// <summary>
	/// Указатели на базовые узлы, составляющие ветку.
	/// Первый и последний из них являются узлами.
	/// </summary>
	public ArrayList pKnots = new ArrayList();

	/// <summary>Номер ветви</summary>
	public int Number = -1;

	/// <summary>
	/// Направление тока в цепи. Если  = true значит направление c начала в конец,
	/// false - наоборот.
	/// </summary>
	public bool Direction = true;

	// ------------------------------------------------------------------------
	public void DrawNumbers(Graphics gr) {
		if(Length == 0) return;

		for(int i = 0; i < Length; i++)
			if(GetElement(i) == null) return;

		Element		MiddleEl = GetElement(Length / 2);
		Size		s = new Size(10,10);
		FontFamily  fontFamily = new FontFamily("Times New Roman");
		Font        font = new Font(fontFamily, 12, FontStyle.Bold, 
			GraphicsUnit.Pixel);
		SolidBrush  solidBrush = new SolidBrush(Color.FromArgb(255,0,0));
		StringFormat stringFormat = new StringFormat();

		stringFormat.Alignment = StringAlignment.Center;
		stringFormat.LineAlignment = StringAlignment.Center;
		if(Convert.ToDouble(Length / 2) == Length / 2.0f) {
			Point p = MiddleEl.pKnot1.Coord;

			if(MiddleEl.IsVertical) p.Y += frmMain.KNOTS_DIST / 2;
			gr.DrawString(Number.ToString(), font, solidBrush, p, stringFormat);
		}
		else {
			Rectangle	rect = new Rectangle();

			if(MiddleEl.IsVertical) {
				rect.X = MiddleEl.pKnot1.Coord.X - s.Width;
				rect.Y = MiddleEl.pKnot1.Coord.Y;
				rect.Width = 2 * s.Width;
				rect.Height = frmMain.KNOTS_DIST;
			}
			else {
				rect.X = MiddleEl.pKnot1.Coord.X;
				rect.Y = MiddleEl.pKnot1.Coord.Y - s.Height;
				rect.Width = frmMain.KNOTS_DIST;
				rect.Height = 2 * s.Height;
			}
			gr.DrawString(Number.ToString(), font, solidBrush, rect, stringFormat);
		}
	}

	// ------------------------------------------------------------------------
	/// <summary>Длинна ветки, т.е. сколько элементов цепи находится в ней</summary>
	public int Length {
		get {
			return (pKnots.Count < 2) ? 0 : pKnots.Count - 1;
		}
	}

	// ------------------------------------------------------------------------
	/// <summary>Возвращает i-ный от начала элемент цепи (нумерация с 0).</summary>
	public Element GetElement(int i) {
		if(0 > pKnots.Count - 1 || pKnots.Count - 1 < i) return null;
		return BaseKnot.GetElementBetweenKnots(
			pKnots[i] as BaseKnot, pKnots[i+1] as BaseKnot);
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// Ищет (с начала) первый элемент заданного типа в цепи.
	/// Если элементов такого типа в цепи нет возвращает null
	/// </summary>
	public Element FindElement(Type ElemType) {
		Element CurrEl;

		for(int i = 0; i < this.Length; i++) {
			CurrEl = GetElement(i);
			if(CurrEl == null) return null;
			if(CurrEl.GetType() == ElemType) return CurrEl;
		}
		return null;
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// Возвращает индексы (типа int) всех элементов заданного типа на ветке.
	/// Если тип не является элементом возвр. null
	/// </summary>
	public ArrayList FindElements(Type ElemType) {
		if(!ElemType.IsSubclassOf(typeof(Element))) return null;

		ArrayList res = new ArrayList();
		Element El;

		for(int i = 0; i < Length; i++) {
			El = GetElement(i);
			if(El.GetType() == ElemType) res.Add(i);
		}
		return res;
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// Возвращает индекс заданного элемента на ветке.
	/// Если на ветке его нет возвр. -1
	/// </summary>
	public int IndexOf(Element El) {
		for(int i = 0; i < Length; i++) {
			if(El.Equals(GetElement(i))) return i;
		}
		return -1;
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// Находит свободное место на ветке. 
	/// Свободным местом считается любой элемент типа соединение.
	/// Если мест несколько выбирается ближнее к середине
	/// Если места не возвращает -1
	/// </summary>
	public int FindFreePlace() {
		int Middle = Middle = Length / 2;
		int Pos = 0;
		Element CurrEl;

		if(GetElement(Middle) is Connection) return Middle;
		while(Pos <= Middle) {
			Pos++;
			CurrEl = GetElement(Middle + Pos);
			if(CurrEl is Connection) {
				return Middle + Pos;
			}
			CurrEl = GetElement(Middle - Pos);
			if(CurrEl is Connection) {
				return Middle - Pos;
			}
		}
		return -1;
	}

	// ------------------------------------------------------------------------
	/// <summary>
	/// Возвращает кол-во свободных мест на ветке.
	/// </summary>
	public int CountFreePlaces() {
		int Count = 0;

		for(int i = 0; i < Length; i++)
			if(!(GetElement(i) is Connection)) Count++;
		return Count;
	}

	// ------------------------------------------------------------------------
	public override string ToString() {
		return Number.ToString();
	}

	// ------------------------------------------------------------------------
	// -------------------- Статические методы --------------------------------
	// ------------------------------------------------------------------------

	// ------------------------------------------------------------------------
	/// <summary>Производит разбивку схемы на ветки. 
	/// Если схема не замкнута возвращает false</summary>
	/// <param name="Branches">Возвращаемый массив веток</param>
	public static bool CreateBranches(BaseKnot[,] Schema, out ArrayList Branches) {
		BaseKnot CurrKnot, StartKnot;
		Element CurrEl;
		Branch CurrBranch = new Branch();
		int tmp, tmp2 = -1, i, j;

		Branches = new ArrayList();
		for(int x = 0; x < Schema.GetLength(0); x++) {
			for(int y = 0; y < Schema.GetLength(1); y++) {
				if(!Schema[x,y].IsKnot && !Schema[x,y].IsRemovableKnot) continue;
				StartKnot = CurrKnot = Schema[x,y];
				for(i = 0; i < StartKnot.numElems; i++) {
					CurrBranch.pKnots.Add(StartKnot);
					CurrKnot = Schema[x,y];
					CurrEl = StartKnot.GetElement(i);
					do { // идем по ветке
						if(CurrEl == null) return false;
						CurrKnot = (CurrEl.pKnot1 != CurrKnot) ? CurrEl.pKnot1 : CurrEl.pKnot2;
						if(CurrKnot.IsKnot || CurrKnot.IsRemovableKnot) 
						{ // если баз. узел - узел. - конец ветки
							CurrBranch.pKnots.Add(CurrKnot);

							// часный случай
							if(CurrBranch.pKnots.Count == 2) {
								for( j = 0; j < Branches.Count; j++) {
									tmp  = (Branches[j] as Branch).pKnots.IndexOf(CurrBranch.pKnots[0]);
									tmp2 = (Branches[j] as Branch).pKnots.IndexOf(CurrBranch.pKnots[1]);
									if(tmp != -1 && tmp2 != -1 && ((Branch)Branches[j]).pKnots.Count == 2) 
										goto NextBranch;
								}
							}

							Branches.Add(CurrBranch);
NextBranch:
							CurrBranch = new Branch();
							break;
						}
						CurrEl = (CurrKnot.GetElement(0) != CurrEl) ? 
							CurrKnot.GetElement(0) : CurrKnot.GetElement(1);
						// проверяем не повторяется ли ветка
						for(j = 0; j < Branches.Count; j++) {
							tmp = (Branches[j] as Branch).pKnots.IndexOf(CurrKnot);
							if(tmp != -1 && tmp != 0 && 
								tmp != (Branches[j] as Branch).pKnots.Count - 1) {
								CurrBranch.pKnots.Clear();
								goto NextBranch2;
							}
						}
						CurrBranch.pKnots.Add(CurrKnot);
					} while(true);
NextBranch2:;
				}
			}
		}
#if(DEBUG)
		if(!ChekBranches(Branches)) 
			System.Windows.Forms.MessageBox.Show("Ветви построились неправильно!", "Branch.CreateBranches");
#endif
		NumerateBranches(Branches);
		return true;
	}

	// ------------------------------------------------------------------------
	/// <summary>Пронумеровывает ветви начиная с 1</summary>
	public static void NumerateBranches(ArrayList Branches) {
		for(int i = 0; i < Branches.Count; i++)
			(Branches[i] as Branch).Number = i + 1;
	}

	// ------------------------------------------------------------------------
	/// <summary>Возвращает ветвь с заданным номером (не путать с индексом!)</summary>
	public static Branch GetBranchFromNo(ArrayList Branches,int BranchNo) {
		Branch b;

		for(int i = 0; i < Branches.Count; i++) {
			b = Branches[i] as Branch;
			if(b.Number == BranchNo) return b;
		}
		return null;
	}
	// ------------------------------------------------------------------------
	/// <summary>Упрощает однородные элементы заданного типа на ветви.</summary>
	public static void SimplifyElement(Type ElemType) {
		
	}
	// ------------------------------------------------------------------------
#if(DEBUG)
	public static bool ChekBranches(ArrayList Branches) {
		Branch CurrBranch;
		BaseKnot CurrKnot;

		for(int i = 0; i < Branches.Count; i++) {
			CurrBranch = Branches[i] as Branch;
			CurrKnot = CurrBranch.pKnots[0] as BaseKnot;
			if(!CurrKnot.IsKnot && !CurrKnot.IsRemovableKnot) return false;
			CurrKnot = CurrBranch.pKnots[CurrBranch.pKnots.Count - 1] as BaseKnot;
			if(!CurrKnot.IsKnot && !CurrKnot.IsRemovableKnot) return false;
			for(int i2 = 1; i2 < CurrBranch.pKnots.Count - 1; i2++) 
				if(((BaseKnot) CurrBranch.pKnots[i2]).IsKnot) return false;
		}
		return true;
	}
#endif
}
}