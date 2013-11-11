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

public class RoomController : MonoBehaviour {
	static float nomFlowRate = 10f; //m^3/s		//value chosen is about what a commercial air conditioner can do
	
	public float pressure = 80000f; //Pa		//value is nominal air pressure
	public float temperature = 293.15f; //K		//value is room temperature
	public float o2 = .2f; //					//value is air O2 percent
	
	public Gases idealPercent {get; private set;} //
	public float idealR {get; private set;} // kJ/kg*K
	public float idealCp {get; private set;} // kJ/kg*K
	public AnimationCurve iFlowRate {get; private set;} //m^3/s (Pa)
	public AnimationCurve oFlowRate {get; private set;} //m^3/s (Pa)
	
	public void Refresh() {
		idealPercent = new Gases(1-o2, o2, 0,0,0,0);
		idealR = Atmosphere.CalculateIdealConstant(idealPercent);
		idealCp = Atmosphere.CalculateHeatCapacity(idealPercent);
		
		iFlowRate = new AnimationCurve(new Keyframe(0, 0), new Keyframe(pressure, nomFlowRate), new Keyframe(2*pressure, 2*nomFlowRate));
		oFlowRate = new AnimationCurve(new Keyframe(0, 2*nomFlowRate), new Keyframe(pressure, nomFlowRate), new Keyframe(2*pressure, 0));
	}
}

