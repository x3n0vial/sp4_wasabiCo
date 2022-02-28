using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_POV : MonoBehaviour
{
    Transform player;

    bool m_IsPlayerInRange;

    private void Start()
    {
        player = GameHandler.instance.player.transform;
    }

    void OnTriggerEnter(Collider other)
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, player.position, out hit, -1)) //if behind wall, lose player
        {
            if (hit.transform.CompareTag("Player"))
            {
                if (other.transform == player)
                {
                    m_IsPlayerInRange = true;
                }
            }
        }
    }

    public bool GetPlayerInView()
    {
        return m_IsPlayerInRange;
    }

    public void SetPlayerInView(bool playerInView)
    {
        m_IsPlayerInRange = playerInView;
    }
}
