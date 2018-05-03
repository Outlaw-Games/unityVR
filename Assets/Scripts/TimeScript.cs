using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScript : MonoBehaviour {

    public GameObject[] cubes;

    public int num_activated = 0;

    public float time;
    public bool game_over = false;

    private string UIString;
    public Text scoreText;

    // Use this for initialization
    void Start () {

        cubes = GameObject.FindGameObjectsWithTag("Cube");

        scoreText.text = "Number hit: " + num_activated + " / 12";
    }

    // Update is called once per frame
    void Update ()
    {
        time = time + Time.deltaTime;

		foreach(GameObject cube in cubes)
        {
            if(cube.GetComponent<LightOn>().isActived())
            {
                num_activated = num_activated + 1;
            }
        }

        //If all cubes have been touched
        if(num_activated == cubes.Length)
        {
            if (game_over != true)
            {
                Debug.Log(time);
                scoreText.text = "Number hit: " + num_activated.ToString() + " / 12";
                game_over = true;
            }
        }
        else
        {
            scoreText.text = "Number hit: " + num_activated.ToString() + " / 12";
            num_activated = 0;
        }

	}
}
