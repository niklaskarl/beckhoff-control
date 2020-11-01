using System;
using System.Collections.Immutable;

namespace ControlServer.Data
{
    public static class DataContext
    {
        public static readonly ImmutableArray<Light> Lights = ImmutableArray.Create<Light>(
            new Light()
            {
                Name = "Bad",
                TriggerGroup = 0x4021,
                TriggerOffset = 0x00,
                ReadGroup = 0x4021,
                ReadOffset = 0xA0
            },
            new Light()
            {
                Name = "Schlafzimmer",
                TriggerGroup = 0x4021,
                TriggerOffset = 0x01,
                ReadGroup = 0x4021,
                ReadOffset = 0xA1
            },
            new Light()
            {
                Name = "Wintergarten",
                TriggerGroup = 0x4021,
                TriggerOffset = 0x02,
                ReadGroup = 0x4021,
                ReadOffset = 0xA2
            },
            new Light()
            {
                Name = "Waschküche",
                TriggerGroup = 0x4021,
                TriggerOffset = 0x03,
                ReadGroup = 0x4021,
                ReadOffset = 0xA3
            },
            new Light()
            {
                Name = "Aussenbereich",
                TriggerGroup = 0x4021,
                TriggerOffset = 0x04,
                ReadGroup = 0x4021,
                ReadOffset = 0xA4
            },
            new Light()
            {
                Name = "Gäste WC",
                TriggerGroup = 0x4021,
                TriggerOffset = 0x08,
                ReadGroup = 0x4021,
                ReadOffset = 0xA8
            },
            new Light()
            {
                Name = "Wohnbereich",
                TriggerGroup = 0x4021,
                TriggerOffset = 0x09,
                ReadGroup = 0x4021,
                ReadOffset = 0xA9
            },
            new Light()
            {
                Name = "Essbereich",
                TriggerGroup = 0x4021,
                TriggerOffset = 0x0A,
                ReadGroup = 0x4021,
                ReadOffset = 0xAA
            },
            new Light()
            {
                Name = "Küche",
                TriggerGroup = 0x4021,
                TriggerOffset = 0x0B,
                ReadGroup = 0x4021,
                ReadOffset = 0xAB
            },
            new Light()
            {
                Name = "Flur",
                TriggerGroup = 0x4021,
                TriggerOffset = 0x0C,
                ReadGroup = 0x4021,
                ReadOffset = 0xAC
            },
            new Light()
            {
                Name = "Abstellraum",
                TriggerGroup = 0x4021,
                TriggerOffset = 0x0D,
                ReadGroup = 0x4021,
                ReadOffset = 0xAD
            });
    }
}
