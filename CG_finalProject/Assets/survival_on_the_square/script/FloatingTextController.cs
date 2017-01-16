using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingTextController : MonoBehaviour {


	public GameObject pGreenText;
	public GameObject pRedText;


	public void createDamageText(string text, Transform location){
		// text position
		Vector3 position = location.position;
		position.y += 3;

		// create text
		GameObject instance = Instantiate (pRedText);
		instance.transform.SetParent (gameObject.transform, false);
		instance.transform.position = position;

		// set text and destroy time
		FloatingText script = instance.transform.FindChild ("PopupTextParent").GetComponent<FloatingText>();
		script.setText (text);
		Destroy (instance, script.animator.GetCurrentAnimatorClipInfo (0)[0].clip.length);
	}



	public void createHealthText(string text, Transform location){
		// text position
		Vector3 position = location.position;
		position.y += 3;

		// create text
		GameObject instance = Instantiate (pGreenText);
		instance.transform.SetParent (gameObject.transform, false);
		instance.transform.position = position;

		// set text and destroy time
		FloatingText script = instance.transform.FindChild ("PopupTextParent").GetComponent<FloatingText>();
		script.setText (text);
		Destroy (instance, script.animator.GetCurrentAnimatorClipInfo (0)[0].clip.length);
	}
}
