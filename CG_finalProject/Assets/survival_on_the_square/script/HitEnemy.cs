using UnityEngine;
using System.Collections;

public class HitEnemy : MonoBehaviour {
    public GameObject mc; 
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Physics.IgnoreCollision(mc.transform.GetComponent<Collider>(), GetComponent<Collider>());

    }

    void OnCollisionEnter(Collision collision)
    {//碰撞發生時呼叫
        GameObject hitObj = collision.gameObject;

        //碰撞敵人後扣lp
        if (hitObj.tag == "enemy")
        {//當撞到的collider具有enemy時
            hitObj.GetComponent<AIScript>().hurt(mc, 10f, 5f);
        }
        
    }
}
