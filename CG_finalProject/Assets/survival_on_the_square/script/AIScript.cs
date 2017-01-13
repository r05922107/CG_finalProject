using UnityEngine;
using System.Collections;

public class AIScript : MonoBehaviour {
	
	// Objects
	public GameObject trackObject;
    private Animator anim;
	private Rigidbody rbody;

    // properties
    public float hp = 100f;
	public float speed = 1f;
	public float trackDistance = 50000f;
	public float attackMaxCD = 3f;
	public float attackDistance = 6f;
	public float attackDamage = 10f; 

	// variables
	private float attackCD;
    private bool attacking;
    private int attackTime;
    private int thisTypeAttackTime = 20;
    private Vector3 targetDirection;


    // initial
    void Start () {
		attackCD = 0;
        attacking = false;
        attackTime = thisTypeAttackTime;
        anim = GetComponent<Animator>();
		rbody = GetComponent<Rigidbody>();
    }
	
	// update
	void Update () {
		// update attackCD
		attackCD = attackCD - Time.smoothDeltaTime <= 0 ? 0 : attackCD - Time.smoothDeltaTime;

		// update motions
		motionUpdate ();
        fallDetect();


    }
		
	// motion 
	private void motionUpdate(){
		// track player
		if (trackObject != null) {
			// Rotate forward to Target  
			Vector3 forward = gameObject.transform.forward;
			forward.y = 0;
			forward = forward.normalized;
			targetDirection = (trackObject.transform.position - gameObject.transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * 5 );

			// Attack or Move
			if (targetDirection.magnitude < attackDistance || attacking) { // attack target
				if (attackCD == 0)  { // attack
					attack (trackObject, 0);
				} else {
					// CD-ing
					// idle
				}
			}else if (targetDirection.magnitude < trackDistance &&  (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))) { // move to player
				gameObject.transform.Translate (0, 0, speed * Time.smoothDeltaTime);
			}
		} else {
			// no target
			// idle
		}


	}




    private void fallDetect()
    {
        if (transform.position.y < -20f)
        {
            Destroy(gameObject);
        }
    }


    // attack target by hurt function in target
    private void attack(GameObject target, int attackType){
        // attack animation
        if (!attacking)
        {
            attacking = true;
            anim.Play("Attack1");
        }
        else
        {
            // hurt target
            if (attackTime == 0) {
                if (targetDirection.magnitude < attackDistance)
                {
                    target.GetComponent<mainChaAct>().hurt(gameObject, attackDamage, 5f);
                }

                // reset CD
                attacking = false;
                attackCD = attackMaxCD;
                attackTime = thisTypeAttackTime;
            }
            
            attackTime--;
            
        }
		// do some animation

	}




	// be hurt
	public void hurt(GameObject attacker, float damage, float knockback){


		// decrease HP
		hp -= damage;

		// be knockback 
		Vector3 attackDirection = attacker.transform.forward.normalized;
		attackDirection.y = 0;
		attackDirection = attackDirection.normalized;

		rbody.velocity =  attackDirection * knockback * 5;
		//gameObject.transform.position = gameObject.transform.position + knockback * attackDirection;

		// check dead
		if (hp <= 0 || gameObject.transform.position.y < -20 ) {
			Destroy (transform.root.gameObject);
		}
	}

    public void dead() {
        //when enemyHP = 0
        //do something
    }










	// math function
    private float angle_360(Vector3 from_, Vector3 to_){  
		Vector3 v3 = Vector3.Cross(from_,to_);  
		if(v3.y > 0)  
			return Vector3.Angle(from_,to_);  
		else  
			return 360-Vector3.Angle(from_,to_);  
	}
}
