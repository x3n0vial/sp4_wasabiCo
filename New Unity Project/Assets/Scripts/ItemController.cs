using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public GameObject Slot; //reference to your hands/the position where you want your object to go
    bool canpickup; //a bool to see if you can or cant pick up the item
    GameObject Pickable; // the gameobject onwhich you collided with
    bool hasItem; // a bool to see if you have an item in your hand

    public float throwForce = 5000;

    void Start()
    {
        canpickup = false;    //setting both to false
        hasItem = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canpickup == true) // if you enter thecollider of the objecct
        {
            if (Input.GetKeyDown(GameSettings.PICKUP_ITEM_KEY))  // pickup 
            {
                if (hasItem) // drop curr item if player is already holding sth
                {

                    Collider[] collider1 = Pickable.GetComponents<Collider>();
                    foreach (Collider d in collider1)
                    {
                        d.enabled = true;
                    }
                    Pickable.GetComponent<Collider>().enabled = true;
                    Pickable.GetComponent<Rigidbody>().isKinematic = false; // make the rigidbody work again
                    Pickable.GetComponent<Rigidbody>().useGravity = true;
                    Pickable.transform.parent = null;
                    Debug.Log("Drop");
                }

                Collider[] collider = Pickable.GetComponents<Collider>();
                foreach (Collider c in collider)
                {
                    c.enabled = false;
                }
                Pickable.transform.rotation = new Quaternion(0, 0, 0, 0);
                Pickable.GetComponent<Rigidbody>().isKinematic = true;   //makes the rigidbody not be acted upon by forces
                Pickable.transform.position = Slot.transform.position; // sets the position of the object to your hand position
                Pickable.transform.parent = Slot.transform; //makes the object become a child of the parent so that it moves with the hands
                Pickable.GetComponent<Rigidbody>().useGravity = false;
                hasItem = true;
            }
        }
        if (Input.GetKeyDown(GameSettings.THROW_ITEM_KEY) && hasItem == true) // drop
        {
            Collider[] collider = Pickable.GetComponents<Collider>();
            foreach (Collider c in collider)
            {
                c.enabled = true;
            }

            Pickable.GetComponent<Collider>().enabled = true;
            Pickable.GetComponent<Rigidbody>().isKinematic = false; // make the rigidbody work again
            Pickable.GetComponent<Rigidbody>().useGravity = true;

            Vector3 dir = transform.rotation * Vector3.forward;
            Pickable.GetComponent<Rigidbody>().AddForce(dir * throwForce);

            hasItem = false;
            Pickable.transform.parent = null; // make the object no be a child of the hands
        }
    }
    private void OnTriggerEnter(Collider other) // to see when the player enters the collider
    {
        if (other.gameObject.tag == "Pickable") //on the object you want to pick up set the tag to be anything, in this case "object"
        {
            canpickup = true;  //set the pick up bool to true
            Pickable = other.gameObject; //set the gameobject you collided with to one you can reference
        }
    }
    private void OnTriggerExit(Collider other)
    {
        canpickup = false; //when you leave the collider set the canpickup bool to false

    }
}
