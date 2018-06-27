using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IncreaseAngle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    UnityEngine.UI.Button button;
    private BulletEmissor bulletEmissor;


    // Use this for initialization
    void Start () {
        bulletEmissor = GameObject.Find("BulletEmissor").GetComponent<BulletEmissor>();
    }

    void Update()
    {
        if (!ispressed)
            return;
        // DO SOMETHING HERE
        //Debug.Log("Pressed");
        //Debug.Log(bulletEmissor.transform.name);
        bulletEmissor.IncreaseAngle();
    }
    bool ispressed = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        ispressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ispressed = false;
    }

}
