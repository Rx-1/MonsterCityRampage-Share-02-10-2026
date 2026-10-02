using UnityEngine;
using System.Collections;

public class BuildingScript : MonoBehaviour {

	public float health = 1000;
	float baseDamage = 0.1f;
	public int myPointValue = 100;
	
	public float memoryEraseSpeed = 0.01f;
	public float impactTolerance = 5;
	public float dragTolerance = 40;

	public GameObject pedestrianPrefab;
	public int pedestrianMin = 3;
	public int pedestrianMax = 6;

	public GameObject santaBusPrefab;
	public float santaChance = 0.1f;
	public GameObject ExplosionPrefab;
	public GameObject ExplosionSound;
	
	GameObject whoGetsPointsForMe = null;

	float timer = 0;

	Rigidbody RB;

	void OnCollisionEnter (Collision c) {
		if (c.collider.gameObject.tag == "Arm") {
			health -= c.gameObject.GetComponentInParent<TestBallMovement> ().swipeDamage;
		} else if (c.rigidbody == null || RB.mass / c.rigidbody.mass < 20) {
			float colliderMass;
			if (c.rigidbody == null) {
				colliderMass = Mathf.Infinity;
			} else {
				colliderMass = c.rigidbody.mass;
			}
			float impactMass = Mathf.Min(RB.mass, colliderMass);
			if(c.relativeVelocity.magnitude > impactTolerance) {
				health -= impactMass * c.relativeVelocity.magnitude * baseDamage;
			}
			if (c.gameObject.tag == "Body") {
				health -= c.gameObject.GetComponentInParent<TestBallMovement> ().ramDamage * Time.fixedDeltaTime;
			}
		}
		if (c.gameObject.layer == LayerMask.NameToLayer ("IKILL")) {
			whoGetsPointsForMe = c.gameObject;
		} else if (whoGetsPointsForMe == null && c.gameObject.GetComponent<BuildingScript> () != null && c.gameObject.GetComponent<BuildingScript> ().whoGetsPointsForMe != null) {
			whoGetsPointsForMe = c.gameObject.GetComponent<BuildingScript> ().whoGetsPointsForMe;
		}
		if (health < 0) {
			if (whoGetsPointsForMe != null) {
				whoGetsPointsForMe.GetComponentInParent<TestBallMovement> ().myPoints += myPointValue;
			}
			if (IDIEScript.slaughterAmount < IDIEScript.slaughterMax && pedestrianPrefab != null) {
			int pedestrianNumber = Random.Range(pedestrianMin, pedestrianMax + 1);
				for (int i = 0; i < pedestrianNumber; i ++) {
					if (IDIEScript.slaughterAmount < IDIEScript.slaughterMax) {
						GameObject pedestrian = Instantiate(pedestrianPrefab);
						pedestrian.transform.position = transform.position;
						pedestrian.GetComponent<RandomPedestrianScript>().RandomPedestrian (RB.velocity);
					} else {
						i = pedestrianNumber;
					}
				}
			}
			if (santaBusPrefab != null) {
				float chance = Random.Range(Mathf.Epsilon, 100f);
				if (chance <= santaChance) {
					GameObject santaBus = Instantiate(santaBusPrefab);
					santaBus.transform.position = transform.position;
					santaBus.GetComponent<Rigidbody>().velocity = RB.velocity;
				}
			}
			if (ExplosionPrefab != null) {
				GameObject Explosion = Instantiate(ExplosionPrefab);
				Explosion.transform.position = transform.position;
			}
			if (ExplosionSound != null) {
				GameObject ExplosionS = Instantiate (ExplosionSound);
				ExplosionS.transform.position = transform.position;
			}
			Destroy(gameObject);
		}
	}

	void OnCollisionStay (Collision c) {
		if (c.rigidbody == null || RB.mass / c.rigidbody.mass < 20) {
			float colliderMass;
			if (c.rigidbody == null) {
				colliderMass = Mathf.Infinity;
			} else {
				colliderMass = c.rigidbody.mass;
			}
			float impactMass = Mathf.Min(RB.mass, colliderMass);
			if(c.relativeVelocity.magnitude > dragTolerance) {
				health -= impactMass * c.relativeVelocity.magnitude * baseDamage * Time.fixedDeltaTime;
			}
			if (c.gameObject.tag == "Body") {
				health -= c.gameObject.GetComponentInParent<TestBallMovement> ().ramDamage * Time.fixedDeltaTime;
			}
		}
		if (c.gameObject.layer == LayerMask.NameToLayer ("IKILL")) {
			whoGetsPointsForMe = c.gameObject;
		} else if (whoGetsPointsForMe == null && c.gameObject.GetComponent<BuildingScript> () != null && c.gameObject.GetComponent<BuildingScript> ().whoGetsPointsForMe != null) {
			whoGetsPointsForMe = c.gameObject.GetComponent<BuildingScript> ().whoGetsPointsForMe;
		}
		if (health < 0) {
			if (whoGetsPointsForMe != null) {
				whoGetsPointsForMe.GetComponentInParent<TestBallMovement> ().myPoints += myPointValue;
			}
			if (IDIEScript.slaughterAmount < IDIEScript.slaughterMax && pedestrianPrefab != null) {
				int pedestrianNumber = Random.Range(pedestrianMin, pedestrianMax + 1);
				for (int i = 0; i < pedestrianNumber; i ++) {
					if (IDIEScript.slaughterAmount < IDIEScript.slaughterMax) {
						GameObject pedestrian = Instantiate(pedestrianPrefab);
						pedestrian.transform.position = transform.position;
						pedestrian.GetComponent<RandomPedestrianScript>().RandomPedestrian (RB.velocity);
					} else {
						i = pedestrianNumber;
					}
				}
			}
			if (santaBusPrefab != null) {
				float chance = Random.Range(Mathf.Epsilon, 100f);
				if (chance <= santaChance) {
					GameObject santaBus = Instantiate(santaBusPrefab);
					santaBus.transform.position = transform.position;
					santaBus.GetComponent<Rigidbody>().velocity = RB.velocity;
				}
			}
			if (ExplosionPrefab != null) {
				GameObject Explosion = Instantiate(ExplosionPrefab);
				Explosion.transform.position = transform.position;
			}
			if (ExplosionSound != null) {
				GameObject ExplosionS = Instantiate (ExplosionSound);
				ExplosionS.transform.position = transform.position;
			}
			Destroy(gameObject);
		}
	}

	void OnJointBreak () {

	}

	// Use this for initialization
	void Start () {
		RB = gameObject.GetComponent<Rigidbody> ();
	}
	
	void FixedUpdate () {
		if (RB.velocity.magnitude < memoryEraseSpeed && whoGetsPointsForMe != null) {
			timer += Time.deltaTime;
			if (timer > 1) {
				whoGetsPointsForMe = null;
			}
		} else {
			timer = 0;
		}
	}
}
