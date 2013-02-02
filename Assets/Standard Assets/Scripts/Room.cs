using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {
	private float R; //J/kg*K specific ideal gas constant
	private float c_p = .001006f; //MJ/kg*K constant pressure specific heat capacity
	
	[UnityEngine.SerializeField]
	private float _volume; //m^3
	public float Volume {
		get {return _volume;}
		set {_volume = value; CalculateGases();}
	}
	
	[UnityEngine.SerializeField]
	private float _heat; //MJ
	public float Heat {
		get {return _heat;}
		set {_heat = value;}
	}
	
	//[UnityEngine.SerializeField]
	//private float _temperature; //K
	public float Temperature {
		get {return _heat/(c_p*_mass.Total);}
		set {_heat = value*c_p*_mass.Total;}
	}
	
	//[UnityEngine.SerializeField]
	//private float _pressure; //Pa
	public float Pressure {
		get {return R*Temperature/_volume;}
	}
	
	//Gas values
	[UnityEngine.SerializeField]
	private Gases _mass;
	public Gases Mass {
		get {return _mass;}
		set {_mass = value; CalculateGases();}
	}
	
	private Gases _percent = new Gases();
	public Gases Percent {
		get {return _percent;}
	}
	
	public float GasDensity;
	
	void Start() {
		CalculateGases();
	}
	
	public void CalculateGases() {
		//calculate values using the ideal gas law for mixtures: P = R_total * T / V
		// R_total is the specific gas constant for the mixture, defined as R_total = (R_sp_1*m_1 + R_sp_2*m_2 ...+ R_sp_n*m_n)
		R = (_mass.N2*296.8f + _mass.O2*259.8f + _mass.CO2*188.9f + _mass.CO*297f + _mass.CH4*518.3f); //R values are all in J/kg*K
		//R = 250.0f; //ICOE
		//TODO: Currently missing R values for NOX, may need to specify different gases instead.
		
		//gas addition is an isothermal process.
		//_pressure = (R * _temperature) / _volume;
		
		//calculate percent values of gases
		float masstotal = _mass.Total;
		_percent.N2 = _mass.N2/masstotal;
		_percent.O2 = _mass.O2/masstotal;
		_percent.CO2 = _mass.CO2/masstotal;
		_percent.CO = _mass.CO/masstotal;
		_percent.CH4 = _mass.CH4/masstotal;
		_percent.NOX = _mass.NOX/masstotal;
		
		//finally calculate the density of the atmospheric gases
		GasDensity = masstotal/_volume;
	}
}
