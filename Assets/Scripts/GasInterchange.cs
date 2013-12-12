using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GasInterchange : MonoBehaviour {	
	Dictionary<string, List<GasTank>> tanks = new Dictionary<string, List<GasTank>>(){
		{"Nitrogen", new List<GasTank>()},
		{"Oxygen", new List<GasTank>()},
		{"Carbon", new List<GasTank>()},
		{"Toxin", new List<GasTank>()}};
	
	//connect a new tank to the interchange
	public void AddTank(GasTank tank, string type) {
		if (tanks.ContainsKey(type)) {
			if (!tanks[type].Contains(tank)) {
				tanks[type].Add(tank);
			}
		}
	}
	
	//disconnect a tank from the interchange
	public void RemoveTank(GasTank tank, string type) {
		if (tanks.ContainsKey(type)) {
			if (tanks[type].Contains(tank)) {
				tanks[type].Remove(tank);
			}
		}
	}
	
	//process the gas input, modifying it to match the desired amount
	public void ProcessGas(AtmospherePacket input, float amount, string type) {
	}
}