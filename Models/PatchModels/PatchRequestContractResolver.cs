using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;

namespace LibrarianWorkplaceAPI.Models.PatchModels
{
    public class PatchRequestContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            prop.SetIsSpecified += (op, op1) =>
            {
                if (op is PatchBaseModel patchBaseModel)
                {
                    patchBaseModel.SetHasProperty(prop.PropertyName);
                }
            };

            return prop;
        }
    }
}
