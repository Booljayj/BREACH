using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Socket : Interactor {
	public Item.Type socketType;
	public Holder holder;
	public Item connected;

	public override void Interact(Hands hands) {
		if (connected != null && hands.held == null) {
			if (holder != null && !holder.locked) {
				holder.Disconnect();
				hands.held = connected;
				connected = null;
				hands.held.Interact(hands);
			}
		}
	}

	public bool CanConnectItem(Item item) {
		if (holder.locked || connected != null || item.itemType != socketType) return false;
		else return true;
	}

	public void ConnectItem(Item item) {
		item.transform.position = transform.position;
		connected = item;
	}
}

public abstract class Holder : MonoBehaviour {
	public abstract void Connect(Item item);
	public abstract void Disconnect();

	public bool locked {get; protected set;}
	public abstract void Lock();
	public abstract void Unlock();
}