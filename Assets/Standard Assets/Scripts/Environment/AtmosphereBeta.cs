//  
//  Atmosphere2.cs
//  
//  Author:
//       Justin Bool <booljayj@gmail.com>
// 
//  Copyright (c) 2013 
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AtmosphereBeta : MonoBehaviour {
	#region Static Properties
	static private DynamicDictionary<string, Property> Properties = new DynamicDictionary<string, Property>(){
		{"Nitrogen", new Property(296.8f, 1.04f)},
		{"Oxygen", new Property(259.8f, .919f)},
		{"CarbonDioxide", new Property(188.9f, .844f)},
		{"CarbonMonoxide", new Property(297f, 1.02f)},
		{"Methane", new Property(518.3f, 2.22f)}
	};
	
	static public AtmosphereBeta Zero {get; private set;}
	#endregion
	
	#region Primary Properties
	public float Volume {get; set;} // m^3
	
	public float Heat {get; set;} // kJ
	
	private Dictionary<string, float> gMasses; // kg
	public float this[string type] {
		get {
			if (!gMasses.ContainsKey(type)) return 0;
			return gMasses[type];
		}
		set {
			if (!gMasses.ContainsKey(type)) {
				gMasses.Add(type, value);
			} else {
				gMasses[type] = value;
			}
			Mass += value;
		}
	}
	public float Mass {get; private set;} // kg
	#endregion
	
	#region Atmospheric Properties
	public float R { // J/K
		get {
			float _R = 0;
			foreach (string type in gMasses.Keys)
				_R += gMasses[type]*Properties[type].R; //individual R values are all in J/kg*K
			return _R;
		}
	}
	
	public float Cp { // kJ/K
		get {
			float _Cp = 0;
			foreach (string type in gMasses.Keys)
				_Cp += gMasses[type]*Properties[type].Cp; //individual Cp values are all in kJ/kg*K
			return _Cp;
		}
	}
	#endregion
	
	#region Derived Properties
	public Dictionary<string, float> Percent { // %
		get {
			Dictionary<string, float> percent = new Dictionary<string, float>(gMasses);
			foreach (string type in percent.Keys) {
				percent[type] /= Mass;
			}
			return percent;
		}
	}
	
	public float Temperature { // K
		get {return Heat/Cp;}
		set {Heat = value*Cp;} //for convenience
	}
	
	public float Pressure { // Pa
		get {return R*Temperature/Volume;}
	}
	
	public float Density { // kg/m^3
		get {return Mass/Volume;}
	}
	
	public int Length {
		get {return gMasses.Count;}
	}
	#endregion
	
	#region Constructors
	public AtmosphereBeta(AtmosphereBeta A) {
		gMasses = new Dictionary<string, float>(A.gMasses);
		Heat = A.Heat;
		Volume = A.Volume;
	}
	
	public AtmosphereBeta(Dictionary<string, float> gMass = null, float heat = 0f, float volume = 0f) {
		if (gMass == null)
			gMasses = new Dictionary<string, float>();
		else
			gMasses = new Dictionary<string, float>(gMass);
		Heat = heat;
		Volume = 0;
	}
	#endregion
	
	#region Methods
	public AtmosphereBeta GetMass(float mass) {
		if (mass >= Mass)
			return new AtmosphereBeta(this);
		else
			return this*(mass/Mass);
	}
	
	public AtmosphereBeta GetVolume(float volume) {
		if (volume >= Volume)
			return new AtmosphereBeta(this);
		else {
			AtmosphereBeta Ar = this*(volume/Volume);
			Ar.Volume = volume;
			return Ar;
		}
	}
	
	static public bool Approximately(AtmosphereBeta A1, AtmosphereBeta A2) {
		if (!Mathf.Approximately(A1.Volume, A2.Volume)) return false;
		if (!Mathf.Approximately(A1.Heat, A2.Heat)) return false;
		if (!Mathf.Approximately(A1.Mass, A2.Mass)) return false;
		
		if (!Mathf.Approximately(A1.Length, A2.Length)) return false;
		foreach (KeyValuePair<string, float> pair in A1) {
			if (!Mathf.Approximately(A2[pair.Key], pair.Value)) return false;
		}
		return true;
	}
	#endregion
	
	#region Operators
	public IEnumerator GetEnumerator() {
		return gMasses.GetEnumerator();
	}
	
	static public AtmosphereBeta operator+(AtmosphereBeta A1, AtmosphereBeta A2) { //this addition is directional, with ar.volume == a1.volume
		AtmosphereBeta Ar = new AtmosphereBeta(null, A1.Heat + A2.Heat, A1.Volume);
		foreach (KeyValuePair<string, float> pair in A1)
			Ar[pair.Key] += pair.Value;
		foreach (KeyValuePair<string, float> pair in A2)
			Ar[pair.Key] += pair.Value;
		return Ar;
	}
	
	static public AtmosphereBeta operator-(AtmosphereBeta A1, AtmosphereBeta A2) {
		AtmosphereBeta Ar = new AtmosphereBeta(null, A1.Heat - A2.Heat, A1.Volume);
		foreach (KeyValuePair<string, float> pair in A1)
			Ar[pair.Key] += pair.Value;
		foreach (KeyValuePair<string, float> pair in A2)
			Ar[pair.Key] -= pair.Value;
		return Ar;
	}
	
	static public AtmosphereBeta operator*(AtmosphereBeta A, float c) {
		AtmosphereBeta Ar = new AtmosphereBeta(null, A.Heat*c, A.Volume);
		foreach (KeyValuePair<string, float> pair in A)
			Ar[pair.Key] = pair.Value*c;
		return Ar;
	}
	static public AtmosphereBeta operator*(float c, AtmosphereBeta A) {
		return A*c;
	}
	
	static public AtmosphereBeta operator/(AtmosphereBeta A, float c) {
		AtmosphereBeta Ar = new AtmosphereBeta(null, A.Heat/c, A.Volume);
		foreach (KeyValuePair<string, float> pair in A)
			Ar[pair.Key] = pair.Value/c;
		return Ar;
	}
	
	
	#endregion
}

public class Property {
	public Property() {}
	public Property(float R = 0, float Cp = 0, float k = 0) {
		this.R = R;
		this.Cp = Cp;
		this.k = k;
	}
	
	public float R {get; private set;}
	public float Cp {get; private set;}
	public float k {get; private set;}
}