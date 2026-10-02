using UnityEngine;
using System.Collections;

public class Victory : MonoBehaviour {

	public static int victoryPoints;
	public static bool hasWon = false;

	public GameObject Camera1;
	public Vector3 Camera1pos;
	public Vector3 Camera1rot;
	public GameObject Camera2;
	public Vector3 Camera2pos;
	public Vector3 Camera2rot;

	public GameObject throphy;
	public GameObject CitySound;
	public GameObject EndScreenUI;

	bool hasDoneVictory = false;

	public int pointLimit = 3000;
	public float anglesPerSecond = 30;

	// Use this for initialization
	void Start () {
		victoryPoints = pointLimit;
	}
	
	// Update is called once per frame
	void Update () {
		if (hasWon != hasDoneVictory) {
			Cursor.visible = true;
			hasDoneVictory = true;
			Camera1.transform.SetParent(transform);
			Camera1.transform.position = Camera1pos + transform.position;
			Camera1.transform.rotation = Quaternion.Euler(Camera1rot);
			Camera2.transform.SetParent(transform);
			Camera2.transform.position = Camera2pos + transform.position;
			Camera2.transform.rotation = Quaternion.Euler(Camera2rot);
			throphy.SetActive(true);
			CitySound.SetActive(false);
			EndScreenUI.SetActive(true);
		}
		if (hasWon) {
			transform.Rotate(0,anglesPerSecond * Time.deltaTime,0);
		}
	}
}
