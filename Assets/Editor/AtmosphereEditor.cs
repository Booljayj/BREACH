using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Atmosphere))]
public class AtmosphereEditor : Editor {
	Atmosphere a;
	
	bool viewGases;
	int gasindex;
	float gasvalue;
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
		
		viewGases = EditorGUILayout.Foldout(viewGases, string.Format("Mass: {0:F2} kg", a.mass));
		if (viewGases) {
			gases = new List<string>(a.Keys);
			foreach (string gas in gases) {
				EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					EditorGUILayout.LabelField(string.Format("{0}:",gas), GUILayout.Width(100f));
					a[gas] = EditorGUILayout.FloatField(a[gas]);
					EditorGUILayout.LabelField(string.Format("{0:P}", a.percent[gas]), GUILayout.Width(50f));
					if (GUILayout.Button("X", GUILayout.Width(30f))) {
						a.Remove(gas);
					}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				gasindex = EditorGUILayout.Popup(gasindex, validGases.ToArray(), GUILayout.Width(100f));
				gasvalue = EditorGUILayout.FloatField(gasvalue);
				if (GUILayout.Button("Add", GUILayout.Width(80f))) {
					a[validGases[gasindex]] = gasvalue;
				}
			EditorGUILayout.EndHorizontal();
		}
		
		EditorGUILayout.Separator();
		EditorGUILayout.LabelField(string.Format("Pressure: {0:F2} kPa", a.Pressure));
		EditorGUILayout.LabelField(string.Format("Temperature: {0:F2} K", a.Temperature));
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(string.Format("R: {0:F2} kJ/K", a.R));
		EditorGUILayout.LabelField(string.Format("k: {0:F2} kJ/K", a.k));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(string.Format("Cp: {0:F2} kJ/K", a.Cp));
			EditorGUILayout.LabelField(string.Format("Cv: {0:F2} kJ/K", a.Cv));
		EditorGUILayout.EndHorizontal();
	}
}
