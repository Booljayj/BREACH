using UnityEngine;
using UnityEditor;
using System.Collections;

public class GridSnapWindow : EditorWindow {
	public Vector3 axis1 = new Vector3();
	public Vector3 axis2 = new Vector3();
	public float spacing;

	bool valid;
	Vector3 axis3 = new Vector3();
	Matrix4x4 transformation = new Matrix4x4();

	[MenuItem ("Window/Grid Snap")]
	static void Init() {
		GridSnapWindow window = EditorWindow.GetWindow<GridSnapWindow>();
		window.Validate();
	}

	void Validate() {
		if (axis1 != Vector3.zero && axis2 != Vector3.zero) {
			Debug.Log("1");
			if (axis1 != axis2) {
				axis3 = Vector3.Cross(axis1, axis2);
				transformation.SetRow(0, new Vector4(axis1.x, axis1.y, axis1.z, 1f));
				transformation.SetRow(1, new Vector4(axis2.x, axis2.y, axis2.z, 1f));
				transformation.SetRow(2, new Vector4(axis3.x, axis3.y, axis3.z, 1f));
				transformation.SetRow(3, Vector4.one);
				valid = true;
				return;
			}
		}
		valid = false;
	}

	void OnGUI() {
		axis1 = EditorGUILayout.Vector3Field("Axis 1", axis1);
		axis2 = EditorGUILayout.Vector3Field("Axis 2", axis2);
		spacing = EditorGUILayout.FloatField("Spacing", spacing);

		if (GUILayout.Button("Snap")) {
			Snap();
		}

		if (GUI.changed) {
			Validate();
		}
	}

	void Snap() {
		if (Selection.activeGameObject) {
			if (valid) {
				Vector4 cartesian = Selection.activeTransform.position;
				Vector4 arbitrary = transformation.inverse * new Vector4(cartesian.x, cartesian.y, cartesian.z, 1f);
				arbitrary = new Vector4(Mathf.Round(arbitrary.x/spacing)*spacing,
				                        Mathf.Round(arbitrary.y/spacing)*spacing,
				                        Mathf.Round(arbitrary.z/spacing)*spacing,
				                        1f);
				cartesian = transformation * arbitrary;
				Selection.activeTransform.position = new Vector3(cartesian.x, cartesian.y, cartesian.z);
			}
		}
	}
}
