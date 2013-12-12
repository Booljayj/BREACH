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
	/*
	 * When right click is pressed:
	 * if the character is zoomed into a panel, zoom out
	 * if the character is facing a panel, zoom in
	 * if the character is facing a plug, attempt to use the plug
	 * if the character is facing an item, attempt to pick it up
	 * if the character is holding an item, drop it
	 */
	public Transform pointer;
	public float range;
	public float smoothing = 2f;
	
	public string panelTag;
	public string plugTag;
	public string itemTag;
	
	Ray ray;
	RaycastHit hit;
	Transform hitObj;
	enum Facing {None, Panel, Plug, Item};
	Facing facing;
	
	Transform held;
	bool zoomed;
	
	void Update() {
		ray = new Ray(pointer.position, pointer.forward);
		if (Physics.Raycast(ray, out hit, range)) {
			if (hit.transform.tag == panelTag)
				facing = Facing.Panel;
			else if (hit.transform.tag == plugTag)
				facing = Facing.Plug;
			else if (hit.transform.tag == itemTag)
				facing = Facing.Item;
			else
				facing = Facing.None;
		}
		
		if (Input.GetMouseButtonDown(1)) {
			if (zoomed) {
				zoomed = false;
				StopAllCoroutines();
				StartCoroutine("ZoomToHead", pointer);
			} else if (facing == Facing.Panel) {
				zoomed = true;
				PanelInteractor panel = hitObj.GetComponent<PanelInteractor>();
				StopAllCoroutines();
				StartCoroutine("ZoomToPanel", panel.camTarget);
				//get the panel component
				//stop all coroutines
				//start coroutine zoomtopanel, panel.camtarget
			} else if (facing == Facing.Plug) {
			} else if (facing == Facing.Item) {
			}
		}
	}
	
	IEnumerator ZoomToHead(Transform target) {
		Transform cam = Camera.main.transform;
		while (Vector3.Distance(target.position, cam.position) > .05f && Quaternion.Angle(target.rotation, cam.rotation) > .05f) {
			cam.position = Vector3.Lerp(cam.position, target.position, smoothing*Time.deltaTime);
			cam.rotation = Quaternion.Lerp(cam.rotation, target.rotation, smoothing*Time.deltaTime);
			yield return null;
		}
		SendMessage("Freeze", false);
	}
	
	IEnumerator ZoomToPanel(Transform target) {
		Transform cam = Camera.main.transform;
		SendMessage("Freeze", true);
		while (Vector3.Distance(target.position, cam.position) > .05f && Quaternion.Angle(target.rotation, cam.rotation) > .05f) {
			cam.position = Vector3.Lerp(cam.position, target.position, smoothing*Time.deltaTime);
			cam.rotation = Quaternion.Lerp(cam.rotation, target.rotation, smoothing*Time.deltaTime);
			yield return null;
		}
	}
}

