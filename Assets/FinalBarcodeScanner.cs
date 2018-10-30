using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FinalBarcodeScanner : MonoBehaviour, ISpeechHandler {
    public List<NavMeshSurface> surfaces = new List<NavMeshSurface>();
    public Text guideText;
    public Text errors;
    public GameObject pickListPanel;
    public GameObject spatialMapping;
    public Material combinedMeshMaterial;
    public Camera cam;
    public Text debugText;
    public GameObject microsoftLogo;
    public Text itemText;
    public GameObject correctImage;
    public GameObject wrongImage;

    public static bool allowToScanItems = false;
    private bool areWePicking = false;
    public static bool areWeScanning = false;
    private GuideScript guideScript;
    private GameObject cube = null;
    private string copyBarcodeValue = "";
    private string confirmBarcode = "";
    public static List<LocationNumber> locationNumbers = new List<LocationNumber>();

    public static bool firstTimeYes = false;

    public void OnSpeechKeywordRecognized(SpeechEventData eventData)
    {
        if (eventData.RecognizedText.Equals("scan") && areWePicking == false)
        {
            areWeScanning = false;
            if (allowToScanItems == false) {
                if (copyBarcodeValue.Equals("A17"))
                {
                    copyBarcodeValue = "Baydoor";
                    errors.text = "";
                    guideText.text = "baydoor scanned, say 'end' if you wish to proceed to picking, say 'back' to cancel";
                    return;
                }
                confirmBarcode = copyBarcodeValue;
                guideText.text = "You have scanned: " + copyBarcodeValue +", say 'yes' to confirm the location";
                StartCoroutine(ShowCorrectLogo(true));
            }
            if (allowToScanItems == true) {
                if (ItemsLibary.getItem(copyBarcodeValue).Equals("")) {
                    StartCoroutine(ShowCorrectLogo(false));
                    errors.text = "you did not scan an item, try again, if you're done with items say: 'done'";
                    areWeScanning = true;
                    return;
                }
                StartCoroutine(ShowCorrectLogo(true));
                copyBarcodeValue = ItemsLibary.getItem(copyBarcodeValue);
                guideText.text = copyBarcodeValue + " added to: " + confirmBarcode;
                errors.text = "Item scanned: " + copyBarcodeValue + ", say 'scan' to add more items to location, say 'done' to confirm the items to location";
                guideScript.InsertPickUpItemToPanel(copyBarcodeValue, confirmBarcode);
                areWeScanning = true;
            }
            string isConfirmedLocationOrNot = ItemsLibary.getItem(confirmBarcode);
            if (cube == null && isConfirmedLocationOrNot.Equals("") && !copyBarcodeValue.Equals(""))
            {
                //if cube is null then we instantiate a cube that is used as postition for text meshes, also follow camera.
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.AddComponent<TapToPlace>();
                cube.GetComponent<TapToPlace>().IsBeingPlaced = true;
                cube.GetComponent<TapToPlace>().DefaultGazeDistance = 10;
                cube.GetComponent<TapToPlace>().AllowMeshVisualizationControl = false;
                cube.GetComponent<Transform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
                cube.transform.position = cam.transform.position + cam.transform.forward * 2;
                cube.GetComponent<MeshRenderer>().enabled = false;
            }
            else if (cube != null && isConfirmedLocationOrNot.Equals("") && !copyBarcodeValue.Equals(""))
            {//if cube exists, then just make it follow the camera.
                cube.GetComponent<TapToPlace>().IsBeingPlaced = true;
            }
        }
        if (eventData.RecognizedText.Equals("scan") && areWePicking == true)
        {
            areWeScanning = false;
            if (copyBarcodeValue.Equals("A17") && SpawnRobots.startPosition.GetComponent<TextMesh>().text.Equals("Baydoor") && allowToScanItems == false)
            {
                copyBarcodeValue = "Baydoor";
                errors.text = "";
                debugText.text = "";
                itemText.text = "";
                microsoftLogo.SetActive(true);
                pickListPanel.SetActive(false);
                guideText.text = "there are no tasks left and you have scanned baydoor, job done! go home!";
                GameObject.Find("myControls").GetComponent<SpawnRobots>().enabled = false;
                return;
            }
            if (ItemsLibary.getItem(copyBarcodeValue).Equals("") && locationNumbers[SpawnRobots.counter].Description.Equals(copyBarcodeValue) && allowToScanItems == false)
            {
                StartCoroutine(ShowCorrectLogo(true));
                errors.text = "You have scanned the correct location number, now scan the items";
                confirmBarcode = copyBarcodeValue;
                allowToScanItems = true;
                areWeScanning = true;
                return;
            }
            else
            {
                if (copyBarcodeValue.Equals("A17")) {
                    StartCoroutine(ShowCorrectLogo(false));
                    errors.text = "You have scanned baydoor but your job is not done! Scan the correct location number.";
                } else
                {
                    StartCoroutine(ShowCorrectLogo(false));
                    errors.text = "Wrong location number scanned. Try again.";
                }
               

            }
            if (allowToScanItems == true)
            {
                if (ItemsLibary.getItem(copyBarcodeValue).Equals("")) {
                    StartCoroutine(ShowCorrectLogo(false));
                    errors.text = "you did not scan an item, try again, if you're done picking up items here say 'done'";
                    areWeScanning = true;
                    return;
                }
                copyBarcodeValue = ItemsLibary.getItem(copyBarcodeValue);
                //errors.text = "something went wrong? item scanned was: " + copyBarcodeValue + ", confirmed barcode was: " + confirmBarcode;
                for (int i = 0; i < GuideScript.items.Count; i++)
                {

                    // first checks, if the scanned barcode exist in the list, and if is belongs to the current locationnumber
                    if (GuideScript.items[i].Name.Equals(copyBarcodeValue) && GuideScript.items[i].BelongsToLocation.Equals(confirmBarcode))
                    {
                        errors.text = "You have picked up an item : " + copyBarcodeValue + ", say 'scan' to pickup more items from location, say 'done' for next location";
                        GuideScript.items.Remove(GuideScript.items[i]);
                        StartCoroutine(guideScript.UpdateItemsPanel()); //update the panel, so that it shows that we have picked up an item, now pickup shows the remanining items
                        //StartCoroutine(ShowCorrectLogo(true));
                    }
                }
            }
            areWeScanning = true;
        }
        if (eventData.RecognizedText.Equals("yes") && areWePicking == false && firstTimeYes == true && allowToScanItems == false)
        {
            string isConfirmedLocationOrNot = ItemsLibary.getItem(confirmBarcode);
            if (isConfirmedLocationOrNot.Equals(""))
            {
                Create3DTextOnWall(confirmBarcode);
                allowToScanItems = true;
            }
            else
            {
                StartCoroutine(ShowCorrectLogo(false));
                errors.text = "you did not scan a location number";
                areWeScanning = true;
            }
        }
        if(eventData.RecognizedText.Equals("back") && guideText.text.Equals(
        "baydoor scanned, say 'end' if you wish to proceed to picking, say 'back' to cancel"))
        {
            areWeScanning = true;
            errors.text = "baydoor canceled, scan missing location numbers!";
        }
        if (eventData.RecognizedText.Equals("end") && guideText.text.Equals(
            "baydoor scanned, say 'end' if you wish to proceed to picking, say 'back' to cancel")
            && areWePicking == false)
        {
            Create3DTextOnWall(copyBarcodeValue);
            areWePicking = true;
            allowToScanItems = false;
            areWeScanning = false;
            //combine child meshes in spatialmapping
            MeshFilter[] meshFilters = GameObject.Find("SpatialMapping").GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            int i = 1;
            while (i < meshFilters.Length)
            {

                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);
                i++;
            }
            spatialMapping.transform.GetComponent<MeshFilter>().mesh = new Mesh();
            spatialMapping.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            spatialMapping.transform.gameObject.SetActive(true);
            spatialMapping.GetComponent<Renderer>().material = combinedMeshMaterial;

            //spatialMapping.GetComponent<MeshRenderer>().enabled = false;

            StartCoroutine(waitBake());
        }
        if (eventData.RecognizedText.Equals("go") && guideText.text.Equals(
            "area has been optimized, now you can pickup the goods from the picklist by saying: 'go'") 
            && areWePicking == true)
        {
            //Print the name of the LocationNumber
            guideText.text = "follow the line and scan the location number: " + locationNumbers[0].Description;
            errors.text = "we are in picking mode.";
            GameObject.Find("myControls").GetComponent<SpawnRobots>().enabled = true;
            areWeScanning = true;
            pickListPanel.GetComponent<Image>().enabled = true;
            foreach (Transform child in pickListPanel.transform)
            {
                child.gameObject.GetComponent<Text>().enabled = false;
            }
        }
        if (eventData.RecognizedText.Equals("done") && areWePicking == false && allowToScanItems == true)
        {
            guideText.text = "find a new location-number to scan";
            errors.text = "you have succesfully added items to a location";
            allowToScanItems = false;
            areWeScanning = true;
        }
        if (eventData.RecognizedText.Equals("done") && areWePicking == true && allowToScanItems == true)
        {
            bool anyItemsLeft = true;
            for (int i = 0; i < GuideScript.items.Count; i++)
            {

                if (GuideScript.items[i].BelongsToLocation.Equals(confirmBarcode))
                {
                    anyItemsLeft = true;
                    errors.text = "You are not done with Scanning items! - Go further";
                    break; //break because you are not done with scanning items.
                }
                else
                {
                    anyItemsLeft = false;
                }

            }
            if (GuideScript.items.Count == 0)
            {
                anyItemsLeft = false;
            }
            if (anyItemsLeft == false)
            {
                // Change direction of the Robot with the counter. 
                SpawnRobots.counter++;
                allowToScanItems = false;
                errors.text = "items have been picked up, now go to: " + locationNumbers[SpawnRobots.counter].Description;
            }
            areWeScanning = true;
        }
    }

    // Use this for initialization
    void Start () {
        guideScript = new GuideScript();
#if !UNITY_EDITOR
    MediaFrameQrProcessing.Wrappers.ZXingQrCodeScanner.ScanFirstCameraForQrCode(
        result =>
        {
          UnityEngine.WSA.Application.InvokeOnAppThread(() =>
          {
              if (areWeScanning == true) {
                  copyBarcodeValue = result;
                  guideText.text = "got result: " + copyBarcodeValue + ", say scan to confirm";
              }
            
          }, 
          false);
        },
        null);
#endif
    }

    private string ItemOrLocationNumber(string barcode) {
        if (ItemsLibary.getItem(barcode).Equals("")) {
            return barcode;
        }
        if (!ItemsLibary.getItem(barcode).Equals("")) {
            return ItemsLibary.getItem(barcode);
        }
        return "";
    }

    // Update is called once per frame
    void Update () {
		
	}
    public void Create3DTextOnWall(string barcodeResult)
    {
        if (!copyBarcodeValue.Equals(""))
        {
            cube.GetComponent<TapToPlace>().IsBeingPlaced = false;
            GameObject textMesh = new GameObject("TextOnWall");
            textMesh.AddComponent<TextMesh>();
            textMesh.AddComponent<TapToPlace>();
            textMesh.GetComponent<TextMesh>().text = barcodeResult;
            textMesh.transform.position = GameObject.Find("DefaultCursor").transform.position;
            textMesh.transform.rotation = cam.transform.rotation;
            textMesh.GetComponent<TextMesh>().anchor = UnityEngine.TextAnchor.MiddleCenter;
            textMesh.GetComponent<TextMesh>().fontSize = 1024;
            textMesh.GetComponent<TextMesh>().characterSize = 0.001f;
            textMesh.AddComponent<BoxCollider>();

            locationNumbers.Add(new LocationNumber(textMesh, barcodeResult));
            string combine = "";
            for (int i = 0; i < locationNumbers.Count; i++)
            {
                combine += "Locations scanned: " + locationNumbers[i].Description + ", ";
            }
            debugText.text = combine;
            if (!copyBarcodeValue.Equals("Baydoor"))
            {
                guideText.text = "placement complete, now scan the items";
                allowToScanItems = true;
            }
            /*if (guideScript.areWeRecording)
            {
                guideScript.canWeScanBarcodes = true;
                return;
            }*/
            areWeScanning = true; //allow barcode-scanning again.
        }
        else
        {
            guideText.text = "something was wrong with the barcode(value is empty) Try again to scan";
            /*if (guideScript.areWeRecording)
            {
                guideScript.canWeScanBarcodes = true;
                return;
            }*/
            areWeScanning = true;
        }
    }
    public IEnumerator waitBake()
    {
        guideText.text = "optimizing... hold the camera still...";
        
        yield return new WaitForSeconds(5f);
            //bake the combined spatialmapping mesh
            surfaces[0] = GameObject.Find("SpatialMapping").GetComponent<NavMeshSurface>();
            for (int j = 0; j < surfaces.Count; j++)
            {
                surfaces[j].BuildNavMesh();
            }
        guideText.text = "area has been optimized, now you can pickup the goods from the picklist by saying: 'go'";
    }
    private IEnumerator ShowCorrectLogo(bool whichCheckmark) {
        if (whichCheckmark == true) {
            correctImage.SetActive(true);
            yield return new WaitForSeconds(2f);
            correctImage.SetActive(false);
        }
        if (whichCheckmark == false) {
            wrongImage.SetActive(true);
            yield return new WaitForSeconds(2f);
            wrongImage.SetActive(false);
        }
        
    }
}
