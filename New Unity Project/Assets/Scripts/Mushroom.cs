using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public InventoryManager inventoryManager;
    bool pickedup = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (pickedup == true)
        {
            gameObject.SetActive(false);
            inventoryManager.PlusOne();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == GameHandler.instance.player.GetComponent<CapsuleCollider>())
        {
            inventoryManager.ShowPickupNotice();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other == GameHandler.instance.player.GetComponent<CapsuleCollider>())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                pickedup = true;
                inventoryManager.HidePickupNotice();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == GameHandler.instance.player.GetComponent<CapsuleCollider>())
        {
            inventoryManager.HidePickupNotice();
        }
    }
}