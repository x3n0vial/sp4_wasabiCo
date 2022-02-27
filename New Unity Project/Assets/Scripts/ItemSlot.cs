using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    bool hasItem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setHasItem(bool hasItem)
    {
        this.hasItem = hasItem;
    }

    public bool getHasItem()
    {
        return hasItem;
    }
}
