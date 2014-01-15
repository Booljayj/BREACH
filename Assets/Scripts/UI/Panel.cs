//
//  Panel.cs
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

[RequireComponent(typeof(Collider))]
public class Panel : Activator {
	public Transform camTarget;
	public Vector3 lookOffset;
	public float smoothing = 1f;

	void Start() {
		camTarget.LookAt(transform.position+transform.rotation*lookOffset);
	}

	public override void Activate(Hands hands) {
		StopAllCoroutines();
		Debug.Log("Zooming to panel");
		StartCoroutine(ZoomTo(camTarget, hands.transform.parent.gameObject, true));
		hands.next = NextActivate;
	}

	public bool NextActivate(Hands hands) {
		StopAllCoroutines();
		Debug.Log("Zooming to head");
		StartCoroutine(ZoomTo(hands.head, hands.transform.parent.gameObject, false));
		return true;
	}

	IEnumerator ZoomTo(Transform target, GameObject player, bool freeze) {
		Transform cam = Camera.main.transform;

		player.SendMessage("Freeze", true);

		while (Vector3.Distance(target.position, cam.position) > .05f || Quaternion.Angle(target.rotation, cam.rotation) > .05f) {
			cam.position = Vector3.Lerp(cam.position, target.position, smoothing*Time.deltaTime);
			cam.rotation = Quaternion.Lerp(cam.rotation, target.rotation, smoothing*Time.deltaTime);
			yield return null;
		}
		cam.position = target.position;
		cam.rotation = target.rotation;

		if (!freeze)
			player.SendMessage("Freeze", false);
	}

	void OnGizmosSelected() {
		Gizmos.DrawSphere(camTarget.position, .01f);
		Gizmos.DrawFrustum(camTarget.position, Camera.main.fieldOfView, Vector3.Distance(camTarget.position, transform.position), Camera.main.nearClipPlane, Camera.main.aspect);
		Gizmos.DrawLine(camTarget.position, transform.position+transform.rotation*lookOffset);
	}
}

