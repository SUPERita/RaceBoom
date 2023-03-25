
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystem
{
    private static string salt = "Yhsd87";

    //string[]
    public static void SaveStringArrayAtLocation(string[] data, string location)
    {
        //Debug.Log(number + " the number SavePlayer in SaveSystem got");
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player." + location + salt;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static string[] LoadStringArrayFromLocation(string location)
    {

        string path = Application.persistentDataPath + "/player." + location + salt;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            string[] data = formatter.Deserialize(stream) as string[];
            stream.Close();

            return data /* new PlayerData(1) */;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }

    }

    //int[]
    public static void SaveIntArrayAtLocation(int[] data, string location)
    {
        //Debug.Log(number + " the number SavePlayer in SaveSystem got");
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player." + location + salt;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static int[] LoadIntArrayFromLocation(string location)
    {

        string path = Application.persistentDataPath + "/player." + location + salt;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            if (stream.Length == 0) { return null; }

            int[] data = formatter.Deserialize(stream) as int[];
            stream.Close();

            return data /* new PlayerData(1) */;
        }
        else
        {
            //Debug.LogError("Save file not found in " + path);
            return null;
        }

    }

    //int
    public static void SaveIntAtLocation(int data, string location)
    {
        //Debug.Log(number + " the number SavePlayer in SaveSystem got");
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player." + location + salt;
        FileStream stream = new FileStream(path, FileMode.Create);

        int[] data2 = new int[] { data };

        formatter.Serialize(stream, data2);
        stream.Close();
    }
    public static int LoadIntFromLocation(string location, int defaultValue = 0)
    {

        string path = Application.persistentDataPath + "/player." + location + salt;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            if (stream.Length == 0) { return 0; }

            int[] data = formatter.Deserialize(stream) as int[];
            stream.Close();

            return data[0] /* new PlayerData(1) */;
        }
        else
        {
            //Debug.LogError("Save file not found in " + path);
            return defaultValue;
        }

    }

    //float
    public static void SaveFloatAtLocation(float data, string location)
    {
        //Debug.Log(number + " the number SavePlayer in SaveSystem got");
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player." + location + salt;
        FileStream stream = new FileStream(path, FileMode.Create);
        float[] data2 = new float[] { data };

        formatter.Serialize(stream, data2);
        stream.Close();
    }
    public static float LoadFloatFromLocation(string location)
    {

        string path = Application.persistentDataPath + "/player." + location + salt;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            if (stream.Length == 0) { return 0; }

            float[] data = formatter.Deserialize(stream) as float[];
            stream.Close();

            return data[0] /* new PlayerData(1) */;
        }
        else
        {
            //Debug.LogError("Save file not found in " + path);
            return 0;
        }

    }

    //bool[]
    public static void SaveArrayBoolAtLocation(bool[] data, string location)
    {
        //Debug.Log(number + " the number SavePlayer in SaveSystem got");
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player." + location + salt;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static bool[] LoadBoolArrayFromLocation(string location)
    {

        string path = Application.persistentDataPath + "/player." + location + salt;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            bool[] data = (bool[])formatter.Deserialize(stream);
            stream.Close();

            return data /* new PlayerData(1) */;
        }
        else
        {
            //Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    //bool
    public static void SaveBoolAtLocation(bool data, string location)
    {
        //Debug.Log(number + " the number SavePlayer in SaveSystem got");
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player." + location + salt;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static bool LoadBoolFromLocation(string location, bool defaultValue = false)
    {

        string path = Application.persistentDataPath + "/player." + location + salt;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            bool data = (bool)formatter.Deserialize(stream);
            stream.Close();

            return data /* new PlayerData(1) */;
        }
        else
        {
            //Debug.LogError("Save file not found in " + path);
            return defaultValue;
        }
    }




}