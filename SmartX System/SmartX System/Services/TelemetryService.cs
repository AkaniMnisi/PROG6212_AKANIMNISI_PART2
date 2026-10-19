using SmartX.Shared.Models;

namespace SmartX_System.Services
{
    public class TelemetryService
    {
    
        private readonly List<TelemetryPacket<object>> _telemetryHistory = new();

       
        private readonly Dictionary<string, Dictionary<string, List<string>>> _facilityTree = new()
        {
            { "Facility A", new Dictionary<string, List<string>>
                {
                    { "Zone 1", new List<string> { "Sub-Zone B", "Sub-Zone C" } }
                }
            }
        };

        public void ProcessPacket(TelemetryPacket<object> packet)
        {
            
            if (ValidateLocationRecursive("Facility A", packet.Location))
            {
                _telemetryHistory.Add(packet);
            }
            else
            {
                throw new Exception("Invalid sensor location configuration. Recursive check failed.");
            }
        }

       
        private bool ValidateLocationRecursive(string currentNode, string targetLocation)
        {
            if (currentNode == targetLocation) return true;

            if (_facilityTree.ContainsKey(currentNode))
            {
                foreach (var child in _facilityTree[currentNode].Keys)
                {
                    if (ValidateLocationRecursive(child, targetLocation)) return true;
                }
            }
            return false;
        }

        public void AttachLogToSensor(string macAddress, string filePath)
        {
            var packet = _telemetryHistory.FirstOrDefault(p => p.MacAddress == macAddress);
            if (packet != null)
            {
                packet.AttachedLogs.Add(filePath);
            }
        }
    }
}