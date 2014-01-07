using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GasInterchange : MonoBehaviour {	
	Dictionary<string, List<Tank>> tanks = new Dictionary<string, List<Tank>>(){
		{"Nitrogen", new List<Tank>()},
		{"Oxygen", new List<Tank>()},
		{"Carbon", new List<Tank>()},
		{"Toxin", new List<Tank>()}};
	
	//connect a new tank to the interchange
	public void AddTank(Tank tank, string type) {
		if (tanks.ContainsKey(type)) {
			if (!tanks[type].Contains(tank)) {
				tanks[type].Add(tank);
			}
		}
	}
	
	//disconnect a tank from the interchange
	public void RemoveTank(Tank tank, string type) {
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