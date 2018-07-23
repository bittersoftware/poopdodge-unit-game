using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopController : MonoBehaviour {

    public GameObject poopSplash;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            Instantiate(poopSplash, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if (collision.tag == "Player")
        {
            Instantiate(poopSplash, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
