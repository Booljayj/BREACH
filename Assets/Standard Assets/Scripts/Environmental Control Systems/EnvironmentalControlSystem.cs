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
	private Gases N,O,C,T; //transport vectors
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
			float Rideal = Atmosphere.CalculateIdealConstant(percent_ideal);
			
			//determine the ideal mass and heat transfers. P = mRT/V
			mass_ideal = percent_ideal*pressure_ideal*room.Volume/(temp_ideal*Rideal);
			
			//find the ideal mass output of the ECS
			dm_ideal = mass_ideal*outputCurve.Evaluate(room.Pressure)*Time.deltaTime;
			
			//pull some mass from the room
			dm = room.Percent*intakeCurve.Evaluate(room.Pressure)*Time.deltaTime;
			dm = room.GetGases(dm);
			room.Mass -= dm;
			
			//separate gases into vectors
			N = new Gases(dm.N2, 0,0,0,0,0);
			O = new Gases(0, dm.O2, 0,0,0,0);
			C = new Gases(0,0, dm.CO2, 0,0,0);
			T = new Gases(0,0,0, dm.CO, dm.CH4, dm.NOX);
			
			//vent toxins
			T = new Gases();
			
			//shift N towards ideal amount
			N = ModGas(N, new Gases(dm_ideal.N2, 0,0,0,0,0), GasType.Nitrogen);
			
			//shift O towards ideal amount
			O = ModGas(O, new Gases(0, dm_ideal.O2, 0,0,0,0), GasType.Oxygen);
			
			//shift CO2 towards ideal damount
			C = ModGas(C, new Gases(), GasType.CarbonDioxide);
			
			//vent CO2
			C = new Gases();
			
			//return modified gases to the room
			room.Mass += (N+O+C+T);
			
			
			//=====HEAT
		}
	}
	
	private void InStep(Atmosphere A, ref Gases gas) {
		//pull a certain volume of gas from the room
		//float dV = .1f; //m^3
		
		//Gases dm = A.PullMass(dV*A.GasDensity);
		
		//separate the gas using the Mass Cyclometer into N,O,C,T
		
		//vent T to space
		
		//push N to tanks
		
		//push O to tanks
		
		//push C to tanks
		//vent remaining C if set
		
		//return remainder gases
	}
	
	private void OutStep(Atmosphere A, ref Gases gas) {
		//calculate nominal values
		
		//move N to nominal, venting if necessary
		
		//move O to nominal, venting if necessary
		
		//add N,O, and C, return to room
	}
	
	private Gases ModGas(Gases m, Gases m_ideal, GasType type) {
		List<GasTankHolder> list = new List<GasTankHolder>();
		Gases m_out = new Gases();
		
		switch (type) {
		case GasType.Nitrogen:
			list = nitrogen;
			break;
		case GasType.Oxygen:
			list = oxygen;
			break;
		case GasType.CarbonDioxide:
			list = carbondioxide;
			break;
		}
		
		foreach (GasTankHolder holder in list) {
			if (Mathf.Approximately(m.Total, m_ideal.Total)) {
				continue;
				
			} else if (m.Total > m_ideal.Total) { //we need to lower m;
				m_out = m_ideal; //we assume a perfect transfer.
				m_out += holder.tank.Push(m - m_ideal); //push excess into the tank, return remainder
				m = m_out; //set up for the next loop from this point
				
			} else { //we need to raise m;
				m_out = m; //we assume no pull occurred
				m_out += holder.tank.Pull(m_ideal.Total - m.Total); //pull addition from tank
				m = m_out; //set up for the next loop from this point
			}
		}
		
		return m_out;
	}
}