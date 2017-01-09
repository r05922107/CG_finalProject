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
	private int enemyRotateSpeed = 1;

	// variables
	private float attackCD;


	// Use this for initialization
	void Start () {
		attackCD = enemyAttackCD;
	}
	
	// Update is called once per frame
	void Update () {
		// update attackCD
		attackCD = attackCD - Time.smoothDeltaTime <= 0 ? 0 : attackCD - Time.smoothDeltaTime;

		// compute direction vector to player 
		Vector3 forward = enemyObject.transform.forward;
		Vector3 direction = (playerObject.transform.position - enemyObject.transform.position);
		float distance = direction.sqrMagnitude;

		Vector2 forward2D = new Vector2 (forward.x, forward.z).normalized;
		Vector2 direction2D = new Vector2 (direction.x, direction.z).normalized;

		float rotate = Vector3.Cross (forward2D, direction2D).z;
		float face = Vector3.Dot (forward2D, direction2D);

		//track
		if (rotate > 0.05) { // right rotate 
			enemyObject.transform.Rotate (0, enemyRotateSpeed, 0);
		} else if (rotate < -0.05) { // left rotate
			enemyObject.transform.Rotate (0, -1 * enemyRotateSpeed, 0);
		} else if (distance < -1) { // attack
			if (attackCD > 0) { // return if CD-ing
				return;
			}
			// 
			//Rigidbody shoot = (Rigidbody)Instantiate (bullet, firePoint.transform.position, firePoint.transform.rotation);
			//shoot.velocity = direction;
			//Physics.IgnoreCollision (firePoint.transform.root.GetComponent<Collider> (), shoot.GetComponent<Collider> ());
			print("Attack !\n");
			attackCD = enemyAttackCD;
		} else if (distance < 50000) { // move to player
			enemyObject.transform.Translate (0, 0, (-1) * enemySpeed * Time.smoothDeltaTime);
		}
	
	}

	public int hurt(GameObject attacker, float damege){
		// enemyHP deacrease by damage
		print(attacker.gameObject.tag + " attack " + damege + " damage.\n");
		enemyHP -= damege;
		print("Remined HP :" + enemyHP + "\n");
		return 1;
	}


}
