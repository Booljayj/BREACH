using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Socket : Interactor {
	public Item.Type socketType;
	public Holder holder;
	public Item connectedItem;

	public override void Interact(Hands hands) {
		if (connectedItem != null && hands.held == null) {
			if (holder != null) {
				if (holder.isLocked) return;
				holder.Disconnect();
			}
			hands.held = connectedItem;
			connectedItem = null;
			hands.held.Interact(hands);
		}
	}

	public bool CanConnectItem(Item item) {
		if (holder.isLocked || connectedItem != null || item.itemType != socketType) return false;
		else return true;
	}

	public void ConnectItem(Item item) {
		item.transform.position = transform.position;
		connectedItem = item;
	}
}

public abstract class Holder : MonoBehaviour {
	public event EventHandler Connected;
	public event EventHandler Disconnected;
	public event EventHandler Locked;
	public event EventHandler Unlocked;
	public bool isLocked {get; protected set;}

	public abstract void Connect(Item item);
	public abstract void Disconnect();
	public abstract void Lock();
	public abstract void Unlock();

	protected void OnConnect() {
		if (Connected != null) Connected();
	}
	protected void OnDisconnect() {
		if (Disconnected != null) Disconnected();
	}
	protected void OnLock() {
		if (Locked != null) Locked();
	}
	protected void OnUnlock() {
		if (Unlocked != null) Unlocked();
	}
}