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
using dnSpy.Contracts.Debugger.DotNet.Evaluation;
using dnSpy.Contracts.Debugger.DotNet.Evaluation.ValueNodes;
using dnSpy.Contracts.Debugger.DotNet.Text;
using dnSpy.Contracts.Debugger.Evaluation;
using dnSpy.Debugger.DotNet.Metadata;

namespace dnSpy.Roslyn.Shared.Debugger.ValueNodes {
	abstract class LanguageValueNodeFactory : DbgDotNetValueNodeFactory {
		public sealed override DbgDotNetValueNode Create(DbgEvaluationContext context, DbgDotNetText name, DbgDotNetValue value, string expression, string imageName, bool isReadOnly, bool causesSideEffects, DmdType expectedType) =>
			new DbgDotNetValueNodeImpl(name, value, expression, imageName, isReadOnly, causesSideEffects, expectedType, null);

		public sealed override DbgDotNetValueNode CreateException(DbgEvaluationContext context, uint id, DbgDotNetValue value) {
			throw new NotImplementedException();//TODO:
		}

		public sealed override DbgDotNetValueNode CreateStowedException(DbgEvaluationContext context, uint id, DbgDotNetValue value) {
			throw new NotImplementedException();//TODO:
		}

		public sealed override DbgDotNetValueNode CreateReturnValue(DbgEvaluationContext context, uint id, DbgDotNetValue value, DmdMethodBase method) {
			throw new NotImplementedException();//TODO:
		}

		public sealed override DbgDotNetValueNode CreateError(DbgEvaluationContext context, DbgDotNetText name, string errorMessage, string expression) =>
			new DbgDotNetValueNodeImpl(name, null, expression, PredefinedDbgValueNodeImageNames.Error, true, false, null, errorMessage);

		public sealed override DbgDotNetValueNode CreateTypeVariables(DbgEvaluationContext context, DbgDotNetText name, DbgDotNetTypeVariableInfo[] typeVariableInfos) {
			throw new NotImplementedException();//TODO:
		}
	}
}
