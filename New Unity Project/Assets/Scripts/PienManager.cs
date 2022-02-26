using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PienManager : MonoBehaviour
{
    public int noOfPien;
    public Text text_noOfPien;


    void Update()
    {
       text_noOfPien.text = noOfPien.ToString();
    }
    // Start is called before the first frame update
    public int getNoOfPien()
    {
        return noOfPien;
    }

    public void setNoOfPien(int n)
    {
        noOfPien = n;
    }

    public void subtractNoOfPien()
    {
        noOfPien--;
    }
}
