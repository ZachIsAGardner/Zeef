using System;
using UnityEngine;

namespace Zeef 
{
    public static class FloatExtensions 
    { 
        public static float MoveOverTime(this float num, float destination, float time) 
        {
			return Mathf.Lerp(num, destination, 1 - Mathf.Pow(time, Time.deltaTime));
        }

        public static string Moneyify(this float num) 
        {
            string[] splits = num.ToString().Split(',');

            if (splits.Length < 2) 
                splits = new string[]{ splits[0], "00" };

            while(splits[1].Length < 2)
                splits[1] += "0";

            splits[1] = splits[1].Substring(0, 2);

            return "$" + String.Join(".", splits);
        }
    }
}