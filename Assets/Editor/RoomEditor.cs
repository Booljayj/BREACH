using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor {
	public override void OnInspectorGUI() {
		Room t = (Room)target;
		
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Volume", GUILayout.Width(100f));
			t.Volume = EditorGUILayout.FloatField(t.Volume, GUILayout.Width(100f));
			EditorGUILayout.LabelField("m^3");
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Heat", GUILayout.Width(100f));
			t.Heat = EditorGUILayout.FloatField(t.Heat, GUILayout.Width(100f));
			EditorGUILayout.LabelField("MJ Temp: "+t.Temperature.ToString("N1"));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Pressure", GUILayout.Width(100f));
			EditorGUILayout.LabelField(t.Pressure.ToString("N1"), GUILayout.Width(100f));
			EditorGUILayout.LabelField("Pa");
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.LabelField("Gas Content");
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("N2", GUILayout.Width(30f));
			t.Mass.N2 = EditorGUILayout.FloatField(t.Mass.N2);
			EditorGUILayout.LabelField("kg    "+(t.Percent.N2).ToString("P1"));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("O2", GUILayout.Width(30f));
			t.Mass.O2 = EditorGUILayout.FloatField(t.Mass.O2);
			EditorGUILayout.LabelField("kg    "+(t.Percent.O2).ToString("P1"));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("CO2", GUILayout.Width(30f));
			t.Mass.CO2 = EditorGUILayout.FloatField(t.Mass.CO2);
			EditorGUILayout.LabelField("kg    "+(t.Percent.CO2).ToString("P1"));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("CO", GUILayout.Width(30f));
			t.Mass.CO = EditorGUILayout.FloatField(t.Mass.CO);
			EditorGUILayout.LabelField("kg    "+(t.Percent.CO).ToString("P1"));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("CH4", GUILayout.Width(30f));
			t.Mass.CH4 = EditorGUILayout.FloatField(t.Mass.CH4);
			EditorGUILayout.LabelField("kg    "+(t.Percent.CH4).ToString("P1"));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("NOX", GUILayout.Width(30f));
			t.Mass.NOX = EditorGUILayout.FloatField(t.Mass.NOX);
			EditorGUILayout.LabelField("kg    "+(t.Percent.NOX).ToString("P1"));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.LabelField("Gas Density: "+t.GasDensity.ToString("N1")+" kg/m^3");
	}
}

