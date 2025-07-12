using System.Collections.Generic;
using UnityEngine;

public class MarketPost : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> fruitPrefabs;

	[SerializeField]
	private Transform box1;
	[SerializeField]
	private Transform box2;
	[SerializeField]
	private Transform box3;
	
	private GameObject GetRandomFruitPrefab()
	{
		return fruitPrefabs[Random.Range(0, fruitPrefabs.Count)];
	}
	void Start()
	{
		if (fruitPrefabs.Count > 0)
		{
			Instantiate(GetRandomFruitPrefab(), box1.position, box1.rotation, box1);
			Instantiate(GetRandomFruitPrefab(), box2.position, box2.rotation, box2);
			Instantiate(GetRandomFruitPrefab(), box3.position, box3.rotation, box3);
		}
	}
}
