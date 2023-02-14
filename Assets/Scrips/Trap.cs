using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerRunnerController player = other.gameObject.GetComponent<PlayerRunnerController>();

        if(player != null){ //tarkistetaan oliko se objekti mitä kosketettiin pelaaja
            player.Hit(); //kerrotaan pelaajalle ettö se osui ansaan
        }
    }
}
