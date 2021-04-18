﻿using System.Collections.Generic;
using DevBot9.Protocols.Homie;
using TestApp;

namespace BlazorHomieDashboard {
    public class HomieDevice {
        public List<HomieNode> Nodes { get; } = new();

        public string Name { get; set; }

        private ClientDevice _clientDevice;

        public void Initialize(HomieTopicTreeParser.Device deviceMetadata, Device.PublishToTopicDelegate publish, Device.SubscribeToTopicDelegate subscribe) {
            Name = (string)deviceMetadata.Attributes["$name"];

            _clientDevice = DeviceFactory.CreateClientDevice(deviceMetadata.Id);

            foreach (var nodeMetaData in deviceMetadata.Nodes) {
                var node = new HomieNode();
                node.Name = (string)nodeMetaData.Attributes["$name"];
                Nodes.Add(node);

                foreach (var propertyMetadata in nodeMetaData.Properties) {
                    switch (propertyMetadata.DataType) {
                        case DataType.Integer: {
                            var newProperty = _clientDevice.CreateClientIntegerProperty(propertyMetadata);
                            node.Properties.Add(newProperty);
                            break;
                        }

                        case DataType.Float: {
                            var newProperty = _clientDevice.CreateClientFloatProperty(propertyMetadata);
                            node.Properties.Add(newProperty);
                            HistoryService.Instance.StartHistoryMonitoring(newProperty);
                            break;
                        }

                        case DataType.Boolean: {
                            var newProperty = _clientDevice.CreateClientBooleanProperty(propertyMetadata);
                            node.Properties.Add(newProperty);
                            break;
                        }

#warning Shouldn't these have their own class?
                        case DataType.Enum:
                        case DataType.Color:
                        case DataType.DateTime:
                        case DataType.String: {
                            var newProperty = _clientDevice.CreateClientStringProperty(propertyMetadata);
                            node.Properties.Add(newProperty);
                            break;
                        }
                    }
                }
            }

            _clientDevice.Initialize(publish, subscribe);
        }

        public void HandlePublishReceived(string topic, string payload) {
            _clientDevice.HandlePublishReceived(topic, payload);
        }
    }
}