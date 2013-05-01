using UnityEngine;
using System.Collections;

//An airlock is a mechanism that connects two atmospheres. It can represent a door, a hull breach, or even the valve on a gas tank.
public class Airlock : MonoBehaviour {
	public Atmosphere RoomA;
	public Atmosphere RoomB;
	public float height = 2f;
	public float width = 2f;
	
	public bool open;
	
	private Atmosphere A1, A2;
	private float Y;
	private float diffFactor; //difference factor
	private float massFlowRate;
	private Gases massPacket;
	private float heatPacket;
	
	void Update() {
		if (open) {
			//if (Mathf.Round(RoomA.Pressure) == Mathf.Round(RoomB.Pressure)) {
			if (!Mathf.Approximately(RoomA.Pressure, RoomB.Pressure)) {
				if (RoomA.Pressure > RoomB.Pressure) {
					A1 = RoomA;
					A2 = RoomB;
				} else {
					A1 = RoomB;
					A2 = RoomA;
				}
			
				//calculate the expansion factor Y = 1-(1-r)*(.41+.35 b^4)/k
				//b is simplified to .25, r is defined as P2/P1
				Y = 1f - (1f-A2.Pressure/A1.Pressure)*(.4114f)/1.4f; //TODO: Simplify this. Now.
			
				//calculate the difference scale factor T = sqrt(1-p2/p1);
				diffFactor = Mathf.Sqrt(1-A2.Pressure/A1.Pressure);
				
				//calculate the mass flow rate m = CYA*sqrt(2*rho*(P1-P2)).
				//C is simplified to .6.
				massFlowRate = diffFactor*.6f*Y*height*width*Mathf.Sqrt(2f*A1.GasDensity*(A1.Pressure-A2.Pressure));
				
				//take some mass from A1, move it to A2
				A1.PullMassPacket(massFlowRate * Time.deltaTime, out massPacket, out heatPacket);
				A2.PushMassPacket(massPacket, heatPacket);
			}
			
			//perform convective heat transfer, regardless of pressure situation
			//heatPacket = .3*height*width*(A1.Temperature-A2.Temperature);
			
			Debug.Log("Transfering "+massFlowRate.ToString()+" kg from room "+A1.name);
		}
	}
}