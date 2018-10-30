using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.XR.WSA.WebCam;
public class GuideScript : MonoBehaviour, ISpeechHandler {
    public GameObject yes;
    public GameObject no;
    public GameObject surfaceCount, wallCount, allTringles, spatialUnderstanding;
    public Text guideText;
    public GameObject pickListPanel;
    public Font font;
    public static List<Item> items = new List<Item>();
    public List<NavMeshSurface> surfaces = new List<NavMeshSurface>();
    public Material material;

    public bool canWeScanBarcodes = false; //allows zxsing scanner to show user which barcode is being scanned
    //public bool areWeRecording = false;
    public string currentBarcodeValue = ""; //the last found barcode value

    public void OnSpeechKeywordRecognized(SpeechEventData eventData)
    {
        //if we are in first main menu, then yes will activate recording and we notify the user that recording has been chosen
        if (eventData.RecognizedText.Equals("yes") && guideText.text.Equals("Go and scan any missing areas, when you are ready say 'Yes'"))
        {
            GameObject.Find("MicrosoftLogo").SetActive(false);
            guideText.text = "Welcome to the Microsoft Hololens Warehouse application, \n" +
                            "say 'one' to find out how large an area can be scanned\n" +
                            "say 'two' to start putting products at a chosen location";
            /*MessageOnCanvas("you have chosen to record the session\n" +
                            "say 'one' to find out how large an area can be scanned\n" +
                            "say 'two' to start putting products at a chosen location", false);*/
            SpatialMappingManager.Instance.DrawVisualMeshes = false;
            SpatialMappingManager.Instance.StopObserver();
            //areWeRecording = true;
        }
        //if we are in first main menu, then no will ignore recording and we notify the user that recording has not been chosen
        /*if (eventData.RecognizedText.Equals("no") && guideText.text.Equals("Go and scan any missing areas, when you are ready say 'Yes' or 'No' to record"))
        {
            guideText.text = "you have not chosen to record the session\n" +
                "say 'one' to find out how large an area can be scanned\n" +
                "say 'two' to start putting products at a chosen location";
            MessageOnCanvas("you have not chosen to record the session\n" +
                "say 'one' to find out how large an area can be scanned\n" +
                "say 'two' to start putting products at a chosen location", false);
            SpatialMappingManager.Instance.DrawVisualMeshes = false;
            SpatialMappingManager.Instance.StopObserver();
        }*/
        //even if you are recording or not, saying one will activate the countTriangles script, so that the user can experience with spatial understanding
        if (eventData.RecognizedText.Equals("one") && (guideText.text.Equals("Welcome to the Microsoft Hololens Warehouse application, \n" +
                            "say 'one' to find out how large an area can be scanned\n" +
                            "say 'two' to start putting products at a chosen location") || guideText.text.Equals("you have chosen to record the session\n" +
                            "say 'one' to find out how large an area can be scanned\n" +
                            "say 'two' to start putting products at a chosen location")))
        {
            GameObject.Find("myControls").GetComponent<CountTriangles>().enabled = true;
            surfaceCount.SetActive(true); wallCount.SetActive(true); allTringles.SetActive(true);
            spatialUnderstanding.SetActive(true);
        }
        // start putting products at a chosen location
        if (eventData.RecognizedText.Equals("two") && (guideText.text.Equals("Welcome to the Microsoft Hololens Warehouse application, \n" +
                            "say 'one' to find out how large an area can be scanned\n" +
                            "say 'two' to start putting products at a chosen location") || guideText.text.Equals("you have chosen to record the session\n" +
                            "say 'one' to find out how large an area can be scanned\n" +
                            "say 'two' to start putting products at a chosen location")))
        {
            GameObject.Find("myControls").GetComponent<FinalBarcodeScanner>().enabled = true;
            guideText.text = "You can now scan barcodes, go and scan your first location number!";
            FinalBarcodeScanner.areWeScanning = true;
            FinalBarcodeScanner.firstTimeYes = true;
            pickListPanel.SetActive(true);
        }

    }
    void Update()
    {
    }
    //this method is for saying yes or no to record, maybe we will use it again later.
    /*public void MessageOnCanvas(string message, bool needConfirm)
    {
        if (needConfirm == true && !message.Equals(""))
        {
            yes.SetActive(true);
            no.SetActive(true);
            guideText.GetComponent<Text>().text = message;

        }
        else
        {
            yes.SetActive(false);
            no.SetActive(false);
            guideText.GetComponent<Text>().text = message;
        }

    }*/

    // Use this for initialization
    void Start () {
        //MessageOnCanvas("Go and scan any missing areas, when you are ready say 'Yes' or 'No' to record", true);
        guideText.text = "Go and scan any missing areas, when you are ready say 'Yes'";
        //if yes record, if no just navigate.
        //This script wil be a controller, and activate the different scripts. 



    }

