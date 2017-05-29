using UnityEngine;
using System.Collections;

public class GroundScroll : MonoBehaviour {

	public Vector2 speed;

	private Material mat;

	void Start()
	{
		mat = GetComponent<MeshRenderer> ().material;
	}
		

	// Update is called once per frame
	void FixedUpdate () 
	{
		mat.mainTextureOffset += speed*Time.deltaTime;
	}
}
