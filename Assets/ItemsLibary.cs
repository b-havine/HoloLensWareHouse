using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsLibary : MonoBehaviour {

    //can be used for:
    //(1) convert barcode to item name, if barcode exists in the if statements
    //(2) if no items existed according to the barcode, then the barcode might be a location number.
    public static string getItem(string barcode)
    {
        string test = "";
        if (barcode.Equals("012378A93L2N"))
        {
            test = "Tommy Jeans";
        }
        if (barcode.Equals("245123B23A22"))
        {
            test = "Lloyd";
        }
        if (!test.Equals("")) {
            return test;
        }
        return "";
    }
}
