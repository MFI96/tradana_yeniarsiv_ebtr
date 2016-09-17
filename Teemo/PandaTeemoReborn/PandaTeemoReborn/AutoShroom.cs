using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using EloBuddy;
using SharpDX;

namespace PandaTeemoReborn
{
    internal class AutoShroom
    {
        /// <summary>
        /// Holds all the Shroom Positions that will be used by Auto Shroom.
        /// </summary>
        public static List<Vector3> ShroomPosition = new List<Vector3>();

        /// <summary>
        /// Holds all the Shroom Positions that was assigned by players.
        /// </summary>
        public static List<Vector3> PlayerAssignedShroomPosition = new List<Vector3>();

        /// <summary>
        /// To Avoid Multiple Initialize Calls.
        /// </summary>
        static AutoShroom()
        {
            if (Extensions.MenuValues.AutoShroom.EnableShroom)
            {
                LoadPositions();
                LoadDefaultPositions();
            }
        }

        /// <summary>
        /// Initializes AutoShroom
        /// </summary>
        public static void Initialize()
        {
        }

        /// <summary>
        /// Assigns Player Assigned Positions to the addon
        /// </summary>
        private static void AssignPosition()
        {
            foreach (var pos in PlayerAssignedShroomPosition.Where(pos => !ShroomPosition.Contains(pos)))
            {
                ShroomPosition.Add(pos);
            }
        }

        /// <summary>
        /// Assigns given position to the addon
        /// </summary>
        /// <param name="position"></param>
        private static void AssignPosition(Vector3 position)
        {
            if (ShroomPosition.Contains(position))
            {
                return;
            }
            ShroomPosition.Add(position);
        }

        /// <summary>
        /// Loads in Positions saved in addon
        /// </summary>
        private static void LoadDefaultPositions()
        {
            if (!Extensions.MenuValues.AutoShroom.UseDefaultPosition)
            {
                return;
            }

            if (Game.MapId == GameMapId.SummonersRift)
            {
                ShroomPosition.Add(new Vector3(1166.724f, 12297.83f, 52.83826f));
                ShroomPosition.Add(new Vector3(1667.684f, 12996.45f, 52.83789f));
                ShroomPosition.Add(new Vector3(2415.056f, 13507.24f, 52.83826f));
                ShroomPosition.Add(new Vector3(2969.118f, 11033.12f, -70.2428f));
                ShroomPosition.Add(new Vector3(4431.33f, 11799.48f, 56.75671f));
                ShroomPosition.Add(new Vector3(3412.402f, 7774.899f, 53.09167f));
                ShroomPosition.Add(new Vector3(5033.338f, 8491.592f, -17.38232f));
                ShroomPosition.Add(new Vector3(6536.028f, 8328.28f, -71.15137f));
                ShroomPosition.Add(new Vector3(8409.39f, 6482.129f, -71.2406f));
                ShroomPosition.Add(new Vector3(9425.655f, 5660.457f, -71.24036f));
                ShroomPosition.Add(new Vector3(10177.76f, 4826.566f, -71.24072f));
                ShroomPosition.Add(new Vector3(11850.96f, 3937.114f, -67.8418f));
                ShroomPosition.Add(new Vector3(13359.2f, 2402.382f, 51.36682f));
                ShroomPosition.Add(new Vector3(12493.16f, 1482.778f, 53.56189f));
                ShroomPosition.Add(new Vector3(10375.6f, 3080.512f, 49.72595f));
                ShroomPosition.Add(new Vector3(8081.267f, 3487.414f, 51.55066f));
                ShroomPosition.Add(new Vector3(6534.422f, 4689.892f, 48.5271f));
                ShroomPosition.Add(new Vector3(8610.946f, 4743.022f, 52.1106f));
                ShroomPosition.Add(new Vector3(5582.829f, 3506.9f, 51.42273f));
                ShroomPosition.Add(new Vector3(2323.918f, 9719.254f, 53.62268f));
                ShroomPosition.Add(new Vector3(3620.55f, 9329.173f, 2.75769f));
                ShroomPosition.Add(new Vector3(4695.097f, 10044.96f, -71.23145f));
                ShroomPosition.Add(new Vector3(5197.724f, 9138.628f, -71.2406f));
                ShroomPosition.Add(new Vector3(6239.813f, 10285.85f, 54.07532f));
                ShroomPosition.Add(new Vector3(6995.702f, 11379.84f, 54.58472f));
                ShroomPosition.Add(new Vector3(8288.187f, 10243.64f, 50.11487f));
                ShroomPosition.Add(new Vector3(5684.987f, 12746.28f, 52.83813f));
                ShroomPosition.Add(new Vector3(7164.933f, 14115.25f, 52.83826f));
                ShroomPosition.Add(new Vector3(9235.373f, 11457.73f, 53.27258f));
                ShroomPosition.Add(new Vector3(7946.956f, 11830.69f, 56.47693f));
                ShroomPosition.Add(new Vector3(9989.992f, 7939.03f, 51.65698f));
                ShroomPosition.Add(new Vector3(11522.2f, 7136.796f, 51.72546f));
                ShroomPosition.Add(new Vector3(14085.62f, 6998.261f, 52.30627f));
                ShroomPosition.Add(new Vector3(12516.67f, 5237.725f, 51.72925f));
                ShroomPosition.Add(new Vector3(6864.854f, 3085.046f, 51.80273f));
            }
        }

