namespace NTTCinemas.Models.ViewModels
{
    internal class SeatPoint
    {
        public SeatPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}