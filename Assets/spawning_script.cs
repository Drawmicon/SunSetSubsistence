using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

/// <summary>
/// ERROR: spawning bigger than 3x3 or 2x4 will make the next spawned objects not destroyed on collision
/// </summary>
public class spawning_script : MonoBehaviour
{
	//public GameObject spawnPoint;
	public GameObject[] prefab;//dont reference scene prefab, otherwise it might get destroyed and the spawner wont know what to reference
	public float delayTimer;

	public Vector3[] positions;
	public string[] names;

	private int maxSpawn;
	private int currentSpawnCount;//current total of spawned objects from spawn point
	public int maxSpawnCount;//max spawn of spawn point

	public float objectSpacing = 1;
	public int itemRowSize;
	public int itemColumnSize;

	public float spawnRadius;
	RaycastHit hit;
	GameObject farm;

	public int prefabSelection;
	public bool autoSelection;

	public float[] timers;
	public float positionRadius;

	public bool randomPositions;

	// Start is called before the first frame update
	void Start()
	{
		
		positions = new Vector3[itemRowSize * itemColumnSize];
		timers = new float[itemRowSize * itemColumnSize];

		if (autoSelection)
		{
			prefabSelection = Random.Range(0, prefab.Length - 1);
		}

		maxSpawn = itemColumnSize * itemRowSize;

		currentSpawnCount = 0;
		farm = new GameObject();
		farm.name = this.name + ": Farm";

		int counter = 0;
		Vector3 temp = Vector3.zero;
		for(int i = 0; i < itemColumnSize; i++)
        {
			temp.z = (objectSpacing * i);
			for (int j = 0; j < itemRowSize; j++)
			{
				temp.x = (objectSpacing * j);

				//needs work, grid deforms at angle, instead make objects child of empty game object, then it will have same rotation

				positions[counter] = temp;//[i+j] saves to same position to wrong position, not good hash 0+1 == 1+0
				counter++;
				//Debug.Log("Temp(array) (" + i + ", " + j + ") : " + positions[counter] + "\n");
				//Debug.Log("Temp(Final) (" +  i + ", "+ j + ") : " + temp + "\n");
			}
		}
	}

	void circleSpawn()
	{
		for (int i = 0; i<maxSpawn; i++) { 
			Vector3 temp = new Vector3((Random.insideUnitCircle * spawnRadius).x, 0, (Random.insideUnitCircle * spawnRadius).y);
			positions[i] = temp + transform.position;
			if (Physics.CheckSphere(positions[i], objectSpacing))
            {
				positions[i]=Vector3.zero;
            }
		}
		
		//when destroyed, item will reference this script and state false on the array of bools for each position, 
		//it will find its position based on its name's last character, the number position
		for (int j = 0; j < positions.Length; j++)
		{
			if (positions[j] != Vector3.zero)
			{
				GameObject tempo = Instantiate(prefab[prefabSelection], positions[j], Quaternion.identity);
			}
		}
	}

	//randomly spawn in radius, doesnt spawn if object exist in position
	void cirleSpawnPositions()
	{
		//add ground.y + 1 to position, so terrain doesnt prevent object from being instantiated, only other objects would

		GameObject ground = GameObject.FindGameObjectWithTag("Ground");
		for (int i = 0; i < maxSpawn; i++)
		{
			Vector3 temp = new Vector3((Random.insideUnitCircle * spawnRadius).x, ground.transform.position.y+1, (Random.insideUnitCircle * spawnRadius).y);
			//temp += this.transform.localPosition;
			if (Physics.CheckSphere(positions[i], objectSpacing))
			{
				positions[i] = temp;
			}
			else
            {
				i--;
            }
		}
	}


	void Update()
	{
		if (currentSpawnCount < maxSpawnCount)
		{
			if (randomPositions)
			{
				cirleSpawnPositions();
			}

			if (farm.transform.childCount < maxSpawn)
			{
				for (int i = 0; i < positions.Length; i++)
				{

					if (farm.transform.Find(this.name + ": " + prefab[prefabSelection].name + i) == null)
					{
						if (timers[i] <= 0)
						{
							//if nothing exist at position
							//if (Physics.CheckSphere(positions[i], positionRadius))
							//{
							//Debug.Log("Child DNE: " + this.name + ": " + prefab[0].name + i);
							//create game objects at identity grid position
							//Debug.Log("Instantiate (" + i + ") : " + positions[i] + "\n");

							if (randomPositions)
							{
								cirleSpawnPositions();
								prefabSelection = Random.Range(0, prefab.Length - 1);
							}

							GameObject temp = Instantiate(prefab[prefabSelection], positions[i], Quaternion.identity);
							currentSpawnCount++;

							temp.name = this.name + ": " + prefab[prefabSelection].name + i;
							temp.tag = "Item";
							//set game objects as child of empty game object
							temp.transform.parent = farm.transform;

							//set positions as global position of temp object, based on local position
							//aka get local position of temp and translate to global position. then set that vector to positions array
							positions[i] = transform.TransformPoint(temp.transform.localPosition);


							//}
						}
						else
						{
							timers[i] -= Time.deltaTime;
						}
					}
				}
				//move and rotate emtpy game object to spawnpoint's position and y-axis rotation
				farm.transform.position = this.transform.position;
				farm.transform.eulerAngles = new Vector3(farm.transform.eulerAngles.x, this.transform.eulerAngles.y, farm.transform.eulerAngles.z);
			}

		}
	}
}
