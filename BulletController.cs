using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float bulletSpeed;

    private float screenLimitLeft  = -3.2f;
    private float screenLimitRight = +3.2f;
    private float screenLimitTop   = +5.2f;

    private Rigidbody2D rb2D;
    private Vector2 velocity;
    private float launchAngle;

    public float windSpeed;

    [SerializeField]
    private GameObject _enemyExplosionPrefab;

    // Use this for initialization
    void Start () {
        rb2D = gameObject.GetComponent<Rigidbody2D>();

        windSpeed = FindObjectOfType<GameManager>().wind;
        rb2D.AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);

        launchAngle = transform.rotation.eulerAngles.z;


    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if(transform.position.x < screenLimitLeft || transform.position.x > screenLimitRight || transform.position.y > screenLimitTop)
        {

            FindObjectOfType<GameManager>().bulletsDestroyed++;
            Destroy(this.gameObject);
                       
        }

        if ((Mathf.Abs(Mathf.Round(windSpeed * 100f) / 10f)) >= 1)
        {
            rb2D.AddForce(transform.right * windSpeed);
            transform.Rotate(Vector3.forward, -windSpeed/2);

            //Debug.Log("windbullet: " + windSpeed);

            if (Mathf.Abs(transform.rotation.eulerAngles.z) - launchAngle < 45)
            {
            }
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collided with: " + collision.name);
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
