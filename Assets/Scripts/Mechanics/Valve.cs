//  
//  Valve.cs
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

//TODO Valve should be instead a class that updates an airlock with a new location and opens/closes it
public class Valve : MonoBehaviour {
	public Vector3 Location; //the valve's location relative to this gameobject.
	public float Area; //the valve's area
	public Atmosphere Inside;
	public Atmosphere Outside;
	
	private float Y; //expansion factor
	private float T; //difference factor
	private float mdot;
	
	private gases dm;
	private float dQ;
	
	public bool open;
	public bool debug = false;
	
	void Update() {
		
	}
}

