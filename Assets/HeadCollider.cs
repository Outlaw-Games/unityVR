using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollider : MonoBehaviour {

    public GameObject lasthit;
    public int number_of_obstacles_hit = 0;

    public GameObject hmd;

	// Use this for initialization
	void Start () {
        hmd = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(hmd.transform.position.x, transform.position.y, hmd.transform.position.z);
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject == lasthit && col.gameObject.tag != "Floor")
        {
            //Do nothing
        }
        else
        {
            lasthit = col.gameObject;
            number_of_obstacles_hit++;
        }
    }

    public int getNumObstaclesHit()
    {
        return number_of_obstacles_hit;
    }

    public void reset()
    {
        number_of_obstacles_hit = 0;
    }
}
