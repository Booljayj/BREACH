//  
//  PanelActivator.cs
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

public class PanelActivator : MonoBehaviour, Activator {
	public Transform camTarget;
	public Vector3 centerOffset;
	public float smoothing;
	
	void Start() {
		if (camTarget == null) gameObject.SetActive(false);
		camTarget.LookAt(transform.position+centerOffset);
	}
	
	public bool Activate(GameObject character, Pointer pointer) {
		StopAllCoroutines();
		Screen.lockCursor = false;
		StartCoroutine("ZoomToPanel", character);
		return true;
	}
	
	public bool ActiveUpdate(GameObject character) {
		if (Input.GetMouseButtonDown(1)) return false;
		return true;
	}
	
	public void Deactivate(GameObject character) {
		StopAllCoroutines();
		Screen.lockCursor = true;
		StartCoroutine("ZoomToHead", character);
	}
	
	IEnumerator ZoomToHead(GameObject character) {
		Transform target = character.transform.FindChild("Head");
		Transform cam = Camera.main.transform;
		while (Vector3.Distance(target.position, cam.position) > .05f && Quaternion.Angle(target.rotation, cam.rotation) > .05f) {
			cam.position = Vector3.Lerp(cam.position, target.position, smoothing*Time.deltaTime);
			cam.rotation = Quaternion.Lerp(cam.rotation, target.rotation, smoothing*Time.deltaTime);
			yield return null;
		}
		character.SendMessage("Freeze", false);
	}
	
	IEnumerator ZoomToPanel(GameObject character) {
		Transform cam = Camera.main.transform;
		character.SendMessage("Freeze", true);
		while (Vector3.Distance(camTarget.position, cam.position) > .05f && Quaternion.Angle(camTarget.rotation, cam.rotation) > .05f) {
			cam.position = Vector3.Lerp(cam.position, camTarget.position, smoothing*Time.deltaTime);
			cam.rotation = Quaternion.Lerp(cam.rotation, camTarget.rotation, smoothing*Time.deltaTime);
			yield return null;
		}
	}
}

