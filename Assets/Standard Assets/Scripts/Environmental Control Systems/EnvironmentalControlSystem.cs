using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentalControlSystem : MonoBehaviour {
	public List<Atmosphere> rooms;
	
	public List<GasTankHolder> nitrogen; //tanks filled with nitrogen (N2).
	public List<GasTankHolder> oxygen; //tanks filled with oxygen (O2).
	public List<GasTankHolder> carbondioxide; //tanks filled with carbon dioxide (CO2). This is a staging area for the scrubber.
	public List<ScrubberTankHolder> scrubbers;
	
	public float gasExchangeRate; //rate of gas exchange, in kg/s
	public float gasScrubRate; //rate of O2 scrubbing, in kg/s
	public float idealPressure = 80000f; //80 kPa is good for now
	public float idealTemperature = 293f; //20 C is good for now
	public Gases idealPercent = new Gases(.75f, .25f, 0f,0f,0f,0f); //slightly high-O2 envir
	public float tolerance = .05f; //tolerance for ideals
	
	public AnimationCurve intakeCurve;// = new AnimationCurve(new Keyframe(idealPressure, gasExchangeRate));
	public AnimationCurve outputCurve;// = new AnimationCurve(new Keyframe(idealPressure, gasExchangeRate));
	
	private Gases massPacket;
	private Gases idealPacket;
	
	// Use this for initialization
	void Start () {
	}
	
	//TODO: Not functional. Fix that.
	void Update() {
		foreach (Atmosphere atmos in rooms) {
			// ga-damn the solution to this problem is simple. I wasn't thinking about how real-world systems work
			
			//step 1: pull gases from the room. Pull more or less based on the current pressure of the room.
			//massPacket = atmos.GetMassPacket(intakeCurve.Evaluate(atmos.Pressure));
			
			//step 2: throw retrieved gases into the tanks
			nitrogen[0].tank.atmos.Mass.N2 += massPacket.N2;
			oxygen[0].tank.atmos.Mass.O2 += massPacket.O2;
			carbondioxide[0].tank.atmos.Mass.CO2 += massPacket.CO2;
			
			//step 4: prepare output gas packet. Push more of less based on the current pressure of the room.
			idealPacket = idealPercent*outputCurve.Evaluate(atmos.Pressure);
			
			//step 5: fill output packet with available gas pulled from tanks. Tanks may or may not be pure.
			massPacket = new Gases();
			//massPacket += nitrogen[0].tank.atmos.GetMassPacket(idealPacket.N2);
			//massPacket += oxygen[0].tank.atmos.GetMassPacket(idealPacket.O2);
			
			//step 6: give mass packet to room atmosphere
			atmos.Mass += massPacket;
			
			//step 7: mass packet is at ideal temperature, so give that much heat to atmos.
			atmos.Heat += Atmosphere.CalculateHeatCapacity(massPacket)*idealTemperature;
		}
	}
}