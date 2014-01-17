using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GasInterchange : MonoBehaviour {
	public List<string> channelNames = new List<string>(){
		"Nitrogen",
		"Oxygen",
		"Carbon",
		"Toxin"};

	Dictionary<string, List<Tank>> channels = new Dictionary<string, List<Tank>>();

	void Start() {
		foreach (string name in channelNames) {
			channels.Add(name, new List<Tank>());
		}
	}
	
	//connect a new tank to the interchange
	public void AddTank(Tank tank, string channel) {
		if (channels.ContainsKey(channel)) {
			if (!channels[channel].Contains(tank)) {
				channels[channel].Add(tank);
			}
		}
	}
	
	//disconnect a tank from the interchange
	public void RemoveTank(Tank tank, string channel) {
		if (channels.ContainsKey(channel)) {
			if (channels[channel].Contains(tank)) {
				channels[channel].Remove(tank);
			}
		}
	}
	
	//process the gas input, modifying it to match the desired amount
	public void ProcessGas(AtmospherePacket input, float amount, string channel) {
	}
}