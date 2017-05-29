using UnityEngine;
using System.Collections;

public class EnemyAMovement : MonoBehaviour {

	public float shipSpeed;

	public float tilt;

	private Rigidbody rb;

	public Transform smoke;

	public GameObject[] damageEffects;

	private bool alive, warp;

	private AudioSource[] sounds;
	public AudioClip explo;

	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
		sounds = GetComponents<AudioSource> ();
		alive = true;
		rb.AddForce (Vector3.forward * shipSpeed*50f);
		warp = true;
	}

	void takeDamage()
	{
		sounds [0].Play ();
		GameObject sparks = Instantiate (damageEffects [0], smoke.position, smoke.rotation) as GameObject;
		sparks.transform.SetParent (smoke);
		GameObject smokes = Instantiate (damageEffects [1], smoke.position, smoke.rotation) as GameObject;
		smokes.transform.SetParent (smoke);
		alive = false;
	}
	

	void FixedUpdate () 
	{
		if (transform.position.z <= 100f && warp) 
		{
			sounds [1].Play ();
			rb.AddForce (Vector3.back * shipSpeed*45f);
			warp = false;
		} 
		if (!alive) {
			rb.AddForce (Vector3.down*10f);
			transform.Rotate (0, 0, 250 * Time.deltaTime);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Ground")) 
		{
			AudioSource.PlayClipAtPoint (explo, Camera.main.transform.position);
			Instantiate (damageEffects[2], transform.position, damageEffects[2].transform.rotation);
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().killShip (100);
			Destroy (gameObject);
		}
		if (other.CompareTag ("PlayerShot")) 
		{
			Destroy (other.gameObject);
			takeDamage ();
			rb.AddExplosionForce (2f, new Vector3(transform.position.x - Random.Range(-1f, 1f), transform.position.y, transform.position.z), 2f, 2f, ForceMode.Impulse);
		}
		if (other.CompareTag ("Player")) 
		{
			takeDamage ();
			rb.AddExplosionForce (1f, new Vector3(transform.position.x - Random.Range(-1f, 1f), transform.position.y, transform.position.z), 2f, 2f, ForceMode.Impulse);
		}
	}
}
