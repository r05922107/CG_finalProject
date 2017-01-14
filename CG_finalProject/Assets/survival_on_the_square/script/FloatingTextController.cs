using UnityEngine;
using System.Collections;

public class FloatingTextController : MonoBehaviour {

	public GameObject popupText;
	public GameObject damageTextCanvas;



	public void createFloatingText(string text, Transform location){
		// text position
		Vector3 position = location.position;
		position.y = 3;

		// create damage text
		GameObject instance = Instantiate (popupText);
		instance.transform.SetParent (damageTextCanvas.transform, false);
		instance.transform.position = position;

		FloatingText script = instance.GetComponent<FloatingText>();
		script.setText (text);
	}
}
