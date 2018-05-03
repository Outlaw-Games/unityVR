using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStruckController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col)
	{
		foreach (ContactPoint contact in col.contacts)
		{
			Debug.DrawRay (contact.point, contact.normal, Color.white);
		}
		if (col.gameObject.CompareTag("Cane")) 
		{
			
			Debug.Log ("you hit the CANEEEE");
			//trigger play particle
			GetComponent<ParticleSystem> ().Play ();
		}
		// Trigger light up
	}
}
