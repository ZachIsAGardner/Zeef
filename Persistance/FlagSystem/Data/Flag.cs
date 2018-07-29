using UnityEngine;

namespace Zeef.Persistance {

    [System.Serializable]
    public class Flag {
        
        [SerializeField] private int id;
        public int ID { get { return id; } }

        [SerializeField] private string name;
        public string Name { get { return name; } }

        [SerializeField] private bool value;
        public bool Value { get { return value; } set { this.value = value; }}

        public Flag(string name, int id, bool value) {
            this.name = name;
            this.id = id;
            this.value = value;
        }
    }
}