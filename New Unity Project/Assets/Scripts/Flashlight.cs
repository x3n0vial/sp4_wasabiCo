using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Flashlight : MonoBehaviour
{
	[Space(10)]
	[SerializeField()] GameObject Lights; // all light effects and spotlight
	[SerializeField()] AudioSource switch_sound; // audio of the switcher
	[SerializeField()] ParticleSystem dust_particles; // dust particles


	public Camera cam;
	public FlashlightUI flashlightUI;
	public LayerMask blockLightLayers;

	private Light spotlight;
	private Material ambient_light_material;
	private Color ambient_mat_color;

	private bool is_enabled = true;

	private float battery_amt = 100.0f;
	private float battery_use_rate = 0.75f;

	private List<Collider> withinLightList = new List<Collider>();



	// Use this for initialization
	void Start()
	{
		// cache components
		spotlight = Lights.transform.Find("Spotlight").GetComponent<Light>();
		ambient_light_material = Lights.transform.Find("ambient").GetComponent<Renderer>().material;
		ambient_mat_color = ambient_light_material.GetColor("_TintColor");
	}


	private void Update()
	{
		if (!PauseMenu.GamePaused)
		{
			// Flashlight Input Updates
			if (Input.GetKeyDown(GameSettings.FLASHLIGHT_KEY))
				Switch();

			// Get Mouse Pos relative to centre of screen
			Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1);
			Vector3 mousePos = cam.ScreenToViewportPoint(mouseScreenPos);

			// Transform to x and z
			Vector3 targetDir = new Vector3(mousePos.x - 0.5f, 0, mousePos.y - 0.5f);
			targetDir.Normalize();
			// Transform based on camera's direction
			targetDir = Camera.main.transform.TransformDirection(targetDir);
			// Get angle to rotate
			float theta = Mathf.Acos(Vector3.Dot(targetDir, new Vector3(0, 0, -1) / (targetDir.magnitude)));
			
			if (targetDir.x > 0)
				theta *= -1;
			
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Mathf.Rad2Deg * theta, transform.rotation.eulerAngles.z);
			
			// Battery Updates
			if (is_enabled)
			{
				battery_amt -= battery_use_rate * Time.deltaTime;
				flashlightUI.UpdateBatteryBar(battery_amt);
				if (battery_amt < 0)
				{
					battery_amt = 0;
					Switch(false); // turn off lights
				}
			}
		}

	}

	
	/// changes the intensivity of lights from 0 to 100.
	/// call this from other scripts.
	public void Change_Intensivity(float percentage)
	{
		percentage = Mathf.Clamp(percentage, 0, 100);


		spotlight.intensity = (8 * percentage) / 100;

		ambient_light_material.SetColor("_TintColor", new Color(ambient_mat_color.r, ambient_mat_color.g, ambient_mat_color.b, percentage / 2000));
	}



	/// switch current state  ON / OFF.
	/// call this from other scripts.
	public void Switch()
	{
		is_enabled = !is_enabled;

		if (battery_amt == 0)
			is_enabled = false;

		Lights.SetActive(is_enabled);

		if (switch_sound != null)
			switch_sound.Play();

		flashlightUI.UpdateDisplay(is_enabled);
	}

	public void Switch(bool enable)
	{
		is_enabled = enable;

		if (battery_amt == 0)
			is_enabled = false;

		Lights.SetActive(is_enabled);

		if (switch_sound != null)
			switch_sound.Play();

		flashlightUI.UpdateDisplay(is_enabled);
	}


	
	/// enables the particles.
	public void Enable_Particles(bool value)
	{
		if (dust_particles != null)
		{
			if (value)
			{
				dust_particles.gameObject.SetActive(true);
				dust_particles.Play();
			}
			else
			{
				dust_particles.Stop();
				dust_particles.gameObject.SetActive(false);
			}
		}
	}

	public bool IsEnabled()
    {
		return is_enabled;
    }

	public float GetBattery()
    {
		return battery_amt;
    }

	public void RefillBattery()
    {
		battery_amt = 100.0f;
		flashlightUI.UpdateBatteryBar(battery_amt);
	}

	public bool CheckIfInFlashlight(Collider col)
    {
		if (!is_enabled)
			return false;

		foreach (Collider go_col in withinLightList)
        {
			if (go_col == col)
            {
				RaycastHit hitData;
				bool blocked = Physics.Linecast(transform.position, col.gameObject.transform.position, out hitData, blockLightLayers);
				if (!blocked 
					|| blocked && hitData.collider == col)
					
                {
					return true;
                }
            }
        }
	
		return false;
    }

    private void OnTriggerEnter(Collider other)
    {
		withinLightList.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
		withinLightList.Remove(other);
    }
}
