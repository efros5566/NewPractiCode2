using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jsaaaaaa
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }
        public HtmlElement()
        {
            Attributes = new List<string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
        }
        public IEnumerable<HtmlElement> Descendants()
        {
            var queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var currentElement = queue.Dequeue();
                yield return currentElement;

                foreach (var child in currentElement.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            var currentElement = this;
            while (currentElement != null)
            {
                yield return currentElement;
                currentElement = currentElement.Parent;
            }
        }
        public HashSet<HtmlElement> GetElementsBySelector(Selector selector)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();
            FindRecursive(selector, result);
            return result;
        }
        private void FindRecursive(Selector selector, HashSet<HtmlElement> result)
        {
            var descendants = Descendants();

            foreach (var descendant in descendants)
            {
                if (descendant.MatchesSelector(selector))
                {
                    result.Add(descendant);
                }
            }
        }
        public bool MatchesSelector(Selector selector)
        {
            bool flag = true;
            if (!string.IsNullOrEmpty(selector.TagName) && Name != selector.TagName)
                return false;
            if (!string.IsNullOrEmpty(selector.Id) && Id != selector.Id)
                return false;
            if (selector.Classes != null && selector.Classes.Count > 0)
            {
                foreach (var className in selector.Classes)
                {
                    if (!Classes.Contains(className))
                        return false;
                }
            }
            if (selector.Child != null)
            {
                if (!flag)
                {
                    foreach (var child in Children)
                    {
                        flag = child.Children.Any(child => child.MatchesSelector(selector.Child));
                    }
                }
            }
            return flag;
        }
    }
}
