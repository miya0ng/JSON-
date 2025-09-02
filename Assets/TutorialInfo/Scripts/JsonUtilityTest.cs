using UnityEngine;
using Newtonsoft.Json;
using System;
using System.IO;

[Serializable]
public class PlayerState
{
    public string playerName;
    public int lives;
    public float health;
    public int[] array;

    public override string ToString()
    {
        return $"{playerName}/ {lives} / {health} / {array[0]}";
    }
    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}

public class SomeClasse
{
    public int person;
}
public class JsonUtiltiyTest : MonoBehaviour
{
}
