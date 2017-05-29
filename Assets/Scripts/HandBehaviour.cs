using UnityEngine;
using System.Collections;

public class HandBehaviour : MonoBehaviour {


	public GameObject shot;
	public GameObject[] damageEffects;
	public Transform gun;
	public int hitPoints;
	private Rigidbody rb;

	private float offset;
	public float direction, modifier;

	private AudioSource[] sounds;
	public AudioClip explo;

	private bool alive;

	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
		sounds = GetComponents<AudioSource> ();
		alive = true;
		hitPoints = 25;
		StartCoroutine (attack ());
	}

	void Update()
	{
		offset = direction * Mathf.Sin (modifier);
		modifier += 0.025f;
	}
	

	void FixedUpdate () 
	{
		transform.Translate (new Vector3 (offset, 0.0f, 0.0f) * 5f * Time.deltaTime);
		if (!alive) {
			rb.AddForce(Vector3.down * 5f);
			transform.Rotate (0, 0, 400 * Time.deltaTime);
		}
	}

	void takeDamage()
	{
		if (hitPoints > 0) {
			sounds [0].Play ();
			hitPoints--;
			Instantiate (damageEffects [0], gun.position, gun.rotation);
		} else {
			alive = false;
			Instantiate (damageEffects [0], gun.position, gun.rotation);
		}
	}


	public IEnumerator attack()
	{
		while (alive) {
			yield return new WaitForSeconds (Random.Range (1f, 6f));
			sounds [2].Play ();
			yield return new WaitForSeconds (1f);
			for (int i = 0; i < 10; i++) {
				sounds [1].Play ();
				Instantiate (shot, gun.position, gun.rotation);
				yield return new WaitForSeconds (0.25f);
			}
		}
	}
		
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Ground")) 
		{
			AudioSource.PlayClipAtPoint (explo, Camera.main.transform.position);
			Instantiate (damageEffects[3], new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), damageEffects[3].transform.rotation);
			if(gameObject.CompareTag("leftHand"))
				GameObject.FindGameObjectWithTag("BossFight").GetComponent<BossFight>().setDead(1);
			else if(gameObject.CompareTag("rightHand"))
				GameObject.FindGameObjectWithTag("BossFight").GetComponent<BossFight>().setDead(2);
			Destroy (gameObject);
		}
		if (other.CompareTag ("PlayerShot")) 
		{
			if (alive) {
				Destroy (other.gameObject);
				takeDamage ();
			}
		}
	}
}
