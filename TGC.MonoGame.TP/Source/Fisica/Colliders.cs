using BepuPhysics;
using System.Collections.Concurrent;
using System.Collections.Generic;
using PistonDerby.Elementos;

namespace PistonDerby.Collisions;

class Colliders
{
    private readonly ConcurrentDictionary<StaticHandle, ElementoEstatico> SColliders = new ConcurrentDictionary<StaticHandle, ElementoEstatico>();
    private readonly ConcurrentDictionary<BodyHandle, ElementoDinamico> DColliders = new ConcurrentDictionary<BodyHandle, ElementoDinamico>();
    private readonly ConcurrentDictionary<BodyHandle, ElementoDinamicoIndependiente> DIColliders = new ConcurrentDictionary<BodyHandle, ElementoDinamicoIndependiente>();

    internal void RegisterCollider(StaticHandle handle, ElementoEstatico handler) => SColliders.TryAdd(handle, handler);
    internal void RegisterCollider(BodyHandle handle, ElementoDinamico handler) => DColliders.TryAdd(handle, handler);
    internal void RegisterCollider(BodyHandle handle, ElementoDinamicoIndependiente handler) => DIColliders.TryAdd(handle, handler);

    internal ElementoEstatico GetHandler(StaticHandle handle) => SColliders.GetValueOrDefault(handle);
    internal ElementoDinamico GetHandler(BodyHandle handle) => DColliders.GetValueOrDefault(handle);

    internal void UnregisterCollider(StaticHandle handle) => SColliders.TryRemove(handle, out _);
    internal void UnregisterCollider(BodyHandle handle) => DColliders.TryRemove(handle, out _);
}
