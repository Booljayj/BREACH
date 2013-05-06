using UnityEngine;
using System.Collections;

public enum GasType {
	Nitrogen,
	Oxygen,
	CarbonDioxide,
	CarbonMonoxide,
	Methane,
	NitrousOxides,
}

[System.SerializableAttribute]
public class Gases {
	public float N2;
	public float O2;
	public float CO2;
	
	public float CO;
	public float CH4;
	public float NOX;
	
	public float Total {
		get {return N2+O2+CO2+CO+CH4+NOX;}
	}
	public float AbsTotal {
		get {return Mathf.Abs(N2)+Mathf.Abs(O2)+Mathf.Abs(CO2)+Mathf.Abs(CO)+Mathf.Abs(CH4)+Mathf.Abs(NOX);}
	}
	
	public float PTotal {
		get {float t = 0;
			if (N2 > 0) t += N2;
			if (O2 > 0) t += O2;
			if (CO2 > 0) t += CO2;
			if (CO > 0) t += CO;
			if (CH4 > 0) t += CH4;
			if (NOX > 0) t += NOX;
			return t;
		}
	}
	public float NTotal {
		get {return (this*-1f).PTotal;}
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
		return new Gases(A.N2-B.N2,
			A.O2-B.O2,
			A.CO2-B.CO2,
			A.CO-B.CO,
			A.CH4-B.CH4,
			A.NOX-B.NOX);
	}
	public static Gases operator * (Gases g, float s) {
		return new Gases(g.N2*s,
			g.O2*s,
			g.CO2*s,
			g.CO*s,
			g.CH4*s,
			g.NOX*s);
	}
	public static Gases operator / (Gases g, float s) {
		return new Gases(g.N2/s,
			g.O2/s,
			g.CO2/s,
			g.CO/s,
			g.CH4/s,
			g.NOX/s);
	}
}

