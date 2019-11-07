using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PlayerMovement : MonoBehaviour {

	Movement move;
	Vector3 initialPosition;
	// Use this for initialization
	void Start () {
		move = GetComponent<Movement> ();
		initialPosition = transform.position;
		move._dir = Movement.Direction.still;
	}

	public void restart(){
		transform.position = initialPosition;
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.R)) {
			SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1f;
		}

		if (Input.GetAxisRaw ("Vertical") > 0) {
			//move.move (new Vector2(0,1));

			if (move.checkDirectionClear (new Vector2(0,1))) {
				move._dir = Movement.Direction.up;
				transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90f));
			} else {
				//move._dir = Movement.Direction.still;
			}
		} else if (Input.GetAxisRaw ("Horizontal") > 0) {
			//move.move (new Vector2(1,0));

			if (move.checkDirectionClear (new Vector2(1,0))) {
				move._dir = Movement.Direction.right;
				transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f));
			} else {
				//move._dir = Movement.Direction.still;
			}
		} else if (Input.GetAxisRaw ("Vertical") < 0) {
			//move.move (new Vector2(0,-1));

			if (move.checkDirectionClear (new Vector2(0,-1))) {
				move._dir = Movement.Direction.down;
				transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 270f));
			} else {
				//move._dir = Movement.Direction.still;
			}
		} else if (Input.GetAxisRaw ("Horizontal") < 0) {
			//move.move (new Vector2(-1,0));

			if (move.checkDirectionClear (new Vector2(-1,0))) {
				move._dir = Movement.Direction.left;
				transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180f));
			} else {
				//move._dir = Movement.Direction.still;
			}
		} else {
			//move._dir = Movement.Direction.still;
		}
	}
}
