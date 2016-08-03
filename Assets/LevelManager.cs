using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour {
	private bool isBusy = false;
	public Text statusText;

	IEnumerator loseDelay()
	{

		isBusy = true;

		yield return new WaitForSeconds(2);

		Application.LoadLevel (Application.loadedLevel);

		isBusy = false;

	}

	IEnumerator winDelay()
	{

		isBusy = true;

		Debug.Log ("waiting!");

		yield return new WaitForSeconds(2);

		Debug.Log ("loading new level");


		Application.LoadLevel (Application.loadedLevel + 1);

		Debug.Log ("load attempted@");


		isBusy = false;

	}

	//private float seconds = 5;

	public void LoadLevel(string name) {
		Debug.Log ("Level load requested for: " + name);
		Application.LoadLevel (name);
	}
	public void QuitRequest() {
		Debug.Log ("I want to get off Mr. Bones' Wildride!");
		Application.Quit ();
	}
	public void LoseRestart () {
		if (!isBusy) {
			StartCoroutine (loseDelay ());
		}
	}
	public void winLoad () {
		if (!isBusy) {
			Debug.Log ("asking if busy");
			StartCoroutine (winDelay ());
		}
	}

	bool Pause = false;

	void Update()
	{

		if (Pause == false)
		{
			Time.timeScale = 1;
		}

		else 
		{
			Time.timeScale = 0;
		}


		if (Input.GetKeyDown(KeyCode.P))
		{
			if (Pause == true)
			{
				Pause = false;
				statusText.text = "";

			}

			else
			{
				Pause = true;
				statusText.text = "GAME PAUSED";
			}
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			Application.LoadLevel (Application.loadedLevel);
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			Application.LoadLevel ("Game");
		}

	}
}
