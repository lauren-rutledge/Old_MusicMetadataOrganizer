using System;
using System.Collections.Generic;

namespace MusicMetadataOrganizer
{
    public class UpdateHelper
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

        public void RemoveProperty(string property)
        {
            if (!_propsToChange.Contains(property))
                throw new ArgumentException();
            for (int i = 0; i < _propsToChange.Count; i++)
            {
                if (_propsToChange[i] == property)
                {
                    _propsToChange.RemoveAt(i);
                    _oldVals.RemoveAt(i);
                    _newVals.RemoveAt(i);
                    break;
                }
            }
        }

        public void UpdateMasterFile()
        {
            for (int i = 0; i < _propsToChange.Count; i++)
            {
                _file.TagLibProps[_propsToChange[i]] = _newVals[i];
            }
        }
    }
}
