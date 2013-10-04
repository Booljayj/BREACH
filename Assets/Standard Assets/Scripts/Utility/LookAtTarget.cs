using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour {
	public Transform target;
	public float speed;
	public bool flip;
	public bool closeFreeze;
	
	Transform cam;
	
	// Use this for initialization
	void Start () {
		if (target == null) {
			gameObject.SetActive(false);
		}
		cam = Camera.main.transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Vector3 dir = target.position - transform.position;
		if (flip) {
			dir = -dir;
		}
		if (closeFreeze && dir.sqrMagnitude < .1f) {
			return;
		}
		
		Quaternion rot = Quaternion.LookRotation(dir);
		transform.rotation = (speed > 0f) ? Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * speed) : rot;
	}
}
