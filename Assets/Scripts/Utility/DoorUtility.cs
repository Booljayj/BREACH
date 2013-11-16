//  
//  DoorUtility.cs
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

public class DoorUtility : MonoBehaviour {
	public Door door;
	
	void Start() {
		door.DoorOpening += SetOpen;
		door.DoorClosing += SetClosed;
		transform.position = door.transform.position + Vector3.up*3f;
	}

	void OnMouseDown() {
		door.Toggle();
	}
	
	void SetOpen(Door d) {
		renderer.material.color = Color.green;
	}
	
	void SetClosed(Door d) {
		renderer.material.color = Color.red;
	}
}

