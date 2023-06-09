﻿namespace GameLibraryV2.Models.Common
{
    public class SystemRequirements
    {
        public int Id { get; set; }

        public string Type { get; set; } = null!;

        public string? OC { get; set; }

        public string? Processor { get; set; }

        public string? RAM { get; set; }

        public string? VideoCard { get; set; }

        public string? DirectX { get; set; }

        public string? Ethernet { get; set; }

        public string? HardDriveSpace { get; set; }

        public string? Additional { get; set; }
    }
}
