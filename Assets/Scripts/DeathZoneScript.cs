using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoneScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TakeDamage(collision);
            //Debug.Log(collision.gameObject.name);
        }
    }

    private void TakeDamage(Collider2D collision)
    {
        collision.GetComponent<Player>().TakeDamageFormAnotherPlayer(1000);
    }
}
