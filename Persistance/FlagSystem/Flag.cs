namespace Zeef.Persistance {
    [System.Serializable]
    public class Flag {
        // public fields so they can be viewed in inspector
        public string name;
        public bool value;
        public int id;

        public Flag(string name, int id, bool value) {
            this.name = name;
            this.id = id;
            this.value = value;
        }
    }
}