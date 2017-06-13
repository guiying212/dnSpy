﻿/*
    Copyright (C) 2014-2017 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace dnSpy.Debugger.DotNet.Metadata.Impl {
	sealed class DmdMethodRef : DmdMethodInfoBase {
		public override string Name { get; }
		public override DmdType DeclaringType => __resolvedMethod_DONT_USE?.DeclaringType ?? declaringTypeRef;
		public override DmdType ReflectedType => DeclaringType;
		public override bool IsMetadataReference => true;
		public override int MetadataToken => ResolvedMethod.MetadataToken;
		public override DmdMethodImplAttributes MethodImplementationFlags => ResolvedMethod.MethodImplementationFlags;
		public override DmdMethodAttributes Attributes => ResolvedMethod.Attributes;
		public override bool IsGenericMethodDefinition => ResolvedMethod.IsGenericMethodDefinition;
		public override bool IsGenericMethod => ResolvedMethod.IsGenericMethod;

		DmdMethodInfo ResolvedMethod => GetResolvedMethod(throwOnError: true);
		DmdMethodInfo GetResolvedMethod(bool throwOnError) {
			if ((object)__resolvedMethod_DONT_USE != null)
				return __resolvedMethod_DONT_USE;
			lock (LockObject) {
				if ((object)__resolvedMethod_DONT_USE != null)
					return __resolvedMethod_DONT_USE;
				var declType = declaringTypeRef.Resolve(throwOnError);
				if ((object)declType != null) {
					var nonGenericInstDeclType = declType.IsGenericType ? declType.GetGenericTypeDefinition() : declType;
					var nonGenericInstDeclTypeMethod = nonGenericInstDeclType?.GetMethod(Name, rawMethodSignature, throwOnError: false) as DmdMethodDef;
					if ((object)nonGenericInstDeclTypeMethod != null) {
						__resolvedMethod_DONT_USE = (object)nonGenericInstDeclTypeMethod.DeclaringType == declType ?
							nonGenericInstDeclTypeMethod :
							declType.GetMethod(nonGenericInstDeclTypeMethod.MetadataToken) as DmdMethodDef;
						Debug.Assert((object)__resolvedMethod_DONT_USE != null);
					}
				}
				if ((object)__resolvedMethod_DONT_USE != null) {
					Debug.Assert(__resolvedMethod_DONT_USE.ReflectedType == declaringTypeRef);
					return __resolvedMethod_DONT_USE;
				}
				if (throwOnError)
					throw new MethodResolveException(this);
				return null;
			}
		}
		DmdMethodInfo __resolvedMethod_DONT_USE;

		readonly DmdType declaringTypeRef;
		readonly DmdMethodSignature rawMethodSignature;
		readonly DmdMethodSignature methodSignature;

		public DmdMethodRef(DmdType declaringTypeRef, string name, DmdMethodSignature rawMethodSignature, DmdMethodSignature methodSignature) {
			this.declaringTypeRef = declaringTypeRef ?? throw new ArgumentNullException(nameof(declaringTypeRef));
			Name = name ?? throw new ArgumentNullException(nameof(name));
			this.rawMethodSignature = rawMethodSignature ?? throw new ArgumentNullException(nameof(rawMethodSignature));
			this.methodSignature = methodSignature ?? throw new ArgumentNullException(nameof(methodSignature));
		}

		public override DmdMethodInfo Resolve(bool throwOnError) => GetResolvedMethod(throwOnError);
		public override ReadOnlyCollection<DmdParameterInfo> GetParameters() => ResolvedMethod.GetParameters();
		public override ReadOnlyCollection<DmdType> GetGenericArguments() => ResolvedMethod.GetGenericArguments();
		public override DmdMethodBody GetMethodBody() => ResolvedMethod.GetMethodBody();
		public override DmdMethodSignature GetMethodSignature() => methodSignature;
		public override IList<DmdCustomAttributeData> GetCustomAttributesData() => ResolvedMethod.GetCustomAttributesData();
		public override DmdParameterInfo ReturnParameter => ResolvedMethod.ReturnParameter;
		public override DmdMethodInfo GetBaseDefinition() => ResolvedMethod.GetBaseDefinition();
		internal override DmdMethodInfo GetParentDefinition() => ResolvedMethod.GetParentDefinition();
		public override DmdMethodInfo GetGenericMethodDefinition() => ResolvedMethod.GetGenericMethodDefinition();
		public override DmdMethodInfo MakeGenericMethod(IList<DmdType> typeArguments) => ResolvedMethod.MakeGenericMethod(typeArguments);
	}
}
