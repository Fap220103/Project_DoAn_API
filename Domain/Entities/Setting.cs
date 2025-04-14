using Domain.Bases;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Entities
{
    public class Setting : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public Setting() : base() { } //for EF Core
        public Setting(
            string key,
            string value
            ) : base()
        {
            Key = key;
            Value = value;
        }

        public void Update(
            string key,
            string value
            )
        {
            Key = key;
            Value = value;
        }
    }
}
