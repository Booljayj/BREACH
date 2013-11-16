using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GasInterchange : MonoBehaviour {	
	Dictionary<string, List<GasTank>> tanks;
	
	void Start() {
		tanks = new Dictionary<string, List<GasTank>>();
		tanks.Add("Nitrogen", new List<GasTank>());
		tanks.Add("Oxygen", new List<GasTank>());
		tanks.Add("Carbon", new List<GasTank>());
		tanks.Add("Toxin", new List<GasTank>());
	}
	
	//add a new tank to the interchange
	public void ActivateTank(GasTank tank, string type) {
		tanks[type].Add(tank);
	}
	
	//remove a tank from the interchange
	public void DeactivateTank(GasTank tank, string type) {
		tanks[type].Remove(tank);
	}
	
	//process the gas input, modifying it to match the desired amount
	public AtmospherePacket ProcessGas(AtmospherePacket input, float amount, string type) {
		return new AtmospherePacket();
	}
}
