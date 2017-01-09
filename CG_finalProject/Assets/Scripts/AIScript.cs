using UnityEngine;
using System.Collections;

public class AIScript : MonoBehaviour {
	
	// Objects
	public GameObject playerObject;
	public GameObject enemyObject;

	// properties
	public float enemyHP = 100f;
	public int enemySpeed = 1;
	public float enemyAttackCD = 3;
	public int enemyRotateSpeed = 1;

	// variables
	private float attackCD;


	// Use this for initialization
	void Start () {
		attackCD = 0;
	}
	
	// Update is called once per frame
	void Update () {
		// update attackCD
		attackCD = attackCD - Time.smoothDeltaTime <= 0 ? 0 : attackCD - Time.smoothDeltaTime;

		// check player exist
		if (playerObject == null) {
			return;
		}

		// compute forward and direction vector to player 
		Vector3 forward = enemyObject.transform.forward;
		Vector3 direction = (playerObject.transform.position - enemyObject.transform.position);

		// compute angle and distance to player
		float distance = direction.sqrMagnitude;
		float angle = angle_360(forward,direction);

		// track
		if (350 > angle && angle > 180) { // left rotate 
			enemyObject.transform.Rotate (0, -1 * enemyRotateSpeed, 0);
		} else if (180 >= angle && angle > 10) { // right rotate
			enemyObject.transform.Rotate (0, enemyRotateSpeed, 0);
		} else if (distance < 8) { // attack
			if (attackCD > 0) { // return if CD-ing
				return;
			} else {
				attack (playerObject, 0);
			}
		} else if (distance < 50000) { // move to player
			enemyObject.transform.Translate (0, 0, enemySpeed * Time.smoothDeltaTime);
		}
	
	}


	private void attack(GameObject target, int attackType){
		//hurt (playerObject, 10, 5);
		print("Attack !\n");

		// attack animation
		// do some animation

		// hurt target
		// simple test
		//target.transform.Translate (0, 0, -5);

		// reset CD
		attackCD = enemyAttackCD;
	}


	public void hurt(GameObject attacker, float damage, float knockback){
		// decrease HP
		enemyHP -= damage;

		// be knockback 
		Vector3 attackDirection = attacker.transform.forward.normalized;
		enemyObject.transform.position = enemyObject.transform.position + knockback * attackDirection;

		// check dead
		if (enemyHP <= 0) { // if dead destroyed itself
			Destroy (enemyObject);
		}
	}


	private float angle_360(Vector3 from_, Vector3 to_){  
		Vector3 v3 = Vector3.Cross(from_,to_);  
		if(v3.y > 0)  
			return Vector3.Angle(from_,to_);  
		else  
			return 360-Vector3.Angle(from_,to_);  
	}
		
}
