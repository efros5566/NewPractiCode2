using jsaaaaaa;
using System.Text.Json;
using System.Text.RegularExpressions;

var html = await Load("https://learn.malkabruk.co.il/");
var cleanHtml = new Regex("\\s").Replace(html, " ");
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);
var htmlLinesArrey = htmlLines.ToArray();
Console.WriteLine();

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
for (int i = 0; i < htmlLinesArrey.Length; i++)
{
    Console.WriteLine(htmlLinesArrey[i]);
}

void Serialize()
{
    //שורש
    HtmlElement root;
    //מצביע למקום הנוכחי בעץ
    HtmlElement currentElement = null;
    //מערך בגודל מערך האלמנטים בקובץ
    HtmlElement[] elements = new HtmlElement[htmlLinesArrey.Length];
    //משתנה לוקאלי
    int count = -1;
    //מעבר על מערך התגיות 
    for (int i = 0; i < htmlLinesArrey.Length; i++)
    {
        var tagName = htmlLinesArrey[i].Split(" ")[0];
        if (HtmlHelper.instance.HtmlTags.Contains(tagName))
        {
            currentElement = new HtmlElement();
            currentElement.Name = tagName;
            var elementAtrributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlLinesArrey[i]);
            foreach (Match match in elementAtrributes)
            {
                if (match.Success)
                {
                    string attributeName = match.Groups[1].Value;
                    if (attributeName == "id")
                        currentElement.Id = match.Groups[2].Value;
                    else if (attributeName == "class")
                    {
                        var classPattern = new Regex(@"class=""([^""]*)""").Matches(htmlLinesArrey[i]);
                        foreach (Match item in classPattern)
                        {
                            if (item.Success && item.Groups.Count >= 2)
                            {
                                string classesValue = item.Groups[1].Value;
                                currentElement.Classes = classesValue.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                Console.WriteLine("Classes:");
                                foreach (string className in currentElement.Classes)
                                    Console.WriteLine(className);
                            }
                        }
                    }
                    else
                    {
                        currentElement.Attributes.Add(match.Groups[0].Value);
                    }
                }

            }
            elements[++count] = currentElement;
        }
        else if (tagName == "/html")
            break;
        else if (tagName.StartsWith("/"))
        {
            currentElement.Parent = elements[count - 1];
            elements[count - 1].Children.Add(currentElement);
            currentElement = currentElement.Parent;
            count--;
        }
        else if (!tagName.StartsWith("/"))
        {
            currentElement.InnerHtml = htmlLinesArrey[i];
        }

        if (i == 0)
        {
            root = elements[0];
        }
    }
}
Console.WriteLine();

