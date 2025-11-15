using System;
using Godot;

namespace Nordtap.GameManager.Extension;

public static class NodeExtension
{
    public static IServiceProvider GetServiceProvider(this Node node)
    {
        return ServiceProviderManager.GetServiceProvider();
    }
}