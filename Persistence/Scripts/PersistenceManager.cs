using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Zeef.Persistence {

	/// <summary>
	/// PersistenceManager handles interactions with save-data.
	/// </summary>
    public class PersistenceManager : SingleInstance<PersistenceManager> {

        [Required]
        [SerializeField] string fileName = "save_data.dat";
        public static string FileName { get { return GetInstance().fileName; } } 

		// ---

        public static void Save(object saveData) {
			using (FileStream file = File.Create(Application.persistentDataPath + "/" + FileName)) {
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(file, saveData);
			}
		}

		public static void Delete() {
			if (DataExists()) 
				File.Delete(Application.persistentDataPath + "/" + FileName);
		}
	
		public static T Load<T>() where T : class {			
			if (!DataExists()) throw new Exception($"There is no data with the filename {FileName} to load.");

			using (FileStream file = File.Open(Application.persistentDataPath + "/" + FileName, FileMode.Open)) {
				BinaryFormatter bf = new BinaryFormatter();
				T saveData = (T)bf.Deserialize(file);
				return saveData;
			}
		}

		public static T TryLoad<T>() where T : class {
			if (!DataExists()) return null;

			using (FileStream file = File.Open(Application.persistentDataPath + "/" + FileName, FileMode.Open)) {
				BinaryFormatter bf = new BinaryFormatter();
				T saveData = (T)bf.Deserialize(file);
				return saveData;
			}
		}

		public static bool DataExists() => 
			File.Exists(Application.persistentDataPath + "/" + FileName);
    }
}