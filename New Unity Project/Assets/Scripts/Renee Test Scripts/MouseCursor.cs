using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public GameObject trailEffect;
    public GameObject empty;
    public float size;
    public float timeSpawn = 0.1f;
    GameObject obj;
    void Start()
    {
        //Cursor.visible = false;
    }
    void Update()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;
        if (timeSpawn <= 0)
        {

            obj = Instantiate(trailEffect, cursorPos, Quaternion.identity);
            timeSpawn = 0.1f;

            obj.transform.parent = empty.transform;
        }
        else
        {
            //decrease particles when not active
            timeSpawn -= Time.deltaTime;
        }
    }
}
