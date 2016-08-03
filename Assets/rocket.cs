using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class rocket : MonoBehaviour {
	private LevelManager levelManager;
	public GameObject fire;
	public GameObject explosion;
	public GameObject explosionSound;
	public GameObject rocketSound;
	public GameObject completeSound;
	public GameObject leftRCS;
	public GameObject rightRCS;
	private Vector3 offset;

	public float amount;
	public float startUpForce;
	public float startRightForce;
	public float startRotForce;
	public float speed;
	public float fuel = 100.2f;
	public float rcs = 100.1f;


	public bool rcsBusy = false;


	public Text xText;
	public Text yText;
	public Text rText;
	public Text statusText;
	public Text fuelText;
	public Text rcsText;

	public float seconds = 20;

	float xVelocity = 0;
	float yVelocity = 0;
	float rVelocity = 0;


	void Start () {
		fuel = 100.2f; //.2 over because on label update, 0.2f is taken away
		rcs = 100.1f; //.2 over because on label update, 0.2f is taken away
		SetVelocityText ();
		SetFuelText ();
		SetRCSText ();

		rcsBusy = false;

		levelManager = GameObject.FindObjectOfType<LevelManager> ();

		GetComponent<Rigidbody2D> ().AddForce (transform.up * startUpForce * Time.deltaTime);
		GetComponent<Rigidbody2D> ().AddForce (transform.right * startRightForce * Time.deltaTime);
		GetComponent<Rigidbody2D> ().AddTorque (startRotForce * Time.deltaTime);

	}

	/*public void MoveUp() {
		fire.SetActive (true);
		rocketSound.GetComponent<AudioSource>().Play();
		SetFuelText ();
		GetComponent<Rigidbody2D>().AddForce (transform.up * speed * Time.deltaTime);
	}*/


	void FixedUpdate () {
		rcsBusy = false;

		/*#if UNITY_ANDROID
		//Vector3 accReset = Vector3.zero;
		//accReset.y = Input.acceleration.y * .5f;
		transform.rotation = new Quaternion(transform.rotation.x,transform.rotation.y,Input.acceleration.y * .5f,transform.rotation.w);                                                   
		#endif*/
		explosion.transform.position = transform.position;


		fire.SetActive (false);
		rocketSound.GetComponent<AudioSource>().Pause();

		//#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
		if (Input.GetKey("w") && fuel > 0f) {
			fire.SetActive (true);
			rocketSound.GetComponent<AudioSource>().Play();
			SetFuelText ();
			GetComponent<Rigidbody2D>().AddForce (transform.up * speed * Time.deltaTime);
		}

		//#else
		for (int i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch(i).phase != TouchPhase.Ended) {
				fire.SetActive (true);
				rocketSound.GetComponent<AudioSource>().Play();
				SetFuelText ();
				GetComponent<Rigidbody2D>().AddForce (transform.up * speed * Time.deltaTime);      
			}
		}

		//#endif

		if (Input.GetKey ("a") && rcs > 0f) {
			GetComponent<Rigidbody2D> ().AddTorque (amount * Time.deltaTime);
			rightRCS.SetActive (true);
			leftRCS.SetActive (false);
			SetRCSText ();
			rcsBusy = true;
		}
		if (Input.GetKey ("d") && rcs > 0f) {
			GetComponent<Rigidbody2D> ().AddTorque (-amount * Time.deltaTime);
			leftRCS.SetActive (true);
			rightRCS.SetActive (false);
			SetRCSText ();
			rcsBusy = true;
		}
		if (Input.GetKeyUp ("a")) {
			rightRCS.SetActive (false);
			rcsBusy = false;
		}
		if (Input.GetKeyUp ("d")) {
			leftRCS.SetActive (false);
			rcsBusy = false;
		}

		//GetComponent<Rigidbody2D> ().AddTorque (Input.GetAxis("Horizontal") * -amount * Time.deltaTime);
			

		//fire.transform.position = transform.position + offset;
		//fire.transform.rotation = transform.rotation;

		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		Vector3 v3Velocity = rb.velocity;
		float v3Rotation = rb.angularVelocity;

		xVelocity = v3Velocity.x * 3;
		xVelocity = Mathf.Round(xVelocity);
		xVelocity = Mathf.Abs(xVelocity);

		yVelocity = v3Velocity.y * 3;
		yVelocity = Mathf.Round(yVelocity);
		yVelocity = Mathf.Abs(yVelocity);

		rVelocity = v3Rotation;


		SetVelocityText ();

		if (rVelocity > 3 && !Input.GetKey ("a") && !Input.GetKey ("d")) {
			GetComponent<Rigidbody2D> ().AddTorque (-30f * Time.deltaTime);
			leftRCS.SetActive (true);
			SetRCSText ();
		} else if (rVelocity < -3 && !Input.GetKey ("a") && !Input.GetKey ("d")) {
			GetComponent<Rigidbody2D> ().AddTorque (30f * Time.deltaTime);
			rightRCS.SetActive (true);
			SetRCSText ();
		} else if (rcsBusy == false) {
			rightRCS.SetActive (false);
			leftRCS.SetActive (false);
		}

		//if (!isBusy) {
		//	StartCoroutine (shortDelay ());
		//}


	}

	void SetVelocityText() {
		xText.text = "X: " + xVelocity.ToString () + "M/S";
		yText.text = "Y: " + yVelocity.ToString () + "M/S";
		rText.text = rVelocity.ToString();
	}
	void SetFuelText() {
		fuel = fuel - 0.2f;
		fuel = Mathf.Round(fuel * 10f) / 10f;
		fuelText.text = "FUEL: " + fuel.ToString () + "%";
	}
	void SetRCSText() {
		rcs = rcs - 0.1f;
		rcs = Mathf.Round(rcs * 10f) / 10f;
		rcsText.text = "RCS: " + rcs.ToString () + "%";
	}
		
	//IEnumerator shortDelay()
	//{

		//isBusy = true;

		//yield return new WaitForSeconds(0.2f);



		//isBusy = false;

	//}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Moon") && xVelocity + yVelocity > 2) {
			statusText.text = "YOU LOSE!";

			gameObject.SetActive (false);
			fire.SetActive (false);
			rocketSound.GetComponent<AudioSource>().Pause();
			explosion.SetActive (true);
			explosionSound.GetComponent<AudioSource>().Play();

			levelManager.LoseRestart ();
		}
		if (other.gameObject.CompareTag ("Platform") && xVelocity + yVelocity > 4) {
			statusText.text = "YOU LOSE!";

			gameObject.SetActive (false);
			fire.SetActive (false);
			rocketSound.GetComponent<AudioSource>().Pause();
			explosion.SetActive (true);
			explosionSound.GetComponent<AudioSource>().Play();

			levelManager.LoseRestart ();
		} 
		if (other.gameObject.CompareTag ("Platform") && xVelocity + yVelocity <= 4) {
			statusText.text = "LEVEL COMPLETED!";
			completeSound.GetComponent<AudioSource>().Play();
			levelManager.winLoad ();
		} 
		if (other.gameObject.CompareTag ("Border")){
			statusText.text = "COME BACK YA DICKHEAD";
		}
	}

}