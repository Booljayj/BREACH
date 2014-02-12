using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomViewer : MonoBehaviour {
	public RoomController room;

	public BarMeter[] channels = new BarMeter[4];
	public GuageMeter pressure;
	public GuageMeter temperature;

	List<KeyValuePair<string, float>> gases;
	float pressureValue;
	float temperatureValue;

	void Start() {
		//connect the Refresh method to any changed events in the room.
		if (room == null) {
			enabled = false;
			return;
		}

		StartCoroutine(Refresh());
	}

	//Using update for now, but should switch to an async Refresh method
	IEnumerator Refresh() {
		while (true) {
			channels[0].Value = room.atmosphere.percent["Nitrogen"];
			channels[1].Value = room.atmosphere.percent["Oxygen"];
			channels[2].Value = room.atmosphere.percent["Carbon Dioxide"];
			channels[3].Value = 1f-(room.atmosphere.percent["Nitrogen"]+room.atmosphere.percent["Oxygen"]+room.atmosphere.percent["Carbon Dioxide"]);

			pressure.Value = room.atmosphere.Pressure;
			temperature.Value = room.atmosphere.Temperature;

			yield return new WaitForSeconds(.5f);
		}
	}
}