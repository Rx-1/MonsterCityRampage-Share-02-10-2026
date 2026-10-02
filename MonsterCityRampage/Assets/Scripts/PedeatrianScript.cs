using UnityEngine;
using System.Collections;

public class PedeatrianScript : MonoBehaviour {

	Rigidbody RB;
	Vector3 MyVelo;
	float Timer = 0;

	public float speed = 5;

	// Use this for initialization
	void Start () {
		RB = gameObject.GetComponent<Rigidbody> ();
		RB.rotation = Quaternion.Euler(0, Random.Range (0, 360), 0);
		MyVelo = speed * transform.forward;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Timer += Time.deltaTime;
		if (Timer > 5) {
			RB.rotation = Quaternion.Euler(0, Random.Range (0, 360), 0);
			Timer -= Random.Range(3f, 5f);
		}
		MyVelo = speed * transform.forward;
		RB.velocity = MyVelo + RB.velocity.y * Vector3.up;
	}
}
