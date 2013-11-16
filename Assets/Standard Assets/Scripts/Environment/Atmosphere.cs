using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Atmosphere : MonoBehaviour {
	void Awake() {
		Debug.Log("Atmosphere Enabled");
		
		if (gases == null) gases = ScriptableObject.CreateInstance<GasDictionary>();
		if (percent == null) percent = ScriptableObject.CreateInstance<GasDictionary>();
	}
	
	void OnEnable() {
		Recalculate();
	}
	
	void Reset() {
		volume = 1f; heat = 0f; mass = 0f;
		gases.Clear(); percent.Clear();
		R = 0; Cp = 0; k = 0;
	}
	
	public float this[string type] {
		get {return gases[type];}
		set {
			mass -= gases[type];
			gases[type] = value;
			mass += value;
			Recalculate();
		}
	}
	
	public void Remove(string type) {
		mass -= gases[type];
		gases.Remove(type);
		Recalculate();
	}
	
	private void Recalculate() {
		R = 0; Cp = 0; k = 0; percent.Clear();

		foreach (string type in gases.Keys) {
			Property gas = Properties.Get(type);
			float gmass = this[type];

			R += gmass*gas.R; //individual R values are all in J/kg*K
			Cp += gmass*gas.Cp; //individual Cp values are all in kJ/kg*K
			k += gmass*gas.k;
			percent[type] = gmass/mass;
		}
		k /= mass;
	}
	
	#region Primary Fields //=================================================================
	public float volume = 1f; // m^3
	public float heat; // kJ
	public float mass; // kg
	
	//storage for the gas dictionary
	public GasDictionary gases;// = ScriptableObject.CreateInstance<GasDictionary>();
	public GasDictionary percent;// = ScriptableObject.CreateInstance<GasDictionary>();
	//[SerializeField] public GasDictionary gases = new GasDictionary(); // kg
	//[SerializeField] public GasDictionary percent = new GasDictionary(); // %
	//[SerializeField] public List<GasPair> gaslist = new List<GasPair>();
	#endregion

	#region Mass Properties //=============================================================
	public float R {get; private set;} // J/K
	public float Cp {get; private set;} // kJ/K
	public float k {get; private set;} // %
	#endregion

	#region Derived Properties //=================================================================
	public float Temperature { // K
		get {return heat/Cp;}
		set {heat = value*Cp;} //for convenience
	}

	public float Pressure { // Pa
		get {return R*Temperature/volume;}
	}

	public float Density { // kg/m^3
		get {return mass/volume;}
	}
	#endregion
	
	#region Dictionary Accessors //===============================================================
	public int Count {
		get {return gases.Count;}
	}
	
	public System.Collections.Generic.Dictionary<string, float>.KeyCollection Keys {
		get {return gases.Keys;}
	}
	
	public System.Collections.Generic.Dictionary<string, float>.ValueCollection Values {
		get {return gases.Values;}
	}
	
	public IEnumerator GetEnumerator() {
		return gases.GetEnumerator();
	}
	#endregion
	
	#region Get Methods //========================================================================
	//get a specific volume
	public AtmospherePacket GetVolume(float vol = -1f) {
		AtmospherePacket Ap = new AtmospherePacket();
		float ratio;
		
		if (vol >= volume || vol == -1f)
			ratio = 1f;
		else
			ratio = vol/volume;
		
		foreach (KeyValuePair<string, float> pair in gases)
			Ap[pair.Key] = pair.Value*ratio;
		Ap.heat = heat*ratio;
		Ap.volume = volume;
		
		return Ap;
	}
	
	//get a specific mass
	public AtmospherePacket GetMass(float ma = -1f) {
		AtmospherePacket Ap = new AtmospherePacket();
		float ratio;
		
		if (ma >= mass || ma == -1f)
			ratio = 1f;
		else
			ratio = ma/mass;
		
		foreach (KeyValuePair<string, float> pair in gases)
			Ap[pair.Key] = pair.Value*ratio;
		Ap.heat = heat*ratio;
		Ap.volume = volume*ratio;
		
		return Ap;
	}
	#endregion

	#region Transport Methods //==================================================================
	//pull a specified atmosphere from this one
	public void Pull(AtmospherePacket atmos) {
		foreach (string type in atmos.Keys) {
			gases[type] -= atmos[type];
		}
		mass -= atmos.mass;
		heat -= atmos.heat;
		
		Recalculate();
	}
	
	//push a specified atmosphere into this one
	public void Push(AtmospherePacket atmos) {
		foreach (string type in atmos.Keys) {
			gases[type] += atmos[type];
		}
		mass += atmos.mass;
		heat += atmos.heat;
		
		Recalculate();
	}
	#endregion
}
