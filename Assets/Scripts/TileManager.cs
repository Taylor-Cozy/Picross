using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    public bool place, clear, block, unblock = false;

    public void setPlace()
    {
        clear = block = unblock = false;
        place = true;
    }    
    
    public void setClear()
    {
        place = block = unblock = false;
        clear = true;
    }    
    
    public void setBlocked()
    {
        clear = place = unblock = false;
        block = true;
    }

    public void setUnblocked()
    {
        clear = place = block = false;
        unblock = true;
    }

    public bool getAllClear()
    {
        if(!place && !clear && !block && !unblock)
        {
            return true;
        } else
        {
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            place = clear = block = unblock = false;
        }
    }
}
