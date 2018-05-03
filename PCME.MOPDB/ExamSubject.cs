using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class ExamSubject
    {
        public ExamSubject()
        {
            ExamStationRoom = new HashSet<ExamStationRoom>();
            TrainApply = new HashSet<TrainApply>();
            TrainStationBelong = new HashSet<TrainStationBelong>();
        }

        public string SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int TypeId { get; set; }
        public int? CreditHour { get; set; }
        public DateTime? TrainDate { get; set; }
        public DateTime? ApplyStartDate { get; set; }
        public DateTime? ApplyEndDate { get; set; }
        public string ClassId { get; set; }
        public int? State { get; set; }
        public decimal? ApplyCost { get; set; }
        public decimal? ExamCost { get; set; }
        public string TrainStationId { get; set; }
        public DateTime? Ksdate { get; set; }
        public string Pxsj { get; set; }
        public string Kssj { get; set; }
        public int Ksokline { get; set; }
        public int? Kstype { get; set; }
        public int Yxline { get; set; }
        public int Pxtype { get; set; }
        public int Mscount { get; set; }

        public ICollection<ExamStationRoom> ExamStationRoom { get; set; }
        public ICollection<TrainApply> TrainApply { get; set; }
        public ICollection<TrainStationBelong> TrainStationBelong { get; set; }
    }
}
