using UnityEngine;
using System.Collections;

public class RoomDataViewer : MonoBehaviour {
	Atmosphere atmos;
	public TextMesh tmesh;
	public Font font;
	
	private GameObject viewer;
	
	// Use this for initialization
	void Start () {
		if (atmos == null) {
			atmos = GetComponent<Atmosphere>();
		}
		
		if (tmesh == null) {
			viewer = (GameObject)Instantiate(Resources.Load("Utility/Room Data Viewer"));
			viewer.transform.position = transform.position+Vector3.up;
			tmesh = viewer.GetComponent<TextMesh>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		tmesh.text = "P: "+atmos.Pressure.ToString()+"\nT: "+atmos.Temperature.ToString();
	}
}

