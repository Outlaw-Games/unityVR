using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateFloor : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab_floor;
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;

    // Use this for initialization
    void Start ()
    {
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                Instantiate(prefab_floor, new Vector3(this.transform.position.x + w * prefab_floor.transform.localScale.x, -0.15f, this.transform.position.z + h * prefab_floor.transform.localScale.z), Quaternion.identity);
              //  Debug.Log("ANOTHER ONE");
            }
        }
	}

}
