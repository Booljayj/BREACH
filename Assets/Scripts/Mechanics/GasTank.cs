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
	public Valve valve;
	public string GasType;
	
	public float Capacity = 100000f; // Pa
	
	public float Remaining {
		get {return Capacity - atmosphere.Pressure;}
	}
	
	void Start () {
		valve = GetComponent<Valve>();
	}
	
	//pull some gas from the tank.
	public AtmospherePacket Pull (float mass) {
		//TODO Add Function
		return new AtmospherePacket();
	}
	
	//push some gas into the tank.
	public void Push (AtmospherePacket atmos) {
		//TODO Add Function
	}
}

