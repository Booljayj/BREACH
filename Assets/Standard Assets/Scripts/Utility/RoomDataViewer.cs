using UnityEngine;
using System.Collections;

public class RoomDataViewer : MonoBehaviour {
	public Atmosphere atmos;
	public UIBaseLabel pressure;
	public UIBaseLabel temperature;
	
	// Use this for initialization
	void Start () {
		if (atmos == null || pressure == null || temperature == null) {
			Debug.LogWarning("Data Viewer is missing information, shutting down");
			gameObject.SetActive(false);
			return;
		}
	}
	
	// Update is called once per frame
	void Update () {
		pressure.text = atmos.Pressure.ToString("G5");
		temperature.text = atmos.Temperature.ToString("G5");
	}
}