//
//  Hands.cs
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

[RequireComponent(typeof(Rigidbody))]
public class Hands : MonoBehaviour {
	public Transform head; //the transform used to point the raycast;
	public float distance = 1.5f; //the raycast/interactin distance.
	public LayerMask activatorLayer; //the layer that all activators are on.

	public delegate bool NextHandler(Hands hands);
	public NextHandler next;
	
	public Activator activator {get; private set;} //the current activator, can be null
	public Item held {get; private set;} //the currently held item, can be null

	RaycastHit hit;
	Ray ray;

	void Update() {
		if (!Input.GetMouseButtonDown(1)) return;

		//perform a raycast, set the hit object as inter
		ray = new Ray(head.position, head.forward);
		if (Physics.Raycast(ray, out hit, distance, activatorLayer.value)) {
			activator = hit.transform.GetComponent<Activator>();
		} else {
			activator = null;
		}

		if (next != null) { //is there an action queued up?
			if (next(this)) //perform the action, remove it if it returns true;
				next = null;
		} else if (activator != null) { //activate the activator
			activator.Activate(this);
		}
	}
}

public abstract class Activator : MonoBehaviour {
	public abstract void Activate(Hands hands);
}