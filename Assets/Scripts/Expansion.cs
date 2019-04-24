using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expansion : MonoBehaviour {

    public GameObject leafPrefab;

    //public int maxLeafSpawnPerSecond = 15;
    public float leafSpawnPerFrame = 0.5f;
    public float rareLeafSpawn = 0.02f;
    [SerializeField]
    public List<GameObject> leaves;
    public int outsideLeaves;

    public GameObject[] flowersPrefab;
    public List<GameObject> flowers;
    public int leavesPerFlower = 60;

    public bool freezeExpansion;

	// Use this for initialization
	void Start () {
        outsideLeaves = 0;
	}
	
	// Update is called once per frame
	void Update () {
        //UpdateSpawnedLeafLastSecond();
        CleanList();
        
        if (!freezeExpansion)
            SpawnNewLeaf();

        SpawnFlower();
	}

    void SpawnNewLeaf()
    {
        if (Random.value < leafSpawnPerFrame)
        {
            //Looking for a leaf to spawn nearby
            //Need to be weighted toward recently spawned leaves : done
            //Need to handle an empty list : done
            GameObject newLeaf;
            int i = Random.Range(0, 15);

            if (i > leaves.Count)
            {
                Vector2 randomCircle = Random.insideUnitCircle.normalized * (leafPrefab.GetComponent<SpriteRenderer>().bounds.extents.x);
                newLeaf = Utils.InstantiateChild(leafPrefab, new Vector3(randomCircle.x, randomCircle.y), Quaternion.identity, this.transform);
                if (newLeaf.GetComponent<LeafManager>().isOutside)
                {
                    outsideLeaves++;
                }
            }
            else
            {
                leaves.Sort(delegate (GameObject x, GameObject y)
                {
                    return x.transform.childCount.CompareTo(y.transform.childCount);
                });
                
                newLeaf = Instantiate(leafPrefab, SpawnLocationNearby(leaves[i]), Utils.RandomRotation2D(leaves[i].transform), leaves[i].transform);
                if (newLeaf.GetComponent<LeafManager>().isOutside)
                {
                    outsideLeaves++;
                }
            }
            leaves.Add(newLeaf);
            
        }

        if (Random.value < rareLeafSpawn)
        {
            GameObject parent = leaves[Random.Range(0, leaves.Count)];
            GameObject newLeaf = Instantiate(leafPrefab, SpawnLocationNearby(parent), Utils.RandomRotation2D(parent.transform), parent.transform);
            if (newLeaf.GetComponent<LeafManager>().isOutside)
            {
                outsideLeaves++;
            }
            leaves.Add(newLeaf);
        }
    }

    void SpawnFlower()
    {
        if (leaves.Count > (flowers.Count + 1) * leavesPerFlower)
        {
            GameObject parent = leaves[Random.Range(0, leaves.Count)];
            GameObject randomFlower = flowersPrefab[Random.Range(0, flowersPrefab.Length)];
            GameObject newFlower = Instantiate(randomFlower, parent.transform);
            flowers.Add(newFlower);
        }
    }


    Vector3 SpawnLocationNearby(GameObject go)
    {
        return go.GetComponent<SpriteRenderer>().bounds.center;
    }

    void CleanList()
    {
        
        leaves.RemoveAll(leaf => leaf == null);
        
        foreach(GameObject leaf in leaves.FindAll(leaf => leaf.GetComponent<LeafManager>().isDead == true)) {
            if (leaf.GetComponent<LeafManager>().isOutside == true)
            {
                outsideLeaves--;
            }
            leaves.Remove(leaf);
        }

        flowers.RemoveAll(flower => flower == null);

    }

}
