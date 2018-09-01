using System;
using UnityEngine;

namespace Zeef.Persistence {

    public class PersistenceManager : MonoBehaviour {

        [Required]
        [SerializeField] string fileName = "save_data.dat";
        public static string FileName { get { return GetPersistenceManager().fileName; } } 

        [SerializeField] static PersistenceManager persistenceManager;
        static PersistenceManager GetPersistenceManager() {
            if (persistenceManager == null) 
                throw new Exception("No PersistenceManager was found. Yet one was requested.");

            return persistenceManager;
        }

        void Awake() {
            if (persistenceManager != null)
                throw new Exception("Only one PersistenceManager may exist at a time.");

            persistenceManager = this;
        }
    }
}