using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Flashlight : MonoBehaviour
{
	[Space(10)]
	[SerializeField()] GameObject Lights; // all light effects and spotlight
	[SerializeField()] AudioSource switch_sound; // audio of the switcher
	[SerializeField()] ParticleSystem dust_particles; // dust particles



	private Light spotlight;
	private Material ambient_light_material;
	private Color ambient_mat_color;
	private bool is_enabled = false;



	private float battery_amt = 100.0f;
	private float battery_use_rate = 2.0f;



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
		if (Input.GetKeyDown(KeyCode.F))
			Switch();
		
		if (is_enabled)
        {
			battery_amt -= battery_use_rate * Time.deltaTime;
			if (battery_amt < 0)
            {
				battery_amt = 0;
				Switch(false); // turn off lights
            }
        }

	}

	/// <summary>
	/// changes the intensivity of lights from 0 to 100.
	/// call this from other scripts.
	/// </summary>
	public void Change_Intensivity(float percentage)
	{
		percentage = Mathf.Clamp(percentage, 0, 100);


		spotlight.intensity = (8 * percentage) / 100;

		ambient_light_material.SetColor("_TintColor", new Color(ambient_mat_color.r, ambient_mat_color.g, ambient_mat_color.b, percentage / 2000));
	}




	/// <summary>
	/// switch current state  ON / OFF.
	/// call this from other scripts.
	/// </summary>
	public void Switch()
	{
		is_enabled = !is_enabled;

		if (battery_amt == 0)
			is_enabled = false;

		Lights.SetActive(is_enabled);

		if (switch_sound != null)
			switch_sound.Play();
	}

	public void Switch(bool enable)
	{
		is_enabled = enable;

		if (battery_amt == 0)
			is_enabled = false;

		Lights.SetActive(is_enabled);

		if (switch_sound != null)
			switch_sound.Play();
	}





	/// <summary>
	/// enables the particles.
	/// </summary>
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



    
}
