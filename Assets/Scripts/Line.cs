using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCol;
    public AudioSource deathAudioSource;

	// Use this for initialization
	void Start () {
        deathAudioSource = GameObject.Find("DeathAudioSource").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Leaf"))
        {
            other.gameObject.GetComponent<LeafManager>().TriggerDeath();
            deathAudioSource.Play();
        }
        
    }
}
