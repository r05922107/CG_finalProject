using UnityEngine;
using System.Collections;

public class AIScript : MonoBehaviour {
	
	// Objects
	public GameObject trackObject;
    private Animator anim;
	private Rigidbody rbody;

    // properties
    private float hp = 100f;
    private float speed;
    private float trackDistance = 50000f;
    private float attackMaxCD;
    private float attackDistance;
    private float attackDamage;
    private float attackKnockBack = 5f;
    private float defend = 0f;
    private float knockDefend = 0f;
    public int enemyType;

	// variables
	private float attackCD;
    private bool attacking;
    private int attackTime;
    private int thisTypeAttackTime = 20;
    private Vector3 targetDirection;
    private GameObject LandBox;


    // initial
    void Start () {

        if (enemyType == 1)  //spear
        {
            attackMaxCD = 3f;
            attackDistance = 7f;
            attackDamage = 5f;
            speed = 4.5f;
            attackKnockBack = 4f;
            thisTypeAttackTime = 30;
        }
        else if (enemyType == 2)  //hammer
        {
            attackMaxCD = 4f;
            attackDistance = 5f;
            attackDamage = 20f;
            speed = 2.5f;
            knockDefend = 1f;
            defend = 2f;
            attackKnockBack = 7f;
            thisTypeAttackTime = 30;
        }
        else if (enemyType == 3)  //swordman
        {
            hp = 100f;
            attackMaxCD = 2.5f;
            attackDistance = 6f;
            attackDamage = 12f;
            speed = 3.5f;
            defend = 1f;
        }
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

		// update HpBar face to main camera
		if (transform.FindChild ("HealthBar") != null ) {
			Transform HealthBarTransform = transform.FindChild ("HealthBar");
			HealthBarTransform.LookAt (HealthBarTransform.position + Camera.main.transform.rotation * Vector3.back,
				Camera.main.transform.rotation * Vector3.down);
		}

        LandBox = GameObject.Find("LandBox");
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
			float angle = angle_360(gameObject.transform.forward, targetDirection);
            //targetDirection = new Vector3(trackObject.transform.position.x - gameObject.transform.position.x, 0, trackObject.transform.position.z - gameObject.transform.position.z);
			

			// Attack or Move
			if (targetDirection.magnitude < attackDistance && (angle < 30 || angle > 330) || attacking) { // attack target
				if (attackCD == 0)  { // attack
					attack (trackObject, 0);
				} else {
					// CD-ing
					// idle
				}
			}else if (targetDirection.magnitude < trackDistance &&  (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1")) && gameObject.transform.position.y > -10f) { // move to player
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * 5);
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
            dead();
        }
    }


    // attack target by hurt function in target
    private void attack(GameObject target, int attackType){
        Vector3 direction = trackObject.transform.position - gameObject.transform.position;
        float angle = angle_360(gameObject.transform.forward, direction);
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
                if (targetDirection.magnitude < attackDistance &&  (angle < 30 || angle > 330))
                {
                    target.GetComponent<mainChaAct>().hurt(gameObject, attackDamage, attackKnockBack);
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
		hp -= (damage - defend);

		// damage text animation
		string damageText = ((int)(damage - defend)).ToString();
		GameObject DamageTextSystem = GameObject.Find("DamageTextSystem");
		if(DamageTextSystem != null){
			DamageTextSystem.GetComponent<FloatingTextController>().createFloatingText(damageText,transform);
		}


        // update HpBar
        if (transform.FindChild("HealthBar") != null && transform.FindChild("HealthBar").FindChild("HpBar") != null)
        {
            Transform HpBarTransform = transform.FindChild("HealthBar").FindChild("HpBar");
            HpBarTransform.localScale = new Vector3(hp / 100f, HpBarTransform.localScale.y, HpBarTransform.localScale.z);
        }

        // be knockback 
        Vector3 attackDirection = attacker.transform.forward.normalized;
		attackDirection.y = 0;
		attackDirection = attackDirection.normalized;

		rbody.velocity =  attackDirection * (knockback- knockDefend) * 5;
		//gameObject.transform.position = gameObject.transform.position + knockback * attackDirection;

		// check dead
		if (hp <= 0 || gameObject.transform.position.y < -20 ) {
            dead();

        }
	}

    public void dead() {
        //when enemyHP = 0
        //do something
        LandBox.GetComponent<stageProducer>().killEnemy();

        Destroy(gameObject);
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
