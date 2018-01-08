using MusicMetadataOrganizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetadataUpdaterGUI
{
    internal class UpdateHelper
    {
        private MasterFile _file;
        public MasterFile File { get { return _file; } }
        private List<string> _propsToChange;
        public List<string> PropsToChange { get { return _propsToChange; } }
        private List<string> _oldVals;
        public List<string> OldVals { get { return _oldVals; } }
        private List<string> _newVals;
        public List<string> NewVals { get { return _newVals; } }

        public UpdateHelper(MasterFile file, List<string> propsToChange, List<string> oldVals, List<string> newVals)
        {
            _file = file;
            _propsToChange = propsToChange;
            _oldVals = oldVals;
            _newVals = newVals;
        }
    }
}
