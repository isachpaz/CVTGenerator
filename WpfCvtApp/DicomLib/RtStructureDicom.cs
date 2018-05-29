using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Dicom;
using DicomLib.RtData;

namespace DicomLib
{
    public class RtStructureDicom
    {
        public RtStructureDicom(StructureSet ssObject)
        {
            this.StructureSet = ssObject;
        }

        public StructureSet StructureSet { get; }
        public static RtStructureDicom ImportFromFile(string sDicomFile)
        {
            return RtStructureDicom.ReadFile(sDicomFile);
        }

        private static RtStructureDicom ReadFile(string sDicomFile)
        {
            DicomFile df = DicomFile.Open(sDicomFile);
            var dataset = df.Dataset;

            var modality = dataset.Get<string>(DicomTag.Modality);
            if (!modality.ToUpper().Contains("RTSTR"))
            {
                throw new ArgumentException("No RT Structure file provided.");
            }

            string ssName = String.Empty;
            try
            {
                ssName = dataset.Get<string>(DicomTag.StructureSetName);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }

            var ssObject = new StructureSet(ssName);

            RtStructureDicom rtStructureDicom = new RtStructureDicom(ssObject);

            var ssFrameOfReferenceSequence = dataset.Get<DicomSequence>(DicomTag.ReferencedFrameOfReferenceSequence);
            foreach (DicomDataset item in ssFrameOfReferenceSequence.Items)
            {
                try
                {
                    var id = item.Get<string>(DicomTag.FrameOfReferenceUID);
                    ssObject.AddFrameOfReferenceUid(id);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }
            }

            var structureSetRoiSequence = dataset.Get<DicomSequence>(DicomTag.StructureSetROISequence);
            foreach (DicomDataset item in structureSetRoiSequence.Items)
            {
                var name = item.Get<string>(DicomTag.ROIName);
                var roiId = item.Get<int>(DicomTag.ROINumber);
                //Trace.WriteLine(item.Get<string>(DicomTag.ROIName));
                Structure structure = new Structure(name, roiId);
                ssObject.Add(structure);
            }

            var roiContourSequence = dataset.Get<DicomSequence>(DicomTag.ROIContourSequence);
            foreach (DicomDataset item in roiContourSequence.Items)
            {
                try
                {
                    var roiColor = item.Get<int[]>(DicomTag.ROIDisplayColor);
                    var contourSequence = item.Get<DicomSequence>(DicomTag.ContourSequence);
                    var roiId = item.Get<int>(DicomTag.ReferencedROINumber);
                    Structure structure = ssObject.GetStructureById(roiId);
                    structure.Color = Color.FromRgb((byte)roiColor[0], (byte)roiColor[1], (byte)roiColor[2]);
                    foreach (DicomDataset contourItem in contourSequence.Items)
                    {
                        Contour contourOnAPlane = new Contour();
                        int numerOfContourPoints = contourItem.Get<int>(DicomTag.NumberOfContourPoints);
                        var contourData = contourItem.Get<float[]>(DicomTag.ContourData);
                        Debug.Assert(contourData.Length == numerOfContourPoints * 3);

                        contourOnAPlane.SetZPosition(contourData[2]);
                        for (int i = 0; i < contourData.Length; i = i + 3)
                        {
                            float x = contourData[i];
                            float y = contourData[i + 1];
                            float z = contourData[i + 2];

                            contourOnAPlane.AddPoint(x, y, z);
                        }
                        structure.AddContour(contourOnAPlane);
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }

            }
            return rtStructureDicom;
        }

    }
}
