﻿using System;
using System.Linq;
using Generator.Model;

namespace Generator.Renderer.Public;

internal static class ForeignTypedRecord
{
    public static string Render(GirModel.Record record)
    {
        var name = Model.ForeignTypedRecord.GetPublicClassName(record);
        var internalHandleName = Model.ForeignTypedRecord.GetFullyQuallifiedOwnedHandle(record);

        return $@"
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#nullable enable

namespace {Namespace.GetPublicName(record.Namespace)};

// AUTOGENERATED FILE - DO NOT MODIFY

{PlatformSupportAttribute.Render(record as GirModel.PlatformDependent)}
public partial class {name}
{{
    public {internalHandleName} Handle {{ get; }}

    public {name}({internalHandleName} handle)
    {{
        Handle = handle;
        Initialize();
    }}

    //TODO: This is a workaround constructor as long as we are
    //not having https://github.com/gircore/gir.core/issues/397
    private {name}(IntPtr ptr, bool ownsHandle) : this(ownsHandle
        ? new {Model.ForeignTypedRecord.GetFullyQuallifiedOwnedHandle(record)}(ptr)
        : new {Model.ForeignTypedRecord.GetFullyQuallifiedUnownedHandle(record)}(ptr).OwnedCopy()){{ }}

    // Implement this to perform additional steps in the constructor
    partial void Initialize();

    {FunctionRenderer.Render(record.TypeFunction)}
}}";
    }
}
