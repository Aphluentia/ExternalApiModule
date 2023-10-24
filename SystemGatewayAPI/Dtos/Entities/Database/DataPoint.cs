using Newtonsoft.Json;
using System;
using System.Text.Json;
using System.Xml.Linq;

namespace SystemGatewayAPI.Dtos.Entities.Database
{
    public class DataPoint
    {
        public string SectionName { get; set; }
        public bool isDataEditable { get; set; }

        private object _Content { get; set; }

        public object Content
        {
            get { return _Content; }
            set { _Content = (value == null ? "" : JsonDocument.Parse(value.ToString())); }
        }

    }
}
