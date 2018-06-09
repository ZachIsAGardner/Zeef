using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Zeef.Persistance {
	public static class SaveLoad {
		public static void Save(SaveData saveData, string fileName) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create(Application.persistentDataPath + "/" + fileName);

			bf.Serialize(file, saveData);
			file.Close();
		}

		public static bool DataExists(string fileName) {
			return File.Exists(Application.persistentDataPath + "/" + fileName);
		}

		public static T Load<T>(string fileName) where T : SaveData {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/" + fileName, FileMode.Open);

			T saveData = (T)bf.Deserialize(file);
			file.Close();

			return saveData;
		}
	}
}