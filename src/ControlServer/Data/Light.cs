using System;

namespace ControlServer.Data
{
    public sealed class Light
    {
        public string Name { get; set; }

        public string Icon { get; set; }
        
        public int TriggerGroup { get; set; }
        
        public int TriggerOffset { get; set; }
        
        public int ReadGroup { get; set; }
        
        public int ReadOffset { get; set; }
    }
}
