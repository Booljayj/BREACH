using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PointerState {
	Inactive,
	Active,
	Hold,
}

public class HoloUI : MonoBehaviour {
	public string configFile;
	public List<Widget> widgets;
	
	private Texture2D texture;
	public int sizeX, sizeY;
	
	public bool showPointer;
	public Texture2D pointerImage;
	private Vector2 pointerPos;
	private PointerState pointerState;
	
	[ExecuteInEditMode]
	void Start() {
		//create texture
		texture = new Texture2D(sizeX, sizeY);
		renderer.material.mainTexture = texture;
		
		//setup containers
		widgets = new List<Widget>();
		
		//read config file and create all widgets. This should only be used for schemas, since the widgets need to be connected afterward.
		
	}
	
	// Update is called once per frame
	[ExecuteInEditMode]
	void Update () {
		//shortcut if the camera is far away
		if ((transform.position - Camera.mainCamera.transform.position).sqrMagnitude > 20f) return;
		
		//update and draw each widget
		foreach (Widget w in widgets) {
			if (w.enabled) {
				w.Update(pointerPos, pointerState);
				w.Draw(texture);
			}
		}
		
		if (showPointer) {
			texture.SetPixels((int)pointerPos.x, (int)pointerPos.y, pointerImage.width, pointerImage.height, pointerImage.GetPixels());
		}
		
		//shift pointer state down if necessary
		if (pointerState == PointerState.Active) {
			pointerState = PointerState.Hold;
		}
	}
	
	public void UpdatePointer(Vector2 uvcoords, bool active) {
		pointerPos.x = uvcoords.x*sizeX;
		pointerPos.y = uvcoords.y*sizeY;
		
		if (active) {
			if (pointerState == PointerState.Inactive) {
				pointerState = PointerState.Active;
			}
		}
		else {
			pointerState = PointerState.Inactive;
		}
	}
}
