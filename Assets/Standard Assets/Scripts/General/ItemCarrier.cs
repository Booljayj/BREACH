using UnityEngine;
using System.Collections;

public class ItemCarrier : MonoBehaviour {
	public GameObject currentItem;
	
	private Ray ray;
	private RaycastHit rayHit;

	void Update() {
		ray = new Ray(transform.position, transform.forward);
		if (Physics.Raycast(ray, out rayHit, 2.0f)) {
			rayHit.transform.GetComponent<Plug>();
		}
	}
}
