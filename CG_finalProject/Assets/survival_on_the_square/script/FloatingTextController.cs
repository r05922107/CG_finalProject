using UnityEngine;
using System.Collections;

public class FloatingTextController : MonoBehaviour {


	public GameObject DamageTextCanvasPrefab;


	public void createFloatingText(string text, Transform location){
		// text position
		Vector3 position = location.position;
		position.y += 3;

		// create damage text
		GameObject instance = Instantiate (DamageTextCanvasPrefab);
		instance.transform.SetParent (gameObject.transform, false);
		instance.transform.position = position;

		FloatingText script = instance.transform.FindChild ("PopupTextParent").GetComponent<FloatingText>();
		script.setText (text);
		Destroy (instance, script.animator.GetCurrentAnimatorClipInfo (0)[0].clip.length);
	}
}
