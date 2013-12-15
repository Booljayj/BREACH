using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomViewer : MonoBehaviour {
	public RoomController room;

	//create header, add reference here

	public PercentageBar[] channels = new PercentageBar[5];

	//public Guage pressure;
	//public Guage temperature;
	UISprite pressureNeedle;
	UILabel pressureLabel;
	UISprite temperatureNeedle;
	UILabel temperatureLabel;

	//formatting
	float minPressureValue = 50f; // kPa
	float stepPressureValue = -1.8f; // deg/kPa

	float minTemperatureValue = 200f; // K
	float stepTemperatureValue = -1.8f; // deg/K

	//storage
	float channelValue;
	List<KeyValuePair<string, float>> gases;
	float pressureValue;
	float temperatureValue;

	void Start() {
		//connect the Refresh method to any changed events in the room.

		//control
	}

	//Using update for now, but should switch to an async Refresh method
	void Update() {
		//update room status here

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

//		pressureValue = room.atmosphere.Pressure;
//		pressureNeedle.transform.rotation = Quaternion.Euler(0,0, Mathf.Clamp((pressureValue - minPressureValue)*stepPressureValue, -180, 0));
//		pressureLabel.text = string.Format("{0:F1} kPa", pressureValue);
//
//		temperatureValue = room.atmosphere.Temperature;
//		temperatureNeedle.transform.localRotation = Quaternion.Euler(0,0, Mathf.Clamp((temperatureValue - minTemperatureValue)*stepTemperatureValue, -180, 0));
//		temperatureLabel.text = string.Format("{0:F1} K", temperatureValue);
	}
}