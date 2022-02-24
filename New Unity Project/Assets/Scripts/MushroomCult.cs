using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MushroomCult : MonoBehaviour
{
    public InventoryManager inventoryManager;

    bool hasPlayer = false;

    GameObject mushroom1;
    GameObject mushroom2;
    GameObject mushroom3;
    GameObject mushroom4;

    bool shroom1 = false;
    bool shroom2 = false;
    bool shroom3 = false;
    bool shroom4 = false;

    bool complete = false;

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
        if (Input.GetKeyDown(KeyCode.E) && hasPlayer == true)
        {
            if (shroom1 == false && inventoryManager.counter > 0)
            {
                mushroom1.SetActive(true);
                shroom1 = true;
                inventoryManager.counter--;
                inventoryManager.HidePlaceNotice();
            }
            else if (shroom2 == false && inventoryManager.counter > 0)
            {
                mushroom2.SetActive(true);
                shroom2 = true;
                inventoryManager.counter--;
                inventoryManager.HidePlaceNotice();
            }
            else if (shroom3 == false && inventoryManager.counter > 0)
            {
                mushroom3.SetActive(true);
                shroom3 = true;
                inventoryManager.counter--;
                inventoryManager.HidePlaceNotice();
            }
            else if (shroom4 == false && inventoryManager.counter > 0)
            {
                mushroom4.SetActive(true);
                shroom4 = true;
                inventoryManager.counter--;
                inventoryManager.HidePlaceNotice();
                complete = true;
            }
            else if (complete == true)
            {
                inventoryManager.HideLeaveNotice();
                SceneManager.LoadScene("SceneNightmare");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == GameHandler.instance.player.GetComponent<CapsuleCollider>())
        {
            hasPlayer = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other == GameHandler.instance.player.GetComponent<CapsuleCollider>() && inventoryManager.counter > 0)
        {
            inventoryManager.ShowPlaceNotice();
        }
        if (other == GameHandler.instance.player.GetComponent<CapsuleCollider>() && complete == true)
        {
            inventoryManager.ShowLeaveNotice();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == GameHandler.instance.player.GetComponent<CapsuleCollider>())
        {
            hasPlayer = false;
            inventoryManager.HidePlaceNotice();
            inventoryManager.HideLeaveNotice();
        }
    }
}