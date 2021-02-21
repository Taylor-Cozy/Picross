using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Board board;
    public InputField inputField;
    public TileManager tm;
    public Board boardPrefab;
    public bool GameSession = false;
    public GameObject levelUI;
    public GameObject winnerUI;
    public GameObject nextLevelUI;
    public GameObject levelSelectButton;
    public TMP_Text title;
    public string levelName = "";

    public Button[] levels;
    public Button currentLevel;

    public string solution = "0000000000000000000000000";

    // Start is called before the first frame update
    void Start()
    {
        //board.solution = solution;
    }


    // Update is called once per frame
    void Update()
    {
        if (GameSession)
        {
            // When mouse released, reset all tiles so they can be drawn on again
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                CheckSolution();
            }
        }
    }

    public void StartGame(Button b)
    {
        board = Instantiate(boardPrefab, new Vector2(0, 0), transform.rotation);
        board.width = b.GetComponentInChildren<LevelButton>().width;
        board.height = b.GetComponentInChildren<LevelButton>().height;
        board.solution = b.GetComponentInChildren<LevelButton>().solution;
        levelName = b.GetComponentInChildren<LevelButton>().title;
        board.tm = tm;
        board.begin();
        GameSession = true;
        levelUI.gameObject.SetActive(false);
        currentLevel = b;
    }

    public void SolutionToFile()
    {
        // Path to file
        string path = Application.dataPath + "/LevelFiles/" + board.getWidth() + "by" + board.getHeight() + "levels.txt";

        // Create file if it doesn't exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "");
        }
        
        // Content of file
        string sol = GenerateString();
        string line = board.getWidth() + " " + board.getHeight() + " " + sol + " " + inputField.text + "\n";
        inputField.text = "";
        board.ClearBoard();

        // Add the text
        File.AppendAllText(path, line);
    }

    string GenerateString()
    {
        string attempt = "";
        foreach (GameObject x in board.getTiles())
        {
            if (x.GetComponent<Tile>().getActive())
            {
                attempt += "1";
            }
            else
            {
                attempt += "0";
            }
        }

        return attempt;
    }

    public void CreateSolution()
    {
        Debug.Log(GenerateString());
    }

    public void endLevel()
    {
        if(board != null)
        {
            GameSession = false;
            board.DeleteBoard();
        }
    }

    bool CheckSolution()
    {
        List<List<int>> testRules = board.GenerateRules(GenerateString());
        List<List<int>> setRules = board.getRules();
        List<List<GameObject>> ruleTiles = board.getRuleTiles();

        bool winner = true;

        for(int i = 0; i < testRules.Count; i++)
        {
            bool satisfied = true;
            if(testRules[i].Count != setRules[i].Count)
            {
                satisfied = false;
            } else
            {
                for (int j = 0; j < testRules[i].Count; j++)
                {
                    if (testRules[i][j] != setRules[i][j])
                    {
                        satisfied = false;
                    }
                }
            }

            foreach (GameObject x in ruleTiles[i])
            {
                x.GetComponent<RuleTile>().setSatisfied(satisfied);
            }

            if (!satisfied)
                winner = false;
        }

        if (winner)
        {
            foreach(GameObject x in board.getTiles())
            {
                x.GetComponent<BoxCollider2D>().enabled = false;
            }
            winnerUI.SetActive(true);
            levelSelectButton.SetActive(true);
            title.text = levelName;
            nextLevelUI.SetActive(true);
            winnerUI.GetComponent<AudioSource>().Play();
            //Debug.Log(name);
        }

        return winner;
    }

    public void ReturnToLevelSelect()
    {
        winnerUI.SetActive(false);
        levelSelectButton.SetActive(false);
        nextLevelUI.SetActive(false);
    }

    public void nextLevel()
    {
        bool levelFound = false;
        foreach(Button b in levels)
        {
            if (levelFound)
            {
                endLevel();
                ReturnToLevelSelect();
                StartGame(b);
                break;
            }
            if (currentLevel == b)
            {
                levelFound = true;
            }
        }
    }
}
