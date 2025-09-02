using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;

[System.Serializable]
public class ObjectData
{
    public Vector3 position;
    public Vector3 scale;
    //public Quaternion rotation;
}    
public class JsonTest2 : MonoBehaviour
{
    public static readonly string SaveFileName = "cube.json";
    public static string SaveFilePath => Path.Combine(Application.persistentDataPath, SaveFileName);

    public GameObject prefab;
    public List<GameObject> prefabs;
    public List<ObjectData> objectDataList = new List<ObjectData>();


    public void Start()
    {

    }

    public void Save()
    {
        foreach (var obj in prefabs)
        {
            var objdata = new ObjectData();
            objdata.position = obj.transform.position;
            objdata.scale = obj.transform.localScale;
           // objdata.rotation = obj.transform.rotation;
            objectDataList.Add(objdata);
        }

        var jsonObjectData = JsonConvert.SerializeObject(objectDataList, Formatting.Indented, new Vector3Converter());
        File.WriteAllText(SaveFilePath, jsonObjectData);
    }

    public void Load()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.LogError($"File not found: {SaveFilePath}");
            return;
        }

        var json = File.ReadAllText(SaveFilePath);
        var transform = JsonConvert.DeserializeObject<List<ObjectData>>(json, new Vector3Converter());
        for(int i = 0; i < prefabs.Count; i++)
        {
            Instantiate(prefabs[i], transform[i].position, Quaternion.identity).transform.localScale = transform[i].scale;
        }
    }

    public void RemoveButton()
    {
        var cube = GameObject.FindGameObjectWithTag("Player");
        Destroy(cube);
    }

    public void RandomButton()
    {
        prefabs = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            var pos = new Vector3(Random.Range(0, 10), Random.Range(10, 30), Random.Range(0, 10));
            var newPrefab = Instantiate(prefab, pos, Quaternion.identity);
            prefabs.Add(newPrefab);
        }
    }
}