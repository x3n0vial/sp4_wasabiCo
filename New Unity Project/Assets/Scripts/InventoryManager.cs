using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public int counter = 0;
    Text count;
    GameObject pickupNotice;
    GameObject placeNotice;

    // Start is called before the first frame update
    void Start()
    {
        count = transform.Find("Panel").Find("count").gameObject.GetComponent<Text>();
        pickupNotice = transform.Find("PickupNotice").gameObject;
        placeNotice = transform.Find("PlaceNotice").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        count.text = "x" + counter.ToString();
    }

    public void ShowPickupNotice()
    {
        pickupNotice.SetActive(true);
    }
    public void HidePickupNotice()
    {
        pickupNotice.SetActive(false);
    }
    public void ShowPlaceNotice()
    {
        placeNotice.SetActive(true);
    }
    public void HidePlaceNotice()
    {
        placeNotice.SetActive(false);
    }
    public void PlusOne()
    {
        counter++;
    }
    public void MinusOne()
    {
        counter--;
    }
}
