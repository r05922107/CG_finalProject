using UnityEngine;
using System.Collections;

public class stageProducer : MonoBehaviour {

    public GameObject mainCharacter;
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
    private Vector3 pos5;
    private Vector3 pos6;
    private Vector3 pos7;
    private Vector3 pos8;

    private Vector3 posMid;


    // Use this for initialization
    void Start () {
        pos1 = posObject1.transform.position;
        pos2 = posObject2.transform.position;
        pos3 = posObject3.transform.position;
        pos4 = posObject4.transform.position;
        pos5 = (pos1 + pos2 ) / 2;
        pos6 = (pos3 + pos2) / 2;
        pos7 = (pos3 + pos4) / 2;
        pos8 = (pos1 + pos4) / 2;
        posMid = (pos1 + pos2 + pos3 + pos4) / 4;
        enemy1.GetComponent<AIScript>().trackObject = mainCharacter;
        enemy2.GetComponent<AIScript>().trackObject = mainCharacter;
        enemy3.GetComponent<AIScript>().trackObject = mainCharacter;
        //enemy4.GetComponent<AIScript>().trackObject = mainCharacter;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire2"))
        {
            //產生敵人在發射點
            Instantiate(enemy1, pos1, transform.rotation);
            Instantiate(enemy1, pos2, transform.rotation);
            Instantiate(enemy1, pos3, transform.rotation);
            Instantiate(enemy1, pos4, transform.rotation);

        }

        if (Input.GetButtonDown("Fire3"))
        {
            //產生敵人在發射點
            Instantiate(enemy2, posMid, transform.rotation);
        }

        if (Input.GetButtonDown("Jump"))
        {
            //產生敵人在發射點
            Instantiate(enemy3, pos5, transform.rotation);
            Instantiate(enemy3, pos7, transform.rotation);
        }

    }
}
