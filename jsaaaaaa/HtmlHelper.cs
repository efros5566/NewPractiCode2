using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace jsaaaaaa
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _instance=new HtmlHelper();
        public static HtmlHelper instance => _instance;
        public string[] HtmlTags { get; private set; }
        public string[] HtmlVoidTags { get; private set; }

        private HtmlHelper()
        {
            var allTagsJson = File.ReadAllText("HtmlTags.json");
            var allVoidTagsJson = File.ReadAllText("HtmlVoidTags.json");

            // המרת הנתונים מ-JSON למערכים באמצעות JsonSerilizer
            HtmlTags = JsonSerializer.Deserialize<string[]>(allTagsJson);
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(allVoidTagsJson);
        }

    }
}

