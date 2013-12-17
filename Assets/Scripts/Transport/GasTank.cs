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

	public float MaxPressure = 200f; // kPa
	public float Pressure {get {return atmosphere.Pressure;}} // kPa
	public float Remaining {get {return MaxPressure-atmosphere.Pressure;}} // kPa

	//Events Here

	//pull some gas from the tank.
	public AtmospherePacket PullMass (float mass) {
		AtmospherePacket Ap = atmosphere.GetMass(mass);
		atmosphere.Pull(Ap);

		return Ap;
	}

	public AtmospherePacket PullVolume (float volume) {
		AtmospherePacket Ap = atmosphere.GetVolume(volume);
		atmosphere.Pull(Ap);

		return Ap;
	}
	
	//push some gas into the tank, up to maxpressure.
	public void Push (AtmospherePacket atmos) {
		//AtmospherePacket combined = atmos+atmosphere;
	}
}

