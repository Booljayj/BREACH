using UnityEngine;
using System.Collections;

public class GasTankHolder : MonoBehaviour {
	public GasInterchange interchange;
	
	public GasTank tank;
	public string type;
	
	bool locked = false;
	
	public bool Locked() {
		if (locked) return true;
		return false;
	}
	public void Connect(GameObject obj) {
		tank = obj.GetComponent<GasTank>();
	}
	public void Disconnect(GameObject obj) {
		tank = null;
	}
	
	//the holder must be locked and filled with a tank in order to activate
	void Activate() {
		if (locked || tank == null) return;
		interchange.AddTank(tank, type);
		locked = true;
	}
	
	//the holder must be locked, filled with a tank, and activated in order to deactivate
	void Deactivate() {
		if (!locked || tank == null) return;
		interchange.RemoveTank(tank, type);
		locked = false;
	}
}