        /// <summary>
        /// Loads in positions saved on file
        /// </summary>
        private static void LoadPositions()
        {
            var sandboxConfig = EloBuddy.Sandbox.SandboxConfig.DataDirectory + @"\PandaTeemoReborn\";
            var xFile = sandboxConfig + Game.MapId + @"\" + "xFile" + ".txt";
            var yFile = sandboxConfig + Game.MapId + @"\" + "yFile" + ".txt";
            var zFile = sandboxConfig + Game.MapId + @"\" + "zFile" + ".txt";

            if (!File.Exists(xFile) || !File.Exists(yFile) || !File.Exists(zFile))
            {
                SavePositions();
            }

            string[] xPositions = null;
            string[] yPositions = null;
            string[] zPositions = null;

            if (File.Exists(xFile) && File.Exists(yFile) && File.Exists(zFile))
            {
                xPositions = File.ReadAllLines(xFile);
                yPositions = File.ReadAllLines(yFile);
                zPositions = File.ReadAllLines(zFile);
            }

            if (xPositions == null)
            {
                return;
            }

            var xFloat = new float[xPositions.Length];
            var yFloat = new float[yPositions.Length];
            var zFloat = new float[zPositions.Length];

            for (var i = 0; i < xPositions.Length; i++)
            {
                var x = xPositions[i];
                float.TryParse(x, out xFloat[i]);
            }

            for (var i = 0; i < yPositions.Length; i++)
            {
                var y = yPositions[i];
                float.TryParse(y, out yFloat[i]);
            }

            for (var i = 0; i < zPositions.Length; i++)
            {
                var z = zPositions[i];
                float.TryParse(z, out zFloat[i]);
            }

            var positions = new Vector3[xFloat.Length];

            for (var i = 0; i < positions.Length; i++)
            {
                positions[i] = new Vector3(xFloat[i], yFloat[i], zFloat[i]);
            }

            AddShroomLocations(positions.ToList());
            AssignPosition();
        }

        /// <summary>
        /// Adds a position to Player Assigned List
        /// </summary>
        /// <param name="position"></param>
        public static void AddShroomLocation(Vector3 position)
        {
            PlayerAssignedShroomPosition.Add(position);
            AssignPosition(position);
        }

