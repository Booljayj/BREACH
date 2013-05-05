using UnityEngine;
using System.Collections;

//An airlock is a mechanism that connects two atmospheres. It can represent a door, a hull breach, or even the valve on a gas tank.
public class Airlock : MonoBehaviour {
	public Atmosphere RoomA; //first connected atmosphere
	public Atmosphere RoomB; //second connected atmosphere
	public float height = 2f; //door height in m
	public float width = 2f; //door width in m
	
	public bool open;
	
	private Atmosphere A1, A2;
	private float Y;
	private float T; //difference factor
	private float mdot;
	private Gases dm;
	private float dQ;
	
	void Update() {
		if (open) {
			//if (Mathf.RoundToInt(RoomA.Pressure) != Mathf.RoundToInt(RoomB.Pressure)) {
			if (!Mathf.Approximately(RoomA.Pressure, RoomB.Pressure)) {
				if (RoomA.Pressure > RoomB.Pressure) {
					A1 = RoomA;
					A2 = RoomB;
				} else {
					A1 = RoomB;
					A2 = RoomA;
				}
			
				//calculate the expansion factor Y = 1-(1-r)*(.41+.35 b^4)/k
				Y = 1f - (1f-A2.Pressure/A1.Pressure)*(.4114f)/1.4f; //TODO: Simplify this. Now.
			
				//calculate the difference scale factor T = sqrt(1-p2/p1);
				T = Mathf.Sqrt(1-Mathf.Pow(A2.Pressure/A1.Pressure, 4));
				
				//calculate the mass flow rate m = CYA*sqrt(2*rho*(P1-P2)).
				mdot = T*.6f*Y*height*width*Mathf.Sqrt(2f*A1.GasDensity*(A1.Pressure-A2.Pressure));
				
				//take some mass from A1, move it to A2
				A1.PullMassPacket(mdot * Time.deltaTime, out dm, out dQ);
				A2.PushMassPacket(out dm, out dQ);
				
				Debug.Log("Transfering "+massFlowRate.ToString()+" kg from room "+A1.name);
			}
			
			//perform convective heat transfer, regardless of pressure situation
			//heatPacket = .3*height*width*(A1.Temperature-A2.Temperature);
		}
	}
}