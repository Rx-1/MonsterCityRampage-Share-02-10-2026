using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestBallMovement : MonoBehaviour {

	CapsuleCollider CapColl;
	Rigidbody RB;
	Animator Anim;

	bool punch = false;
	bool uppercut = false;
	bool jump = false;
	float jumpTimer = 0;
	float raycastLength = 10;
	int latePoints = 0;
	public int myPoints = 0;

	public Slider PointSlider;
	public Text ScoreText;

	public float jumpDelay = 1;
	float jumpDelayTimer = 0;
	//
	public Animator Spanimator;
	public Animator Karmimator;

	Animator Monstermator;

	public float swipeDamage = 100;
	public float ramDamage = 50;

	public KeyCode advanceKey = KeyCode.UpArrow;
	public KeyCode backDownKey = KeyCode.DownArrow;
	public KeyCode turnRightKey = KeyCode.RightArrow;
	public KeyCode turnLeftKey = KeyCode.LeftArrow;
	public KeyCode jumpKey = KeyCode.Space;
	public KeyCode punchKey = KeyCode.Z;
	public KeyCode upperCutKey = KeyCode.X;

	public float speed = 10;
	public float AccelerationAngular = 10;
	public float jumpSpeed = 50;
	public float raycastLengthPercent = 110;

	public float stunTime = 3;
	public float stunInvunerability = 3;

	float stunTimer;

	void OnCollisionEnter (Collision c) {
		if (c.collider.gameObject.tag == "Arm" && stunTimer > stunTime + stunInvunerability) {
			stunTimer = 0;
			RB.constraints = RigidbodyConstraints.None;
		}
	}

	// Use this for initialization
	void Start () {
		stunTimer = stunTime + stunInvunerability;

		Cursor.visible = false;

		CapColl = gameObject.GetComponent<CapsuleCollider> ();
		RB = gameObject.GetComponent<Rigidbody> ();
		Anim = gameObject.GetComponent<Animator> ();
		if (Spanimator.isActiveAndEnabled) {
			Monstermator = Spanimator;
		} else {
			Monstermator = Karmimator;
		}
	}

	// Update is called once per frame
	void Update () {


		ScoreText.text = myPoints.ToString();
		PointSlider.value = myPoints;

		Monstermator.SetBool ("StunBool", false);

		Monstermator.SetBool ("WalkBool", false);

		Monstermator.SetBool ("LandingBool", false);

		if (stunTimer <= stunTime) {
			Monstermator.SetBool ("StunBool", true);
		}

		if (Physics.SphereCast (new Ray(transform.position, Vector3.down), CapColl.radius * transform.localScale.x, raycastLength) && stunTimer > stunTime){
			Monstermator.SetBool("LandingBool", true);
		}
		
		if ((Input.GetKey (backDownKey) != Input.GetKey (advanceKey) || Input.GetKey (turnLeftKey) != Input.GetKey (turnRightKey)) && Physics.SphereCast (new Ray(transform.position, Vector3.down), CapColl.radius * transform.localScale.x, raycastLength) && !Victory.hasWon && stunTimer > stunTime){
			Monstermator.SetBool("WalkBool", true);
		}
		if (Input.GetKeyDown (punchKey) && !Victory.hasWon && stunTimer > stunTime) {
			Monstermator.SetTrigger("LeftPunchTrigger");
			punch = true;
		}
		if (Input.GetKeyDown (upperCutKey) && !Victory.hasWon && stunTimer > stunTime) {
			Monstermator.SetTrigger ("UppercutTrigger");
			uppercut = true;
		}
		if (Input.GetKeyDown (jumpKey) && Physics.SphereCast (new Ray(transform.position, Vector3.down), CapColl.radius * transform.localScale.x, raycastLength) && jump == false && !Victory.hasWon && stunTimer > stunTime) {
			Monstermator.SetTrigger ("JumpTrigger");
			jump = true;
		}
	}
	
	// FixedUpdate is called once per physics update
	void FixedUpdate () {

		if (myPoints >= Victory.victoryPoints) {
			Victory.hasWon = true;
		}

		RB.constraints = RigidbodyConstraints.None;
		if (myPoints > latePoints) {
			float change = myPoints - latePoints;
			RB.mass += change;
			swipeDamage += change;
			ramDamage += change / 10;
			change = Mathf.Pow(change, 1f / 5f) / 50;
			transform.localScale += new Vector3 (change, change, change);
			transform.position += Vector3.up * ((change * CapColl.height) / 2);
			latePoints = myPoints;
		}

		raycastLength = (((transform.localScale.y * CapColl.height)) / 2) - (CapColl.radius * transform.localScale.x) + 0.1f;// * (raycastLengthPercent / 100f);
		jumpTimer += Time.deltaTime;

		if (Physics.SphereCast (new Ray(transform.position, Vector3.down), CapColl.radius * transform.localScale.x, raycastLength) && jumpTimer > 1 && stunTimer > stunTime) {
			RB.constraints = RigidbodyConstraints.FreezePositionY;
		}

		RB.velocity = Vector3.zero + RB.velocity.y * Vector3.up;
		Vector3 direction = Vector3.zero;
		if (Input.GetKey (advanceKey) && !Victory.hasWon && stunTimer > stunTime) {
			direction += transform.forward;
		}
		if (Input.GetKey (backDownKey) && !Victory.hasWon && stunTimer > stunTime){
			direction += transform.forward * -1;
		}
		if (Input.GetKey (turnLeftKey) && !Victory.hasWon && stunTimer > stunTime) {
			if (Physics.SphereCast (new Ray(transform.position, Vector3.down), CapColl.radius * transform.localScale.x, raycastLength)) {
				Quaternion deltaRotation = Quaternion.Euler(Vector3.up * -AccelerationAngular * Time.deltaTime);
				RB.MoveRotation(RB.rotation * deltaRotation);
			} else {
				direction += transform.right * -1;
			}
		}
		if (Input.GetKey (turnRightKey) && !Victory.hasWon && stunTimer > stunTime){
			if (Physics.SphereCast (new Ray(transform.position, Vector3.down), CapColl.radius * transform.localScale.x, raycastLength)) {
				Quaternion deltaRotation = Quaternion.Euler(Vector3.up * AccelerationAngular * Time.deltaTime);
				RB.MoveRotation(RB.rotation * deltaRotation);
			} else {
				direction += transform.right;
			}
		}
		RB.velocity = direction.normalized * speed + RB.velocity.y * Vector3.up;
		RB.angularVelocity = Vector3.zero;
		if (punch) {
			Anim.SetTrigger("MonsterTrigger");
			punch = false;
		}
		if (uppercut) {
			Anim.SetTrigger("MonsterTrigger2");
			uppercut = false;
		}

		if (jump) {
			jumpDelayTimer += Time.deltaTime;
			if (jumpDelayTimer >= jumpDelay) {
				jumpTimer = 0;
				RB.constraints = RigidbodyConstraints.None;
				RB.velocity += Mathf.Sqrt(2 * Physics.gravity.magnitude * jumpSpeed * transform.localScale.y * CapColl.height) * Vector3.up - RB.velocity.y * Vector3.up;
				jump = false;
				jumpDelayTimer = 0;
			}
		}

		RB.rotation = Quaternion.Euler(0, RB.rotation.eulerAngles.y, 0);
		stunTimer += Time.deltaTime;
	}
}
