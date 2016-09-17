﻿using System.Drawing;
using EloBuddy;

namespace EvadePlus.SkillshotTypes.SpecialTypes
{
    public class YasuoQ : LinearMissileSkillshot
    {
        public override EvadeSkillshot NewInstance()
        {
            var newInstance = new YasuoQ() { SpellData = SpellData };
            return newInstance;
        }

        public override void OnSpellDetection(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            _startPos = Caster.ServerPosition;
            _endPos = _startPos.ExtendVector3(CastArgs.End, -SpellData.Range);
        }

        public override void OnDraw()
        {
            if (!IsValid)
            {
                return;
            }

            Utils.Draw3DRect(StartPosition, EndPosition, SpellData.Radius*2, Color.White, 2, false);
        }
    }
}
