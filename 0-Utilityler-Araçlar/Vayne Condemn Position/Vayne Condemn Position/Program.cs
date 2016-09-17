using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace Vayne_Condemn_Position
{
    class Program
    {
        public static Spell.Active Q;
        public static Spell.Targeted E;
        private static Menu _menu;

        static void Main(string[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Vayne")
            { return; }

            
            Q = new Spell.Active(SpellSlot.Q, 325);
            E = new Spell.Targeted(SpellSlot.E, 550);

            _menu = MainMenu.AddMenu("Condemn Position", "VCmenu");
            _menu.AddGroupLabel("Vayne Condemn(E) Ayarı");
            _menu.AddSeparator();
            _menu.Add("Usedrawings", new CheckBox("Göstergeyi Kullan"));

            Chat.Print("Vayne Condemn Position Yuklendi", System.Drawing.Color.BlueViolet);

            Drawing.OnDraw += OnDraw;
        }


        public static void OnDraw(EventArgs args)
        {
                    var t = TargetSelector.GetTarget(E.Range + Q.Range, DamageType.Physical);
                    if (t.IsValidTarget() && _menu["Usedrawings"].Cast<CheckBox>().CurrentValue)
                    {
                        var color = System.Drawing.Color.Red;
                        for (var i = 1; i < 8; i++)
                        {
                            var targetBehind = t.Position + Vector3.Normalize(t.ServerPosition - ObjectManager.Player.Position) * i * 50;

                            if (!targetBehind.IsWall())
                            {
                                color = System.Drawing.Color.Aqua;
                            }
                            else
                            {
                                color = System.Drawing.Color.Red;
                            }
                        }

                        var tt = t.Position + Vector3.Normalize(t.ServerPosition - ObjectManager.Player.Position) * 8 * 50;

                        var startpos = t.Position;
                        var endpos = tt;
                        var endpos1 = tt + (startpos - endpos).To2D().Normalized().Rotated(45 * (float)Math.PI / 180).To3D() * t.BoundingRadius;
                        var endpos2 = tt + (startpos - endpos).To2D().Normalized().Rotated(-45 * (float)Math.PI / 180).To3D() * t.BoundingRadius;

                        var width = 2;

                        var x = new Geometry.Polygon.Line(startpos, endpos);
                        {
                            x.Draw(color, width);
                        }

                        var y = new Geometry.Polygon.Line(endpos, endpos1);
                        {
                            y.Draw(color, width);
                        }

                        var z = new Geometry.Polygon.Line(endpos, endpos2);
                        {
                            z.Draw(color, width);
                        }
                    }
                }
            }
        }

