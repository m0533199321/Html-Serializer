using html;
using System.Text.RegularExpressions;
using System.Threading.Channels;

static async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

var html = await Load("https://hebrewbooks.org/beis");
//var cleanHtml = new Regex("\\s").Replace(html, "");
//var cleanHtml = new Regex("[\\t\\n\\r\\v\\f]").Replace(html, "");
//cleanHtml = Regex.Replace(cleanHtml, @"[ ]{2,}", " ");
//var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);

var cleanHtml = new Regex("\\s").Replace(html, " ");
var tagMatches = Regex.Matches(cleanHtml, @"<\/?([a-zA-Z][a-zA-Z0-9]*)\b[^>]*>|([^<]+)").Where(l => !String.IsNullOrWhiteSpace(l.Value));

var htmlLines = new List<string>();
foreach (Match item in tagMatches)
{
    string tag = item.Value.Trim();
    if (tag.StartsWith('<'))
        tag = tag.Trim('<', '>');
    htmlLines.Add(tag);
}

BuildTree buildTree = new BuildTree();
buildTree.Build(htmlLines);

Selector selector = Selector.PerStringToSelectors("div a.inactBG");
Selector tmp = selector;
while (tmp != null)
{
    Console.WriteLine(tmp);
    tmp = tmp.Child;
}

HashSet<HtmlElement> elements = buildTree.Root.FindElementsBySelector(selector);
foreach (HtmlElement element in elements)
{
    Console.WriteLine("--------------------------------");
    Console.WriteLine(element);
}


