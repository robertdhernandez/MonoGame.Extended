using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MonoGame.Extended.Gui.Controls;

namespace MonoGame.Extended.Gui
{
    public class GuiControlStyle
    {
        private Dictionary<string, object> _previousState;

        public GuiControlStyle(Type targetType)
        {
            TargetType = targetType;
            Setters = new Dictionary<string, object>();
        }

        public Type TargetType { get; }
        public Dictionary<string, object> Setters { get; }
        
        public void ApplyIf(GuiControl control, bool predicate)
        {
            if (predicate)
                Apply(control);
            else
                Revert(control);
        }

        public void Apply(GuiControl control)
        {
            _previousState = Setters
                .ToDictionary(i => i.Key, i => TargetType.GetRuntimeProperty(i.Key).GetValue(control));

            ChangePropertyValues(control, Setters);
        }

        public void Revert(GuiControl control)
        {
            if (_previousState != null)
                ChangePropertyValues(control, _previousState);

            _previousState = null;
        }

        private static void ChangePropertyValues(GuiControl control, Dictionary<string, object> setters)
        {
            var targetType = control.GetType();

            foreach (var propertyName in setters.Keys)
            {
                var propertyInfo = targetType.GetRuntimeProperty(propertyName);
                var value = setters[propertyName];
                propertyInfo.SetValue(control, value);
            }
        }
    }
}