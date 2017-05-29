using UnityEngine;
using System.Collections;

public class EnemyShot : MonoBehaviour {

	public float speed;
	public GameObject groundExplo;
	private GameObject player;
	private Transform target;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		if (player != null) {
			target = player.GetComponent<Transform> ();
			SetPosition ();
		}
		else
			GetComponent<Rigidbody> ().velocity = transform.up * speed;

	}

	void SetPosition()
	{
		GetComponent<Rigidbody> ().velocity = (target.position - transform.position).normalized * speed;
		GetComponent<Rigidbody> ().rotation = Quaternion.LookRotation(target.position - transform.position);
	}

	void Update()
	{
		if (GameObject.FindGameObjectWithTag ("Player") == null)
			Destroy (gameObject);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Ground")) {
			//Instantiate (groundExplo, transform.position, groundExplo.transform.rotation);
			Destroy (gameObject);
		}
	}

}
