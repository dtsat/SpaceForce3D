using UnityEngine;
using System.Collections;

public class EnemyB2Movement : MonoBehaviour {

	public float shipSpeed;

	public float tilt;

	private Rigidbody rb;

	public Transform smoke;
	public Transform[] guns;

	public GameObject[] damageEffects;

	private bool alive;

	public GameObject shot;

	private AudioSource[] sounds;
	public AudioClip explo;

	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
		sounds = GetComponents<AudioSource> ();
		alive = true;
		rb.AddForce (Vector3.left * shipSpeed*1.25f);
		rb.AddForce (Vector3.back * shipSpeed*1.25f);
		StartCoroutine (attack ());

	}

	void takeDamage()
	{
		sounds [0].Play ();
		GameObject sparks = Instantiate (damageEffects [0], smoke.position, smoke.rotation) as GameObject;
		sparks.transform.SetParent (smoke);
		sparks.transform.localScale = new Vector3 (2f, 2f, 2f);
		GameObject smokes = Instantiate (damageEffects [1], smoke.position, smoke.rotation) as GameObject;
		smokes.transform.SetParent (smoke);
		smokes.transform.localScale = new Vector3 (2f, 2f, 2f);
		alive = false;
	}


	void FixedUpdate () 
	{ 
		if(alive)
			rb.rotation = Quaternion.Euler (180f, 35f, rb.velocity.x * -tilt);
		else {
			rb.AddForce (Vector3.down*50f);
			transform.Rotate (0, 0, 250 * Time.deltaTime);
		}
	}

	IEnumerator attack()
	{
		yield return new WaitForSeconds (Random.Range (1f, 6f));
		for(int i=0; i<3; i++)
		{
			if(alive)
			{
				sounds [1].Play ();
				Instantiate (shot, guns [0].position, guns [0].rotation);
				Instantiate (shot, guns [1].position, guns [1].rotation);
				yield return new WaitForSeconds (1f);
			}
		}
	}
			
		

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Ground")) 
		{
			AudioSource.PlayClipAtPoint (explo, Camera.main.transform.position);
			Instantiate (damageEffects[2], transform.position, damageEffects[2].transform.rotation);
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().killShip (300);
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
