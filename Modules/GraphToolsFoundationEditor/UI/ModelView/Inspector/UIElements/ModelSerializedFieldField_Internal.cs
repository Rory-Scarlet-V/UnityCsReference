// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.CommandStateObserver;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.GraphToolsFoundation.Editor
{
    /// <summary>
    /// A UI field to display a field from a <see cref="GraphElementModel"/> or its surrogate, if it implements <see cref="IHasInspectorSurrogate"/>. Used by the <see cref="SerializedFieldsInspector"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to display.</typeparam>
    class ModelSerializedFieldField_Internal<TValue> : ModelPropertyField<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelSerializedFieldField_Internal{TValue}"/> class.
        /// </summary>
        /// <param name="commandTarget">The dispatcher to use to dispatch commands when the field is edited.</param>
        /// <param name="models">The inspected model.</param>
        /// <param name="inspectedObjects">The models that owns the field.</param>
        /// <param name="inspectedField">The inspected field.</param>
        /// <param name="fieldTooltip">The tooltip for the field.</param>
        public ModelSerializedFieldField_Internal(
            ICommandTarget commandTarget,
            IEnumerable<Model> models,
            IEnumerable<object> inspectedObjects,
            FieldInfo inspectedField,
            string fieldTooltip)
            : base(commandTarget, models, inspectedField.Name, null, fieldTooltip)
        {
            m_ValueGetter = MakeFieldValueGetter(inspectedField, inspectedObjects);

            switch (m_Field)
            {
                case null:
                    break;

                case PopupField<string> _:
                    RegisterChangedCallback<string>(evt => Enum.Parse(typeof(TValue), evt.newValue),
                        inspectedObjects, inspectedField);
                    break;

                case BaseField<Enum> _:
                    RegisterChangedCallback<Enum>(evt => evt.newValue, inspectedObjects, inspectedField);
                    break;

                default:
                    RegisterChangedCallback<TValue>(evt => evt.newValue, inspectedObjects, inspectedField);
                    break;
            }
        }

        void RegisterChangedCallback<TCallbackValue>(Func<ChangeEvent<TCallbackValue>, object> valueExtractor,
            IEnumerable<object> inspectedObjects, FieldInfo inspectedField)
        {
            if (inspectedObjects.First() is GraphElementModel)
            {
                m_Field.RegisterCallback<ChangeEvent<TCallbackValue>, ModelPropertyField<TValue>>(
                    (e, f) =>
                    {
                        var newValue = valueExtractor(e);
                        var command = new SetInspectedGraphElementModelFieldCommand(newValue, inspectedObjects.OfType<GraphElementModel>(), inspectedObjects, inspectedField);
                        f.CommandTarget.Dispatch(command);
                    }, this);
            }
            else if (inspectedObjects.Count() == 1 && inspectedObjects.First() is GraphModel graphModel)
            {
                m_Field.RegisterCallback<ChangeEvent<TCallbackValue>, ModelPropertyField<TValue>>(
                    (e, f) =>
                    {
                        var newValue = valueExtractor(e);
                        var command = new SetInspectedGraphModelFieldCommand(newValue, graphModel, inspectedObjects, inspectedField);
                        f.CommandTarget.Dispatch(command);
                    }, this);
            }
        }

        static Func<TValue> MakeFieldValueGetter(FieldInfo fieldInfo, IEnumerable<object> inspectedObjects)
        {
            if (fieldInfo != null && inspectedObjects != null)
            {
                var usePropertyAttribute = fieldInfo.GetCustomAttribute<InspectorUsePropertyAttribute>();
                if (usePropertyAttribute != null)
                {
                    var propertyInfo = ModelHelpers.GetCommonBaseType(inspectedObjects).GetProperty(usePropertyAttribute.PropertyName);
                    if (propertyInfo != null)
                    {
                        Debug.Assert(typeof(TValue) == propertyInfo.PropertyType);
                        if (inspectedObjects.Count() == 1)
                            return () => (TValue)propertyInfo.GetMethod.Invoke(inspectedObjects.First(), null);

                        return MakeMultipleGetter(obj => (TValue)propertyInfo.GetMethod.Invoke(obj, null), inspectedObjects);
                    }
                }

                Debug.Assert(typeof(TValue) == fieldInfo.FieldType);
                if (inspectedObjects.Count() == 1)
                    return () => (TValue)fieldInfo.GetValue(inspectedObjects.First());

                return MakeMultipleGetter(obj => (TValue)fieldInfo.GetValue(obj), inspectedObjects);
            }

            return null;
        }

        static Func<TValue> MakeMultipleGetter(Func<object, TValue> getFunction, IEnumerable<object> objects)
        {
            TValue value = getFunction(objects.First());

            foreach (var obj in objects.Skip(1))
            {
                if (!Equals(value, getFunction(obj)))
                    return k_GetMixed;
            }

            return () => getFunction(objects.First());
        }
    }
}
