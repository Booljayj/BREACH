//  
//  Interaction.cs
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

public class Interaction : MonoBehaviour {
	public Transform head;
	public float range;
	public float zoomSpeed;
	public LayerMask interactionLayer;

	GameObject held;
	Plug plug;
	Item item;
	Panel panel;

	Ray ray;
	RaycastHit hit;
	bool zoomed;
	bool working = false;

	void Update() {
		if (!Input.GetMouseButtonDown(1) || working) return;

		if (zoomed) {
			zoomed = false;
			StopAllCoroutines();
			StartCoroutine(ZoomTo(head, false));
		} else {
			ray = new Ray(head.position, head.forward);
			Physics.Raycast(ray, out hit, range, interactionLayer.value);
			
			if (held != null) { //we are holding something
				if (hit.collider != null && IsPlug(hit.transform) && plug.CanConnect(held.GetComponent<Item>())) {
					//attempt to connect the held item
				} else {
					//drop the held item
				}
			} else {
				if (hit.collider != null && IsPlug(hit.transform) && plug.CanDisconnect()) { //we clicked on a plug, which has an item
					//attempt to disconnect the plug
				} else if (hit.collider != null && IsItem(hit.transform)) { //we clicked on a free item, so pick it up
					//attempt to pick up the item
				} else if (hit.collider != null && IsPanel(hit.transform)) { //zooming in to a panel
					zoomed = true;
					StopAllCoroutines();
					StartCoroutine(ZoomTo(panel.camTarget, true));
				}
			}
		}
	}

	bool IsPlug(Transform trans) {
		plug = trans.GetComponent<Plug>();
		if (plug != null) return true;
		return false;
	}

	bool IsPanel(Transform trans) {
		panel = trans.GetComponent<Panel>();
		if (panel != null) return true;
		return false;
	}

	bool IsItem(Transform trans) {
		item = trans.GetComponent<Item>();
		if (item != null) return true;
		return false;
	}

	IEnumerator ZoomTo(Transform target, bool freeze) {
		SendMessage("Freeze", true);
		while(Vector3.Distance(transform.position, target.position) > .05f || Quaternion.Angle(transform.rotation, target.rotation) > .05f) {
			transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime*zoomSpeed);
			transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime*zoomSpeed);
			yield return null;
		}
		transform.position = target.position;
		transform.rotation = target.rotation;
		if (!freeze) SendMessage("Freeze", false);
	}

	IEnumerator PickItem(Item item) {
		yield return null;
	}

	IEnumerator DropItem(Item item) {
		yield return null;
	}

	IEnumerator PlugConnect(Plug plug) {
		held.transform.position = plug.transform.position;
		plug.Connect(held);
		yield return null;
	}

	IEnumerator PlugDisconnect(Plug plug) {
		yield return null;
	}
}

