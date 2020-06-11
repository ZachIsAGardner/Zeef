using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zeef {

    public enum MatchSpecificityEnum 
    {
        CaseSensitive,
        CaseInsensitive
    }

    public static class StringExtensions 
    {
        public static string Prepend(this string str, string prependStr) 
        {
            return prependStr + str;
        }

        public static bool Matches(this string me, string comparator, MatchSpecificityEnum specifity = MatchSpecificityEnum.CaseInsensitive) 
        {
            switch (specifity) 
            {
                case MatchSpecificityEnum.CaseInsensitive:
                    return me.ToLower() == comparator.ToLower();

                case MatchSpecificityEnum.CaseSensitive:
                    return me == comparator.ToLower();

                default:
                    throw new Exception("Invalid specificity");
            }
        }

        // [Apples, Oranges, Condoms]
        // "Apples, Oranges, and Condoms"
        public static string Andify(this List<string> list) 
        {
            return Ify(list, "and");
        }
        public static string Andify(this string[] arr) 
        {
            return Ify(arr.ToList(), "and");
        }
        public static string Andify(this IEnumerable<string> enumerable) 
        {
            return Ify(enumerable.ToList(), "and");
        }

        // [Love, Pizza, Death]
        // "Love, Pizza, or Death"
        public static string Orify(this List<string> list) 
        {
            return Ify(list, "or");
        }
        public static string Orify(this string[] arr) 
        {
            return Ify(arr.ToList(), "or");
        }

        private static string Ify(List<string> list, string connector) 
        {
            string result = string.Join(", ", list.ToArray());
            int last = result.LastIndexOf(',');
            
            if (last == -1) return result;

            result = result.Remove(last, 1).Insert(last, " " + connector);

            return result;
        }
    }
}