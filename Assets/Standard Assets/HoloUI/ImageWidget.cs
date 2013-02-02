using UnityEngine;
using System.Collections;

[System.Serializable]
public class ImageWidget : Widget {
	public Texture2D image;
	
	public override void Draw(Texture2D texture) {
		texture.SetPixels(Mathf.RoundToInt(bounds.xMin), Mathf.RoundToInt(bounds.yMin), image.width, image.height, image.GetPixels());
		
	}
}

