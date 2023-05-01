﻿namespace Generator.Renderer.Public.Parameter;

internal class ClassGLibPtrArray : ParameterConverter
{
    public bool Supports(GirModel.AnyType anyType)
    {
        return anyType.IsGLibPtrArray<GirModel.Class>();
    }

    public ParameterTypeData Create(GirModel.Parameter parameter)
    {
        return new ParameterTypeData(
            Direction: GetDirection(parameter),
            NullableTypeName: GetNullableTypeName(parameter)
        );
    }

    private static string GetNullableTypeName(GirModel.Parameter parameter)
    {
        return "GLib.PtrArray";
        // var cls = (GirModel.PointerArrayType)parameter.AnyTypeOrVarArgs.AsT0.AsT1;
        // return Model.ComplexType.GetFullyQualified(cls);
    }

    private static string GetDirection(GirModel.Parameter parameter) => parameter switch
    {
        { Direction: GirModel.Direction.InOut } => ParameterDirection.Ref(),
        { Direction: GirModel.Direction.Out, CallerAllocates: true } => ParameterDirection.Ref(),
        { Direction: GirModel.Direction.Out } => ParameterDirection.Out(),
        _ => ParameterDirection.In()
    };
}
