using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SpawnCane : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField]
    private SteamVR_Controller.Device device;
    [SerializeField]
    private GameObject canePrefab;
    [SerializeField]
    private bool caneSpawned = false;
    [SerializeField]
    private float cane_length_in_heads = 6.5f;
    [SerializeField]
    private float heads_in_height_ratio = 7.5f;
    [SerializeField]
    private float cane_length = 0.1f;
    #endregion

    #region Private Variables
    private SteamVR_TrackedObject trackedObject = null;
    private GameObject head;
    private float distance_from_floor = 1.0f;
    private float max_player_head_height;
    #endregion

    // Use this for initialization
    void Start()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        head = GameObject.FindGameObjectWithTag("MainCamera");
        max_player_head_height = head.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        if (head.transform.position.y > max_player_head_height)
        {
            max_player_head_height = head.transform.position.y;
        }

        // poll device
        device = SteamVR_Controller.Input((int)trackedObject.index);

        // if player pulls trigger
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            // and cane is not allready spawned
            if (caneSpawned == false)
            {
                // perform a raycast to the ground from the players head
                RaycastHit hit;

                Ray downRay = new Ray(head.transform.position, -Vector3.up);
                if (Physics.Raycast(downRay, out hit))
                {
                    // if the raycast hits the floor
                    if (hit.transform.gameObject.tag == "Floor")
                    {
                        // get the distance of the head from the floor
                        distance_from_floor = head.transform.position.y - hit.point.y;
                        Debug.Log("Distance from floor: " + distance_from_floor);
                        // set cane length to the distance of the head from the floor

                        // if the distance from the floor is less than 90% of the max height of the player recorded
                        if (distance_from_floor < max_player_head_height * 0.90f)
                        {
                            // use the max player height to determine length of the cane
                            cane_length = (max_player_head_height / heads_in_height_ratio) * cane_length_in_heads;
                        }
                        else
                        {
                            // use current player height to determine the length of the cane
                            cane_length = (distance_from_floor / heads_in_height_ratio) * cane_length_in_heads;
                        }

                        Debug.Log("cane_length" + cane_length);
                    }
                }

                Debug.Log("Tried to spawn it");
                Debug.Log("Trigger depressed and Grip pressed");

                GameObject newCane = Instantiate(canePrefab, transform.position, Quaternion.identity);
                Bounds bounds = newCane.GetComponent<MeshFilter>().mesh.bounds;
                Vector3 size = bounds.size;
                Debug.Log("Bounds size:" + size);


                newCane.transform.localScale = new Vector3(newCane.transform.localScale.x, cane_length * 0.5f, newCane.transform.localScale.z);

                newCane.transform.parent = gameObject.transform;
                newCane.transform.localPosition = Vector3.zero;

                Quaternion rot = Quaternion.Euler(90, 0, 0);
                newCane.transform.localRotation = rot;

                Vector3 locTrans = new Vector3(0.0f, -0.03f, (cane_length / 2) * 0.9f);

                newCane.transform.localPosition = locTrans;
                newCane.transform.tag = "Cane";

                caneSpawned = true;
            }
        }
    }
}
