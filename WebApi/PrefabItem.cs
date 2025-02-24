namespace WebApi
{
    public class PrefabItem
    {
        public string Id { get; set; }
        public int PrefabId { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float Rotation { get; set; }
        public int FloatingLayer { get; set; }
        public string EnvironmentID { get; set; }
    }
}
