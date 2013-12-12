//  
//  Pointer.cs
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

public class Pointer : MonoBehaviour {
	public float range = 2.0f;
	public Transform head;
	
	Ray ray;
	RaycastHit hit;
	
	Activator activator = null;
	bool activating = false;
	public DelayedActivate delayed;
	public Transform stored;
	
	void Start() {
		if (head == null) head = transform.FindChild("Head");
	}
	
	void Update () {
		if (!activating) {
			//we're not currently using an activator, so actively search for a new one and listen to start activating.
			ray = new Ray(head.position, head.forward);
			if (Physics.Raycast(ray, out hit, range)) {
				activator = (Activator)hit.transform.GetComponent(typeof(Activator));
				
				if (activator != null && Input.GetMouseButtonDown(1)) {
					activating = activator.Activate(gameObject, this); //started a new activator
				} else if (delayed != null && Input.GetMouseButtonDown(1)) {
					activating = delayed(gameObject, this); //we have a delayed action, so do it now
					delayed = null; //reset the delayed action
				}
			}
		} else if (!activator.ActiveUpdate(gameObject)) {
			//we've just stopped activating
			activating = false;
			activator.Deactivate(gameObject);
		}
	}
}
