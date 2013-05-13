using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Airlock))]
public class AirlockEditor : Editor {
	Vector2 size;
	
	public override void OnInspectorGUI() {
		Airlock t = (Airlock)target;
		
		EditorGUILayout.LabelField("Atmospheres");
		EditorGUILayout.BeginHorizontal();
			t.RoomA = (Atmosphere)EditorGUILayout.ObjectField(t.RoomA, typeof(Atmosphere), true);
			t.RoomB = (Atmosphere)EditorGUILayout.ObjectField(t.RoomB, typeof(Atmosphere), true);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Size:    H", GUILayout.Width(50f));
			t.height = EditorGUILayout.FloatField(t.height, GUILayout.Width(60f));
			EditorGUILayout.LabelField("W", GUILayout.Width(15f));
			t.width = EditorGUILayout.FloatField(t.width, GUILayout.Width(60f));
		EditorGUILayout.EndHorizontal();
		
		t.open = EditorGUILayout.Toggle("Open", t.open);
		t.debug = EditorGUILayout.Toggle("Debug", t.debug);
	}
}
