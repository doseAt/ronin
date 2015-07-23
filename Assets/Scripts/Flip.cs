using UnityEngine;
using System.Collections;

public class Flip : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
		// Multiply the player's x local scale by -1.

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
