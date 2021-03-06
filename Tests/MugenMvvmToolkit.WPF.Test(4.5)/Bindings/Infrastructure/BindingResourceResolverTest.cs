﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MugenMvvmToolkit.Binding.Behaviors;
using MugenMvvmToolkit.Binding.Converters;
using MugenMvvmToolkit.Binding.Infrastructure;
using MugenMvvmToolkit.Binding.Interfaces;
using MugenMvvmToolkit.Binding.Models;
using MugenMvvmToolkit.Models;
using MugenMvvmToolkit.Test.TestModels;
using Should;

namespace MugenMvvmToolkit.Test.Bindings.Infrastructure
{
    [TestClass]
    public class BindingResourceResolverTest : BindingTestBase
    {
        #region Methods

        [TestMethod]
        public void ResolverShouldRegisterAndResolveConverter()
        {
            const string name = "name";
            var source = new InverseBooleanValueConverter();
            var resolver = CreateBindingResourceResolver();

            resolver.ResolveConverter(name, EmptyContext, false).ShouldBeNull();
            resolver.AddConverter(name, source, true);
            resolver.ResolveConverter(name, EmptyContext, true).ShouldEqual(source);
        }

        [TestMethod]
        public void ResolverShouldUnregisterConverter()
        {
            const string name = "name";
            var source = new InverseBooleanValueConverter();
            var resolver = CreateBindingResourceResolver();

            resolver.AddConverter(name, source, true);
            resolver.ResolveConverter(name, EmptyContext, true).ShouldEqual(source);

            resolver.RemoveConverter(name).ShouldBeTrue();
            resolver.ResolveConverter(name, EmptyContext, false).ShouldBeNull();
        }

        [TestMethod]
        public void ResolverShouldThrowExceptionIfConverterIsNotRegistered()
        {
            var resolver = CreateBindingResourceResolver();
            ShouldThrow(() => resolver.ResolveConverter("test", EmptyContext, true));
        }

        [TestMethod]
        public void ResolverShouldThrowExceptionIfConverterIsAlreadyRegisteredRewriteFalse()
        {
            const string name = "name";
            var source = new InverseBooleanValueConverter();
            var resolver = CreateBindingResourceResolver();
            resolver.AddConverter(name, source, false);
            ShouldThrow(() => resolver.AddConverter(name, source, false));
        }

        [TestMethod]
        public void ResolverShouldNotThrowExceptionIfConverterIsAlreadyRegisteredRewriteTrue()
        {
            const string name = "name";
            var source = new InverseBooleanValueConverter();
            var source2 = new InverseBooleanValueConverter();
            var resolver = CreateBindingResourceResolver();
            resolver.AddConverter(name, source, true);
            resolver.AddConverter(name, source2, true);
            resolver.ResolveConverter(name, EmptyContext, true).ShouldEqual(source2);
        }

        [TestMethod]
        public void ResolverShouldRegisterAndResolveObject()
        {
            const string name = "name";
            var source = new BindingResourceObject("test");
            var resolver = CreateBindingResourceResolver();

            resolver.ResolveObject(name, EmptyContext, false).ShouldBeNull();
            resolver.AddObject(name, source, true);
            resolver.ResolveObject(name, EmptyContext, true).ShouldEqual(source);
        }

        [TestMethod]
        public void ResolverShouldUnregisterObject()
        {
            const string name = "name";
            var source = new BindingResourceObject("test");
            var resolver = CreateBindingResourceResolver();

            resolver.AddObject(name, source, true);
            resolver.ResolveObject(name, EmptyContext, true).ShouldEqual(source);

            resolver.RemoveObject(name).ShouldBeTrue();
            resolver.ResolveObject(name, EmptyContext, false).ShouldBeNull();
        }

        [TestMethod]
        public void ResolverShouldThrowExceptionIfObjectIsNotRegistered()
        {
            var resolver = CreateBindingResourceResolver();
            ShouldThrow(() => resolver.ResolveObject("test", EmptyContext, true));
        }

