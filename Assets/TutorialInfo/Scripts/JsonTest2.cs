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
    public List<GameObject> prefabs = new List<GameObject>();
    public List<ObjectData> objectDataList = new List<ObjectData>();
    private int spawnCount = 5;

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
        Debug.Log($"Saving to: {SaveFilePath}");
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
        for(int i = 0; i < transform.Count; i++)
        {
            Instantiate(prefab, transform[i].position, Quaternion.identity).transform.localScale = transform[i].scale;
        }
    }

    public void RemoveButton()
    {
        //if (File.Exists(SaveFilePath))
        //{
        //    File.Delete(SaveFilePath);
        //    Debug.Log("JSON 파일 삭제 완료: " + SaveFilePath);
        //}
        var findGo = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < findGo.Length; i++)
        {
            Destroy(findGo[i]);
            prefabs.Remove(findGo[i]);
        }
        objectDataList.Clear();

    }

    public void RandomButton()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var pos = new Vector3(Random.Range(0, 10), Random.Range(10, 30), Random.Range(0, 10));
            var newPrefab = Instantiate(prefab, pos, Quaternion.identity);
            prefabs.Add(newPrefab);
        }
    }
}