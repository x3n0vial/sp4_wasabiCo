using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PienManager : MonoBehaviour
{
    public int noOfPien;

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
