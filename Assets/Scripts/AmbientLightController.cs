using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientLightController : MonoBehaviour {

	public enum LightState
	{
		FULL_LIGHT = 0,
		HALF_LIGHT,
		NO_LIGHT,
        REGAIN_LIGHT
	};

	public float fullLightDuration = 10.0f;
	public float halfLightDuration = 10.0f;
    public float regainLightDuration = 1.5f;

	public float fadeSlowing = 0.0f;

	public Color ambientColor;
	public Color skyboxColor;
	public Color dimmedColor;
	public Color blackColor;

    public bool addded_feature = true;

	public LightState ambienceState;

    public float time_in_darkness = 3.0f;

	private float timer = 0.0f;

    private bool waited = false;

   	// Use this for initialization
	void Start () {

		ambientColor = RenderSettings.ambientLight;
		skyboxColor = RenderSettings.skybox.color;
		timer = fullLightDuration;
		ambienceState = LightState.FULL_LIGHT;

	}
	
	// Update is called once per frame
	void Update () {

		timer -= Time.deltaTime;

		if (timer <= 0.0f && ambienceState == LightState.FULL_LIGHT) 
		{
			if (RenderSettings.ambientIntensity >= 0.5f) 
			{
				RenderSettings.ambientIntensity -= Time.deltaTime * fadeSlowing;
				RenderSettings.reflectionIntensity -= Time.deltaTime* fadeSlowing;
				RenderSettings.ambientLight = Color.Lerp(ambientColor, dimmedColor, 1.5f);
				RenderSettings.skybox.color = Color.Lerp(skyboxColor, dimmedColor, 1.5f);
                Debug.Log("GOING DOWN");
			} 
			else 
			{
				ambienceState = LightState.HALF_LIGHT;
				ambientColor = dimmedColor;
				skyboxColor = dimmedColor;
				timer = halfLightDuration;
			}
		}
		if (timer <= 0.0f && ambienceState == LightState.HALF_LIGHT) 
		{
			if (RenderSettings.ambientIntensity >= 0.0f) 
			{
				RenderSettings.ambientIntensity -= Time.deltaTime * fadeSlowing;
				RenderSettings.reflectionIntensity -= Time.deltaTime* fadeSlowing;
				RenderSettings.ambientLight = Color.Lerp(ambientColor, blackColor, 1.5f);
				RenderSettings.skybox.color = Color.Lerp(skyboxColor, dimmedColor, 1.5f);
			} 
			else 
			{
				ambienceState = LightState.NO_LIGHT;
				timer = 0.0f;
			}
		}
        if (ambienceState == LightState.REGAIN_LIGHT)
        {
            if (RenderSettings.ambientIntensity <= 0.7f)
            {
                RenderSettings.ambientIntensity += Time.deltaTime * fadeSlowing;
                RenderSettings.reflectionIntensity += Time.deltaTime * fadeSlowing;
            }
            else
            {
                halfLightDuration = 0.5f;
                fullLightDuration = 0.5f;

                timer = fullLightDuration;
                ambienceState = LightState.FULL_LIGHT;
            }
        }
        if (ambienceState == LightState.NO_LIGHT)
        {
            if(waited == false && addded_feature == true)
            {
                StartCoroutine(Wait());
                Debug.Log("Started cooldown");
            }
        }
	}

    IEnumerator Wait()
    {
        waited = true;

        yield return new WaitForSeconds(time_in_darkness);

        Debug.Log("Cooldown ended");

        ambienceState = LightState.REGAIN_LIGHT;

        timer = regainLightDuration;

        waited = false;
    }
}
