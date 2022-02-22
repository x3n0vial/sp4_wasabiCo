using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public GameObject brokenrock;
    public GameObject tallrock;
    public Flashlight flashlight;

    int breaking = 0;
    int breakspeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (flashlight.CheckIfInFlashlight(GetComponent<BoxCollider>()) == true)
            Debug.Log("true");

        //breaking += breakspeed;

        if (breaking >= 10)
        {
            brokenrock.SetActive(true);
            tallrock.SetActive(false);
        }
    }
}
