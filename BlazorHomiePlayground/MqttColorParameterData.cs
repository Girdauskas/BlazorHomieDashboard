﻿using System;

namespace BlazorHomiePlayground {
    public class MqttColorParameterData {
        public string Caption { get; set; }
        public string ActualValue { get; set; }
        public string TargetValue { get; set; }
        public Action SetTargetValue { get; set; }
    }
}