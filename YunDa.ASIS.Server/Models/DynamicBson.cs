using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;


namespace YunDa.ASIS.Server.Models
{
    public abstract class DynamicBson
    {
        [JsonIgnore]
        public bool CanUpdate { get; set; }

        protected abstract void UpdateProperty<T>(T value, [CallerMemberName] string propertyName = null);
    }

}
