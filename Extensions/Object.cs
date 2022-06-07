using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueType.Extensions
{
    public static class Object
    {
        public delegate T ConditionalCall<T>(ref T val);
        public static T HandleIf<T>(this T target, bool condition, ConditionalCall<T> conditionalCall)
        {
            if(condition)
                conditionalCall(ref target);
            
            return target;
        }
    }
}
