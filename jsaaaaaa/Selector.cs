using System;
using System.Collections.Generic;

namespace jsaaaaaa
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        // פונקציה סטטית להמרת מחרוזת שאילתה לאובייקט Selector
        public static Selector ParseSelectorString(string selectorString)
        {
            Selector rootSelector = new Selector();
            Selector currentSelector = rootSelector;

            // פרק את המחרוזת לחלקים לפי המפרידים # ו-.
            string[] parts = selectorString.Split(new char[] { '#', '.' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in parts)
            {
                if (part.StartsWith("#"))
                {
                    // יש חלק במחרוזת שמתחיל ב-#, זהו Id
                    currentSelector.Id = part.Substring(1);
                }
                else if (part.StartsWith("."))
                {
                    // יש חלק במחרוזת שמתחיל ב-., זהו Class
                    if (currentSelector.Classes == null)
                        currentSelector.Classes = new List<string>();

                    currentSelector.Classes.Add(part.Substring(1));
                }
                else
                {
                    // חלק במחרוזת שאינו מתחיל ב-# או ב-., זהו TagName (אם תקין)
                    if (HtmlHelper.instance.HtmlTags.Contains(part)|| HtmlHelper.instance.HtmlVoidTags.Contains(part))
                    {
                        Selector newSelector = new Selector();
                        newSelector.Parent = currentSelector;
                        currentSelector.Child = newSelector;
                        currentSelector = newSelector;

                        currentSelector.TagName = part;
                    }
                    // אחרת, טופל כחלק מתוך ה-Tag ולא מזוהה כ-TagName
                    else
                    {
                        if (currentSelector.Classes == null)
                            currentSelector.Classes = new List<string>();
                        currentSelector.Classes.Add(part);
                    }
                }
            }
            return rootSelector;
        }

    }
}
