namespace Zeef 
{
    public static class ArrayExtensions 
    {  
        public static T Random<T> (this T[] arr) => arr[Utility.RandomInt(arr.Length - 1)];

        public static bool IsNullOrEmpty<T> (this T[] arr) => arr == null || arr.Length < 1;   
    }
}