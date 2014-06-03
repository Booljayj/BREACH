using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//An airlock is a mechanism that connects two atmospheres, allowing some amount of heat and mass to flow between them.
//It can represent a door, a hull breach, or even the valve on a gas tank.
public class Airlock : MonoBehaviour {
	public Messenger message;

	#region Control Variables
	public Atmosphere roomA; //first connected atmosphere
	public Atmosphere roomB; //second connected atmosphere
	
	public float area = 4f; //airlock area
	public float C = 1.2f;

	public bool partial;
	public bool full;
	public float partialPercentage = .1f;
	#endregion

	#region Calculation Variables
	Atmosphere A1, A2;

	float Y; //expansion factor
	float T; //difference factor
	float Q;
	AtmospherePacket dV;

	float rhobar;
	float D; //adjusted diffusion factor
	float De; //extended diffusion factor
	Dictionary<string, float> J;

	float E; //combined property factor
	float hbar;
	float Qdot;
	#endregion

	#region Logic
	void Start() {
		if (!message) {
			Debug.LogError("Airlock Requires a Messenger reference");
			return;
		}

		message.Register<AirlockMessage>(ProcessMessage);
	}

	public void ProcessMessage(AirlockMessage m) {
		if (m == AirlockMessage.PartialOpen) {
			partial = true;
		} else if (m == AirlockMessage.PartialClose) {
			partial = false;
		} else if (m == AirlockMessage.FullOpen) {
			full = true;
		} else if (m == AirlockMessage.FullClose) {
			full = false;
		}
	}

	void Update() {
		if (partial || full) {
			if (roomA == null || roomB == null) return;

			if (roomA.Pressure >= roomB.Pressure) {
				A1 = roomA;
				A2 = roomB;
			} else {
				A1 = roomB;
				A2 = roomA;
			}

			//Pressure-dependent processes.
			if (!Mathf.Approximately(A1.Pressure, A2.Pressure)) {
				//calculate the expansion factor Y = 1-(1-r)*(.41+.35 b^4)/k
				Y = 1f - (1f-A2.Pressure/A1.Pressure)*(.2938f);

				//calculate the difference scale factor T = sqrt(1-p2/p1);
				T = Mathf.Sqrt(1-Mathf.Pow(A2.Pressure/A1.Pressure, 4));

				//calculate the volumetric flow rate Q = TCYA*sqrt(2*(P1-P2)/rho).
				Q = T*.6f*Y*area*Mathf.Sqrt(2f*(A1.Pressure-A2.Pressure)/A1.Density);

				//compensate for partial opening
				if (partial && !full) Q = Q*partialPercentage;

				//calculate the dm from pressure differences
				dV = A1.GetVolume(Q*Time.deltaTime);
			} else {
				dV = new AtmospherePacket();
			}

			//Diffusion-dependent mass transfer
			//calculate the average density
			rhobar = (A1.mass + A2.mass)/(A1.volume + A2.volume);

			//calculate the diffusion factor D = Do*T^(3/2)/P
			D = .2636f*2*Mathf.Pow((A1.Temperature + A2.Temperature)/2f, 1.5f)/(A1.Pressure + A2.Pressure);
			De = D*rhobar*area*Time.deltaTime;

			//calculate and add in the diffusion mass flow rate J
			foreach (string key in A1.Keys) {
				dV[key] += A1.percent[key]*De;
			}
			foreach (string key in A2.Keys) {
				dV[key] -= A2.percent[key]*De;
			}
			
			//Temperature-dependent heat transfer
			//calculate the rayleigh factor E = 2.93*(T/50)^-.34
			E = 2.93f*Mathf.Pow((A1.Temperature+A2.Temperature)/100f, -.34f);

			//calculate the convection coefficient. It's about 10 times higher than it should be, for the sake of game flow.
			hbar = E*Mathf.Pow(Mathf.Abs(A1.Temperature-A2.Temperature)/2f, .25f)/1e3f; //divide by 1e6 because heat is stored in MJ, and hbar uses J

			//calculate and add in the convection heat transfer
			Qdot = area*hbar*(A1.Temperature-A2.Temperature)*Time.deltaTime;
			dV.heat += Qdot;

			//Perform the transfer, always from 1 to 2.
			A1.Pull(dV);
			A2.Push(dV);
		}
	}
	#endregion
}
