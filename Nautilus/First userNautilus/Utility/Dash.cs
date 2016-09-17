using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace Nautilus.Utility
{
    public static class Dash
    {
        #region Static Fields

        /// <summary>
        ///     DetectedDashes list.
        /// </summary>
        private static readonly Dictionary<int, DashArgs> DetectedDashes = new Dictionary<int, DashArgs>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="Dash" /> class.
        /// </summary>
        static Dash()
        {
            Obj_AI_Base.OnNewPath += Obj_AI_Base_OnNewPath;
        }

        private static void Obj_AI_Base_OnNewPath(Obj_AI_Base kappa, GameObjectNewPathEventArgs args)
        {
            if(args.IsDash && kappa is AIHeroClient) {
            EloBuddy.SDK.Events.Dash.OnDash += Dash_OnDash;
            }
        }

        private static void Dash_OnDash(Obj_AI_Base sender, EloBuddy.SDK.Events.Dash.DashEventArgs e)
        {
            var hero = sender as AIHeroClient;
            if (hero != null && hero.IsValid)
            {
                if (!DetectedDashes.ContainsKey(hero.NetworkId))
                {
                    DetectedDashes.Add(hero.NetworkId, new DashArgs());
                }
                var path = new List<Vector2> { hero.ServerPosition.ToVector2() };
                if(e.Path != null) {
                path.AddRange(e.Path.ToList());
                }
                DetectedDashes[hero.NetworkId] = new DashArgs
                {
                    StartTick = (int)((Game.Time * 1000) - (Game.Ping / 2)),
                    Speed = e.Speed,
                    StartPos = hero.ServerPosition.To2D(),
                    Unit = sender,
                    Path = path,
                    EndPos = DetectedDashes[hero.NetworkId].Path.Last(),
                    EndTick =
                                                             DetectedDashes[hero.NetworkId].StartTick
                                                             + (int)
                                                               (1000
                                                                * (DetectedDashes[hero.NetworkId].EndPos.Distance(
                                                                    DetectedDashes[hero.NetworkId].StartPos)
                                                                   / DetectedDashes[hero.NetworkId].Speed)),
                    Duration =
                                                             DetectedDashes[hero.NetworkId].EndTick
                                                             - DetectedDashes[hero.NetworkId].StartTick
                };

            }
        }

        #endregion

        #region Delegates

        /// <summary>
        ///     OnDash Delegate.
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="e">Dash Arguments Container</param>
        public delegate void OnDashDelegate(object sender, DashArgs e);

        #endregion

        #region Public Events

        /// <summary>
        ///     OnDash Event.
        /// </summary>
        public static event OnDashDelegate OnDash;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the speed of the dashing unit if it is dashing.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="DashArgs" />.
        /// </returns>
        public static DashArgs GetDashInfo(this Obj_AI_Base unit)
        {
            DashArgs value;
            return DetectedDashes.TryGetValue(unit.NetworkId, out value) ? value : new DashArgs();
        }

        /// <summary>
        ///     Returns true if the unit is dashing.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool UsingDash(this Obj_AI_Base unit)
        {
            DashArgs value;
            if (DetectedDashes.TryGetValue(unit.NetworkId, out value) && unit.Path.Length != 0)
            {
                return value.EndTick != 0;
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     New Path subscribed event function.
        /// </summary>
        /// <param name="sender"><see cref="Obj_AI_Base" /> sender</param>
        /// <param name="args">New Path event data</param>

        #endregion

        /// <summary>
        ///     Dash event data.
        /// </summary>
        public class DashArgs : EventArgs
        {
            #region Public Properties

            /// <summary>
            ///     Gets or sets the dash duration.
            /// </summary>
            public int Duration { get; set; }

            /// <summary>
            ///     Gets or sets the end position.
            /// </summary>
            public Vector2 EndPos { get; set; }

            /// <summary>
            ///     Gets or sets the end tick.
            /// </summary>
            public int EndTick { get; set; }

            /// <summary>
            ///     Gets or sets a value indicating whether is blink.
            /// </summary>
            public bool IsBlink { get; set; }

            /// <summary>
            ///     Gets or sets the path.
            /// </summary>
            public List<Vector2> Path { get; set; }

            /// <summary>
            ///     Gets or sets the speed.
            /// </summary>
            public float Speed { get; set; }

            /// <summary>
            ///     Gets or sets the start position.
            /// </summary>
            public Vector2 StartPos { get; set; }

            /// <summary>
            ///     Gets or sets the start tick.
            /// </summary>
            public int StartTick { get; set; }

            /// <summary>
            ///     Gets or sets the unit.
            /// </summary>
            public Obj_AI_Base Unit { get; set; }

            #endregion
        }
    }
}