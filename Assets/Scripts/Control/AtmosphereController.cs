//  
//  AtmosphereController.cs
//  
//  Author:
//       Justin Bool <booljayj@gmail.com>
// 
//  Copyright (c) 2013 
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using UnityEngine;
using System.Collections;

public class AtmosphereController : MonoBehaviour {
	public Atmosphere atmosphere;
	public bool bActive;
	
	//Settings =================
	public float idealPressure = 80000f;
	public float idealTemperature = 293.15f;
	public float idealO2 = .2f;
	
	//Labels ===================
	UILabel pressureLabel;
	UILabel temperatureLabel;
	
	UILabel nitrogenLabel;
	UILabel oxygenLabel;
	UILabel carbonLabel;
	UILabel toxinLabel;
	
	//Controls =================
	UISlider pressureSlider;
	UISlider temperatureSlider;
	UISlider o2Slider;
	
	//Constants ================
	static float minBarHeight = 30f;
	static float maxBarHeight = 300f;
	
	static float minPressure = 50f;
	static float maxPressure = 100f;
	
	static float minTemperature = 273.15f;
	static float maxTemperature = 313.15f;
	
	void Start () {
		pressureLabel = transform.FindChild("Pressure Label").GetComponent<UILabel>();
		temperatureLabel = transform.FindChild("Temperature Label").GetComponent<UILabel>();
		
		nitrogenLabel = transform.FindChild("Nitrogen Label").GetComponent<UILabel>();
		oxygenLabel = transform.FindChild("Oxygen Label").GetComponent<UILabel>();
		carbonLabel = transform.FindChild("Carbon Label").GetComponent<UILabel>();
		toxinLabel = transform.FindChild("Toxin Label").GetComponent<UILabel>();
		
		pressureSlider = transform.FindChild("Pressure Slider").GetComponent<UISlider>();
		temperatureSlider = transform.FindChild("Temperature Slider").GetComponent<UISlider>();
		o2Slider = transform.FindChild("O2 Slider").GetComponent<UISlider>();
		
		//setup callbacks
		pressureSlider.eventReceiver = gameObject; pressureSlider.functionName = "OnPressureUpdate";
		temperatureSlider.eventReceiver = gameObject; temperatureSlider.functionName = "OnTemperatureUpdate";
		o2Slider.eventReceiver = gameObject; o2Slider.functionName = "OnO2Update";
		
		StartCoroutine("Display");
	}
	
	IEnumerator Display() {
		while (enabled) {
			//update values
			float pressure = atmosphere.Pressure/1000f;
			pressureLabel.text = pressure.ToString("G4");
			temperatureLabel.text = (atmosphere.Temperature-273.15f).ToString("G4");
			
			nitrogenLabel.text = (atmosphere.Percent.N2*pressure).ToString("G4");
			oxygenLabel.text = (atmosphere.Percent.O2*pressure).ToString("G4");
			carbonLabel.text = (atmosphere.Percent.CO2*pressure).ToString("G4");
			float toxins = (atmosphere.Percent.CO+atmosphere.Percent.CH4+atmosphere.Percent.NOX)*pressure;
			toxinLabel.text = toxins.ToString("G4");
		
			yield return new WaitForSeconds(.25f);
		}
	}
	
	void OnPressureUpdate(float val) {
		idealPressure = minPressure + val*(maxPressure-minPressure);
	}
	
	void OnTemperatureUpdate(float val) {
		idealTemperature = minTemperature + val*(maxTemperature - minTemperature);
	}
	
	void OnO2Update(float val) {
		idealO2 = val;
	}
}

