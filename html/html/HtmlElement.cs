using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace html
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement()
        {
            Classes = new List<string>();
            Children = new List<HtmlElement>();
            Attributes = new Dictionary<string, string>();
        }
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);
            HtmlElement htmlElement;
            while (queue.Count > 0)
            {
                yield return htmlElement = queue.Dequeue();
                foreach (HtmlElement child in htmlElement.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement element = this;
            while (element != null)
            {
                yield return element;
                element = element.Parent;
            }
        }

        public bool EqualHtmlElementWithSelector(HtmlElement htmlElement, Selector selector)
        {
            foreach (string class1 in selector.Classes)
            {
                if (!htmlElement.Classes.Contains(class1))
                    return false;
            }
            return (selector.TagName == null || htmlElement.Name == selector.TagName) &&
                (selector.Id == null || htmlElement.Id == selector.Id);
        }

        public void FindElementsBySelectorRec(HtmlElement htmlElement, Selector selector, HashSet<HtmlElement> setElements)
        {
            var listElements = htmlElement.Descendants()
                                          .Where(element => EqualHtmlElementWithSelector(element, selector))
                                          .ToList();

            if (selector.Child == null)
            {
                setElements.UnionWith(listElements);
            }
            else
            {
                foreach (var lstelement in listElements)
                {
                    FindElementsBySelectorRec(lstelement, selector.Child, setElements);
                }
            }
        }


        public HashSet<HtmlElement> FindElementsBySelector(Selector selector)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();
            FindElementsBySelectorRec(this, selector, result);
            return result;
        }

        public override string ToString()
        {
            string s = "\nName: " + Name + "\n Id: " + Id + "\n";
            foreach (string atr in Attributes.Keys)
            {
                s += "[" + atr + ": " + Attributes[atr] + "] ";
            }
            if (Classes.Count != 0)
            {
                s += "\nClass:";
                foreach (string class1 in Classes)
                {
                    s += " " + class1;
                }
            }
            s += "\nInnerHtml: " + InnerHtml;
            return s;
        }
    }
}

