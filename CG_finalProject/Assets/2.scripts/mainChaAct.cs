using UnityEngine;
using System.Collections;

public class mainChaAct : MonoBehaviour {

	public GameObject myObject;
	public GameObject forwardPoint;
    public GameObject hitBox;
    public Rigidbody rigBody;
    public Animator anim;
    public float mSpeed = 0.1F;
    public float rSpeed = 1;
    public float mcAttackCD = 5;
    public float HP = 100f;

    private int counter = 0;
    private float inputH;
    private float inputV;
    private float attackCD;
    //private bool attack;

	GameObject[] targetTransform;

    Vector3 fp;
    float way = 75;
    float c = 1;
    

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	
	}
	
	// Update is called once per frame
	void Update () {
        anim.Play("idle", -1, 0f);
        inputH = Input.GetAxis("Horizontal");//獲取水平軸向按鍵
        inputV = Input.GetAxis("Vertical");//獲取垂直軸向按鍵
        anim.SetFloat("InputH", inputH);
        anim.SetFloat("InputV", inputV);
        transform.Translate(0, 0, mSpeed * inputV, Space.World);
        transform.Translate(mSpeed * inputH, 0, 0, Space.World);
        

        fp = forwardPoint.transform.position;
        Vector2 dir2 = new Vector2(fp.x - transform.position.x, fp.z - transform.position.z);
        float angle = Vector2.Angle(dir2, new Vector2(inputH, inputV));

        if ((inputH != 0 || inputV != 0) && angle > 5)
        {
            if (angle - way > 1)
            {
                c = c * (-1);
                Debug.Log("reverse!!!!!");
            }
            transform.transform.Rotate(0, rSpeed * c, 0);
            way = angle;
        }

        if (Input.GetButtonDown("Fire1")) {
            //hitBox.SetActive(true);
			attack ();
            counter = 30;
        }

        if (counter > 0)
        {
            counter--;
        }

        if (counter == 0)
        {
            hitBox.SetActive(false);
        }
        //float moveX = inputH * 50f * Time.deltaTime;
        //float moveZ = inputV * 50f * Time.deltaTime;

        //rigBody.velocity = new Vector3(moveX, 0f, moveZ);


        //transform.RotateAround(transform.TransformPoint(transform.position), Vector3.up, 20 * Time.deltaTime);
        //transform.Translate(0, 0, mSpeed * v);//根據水平軸向按鍵來前進或後退
        //transform.Translate(mSpeed * h, 0, 0);//

    }
	public void attack(){
		// animation
		anim.Play("Attack1");

		// get enemy
		targetTransform = GameObject.FindGameObjectsWithTag("enemy");
		foreach(GameObject enemy in targetTransform){ // each enemy
			Vector3 direction = enemy.transform.position - myObject.transform.position;
			float distance = direction.magnitude;
			float angle = angle_360(myObject.transform.forward,direction);

			if(distance < 10 && (angle < 30 || angle > 330)){
				enemy.GetComponent<AIScript>().hurt(myObject, 10f, 5f);
			}
		}
	}

    public void hurt(GameObject attacker, float damage, float knockback)
    {
		anim.Play("GetHit");
        // decrease HP
        HP -= damage;

        // be knockback 
        Vector3 attackDirection = attacker.transform.forward.normalized;
        this.transform.position = this.transform.position + knockback * attackDirection;

        // check dead
        if (HP <= 0)
        { // if dead destroyed itself
            Destroy(this);
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
