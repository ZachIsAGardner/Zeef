using System.Collections;
using UnityEngine;

namespace Zeef {
    public static class StringExtensions {

        public static string Prepend(this string str, string prependStr) {
            return prependStr + str;
        }

    }
}