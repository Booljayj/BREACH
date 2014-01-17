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

	SpringJoint joint;

	public override void Interact(Hands hands) {
		if (hands.held != null) return; //this shouldn't ever happen. sanity check

		Physics.IgnoreCollision(collider, hands.transform.parent.collider);
		transform.parent = hands.transform;
		rigidbody.useGravity = false;

		joint = gameObject.AddComponent<SpringJoint>();
		joint.spring = 50f;
		joint.damper = .9f;
		joint.maxDistance = .01f;
		joint.connectedBody = hands.rigidbody;
		joint.connectedAnchor = Vector3.zero;
		joint.anchor = Vector3.zero;

		hands.next = NextInteract;
	}

	public bool NextInteract(Hands hands) {
		if (hands.interactor != null) {
			Socket socket = hands.interactor.GetComponent<Socket>();
			if (socket != null && socket.CanConnectItem(this)) {
				socket.ConnectItem(this);
			}
		}

		Physics.IgnoreCollision(collider, hands.transform.parent.collider, false);
		transform.parent = null;
		rigidbody.useGravity = true;

		Destroy (joint);

		return true;
	}
}