    private int counter = 0;
    public void InsertPickUpItemToPanel(string itemName, string locationNumber) {
        if (GameObject.Find("item") == null)
        {
            GameObject gameObject = new GameObject("item");
            gameObject.tag = "item";
            gameObject.layer = 5; // 5 = ui.
            gameObject.AddComponent<Text>().text = itemName;
            gameObject.GetComponent<Text>().font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
            gameObject.GetComponent<Text>().color = Color.black;
            gameObject.GetComponent<Text>().fontSize = 20;
            gameObject.GetComponent<RectTransform>().transform.localScale += new Vector3(4, 0, 0);
            //gameObject.GetComponent<RectTransform>().transform.localScale = new Vector3(1f,1f,1f);
            gameObject.transform.SetParent(GameObject.Find("pickList").transform, false);
            //These 2 lines moves the text in the middle of the panel and all the way to the top.
            gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
            gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            //Place the text the right place. This will move the text a bit down and to the left.
            gameObject.GetComponent<RectTransform>().localPosition -= new Vector3(250, 90f, 0f);
            items.Add(new Item(counter, itemName, 1, gameObject.transform, locationNumber));
            string plusser = "";
            for (int i = 0; i < items.Count; i++)
            {
                plusser += items[i].Name + ", " + items[i].BelongsToLocation;
            }
            GameObject.Find("Item").GetComponent<Text>().text = plusser;
            gameObject.GetComponent<Text>().enabled = false;
            counter++;
        } else
        {
            int holder = 0;
            for (int i = 0; i < items.Count; i++) {
                if (holder < items[i].Id) {
                    holder = items[i].Id;  
                }
            }
            Debug.Log(items.Count);
            GameObject gameObject = new GameObject("item");
            gameObject.tag = "item";
            gameObject.layer = 5; // 5 = ui.
            gameObject.AddComponent<Text>().text = itemName;
            gameObject.GetComponent<Text>().font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
            gameObject.GetComponent<Text>().color = Color.black;
            gameObject.GetComponent<Text>().fontSize = 20;
            gameObject.GetComponent<RectTransform>().transform.localScale += new Vector3(4,0,0);
            gameObject.transform.SetParent(GameObject.Find("pickList").transform, false);
            gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
            gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            Transform lowestGameOBject = items[holder].PanelPosition;
            gameObject.GetComponent<RectTransform>().localPosition = lowestGameOBject.GetComponent<RectTransform>().localPosition;
            gameObject.GetComponent<RectTransform>().localPosition -= new Vector3(0,50,0);
            items.Add(new Item(counter,itemName, 1, gameObject.transform, locationNumber));
            string plusser = "";
            for (int i = 0; i < items.Count; i++)
            {
                //plusser += items[i].Name + ", " +  items[i].BelongsToLocation + "\n";
                plusser += items[i].nameAndBelongs;
            }
            GameObject.Find("Item").GetComponent<Text>().text = plusser;
            gameObject.GetComponent<Text>().enabled = false;
            counter++;
        }
       
    }
    public IEnumerator UpdateItemsPanel() {
        //Delete all Gameobjects with tag "item"
        GameObject[] itemsFromHierarchy;
        itemsFromHierarchy = GameObject.FindGameObjectsWithTag("item");
        foreach (GameObject item in itemsFromHierarchy)
        {
            Destroy(item);

        }
        yield return new WaitForSeconds(1.5f);

        if (GameObject.FindGameObjectsWithTag("item").Length == 0)
        {
           GameObject gameObject = new GameObject("item");
            gameObject.tag = "item";
            gameObject.layer = 5; // 5 = ui.
            gameObject.AddComponent<Text>().text = items[0].Name;
            gameObject.GetComponent<Text>().font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
            gameObject.GetComponent<Text>().color = Color.black;
            gameObject.GetComponent<Text>().fontSize = 20;
            gameObject.GetComponent<RectTransform>().transform.localScale += new Vector3(4, 0, 0);
            //gameObject.GetComponent<RectTransform>().transform.localScale = new Vector3(1f,1f,1f);
            gameObject.transform.SetParent(GameObject.Find("pickList").transform, false);
            //These 2 lines moves the text in the middle of the panel and all the way to the top.
            gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
            gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            //Place the text the right place. This will move the text a bit down and to the left.
            gameObject.GetComponent<RectTransform>().localPosition -= new Vector3(250, 90f, 0f);
            items[0].PanelPosition = gameObject.transform;
            string plusser = "";
            for (int i = 0; i < items.Count; i++)
            {
                plusser += items[i].Name + ", " + items[i].BelongsToLocation;
            }
            GameObject.Find("Item").GetComponent<Text>().text = plusser;
            counter++;
        }
        for (int i = 1; i < items.Count; i++)
        {
            GameObject gameObject = new GameObject("item");
            Debug.Log(items.Count);
            gameObject.tag = "item";
            gameObject.layer = 5; // 5 = ui.
            gameObject.AddComponent<Text>().text = items[i].Name;
            gameObject.GetComponent<Text>().font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
            gameObject.GetComponent<Text>().color = Color.black;
            gameObject.GetComponent<Text>().fontSize = 20;
            gameObject.GetComponent<RectTransform>().transform.localScale += new Vector3(4, 0, 0);
            gameObject.transform.SetParent(GameObject.Find("pickList").transform, false);
            gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
            gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            Transform lowestGameOBject = items[i-1].PanelPosition;
            gameObject.GetComponent<RectTransform>().localPosition = lowestGameOBject.GetComponent<RectTransform>().localPosition;
            gameObject.GetComponent<RectTransform>().localPosition -= new Vector3(0, 50, 0);
            items[i].PanelPosition = gameObject.transform;
        }
    }
        }
	
	// Update is called once per frame