        [TestMethod]
        public void ResolverShouldThrowExceptionIfObjectIsAlreadyRegisteredRewriteFalse()
        {
            const string name = "name";
            var source = new BindingResourceObject("test");
            var resolver = CreateBindingResourceResolver();
            resolver.AddObject(name, source, false);
            ShouldThrow(() => resolver.AddObject(name, source, false));
        }

        [TestMethod]
        public void ResolverShouldNotThrowExceptionIfObjectIsAlreadyRegisteredRewriteTrue()
        {
            const string name = "name";
            var source = new BindingResourceObject("test");
            var source2 = new BindingResourceObject("test");
            var resolver = CreateBindingResourceResolver();
            resolver.AddObject(name, source, false);
            resolver.AddObject(name, source2, true);
            resolver.ResolveObject(name, EmptyContext, true).ShouldEqual(source2);
        }

        [TestMethod]
        public void ResolverShouldRegisterAndResolveMethod()
        {
            const string name = "name";
            var source = new BindingResourceMethod((list, objects, c) => objects[0], typeof(object));
            var resolver = CreateBindingResourceResolver();

            resolver.ResolveMethod(name, EmptyContext, false).ShouldBeNull();
            resolver.AddMethod(name, source, true);
            resolver.ResolveMethod(name, EmptyContext, true).ShouldEqual(source);
        }

        [TestMethod]
        public void ResolverShouldUnregisterMethod()
        {
            const string name = "name";
            var source = new BindingResourceMethod((list, objects, c) => objects[0], typeof(object));
            var resolver = CreateBindingResourceResolver();

            resolver.AddMethod(name, source, true);
            resolver.ResolveMethod(name, EmptyContext, true).ShouldEqual(source);

            resolver.RemoveMethod(name).ShouldBeTrue();
            resolver.ResolveMethod(name, EmptyContext, false).ShouldBeNull();
        }

        [TestMethod]
        public void ResolverShouldThrowExceptionIfMethodIsNotRegistered()
        {
            var resolver = CreateBindingResourceResolver();
            ShouldThrow(() => resolver.ResolveMethod("test", EmptyContext, true));
        }

        [TestMethod]
        public void ResolverShouldThrowExceptionIfMethodIsAlreadyRegisteredRewriteFalse()
        {
            const string name = "name";
            var source = new BindingResourceMethod((list, objects, c) => objects[0], typeof(object));
            var resolver = CreateBindingResourceResolver();
            resolver.AddMethod(name, source, false);
            ShouldThrow(() => resolver.AddMethod(name, source, false));
        }

        [TestMethod]
        public void ResolverShouldThrowExceptionIfMethodIsAlreadyRegisteredRewriteTrue()
        {
            const string name = "name";
            var source = new BindingResourceMethod((list, objects, c) => objects[0], typeof(object));
            var source2 = new BindingResourceMethod((list, objects, c) => objects[0], typeof(object));
            var resolver = CreateBindingResourceResolver();
            resolver.AddMethod(name, source, true);
            resolver.AddMethod(name, source2, true);
            resolver.ResolveMethod(name, EmptyContext, true).ShouldEqual(source2);
        }

        [TestMethod]
        public void ResolverShouldRegisterAndResolveType()
        {
            const string name = "name";
            var source = typeof(object);
            var resolver = CreateBindingResourceResolver();

            resolver.ResolveType(name, EmptyContext, false).ShouldBeNull();
            resolver.AddType(name, source, true);
            resolver.ResolveType(name, EmptyContext, true).ShouldEqual(source);
        }

        [TestMethod]
        public void ResolverShouldUnregisterType()
        {
            const string name = "name";
            var source = typeof(object);
            var resolver = CreateBindingResourceResolver();

            resolver.AddType(name, source, true);
            resolver.ResolveType(name, EmptyContext, true).ShouldEqual(source);

            resolver.RemoveType(name).ShouldBeTrue();
            resolver.ResolveType(name, EmptyContext, false).ShouldBeNull();
        }

        [TestMethod]
        public void ResolverShouldThrowExceptionIfTypeIsNotRegistered()
        {
            var resolver = CreateBindingResourceResolver();
            ShouldThrow(() => resolver.ResolveType("test", EmptyContext, true));
        }

