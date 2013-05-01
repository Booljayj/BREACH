using UnityEngine;
using System.Collections;

public enum GasTankState {
	Empty,
	Normal,
	Full,
}

public class GasTank : MonoBehaviour {
	public Atmosphere atmos;
	
	public GasType gastype;
	public float maxCapacity;
	
	public bool pure;
	public GasTankState tankState;
	
	// Use this for initialization
	void Start () {
		atmos = GetComponent<Atmosphere>();
	}
	
	// Update is called once per frame
	void Update () {
		pure = (atmos.Percent[gastype] >= .9999f) ? true : false;
		
	}
	
	//request to push mass into tank. Return amount that does not fit.
	public float PushGas(float amount, GasType type) {
		if (atmos.Mass.Total + amount > maxCapacity) {
			float topoff = maxCapacity - atmos.Mass.Total;
			atmos.Mass[type] += topoff;
			tankState = GasTankState.Full;
			return amount - topoff;
		} else {
			atmos.Mass[type] += amount;
			tankState = GasTankState.Normal;
			return 0;
		}
	}
	
	//request to pull mass from tank. Return amount available.
	public float PullGas(float amount, GasType type) {
		if (atmos.Mass[type] < amount) {
			float ret = atmos.Mass[type];
			atmos.Mass[type] = 0;
			tankState = GasTankState.Empty;
			return ret;
		} else {
			atmos.Mass[type] -= amount;
			tankState = GasTankState.Normal;
			return amount;
		}
	}
}

