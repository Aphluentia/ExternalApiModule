﻿using ExternalAPI.Helpers;
using ExternalAPI.Models.Enums;

namespace ExternalAPI.Models.Entities
{
    public class Module
    {
        public ModuleType ModuleType { get; set; }
        public string Data { get; set; }
        public Guid Id { get; set; }
        public DateTime DateTime => DateTime.UtcNow;
        public string checksum => ChecksumHelper.ComputeMD5(Data);
    }
}
