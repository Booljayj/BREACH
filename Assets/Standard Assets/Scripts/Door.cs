using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	public Room RoomA;
	public Room RoomB;
	public float Area; //m^2 the approximate area of the door;
	
	public bool open;
	
	private float massFlow;
	private Gases massPacket;
	private float heatFlow;
	
	// Update is called once per frame
	void Update () {
		if (open) {
			if (Mathf.Abs(RoomA.Pressure-RoomB.Pressure) < .001f) {
				//pure diffusion and convective heat transfer
				//Debug.Log("Pressure Equalized");
				
			} else if (RoomA.Pressure > RoomB.Pressure) {
				//pressure in A is higher, so mass flows from A to B
				massFlow = (RoomA.Pressure-RoomB.Pressure)*RoomA.GasDensity*Area;
				
				massPacket = RoomA.Percent*massFlow*Time.deltaTime/1000f;
				
				//heatFlow = 0;
				heatFlow = (RoomA.Temperature-RoomB.Temperature)*massPacket.Total*.001006f;
				
				//transfer mass and heat between the rooms
				//Debug.Break();
				RoomA.Mass -= massPacket;
				RoomB.Mass += massPacket;
				RoomA.Heat -= heatFlow;
				RoomB.Heat += heatFlow;
				
			} else if (RoomB.Pressure > RoomA.Pressure) {
				//pressure in B is higher, so mass flows from B to A
				massFlow = (RoomB.Pressure-RoomA.Pressure)*RoomB.GasDensity*Area;
				
				massPacket = RoomB.Percent*massFlow*Time.deltaTime/1000f;
				
				//heatFlow = 0;
				heatFlow = (RoomB.Temperature-RoomA.Temperature)*massPacket.Total*.001006f;
				
				//transfer mass and heat between the rooms
				//Debug.Break();
				RoomB.Mass -= massPacket;
				RoomA.Mass += massPacket;
				RoomB.Heat -= heatFlow;
				RoomA.Heat += heatFlow;
			}
			
			//perform convective heat transfer from A to B using a simple value for h
			if (Mathf.Abs(RoomA.Temperature-RoomB.Temperature) > .001f) {
				heatFlow = (RoomA.Temperature-RoomB.Temperature)*Area*.01f*Time.deltaTime;
				RoomA.Heat -= heatFlow;
				RoomB.Heat += heatFlow;
			}
		}
	}
}
