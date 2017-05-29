using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, yMin, yMax;
}

public class MoveCrosshair : MonoBehaviour {

	public Boundary boundary;

	void FixedUpdate () 
	{
	
		float moveHoriz = Input.GetAxis ("Horizontal");
		float moveVert = Input.GetAxis ("Vertical");


		Vector3 movement = new Vector3 (moveHoriz, moveVert, 0.0f);
		Vector3 finalMovement = new Vector3 (moveHoriz, moveVert, 1f);
		transform.Translate(movement * Time.deltaTime * 10f);
	

		transform.position = new Vector3 (
			Mathf.Clamp (transform.position.x, boundary.xMin, boundary.xMax), 
			Mathf.Clamp (transform.position.y, boundary.yMin, boundary.yMax),
			3f
		);	
	}
}
