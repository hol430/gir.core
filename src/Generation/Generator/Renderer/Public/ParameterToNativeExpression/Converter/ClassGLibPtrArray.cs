using System;
using System.Collections.Generic;
using System.Text;
using Generator.Model;

namespace Generator.Renderer.Public.ParameterToNativeExpressions;

internal class ClassGLibPtrArray : ToNativeParameterConverter
{
    public bool Supports(GirModel.AnyType type)
        => type.IsGLibPtrArray<GirModel.Class>();

    public void Initialize(ParameterToNativeData parameter, IEnumerable<ParameterToNativeData> _)
    {
        var arrayType = parameter.Parameter.AnyTypeOrVarArgs.AsT0.AsT1;

        if (arrayType.IsPointer)
            throw new NotImplementedException($"{parameter.Parameter.AnyTypeOrVarArgs}: Pointed class array can not yet be converted to native.");

        var parameterName = Model.Parameter.GetName(parameter.Parameter);
        var nativeVariableName = parameterName + "Native";

		StringBuilder expression = new StringBuilder();
		if (Transfer.IsOwnedRef(parameter.Parameter.Transfer))
		{
			string addRefExpression = parameter.Parameter.Nullable
				? $"{parameterName}?.Ref();"
				: $"{parameterName}.Ref();";
			expression.AppendLine(addRefExpression);
		}
		expression.AppendLine($"var {nativeVariableName} = {parameterName}.Handle.DangerousGetHandle();");

        parameter.SetSignatureName(parameterName);
        parameter.SetCallName(nativeVariableName);
        parameter.SetExpression(expression.ToString());
    }
}
