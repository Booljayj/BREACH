//
//  DoorProximityTrigger.cs
//
//  Author:
//       Justin Bool <booljayj@gmail.com>
//
//  Copyright (c) 2014 
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

[RequireComponent(typeof(SphereCollider))]
public class DoorProximityTrigger : MonoBehaviour {
	public Door door;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && door.isAutomatic) {
			door.Open();
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player" && door.isAutomatic) {
			door.Close();
		}
	}
}

