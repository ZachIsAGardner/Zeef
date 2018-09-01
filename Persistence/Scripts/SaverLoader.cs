using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Zeef.GameManagement;

namespace Zeef.Persistence {

	public static class SaverLoader {

		public static void Save(object saveData, string fileName) {
			using (FileStream file = File.Create(Application.persistentDataPath + "/" + fileName)) {
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(file, saveData);
			}
		}

		public static void Delete(string fileName) {
			if (DataExists(fileName)) 
				File.Delete(Application.persistentDataPath + "/" + fileName);
		}
	
		public static T Load<T>(string fileName) where T : class {			
			if (!DataExists(fileName)) throw new Exception($"There is no data with the filename {fileName} to load.");

			using (FileStream file = File.Open(Application.persistentDataPath + "/" + fileName, FileMode.Open)) {
				BinaryFormatter bf = new BinaryFormatter();
				T saveData = (T)bf.Deserialize(file);
				return saveData;
			}
		}

		public static T TryLoad<T>(string fileName) where T : class {
			if (!DataExists(fileName)) return null;

			using (FileStream file = File.Open(Application.persistentDataPath + "/" + fileName, FileMode.Open)) {
				BinaryFormatter bf = new BinaryFormatter();
				T saveData = (T)bf.Deserialize(file);
				return saveData;
			}
		}

		public static bool DataExists(string fileName) => 
			File.Exists(Application.persistentDataPath + "/" + fileName);
	}
}