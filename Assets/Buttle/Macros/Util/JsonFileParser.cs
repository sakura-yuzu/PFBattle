using System.Collections.Generic;
using UnityEngine;
using System.IO;

class JsonFileParser
{
  /**
  * @brief JSONファイルを読み込んで、指定した型に変換して返す
  * @param path
  * @return T
  */
  public static T LoadFromJSON<T>(string path)
  {
    string json = File.ReadAllText(path);
    return JsonUtility.FromJson<T>(json);
  }
}