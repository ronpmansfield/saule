﻿using System;
using System.Linq;
using Humanizer;
using Saule.Queries;

namespace Saule.Serialization
{
    /// <summary>
    /// Used to build url paths.
    /// </summary>
    public class DefaultUrlPathBuilder : IUrlPathBuilder
    {
        private readonly string _prefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultUrlPathBuilder"/> class.
        /// </summary>
        public DefaultUrlPathBuilder()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultUrlPathBuilder"/> class.
        /// </summary>
        /// <param name="prefix">A prefix for all urls generated by this instance of the DefaultUrlPathBuilder class.</param>
        public DefaultUrlPathBuilder(string prefix)
        {
            _prefix = prefix;
        }

        internal DefaultUrlPathBuilder(string template, ApiResource resource)
        {
            var templateParts = template.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var dynamicCount = templateParts.Count(t => t.StartsWith("{"));
            var preDynamic = templateParts.TakeWhile(t => !t.StartsWith("{")).ToList();

            if (dynamicCount < 2)
            {
                preDynamic = preDynamic.Take(preDynamic.Count - 1).ToList();
            }

            _prefix = '/'.TrimJoin(preDynamic.ToArray());
        }

        /// <summary>
        /// Returns the UrlPath of the resource, ensuring it starts and ends with '/'
        /// </summary>
        /// <param name="resource">The resource this path refers to.</param>
        /// <returns>A <see cref="string"/> containing the path.</returns>
        public virtual string BuildCanonicalPath(ApiResource resource)
        {
            return '/'.TrimJoin(_prefix, resource.UrlPath)
                .EnsureStartsWith("/")
                .EnsureEndsWith("/");
        }

        /// <summary>
        /// Returns a path in the form `/resource.UrlPath/id/`.
        /// </summary>
        /// <param name="resource">The resource this path refers to.</param>
        /// <param name="id">The unique id of the resource.</param>
        /// <returns>A <see cref="string"/> containing the path.</returns>
        public virtual string BuildCanonicalPath(ApiResource resource, string id)
        {
            return '/'.TrimJoin(
                BuildCanonicalPath(resource), id)
                .EnsureStartsWith("/")
                .EnsureEndsWith("/");
        }

        /// <summary>
        /// Returns a path in the form `/resource.UrlPath/id/relationships/relationship.UrlPath/`.
        /// </summary>
        /// <param name="resource">The resource this path is related to.</param>
        /// <param name="id">The unique id of the resource.</param>
        /// <param name="relationship">The relationship this path refers to.</param>
        /// <returns>A <see cref="string"/> containing the path.</returns>
        public virtual string BuildRelationshipPath(ApiResource resource, string id, ResourceRelationship relationship)
        {
            return '/'.TrimJoin(
                BuildCanonicalPath(resource, id), "relationships", relationship.UrlPath)
                .EnsureStartsWith("/")
                .EnsureEndsWith("/");
        }

        /// <summary>
        /// Returns a path in the form `/resource.UrlPath/id/relationship.UrlPath/`.
        /// </summary>
        /// <param name="resource">The resource this path is related to.</param>
        /// <param name="id">The unique id of the resource.</param>
        /// <param name="relationship">The relationship this path refers to.</param>
        /// <param name="relatedResourceId">The id of the related resource.</param>
        /// <returns>A <see cref="string"/> containing the path.</returns>
        public virtual string BuildRelationshipPath(
            ApiResource resource,
            string id,
            ResourceRelationship relationship,
            string relatedResourceId)
        {
            return '/'.TrimJoin(
                BuildCanonicalPath(resource, id), relationship.UrlPath)
                .EnsureStartsWith("/")
                .EnsureEndsWith("/");
        }
    }
}