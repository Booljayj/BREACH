using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomViewer : MonoBehaviour {
	public RoomController room;

	public BarMeter[] channels = new BarMeter[5];
	public GuageMeter pressure;
	public GuageMeter temperature;

	List<KeyValuePair<string, float>> gases;
	float pressureValue;
	float temperatureValue;

	void Start() {
		//connect the Refresh method to any changed events in the room.
	}

	//Using update for now, but should switch to an async Refresh method
	void Update() {
		gases = room.atmosphere.percent.ToList(); //get list of gas percentage values
		gases.Sort((current,next)=>{return next.Value.CompareTo(current.Value);}); //sort by percentage
		for (int i = 0; i < channels.Length; i++) {
			if (i >= gases.Count)
				channels[i].gameObject.SetActive(false);
			else {
				channels[i].gameObject.SetActive(true);
				channels[i].Name = Properties.Get(gases[i].Key).Abb;
				channels[i].Value = gases[i].Value;
			}
		}

		pressure.Value = room.atmosphere.Pressure;
		temperature.Value = room.atmosphere.Temperature;
	}
}