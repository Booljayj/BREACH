using UnityEngine;
using System.Collections;

public enum GasTankState {
	Empty,
	Normal,
	Full,
}

public class GasTank : MonoBehaviour {
	public Atmosphere atmosphere;
	public Valve valve;
	public GasType gastype;
	
	public float maxPressure = 2000f;
	
	public float Capacity {
		get {return atmosphere.Mass.Total;}
	}
	public float MaxCapacity {
		get {return 0;}
	}
	
	public float Remaining {
		get {return Capacity - atmosphere.Mass.Total;}
	}
	
	void Start () {
		atmosphere = GetComponent<Atmosphere>();
	}
	
	void Update () {
		//pure = (atmos.Percent[gastype] >= .9999f) ? true : false;
		
		if (atmosphere.Pressure > maxPressure) {
			valve.open = true;
		} else {
			valve.open = false;
		}
	}
	
	//pull some gas from the tank.
	public Gases Pull (float amount) {
		Gases outMass;
		if (atmosphere.Mass.Total > amount) {
			outMass = atmosphere.Percent*amount;
			atmosphere.Mass -= outMass;
		} else {
			outMass = atmosphere.Mass;
			atmosphere.Mass = new Gases();
		}
		
		return outMass;
	}
	
	//push some gas into the tank. Return what doesn't fit
	public Gases Push (Gases gas) {
		Gases outMass;
		if (Remaining < gas.Total) { //we don't have enough space for all the gas
			float percent = Remaining/gas.Total; //find what percentage of the input we can accept
			atmosphere.Mass += (gas*percent); //add that percentage to the atmosphere.
			outMass = (gas*(1f-percent)); //the remainder is output
		} else {
			atmosphere.Mass += gas;
			outMass = new Gases();
		}
		
		return outMass;
	}
	
	//request to push mass into tank. Return amount that does not fit.
	public float PushGas(float amount, GasType type) {
		if (atmosphere.Mass.Total + amount > MaxCapacity) {
			float topoff = MaxCapacity - atmosphere.Mass.Total;
			atmosphere.Mass[type] += topoff;
			//tankState = GasTankState.Full;
			return amount - topoff;
		} else {
			atmosphere.Mass[type] += amount;
			//tankState = GasTankState.Normal;
			return 0;
		}
	}
	
	//request to pull mass from tank. Return amount available.
	public float PullGas(float amount, GasType type) {
		if (atmosphere.Mass[type] < amount) {
			float ret = atmosphere.Mass[type];
			atmosphere.Mass[type] = 0;
			//tankState = GasTankState.Empty;
			return ret;
		} else {
			atmosphere.Mass[type] -= amount;
			//tankState = GasTankState.Normal;
			return amount;
		}
	}
}

