﻿#region Copyright

// ****************************************************************************
// <copyright file="XamarinFormsExtensions.cs">
// Copyright (c) 2012-2015 Vyacheslav Volkov
// </copyright>
// ****************************************************************************
// <author>Vyacheslav Volkov</author>
// <email>vvs0205@outlook.com</email>
// <project>MugenMvvmToolkit</project>
// <web>https://github.com/MugenMvvmToolkit/MugenMvvmToolkit</web>
// <license>
// See license.txt in this solution or http://opensource.org/licenses/MS-PL
// </license>
// ****************************************************************************

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using JetBrains.Annotations;
using MugenMvvmToolkit.Binding;
using MugenMvvmToolkit.Binding.Builders;
using MugenMvvmToolkit.Binding.Interfaces;
using MugenMvvmToolkit.Models;
using Xamarin.Forms;

namespace MugenMvvmToolkit
{
    public static class XamarinFormsExtensions
    {
        #region Fields

        private const string NavParamKey = "@~`NavParam";

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when the back button is pressed.
        /// </summary>
        public static event EventHandler<Page, CancelEventArgs> BackButtonPressed;

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when the back button is pressed.
        /// </summary>
        public static bool HandleBackButtonPressed([NotNull] this Page page, Func<bool> baseOnBackButtonPressed = null)
        {
            Should.NotBeNull(page, "page");
            var handler = BackButtonPressed;
            if (handler == null)
                return baseOnBackButtonPressed != null && baseOnBackButtonPressed();
            var args = new CancelEventArgs(false);
            handler(page, args);
            return args.Cancel;
        }

        public static void SetNavigationParameter([NotNull] this Page controller, object value)
        {
            Should.NotBeNull(controller, "controller");
            if (value == null)
                ServiceProvider.AttachedValueProvider.Clear(controller, NavParamKey);
            else
                ServiceProvider.AttachedValueProvider.SetValue(controller, NavParamKey, value);
        }

        public static object GetNavigationParameter([CanBeNull] this Page controller)
        {
            if (controller == null)
                return null;
            return ServiceProvider.AttachedValueProvider.GetValue<object>(controller, NavParamKey, false);
        }
        public static IList<IDataBinding> SetBindings(this BindableObject item, string bindingExpression,
                    IList<object> sources = null)
        {
            return BindingServiceProvider.BindingProvider.CreateBindingsFromString(item, bindingExpression, sources);
        }

        public static T SetBindings<T, TBindingSet>([NotNull] this T item, [NotNull] TBindingSet bindingSet,
            [NotNull] string bindings)
            where T : BindableObject
            where TBindingSet : BindingSet
        {
            Should.NotBeNull(item, "item");
            Should.NotBeNull(bindingSet, "bindingSet");
            Should.NotBeNull(bindings, "bindings");
            bindingSet.BindFromExpression(item, bindings);
            return item;
        }


        public static T SetBindings<T, TBindingSet>([NotNull] this T item, [NotNull] TBindingSet bindingSet,
            [NotNull] Action<TBindingSet, T> setBinding)
            where T : BindableObject
            where TBindingSet : BindingSet
        {
            Should.NotBeNull(item, "item");
            Should.NotBeNull(bindingSet, "bindingSet");
            Should.NotBeNull(setBinding, "setBinding");
            setBinding(bindingSet, item);
            return item;
        }

        public static void ClearBindingsHierarchically([CanBeNull] this BindableObject item, bool clearDataContext, bool clearAttachedValues)
        {
            if (item == null)
                return;
            Type type = item.GetType();
            var attribute = type
                .GetTypeInfo()
                .GetCustomAttribute<ContentPropertyAttribute>(true);
            if (attribute != null)
            {
                var bindingMember = BindingServiceProvider
                    .MemberProvider
                    .GetBindingMember(type, attribute.Name, true, false);
                if (bindingMember != null)
                {
                    object content = bindingMember.GetValue(item, null);
                    var enumerable = content as IEnumerable;
                    if (enumerable == null)
                        ClearBindingsHierarchically(content as BindableObject, clearDataContext, clearAttachedValues);
                    else
                    {
                        foreach (object child in enumerable)
                            ClearBindingsHierarchically(child as BindableObject, clearDataContext, clearAttachedValues);
                    }
                }
            }
            item.ClearBindings(clearDataContext, clearAttachedValues);
        }

        public static void ClearBindings([CanBeNull] this BindableObject item, bool clearDataContext, bool clearAttachedValues)
        {
            BindingExtensions.ClearBindings(item, clearDataContext, clearAttachedValues);
        }

        internal static PlatformInfo GetPlatformInfo()
        {
            return new PlatformInfo(Device.OnPlatform(PlatformType.iOS, PlatformType.Android, PlatformType.WinPhone), new Version(0, 0));
        }

        internal static void AsEventHandler<TArg>(this Action action, object sender, TArg arg)
        {
            action();
        }

        #endregion
    }
}