using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    bool active = false;
    bool blocked = false;

    public Color a;
    public Color b;
    public Color c;

    private Color defaultCol;

    public TileManager tm;

    public void SetDefaultColor(bool x)
    {
        if (x)
        {
            GetComponent<SpriteRenderer>().color = b;
            defaultCol = b;
        } else
        {
            GetComponent<SpriteRenderer>().color = c;
            defaultCol = c;
        }
    }

    //GetComponentInChildren<TextMeshPro>().text = "•";
    private void OnMouseOver()
    {

        if (Input.GetMouseButton(0))
        {
            if (!blocked)
            {
                if (!active && (tm.getAllClear() || tm.place))
                {
                    // Place
                    GetComponent<SpriteRenderer>().color = a; // Set Colour
                    active = true; // Set Active
                    tm.setPlace(); // Change Manager to PLACE mode

                }
                else if (active && (tm.getAllClear() || tm.clear))
                {
                    // Clear
                    GetComponent<SpriteRenderer>().color = defaultCol;
                    active = false;
                    tm.setClear();
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            if (!active)
            {
                if (!blocked && (tm.getAllClear() || tm.block))
                {
                    //block
                    GetComponentInChildren<TextMeshPro>().text = "•";
                    blocked = true;
                    tm.setBlocked();

                }
                else if (blocked && (tm.getAllClear() || tm.unblock))
                {
                    //unblock
                    GetComponentInChildren<TextMeshPro>().text = "";
                    blocked = false;
                    tm.setUnblocked();
                }
            }
        }

        //if (Input.GetMouseButton(0))
        //{
        //    if (!changed && !blocked) 
        //    {
        //        GetComponent<SpriteRenderer>().color = a;
        //        active = true;
        //    }
        //    changed = true;
        //}

        //if (Input.GetMouseButton(1))
        //{
        //    if (!changed)
        //    {
        //        if (active)
        //        {
        //            if (!GetComponentInParent<Board>().getBlocking())
        //            {
        //                GetComponentInParent<Board>().setClearing();
        //                GetComponent<SpriteRenderer>().color = defaultCol;
        //                active = false;
        //            }
        //        }
        //        else if (blocked)
        //        {
        //            if (!GetComponentInParent<Board>().getBlocking())
        //            {
        //                GetComponentInParent<Board>().setClearing();
        //                GetComponentInChildren<TextMeshPro>().text = "";
        //                blocked = false;
        //            }
        //        }
        //        else
        //        {
        //            if (!GetComponentInParent<Board>().getClearing())
        //            {
        //                GetComponentInParent<Board>().setBlocking();
        //                GetComponentInChildren<TextMeshPro>().text = "•";
        //                blocked = true;
        //            }
        //        }
        //        changed = true;
        //    }
        //}
    }

    public void clear()
    {
        active = false;
        blocked = false;
        GetComponentInChildren<TextMeshPro>().text = "";
        GetComponent<SpriteRenderer>().color = defaultCol;
    }

    public bool getActive()
    {
        return active;
    }
}
