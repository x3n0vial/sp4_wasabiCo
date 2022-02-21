using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemControlTet : MonoBehaviour
{

    public Transform Slot;

    void OnMouseDown()
    {
        GetComponent<Rigidbody>().useGravity = false;
        this.transform.position = Slot.position;
        this.transform.parent = GameObject.Find("Item Slot").transform;
    }

    void OnMouseUp()
    {
        this.transform.parent = null;
        GetComponent<Rigidbody>().useGravity = true;
    }
}
