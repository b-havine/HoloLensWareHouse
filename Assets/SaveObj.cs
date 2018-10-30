using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SaveObj : MonoBehaviour, ISpeechHandler
{
    private static int StartIndex = 0;
    // Use this for initialization
    public static void Start()
    {
        StartIndex = 0;
    }
    public static void End()
    {
        StartIndex = 0;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            DoExport(true);
        }
    }
    public void OnSpeechKeywordRecognized(SpeechEventData eventData)
    {
        if (eventData.RecognizedText.Equals("save"))
        {
            Debug.Log("trying to save...");
            DoExport(true);
        }
        if (eventData.RecognizedText.Equals("load"))
        {

        }
    }
    public static string MeshToString(MeshFilter mf, Transform t)
    {
        Vector3 s = t.localScale;
        Vector3 p = t.localPosition;
        Quaternion r = t.localRotation;


        int numVertices = 0;
        Mesh m = mf.sharedMesh;
        if (!m)
        {
            return "####Error####";
        }
        Material[] mats = mf.GetComponent<Renderer>().sharedMaterials;

        StringBuilder sb = new StringBuilder();

        foreach (Vector3 vv in m.vertices)
        {
            Vector3 v = t.TransformPoint(vv);
            numVertices++;
            sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, -v.z));
        }
        sb.Append("\n");
        foreach (Vector3 nn in m.normals)
        {
            Vector3 v = r * nn;
            sb.Append(string.Format("vn {0} {1} {2}\n", -v.x, -v.y, v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.uv)
        {
            sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
        }
        for (int material = 0; material < m.subMeshCount; material++)
        {
            sb.Append("\n");
            sb.Append("usemtl ").Append(mats[material].name).Append("\n");
            sb.Append("usemap ").Append(mats[material].name).Append("\n");

            int[] triangles = m.GetTriangles(material);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                    triangles[i] + 1 + StartIndex, triangles[i + 1] + 1 + StartIndex, triangles[i + 2] + 1 + StartIndex));
            }
        }

        StartIndex += numVertices;
        return sb.ToString();
    }
    public void DoExport(bool makeSubmeshes)
    {
        string meshName = "SpatialMapping";
        string fileName = "cube.obj";
        SaveObj.Start();

        StringBuilder meshString = new StringBuilder();

        meshString.Append("#" + meshName + ".obj"
                            //  + "\n#" + System.DateTime.Now.ToLongDateString()
                            //  + "\n#" + System.DateTime.Now.ToLongTimeString()
                            + "\n#-------"
                            + "\n\n");
        Transform t = GameObject.Find("SpatialMapping").transform;
        Vector3 originalPosition = t.position;
        t.position = Vector3.zero;

        if (!makeSubmeshes)
        {
            meshString.Append("g ").Append(t.name).Append("\n");
        }
        meshString.Append(processTransform(t, makeSubmeshes));

        WriteToFile(meshString.ToString(), fileName);

        t.position = originalPosition;

        SaveObj.End();
        Debug.Log("Exported Mesh: " + fileName);
    }
    static string processTransform(Transform t, bool makeSubmeshes)
    {
        StringBuilder meshString = new StringBuilder();

        meshString.Append("#" + t.name
                        + "\n#-------"
                        + "\n");

        if (makeSubmeshes)
        {
            meshString.Append("g ").Append(t.name).Append("\n");
        }

        MeshFilter mf = t.GetComponent<MeshFilter>();
        if (mf)
        {
            meshString.Append(SaveObj.MeshToString(mf, t));
        }

        for (int i = 0; i < t.childCount; i++)
        {
            meshString.Append(processTransform(t.GetChild(i), makeSubmeshes));
        }

        return meshString.ToString();
    }

    static void WriteToFile(string s, string filename)
    {
        /*     MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(filename));
             using (StreamWriter sw = new StreamWriter(stream))
             {
                 sw.Write(s);
             }*/

        /* using (StreamWriter writer =
            new StreamWriter("output.obj"))
       {
            writer.Write(s);
            //writer.WriteLine("word 2");
            //writer.WriteLine("Line");
        }*/
        /* string path = "test.txt";

         //Write some text to the test.txt file
         StreamWriter writer = new StreamWriter(path, true);
         writer.WriteLine("Test");
         writer.Close();*/



        /*using (StreamWriter sr = new StreamWriter(new FileStream(filename, FileMode.Append)))
        {
            sr.Write(s);
            sr.Flush();
            GameObject.Find("Text").GetComponent<Text>().text = "saved";
        }*/



        /*Object prefab = PrefabUtility.CreatePrefab("Assets/hej.prefab", GameObject.Find("SpatialUnderstanding"));
        //PrefabUtility.ReplacePrefab(GameObject.Find("SpatialUnderstanding"), prefab, ReplacePrefabOptions.ConnectToPrefab);

        AssetDatabase.CreateAsset(new Material(Shader.Find("Specular")),  "Assets/test.prefab");
        AssetDatabase.SaveAssets();*/
        string path = Path.Combine(/*Application.persistentDataPath*/ "" , filename);
        using (TextWriter writer = File.CreateText(path))
        {
            // TODO write text here
            writer.Write(s);
            writer.Flush();
            //GameObject.Find("Text").GetComponent<Text>().text = "saved";
        }
        // Do something with the new file.
    }

}
