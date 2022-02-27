using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pien : MonoBehaviour
{
    Collider collider;

    public Flashlight flashlight;
    public PienManager pienManager;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flashlight.CheckIfInFlashlight(collider))
        {
            this.gameObject.SetActive(false);
            pienManager.subtractNoOfPien();
            pienManager.playAudio();
        }
    }
}
