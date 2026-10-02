using UnityEngine;
using System.Collections;

public class IDIEScript : MonoBehaviour {

	public static int slaughterAmount = 0;
	public static int slaughterMax = 300;
	static int slaughterMin = 100;

	public int myPointValue = 10;
	public GameObject Blood;

	public float lifeSpawn = 60;
	float timer = 0;

	void OnTriggerEnter (Collider killer) {
		killer.gameObject.GetComponentInParent<TestBallMovement> ().myPoints += myPointValue;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.down, out hit)) {
			GameObject BloodSplatter = Instantiate(Blood);
			BloodSplatter.transform.position = hit.point + (Vector3.up * 0.1f);
			BloodSplatter.transform.SetParent(hit.transform);
		}
		slaughterAmount -= 1;
		Destroy (transform.parent.gameObject);
	}

	void Awake () {
		slaughterAmount += 1;
		timer += Random.Range (0f, 1f);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (slaughterAmount > slaughterMin) {
			timer += Time.deltaTime;
			if (timer > lifeSpawn) {
				RaycastHit hit;
				if (Physics.Raycast(transform.position, Vector3.down, out hit)) {
					GameObject BloodSplatter = Instantiate(Blood);
					BloodSplatter.transform.position = hit.point + (Vector3.up * 0.1f);
					BloodSplatter.transform.SetParent(hit.transform);
				}
				slaughterAmount -= 1;
				Destroy (transform.parent.gameObject);
			}
		}
	}
}
