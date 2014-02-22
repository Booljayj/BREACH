using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Socket : Interactor {
	public Item.Type socketType;
	public Item connectedItem;

	public event ItemHandler Connected;
	public event EventHandler Disconnected;

	public bool isLocked {get; set;}

	public override void Interact(Hands hands) {
		if (!isLocked && connectedItem) {
			Debug.Log("Disconnecting");
			connectedItem.Interact(hands);
			DisconnectItem(hands);
		}
	}

	public bool CanConnectItem(Item item) {
		if (connectedItem && item.itemType != socketType) {
			return false;
		}
		return true;
	}

	public void ConnectItem(Item item) {
		if (isLocked) return;

		connectedItem = item;
		connectedItem.rigidbody.isKinematic = true;

		TweenPosition.Begin(connectedItem.gameObject, .2f, transform.position).method = UITweener.Method.EaseInOut;
		UITweener tween = TweenRotation.Begin(connectedItem.gameObject, .2f, transform.rotation);

		tween.eventReceiver = gameObject;
		tween.callWhenFinished = "OnConnected";
	}

	public void DisconnectItem(Hands hands) {
		if (isLocked) return;

		connectedItem.Interact(hands);
		connectedItem.rigidbody.isKinematic = true;
		connectedItem = null;

		OnDisconnected();
	}

	public void OnConnected() {
		if (Connected != null) Connected(connectedItem);
	}

	public void OnDisconnected() {
		if (Disconnected != null) Disconnected();
	}
}

public abstract class Holder : MonoBehaviour {
	public Socket socket;

	public event EventHandler Opening;
	public event EventHandler Opened;
	public event EventHandler Closing;
	public event EventHandler Closed;
	
	public event EventHandler Activated;
	public event EventHandler Deactivated;

	protected bool _isActivated;
	public bool isActivated {
		get {
			return _isActivated;
		}
		protected set {
			if (_isActivated != value) {
				_isActivated = value;
				if (_isActivated) OnActivate();
				if (!_isActivated) OnDeactivate();
			}
		}
	}

	public abstract void Connect(Item item);
	public abstract void Disconnect();

	public abstract void Close();
	public abstract void Open();
	public abstract void Activate();
	public abstract void Deactivate();
	
	public void OnOpening() {
		if (Opening != null) Opening();
	}
	
	public void OnOpened() {
		if (Opened != null) Opened();
		socket.isLocked = false;
	}
	
	public void OnClosing() {
		if (Closing != null) Closing();
		socket.isLocked = true;
	}
	
	public void OnClosed() {
		if (Closed != null) Closed();
	}
	
	public void OnActivate() {
		if (Activated != null) Activated();
	}
	
	public void OnDeactivate() {
		if (Deactivated != null) Deactivated();
	}
}