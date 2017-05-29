using UnityEngine;
using System.Collections;

public class SpawnPowerUp : MonoBehaviour {

	public GameObject powerup, sparks;
	public Transform location;

	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.U)) 
		{
			GetComponent<AudioSource> ().Play ();
			Instantiate (sparks, location.position, location.rotation);
			StartCoroutine (spawn ());
		}
	}

	public IEnumerator spawn()
	{
		Instantiate (sparks, location.position, location.rotation);
		GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (2.5f);
		Instantiate (powerup, transform.position, transform.rotation);
	}
}
