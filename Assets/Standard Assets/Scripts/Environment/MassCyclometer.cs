//  
//  MassCyclometer.cs
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

public class GasMatrix {	
	public Gases N {get; set;}
	public Gases O {get; set;}
	public Gases C {get; set;}
	public Gases T {get; set;}
	
	public GasMatrix(Gases n, Gases o, Gases c, Gases t) {
		N = n; O = o; C = c; T = t;
	}
	public GasMatrix() {}
}

public class MassCyclometer : MonoBehaviour {
	private float error;
	public float Efficiency {
		get {return 1-error;}
		set {error = 1-value;}
	}
	private float tolerance = .001f;
	
	void Update () {
		//update error based on power available
	}
	
	public float[] RandomArray(float error, float tolerance = 0f) {
		float[] x = new float[4];
		float c;
		
		if (1f-error < tolerance) {
			Debug.LogError(string.Format("Invalid Error or Tolerance: {0}, {1}", error, tolerance));
		}
		
		x[0] = Random.Range(1f-error, 1f); //this represents the r-value, or the value for the desired gas
		c = 1-x[0]; //this represents the remainder for the sum of values to equal 1
		for (int i = 1; i < 3; i++) {
			if (c <= tolerance) { //simplify calculation for very small c-values
				x[i] = c;
				break;
			}
			
			x[i] = Random.Range(0f, c); //make a new value for a contaminant
			c -= x[i];
		}
		
		return x;
	}
	
	public void SubProcess(GasType type, float[] x, Gases input, GasMatrix M) {
		if (type == GasType.Nitrogen) {
			M.N.N2 += x[0]*input.N2;
			M.O.N2 += x[1]*input.N2;
			M.C.N2 += x[2]*input.N2;
			M.T.N2 += x[3]*input.N2;			
		} else if (type == GasType.Oxygen) {
			M.N.N2 += x[1]*input.O2;
			M.O.N2 += x[0]*input.O2;
			M.C.N2 += x[2]*input.O2;
			M.T.N2 += x[3]*input.O2;
		} else if (type == GasType.CarbonDioxide) {
			M.N.N2 += x[1]*input.CO2;
			M.O.N2 += x[2]*input.CO2;
			M.C.N2 += x[0]*input.CO2;
			M.T.N2 += x[3]*input.CO2;
		} else {
			M.N[type] += x[1]*input[type];
			M.O[type] += x[2]*input[type];
			M.C[type] += x[3]*input[type];
			M.T[type] += x[0]*input[type];
		}
	}
	
	public void Process(Gases input, Gases N, Gases O, Gases C, Gases T) {
		float[] x;
		GasMatrix M = new GasMatrix(N,O,C,T);
		
		//Nitrogen
		x = RandomArray(error, tolerance);
		SubProcess(GasType.Nitrogen, x, input, M);
		
		//Oxygen
		x = RandomArray(error, tolerance);
		SubProcess(GasType.Oxygen, x, input, M);
		
		//Carbon Dioxide
		x = RandomArray(error, tolerance);
		SubProcess(GasType.CarbonDioxide, x, input, M);
		
		//Carbon Monoxide
		x = RandomArray(error, tolerance);
		SubProcess(GasType.CarbonMonoxide, x, input, M);
		
		//Methane
		x = RandomArray(error, tolerance);
		SubProcess(GasType.Methane, x, input, M);
		
		//NOX
		x = RandomArray(error, tolerance);
		SubProcess(GasType.NitrousOxides, x, input, M);
	}
}

