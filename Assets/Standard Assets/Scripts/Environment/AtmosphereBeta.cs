
 // Atmosphere2.cs

 // Author:
 //      Justin Bool <booljayj@gmail.com>

 // Copyright (c) 2013

 // This program is free software: you can redistribute it and/or modify
 // it under the terms of the GNU General Public License as published by
 // the Free Software Foundation, either version 3 of the License, or
 // (at your option) any later version.

 // This program is distributed in the hope that it will be useful,
 // but WITHOUT ANY WARRANTY; without even the implied warranty of
 // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 // GNU General Public License for more details.

 // You should have received a copy of the GNU General Public License
 // along with this program.  If not, see <http://www.gnu.org/licenses/>.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AtmosphereBeta : MonoBehaviour {
	const public PropertyGroup Properties = new PropertyGroup();

	#region Primary Properties //=================================================================
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
				Mass += value;
				gMasses.Add(type, value);
			} else {
				Mass += value - gMasses[type];
				gMasses[type] = value;
			}
			Recalculate();
		}
	}

	public float Mass {get; private set;} // kg
	#endregion

	#region Mass-Driven Properties //=============================================================
	private void Recalculate() {
		_R = 0; _Cp = 0; _k = 0; _Percent = new Dictionary<string, float>();

		foreach (string type in gMasses.Keys) {
			Property gas = Properties[type];
			float gmass = this[type];

			_R += gmass*gas.R; //individual R values are all in J/kg*K
			_Cp += gmass*gas.Cp; //individual Cp values are all in kJ/kg*K
			_k += gmass*gas.k;
			_Percent[type] = gmass/Mass;
		}
		_k /= Mass;
	}

	private float _R; // J/K
	public float R {get {return _R;}}

	private float _Cp; // kJ/K
	public float Cp {get {return _Cp;}}

	private float _k; // %
	public float k {get {return _k;}}

	private Dictionary<string, float> _Percent; // %
	public Dictionary<string, float> Percent {get {return _Percent;}}
	#endregion

	#region Derived Properties //=================================================================
	public float Temperature { // K
		get {return Heat/_Cp;}
		set {Heat = value*_Cp;} //for convenience
	}

	public float Pressure { // Pa
		get {return _R*Temperature/Volume;}
	}

	public float Density { // kg/m^3
		get {return Mass/Volume;}
	}

	public int Length {
		get {return gMasses.Count;}
	}
	#endregion

	#region Constructors //=======================================================================
	public AtmosphereBeta(AtmosphereBeta A) : this(A.gMasses, A.Heat, A.Volume) {}

	public AtmosphereBeta(Dictionary<string, float> gMass = null, float heat = 0f, float volume = 0f) {
		if (gMass == null)
			gMasses = new Dictionary<string, float>();
		else {
			gMasses = new Dictionary<string, float>(gMass);
			foreach (float value in gMasses.Values) {
				Mass += value;
			}
		}
		Recalculate();

		Heat = heat;
		Volume = 0;
	}
	#endregion

	#region Pull Methods //=======================================================================
	//pull a volume of air from the system, up to the maximum volume
	public AtmosphereBeta PullVolume(float volume) {
		AtmosphereBeta Ar;

		if (volume >= Volume || volume == -1f)
			Ar = new AtmosphereBeta(this);
		else
			Ar = this*(volume/Volume);
		Ar.Volume = volume;

		foreach (string type in gMasses.Keys)
			this[type] -= Ar[type];
		Heat -= Ar.Heat;

		return Ar;
	}

	//pull mass from the system, up to the value given
	public Dictionary<string, float> PullMass(float mass) {
		AtmosphereBeta Ar;

		if (mass >= Mass)
			Ar = new AtmosphereBeta(this);
		else
			Ar = this*(mass/Mass);

		foreach (string type in gMasses.Keys)
			this[type] -= Ar[type];

		return Ar.gMasses;
	}
	#endregion

	#region Push Methods //=======================================================================
	//push a volume of air into the system, with optional constraint on the volume pushed.
	public void PushVolume(AtmosphereBeta Ai, float volume = -1f) {
		AtmosphereBeta Ap; float ratio;

		if (volume == -1f || volume >= Ai.Volume)
			ratio = 1f;
		else
			ratio = volume/Ai.Volume;
		Ap = Ai*ratio;

		foreach (string type in Ap.gMasses.Keys) {
			this[type] += Ap[type];
			Ai[type] -= Ap[type];
		}
		Heat += Ap.Heat;
		Ai.Heat -= Ap.Heat;
	}

	//push mass into the system, with optional constraint on the mass pushed
	public void PushMass(Dictionary<string, float> Mi, float mass = -1f) {
		float masstotal = 0; float ratio;
		foreach (float val in Mi.Values)
			masstotal += val;

		if (mass == -1f || mass >= masstotal)
			ratio = 1f;
		else
			ratio = mass/masstotal;

		foreach (string type in mass.Keys) {
			this[type] += mass[type]*ratio;
			mass[type] -= mass[type]*ratio;
		}
	}
	#endregion

	#region Misc Methods //=======================================================================
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

	public IEnumerator GetEnumerator() {
		return gMasses.GetEnumerator();
	}

	static public AtmosphereBeta operator+(AtmosphereBeta A1, AtmosphereBeta A2) {
		AtmosphereBeta Ar = new AtmosphereBeta(null, A1.Heat + A2.Heat, A1.Volume + A2.Volume);
		foreach (KeyValuePair<string, float> pair in A1)
			Ar[pair.Key] += pair.Value;
		foreach (KeyValuePair<string, float> pair in A2)
			Ar[pair.Key] += pair.Value;
		return Ar;
	}

	static public AtmosphereBeta operator-(AtmosphereBeta A1, AtmosphereBeta A2) {
		AtmosphereBeta Ar = new AtmosphereBeta(null, A1.Heat - A2.Heat, A1.Volume - A2.Volume);
		foreach (KeyValuePair<string, float> pair in A1)
			Ar[pair.Key] += pair.Value;
		foreach (KeyValuePair<string, float> pair in A2)
			Ar[pair.Key] -= pair.Value;
		return Ar;
	}

	static public AtmosphereBeta operator*(AtmosphereBeta A, float c) {
		AtmosphereBeta Ar = new AtmosphereBeta(null, A.Heat*c, A.Volume*c);
		foreach (KeyValuePair<string, float> pair in A)
			Ar[pair.Key] = pair.Value*c;
		return Ar;
	}
	static public AtmosphereBeta operator*(float c, AtmosphereBeta A) {
		return A*c;
	}

	static public AtmosphereBeta operator/(AtmosphereBeta A, float c) {
		AtmosphereBeta Ar = new AtmosphereBeta(null, A.Heat/c, A.Volume/c);
		foreach (KeyValuePair<string, float> pair in A)
			Ar[pair.Key] = pair.Value/c;
		return Ar;
	}
	#endregion
}
