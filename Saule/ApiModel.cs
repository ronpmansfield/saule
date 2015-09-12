﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saule
{
    public abstract class ApiModel
    {
        private List<ModelAttribute> _attributes = new List<ModelAttribute>();
        private List<ModelRelationship> _relationships = new List<ModelRelationship>();

        public IEnumerable<ModelAttribute> Attributes => _attributes;
        public IEnumerable<ModelRelationship> Relationships => _relationships;
        public string ModelType { get; }

        protected ApiModel()
        {
            var name = GetType().Name;
            if(name.EndsWith("Model"))
            {
                ModelType = name.Remove(name.Length - "Model".Length);
            } else
            {
                ModelType = name;
            }
        }

        protected void Attribute(string name)
        {
            _attributes.Add(new ModelAttribute(name));
        }

        protected void BelongsTo(string name, Type type)
        {
            BelongsTo(name, type, name);
        }
        protected void BelongsTo(string name, Type type, string path)
        {
            _relationships.Add(new ModelRelationship(name, RelationshipType.Single, type, path));
        }

        protected void HasMany(string name, Type type)
        {
            HasMany(name, type, name);
        }
        protected void HasMany(string name, Type type, string path)
        {
            _relationships.Add(new ModelRelationship(name, RelationshipType.Many, type, path));
        }
    }
}
