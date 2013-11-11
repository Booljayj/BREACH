using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GasInterchange : MonoBehaviour {	
	Dictionary<GasTankType, List<GasTank>> tanks;
	
	void Start() {
		tanks = new Dictionary<GasTankType, List<GasTank>>();
		tanks.Add(GasTankType.Nitrogen, new List<GasTank>());
		tanks.Add(GasTankType.Oxygen, new List<GasTank>());
		tanks.Add(GasTankType.Carbon, new List<GasTank>());
		tanks.Add(GasTankType.Toxin, new List<GasTank>());
	}
	
	//add a new tank to the interchange
	public void ActivateTank(GasTank tank, GasTankType type) {
		tanks[type].Add(tank);
	}
	
	//remove a tank from the interchange
	public void DeactivateTank(GasTank tank, GasTankType type) {
		tanks[type].Remove(tank);
	}
	
	//process the gas input, modifying it to match the desired amount
	public Gases ProcessGas(Gases input, float amount, GasTankType type) {
		if (type == GasTankType.Null) return new Gases(); //dump all gas, it just disappears.

		foreach (GasTank tank in tanks[type]) {
			if (Mathf.Approximately(input.Total, amount)) {
				break; //close enough to desired amount, just break
			} else if (input.Total > amount) {
				//input = tank.Push(input, amount);
			} else {
				//input = tank.Pull(input, amount);
			}
		}
		
		return input;
	}
}
