using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public GameManager gameManager;

    public Expansion[] expansions;
    public GameObject[] canvas;

    public int flowersNeeded;

    public RectTransform lifebar;
    public Text scoreText;

    public float life = 100.0f;

    public bool levelPaused;
	// Use this for initialization
	void Start () {
        //scoreText.text = "0/" + flowersNeeded;
        foreach (Expansion exp in expansions)
        {
            exp.freezeExpansion = true;
        }
        //levelPaused = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (!levelPaused)
        {
            int totalFlower = 0;
            foreach (Expansion exp in expansions)
            {
                totalFlower += exp.flowers.Count;
            }
            scoreText.text = totalFlower + "/" + flowersNeeded;

            if (totalFlower >= flowersNeeded)
            {
                foreach (Expansion exp in expansions)
                {
                    exp.freezeExpansion = true;

                }
                //Next level
                gameManager.LevelFinished(true);
                levelPaused = true;
            }
            else
            {
                LoseLife();
                if (life <= 0.0f)
                {
                    //Level lost
                    foreach (Expansion exp in expansions)
                    {
                        exp.freezeExpansion = true;

                    }
                    gameManager.LevelFinished(false);
                    levelPaused = true;
                }
            }
        } else
        {
            foreach (Expansion exp in expansions)
            {
                exp.freezeExpansion = true;
            }
        }
	}

    public void RestartLevel()
    {
        foreach (Expansion exp in expansions)
        {
            foreach (Transform child in exp.transform)
            {
                Destroy(child.gameObject);
            }
        }
        life = 100.0f;

        foreach (Expansion exp in expansions)
        {
            exp.freezeExpansion = false;
            exp.outsideLeaves = 0;

        }
        levelPaused = false;
    }

    void LoseLife()
    {
        int totalLeavesOutside = 0;
        foreach (Expansion exp in expansions)
        {
            totalLeavesOutside += exp.outsideLeaves;
        }

        if (totalLeavesOutside == 0)
        {
            life = Mathf.Min(100.0f, life + 0.03f);
        }
        life =  Mathf.Max(0, life - totalLeavesOutside / 300.0f);
        lifebar.anchorMax = new Vector2(life / 100.0f, 1);
    }
}
