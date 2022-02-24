using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomCult : MonoBehaviour
{
    public InventoryManager inventoryManager;

    GameObject mushroom1;
    GameObject mushroom2;
    GameObject mushroom3;
    GameObject mushroom4;

    bool shroom1 = false;
    bool shroom2 = false;
    bool shroom3 = false;
    bool shroom4 = false;

    // Start is called before the first frame update
    void Start()
    {
        mushroom1 = transform.Find("mushroomcult").gameObject;
        mushroom2 = transform.Find("mushroomcult (2)").gameObject;
        mushroom3 = transform.Find("mushroomcult (4)").gameObject;
        mushroom4 = transform.Find("mushroomcult (6)").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other == GameHandler.instance.player.GetComponent<CapsuleCollider>() && inventoryManager.counter > 0)
        {
            inventoryManager.ShowPlaceNotice();
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (shroom1 == false)
                {
                    mushroom1.SetActive(true);
                    shroom1 = true;
                    inventoryManager.counter--;
                    inventoryManager.HidePlaceNotice();
                }
                else if (shroom2 == false)
                {
                    mushroom2.SetActive(true);
                    shroom2 = true;
                    inventoryManager.counter--;
                    inventoryManager.HidePlaceNotice();
                }
                else if (shroom3 == false)
                {
                    mushroom3.SetActive(true);
                    shroom3 = true;
                    inventoryManager.counter--;
                    inventoryManager.HidePlaceNotice();
                }
                else if (shroom4 == false)
                {
                    mushroom4.SetActive(true);
                    shroom4 = true;
                    inventoryManager.counter--;
                    inventoryManager.HidePlaceNotice();
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == GameHandler.instance.player.GetComponent<CapsuleCollider>())
        {
            inventoryManager.HidePlaceNotice();
        }
    }
}