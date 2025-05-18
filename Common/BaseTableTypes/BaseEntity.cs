using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.BaseTableTypes
{
    public class BaseEntity
    {
        public int Id { get; set; }

        private Dictionary<string, object> _originalValues = new();

        private List<string> IgnoreFields = new List<string>()
        {
            "CreatedById", "CreatedDate", "ModifiedById", "ModifiedDate"
        };
        public void TrackChanges()
        {
            _originalValues.Clear();
            foreach (var propertyInfo in GetType().GetProperties())
            {
                if (!IgnoreFields.Contains(propertyInfo.Name))
                    _originalValues[propertyInfo.Name] = propertyInfo.GetValue(this);
            }
        }

        public bool HasChanged()
        {
            return _originalValues.Any(x => !Equals(x.Value, GetType().GetProperty(x.Key)?.GetValue(this)));
        }

        // ... other properties and methods
    }
}
