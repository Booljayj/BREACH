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
				if (holder.isOpen) return;
				holder.Disconnect();
			}
			hands.held = connectedItem;
			connectedItem = null;
			hands.held.Interact(hands);
		}
	}

	public bool CanConnectItem(Item item) {
		if (!holder.isOpen || connectedItem != null || item.itemType != socketType) return false;
		else return true;
	}

	public void ConnectItem(Item item) {
		item.transform.position = transform.position;
		connectedItem = item;
	}
}

public abstract class Holder : MonoBehaviour {
	public event EventHandler Closed;
	public event EventHandler Open;
	public bool isOpen {get; protected set;}
	
	public event EventHandler Inactive;
	public event EventHandler Active;
	public bool isActive {get; protected set;}

	public abstract void Connect(Item item);
	public abstract void Disconnect();
	public abstract void Activate();
	public abstract void Deactivate();
}