namespace SmartX_System.Models
{
    public class SensorData
    {
        public string SensorId { get; set; } = string.Empty;
        public double Value { get; set; }

        
        public static SensorData operator +(SensorData a, SensorData b)
        {
            return new SensorData
            {
                SensorId = $"{a.SensorId}+{b.SensorId}",
                Value = a.Value + b.Value
            };
        }

       
        public static bool operator >(SensorData a, SensorData b) => a.Value > b.Value;
        public static bool operator <(SensorData a, SensorData b) => a.Value < b.Value;
    }
}