using UnityEngine;
using System.Collections;

public class TargetingPlayer : MonoBehaviour {


	// motion parameters
	public float spinSpeed = 180.0f;
	public float power = 50f;
	public int scoreAmount = 0;
	// explosion when hit?
	public GameObject explosionPrefab;

	void Start(){
		
		//Getting the position of player
		Transform targetPosition = GameObject.Find("Player").GetComponent<Transform>();
		Transform currentPosition = gameObject.GetComponent<Transform> ();
		Vector3 direction = targetPosition.position - currentPosition.position;
		Debug.Log (direction);
		// if the projectile does not have a rigidbody component, add one
		if (!gameObject.GetComponent<Rigidbody>()) 
		{
			gameObject.AddComponent<Rigidbody>();
		}
		gameObject.GetComponent<Rigidbody> ().AddForce (direction * power, ForceMode.VelocityChange);
		Debug.Log (direction * power);

	}

	// when collided with another gameObject
	void OnCollisionEnter (Collision newCollision)
	{
		// exit if there is a game manager and the game is over
		if (GameManager.gm) {
			if (GameManager.gm.gameIsOver)
				return;
		}

		// only do stuff if hit by a projectile
		if (newCollision.gameObject.name == "Player") {
			Debug.Log ("Hit the player");
			if (explosionPrefab) {
				// Instantiate an explosion effect at the gameObjects position and rotation
				Instantiate (explosionPrefab, transform.position, transform.rotation);
			}

			// if game manager exists, make adjustments based on target properties
			if (GameManager.gm) {
				GameManager.gm.EndGame();
			}

			// destroy the projectile
			Destroy (newCollision.gameObject);

			// destroy self
			Destroy (gameObject);
		} else if (newCollision.gameObject.tag == "Projectile") {
			if (explosionPrefab) {
				// Instantiate an explosion effect at the gameObjects position and rotation
				Instantiate (explosionPrefab, transform.position, transform.rotation);
			}

			// if game manager exists, make adjustments based on target properties
			if (GameManager.gm) {
				GameManager.gm.targetHit (scoreAmount, 0);
			}

			// destroy the projectile
			Destroy (newCollision.gameObject);

			// destroy self
			Destroy (gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		gameObject.transform.Rotate (Vector3.up * spinSpeed * Time.deltaTime);
	}
}
