using UnityEngine;
using System.Collections;

public class FlyCamera : MonoBehaviour {
	public float speedX = 4;
	public float speedY = 4;
	public float speedZ = 4;
	public float sensitivityYaw = 1;
	public float sensitivityPitch = 1;
	
	Vector3 movement;
	Quaternion look;
	
	Transform cam;

	// Use this for initialization
	void Start () {
		cam = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.LeftShift)) {
			movement = new Vector3(0, Input.GetAxis("Vertical")*speedY, 0);
		} else {
			movement = new Vector3(Input.GetAxis("Horizontal")*speedX, 0, Input.GetAxis("Vertical")*speedZ);
		}
		transform.position += (transform.rotation*movement)*Time.deltaTime;
		cam.position = transform.position;
		
		transform.Rotate(Vector3.up, Input.GetAxis("Mouse X")*sensitivityYaw);
		if (Input.GetMouseButton(1)) {
			cam.Rotate(Vector3.right, -Input.GetAxis("Mouse Y")*sensitivityPitch);
		}
	}
}
