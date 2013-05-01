using UnityEngine;
using System.Collections;

/* Atmosphere defines the contents of the volume of a room.
 * Three primary properties are stored. Everything else is derived using the ideal gas law. They are:
 * 	*Boundaries
 * 		*Volume: length*width*height
 * 	*Heat
 * 		*Temperature: Heat/C_p
 * 	*Mass
 * 		*Percent: Mass/MassTotal
 * 		*R: mass-modified ideal gas constant, 
 * 		*C_p: mass-modified specific heat at constant pressure
 * 
 * Some other properties that are necessary for calculations are:
 * 	*Density: MassTotal/Volume
 * 	*Pressure: R*Temperature/Volume
 */
 
[ExecuteInEditMode()]
public class Atmosphere : MonoBehaviour {
	//this really only exists so I can draw pretty boxes around the atmospheres... not that important right now.
	//public Bounds boundaries; //the boundaries of this atmosphere. Used in the initial volume calculation.
	
	public float R; //J/K mass-modified ideal gas constant
	public float C_p; //MJ/K mass-modified constant pressure heat capacity
	
	//primary properties ================
	[UnityEngine.SerializeField]
	private float _volume; //m^3
	public float Volume {
		get {return _volume;}
		set {_volume = value;}
	}
	
	[UnityEngine.SerializeField]
	private float _heat;
	public float Heat { //MJ
		get {return _heat;}
		set {_heat = value;}
	}
	
	[UnityEngine.SerializeField]
	private Gases _mass;
	public Gases Mass { //kg
		get {return _mass;}
		set {_mass = value; CalculateProperties();}
	}
	
	//derived properties ===================
	public float Temperature { //K
		get {return _heat/C_p;}
		set {_heat = value*C_p;} //this gives devs an easy way to set the temperature to a specific value, since what's actually stored is heat.
	}
	
	public float Pressure { //Pa
		get {return R*Temperature/_volume;}
	}
	
	private Gases _percent = new Gases();
	public Gases Percent { //%
		get {return _percent;}
	}
	
	public float GasDensity { //kg/m^3
		get {return _mass.Total/_volume;}
	}
	
	void Start() {
		CalculateProperties();
	}
	
	public void CalculateProperties() {
		//calculate values using the ideal gas law for mixtures: P = R_total * T / V
		R = (_mass.N2*296.8f + _mass.O2*259.8f + _mass.CO2*188.9f + _mass.CO*297f + _mass.CH4*518.3f); //individual R values are all in J/kg*K
		C_p = (_mass.N2*1.04f + _mass.O2*.919f + _mass.CO2*.844f + _mass.CO*1.02f + _mass.CH4*2.22f)/1000f; //individual c_p values are all in kJ/kg*K
		
		//calculate percentages
		float masstotal = _mass.Total;
		_percent.N2 = _mass.N2/masstotal;
		_percent.O2 = _mass.O2/masstotal;
		_percent.CO2 = _mass.CO2/masstotal;
		_percent.CO = _mass.CO/masstotal;
		_percent.CH4 = _mass.CH4/masstotal;
		_percent.NOX = _mass.NOX/masstotal;
	}
	
	static public float CalculateHeatCapacity(Gases gas) {
		return (gas.N2*1.04f + gas.O2*.919f + gas.CO2*.844f + gas.CO*1.02f + gas.CH4*2.22f);
	}
	
	//retrieve a packet of gas and heat from this atmosphere, with a mass total up to but not exceeding "amount".
	public void PullMassPacket(float amount, out Gases outGas, out float outHeat) {
		float ratio = amount / Mass.Total; //ratio of "what we're taking" over "what we have"
		
		if (ratio > 1) { //trying to pull more than we have, so just give it all.
			outGas = _mass;
			outHeat = _heat;
			
			_mass = new Gases();
			_heat = 0;
		} else {
			outGas = _mass*ratio;
			outHeat = _heat*ratio;
			
			_mass -= outGas;
			_heat -= outHeat;
		}
	}
	
	//deposit some mass and heat into this atmosphere.
	public void PushMassPacket(Gases gas, float heat) {
		_mass += gas;
		_heat += heat;
		
		CalculateProperties();
	}
}