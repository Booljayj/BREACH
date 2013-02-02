using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class Widget {	
	public bool enabled;
	public Rect bounds;
	
	//update the state of this widget. Optionally use the pointer position and state.
	public virtual void Update(Vector2 pointerPos, PointerState pointerState) {}
	
	public virtual void Draw(Texture2D texture) {}
}