        [TestMethod]
        public void ResolverShouldThrowExceptionIfTypeIsAlreadyRegisteredRewriteFalse()
        {
            const string name = "name";
            var source = typeof(object);
            var resolver = CreateBindingResourceResolver();
            resolver.AddType(name, source, false);
            ShouldThrow(() => resolver.AddType(name, source, false));
        }

        [TestMethod]
        public void ResolverShouldThrowExceptionIfTypeIsAlreadyRegisteredRewriteTrue()
        {
            const string name = "name";
            var source = typeof(object);
            var source2 = typeof(string);
            var resolver = CreateBindingResourceResolver();
            resolver.AddType(name, source, true);
            resolver.AddType(name, source2, true);
            resolver.ResolveType(name, EmptyContext, true).ShouldEqual(source2);
        }

        [TestMethod]
        public void ResolverShouldResolveTypeUsingFromTypeMethod()
        {
            const string typeName = "System.AppDomain";
            var type = Type.GetType(typeName, true);
            var resolver = CreateBindingResourceResolver();
            resolver.ResolveType(typeName, EmptyContext, true).ShouldEqual(type);
        }

        [TestMethod]
        public void ResolverShouldReturnKnownTypes()
        {
            var resolver = CreateBindingResourceResolver();
            var types = resolver.GetType()
                .GetFieldEx("_types", MemberFlags.Instance | MemberFlags.NonPublic)
                .GetValueEx<IDictionary<string, Type>>(resolver);
            resolver.GetKnownTypes().SequenceEqual(types.Values.Distinct()).ShouldBeTrue();
        }

        [TestMethod]
        public void ResolverShouldRegisterAndResolveBehavior()
        {
            const string name = "name";
            var source = new BindingBehaviorMock();
            var resolver = CreateBindingResourceResolver();

            resolver.ResolveBehavior(name, EmptyContext, Empty.Array<string>(), false).ShouldBeNull();
            resolver.AddBehavior(name, (context, list) =>
            {
                context.ShouldEqual(EmptyContext);
                list.ShouldEqual(Empty.Array<string>());
                return source;
            }, true);
            resolver.ResolveBehavior(name, EmptyContext, Empty.Array<string>(), true).ShouldEqual(source);
        }

        [TestMethod]
        public void ResolverShouldUnregisterBehavior()
        {
            const string name = "name";
            var source = new BindingBehaviorMock();
            var resolver = CreateBindingResourceResolver();

            resolver.AddBehavior(name, (context, list) => source, true);
            resolver.ResolveBehavior(name, EmptyContext, Empty.Array<string>(), true).ShouldEqual(source);

            resolver.RemoveBehavior(name).ShouldBeTrue();
            resolver.ResolveBehavior(name, EmptyContext, Empty.Array<string>(), false).ShouldBeNull();
        }

        [TestMethod]
        public void ResolverShouldThrowExceptionIfBehaviorIsNotRegistered()
        {
            var resolver = CreateBindingResourceResolver();
            ShouldThrow(() => resolver.ResolveBehavior("test", EmptyContext, null, true));
        }

        [TestMethod]
        public void ResolverShouldThrowExceptionIfBehaviorIsAlreadyRegisteredRewriteFalse()
        {
            const string name = "name";
            var resolver = CreateBindingResourceResolver();
            resolver.AddBehavior(name, (context, list) => null, false);
            ShouldThrow(() => resolver.AddBehavior(name, (context, list) => null, false));
        }

        [TestMethod]
        public void ResolverShouldThrowExceptionIfBehaviorIsAlreadyRegisteredRewriteTrue()
        {
            const string name = "name";
            var resolver = CreateBindingResourceResolver();
            resolver.AddBehavior(name, (context, list) => null, true);
            resolver.AddBehavior(name, (context, list) => NoneBindingMode.Instance, true);
            resolver.ResolveBehavior(name, null, null, true).ShouldEqual(NoneBindingMode.Instance);
        }

        protected virtual IBindingResourceResolver CreateBindingResourceResolver()
        {
            return new BindingResourceResolver();
        }

        #endregion
    }
}