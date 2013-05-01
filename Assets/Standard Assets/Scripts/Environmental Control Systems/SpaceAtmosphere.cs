using UnityEngine;
using System.Collections;

public class SpaceAtmosphere : Atmosphere {
	public new float Temperature { //K
		get {return .00001f;}
		set {;} //this gives devs an easy way to set the temperature to a specific value, since what's actually stored is heat.
	}
	
	public new float Pressure { //Pa
		get {return .00001f;}
	}
	
	public new float Volume {
		get {return float.MaxValue;}
		set {;}
	}
	
	public new float Heat { //MJ
		get {return .00001f;}
		set {;}
	}
	
	public new Gases Mass { //kg
		get {return new Gases();}
		set {;}
	}

	public new Gases Percent { //%
		get {return new Gases();}
	}
}

