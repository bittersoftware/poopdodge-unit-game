using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheEnd : MonoBehaviour {


	public void Quit () {
        Application.Quit();
	}
	

	public void Restart () {
        SceneManager.LoadScene(1);
    }
}
