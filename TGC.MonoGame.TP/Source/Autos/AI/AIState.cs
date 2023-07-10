
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PistonDerby.Autos.AI;
abstract class AIState
{
    internal float ActualSeconds;
    internal void ActualTime(float dTime) => ActualSeconds = dTime;

    internal abstract List<Keys> movimiento(Vector3 objetivo, AutoAI autoAI);
}