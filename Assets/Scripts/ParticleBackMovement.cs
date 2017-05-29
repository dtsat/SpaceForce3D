using UnityEngine;
using System.Collections;

public class ParticleBackMovement : MonoBehaviour {


	void FixedUpdate () 
	{
		transform.Translate (Vector3.up * 10f * Time.deltaTime);
	}
}
