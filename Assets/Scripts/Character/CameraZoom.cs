//
//  CameraZoom.cs
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

public class CameraZoom : MonoBehaviour {
	public float speed;
	public bool Zooming {get; private set;}

	IEnumerator ZoomCoroutine(Transform target) {
		Zooming = true;
		while (Vector3.Distance(transform.position, target.position) >= .05f || Quaternion.Angle(transform.rotation, target.rotation) >= .05f) {
			transform.position = Vector3.Lerp(transform.position, target.position, speed*Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, speed*Time.deltaTime);
		}
		transform.position = target.position;
		transform.rotation = target.rotation;
		Zooming = false;
		yield return null;
	}
}

