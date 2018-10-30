using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationNumber : MonoBehaviour {
    private GameObject locNumOnWall;
    private string description;

    public LocationNumber(GameObject locNumOnWall, string description)
    {
        this.locNumOnWall = locNumOnWall;
        this.description = description;
    }

    public GameObject LocNumOnWall
    {
        get
        {
            return locNumOnWall;
        }

        set
        {
            locNumOnWall = value;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public override string ToString()
    {
        return base.ToString() + ": " + description;
    }
}
