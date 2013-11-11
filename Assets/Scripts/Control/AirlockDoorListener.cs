using UnityEngine;
using System.Collections;

public class AirlockDoorListener : MonoBehaviour {
	Door door;
	Airlock airlock;

	void Start () {
		door = GetComponent<Door>();
		airlock = GetComponent<Airlock>();
		
		door.DoorOpening += OpenAirlock;
		door.DoorClosed += CloseAirlock;
	}
	
	void OpenAirlock(Door d) {
		airlock.open = true;
	}
	
	void CloseAirlock(Door d) {
		airlock.open = false;
	}
}
