using UnityEngine;
using System.Collections;

public class PowerUpSpin : MonoBehaviour {

	void Start()
	{
		GetComponent<Rigidbody> ().AddForce (Vector3.back*300f);
	}

	void FixedUpdate () 
	{
		transform.Rotate (-250 * Time.deltaTime, 0.0f, 0.0f);
	}
}
