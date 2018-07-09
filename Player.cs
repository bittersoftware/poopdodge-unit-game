using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public GameObject bulletEmitter;
    public GameObject Bullet;
    public GameObject farBackground;
    public GameObject midBackground;
    public GameObject closeBackground;

    public float bulletPower = 1;
    public float bulletAngle;
    [SerializeField]
    private GameObject playerArm;
    [Range (1f, 10f)][SerializeField]
    private float moveSpeed = 5.0f;
    [SerializeField]
    int numberOfShots;
    private float screenLimitLeft = -2.9f;
    private float screenLimitRight = +2.9f;
    [SerializeField]
    private float _fireRate = 0.7f;
    private float _canFire = 0.0f;
    private Animator playerAnimator;
    private bool isLookingLeft = false;





    //movement variables
    float directionX;
    Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        this.gameObject.SetActive(true);
        numberOfShots = FindObjectOfType<SpawnManager>().GetNumberOfShots();
        playerAnimator.SetBool("dead", false);
        playerAnimator.SetBool("timeout", false);
        //playerArm = GameObject.Find("PlayerArm").GetComponent<GameObject>();

        // groundRef = GameObject.Find("GroundRef").GetComponent<GroundRef>();


    }
	
	// Update is called once per frame
	void FixedUpdate () {              

        directionX = CrossPlatformInputManager.GetAxis("Horizontal");
        rb.velocity = new Vector2(directionX * moveSpeed, 0);

        if (directionX > 0)
        {
            if (isLookingLeft == true)
            {
                flipPlayer();
            }
            playerAnimator.SetBool("walk", true);
            //Parallax Effect Left
            if (transform.position.x < screenLimitRight)
            {
                closeBackground.transform.Translate(Vector2.left * 0.4f * Time.deltaTime);
                midBackground.transform.Translate(Vector2.left * 0.2f * Time.deltaTime);
                farBackground.transform.Translate(Vector2.left * 0.1f * Time.deltaTime);
            }
        }
        else if (directionX < 0)
        {
            if (isLookingLeft == false)
            {
                flipPlayer();
            }
            playerAnimator.SetBool("walk", true);
            //Parallax Effect right
            if (transform.position.x > screenLimitLeft)
            {
                closeBackground.transform.Translate(Vector2.right * 0.4f * Time.deltaTime);
                midBackground.transform.Translate(Vector2.right * 0.2f * Time.deltaTime);
                farBackground.transform.Translate(Vector2.right * 0.1f * Time.deltaTime);
            }
        }
        else if (directionX == 0)
        {
            playerAnimator.SetBool("walk", false);
        }


        if (transform.position.x < screenLimitLeft)
        {
            transform.position = new Vector2 (screenLimitLeft, transform.position.y);
        }
        else if (transform.position.x > screenLimitRight)
        {
            transform.position = new Vector2 (screenLimitRight, transform.position.y);
        }

        //rotate player arm
        playerArm.transform.rotation =  Quaternion.Euler(0, 0, bulletAngle);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Poop")
        {
            playerAnimator.SetBool("dead", true);
            Invoke("gameOver", 1.5f);
        }
    }

    private void gameOver()
    {
        this.gameObject.SetActive(false);
        //Restart the game
        FindObjectOfType<GameManager>().GameOver();
    }


    private void flipPlayer()
    {
        isLookingLeft = !isLookingLeft;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);

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

            FindObjectOfType<GameManager>().IncrementTotalShots();
            numberOfShots--;
            playerAnimator.SetBool("shoot", true);
            playerArm.GetComponent<Renderer>().enabled = true;
            Invoke("ShootAnim", 0.25f);


        }  
    }

    public void timeoutAnim()
    {
        playerAnimator.SetBool("timeout", true);
        Invoke("gameOver", 1.5f);
    }

    private void ShootAnim()
    {
        

        //GameObject temporaryBulletHandler;
        Instantiate(Bullet, bulletEmitter.transform.position, Quaternion.Euler(0, 0, bulletAngle));

        playerAnimator.SetBool("shoot", false);
        playerArm.GetComponent<Renderer>().enabled = false;

        _canFire = Time.time + _fireRate;
    }

    public void AdjustBulletPower(float newBulletPower)
    {

        bulletPower = newBulletPower;

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
