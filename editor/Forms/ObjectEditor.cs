﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using sth1edwv.GameObjects;

namespace sth1edwv.Forms
{
    public sealed partial class ObjectEditor : Form
    {
        public ObjectEditor(LevelObject levelObject)
        {
            InitializeComponent();
            Font = SystemFonts.MessageBoxFont;

            comboBoxNames.Items.AddRange(LevelObject.Names.Values.ToArray<object>());
            if (LevelObject.Names.TryGetValue(levelObject.Type, out var name))
            {
                comboBoxNames.SelectedItem = name;
            }

            textBoxX.Text = levelObject.X.ToString();
            textBoxY.Text = levelObject.Y.ToString();
            textBoxType.Text = levelObject.Type.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var n = comboBoxNames.SelectedIndex;
            if (n == -1)
                return;
            textBoxType.Text = LevelObject.Names.Keys.ToArray()[n].ToString();
        }
    }
}
