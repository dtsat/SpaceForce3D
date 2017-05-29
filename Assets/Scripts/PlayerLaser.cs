using UnityEngine;
using System.Collections;

public class PlayerLaser : MonoBehaviour {


	public float speed;
	public GameObject groundExplo;
	private GameObject crosshair;
	private Transform playership;
	private Transform target;
	private RaycastHit hit;

	void Start()
	{
		playership = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
		crosshair = GameObject.FindGameObjectWithTag ("Crosshair");
		if (crosshair != null) {
			target = crosshair.GetComponent<Transform> ();
			Vector3 camPos = Camera.main.transform.position;
			if (Physics.Raycast (camPos, (target.position - camPos), out hit, 200f)) 
			{
				Debug.DrawRay (Camera.main.transform.position, target.position);
				SetPosition (hit.point);
			}
			else
				SetPosition(target.position);
		}
		else
			GetComponent<Rigidbody> ().velocity = transform.forward * speed;

	}

	void SetPosition(Vector3 ray)
	{
		GetComponent<Rigidbody> ().velocity = (ray - transform.position).normalized * speed;
		transform.LookAt (ray);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Ground")) {
			Instantiate (groundExplo, transform.position, groundExplo.transform.rotation);
			Destroy (gameObject);
		}
	}
				
}
