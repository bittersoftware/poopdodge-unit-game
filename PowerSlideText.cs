using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerSlideText : MonoBehaviour {

    Text powerSliderToText;
    Slider powerSlider;

    // Use this for initialization
    void Start () {
        powerSlider = GameObject.Find("PowerSlider").GetComponent<Slider>();
	}
	
	public void textUpdate (float value) {

        float powerRange = powerSlider.maxValue - powerSlider.minValue;
        



        powerSliderToText = GetComponent<Text>();
        powerSliderToText.text = Mathf.RoundToInt((value - powerSlider.minValue) / (powerRange / 100)) + "%";

    }


}
