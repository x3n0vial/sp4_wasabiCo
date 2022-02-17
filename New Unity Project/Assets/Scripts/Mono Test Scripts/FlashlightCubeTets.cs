using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightCubeTets : MonoBehaviour
{

    public Flashlight flashlight;
    public Image testImage;
    Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flashlight.CheckIfInFlashlight(collider))
        {
            testImage.color = Color.red;
            Debug.Log("Hit Cube!");
        }
        else
        {
            testImage.color = Color.green;
        }
    }
}
