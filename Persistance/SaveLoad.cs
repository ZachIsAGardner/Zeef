using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Zeef.GameManagement;

namespace Zeef.Persistance {

	[Serializable]
    public class SaveData {
        
        public string Scene { get; private set; }
        public List<FlagsContainer> FlagsContainers { get; private set; }

        public SaveData(string scene, List<FlagsContainer> flagsContainers) {
            Scene = scene;
            FlagsContainers = flagsContainers;
        }
    }

	public static class SaveLoad {

		public static void Save(SaveData saveData, string fileName) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create(Application.persistentDataPath + "/" + fileName);

			bf.Serialize(file, saveData);
			file.Close();
		}
	
		public static T Load<T>(string fileName) where T : SaveData {			
			if (!DataExists(fileName)) throw new Exception($"There is no data with the filename {fileName} to load.");

			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/" + fileName, FileMode.Open);

			T saveData = (T)bf.Deserialize(file);
			file.Close();

			return saveData;
		}

		public static T TryLoad<T>(string fileName) where T : SaveData {
			if (!DataExists(fileName)) return null;

			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/" + fileName, FileMode.Open);

			T saveData = (T)bf.Deserialize(file);
			file.Close();

			return saveData;
		}

		public static bool DataExists(string fileName) => 
			File.Exists(Application.persistentDataPath + "/" + fileName);
	}
}