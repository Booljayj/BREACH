using UnityEngine;
using System.Collections;

public class Airlock : MonoBehaviour {
	public Room room;
	public float Area;
	
	public bool open;
	
	// Update is called once per frame
	void Update () {
		if (open) {
			//TODO: Change this whole thing to match the door class' implementation
			
			//Calculate the mass flow rate out into space. Only uses static pressure. We'll use a pressure of 1e-5 for space, so that the calculation cuts of after a certain point.
			//Just make sure that no mass is flowing back into the room.
			Gases gasPacket = new Gases();
			float massFlow;
						
			//mass_flow_rate = A*(P_b-P_a)
			massFlow = -Area*(room.Pressure-.00001f);
				
			if (massFlow > 0) {
				return; //stop before accidentally putting mass into the system.
			}
			gasPacket = room.Percent*massFlow*Time.deltaTime/1000f;
				
			room.Mass += gasPacket;
		}
	}
}

