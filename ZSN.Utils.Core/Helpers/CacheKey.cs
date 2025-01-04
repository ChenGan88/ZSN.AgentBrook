using System.Reflection;

namespace ZSN.Utils.Core.Helpers
{
    public class CacheKey
    {
        private MethodInfo Method { get; }
        private ParameterInfo[] InputArguments { get; }
        private object[] ParameterValues { get; }

        public CacheKey(MethodInfo method, ParameterInfo[] arguments, object[] values)
        {
            Method = method;
            InputArguments = arguments;
            ParameterValues = values;
        }

        public override bool Equals(object obj)
        {
            CacheKey another = obj as CacheKey;
            if (null == another)
            {
                return false;
            }
            if (!Method.Equals(another.Method))
            {
                return false;
            }
            for (int index = 0; index < InputArguments.Length; index++)
            {
                var argument1 = InputArguments[index];
                var argument2 = another.InputArguments[index];
                if (argument1 == null && argument2 == null)
                {
                    continue;
                }

                if (argument1 == null || argument2 == null)
                {
                    return false;
                }

                if (!argument1.Equals(argument2))
                {
                    return false;
                }
            }
            return true;
        }

        public string GetRedisCacheKey()
        {
            return
                $"{Method.DeclaringType.Namespace}:{Method.DeclaringType.Name}:{Method.Name}:{GetHashCode()}";
        }

        public string GetMemoryCacheKey()
        {
            return
                $"{Method.DeclaringType.Namespace}_{Method.DeclaringType.Name}_{Method.Name}_{GetHashCode()}";
        }

        public override int GetHashCode()
        {
            int hashCode = Method.GetHashCode();
            foreach (var argument in InputArguments)
            {
                hashCode = hashCode ^ argument.GetHashCode();
            }

            foreach (var value in ParameterValues)
            {
                hashCode = hashCode ^ value.GetHashCode();
            }
            return hashCode;
        }
    }
}
