using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlockSpawner : MonoBehaviour
{
    public GameObject movingBlock;
    public float maxY;
    public float minY;
    public float x;
    public float timeBetweenSpawn;
    private float spawnTime;
    //unityn sisällä määritetyn muuttujat
    
    void Update(){
        if(Time.time > spawnTime){ //jos aikaa on kulunut tiettä määrä
            Spawn(); //suoritetaan luomis komento
            spawnTime = Time.time + timeBetweenSpawn; //resetoidaan luomisaika
        }
    }

    void Spawn(){
        float randomY = Random.Range(minY, maxY);
        
        Instantiate(movingBlock, transform.position + new Vector3(x, randomY, 10), transform.rotation);
        //luodaan uusi liikkuva ansa sekalaisesti y-akselin mukaan
    }
}
