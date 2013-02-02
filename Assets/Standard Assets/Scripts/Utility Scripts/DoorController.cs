using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {
	Door door;
	
	void Start () {
		door = GetComponent<Door>();
		
		SetDoorColor();
	}
	
	void OnMouseDown() {
		if (door.open) {
			door.open = false;
		} else {
			door.open = true;
		}
		
		SetDoorColor();
	}
	
	void SetDoorColor() {
		if (door.open) {
			renderer.material.color = Color.green;
		} else {
			renderer.material.color = Color.red;
		}
	}
}
