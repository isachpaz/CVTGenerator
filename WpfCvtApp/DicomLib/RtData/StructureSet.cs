using System.Collections.Generic;

namespace DicomLib.RtData
{
    public class StructureSet
    {
        private List<Structure> _structureCollection;
        private List<string> _frameOfReferenceUids;
        public string Name { get; }

        public IEnumerable<string> FrameOfReferenceUids => _frameOfReferenceUids;
        public IEnumerable<Structure> StructureCollection => _structureCollection;

        public StructureSet(string name)
        {
            Name = name;
            _frameOfReferenceUids = new List<string>();
            _structureCollection = new List<Structure>();
        }

        public void Add(Structure structure)
        {
            _structureCollection.Add(structure);
        }

        public void AddFrameOfReferenceUid(string id)
        {
            this._frameOfReferenceUids.Add(id);
        }

        public Structure GetStructureById(int roiId)
        {
            return _structureCollection.Find(s => s.RoiNumber == roiId);
        }
    }
}