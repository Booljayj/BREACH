//
//  Item.cs
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
public class Item : Interactor {
	public enum Type {Tank, Scrubber, Cell};
	public Type itemType;

	public override void Interact(Hands hands) {
		hands.held = this;
		Debug.Log("Picking up "+name);

		foreach (Transform child in transform) {
			if (child.collider != null) {
				Physics.IgnoreCollision(child.collider, hands.transform.parent.collider);
			}
		}
		Debug.Log("Ignoring collision between "+hands.transform.parent.name+" and "+name);
		transform.parent = hands.transform;
		rigidbody.useGravity = false;

		hands.joint.connectedBody = rigidbody;
		hands.joint.SetTargetRotationLocal(Quaternion.identity, transform.localRotation);

//		joint = hands.gameObject.AddComponent<SpringJoint>();
//		joint.spring = 50f;
//		joint.damper = .9f;
//		joint.maxDistance = .01f;
//		joint.connectedBody = rigidbody;
//		joint.connectedAnchor = Vector3.zero;
//		joint.anchor = Vector3.zero;

		hands.next = NextInteract;
	}

	public bool NextInteract(Hands hands) {
		hands.held = null;
		Debug.Log("Dropping "+name);

		if (hands.interactor != null) {
			Socket socket = hands.interactor.GetComponent<Socket>();
			if (socket != null && socket.CanConnectItem(this)) {
				socket.ConnectItem(this);
			}
		}

		foreach (Transform child in transform) {
			if (child.collider != null) {
				Physics.IgnoreCollision(child.collider, hands.transform.parent.collider, false);
			}
		}
		transform.parent = null;
		rigidbody.useGravity = true;

		hands.joint.connectedBody = null;

		rigidbody.AddForce(hands.transform.parent.GetComponent<CharacterController>().velocity + rigidbody.velocity + hands.transform.forward*.2f, ForceMode.VelocityChange);

		return true;
	}

//	void Update() {
//		Debug.Log(rigidbody.velocity.ToString());
//	}
}

