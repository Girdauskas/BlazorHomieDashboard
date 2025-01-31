﻿using System.Collections.Generic;

namespace Bhd.Shared.DTOs {
    public class Node {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Properties { get; set; } = new();
    }
}
