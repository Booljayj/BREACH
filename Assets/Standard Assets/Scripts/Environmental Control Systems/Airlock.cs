using UnityEngine;
using System.Collections;

//An airlock is a mechanism that connects two atmospheres. It can represent a door, a hull breach, or even the valve on a gas tank.
public class Airlock : MonoBehaviour {
	public Atmosphere RoomA; //first connected atmosphere
	public Atmosphere RoomB; //second connected atmosphere
	public float height = 2f; //door height in m
	public float width = 2f; //door width in m
	
	public bool open;
	public bool convec = false;
	
	//===Calculation variables
	private Atmosphere A1, A2;
	
	private float Y;
	private float T; //difference factor
	private float mdot;
	
	private float rhobar;
	private float D;
	private Gases J;
	
	private float E;
	private float hbar;
	private float Qdot;
	
	private Gases dm, dm2, dm1;
	private float dQ;
	
	void Update() {
		if (open) {
			//Pressure-dependent processes.
			//if (!Mathf.Approximately(RoomA.Pressure, RoomB.Pressure)) {
			if (Mathf.RoundToInt(RoomA.Pressure) != Mathf.RoundToInt(RoomB.Pressure)) {
				if (RoomA.Pressure > RoomB.Pressure) {
					A1 = RoomA;
					A2 = RoomB;
				} else {
					A1 = RoomB;
					A2 = RoomA;
				}
			
				//===
				//calculate the expansion factor Y = 1-(1-r)*(.41+.35 b^4)/k
				Y = 1f - (1f-A2.Pressure/A1.Pressure)*(.2938f);
			
				//calculate the difference scale factor T = sqrt(1-p2/p1);
				T = Mathf.Sqrt(1-Mathf.Pow(A2.Pressure/A1.Pressure, 4));
				
				//calculate the mass flow rate m = TCYA*sqrt(2*rho*(P1-P2)).
				mdot = T*.6f*Y*height*width*Mathf.Sqrt(2f*A1.GasDensity*(A1.Pressure-A2.Pressure));
				
				//calculate the dm from pressure differences
				dm = A1.Percent*mdot*Time.deltaTime;
				
			} else {
				A1 = RoomA;
				A2 = RoomB;
				
				dm = new Gases();
			}
			
			//Temperature-dependent processes.
			//if (!Mathf.Approximately(A1.Temperature, A2.Temperature)) {
			//}
			
				
			//===
			//calculate the average density
			rhobar = (A1.Mass.Total + A2.Mass.Total)/(A1.Volume + A2.Volume);
			
			//calculate the diffusion factor
			D = .2636f*2*Mathf.Pow((A1.Temperature + A2.Temperature)/2f, 1.5f)/(A1.Pressure + A2.Pressure);
			
			//calculate and add in the diffusion mass flow rate J
			dm = dm + (A1.Percent - A2.Percent)*height*width*rhobar*D*Time.deltaTime;
			
			//===
			//separate dm into dm2 and dm1
			dm2 = new Gases(dm.N2 > 0 ? dm.N2 : 0, dm.O2 > 0 ? dm.O2 : 0, dm.CO2 > 0 ? dm.CO2 : 0,
							dm.CO > 0 ? dm.CO : 0, dm.CH4 > 0 ? dm.CH4 : 0, dm.NOX > 0 ? dm.NOX : 0);
			dm1 = new Gases(dm.N2 < 0 ? -dm.N2 : 0, dm.O2 < 0 ? -dm.O2 : 0, dm.CO2 < 0 ? -dm.CO2 : 0,
							dm.CO < 0 ? -dm.CO : 0, dm.CH4 < 0 ? -dm.CH4 : 0, dm.NOX < 0 ? -dm.NOX : 0);
			
			//get available mass from each atmosphere
			dm2 = A1.GetGases(dm2);
			dm1 = A2.GetGases(dm1);
	
			//===
			//calculate the mass-driven heat transfer
			dQ = dm2.Total*A1.Heat/A1.Mass.Total - dm1.Total*A2.Heat/A2.Mass.Total;
			
			//===
			//calculate the rayleigh factor E
			E = 2.93f*Mathf.Pow((A1.Temperature+A2.Temperature)/100f, -.34f);
			
			//calculate the convection coefficient
			hbar = E*Mathf.Pow(Mathf.Abs(A1.Temperature-A2.Temperature)/height, .25f);
			
			//calculate and add in the convection heat transfer
			if (convec) {
				dQ = dQ + height*width*hbar*(A1.Temperature-A2.Temperature)*Time.deltaTime;
			}
			
			//restrict Q to what is available
			if (dQ > 0) {
				dQ = A1.GetHeat(dQ);
			} else {
				dQ = -A2.GetHeat(-dQ);
			}
			
			//===
			//take some mass from A1, move it to A2
			dm = dm2-dm1;
			A1.Mass -= dm;
			A2.Mass += dm;
			
			A1.Heat -= dQ;
			A2.Heat += dQ;
			
			Debug.Log("Transfering "+dm.Total.ToString()+" kg from room "+A1.name);
		}
	}
}