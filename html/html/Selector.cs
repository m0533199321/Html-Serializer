using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace html
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }

        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        private static HtmlHelper helper = HtmlHelper.Instance;

        public Selector()
        {
            Classes = new List<string>();
        }
        public static Selector PerSelector(string query)
        {
            Selector selector = new Selector();
            string[] queries = Regex.Split(query, @"(#|\.)");
            for (int i=0; i<queries.Length;i++)
            {
                if (queries[i].StartsWith('#') && i<queries.Length-1)
                {
                    selector.Id = queries[++i];
                }
                if (queries[i].StartsWith('.') && i < queries.Length - 1)
                {
                    selector.Classes.Add(queries[++i]);
                }
                else if (helper.HtmlTags.Contains(queries[i]))
                    selector.TagName = queries[i];
            }
            return selector;
        }
        public static Selector PerStringToSelectors(string query)
        {
            List<string> queries = query.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            Selector root = PerSelector(queries[0]);
            queries.RemoveAt(0);
            Selector current = root;
            foreach (string q in queries)
            {
                current.Child = PerSelector(q);
                current.Child.Parent = current;
                current = current.Child;
            }
            return root;
        }

        public override string ToString()
        {
            string s="";
            if(TagName != null)
            {
                s += "TagName: " + TagName;
            }
            if (Id != null)
            {
                s += " Id: " + Id;
            }
            if (Classes.Count > 0)
            {
                s += " Class:";
                foreach (var class1 in Classes)
                {
                    s += " "+class1;
                }
            }
            s += "\n!!!!!!!!!!!!!!";
            return s;
        }
    }
}
