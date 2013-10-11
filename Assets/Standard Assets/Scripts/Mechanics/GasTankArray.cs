//  
//  GasTankArray.cs
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

public class GasTankArray : MonoBehaviour {
	public List<GasTankHolder> holders;
	
	public Gases Push(Gases gas) {
		foreach (GasTankHolder holder in holders) {
			if (holder.tank != null) { //there is a tank in this holder
				if (Mathf.Approximately(holder.tank.Remaining,0f)) {
					continue;
				} else if (holder.tank.Remaining < gas.Total) { //it has some space left in it
					float ratio = holder.tank.Remaining/gas.Total;
					holder.tank.atmosphere.Mass += gas*ratio;
					gas -= gas*ratio;
				} else {
					holder.tank.atmosphere.Mass += gas;
					return new Gases();
				}
			}
		}
		
		return gas;
	}
	
	public Gases Pull(float amount) {
		return new Gases();
	}
}

