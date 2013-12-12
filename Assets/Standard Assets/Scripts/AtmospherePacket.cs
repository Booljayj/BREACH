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
		R = 0; Cp = 0; Cv = 0; k = 0; percent.Clear();

		foreach (string type in gases.Keys) {
			Property gas = Properties.Get(type);
			float gmass = this[type];

			R += gmass*gas.R;
			Cp += gmass*gas.Cp;
			Cv += gmass*gas.Cv;
			k += gmass*gas.k;
			percent[type] = gmass/mass;
		}
		k /= mass;
	}
	
	#region Primary Fields //=================================================================
	public float volume = 1f; // m^3
	public float heat; // kJ
	public float mass; // kg
	
	public Dictionary<string, float> gases = new Dictionary<string, float>(); // kg
	public Dictionary<string, float> percent = new Dictionary<string, float>(); // %
	#endregion

	#region Ideal Gas Properties //=============================================================
	public float R {get; private set;} // kJ/K
	public float Cp {get; private set;} // kJ/K
	public float Cv {get; private set;} // kJ/K
	public float k {get; private set;} // %
	#endregion

	#region Derived Properties //=================================================================
	public float Temperature { // K
		get {return heat/Cp;}
		set {heat = value*Cp;} //for convenience
	}

	public float Pressure { // kPa
		get {return R*Temperature/volume;}
	}

	public float Density { // kg/m^3
		get {return mass/volume;}
	}
	#endregion
	
	#region Dictionary Accessors //===============================================================
	public Dictionary<string, float>.KeyCollection Keys {
		get {return gases.Keys;}
	}
	
	public Dictionary<string, float>.ValueCollection Values {
		get {return gases.Values;}
	}
	#endregion
	
	#region Operations //=======================================================================
	static public AtmospherePacket operator+(AtmospherePacket A1, AtmospherePacket A2) {
		AtmospherePacket Ar = new AtmospherePacket();
		foreach (KeyValuePair<string, float> pair in A1.gases)
			Ar.gases.Add(pair.Key, pair.Value);
		foreach (KeyValuePair<string, float> pair in A2.gases) {
			Ar.gases[pair.Key] += pair.Value;
		}
		Ar.mass = A1.mass+A2.mass;
		Ar.heat = A1.heat+A2.heat;
		Ar.volume = A1.volume+A2.volume;
		Ar.Recalculate();
		return Ar;
	}

	static public AtmospherePacket operator-(AtmospherePacket A1, AtmospherePacket A2) {
		AtmospherePacket Ar = new AtmospherePacket();
		foreach (KeyValuePair<string, float> pair in A1.gases)
			Ar.gases.Add(pair.Key, pair.Value);
		foreach (KeyValuePair<string, float> pair in A2.gases) {
			Ar.gases[pair.Key] -= pair.Value;
		}
		Ar.mass = A1.mass-A2.mass;
		Ar.heat = A1.heat-A2.heat;
		Ar.volume = A1.volume-A2.volume;
		Ar.Recalculate();
		return Ar;
	}

	static public AtmospherePacket operator*(AtmospherePacket A, float c) {
		AtmospherePacket Ar = new AtmospherePacket();
		foreach (KeyValuePair<string, float> pair in A.gases)
			Ar.gases.Add(pair.Key, pair.Value*c);
		Ar.mass = A.mass*c;
		Ar.heat = A.heat*c;
		Ar.volume = A.volume*c;
		Ar.Recalculate();
		return Ar;
	}
	static public AtmospherePacket operator*(float c, AtmospherePacket A) {
		return A*c;
	}

	static public AtmospherePacket operator/(AtmospherePacket A, float c) {
		AtmospherePacket Ar = new AtmospherePacket();
		foreach (KeyValuePair<string, float> pair in A.gases)
			Ar.gases.Add(pair.Key, pair.Value/c);
		Ar.mass = A.mass/c;
		Ar.heat = A.heat/c;
		Ar.volume = A.volume/c;
		Ar.Recalculate();
		return Ar;
	}
	#endregion
}

