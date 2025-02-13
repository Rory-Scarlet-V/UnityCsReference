// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Unity.GraphToolsFoundation.Editor
{
    /// <summary>
    /// Inspector for the serializable fields of a <see cref="GraphElementModel"/> or its surrogate, if it implements <see cref="IHasInspectorSurrogate"/>.
    /// </summary>
    class SerializedFieldsInspector : FieldsInspector
    {
        const BindingFlags k_FieldFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        /// <summary>
        /// Determines if the field can be inspected. A field can be inspected if it is public or if it has the
        /// <see cref="SerializeField"/> attribute. In addition, it must not have the <see cref="HideInInspector"/>
        /// attribute nor the <see cref="NonSerializedAttribute"/> attribute.
        /// </summary>
        /// <param name="f">The field to inspect.</param>
        /// <returns>True if the field can be inspected, false otherwise.</returns>
        public static bool CanBeInspected(FieldInfo f)
        {
            if (f != null)
            {
                var isSerializable = (f.Attributes & FieldAttributes.Public) == FieldAttributes.Public ||
                    (f.Attributes & FieldAttributes.Private) == FieldAttributes.Private &&
                    f.GetCustomAttribute<SerializeField>() != null;
                isSerializable &= !f.IsNotSerialized;

                if (isSerializable
                    && f.GetCustomAttribute<HideInInspector>() == null
                    && f.GetCustomAttribute<ObsoleteAttribute>() == null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SerializedFieldsInspector"/> class.
        /// </summary>
        /// <param name="name">The name of the part.</param>
        /// <param name="models">The models displayed in this part.</param>
        /// <param name="rootView">The root view.</param>
        /// <param name="parentClassName">The class name of the parent.</param>
        /// <param name="filter">A filter function to select which fields are displayed in the inspector. If null, defaults to <see cref="CanBeInspected"/>.</param>
        /// <returns>A new instance of <see cref="SerializedFieldsInspector"/>.</returns>
        public static SerializedFieldsInspector Create(string name, IEnumerable<Model> models, RootView rootView,
            string parentClassName, Func<FieldInfo, bool> filter = null)
        {
            return new SerializedFieldsInspector(name, models, rootView, parentClassName, filter);
        }

        Func<FieldInfo, bool> m_Filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializedFieldsInspector"/> class.
        /// </summary>
        /// <param name="name">The name of the part.</param>
        /// <param name="models">The models displayed in this part.</param>
        /// <param name="rootView">The root view.</param>
        /// <param name="parentClassName">The class name of the parent.</param>
        /// <param name="filter">A filter function to select which fields are displayed in the inspector. If null, defaults to <see cref="CanBeInspected"/>.</param>
        protected SerializedFieldsInspector(string name, IEnumerable<Model> models, RootView rootView,
            string parentClassName, Func<FieldInfo, bool> filter)
            : base(name, models, rootView, parentClassName)
        {
            m_Filter = filter ?? CanBeInspected;
        }

        /// <summary>
        /// Gets the objects displayed by the inspector. It usually is the model passed to the constructor, unless the
        /// model implements <see cref="IHasInspectorSurrogate"/>, in which case it is the surrogate object.
        /// </summary>
        /// <returns>The inspected object.</returns>
        public IEnumerable<object> GetInspectedObjects()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global : IHasInspectorSurrogate is for use by clients.
            return m_Models.Select(t => t is IHasInspectorSurrogate surrogate ? surrogate.Surrogate: t);
        }

        /// <inheritdoc />
        protected override IEnumerable<BaseModelPropertyField> GetFields()
        {
            var targets = GetInspectedObjects();

            if (targets == null)
                yield break;

            var type = ModelHelpers.GetCommonBaseType(targets);

            if (type == null)
                yield break;

            var typeList = new List<Type>();

            while (type != null)
            {
                typeList.Insert(0, type);
                type = type.BaseType;
            }

            foreach (var t in typeList)
            {
                var fields = t.GetFields(k_FieldFlags);
                foreach (var fieldInfo in fields.Where(m_Filter))
                {
                    var tooltip = "";
                    var tooltipAttribute = fieldInfo.GetCustomAttribute<TooltipAttribute>();
                    if (tooltipAttribute != null)
                    {
                        tooltip = tooltipAttribute.tooltip;
                    }

                    var modelFieldFieldType = typeof(ModelSerializedFieldField_Internal<>).MakeGenericType(fieldInfo.FieldType);
                    yield return Activator.CreateInstance(
                            modelFieldFieldType, RootView, m_Models, targets, fieldInfo, tooltip)
                        as BaseModelPropertyField;
                }
            }
        }
    }
}
