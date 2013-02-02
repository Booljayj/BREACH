using UnityEngine;
using System.Collections;

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
	
	public Gases () {
	}
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

