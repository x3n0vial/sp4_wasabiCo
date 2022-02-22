using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField()] AudioSource breaksound;

    public GameObject brokenrock;
    public GameObject tallrock;
    public Flashlight flashlight;

    float breaking = 0.0f;
    float breakspeed = 1.0f;

    bool breakupdate = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (breakupdate == false)
            return;

        if (flashlight.CheckIfInFlashlight(tallrock.GetComponent<BoxCollider>()) == true)
            breaking += breakspeed * Time.deltaTime;

        //Debug.Log(breaking);

        if (breaking >= 5.0f)
        {
            breakupdate = false;
            if (breaksound != null)
                breaksound.Play();
            brokenrock.SetActive(true);
            tallrock.SetActive(false);
        }
    }
}
