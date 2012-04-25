using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MyWMP.AttachedProperties
{
    public class AttachedProperties
    {
        #region - RegisterCommandBindings Attached Property -
        public static DependencyProperty CustomCommandBindingsProperty =
            DependencyProperty.RegisterAttached("CustomCommandBindings", typeof(CommandBindingCollection), typeof(AttachedProperties),
            new PropertyMetadata(null, OnCustomCommandBindingChanged));

        public static void SetCustomCommandBindings(UIElement element, CommandBindingCollection value)
        {
            if (element != null)
                element.SetValue(CustomCommandBindingsProperty, value);
        }
        public static CommandBindingCollection GetCustomCommandBindings(UIElement element)
        {
            return (element != null ? (CommandBindingCollection)element.GetValue(CustomCommandBindingsProperty) : null);
        }

        private static void OnCustomCommandBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UIElement element = sender as UIElement;
            if (element != null)
            {
                CommandBindingCollection bindings = e.NewValue as CommandBindingCollection;
                if (bindings != null)
                {
                    element.CommandBindings.AddRange(bindings);
                }
            }
        }
        #endregion
    }
}
