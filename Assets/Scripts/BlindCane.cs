using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BlindCane : MonoBehaviour
{

    private enum HitDirection { None, Top, Bottom, Forward, Back, Left, Right }

    HitDirection hitDirection = HitDirection.None;

    float vibration_length = 400;
    float default_vibration_strength = 200;

    private SteamVR_TrackedObject trackedObj;

    private Rigidbody rb;

    private Dictionary<SteamVR_Controller.Device, Coroutine> activeHapticCoroutines = new Dictionary<SteamVR_Controller.Device, Coroutine>();

    GameObject ControllerRight;
    SteamVR_TrackedObject trackedObjectR;
    SteamVR_Controller.Device deviceR;

    void Awake()
    {
        GameObject ControllerRight = GameObject.Find("Controller (right)");
        SteamVR_TrackedObject trackedObjectR = ControllerRight.GetComponent<SteamVR_TrackedObject>();
        deviceR = SteamVR_Controller.Input((int)trackedObjectR.index);
    }
    // Use this for initialization
    void Start()
    {

        trackedObj = GetComponent<SteamVR_TrackedObject>();

        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    
    void OnCollisionEnter(Collision col)
    {
        col.gameObject.SendMessage("hitTriggered");
         //   .hitTriggered();
        
        RaycastHit MyRayHit;
        Vector3 direction = (col.transform.position - transform.position).normalized;
        Ray MyRay = new Ray(transform.position, direction);

        float strength = col.impulse.magnitude + deviceR.velocity.magnitude / 1.5f;

        float clipped_strength = deviceR.velocity.magnitude;

        float vib_length = vibration_length;

        if (Physics.Raycast(MyRay, out MyRayHit))
        {

            if(col.gameObject.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeAll)
            {
                strength = col.gameObject.GetComponent<Rigidbody>().mass;

                StartHapticVibration(deviceR, vib_length, strength);

            }
            else if (MyRayHit.collider != null)
            {
                Vector3 MyNormal = MyRayHit.normal;
                MyNormal = MyRayHit.transform.TransformDirection(MyNormal);

                if (MyNormal == MyRayHit.transform.up)
                {
                    hitDirection = HitDirection.Top;
                    //Debug.Log("TOP");

                    StartHapticVibration(deviceR, vibration_length, strength);
                }
                else if (MyNormal == -MyRayHit.transform.up)
                {
                    hitDirection = HitDirection.Bottom;
                    //Debug.Log("BOTTOM");

                    StartHapticVibration(deviceR, vibration_length, strength);
                }
                else if(MyNormal == MyRayHit.transform.forward)
                {
                    hitDirection = HitDirection.Forward;
                    //Debug.Log("FORWARD");

                    StartHapticVibration(deviceR, vibration_length, strength);
                }
                else if(MyNormal == -MyRayHit.transform.forward)
                {
                    hitDirection = HitDirection.Back;
                    //Debug.Log("BACK");

                    StartHapticVibration(deviceR, vibration_length, strength);
                }
                else if(MyNormal == MyRayHit.transform.right)
                {
                    hitDirection = HitDirection.Right;
                    //Debug.Log("RIGHT");

                    StartHapticVibration(deviceR, vibration_length, strength);
                }
                else if(MyNormal == -MyRayHit.transform.right)
                {
                    hitDirection = HitDirection.Left;
                    //Debug.Log("LEFT");

                    StartHapticVibration(deviceR, vib_length, strength);
                }
                else
                {
                    StartHapticVibration(deviceR, vibration_length, clipped_strength);
                }
            }
        }
    }

    void OnCollisionExit()
    {
        //When the rod is no longer colliding, stop vibration
        StopHapticVibration(deviceR);
    }

    public void StartHapticVibration(SteamVR_Controller.Device device, float length, float strength)
    {

        if (activeHapticCoroutines.ContainsKey(device))
        {
            //Debug.Log("This device is already vibrating");
            return;
        }

        Coroutine coroutine = StartCoroutine(StartHapticVibrationCoroutine(device, length, strength));
        activeHapticCoroutines.Add(device, coroutine);

    }

    public void StopHapticVibration(SteamVR_Controller.Device device)
    {

        if (!activeHapticCoroutines.ContainsKey(device))
        {
            //Debug.Log("Could not find this device");
            return;
        }
        StopCoroutine(activeHapticCoroutines[device]);
        activeHapticCoroutines.Remove(device);
    }

    protected IEnumerator StartHapticVibrationCoroutine(SteamVR_Controller.Device device, float length, float strength)
    {
        Debug.Log("Vibration started");

        for (float i = 0; i < length; i += Time.deltaTime)
        {
            device.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3200, strength));
            yield return null;
        }

        activeHapticCoroutines.Remove(device);
    }


}
