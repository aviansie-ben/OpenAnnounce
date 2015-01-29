using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Announcements
{
    public static class Sanitizer
    {
        public static readonly SanitizerRules NoHtml = new SanitizerRules();
        public static readonly SanitizerRules StrictRules = new SanitizerRules()
        {
            AllowedTags = new List<SanitizerTag>()
            {
                new SanitizerTag()
                {
                    TagNames = new List<string> { "em", "i" }
                },
                new SanitizerTag()
                {
                    TagNames = new List<string> { "b", "strong" }
                },
                new SanitizerTag()
                {
                    TagNames = new List<string> { "a" },
                    AllowedAttributes = new List<string>() { "href", "target" }
                },
                new SanitizerTag()
                {
                    TagNames = new List<string> { "br", "p" }
                },
                new SanitizerTag()
                {
                    TagNames = new List<string> { "ul", "ol", "li" }
                },
                new SanitizerTag()
                {
                    TagNames = new List<string> { "blockquote" }
                }
            }
        };

        public static string Sanitize(string input, SanitizerRules rules)
        {
            List<string> currentTags = new List<string>();
            string output = "";
            while (input.IndexOf('<') != -1) // Keep looking for tags while they exist
            {
                // Flush anything before the first tag
                output += HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(input.Substring(0, input.IndexOf('<'))));
                input = input.Remove(0, input.IndexOf('<') + 1);

                if (input.IndexOf('>') == -1)
                {
                    // Unclosed tag... Ignore the rest of the input.
                    input = "";
                    break;
                }

                // Retrieve the tag data
                string tagData = input.Substring(0, input.IndexOf('>'));
                input = input.Remove(0, tagData.Length + 1);

                // Remove the final slash (It will be readded later if needed)
                if (input.EndsWith("/"))
                    input.Remove(input.Length - 1);

                // Check the rule to see if the tag is allowable
                SanitizerTag tag = rules.GetTag(tagData);
                if (tag == null)
                    continue;

                // Make sure tags are closed
                if (tagData.StartsWith("/"))
                {
                    int i;
                    for (i = currentTags.Count - 1; i >= 0; i--)
                    {
                        if (currentTags[i] == tagData.Split(' ')[0].Substring(1).ToLower())
                            break;
                    }


                    if (i < 0)
                    {
                        // End tag without a beginning: Ignore
                        continue;
                    }
                    else if (i != currentTags.Count - 1)
                    {
                        // Unclosed internal tags: Close them
                        for (int j = currentTags.Count - 1; j > i; j--)
                        {
                            output += "</" + currentTags[j] + ">";
                            currentTags.RemoveAt(j);
                        }
                    }

                    currentTags.RemoveAt(currentTags.Count - 1);
                }
                else
                {
                    currentTags.Add(tagData.Split(' ')[0]);
                }

                // Output the sanitized tag
                output += "<" + tag.SanitizeTag(tagData) + ">";
            }
            // Flush the remaining text
            output += HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(input));

            // Close any unclosed tags
            for (int i = currentTags.Count - 1; i >= 0; i--)
            {
                output += "</" + currentTags[i] + ">";
            }

            return output;
        }

        public class SanitizerRules
        {
            public List<SanitizerTag> AllowedTags { get; set; }

            public SanitizerRules()
            {
                AllowedTags = new List<SanitizerTag>();
            }

            public SanitizerTag GetTag(string tagData)
            {
                string tagName = tagData.Split(' ')[0];

                if (tagName.StartsWith("/"))
                    tagName = tagName.Substring(1);

                foreach (SanitizerTag tag in AllowedTags)
                {
                    if (tag.TagNames.Contains(tagName.ToLower()))
                        return tag;
                }
                return null;
            }
        }

        public class SanitizerTag
        {
            public List<string> TagNames { get; set; }
            public List<string> AllowedAttributes { get; set; }
            public bool MustSelfClose { get; set; }

            public SanitizerTag()
            {
                AllowedAttributes = new List<string>();
            }

            public string SanitizeTag(string input)
            {
                // Don't allow attributes on end tags
                if (input.StartsWith("/"))
                    return input.Split(' ')[0];

                string output = input.Split(' ')[0];
                input = input.Remove(0, output.Length);

                // Check each attribute
                foreach (string attribute in input.Split(' '))
                {
                    string decodedAttrib = HttpUtility.HtmlDecode(attribute);

                    // Strip any control characters for checking purposes
                    for (int i = 0; i < decodedAttrib.Length; i++)
                    {
                        if (char.IsControl(decodedAttrib[i]))
                        {
                            decodedAttrib = decodedAttrib.Remove(i, 1);
                            i--;
                        }
                    }

                    // Make sure the attribute is allowed, and DO NOT EVER ALLOW JAVASCRIPT!
                    if (AllowedAttributes.Contains(attribute.Split('=')[0]) && !decodedAttrib.Contains("javascript:"))
                    {
                        output += " " + attribute;
                    }
                }

                if (MustSelfClose)
                    output += " /";

                return output;
            }
        }
    }
}