namespace TheLastPlanet.Client.MODALITA.MAINLOBBY
{
    public class BucketMarker
    {
        public MarkerEx Marker;
        public string Name;
        public Scaleform Scaleform;

        public BucketMarker(MarkerEx marker, string name, string scaleform)
        {
            Marker = marker;
            Name = name;
            Scaleform = new Scaleform(scaleform);
        }

        public void Draw()
        {
            Marker.Draw();

            if (!Scaleform.IsLoaded) return;
            Position p = Marker.Position - Cache.PlayerCache.MyPlayer.Ped.Position.ToPosition();
            float heading = API.GetHeadingFromVector_2d(p.X, p.Y);
            Scaleform.Render3D(Marker.Position.ToVector3, new(0, 0, -heading), Marker.Scale / 2);
        }
    }
}
