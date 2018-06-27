using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootCount : MonoBehaviour {

	// Use this for initialization
	void Start () {

        int shots = FindObjectOfType<SpawnManager>().GetNumberOfShots();
        GameObject.Find("RocketIcon").GetComponentInChildren<Text>().text = "x " + shots.ToString();

    }
	
	public void updateButtonText () {
        int shots = FindObjectOfType<Player>().getNumberOfShots();
        GameObject.Find("RocketIcon").GetComponentInChildren<Text>().text = "x " + shots.ToString();
    }
}
