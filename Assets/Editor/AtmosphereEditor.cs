using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Atmosphere))]
public class AtmosphereEditor : Editor {
	Atmosphere a;
	int addindex;
	float addvalue;
	List<string> validGases = new List<string>();
	List<string> gases = new List<string>();
	
	void Awake() {
		a = (Atmosphere)target;
		
		validGases.Clear();
		foreach (string key in Properties.properties.Keys)
			validGases.Add(key);
	}
	
	public override void OnInspectorGUI() {
		EditorGUILayout.LabelField("Custom Atmosphere Inspector");
		
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Volume:", GUILayout.Width(50f));
			a.volume = EditorGUILayout.FloatField(a.volume);
			EditorGUILayout.LabelField("m^3", GUILayout.Width(30f));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Heat:", GUILayout.Width(50f));
			a.heat = EditorGUILayout.FloatField(a.heat);
			EditorGUILayout.LabelField("kJ", GUILayout.Width(30f));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		gases.Clear();
		foreach (string k in a.Keys)
			gases.Add(k);
		foreach (string gas in gases) {
			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				EditorGUILayout.LabelField(gas+":", GUILayout.Width(100f));
				a[gas] = EditorGUILayout.FloatField(a[gas]);
				EditorGUILayout.LabelField(string.Format("{0}%", a.percent[gas]));
				if (GUILayout.Button("X", GUILayout.Width(20f))) {
					a.Remove(gas);
				}
			EditorGUILayout.EndHorizontal();
		}
		
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			addindex = EditorGUILayout.Popup(addindex, validGases.ToArray());
			addvalue = EditorGUILayout.FloatField(addvalue);
			if (GUILayout.Button("Add", GUILayout.Width(50f))) {
				a[validGases[addindex]] = addvalue;
			}
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.LabelField(string.Format("Mass: {0} kg", a.mass.ToString()));
		EditorGUILayout.LabelField(string.Format("Pressure: {0} Pa", a.Pressure.ToString()));
		EditorGUILayout.LabelField(string.Format("Temperature: {0} K", a.Temperature.ToString()));
		EditorGUILayout.LabelField(string.Format("R: {0} K", a.R.ToString()));
		EditorGUILayout.LabelField(string.Format("Cp: {0} K", a.Cp.ToString()));
		EditorGUILayout.LabelField(string.Format("k: {0} K", a.k.ToString()));
	}
}
