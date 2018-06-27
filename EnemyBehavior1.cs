using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior1 : MonoBehaviour {
    [SerializeField]
    private float speed = 1.0f;
    private Vector2 vector2Direction;

    private float screenLimitLeft = -4f;
    private float screenLimitRight = +4f;

    private bool isPooped;
    private bool isGoingRight;

    private int poopTimes;
    private float[] poopArray;
    private int auxUp;
    private int auxDown;

    //private float randomPoop;
    public GameObject Poop;
    public GameObject PoopHole;



    // Use this for initialization
    void Start () {
        vector2Direction = Vector2.left;
        //randomPoop = Random.Range(-2.5f, 2.5f);

        isPooped = false;
        isGoingRight = false;
        poopTimes = 1;
        auxUp = 0;
        auxDown = poopTimes;
        PoopTimeBubbleSort(poopTimes);

	}
	
	// Update is called once per frame
	void Update () {
        //Starting direction
        transform.Translate(vector2Direction * speed  * Time.deltaTime);

        //Check direction
        //Check if already pooped
        if (isGoingRight == false)
        {

            if (!isPooped)
            {
                if (transform.position.x <= poopArray[auxDown - 1])
                {
                    PoopTime();

                    if (auxDown > 1)
                    {
                        auxDown--;
                    }
                    else
                    {
                        auxDown = poopTimes;
                        isPooped = true;
                    }

                }
            }



        }
        if (isGoingRight == true)
        {
            if (!isPooped)
            {
                if (transform.position.x >= poopArray[auxUp])
                {
                    PoopTime();

                    if (auxUp < poopArray.Length-1)
                    {
                        auxUp++;
                    }
                    else
                    {
                        auxUp = 0;
                        isPooped = true;
                    }

                }
            }
        }


        //Mirror and change direction when leaving screen limites
        //update direction bool
        //update ispooped bool to false
        if (transform.position.x < screenLimitLeft && isGoingRight == false)
        {
                       
            Vector2 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            vector2Direction = Vector2.right;
            //randomPoop = Random.Range(-3.5f, 3.5f);
            isGoingRight = true;
            isPooped = false;

        }
        else if (transform.position.x > screenLimitRight && isGoingRight == true)
        {
            Vector2 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            vector2Direction = Vector2.left;
            //randomPoop = Random.Range(-3.5f, 3.5f);
            isGoingRight = false;
            isPooped = false;
        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            //Enemy1 Death animation
            FindObjectOfType<GameManager>().bulletsDestroyed++;
            FindObjectOfType<GameManager>().CheckEndOfLevel();
            Destroy(this.gameObject);
        }
    }

    private void PoopTime()
    {
        GameObject temporaryPooptHandler;

        temporaryPooptHandler = Instantiate(Poop, PoopHole.transform.position, Quaternion.identity) as GameObject;

    }

    private void PoopTimeBubbleSort(int _poopTimes)
    {
        poopTimes = _poopTimes;
        poopArray = new float [poopTimes];

        //filling array
        for (int i = 0; i < poopTimes; i++)
        {
            poopArray[i] = Random.Range(-3.5f, 3.5f);
        }

        //bubble sort
        float temp = 0;

        for (int write = 0; write < poopArray.Length; write++)
        {
            for (int sort = 0; sort < poopArray.Length - 1; sort++)
            {
                if (poopArray[sort] > poopArray[sort + 1])
                {
                    temp = poopArray[sort + 1];
                    poopArray[sort + 1] = poopArray[sort];
                    poopArray[sort] = temp;
                }
            }
        }
    }

    

    public void SetPoopTimes(int _poopTimes)
    {
        poopTimes = _poopTimes;
    }

    public void SetSpeed (float _speed)
    {
        speed = _speed;
    }

}
