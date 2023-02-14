using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position ;
        position.y = (player.position + offset).y;
        transform.position = position;
        //muutetaan kameran sijaintia y-akselissa pelaajaobjektin mukaan.
    }
}
