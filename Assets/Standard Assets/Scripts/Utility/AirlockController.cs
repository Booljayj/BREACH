using UnityEngine;
using System.Collections;

public class AirlockController : MonoBehaviour {
	Airlock al;
	
	void Start () {
		al = GetComponent<Airlock>();
		
		SetAirlockColor();
	}
	
	void OnMouseDown() {
		if (al.open) {
			al.open = false;
		} else {
			al.open = true;
		}
		
		SetAirlockColor();
	}
	
	void SetAirlockColor() {
		if (al.open) {
			renderer.material.color = Color.green;
		} else {
			renderer.material.color = Color.red;
		}
	}
}
