using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TournamentHostProject
{
    class PlayerPanel : Panel
    {

        private Label _name = new Label();
        private Label _score = new Label();

        public PlayerPanel(int y, string name)
        {
            Location = new System.Drawing.Point(0, y);
            Size = new System.Drawing.Size(484, 39);
            Name = "panelY";

            _name.Text = string.Format("Name: {0}", name);
            _name.AutoSize = true;
            _name.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            _score.Text = "Score: {0}";
            _score.AutoSize = true;
            _score.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            _score.Left = _name.PreferredWidth + _name.Left +10;

            Controls.Add(_name);
            Controls.Add(_score);
        }

    }
}
