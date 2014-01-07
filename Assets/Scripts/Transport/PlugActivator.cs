using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Plug : MonoBehaviour {
	public Item.ItemType type;
	public PlugHandler handler;
	GameObject connected;
	
	public bool CanConnect(Item item) {
		if (handler == null || handler.Locked()) return false;
		if (item.type != type) return false;
		return true;
	}
	public void Connect(GameObject obj) {
		connected = obj;
		handler.Connect(connected);
	}

	public bool CanDisconnect() {
		if (handler == null || handler.Locked()) return false;
		if (connected == null) return false;
		return true;
	}
	public GameObject Disconnect() {
		handler.Disconnect(connected);
		return connected;
	}
}

public interface PlugHandler {
	bool Locked(); //returns whether this plug can be accessed
	void Connect(GameObject plug); //called when an object is plugged in
	void Disconnect(GameObject plug); //called when an object is unplugged
}