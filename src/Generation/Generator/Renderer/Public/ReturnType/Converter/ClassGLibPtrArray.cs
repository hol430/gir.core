using Generator.Model;

namespace Generator.Renderer.Public.ReturnType;

internal class ClassGLibPtrArray : ReturnTypeConverter
{
    public RenderableReturnType Create(GirModel.ReturnType returnType)
    {
        var nullableTypeName = ComplexType.GetFullyQualified((GirModel.Class) returnType.AnyType.AsT0) + Nullable.Render(returnType);

        return new RenderableReturnType("GLib.PtrArray");
    }

    public bool Supports(GirModel.ReturnType returnType)
        => returnType.AnyType.IsGLibPtrArray<GirModel.Class>();
}
