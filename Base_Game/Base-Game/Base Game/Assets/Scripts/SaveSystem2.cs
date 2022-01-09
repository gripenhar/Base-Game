using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem2
{
	public static void Save(GameManager GM, int slotNum)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = $@"{Application.persistentDataPath}/game{slotNum}.weef";
		FileStream stream = new FileStream(path, FileMode.Create);

		GameData data = new GameData(GM);

		formatter.Serialize(stream, data);
		stream.Close();
		Debug.Log("done saving2");
	}

	public static GameData Load(int slotNum)
	{
		string path = $@"{Application.persistentDataPath}/game{slotNum}.weef";
		if(File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			GameData data = formatter.Deserialize(stream) as GameData;
			stream.Close();
			//Debug.Log("done loading2");
			return data;
		}
		else
		{
			Debug.LogError("Save file not found in " + path);
			return null;
		}
	}
}
