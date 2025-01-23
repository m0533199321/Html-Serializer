using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace html
{
    public class BuildTree
    {
        public HtmlElement Root { get; set; }
        private HtmlElement Current { get; set; }

        private HtmlHelper helper = HtmlHelper.Instance;

        public BuildTree()
        {
            Root = new HtmlElement();
            Current = Root;
        }

        public string FirstWordInString(string text)
        {
            text = text.Trim();
            return text.Split(' ')[0];
        }

        public void Build(IEnumerable<string> htmlLines)
        {
            string firstWord, rest;

            int numChildrens;

            var lines = htmlLines.SkipWhile(s => !s.StartsWith("html"));

            foreach (var line in lines)
            {
                if (line.StartsWith('/'))
                {
                    if (FirstWordInString(line.Substring(1)) == "html")
                    {
                        Root = Root.Children[0];
                        return;
                    }
                    Current = Current.Parent;
                }
                else
                {
                    firstWord = FirstWordInString(line);
                    firstWord = firstWord.EndsWith('/') ? firstWord.Substring(0, firstWord.Length - 1) : firstWord;
                    if (helper.HtmlTags.Contains(firstWord))
                    {
                        Current.Children.Add(new HtmlElement());
                        numChildrens = Current.Children.Count - 1;
                        Current.Children[numChildrens].Parent = Current;
                        Current = Current.Children[numChildrens];

                        Current.Name = firstWord;

                        rest = line.Substring(firstWord.Length).Trim();
                        var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(rest);

                        foreach (Match match in attributes)
                        {
                            if (match.Groups[1].Value == "id")
                                Current.Id = match.Groups[2].Value;

                            else if (match.Groups[1].Value == "class")
                                Current.Classes = match.Groups[2].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

                            else
                                Current.Attributes.Add(match.Groups[1].Value, match.Groups[2].Value);
                        }

                        if (helper.HtmlVoidTags.Contains(firstWord) || rest.EndsWith('/'))
                            Current = Current.Parent;
                    }
                    else
                    {
                        Current.InnerHtml = line;
                    }
                }
            }
        }
    }
}
