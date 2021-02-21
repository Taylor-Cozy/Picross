using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

/* 
 
Scale the label boxes so they fit the font better.

*/
public class Board : MonoBehaviour
{
    public GameObject RuleTile;
    public GameObject tilePrefab;
    public int width, height;
    public float scaleSize = 0.8f;

    public TileManager tm;

    private GameObject[] tiles;
    
    private List<List<int>> rules;
    private List<List<GameObject>> ruleTiles;

    private bool clearing = false;
    private bool blocking = false;

    public string solution = "";

    public void begin()
    {
        
        ruleTiles = new List<List<GameObject>>();
        rules = GenerateRules(solution);
        CreateBoardUI();
        GenerateRuleButtons();
    }

    public int getWidth()
    {
        return width;
    }

    public int getHeight()
    {
        return height;
    }

    #region Drawing Pen Logic
    public void setClearing()
    {
        clearing = true;
    }

    public bool getClearing()
    {
        return clearing;
    }

    public void setBlocking()
    {
        blocking = true;
    }

    public bool getBlocking()
    {
        return blocking;
    }
    #endregion

    #region Board/Tile Setup
    public void CreateBoardUI()
    {
        ScaleTiles(); // Scale the tiles to fit the screen
        
        tiles = new GameObject[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                tiles[x + (y * width)] = Instantiate(tilePrefab, GetTileOffset(x,y), transform.rotation);
                tiles[x + (y * width)].transform.SetParent(transform);
                tiles[x + (y * width)].GetComponent<Tile>().tm = tm;
                tiles[x + (y * width)].GetComponent<Tile>().SetDefaultColor(y % 2 == 0 ? (x % 2 == 0) : (x % 2 == 1));
                tiles[x + (y * width)].name = "Tile " + (x + (y * width));
            }
        }
    }

    public List<List<int>> getRules()
    {
        return rules;
    }

    public List<List<GameObject>> getRuleTiles()
    {
        return ruleTiles;
    }

    private Vector2 GetTileOffset(int x, int y)
    {
        Vector2 startPos = transform.position;
        Vector2 tileOffset = tilePrefab.GetComponent<SpriteRenderer>().bounds.size;

        float xPos, yPos;
        float longestRuleCol, longestRuleRow;
        longestRuleRow = GetLongestRule(0, width).x;
        longestRuleCol = GetLongestRule(width, rules.Count).x;

        xPos = startPos.x + (tileOffset.x / 2) + (tileOffset.x * x) - ((tileOffset.x * width) / 2) + ((longestRuleRow * tilePrefab.transform.localScale.x) / 2);
        yPos = startPos.y - (tileOffset.y / 2) - (tileOffset.y * y) + ((tileOffset.y * height) / 2) - ((longestRuleCol * tilePrefab.transform.localScale.y) / 2);

        return new Vector2(xPos, yPos);
    }

    public GameObject[] getTiles()
    {
        return tiles;
    }

    public void DeleteBoard()
    {
        foreach(GameObject x in tiles)
        {
            Destroy(x);
        }

        foreach(List<GameObject> y in ruleTiles)
        {
            foreach (GameObject x in y)
            {
                Destroy(x);
            }
        }

        Destroy(gameObject);
    }

    private void ScaleTiles()
    {
        // Get height and width of screen
        float screenHeight = 2f * Camera.main.orthographicSize;
        float screenWidth = screenHeight * Camera.main.aspect;

        Vector3 scale = tilePrefab.transform.localScale;

        Vector2 longestRule = GetLongestRule(0, rules.Count);
        float xBoardSize, yBoardSize;

        if(longestRule.y < width)
        {
            // Longest Rule is along columns
            yBoardSize = height + longestRule.x;
            xBoardSize = width;
        } else
        {
            // Longest Rule is along rows
            yBoardSize = height;
            xBoardSize = width + longestRule.x;
        }

        if(xBoardSize > screenWidth)
        {
            scale.x = (screenWidth / xBoardSize) * scaleSize;
            scale.y = scale.x;
        } 

        if(yBoardSize > screenHeight)
        {
            scale.y = (screenHeight / yBoardSize) * scaleSize;
            scale.x = scale.y;
        }

        tilePrefab.transform.localScale = scale;
    }

    private Vector2 GetLongestRule(int start, int end)
    {
        int largest = 0;
        int largestIndex = 0;

        for(int i = start; i < end; i++)
        {
            if(rules[i].Count > largest)
            {
                largest = rules[i].Count;
                largestIndex = i;
            }
        }

        return new Vector2(largest, largestIndex);
    }

    public List<List<int>> GenerateRules(string sol)
    {
        List<List<int>> ruleSet = new List<List<int>>();
        // Row Rules
        for (int y = 0; y < height; y++)
        {
            // Create new rule for row
            List<int> rule = new List<int>();
            int count = 0; // Count how long the block is

            for(int x = 0; x < width; x++)
            {
                // check for contiguous blocks of 1
                if (sol[x + (y * width)] == '1')
                {
                    count++;
                    if (x == width - 1)
                    {
                        rule.Add(count);
                    }
                }
                else
                {
                    if (count > 0)
                    {
                        rule.Add(count);
                    }
                    count = 0;
                }
            }

            // Add the rule to the set of rules
            ruleSet.Add(rule);
        }

        // Column Rules
        for (int x = 0; x < width; x++)
        {
            // Create new rule for row
            List<int> rule = new List<int>();
            int count = 0; // Count how long the block is

            for (int y = 0; y < height; y++)
            {
                // check for contiguous blocks of 1
                if (sol[x + (y * width)] == '1')
                {
                    count++;
                    if (y == height - 1)
                    {
                        rule.Add(count);
                    }
                }
                else
                {
                    if (count > 0)
                    {
                        rule.Add(count);
                    }
                    count = 0;
                }
            }
            // Add the rule to the set of rules
            ruleSet.Add(rule);
        }

        return ruleSet;
    }

    private void GenerateRuleButtons()
    {
        int count = 0;

        for (int i = 0; i < height; i++)
        {
            RulesArrayToBoxes(count, i * width, true);
            count++;
        }

        for (int i = 0; i < width; i++)
        {
            RulesArrayToBoxes(count, i, false);
            count++;
        }
    }

    private void RulesArrayToBoxes(int ruleIndex, int tileIndex, bool rows)
    {
        List<int> rule = rules[ruleIndex];
        int count = 0;
        List<GameObject> rTiles = new List<GameObject>();
        foreach(int i in rule)
        {
            float xOffset = 0f;
            float yOffset = 0f;
            if (rows)
            {
                xOffset = (rule.Count - count) * tiles[tileIndex].transform.localScale.x;
            } else
            {
                yOffset = (rule.Count - count) * tiles[tileIndex].transform.localScale.y;
            }

            GameObject x = Instantiate(RuleTile, new Vector2(tiles[tileIndex].transform.position.x - xOffset, tiles[tileIndex].transform.position.y + yOffset), transform.rotation);
            x.transform.localScale = tiles[tileIndex].transform.localScale;
            x.GetComponentInChildren<TextMeshPro>().text = i.ToString();
            x.GetComponent<RuleTile>().setValue(i);
            rTiles.Add(x);
            count++;
        }
        ruleTiles.Add(rTiles);
    }

    public void ClearBoard()
    {
        foreach(GameObject x in tiles)
        {
            x.GetComponent<Tile>().clear();
        }
    }
    #endregion
}

// HASH the answers MD5 / SHA / Whatever