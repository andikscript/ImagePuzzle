using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePuzzle
{
    internal class ObjectDesign
    {
        public string labelTimeStart;
        public string labelTimeEnd;
        public string labelMovesMade;
        public string buttonPaused;
        public string buttonResume;

        public ObjectDesign(string labelTimeStart, string labelTimeEnd, string labelMovesMade, string buttonPaused,
            string buttonResume)
        {
            this.labelTimeStart = labelTimeStart;
            this.labelTimeEnd = labelTimeEnd;
            this.labelMovesMade = labelMovesMade;
            this.buttonPaused = buttonPaused;
            this.buttonResume = buttonResume;
        }
    }
}
