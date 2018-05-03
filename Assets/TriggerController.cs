using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour {

	GoalSpawnController controller;
    public HeadCollider headcollider;

    AudioSource audio;

	// Use this for initialization
	void Start () {
		controller = GameObject.FindGameObjectWithTag ("GoalSpawner").GetComponent<GoalSpawnController>();
        headcollider = GameObject.Find("HeadCollider").GetComponent<HeadCollider>();
        audio = gameObject.GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider col)
	{
		//col.gameObject.SendMessage ("hitTriggered");
		//   .hitTriggered();

		//TODO + Charlie's body collider needs tag "Player".
		//Controllers + headset already have tag "Player". 
		if (col.gameObject.CompareTag ("Player") || col.gameObject.CompareTag ("Cane"))  // in here
		{
            //TODO replace this 0 and all other function calls to "goal achieved" with the amount of objects 
            //hit along the way from the player's collider or wherever it's stored.

            audio.Play();
			controller.goalAchieved(headcollider.getNumObstaclesHit());
            headcollider.reset();

            Debug.Log("Reset head collider");
		}
        else
        {
          //  Debug.Log("collision failed");
        }
	}


}
