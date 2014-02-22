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

public delegate void ItemHandler(Item item);

[RequireComponent(typeof(Rigidbody))]
public class Item : Interactor {
	public enum Type {Tank, Scrubber, Cell};
	public Type itemType;

	public override void Interact(Hands hands) {
		hands.held = this;
//		Debug.Log("Picking up "+name);

		transform.parent = hands.transform;
		rigidbody.useGravity = false;

		hands.joint.connectedBody = rigidbody;
		hands.joint.SetTargetRotationLocal(Quaternion.identity, transform.localRotation);
		
		IgnoreCollision(hands.transform.parent.gameObject, true);

		hands.next = NextInteract;
	}

	public bool NextInteract(Hands hands) {
		hands.held = null;
//		Debug.Log("Dropping "+name);

		transform.parent = null;
		rigidbody.useGravity = true;

		hands.joint.connectedBody = null;
		rigidbody.AddForce(hands.transform.parent.GetComponent<CharacterController>().velocity + rigidbody.velocity + hands.transform.forward*.2f, ForceMode.VelocityChange);

		if (hands.interactor != null) {
			Socket socket = hands.interactor.GetComponent<Socket>();
			if (socket != null && socket.CanConnectItem(this)) {
				socket.ConnectItem(this);
			}
			IgnoreCollision(hands.transform.parent.gameObject, false);
		} else {
			IgnoreCollision(hands.transform.parent.gameObject, false);
		}

		return true;
	}

	public void IgnoreCollision(GameObject obj, bool on) {
		foreach (Transform child in transform) {
			if (child.collider) {
				Physics.IgnoreCollision(child.collider, obj.collider, on);
			}
		}
	}
}

