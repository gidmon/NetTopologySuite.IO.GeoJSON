﻿using System;
using System.IO;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;

namespace NetTopologySuite.IO
{
    /// <summary>
    /// Represents a GeoJSON Reader allowing for deserialization of various GeoJSON elements 
    /// or any object containing GeoJSON elements.
    /// </summary>
    public class GeoJsonReader
    {
        private readonly GeometryFactory _factory;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly int _dimension;
        private readonly bool _allowMeasurements;

        /// <summary>
        /// Creates an instance of this class
        /// </summary>
        public GeoJsonReader()
            : this(GeoJsonSerializer.Wgs84Factory, new JsonSerializerSettings())
        {
        }

        /// <summary>
        /// Creates an instance of this class using the provided <see cref="GeometryFactory"/> and
        /// <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <param name="factory">The factory to use when creating geometries</param>
        /// <param name="serializerSettings">The serializer setting</param>
        public GeoJsonReader(GeometryFactory factory, JsonSerializerSettings serializerSettings)
            : this(factory, serializerSettings, 2)
        {
        }

        /// <summary>
        /// Creates an instance of this class using the provided <see cref="GeometryFactory"/> and
        /// <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <param name="factory">The factory to use when creating geometries</param>
        /// <param name="serializerSettings">The serializer setting</param>
        /// <param name="dimension">The number of dimensions to handle.  Must be between 2 and 4.</param>
        public GeoJsonReader(GeometryFactory factory, JsonSerializerSettings serializerSettings, int dimension): this(factory, serializerSettings, dimension, false) { }

        /// <summary>
        /// Creates an instance of this class using the provided <see cref="GeometryFactory"/> and
        /// <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <param name="factory">The factory to use when creating geometries</param>
        /// <param name="serializerSettings">The serializer setting</param>
        /// <param name="dimension">The number of dimensions to handle.  Must be between 2 and 4.</param>
        /// <param name="allowMeasurements">If the coorinate allows measurement value to be present</param>
        public GeoJsonReader(GeometryFactory factory, JsonSerializerSettings serializerSettings, int dimension, bool allowMeasurements)
        {
            _factory = factory;
            _serializerSettings = serializerSettings;
            _dimension = dimension;
            _allowMeasurements = allowMeasurements;
        }

        /// <summary>
        /// Reads the specified json.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public TObject Read<TObject>(string json)
            where TObject : class
        {
            if (json is null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            return Read<TObject>(new JsonTextReader(new StringReader(json)));
        }

        /// <summary>
        /// Reads the specified json.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public TObject Read<TObject>(JsonReader json)
            where TObject : class
        {
            if (json is null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            var g = GeoJsonSerializer.Create(_serializerSettings, _factory, _dimension, _allowMeasurements);
            return g.Deserialize<TObject>(json);
        }
    }
}
