using EloBuddy;

namespace DashSpells
{ 

    public class SpellData
    {
        public Champion champ;
        public string champname;
        public SpellSlot spellKey;
        public bool enabled = false;
        public string spellname = string.Empty;

        public SpellData()
        {

        }

        public SpellData(
            Champion champ,
            string champname,
            SpellSlot spellKey,
            bool enabled,
            string spellname
            )
        {
            this.champ = champ;
            this.champname = champname;
            this.spellKey = spellKey;
            this.enabled = enabled;
            this.spellname = spellname;
        }
    }
}