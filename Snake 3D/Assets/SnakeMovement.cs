using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SnakeMovement : MonoBehaviour {
	
	public GameObject Map;
	public GameObject TailPrefab;
	public GameObject FoodPrefab;
	public int maxFood;
	public float FoodSpawnDelay;
	public int FoodOnStart;
	public bool FoodOnAllFaces;
	public float SnakeMoveDelay;
	int currFood = 0;
	int food = 0;
	Vector3 dir = new Vector3 (0f, 5f, 0f);
	Quaternion lastRot;
	List<Transform> tail = new List<Transform>();
	Vector3 v;

	void Start () {
		InvokeRepeating ("Move", SnakeMoveDelay, SnakeMoveDelay);
		InvokeRepeating ("FoodSpawn", FoodSpawnDelay, FoodSpawnDelay);

		food += FoodOnStart;
	}

	void Update () {
		if (Input.GetKey (KeyCode.RightArrow) && lastRot != Quaternion.Euler (0, 0, 90)){
			transform.rotation = Quaternion.Euler (0, 0, 270);
		}
		else if (Input.GetKey (KeyCode.LeftArrow) && lastRot != Quaternion.Euler (0, 0, 270)){
			transform.rotation = Quaternion.Euler (0, 0, 90);
		}
		else if (Input.GetKey (KeyCode.UpArrow) && lastRot != Quaternion.Euler (0, 0, 180)){
			transform.rotation = Quaternion.Euler (0, 0, 0);
		}
		else if (Input.GetKey (KeyCode.DownArrow) && lastRot != Quaternion.Euler (0, 0, 0)){
			transform.rotation = Quaternion.Euler (0, 0, 180);
		}
	}

	void Move () {

		v = transform.position;

		transform.Translate (dir);
		lastRot = transform.rotation;

		if (food > 0) {
			GameObject g = (GameObject)Instantiate (TailPrefab, v, Quaternion.identity, Map.transform);
			tail.Insert (0, g.transform);
			food--;
		} else if (tail.Count > 0) {
			tail.Last ().position = v;

			tail.Insert (0, tail.Last ());
			tail.RemoveAt (tail.Count - 1);
		}
	}

	void OnTriggerEnter(Collider coll){
		if (coll.name.StartsWith ("Apple")) {
			food++;
			Destroy (coll.gameObject);
			currFood--;
		}
	}

	void FoodSpawn(){

		if (currFood < maxFood) {
			int a = (int)Random.Range (-10, 10);
			int b = (int)Random.Range (-10, 10);

			if (FoodOnAllFaces) {
				switch ((int)Random.Range (1, 6)) {
				case 1:
					Instantiate (FoodPrefab, new Vector3 (a * 5, b * 5, -55), Quaternion.identity, Map.transform);
					break;

				case 2:
					Instantiate (FoodPrefab, new Vector3 (a * 5, b * 5, 55), Quaternion.identity, Map.transform);
					break;

				case 3:
					Instantiate (FoodPrefab, new Vector3 (a * 5, -55, b * 5), Quaternion.identity, Map.transform);
					break;

				case 4:
					Instantiate (FoodPrefab, new Vector3 (a * 5, 55, b * 5), Quaternion.identity, Map.transform);
					break;

				case 5:
					Instantiate (FoodPrefab, new Vector3 (-55, a * 5, b * 5), Quaternion.identity, Map.transform);
					break;

				case 6:
					Instantiate (FoodPrefab, new Vector3 (55, a * 5, b * 5), Quaternion.identity, Map.transform);
					break;

				}
			} else {
				Instantiate (FoodPrefab, new Vector3 (a * 5, b * 5, -55), Quaternion.identity, Map.transform);
			}

			currFood++;
		}

	}
}