using Autofac.Core;
using System.Reflection;

namespace YunDa.ASIS.Server.Utility.Autofac
{
    public class CusotmPropertySelector : IPropertySelector
    {
        public bool InjectProperty(PropertyInfo propertyInfo, object instance)
        {
            //在这里就是判断哪些属性是需要做属性注入的
            return propertyInfo.CustomAttributes.Any(c => c.AttributeType == typeof(CusotmSelectAttribute));
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CusotmSelectAttribute : Attribute
    {
    }
}
