using UnityEngine;
using System.Collections;

public class stageProducer : MonoBehaviour {

    public GameObject posObject1;
    public GameObject posObject2;
    public GameObject posObject3;
    public GameObject posObject4;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;

    private Vector3 pos1;
    private Vector3 pos2;
    private Vector3 pos3;
    private Vector3 pos4;


    // Use this for initialization
    void Start () {
        pos1 = posObject1.transform.position;
        pos2 = posObject2.transform.position;
        pos3 = posObject3.transform.position;
        pos4 = posObject4.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire2"))
        {
            //產生砲彈在發射點
            Instantiate(enemy1, pos1, transform.rotation);
            Instantiate(enemy1, pos2, transform.rotation);
            Instantiate(enemy1, pos3, transform.rotation);
            Instantiate(enemy1, pos4, transform.rotation);

        }

    }
}
