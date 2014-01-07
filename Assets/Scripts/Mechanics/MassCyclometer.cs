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
using System.Collections.Generic;

public class MassCyclometer : MonoBehaviour {
	public List<string> channels = new List<string>(){"Nitrogen", "Oxygen", "Carbon Dioxide"};
	public float tolerance = .001f;

	private float error;
	public float Efficiency {
		get {return 1-error;}
		set {error = 1-value;}
	}
	
	public float[] RandomArray(int destination, float error) {
		float[] x = new float[channels.Count+1];
		float c;
		
		x[destination] = Random.Range(1f-error, 1f); //this represents the r-value, or the value for the desired gas
		c = 1-x[destination]; //this represents the remainder for the sum of values to equal 1
		for (int i = 0; i < channels.Count+1; i++) {
			if (i == destination) continue; //skip over the destination x-value
			
			if (c <= tolerance) { //simplify calculation for very small c-values
				x[i] = c;
				break;
			}

			//remaining gas must go into final channel
			
			x[i] = Random.Range(0f, c); //make a new value for a contaminant
			c -= x[i];
		}
		x[channels.Count] = c; //all remaining gas goes to the final channel
		
		return x;
	}
	
	public List<AtmospherePacket> Process(AtmospherePacket input) {
		List<AtmospherePacket> outputs = new List<AtmospherePacket>(); //the output channels

		int destination; //the destination channel of the processing
		float[] x; //the gas fractions
		float gasHeat; //gas heat
		foreach (string gas in input.Keys) {
			destination = channels.IndexOf(gas);
			if (destination == -1) destination = channels.Count;
			x = RandomArray(destination, error);

			gasHeat = input.percent[gas]*input.heat;
			input.heat -= gasHeat;

			for (int i = 0; i < channels.Count+1; i++) {
				if (outputs[i] == null) outputs.Add(new AtmospherePacket());

				outputs[i][gas] += input[gas]*x[i];
				outputs[i].heat += gasHeat*x[i];
			}
		}

		return outputs;
	}
}

