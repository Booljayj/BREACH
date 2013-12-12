//  
//  RoomController.cs
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

[ExecuteInEditMode]
public class RoomController : MonoBehaviour {
	static float nomFlowRate = 10f; // m^3/s	//value chosen is about what a commercial air conditioner can do
	
	public Atmosphere atmosphere {get; private set;}
	
	public float iPressure = 80f; // kPa		//value is nominal air pressure
	public float iTemperature = 293.15f; // K	//value is room temperature
	public float iO2 = .2f; // %					//value is air O2 percent
	
	public AtmospherePacket iAtmosphere;
	public AnimationCurve inFlowRate; //m^3/s (Pa)
	public AnimationCurve outFlowRate; //m^3/s (Pa)
	
	void Awake() {
		atmosphere = GetComponent<Atmosphere>();
		iAtmosphere = new AtmospherePacket();
		Refresh();
	}
	
	public void Refresh() {
		iAtmosphere.Reset();
		iAtmosphere.volume = atmosphere.volume;
		iAtmosphere["Oxygen"] = iO2;
		iAtmosphere["Oxygen"] = 1-iO2;
		
		inFlowRate = new AnimationCurve(
			new Keyframe(0, 0),
			new Keyframe(iPressure, nomFlowRate),
			new Keyframe(2*iPressure, 2*nomFlowRate)
			);
		outFlowRate = new AnimationCurve(
			new Keyframe(0, 2*nomFlowRate),
			new Keyframe(iPressure, nomFlowRate),
			new Keyframe(2*iPressure, 0)
			);
	}
}

