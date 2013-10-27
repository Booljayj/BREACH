using UnityEngine;
using System.Collections;

public class Plug : MonoBehaviour {
	public string type = "";
	public PlugHandler handler;
	GameObject connected;
	
	public bool CanConnect(GameObject obj) {
		if (handler == null || handler.Locked()) return false;
		if (obj.tag != type) return false;
		return true;
	}
	
	public void Connect(GameObject obj) {
		handler.Connect(obj);
		connected = obj;
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