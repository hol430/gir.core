﻿using Generator.Model;

namespace Generator.Renderer.Public;

internal static class CallbackNotifiedHandler
{
    public static string Render(GirModel.Callback callback)
    {
        var handlerName = Callback.GetNotifiedHandlerName(callback);

        return $@"
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#nullable enable

namespace {Namespace.GetPublicName(callback.Namespace)}
{{
    // AUTOGENERATED FILE - DO NOT MODIFY

    /// <summary>
    /// Notified Handler for {callback.Name}. A notified annotation indicates the closure should
    /// be kept alive until it is manually removed by the user. This removal is indicated by a
    /// destroy_notify event, emitted by the relevant library. Pass <c>DestroyNotify</c> in place of a
    /// destroy_notify callback parameter. 
    /// </summary>
    {PlatformSupportAttribute.Render(callback as GirModel.PlatformDependent)}
    public class {handlerName}
    {{
        public {Namespace.GetInternalName(callback.Namespace)}.{callback.Name}? NativeCallback;
        public GLib.Internal.DestroyNotify? DestroyNotify;

        private {callback.Name}? managedCallback;
        private GCHandle gch;

        public {handlerName}({callback.Name}? managed)
        {{
            DestroyNotify = DestroyCallback;
            managedCallback = managed;
            
            if (managedCallback is null)
            {{
                NativeCallback = null;
                DestroyNotify = null;
            }}
            else
            {{
                gch = GCHandle.Alloc(this);

                {CallbackCommonHandlerRenderUtils.RenderNativeCallback(callback, GirModel.Scope.Notified)}
            }}
        }}

        private void DestroyCallback(IntPtr userData)
        {{
            gch.Free();
        }}
    }}
}}";
    }
}