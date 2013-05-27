using UnityEngine;
using System.Collections;

public enum GasType {
	Nitrogen,
	Oxygen,
	CarbonDioxide,
	WaterVapor,
	
	CarbonMonoxide,
	Methane,
	NitrousOxides,
}

[System.SerializableAttribute]
public class Gases : IEnumerable { //TODO: Implement IEnumerable
	public float N2;
	public float O2;
	public float CO2;
	public float H2O;
	
	public float CO;
	public float CH4;
	public float NOX;
	
	public float Total {
		get {return N2+O2+CO2+CO+CH4+NOX;}
	}
	public float AbsTotal {
		get {return Mathf.Abs(N2)+Mathf.Abs(O2)+Mathf.Abs(CO2)+Mathf.Abs(CO)+Mathf.Abs(CH4)+Mathf.Abs(NOX);}
	}
	
	public Gases Positives {
		get {return new Gases(
				N2 > 0 ? N2 : 0,
				O2 > 0 ? O2 : 0,
				CO2 > 0 ? CO2 : 0,
				CO > 0 ? CO : 0,
				CH4 > 0 ? CH4 : 0,
				NOX > 0 ? NOX : 0);
		}
	}
	
	public float R {
		get {return (N2*296.8f + O2*259.8f + CO2*188.9f + CO*297f + CH4*518.3f);}//individual R values are all in J/kg*K
	}
	
	public float C_p {
		get {return (N2*1.04f + O2*.919f + CO2*.844f + CO*1.02f + CH4*2.22f)/1000f;}//individual c_p values are all in kJ/kg*K
	}
	
	public Gases () {}
	public Gases (float N2, float O2, float CO2, float CO, float CH4, float NOX) {
		this.N2 = N2;
		this.O2 = O2;
		this.CO2 = CO2;
		this.CO = CO;
		this.CH4 = CH4;
		this.NOX = NOX;
	}
	public Gases (Gases i) {
		this.N2 = i.N2;
		this.O2 = i.O2;
		this.CO2 = i.CO2;
		this.CO = i.CO;
		this.CH4 = i.CH4;
		this.NOX = i.NOX;
	}
	
	public float this[GasType type] {
		get {
			switch (type) {
			case GasType.Nitrogen:
				return N2;
			case GasType.Oxygen:
				return O2;
			case GasType.CarbonDioxide:
				return CO2;
			case GasType.CarbonMonoxide:
				return CO;
			case GasType.Methane:
				return CH4;
			case GasType.NitrousOxides:
				return NOX;
			default:
				return 0f;
			}
		}
		set {
			switch (type) {
			case GasType.Nitrogen:
				N2 = value; break;
			case GasType.Oxygen:
				O2 = value; break;
			case GasType.CarbonDioxide:
				CO2 = value; break;
			case GasType.CarbonMonoxide:
				CO = value; break;
			case GasType.Methane:
				CH4 = value; break;
			case GasType.NitrousOxides:
				NOX = value; break;
			}
		}
	}
	
	public static Gases operator + (Gases A, Gases B) {
		return new Gases(A.N2+B.N2,
			A.O2+B.O2,
			A.CO2+B.CO2,
			A.CO+B.CO,
			A.CH4+B.CH4,
			A.NOX+B.NOX);
	}
	public static Gases operator - (Gases A, Gases B) {
		return A+(B*-1f);
	}
	public static Gases operator * (Gases g, float s) {
		return new Gases(g.N2*s,
			g.O2*s,
			g.CO2*s,
			g.CO*s,
			g.CH4*s,
			g.NOX*s);
	}
	public static Gases operator * (float s, Gases g) {
		return g*s;
	}
	public static Gases operator / (Gases g, float s) {
		return new Gases(g.N2/s,
			g.O2/s,
			g.CO2/s,
			g.CO/s,
			g.CH4/s,
			g.NOX/s);
	}
	
	public void Zero() {
		N2 = 0;
		O2 = 0;
		CO2 = 0;
		CO = 0;
		CH4 = 0;
		NOX = 0;
	}
	
	public static bool Approximately (Gases g1, Gases g2) {
		if (Mathf.Approximately(g1.N2, g2.N2) &&
			Mathf.Approximately(g1.O2, g2.O2) &&
			Mathf.Approximately(g1.CO2, g2.CO2) &&
			Mathf.Approximately(g1.CO, g2.CO) &&
			Mathf.Approximately(g1.CH4, g2.CH4) &&
			Mathf.Approximately(g1.NOX, g2.NOX)) {
			return true;
		} else {
			return false;
		}
	}
}

