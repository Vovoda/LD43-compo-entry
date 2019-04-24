using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject linePrefab;

    private Camera cam;

    private bool isClicking;
    private List<Vector2> mouseWorldPositions;

    public AudioSource slashAudioSource;

    public LevelManager[] levels;
    public int curLevel;

    public bool menuOn;
    public GameObject menuPanel;
    public Text resultText;
    public GameObject restartLevelButton;
    public GameObject nextLevelButton;
    public GameObject quitGameButton;

    public GameObject ui;

    public GameObject startScreen;

    public bool goingToNextLevel;

	// Use this for initialization
	void Start () {
        isClicking = false;
        mouseWorldPositions = new List<Vector2>();
        cam = Camera.main;
        menuOn = false;
        menuPanel.SetActive(false);
        foreach(LevelManager level in levels)
        {
            level.RestartLevel();
            level.levelPaused = true;
        }
        
        goingToNextLevel = false;
	}
	
	// Update is called once per frame
	void Update () {
		/*if  (Input.GetKeyDown(KeyCode.Space))
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            curLevel.RestartLevel();
        }*/
        if (goingToNextLevel)
        {
            GoToNextLevel();
        }
	}

    void OnGUI()
    {
        if (!menuOn)
        {
            Event currentEvent = Event.current;

            if (currentEvent.type == EventType.MouseDown || Input.touchCount > 0)
            {
                isClicking = true;
            }

            if (currentEvent.type == EventType.MouseUp || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended))
            {
                if (DragIsValid())
                {
                    CutLeaves();
                }
                else
                {
                    //feedback
                }
                mouseWorldPositions.Clear();
                isClicking = false;
            }

            if (isClicking)
            {
                Vector2 mousePos = new Vector2();

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
                mousePos.x = currentEvent.mousePosition.x;
                mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;

#else
                mousePos.x = Input.touches[0].position.x;
                mousePos.y = Input.touches[0].position.y;
#endif

                mouseWorldPositions.Add(cam.ScreenToWorldPoint(new Vector2(mousePos.x, mousePos.y)));
            }
        }
    }

    bool DragIsValid()
    {
        return true;
    }

    void CutLeaves()
    {
        GameObject myLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        Utils.SetLine(myLine, mouseWorldPositions[0], mouseWorldPositions.Last());
        slashAudioSource.Play();
    }

    public void LevelFinished(bool success)
    {
        menuOn = true;
        menuPanel.SetActive(true);
        if(success)
        {
            resultText.text = "You won !";
            restartLevelButton.SetActive(false);
            if (curLevel < levels.Length - 1)
            {
                nextLevelButton.SetActive(true);
                
            } else
            {
                nextLevelButton.SetActive(false);
                quitGameButton.SetActive(true);
                resultText.text = "Congrats ! You pass the qualification for the title of Lead Landscape Developpment Architect Manager. Thanks for playing !";
            }
                
        }
        else
        {
            resultText.text = "You lost...";
            restartLevelButton.SetActive(true);
            nextLevelButton.SetActive(false);
        }
    }

    public void RestartCurLevel()
    {
        menuOn = false;
        menuPanel.SetActive(false);
        levels[curLevel].RestartLevel();
    }

    public void NextLevel()
    {
        menuOn = false;
        menuPanel.SetActive(false);
        curLevel++;
        goingToNextLevel = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void GoToNextLevel()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 targetPos = new Vector3(levels[curLevel].transform.position.x, cam.transform.position.y, cam.transform.position.z);
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, targetPos, 1.0f);
        if (cam.transform.position == targetPos)
        {
            foreach (LevelManager level in levels)
            {
                level.RestartLevel();
                level.levelPaused = true;
            }
            RestartCurLevel();
            goingToNextLevel = false;
        }
    }

    public void StartGame()
    {
        startScreen.SetActive(false);
        goingToNextLevel = true;
        ui.SetActive(true);
    }
}
