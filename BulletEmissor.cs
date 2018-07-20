using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmissor : MonoBehaviour {

    private Player player;

    public float angle = 0;
    private float maxAngle = +45f;
    private float minAngle = -45f;

    //References for the pivot position of the UI elements
    //public RectTransform targetAnchor;
    Transform selfAnchor;


    //Set the rotating elements RectTransform
    void Start()
    {
        selfAnchor = GetComponent<Transform>();
        player = GameObject.Find("Player").GetComponent<Player>();

    }

    //Update the UI elements rotation
    void Update()
    {
        
        selfAnchor.rotation = Quaternion.Euler(0, 0, angle);
        player.bulletAngle = angle;


    }

    public void DecreaseAngle()
    {
        if (angle > maxAngle)
        {
            return;
        }
        angle = angle + 2;
    }

    public void IncreaseAngle()
    {
        if (angle < minAngle)
        {
            return;
        }
        angle = angle - 2;
    }

    public float getAngle()
    {
        return angle;
    }
}
