using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCollider : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;
    // Start is called before the first frame update
    Transform target;
    bool trigger = false;

    void Start()
    {
        target = GameHandler.instance.player.transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            trigger = true;
        }
    }

    public void Update()
    {
        if (trigger)
        {
            dialogueTrigger.TriggerDialogue();
            trigger = false;
            gameObject.SetActive(false);
        }
    }
}
