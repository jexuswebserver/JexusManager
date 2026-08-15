using System;

namespace JexusManager.Features.HttpRedirect
{
    [Serializable]
    public sealed class HttpRedirectSnapshot
    {
        public bool Enabled { get; set; }
        public string Destination { get; set; }
        public bool ExactDestination { get; set; }
        public bool ChildOnly { get; set; }
        public long ResponseStatus { get; set; }
        public long[] SupportedStatuses { get; set; } = Array.Empty<long>();
    }
}
