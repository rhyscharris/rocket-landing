using UnityEngine;
using System.Collections;

public class level2Script : MonoBehaviour {

	public GameObject rocket;

	public float initialForce;


	// Use this for initialization
	void Start () {
		rocket.GetComponent<Rigidbody2D> ().AddForce (transform.right * initialForce * Time.deltaTime);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
