using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine;

public class MyObjectConverter 
{
    public static string ObjectToJson(object obj)
    {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        MemoryStream stream = new MemoryStream();
        serializer.WriteObject(stream, obj);
        byte[] dataBytes = new byte[stream.Length];
        stream.Position = 0;
        stream.Read(dataBytes, 0, (int) stream.Length);
        return Encoding.UTF8.GetString(dataBytes);
    }
    public static object JsonToObject(string jsonString, object obj)
    {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        return serializer.ReadObject(mStream);
    }
}
