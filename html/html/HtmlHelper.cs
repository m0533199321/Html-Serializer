using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using System.ComponentModel;

namespace html
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper instance = new HtmlHelper();
        public static HtmlHelper Instance => instance;
        public string[] HtmlTags { get; set; }
        public string[] HtmlVoidTags { get; set; }

        private HtmlHelper()
        {
            var tags = File.ReadAllText("JSON/HtmlTags.json");
            var voidTags = File.ReadAllText("JSON/HtmlVoidTags.json");

            HtmlTags = JsonSerializer.Deserialize<string[]>(tags);
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(voidTags);
        }
    }
}
