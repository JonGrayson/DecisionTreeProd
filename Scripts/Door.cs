using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public bool isClosed = false;
    public bool isLocked = false;

    public GameObject doorMesh;

    public Toggle locked;
    public Toggle closed;

    public void Update()
    {
        isDoorLocked();
        isDoorClosed();
    }

    public bool Open()
    {
        if(isClosed && !isLocked)
        {
            isClosed = false;
            return true;
        }

        return false;
    }

    public bool Close()
    {
        if(!isClosed)
        {
            isClosed = true;
        }

        return true;
    }

    public bool isDoorLocked()
    {
        if (locked.isOn == true)
        {
            isLocked = true;
            return true;
        }

        else
        {
            isLocked = false;
            return false;
        }
    }

    public bool isDoorClosed()
    {
        if (closed.isOn == true)
        {
            isClosed = true;
            Close();
            return true;
        }

        else
        {
            isClosed = false;
            Open();
            return false;
        }
    }
}
