using System.Collections.Generic;

namespace Zeef.TwoDimensional
{
    public static partial class InteractionTypeConstant 
    {  
        public const string Any = "Any";
        public const string Contact = "Contact"; 
        public const string Projectile = "Projectile"; 
        public const string Explosion = "Explosion"; 

        public static List<string> All
        { 
            get { 
                List<string> result = new List<string>() 
                { 
                    Any,
                    Contact, 
                    Projectile, 
                    Explosion
                };
                Extra(ref result);
                return result;
            }
        }

        static partial void Extra(ref List<string> list);
    }
}