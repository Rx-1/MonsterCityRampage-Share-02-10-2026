using UnityEngine;
using System.Collections;

public class RandomPedestrianScript : MonoBehaviour {

	public GameObject[] pedestrianPrefab;
	float timer = 0;

	public void RandomPedestrian (Vector3 velo) {
		GameObject pedestrian = Instantiate(pedestrianPrefab[Random.Range(0, pedestrianPrefab.Length)]);
		pedestrian.transform.position = transform.position;
		pedestrian.GetComponent<Rigidbody>().velocity = velo;
		Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(timer > 1)
			Destroy (gameObject);
	}
}
