using UnityEngine;
using System.Collections;

public class mainChaAct : MonoBehaviour {

	public GameObject myObject;
	public GameObject forwardPoint;
    public GameObject effect;//特效
    public GameObject effect2;//特效
    public Rigidbody rigBody;
    public Animator anim;
    public float mSpeed = 0.1F;
    public float rSpeed = 1;
    public float rotationSpeed = 30;
    public float mcAttackCD = 5;
    public float HP = 100f;

	private Rigidbody rbody;
    private float inputH;
    private float inputV;
    Vector3 inputVec;
    Vector3 targetDirection;
    private float attackCD;
    private float superAttackCD;
    private float attackDistance = 6f;
    private float SuperAttackDistance = 10f;
    private float mcSuperAttackCD = 600;
    //private bool attack;

    GameObject[] targetTransform;

    

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
		rbody = GetComponent<Rigidbody>();
        superAttackCD = 0;


    }
	
	// Update is called once per frame
	void Update () {
        //anim.Play("Base.idle");

        if (Input.GetButtonDown("Fire1"))
        {
            //hitBox.SetActive(true);
            print("hit!");
            attack();
        }

        if (Input.GetButtonDown("superFire") && superAttackCD == 0)
        {
            //hitBox.SetActive(true);
            print("hit!");
            superAttack();
            superAttackCD = mcSuperAttackCD;
        }

        if (superAttackCD > 0) {
            superAttackCD--;
        }
        

        inputH = Input.GetAxisRaw("Horizontal");//獲取水平軸向按鍵
        inputV = -(Input.GetAxisRaw("Vertical"));//獲取垂直軸向按鍵
        inputVec = new Vector3(inputH, 0, inputV);

        anim.SetFloat("InputH", inputV);
        anim.SetFloat("InputV", -(inputH));

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1")) {
            UpdateMovement();
        }

        fallDetect();

		if (transform.FindChild ("HealthBar") != null ) {
			Transform HealthBarTransform = transform.FindChild ("HealthBar");
			HealthBarTransform.LookAt (HealthBarTransform.position + Camera.main.transform.rotation * Vector3.back,
				Camera.main.transform.rotation * Vector3.down);
		}

    }
	public void attack(){
		// animation
		anim.Play("Attack1");

        //StartCoroutine(COStunPause(1f));

		// get enemy
		targetTransform = GameObject.FindGameObjectsWithTag("enemy");
        
        foreach (GameObject enemy in targetTransform){ // each enemy
            //Physics.IgnoreCollision(myObject.transform.GetComponent<Collider>(), enemy.transform.GetComponent<Collider>());
            Vector3 direction = enemy.transform.position - myObject.transform.position;
			float distance = direction.magnitude;
			float angle = angle_360(myObject.transform.forward,direction);

			if(distance < attackDistance && (angle < 30 || angle > 330)){
				enemy.GetComponent<AIScript>().hurt(myObject, 10f, 5f);
                Instantiate(effect, transform.position + new Vector3(0, 1.5f, 0) + direction*0.5f, transform.rotation);
            }
		}
	}

    public void superAttack()
    {
        // animation
        anim.Play("Attack2");

        //StartCoroutine(COStunPause(1f));

        // get enemy
        targetTransform = GameObject.FindGameObjectsWithTag("enemy");

        foreach (GameObject enemy in targetTransform)
        { // each enemy
            //Physics.IgnoreCollision(myObject.transform.GetComponent<Collider>(), enemy.transform.GetComponent<Collider>());
            Vector3 direction = enemy.transform.position - myObject.transform.position;
            float distance = direction.magnitude;
            float angle = angle_360(myObject.transform.forward, direction);

            if (distance < attackDistance && (angle < 40 || angle > 320))
            {
                enemy.GetComponent<AIScript>().hurt(myObject, 30f, 14f);
                Instantiate(effect2, transform.position + new Vector3(0, 1.5f, 0) + direction * 0.5f, transform.rotation);
            }
        }
    }

    public void hurt(GameObject attacker, float damage, float knockback)
    {
		anim.Play("GetHit");
        // decrease HP
        HP -= damage;

        // update HpBar
        if (transform.FindChild("HealthBar") != null && transform.FindChild("HealthBar").FindChild("HpBar") != null)
        {
            Transform HpBarTransform = transform.FindChild("HealthBar").FindChild("HpBar");
            HpBarTransform.localScale = new Vector3(HP / 100f, HpBarTransform.localScale.y, HpBarTransform.localScale.z);
        }

        // be knockback 
        Vector3 attackDirection = attacker.transform.forward.normalized;
		attackDirection.y = 0;
		attackDirection = attackDirection.normalized;
		rbody.velocity =  attackDirection * knockback * 5;

        // be knockback 
        //Vector3 attackDirection = attacker.transform.forward.normalized;
        //this.transform.position = this.transform.position + knockback * attackDirection;

        // check dead
        if (HP <= 0)
        { // if dead destroyed itself
			Destroy(myObject);
        }
    }

    private void fallDetect() {
        if (transform.position.y < -20f) {
            Destroy(myObject);
        }
    }


	private float angle_360(Vector3 from_, Vector3 to_){  
		Vector3 v3 = Vector3.Cross(from_,to_);  
		if(v3.y > 0)  
			return Vector3.Angle(from_,to_);  
		else  
			return 360-Vector3.Angle(from_,to_);  
	}

    public IEnumerator COStunPause(float pauseTime)
    {
        yield return new WaitForSeconds(pauseTime);
    }

    void UpdateMovement()
    {
        //get movement input from controls
        //Vector3 motion = inputVec;

        //reduce input for diagonal movement
        //motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1) ? .7f : 1;
        transform.Translate(0, 0, mSpeed * -(inputV), Space.World);
        transform.Translate(mSpeed * inputH, 0, 0, Space.World);

        RotateTowardMovementDirection();
        GetCameraRelativeMovement();
    }

    void GetCameraRelativeMovement()
    {
        Transform cameraTransform = Camera.main.transform;

        // Forward vector relative to the camera along the x-z plane   
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        // Right vector relative to the camera
        // Always orthogonal to the forward vector
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        //directional inputs
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        // Target direction relative to the camera
        targetDirection = h * right + v * forward;
    }

    void RotateTowardMovementDirection()
    {
        if (inputVec != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * rotationSpeed);
        }
    }
}