        /// <summary>
        /// Adds positions given a list.
        /// </summary>
        /// <param name="positions"></param>
        public static void AddShroomLocations(List<Vector3> positions)
        {
            positions.ForEach(p => PlayerAssignedShroomPosition.Add(p));
            positions.ForEach(p => ShroomPosition.Add(p));
        }

        /// <summary>
        /// Removes a position from Player Assigned List
        /// </summary>
        /// <param name="position"></param>
        public static void RemoveShroomLocation(Vector3 position)
        {
            PlayerAssignedShroomPosition.Remove(position);
            ShroomPosition.Remove(position);
        }

        /// <summary>
        /// Removes positions given a list.
        /// </summary>
        /// <param name="positions"></param>
        public static void RemoveShroomLocations(List<Vector3> positions)
        {
            positions.ForEach(p => PlayerAssignedShroomPosition.Remove(p));
            positions.ForEach(p => ShroomPosition.Remove(p));
        }

        /// <summary>
        /// Saves all Player Assigned positions to a file.
        /// </summary>
        public static void SavePositions()
        {
            Chat.Print("PandaTeemo | Saving Positions", System.Drawing.Color.Red);

            var sandboxConfig = EloBuddy.Sandbox.SandboxConfig.DataDirectory + @"\PandaTeemoReborn\";
            var xFile = sandboxConfig + Game.MapId + @"\" + "xFile" + ".txt";
            var yFile = sandboxConfig + Game.MapId + @"\" + "yFile" + ".txt";
            var zFile = sandboxConfig + Game.MapId + @"\" + "zFile" + ".txt";

            if (!Directory.Exists(sandboxConfig))
            {
                Directory.CreateDirectory(sandboxConfig);
                Directory.CreateDirectory(sandboxConfig + GameMapId.SummonersRift);
                Directory.CreateDirectory(sandboxConfig + GameMapId.CrystalScar);
                Directory.CreateDirectory(sandboxConfig + GameMapId.HowlingAbyss);
                Directory.CreateDirectory(sandboxConfig + GameMapId.TwistedTreeline);
            }

            if (Directory.Exists(sandboxConfig) && !Directory.Exists(sandboxConfig + Game.MapId))
            {
                Directory.CreateDirectory(sandboxConfig + Game.MapId);
            }

            var backupDir = sandboxConfig + Game.MapId + @"\Backup\";

            if (!Directory.Exists(backupDir))
            {
                Directory.CreateDirectory(backupDir);
            }

            if (File.Exists(xFile) && File.Exists(yFile) && File.Exists(zFile))
            {
                var xBackup = backupDir + "xFile" + ".txt";
                var yBackup = backupDir + "yFile" + ".txt";
                var zBackup = backupDir + "zFile" + ".txt";

                if (File.Exists(xBackup))
                {
                    File.Delete(xBackup);
                }
                if (File.Exists(yBackup))
                {
                    File.Delete(yBackup);
                }
                if (File.Exists(zBackup))
                {
                    File.Delete(zBackup);
                }

                File.Move(xFile, xBackup);
                File.Move(yFile, yBackup);
                File.Move(zFile, zBackup);
            }

            foreach (var p in PlayerAssignedShroomPosition)
            {
                var x = p.X.ToString(CultureInfo.InvariantCulture);
                var y = p.Y.ToString(CultureInfo.InvariantCulture);
                var z = p.Z.ToString(CultureInfo.InvariantCulture);

                var xStreamWriter = File.AppendText(xFile);
                var yStreamWriter = File.AppendText(yFile);
                var zStreamWriter = File.AppendText(zFile);

                xStreamWriter.WriteLine(x);
                yStreamWriter.WriteLine(y);
                zStreamWriter.WriteLine(z);

                xStreamWriter.Dispose();
                yStreamWriter.Dispose();
                zStreamWriter.Dispose();
            }

            Chat.Print("PandaTeemo | Positions Saved!", System.Drawing.Color.LawnGreen);
        }
    }
}