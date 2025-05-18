using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Common
{
    public class IntIdentityBase : IChangeTracker
    {
        /// <summary>
        /// Key for this table.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        public int CreatedById { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime CreatedDate { get; set; }
        public int ModifiedById { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime ModifiedDate { get; set; }

        private List<string> IgnoreFields = new List<string>()
        {
            "CreatedById", "CreatedDate", "ModifiedById", "ModifiedDate"
        };

        private Dictionary<string, object> _originalValues = new();
        public void TrackChanges()
        {
            _originalValues.Clear();
            foreach (var propertyInfo in GetType().GetProperties())
            {
                var notMappedAttribute = propertyInfo.GetCustomAttribute<NotMappedAttribute>();
                if (notMappedAttribute != null)
                {
                    continue; // Skip properties with [NotMapped]
                }
                // Skip virtual properties
                if (propertyInfo.GetMethod != null && propertyInfo.GetMethod.IsVirtual && !propertyInfo.GetMethod.IsFinal)
                {
                    continue;
                }
                _originalValues[propertyInfo.Name] = propertyInfo.GetValue(this);
            }
        }

        public bool HasChanged()
        {
            return _originalValues.Where(x => !IgnoreFields.Contains(x.Key)).Any(x => !Equals(x.Value, GetType().GetProperty(x.Key)?.GetValue(this)));
        }
        public List<Tuple<string, string, string>> GetChanges()
        {
            var ret = new List<Tuple<string, string, string>>();
            foreach (var changedProperty in _originalValues.Where(x => !Equals(x.Value, GetType().GetProperty(x.Key)?.GetValue(this))))
            {
                if (!IgnoreFields.Contains(changedProperty.Key))
                    ret.Add(Tuple.Create($"{changedProperty.Key}", $"{changedProperty.Value}", $"{GetType().GetProperty(changedProperty.Key)?.GetValue(this)}"));
            }
            return ret;
        }
    }

    public abstract class IntIdentityBaseNoUser
    {
        /// <summary>
        /// Key for this table.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime ModifiedDate { get; set; }
    }

}
