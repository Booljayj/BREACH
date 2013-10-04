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

public class MassCyclometer : MonoBehaviour {
	private float error;
	public float Efficiency {
		get {return 1-error;}
		set {error = 1-value;}
	}
	
	private float rand;
	
	void Start () {
	}
	
	void Update () {
	}
	
//	public void Process(Gases input, out Gases N, out Gases O, out Gases C, out Gases T) {
//		T = new Gases(0,0,0, Random.Range(Efficiency*input.CO, input.CO), Random.Range(Efficiency*input.CH4, input.CH4), 0);
//		
//		rand = Random.Range(Efficiency, 1f);
//		N = new Gases(rand*input.N2, 0,0,0,0,0);
//		
//	}
}

