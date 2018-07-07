using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {


    private float screenLimitLeft  = -3.2f;
    private float screenLimitRight = +3.2f;
    private float screenLimitTop   = +5.2f;
    [SerializeField]
    private GameObject _enemyExplosionPrefab;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        if(transform.position.x < screenLimitLeft || transform.position.x > screenLimitRight || transform.position.y > screenLimitTop)
        {

            FindObjectOfType<GameManager>().bulletsDestroyed++;
            Debug.Log("BulletControler - HEY BULLET DESTROYED!!" );
            Destroy(this.gameObject);
                       
        }

        transform.Translate(Vector2.up * 1.0f * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with: " + collision.name);
        if (collision.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
        else if (collision.tag == "Enemy")
        {
            Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
            FindObjectOfType<GameManager>().bulletsDestroyed++;

            Destroy(this.gameObject);
        }
    }

}
