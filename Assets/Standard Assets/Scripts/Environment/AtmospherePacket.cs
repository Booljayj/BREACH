//  
//  AtmospherePacket.cs
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

public class AtmospherePacket {
	public void Reset() {
		volume = 1f; heat = 0f;
		gases.Clear(); mass = 0f;
		R = 0; Cp = 0; k = 0; percent.Clear();
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
	
	#region Primary Properties //=================================================================
	public float volume; // m^3
	public float heat; // kJ
	public float mass; // kg
	
	public GasDictionary gases = ScriptableObject.CreateInstance<GasDictionary>();
	public GasDictionary percent = ScriptableObject.CreateInstance<GasDictionary>();
	//private GasDictionary _gases = new GasDictionary(); // kg
	
	#endregion

	#region Ideal Gas Properties //=============================================================
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
	
	#region Operations //=======================================================================
	static public AtmospherePacket operator+(AtmospherePacket A1, AtmospherePacket A2) {
		AtmospherePacket Ar = new AtmospherePacket();
		foreach (KeyValuePair<string, float> pair in A1)
			Ar.gases.Add(pair.Key, pair.Value);
		foreach (KeyValuePair<string, float> pair in A2) {
			Ar.gases[pair.Key] += pair.Value;
		}
		Ar.mass = A1.mass+A2.mass;
		Ar.heat = A1.heat+A2.heat;
		Ar.Recalculate();
		return Ar;
	}

	static public AtmospherePacket operator-(AtmospherePacket A1, AtmospherePacket A2) {
		AtmospherePacket Ar = new AtmospherePacket();
		foreach (KeyValuePair<string, float> pair in A1)
			Ar.gases.Add(pair.Key, pair.Value);
		foreach (KeyValuePair<string, float> pair in A2) {
			Ar.gases[pair.Key] -= pair.Value;
		}
		Ar.mass = A1.mass-A2.mass;
		Ar.heat = A1.heat-A2.heat;
		Ar.Recalculate();
		return Ar;
	}

	static public AtmospherePacket operator*(AtmospherePacket A, float c) {
		AtmospherePacket Ar = new AtmospherePacket();
		foreach (KeyValuePair<string, float> pair in A)
			Ar.gases.Add(pair.Key, pair.Value*c);
		Ar.mass = A.mass*c;
		Ar.heat = A.heat*c;
		Ar.Recalculate();
		return Ar;
	}
	static public AtmospherePacket operator*(float c, AtmospherePacket A) {
		return A*c;
	}

	static public AtmospherePacket operator/(AtmospherePacket A, float c) {
		AtmospherePacket Ar = new AtmospherePacket();
		foreach (KeyValuePair<string, float> pair in A)
			Ar.gases.Add(pair.Key, pair.Value/c);
		Ar.mass = A.mass/c;
		Ar.heat = A.heat/c;
		Ar.Recalculate();
		return Ar;
	}
	#endregion
}

