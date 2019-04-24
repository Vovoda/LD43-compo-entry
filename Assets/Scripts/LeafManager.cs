using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafManager : MonoBehaviour {

    [SerializeField]
    public float timeStamp;

    public bool isOutside;

    public bool isDead;
    public int deathTimer;
    public int deathDuration = 30;

    private void Awake()
    {
        isOutside = !isCanvasHere(this.transform.position);
    }

    // Use this for initialization
    void Start () {
        timeStamp = Time.time;
        isDead = false;
        deathTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (isDead)
        {
            if (deathTimer++ % 6 == 0)
            {
                this.GetComponent<SpriteRenderer>().enabled = !this.GetComponent<SpriteRenderer>().enabled;
            }
            if (deathTimer >= deathDuration)
            {
                Destroy(this.gameObject);
            }
        }
	}

    public void TriggerDeath()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        this.GetComponent<BoxCollider2D>().enabled = false;

        LeafManager[] leafManagers = GetComponentsInChildren<LeafManager>();
        foreach(LeafManager lm in leafManagers)
        {
            lm.TriggerDeath();
        }
        //call this function on all child, make sure there's no problem if called multiple times
    }



    bool isCanvasHere(Vector3 position)
    {
        Collider2D[] intersecting = Physics2D.OverlapCircleAll(position, 0.01f);
        foreach (Collider2D col in intersecting)
        {
            if (col.gameObject.CompareTag("Canvas"))
            {
                return true;
            }
        }
        return false;
    }
}
