using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GasTankState {
	Empty,
	Normal,
	Full,
}

public class GasTank : MonoBehaviour {
	public Atmosphere atmosphere;	

	public float MaxPressure = 100f; // kPa
	public float Pressure {get {return atmosphere.Pressure;}}; // kPa
	public float Remaining {get {return MaxPressure-atmosphere.Pressure;}}; // kPa


	
	//pull some gas from the tank.
	public AtmospherePacket PullMass (float mass) {
		//TODO Add Function
		return new AtmospherePacket();
	}

	public AtmospherePacket PullVolume (float volume) {
		//TODO Add Function
		return new AtmospherePacket();
	}
	
	//push some gas into the tank.
	public void Push (AtmospherePacket atmos) {
		//TODO Add Function
	}
}

