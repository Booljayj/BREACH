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
	public gases N {get; set;}
	public gases O {get; set;}
	public gases C {get; set;}
	public gases T {get; set;}
	
	public GasMatrix(gases n, gases o, gases c, gases t) {
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
	
	public float[] RandomArray(int des, float error, float tolerance = .0001f) {
		float[] x = new float[4];
		float c;
		
		if (1f-error < tolerance) {
			Debug.LogError(string.Format("Invalid Error or Tolerance: {0}, {1}", error, tolerance));
		}
		
		des = des > 3 ? 3 : des; //if the dest is greater than 3, set it to 3
		
		x[des] = Random.Range(1f-error, 1f); //this represents the r-value, or the value for the desired gas
		c = 1-x[des]; //this represents the remainder for the sum of values to equal 1
		for (int i = 0; i < 3; i++) {
			if (i == des) continue; //skip over the destination x-value
			
			if (c <= tolerance) { //simplify calculation for very small c-values
				x[i] = c;
				break;
			}
			
			x[i] = Random.Range(0f, c); //make a new value for a contaminant
			c -= x[i];
		}
		
		return x;
	}
	
	public void SubProcess(GasType type, gases input, GasMatrix M) {
		float[] x = RandomArray((int)type, error, tolerance);
		
		M.N[type] += x[0]*input[type];
		M.O[type] += x[1]*input[type];
		M.C[type] += x[2]*input[type];
		M.T[type] += x[3]*input[type];
	}
	
	public void Process(gases input, gases N, gases O, gases C, gases T) {
		GasMatrix M = new GasMatrix(N,O,C,T);
		
		//Nitrogen
		SubProcess(GasType.Nitrogen, input, M);
		
		//Oxygen
		SubProcess(GasType.Oxygen, input, M);
		
		//Carbon Dioxide
		SubProcess(GasType.CarbonDioxide, input, M);
		
		//Carbon Monoxide
		SubProcess(GasType.CarbonMonoxide, input, M);
		
		//Methane
		SubProcess(GasType.Methane, input, M);
		
		//NOX
		SubProcess(GasType.NitrousOxides, input, M);
	}
}

