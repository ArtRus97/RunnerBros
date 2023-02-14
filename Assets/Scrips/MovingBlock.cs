using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float speed = 3.0f;
    private float secondsToDestroy = 30f;

    void Start(){
        StartCoroutine(DestroySelf());
        //aloiteaan itsensä tuhoamis komento
    }

    void FixedUpdate(){
        Vector2 position = GetComponent<Rigidbody2D>().position;
        //tarkistetaan objektin nykyiden sijainti

        position.x = position.x - Time.deltaTime * speed;
        //muutetan sijaintia vasemmalle riippuen ajasta ja nopeudesta
        
        GetComponent<Rigidbody2D>().MovePosition(position);
        //kerrotaan uusi sijainti objektille
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(secondsToDestroy);
        Destroy(gameObject);
        //tukotaan objekti kun määritetty aika on kulunut
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerRunnerController player = other.gameObject.GetComponent<PlayerRunnerController>();

        if(player != null){ //tarkistetaan oliko objekti mitä kosketetiin pelaaja
            player.Hit(); //jos oli lähetään pelaajalle osuma komento
        }
        else{ //jos se ei ollut pelaaja tuhotaan objekti
            Destroy(gameObject);
        }
    }
}
