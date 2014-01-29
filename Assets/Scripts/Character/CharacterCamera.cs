//  
//  TwoPartCamera.cs
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

public class CharacterCamera : MonoBehaviour {
	public Transform body;
	public Transform head;
	
	public float bodySensitivity = 2f;
	public float headSensitivity = 2f;
	
	Transform cam;
	bool frozen;
	
	void Start () {
		cam = Camera.main.transform;
		Screen.lockCursor = true;
	}
	
	void LateUpdate () {
		if (frozen) return;
		
		body.Rotate(Vector3.up, Input.GetAxis("Mouse X")*bodySensitivity);
		head.Rotate(Vector3.right, -Input.GetAxis("Mouse Y")*headSensitivity);
		
		cam.position = head.position;
		cam.rotation = head.rotation;
	}
	
	public void Freeze(bool ifrozen) {
		if (ifrozen) Screen.lockCursor = false;
		else Screen.lockCursor = true;

		frozen = ifrozen;
	}
}

