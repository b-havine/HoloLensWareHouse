using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    private int id;
    private Transform panelPosition;
    private new string name;
    private int quantity;
    private string belongsToLocation;

    public Item(int id, string name, int quantity, Transform panelPosition, string belongsToLocation)
    {
        this.id = id;
        this.name = name;
        this.quantity = quantity;
        this.PanelPosition = panelPosition;
        this.BelongsToLocation = belongsToLocation;
    }


    public int Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public string nameAndBelongs
    {
        get
        {
            return name + ", " + belongsToLocation + "\n" ;
        }
        
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public int Quantity
    {
        get
        {
            return quantity;
        }

        set
        {
            quantity = value;
        }
    }

    public Transform PanelPosition
    {
        get
        {
            return panelPosition;
        }

        set
        {
            panelPosition = value;
        }
    }

    public string BelongsToLocation
    {
        get
        {
            return belongsToLocation;
        }

        set
        {
            belongsToLocation = value;
        }
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
