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

public class Valve : MonoBehaviour {
	public Vector3 Location; //the valve's location relative to this gameobject.
	public float Area; //the valve's area
	public Atmosphere Inside;
	public Atmosphere Outside;
	
	private float Y; //expansion factor
	private float T; //difference factor
	private float mdot;
	
	private Gases dm;
	private float dQ;
	
	public bool open;
	public bool debug = false;
	
	void Update() {
		if (open) {
			//find where Outside is using the Valve location. If none is found, we are venting to space.
			Outside = GameObject.FindWithTag("Space").GetComponent<Atmosphere>();
			
			if (Inside.Pressure > Outside.Pressure) {
			//if (Mathf.RoundToInt(RoomA.Pressure) != Mathf.RoundToInt(RoomB.Pressure)) {
				//calculate the expansion factor Y = 1-(1-r)*(.41+.35 b^4)/k
				Y = 1f - (1f-Outside.Pressure/Inside.Pressure)*(.2938f);
			
				//calculate the difference scale factor T = sqrt(1-p2/p1);
				T = Mathf.Sqrt(1-Mathf.Pow(Outside.Pressure/Inside.Pressure, 4));
				
				//calculate the mass flow rate m = TCYA*sqrt(2*rho*(P1-P2)).
				mdot = T*.6f*Y*Area*Mathf.Sqrt(2f*Inside.GasDensity*(Inside.Pressure-Outside.Pressure));
				
				//calculate the dm from pressure differences
				dm = Inside.Percent*mdot*Time.deltaTime;
			} else {
				dm = new Gases();
			}
			
			//Mass-driven heat transfer
			if (!Mathf.Approximately(dm.AbsTotal, 0)) {
				//get available mass from inside atmosphere
				dm = Inside.GetGases(dm);
				
				//calculate the mass-driven heat transfer
				dQ = dm.Total*Inside.Heat/Inside.Mass.Total;
			} else {
				dQ = 0;
			}
			
			Inside.Mass -= dm;
			Outside.Mass += dm;
			
			Inside.Heat -= dQ;
			Outside.Heat += dQ;
			
			if (debug) Debug.Log("Transfering "+dm.Total.ToString()+"kg and "+dQ.ToString()+"MJ from tank "+name);
		}
	}
}

