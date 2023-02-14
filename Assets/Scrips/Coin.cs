using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerRunnerController controller = other.gameObject.GetComponent<PlayerRunnerController>();

        if (controller != null)
        {
            controller.AddCoin();
            Destroy(gameObject);
        }
    }
}
