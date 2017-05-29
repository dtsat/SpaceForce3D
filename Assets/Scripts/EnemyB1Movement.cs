using UnityEngine;
using System.Collections;

public class EnemyB1Movement : MonoBehaviour {

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
		warp = true;
		rb.AddForce (Vector3.right * shipSpeed*2.5f);
		rb.AddForce (Vector3.back * shipSpeed*3f);
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
		if(alive)
			rb.rotation = Quaternion.Euler (180f, -35f, rb.velocity.x * -tilt);
		else 
		{
			rb.AddForce (Vector3.down*150f);
			transform.Rotate (0, 0, 250 * Time.deltaTime);
		}
		if (alive && transform.position.x > -1f && warp) {
			warp = false;
			sounds [1].Play ();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Ground")) 
		{
			AudioSource.PlayClipAtPoint (explo, Camera.main.transform.position);
			Instantiate (damageEffects[2], transform.position, damageEffects[2].transform.rotation);
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().killShip (500);
			Destroy (gameObject);
		}
		if (other.CompareTag ("PlayerShot")) 
		{
			Destroy (other.gameObject);
			takeDamage ();
			rb.AddExplosionForce (1f, new Vector3(transform.position.x - Random.Range(-1f, 1f), transform.position.y, transform.position.z), 1f, 1f, ForceMode.Impulse);
		}
		if (other.CompareTag ("Player")) 
		{
			takeDamage ();
			rb.AddExplosionForce (1f, new Vector3(transform.position.x - Random.Range(-1f, 1f), transform.position.y, transform.position.z), 1f, 1f, ForceMode.Impulse);
		}
	}
}
