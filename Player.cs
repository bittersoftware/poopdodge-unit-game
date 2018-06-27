using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public GameObject bulletEmitter;
    public GameObject Bullet;

    public float bulletPower = 1;
    public float bulletAngle;

    [Range (1f, 10f)][SerializeField]
    private float moveSpeed = 5.0f;
    [SerializeField]
    int numberOfShots;
    private float screenLimitLeft = -3.5f;
    private float screenLimitRight = +3.5f;
    [SerializeField]
    private float _fireRate = 0.7f;
    private float _canFire = 0.0f;





    //movement variables
    float directionX;
    Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        this.gameObject.SetActive(true);
        numberOfShots = FindObjectOfType<SpawnManager>().GetNumberOfShots();

        // groundRef = GameObject.Find("GroundRef").GetComponent<GroundRef>();


    }
	
	// Update is called once per frame
	void FixedUpdate () {              

        directionX = CrossPlatformInputManager.GetAxis("Horizontal");
        rb.velocity = new Vector2(directionX * moveSpeed, 0);

        if (transform.position.x < screenLimitLeft)
        {
            transform.position = new Vector2 (screenLimitLeft, transform.position.y);
        }
        else if (transform.position.x > screenLimitRight)
        {
            transform.position = new Vector2 (screenLimitRight, transform.position.y);
        }
	}

    public void Shoot()
    {

        if (Time.time > _canFire)
        {
            if (numberOfShots <= 0)
            {
                //TODO: Animate numer of shots of warning message - ZERO SHOTS! 
                return;
            }

            numberOfShots--;

            //GameObject temporaryBulletHandler;
            Instantiate(Bullet, bulletEmitter.transform.position, Quaternion.Euler(0, 0, bulletAngle));

            _canFire = Time.time + _fireRate;
        }
        


    }

    public void AdjustBulletPower(float newBulletPower)
    {

        bulletPower = newBulletPower;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Poop")
        {
            this.gameObject.SetActive(false);
            //Restart the game
            FindObjectOfType<GameManager>().GameOver();


        }
        else if (collision.tag == "Bullet")
        {

            this.gameObject.SetActive(false);
            //Restart the game
            FindObjectOfType<GameManager>().GameOver();
            

        }
    }


    public void setNumberOfShots(int _numberOfShots)
    {
        numberOfShots = _numberOfShots;
    }

    public int getNumberOfShots()
    {
        return numberOfShots;
    }
}
