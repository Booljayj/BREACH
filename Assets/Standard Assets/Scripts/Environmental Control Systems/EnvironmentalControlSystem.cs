using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentalControlSystem : MonoBehaviour {
	public List<Atmosphere> rooms;
	
	public List<GasTankHolder> nitrogen; //tanks filled with nitrogen (N2).
	public List<GasTankHolder> oxygen; //tanks filled with oxygen (O2).
	public List<GasTankHolder> carbondioxide; //tanks filled with carbon dioxide (CO2). This is a staging area for the scrubber.
	public List<ScrubberTankHolder> scrubbers;
	
	public AnimationCurve intakeCurve = new AnimationCurve();//new Keyframe(idealPressure, gasExchangeRate), new Keyframe(0, 0));
	public AnimationCurve outputCurve = new AnimationCurve();//new Keyframe(0, gasExchangeRate), new Keyframe(idealPressure, 0));
	
	public bool ventExcess;
	
	public float O2percent_ideal = .2f;
	public float pressure_ideal = 80000f;
	public float temp_ideal = 293f;
	
	private Gases percent_ideal = new Gases();
	private Gases mass_ideal = new Gases();
	
	private Gases dm_ideal; //ideal mass movement
	private Gases dm; //mass out, to the room
	private float dQ_ideal; //mass in, from the room
	private float dQ; //mass out, from the room
	
	// Use this for initialization
	void Start () {
	}
	
	//TODO: Not functional. Fix that.
	void Update() {
		foreach (Atmosphere room in rooms) {
			//create the gases object with percentages
			percent_ideal = new Gases(1f-O2percent_ideal, O2percent_ideal, 0,0,0,0);
			
			//determine the ideal mass and heat transfers. P = mRT/V
			mass_ideal = percent_ideal*pressure_ideal*room.Volume/(temp_ideal*percent_ideal.R);
			
			//find the ideal mass output of the ECS
			dm_ideal = mass_ideal*outputCurve.Evaluate(room.Pressure)*Time.deltaTime;
			
			//pull some mass from the room
			dm = room.Percent*intakeCurve.Evaluate(room.Pressure)*Time.deltaTime;
			dm = room.GetGases(dm);
			room.Mass -= dm;
			
			//remove toxins
			dm.CO = 0; dm.CH4 = 0; dm.NOX = 0;
			
			//remove carbon dioxide
			foreach (GasTankHolder tankHolder in carbondioxide) {
				if (Mathf.Approximately(dm.CO2, 0f)) break;
				
				dm.CO2 = tankHolder.tank.PushGas(dm.CO2, GasType.CarbonDioxide);
			}
			dm.CO2 = 0; //vent excess/ensure zero
			
			//add or remove nitrogen
			foreach (GasTankHolder tankHolder in nitrogen) {
				if (dm.N2 > dm_ideal.N2) {
					dm.N2 = tankHolder.tank.PushGas(dm.N2 - dm_ideal.N2, GasType.Nitrogen);
				} else if (dm.N2 < dm_ideal.N2) {
					dm.N2 = tankHolder.tank.PullGas(dm_ideal.N2 - dm.N2, GasType.Nitrogen);
				} else {
					break;
				}
			}
			
			//add or remove oxygen
			foreach (GasTankHolder tankHolder in oxygen) {
				if (dm.O2 > dm_ideal.O2) {
					dm.O2 = tankHolder.tank.PushGas(dm.O2 - dm_ideal.O2, GasType.Oxygen);
				} else if (dm.O2 < dm_ideal.O2) {
					dm.O2 = tankHolder.tank.PullGas(dm_ideal.O2 - dm.O2, GasType.Oxygen);
				} else {
					break;
				}
			}
			
			
//			//perform the intake step
//			dm_i = room.Percent*intakeCurve.Evaluate(room.Pressure);
//			dm_i = room.PullMass(dm_i);
//			dQ_i = room.Heat*dm_i.Total/room.Mass.Total;
//			
//			//sort intake into various tanks
//			foreach (GasTankHolder tankHolder in nitrogen) {
//				dm_i.N2 = tankHolder.tank.PushGas(dm_i.N2, GasType.Nitrogen);
//			}
//			foreach (GasTankHolder tankHolder in oxygen) {
//				dm_i.O2 = tankHolder.tank.PushGas(dm_i.O2, GasType.Oxygen);
//			}
//			foreach (GasTankHolder tankHolder in carbondioxide) {
//				dm_i.CO2 = tankHolder.tank.PushGas(dm_i.CO2, GasType.CarbonDioxide);
//			}
		}
	}
}