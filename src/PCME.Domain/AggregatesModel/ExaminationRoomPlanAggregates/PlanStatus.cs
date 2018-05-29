using PCME.Domain.Exceptions;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.ExaminationRoomPlanAggregates
{
    public class PlanStatus: Enumeration
    {
        public static PlanStatus Default = new PlanStatus(1, "正常");
        public static PlanStatus SelectStart = new PlanStatus(2, "选场");
        public static PlanStatus SelectFinish = new PlanStatus(3, "选场结束");
        public static PlanStatus SignInStart = new PlanStatus(4, "签到");
        public static PlanStatus Over = new PlanStatus(5, "考试结束");
        public static PlanStatus Close = new PlanStatus(6, "关闭");

        public static IEnumerable<PlanStatus> List() => new[] {Default, SelectStart, SelectFinish,SignInStart,Over,Close };
        public PlanStatus()
        {

        }
        public PlanStatus(int id, string name) :
            base(id, name)
        {

        }

        public static PlanStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCulture));

            if (state == null)
            {
                throw new ExaminationRoomPlanDomainException($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        public static PlanStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new ExaminationRoomPlanDomainException($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
