using Newtonsoft.Json.Serialization;

namespace Yamaha.AccountStatement.Infrastructure.Resolvers
{
    public class UpperCasePropertyNamesContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToUpper();
        }
    }
}
