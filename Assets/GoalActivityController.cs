using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalActivityController : MonoBehaviour {

	[SerializeField]
	bool isActive = false;

	public void setActive(bool newVal)
	{
		isActive = newVal;
		if (isActive)
		{
			GetComponent<BoxCollider> ().enabled = true;
			GetComponentInChildren<Light> ().enabled = true;
		}
		else 
		{
			GetComponent<BoxCollider> ().enabled = false;
			GetComponentInChildren<Light> ().enabled = false;
		}
	}
}
