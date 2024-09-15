using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveDataManager
{
    private static string keyword = "237416fzelihgfalihgfali";

    public static void SaveToJson(string dataname, object savable, bool encrypt = true, string folder = "SaveFiles/")
    {
        string directory = Path.Combine(Application.persistentDataPath, folder);
        string filename = Path.Combine(directory + dataname);

        string json = JsonConvert.SerializeObject(savable);

        if(encrypt)
        {
            json = EncryptDecrypt(json);
        } 

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            File.WriteAllText(filename, json);
        }
        else
        {
            File.WriteAllText(filename, json);
        }
    }

    public static T LoadFromJson<T>(string dataname, bool decrypt = true, string folder = "SaveFiles/")
    {
        string directory = Path.Combine(Application.persistentDataPath, folder);
        string filename = Path.Combine(directory + dataname);

        if(File.Exists(filename))
        {
            string json = File.ReadAllText(filename);
            if(decrypt)
            {
                json = EncryptDecrypt(json);
            }      
            object newObject = JsonConvert.DeserializeObject<T>(json);
            return (T)newObject;
        }

        object notFoundObj = null;
        return (T)notFoundObj;
    }

    private static string EncryptDecrypt(string json)
    {
        string result = "";

        for (int i = 0; i < json.Length; i++)
        {
            result += (char)(json[i] ^ keyword[i % keyword.Length]);
        }

        return result;
    }
}
