using UnityEngine;
using System.Collections;

public class UiScript : MonoBehaviour {

	bool OkQuit = false;

	public void Restart (){
		OkQuit = true;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (OkQuit == true) {
			Application.Quit();
		}
	}
}
