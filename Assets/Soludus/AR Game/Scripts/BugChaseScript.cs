using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugChaseScript : MonoBehaviour
{

	public GameObject bug;

	public GameObject spawnPosition1;
	public GameObject spawnPosition2;
	public GameObject spawnPosition3;
	public GameObject spawnPosition4;
	public GameObject spawnPosition5;
	public GameObject spawnPosition6;
	public GameObject spawnPosition7;
	public GameObject spawnPosition8;

	public GameObject directionPosition1;
	public GameObject directionPosition2;
	public GameObject directionPosition3;
	public GameObject directionPosition4;
	public GameObject directionPosition5;

	private GameObject spawnedBug;
	public bool allowSpawn = true;

	public bool gameIsActive = false;

	private int randomSpot;
	private int oldSpot;
	private int olderSpot;

    public AudioSource bugsAudio;

	// Use this for initialization
	void OnEnable ()
	{
        bugsAudio.Play();

    }

    private void OnDisable()
    {
        bugsAudio.Stop();
    }

    // Update is called once per frame
    void Update ()
	{

		if (gameIsActive) {

			if (allowSpawn) {
			
				allowSpawn = false;
				StartCoroutine (RandomSpawner ());

			}
		}
	}

	// controls the spawning of bugs. Possibility of spawns: single bug (50%), 2 bugs (25%), 3 bugs (25%)
	IEnumerator RandomSpawner ()
	{

		int random = Random.Range (1, 3);

		if (random == 1) {
			SpawnSingleBug ();
		} else {
			SpawnMultipleBugs ();
		}

		int randomCooldown = Random.Range (3, 5);
		yield return StartCoroutine (SpawnCooldown (randomCooldown));

	}

	// spawns the first bug during Hippa's dialog
	public void SpawnFirstBug ()
	{
		Vector3 firstBugPos = spawnPosition1.transform.position;
		firstBugPos.x += 0.09f;
		spawnedBug = Instantiate (bug, firstBugPos, Quaternion.identity);
		spawnedBug.transform.LookAt (directionPosition2.transform.position);

	}

	// spawns a single bug
	public void SpawnSingleBug ()
	{
		//Debug.Log ("spawning single bug");
		GameObject randomedSpawn = RandomSpawnPosition ();
		while (randomedSpawn == null) {
			//Debug.Log ("lets random new spawn since its still null!");
			randomedSpawn = RandomSpawnPosition ();
		}
		GameObject randomedDir = RandomDirectionPosition ();
		//Debug.Log ("randomed pos: " + randomedSpawn + ", randomed dir: " + randomedDir);
		spawnedBug = Instantiate (bug, randomedSpawn.transform.position, Quaternion.identity);
		spawnedBug.transform.LookAt (randomedDir.transform.position);
	}

	// spawn either 2 or 3 bugs at the same time
	public void SpawnMultipleBugs ()
	{

		int amountOfBugs = Random.Range (2, 4);
		GameObject randomedSpawn;
		GameObject randomedDir;

		if (amountOfBugs == 2) {
			//Debug.Log ("spawning multiple bugs, amountOfBugs: " + amountOfBugs);
			for (int i = 0; i < amountOfBugs; i++) {
				randomedSpawn = RandomSpawnPosition ();
				while (randomedSpawn == null) {
					randomedSpawn = RandomSpawnPosition ();
				}
				randomedDir = RandomDirectionPosition ();
				spawnedBug = Instantiate (bug, randomedSpawn.transform.position, Quaternion.identity);
				spawnedBug.transform.LookAt (randomedDir.transform.position);
			}

		} else {
			//Debug.Log ("spawning multiple bugs, amountOfBugs: " + amountOfBugs);
			for (int i = 0; i < amountOfBugs; i++) {
				randomedSpawn = RandomSpawnPosition ();
				while (randomedSpawn == null) {
					randomedSpawn = RandomSpawnPosition ();
				}
				randomedDir = RandomDirectionPosition ();

				spawnedBug = Instantiate (bug, randomedSpawn.transform.position, Quaternion.identity);
				spawnedBug.transform.LookAt (randomedDir.transform.position);
			}
		}
	}

	// defines how often bugs are spawned
	IEnumerator SpawnCooldown (int randomCooldown)
	{

		yield return new WaitForSeconds (randomCooldown);
		allowSpawn = true;
	}

	// randomizes 1 out of 8 positions for bug's spawn location
	GameObject RandomSpawnPosition ()
	{
		olderSpot = oldSpot;
		oldSpot = randomSpot;

		randomSpot = Random.Range (1, 9);

		if (randomSpot == oldSpot && randomSpot < 8 || randomSpot == olderSpot && randomSpot < 8) {

			randomSpot++;

			if (randomSpot == oldSpot && randomSpot < 8 || randomSpot == olderSpot && randomSpot < 8) {

				randomSpot++;

			} else if (randomSpot == oldSpot && randomSpot == 8 || randomSpot == olderSpot && randomSpot == 8) {

				randomSpot = randomSpot - 2;

			}
		} else if (randomSpot == oldSpot && randomSpot == 8 || randomSpot == olderSpot && randomSpot == 8) {

			randomSpot--;

			if (randomSpot == oldSpot || randomSpot == olderSpot) {

				randomSpot--;

			}
		}

		if (randomSpot == 1) {
			return spawnPosition1;
		} else if (randomSpot == 2) {
			return spawnPosition2;
		} else if (randomSpot == 3) {
			return spawnPosition3;
		} else if (randomSpot == 4) {
			return spawnPosition4;
		} else if (randomSpot == 5) {
			return spawnPosition5;
		} else if (randomSpot == 6) {
			return spawnPosition6;
		} else if (randomSpot == 7) {
			return spawnPosition7;
		} else if (randomSpot == 8) {
			return spawnPosition8;
		} else
			return null;

	}

	// randomizes 1 out of 5 directions for the bug to follow
	GameObject RandomDirectionPosition ()
	{

		int randomDir = Random.Range (1, 6);

		if (randomDir == 1) {
			return directionPosition1;
		} else if (randomDir == 2) {
			return directionPosition2;
		} else if (randomDir == 3) {
			return directionPosition3;
		} else if (randomDir == 4) {
			return directionPosition4;
		} else {
			return directionPosition5;
		}
	}
}
