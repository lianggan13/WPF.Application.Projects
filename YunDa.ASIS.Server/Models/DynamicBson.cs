using Newtonsoft.Json;
using System.Runtime.CompilerServices;


namespace YunDa.ASIS.Server.Models
{
    public abstract class DynamicBson
    {
        [JsonIgnore]
        public bool CanUpdate { get; set; }

        protected abstract void UpdateProperty<T>(T value, [CallerMemberName] string propertyName = null);
    }

}
