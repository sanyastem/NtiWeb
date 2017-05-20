using System;
using System.Collections.Generic;
using System.Linq;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Logging;
using ASUVP.Core.Web.Security;

namespace ASUVP.Online.Services
{
    public interface IInstructionService
    {
        //Instruction GetInstructionDetails(Guid id);
        void DeleteInstructions(Guid[] ids);
        List<InstructionList> GetInstructionByParametrs(Guid companyId, string periodType, string dateBeg, string dateEnd, string stationId, string agreementId, string agrManagerId, string templateId, string statusId, string epStatusId);
        Instruction GetInstruction(Guid id);
    }
    class InstructionService : BaseHttpService, IInstructionService
    {
        private readonly string InstructionGroup = "Instruction";

        public InstructionService(IEventLogger logger) : base(logger)
        {
        }
        //public Instruction GetInstructionDetails(Guid id)
        //{
        //    using (var context = new ProcData())
        //    {
        //        return context.InstructionDetailsGet(id).FirstOrDefault();
        //    }
        //}

        public Instruction GetInstruction(Guid id)
        {
            using (var context = new ProcData())
            {
                return context.InstructionGet(AuthManager.User.CompanyId, id).FirstOrDefault();
            }
        }

        public void DeleteInstructions(Guid[] ids)
        {
            if(ids == null) return;

            foreach (var id in ids)
            {
                //delete instructions
            }
        }

        public List<InstructionList> GetInstructionByParametrs(Guid companyId, string periodType, string dateBeg, string dateEnd, string stationId, string agreementId, string agrManagerId, string templateId, string statusId, string epStatusId)
        {
            var instructions = new List<InstructionList>();
            if (string.IsNullOrEmpty(periodType) && string.IsNullOrEmpty(dateBeg) && string.IsNullOrEmpty(dateEnd) && string.IsNullOrEmpty(stationId) && string.IsNullOrEmpty(agreementId)
                && string.IsNullOrEmpty(agrManagerId) && string.IsNullOrEmpty(templateId) && string.IsNullOrEmpty(statusId) && string.IsNullOrEmpty(epStatusId))
                return instructions;

            using (var context = new ProcData())
            {
                DateTime beginTime = new DateTime(1990, 1, 1);
                DateTime endTime = new DateTime(2050, 1, 1);
                if (!string.IsNullOrEmpty(dateBeg))
                    DateTime.TryParse(dateBeg, out beginTime);
                if (!string.IsNullOrEmpty(dateEnd))
                    DateTime.TryParse(dateEnd, out endTime);

                int period = -1;
                int.TryParse(periodType, out period);

                Guid station;
                Guid.TryParse(stationId, out station);

                Guid agreement;
                Guid.TryParse(agreementId, out agreement);

                Guid agrManager;
                Guid.TryParse(agrManagerId, out agrManager);

                Guid template;
                Guid.TryParse(templateId, out template);

                Guid status;
                Guid.TryParse(statusId, out status);

                Guid epsStatus;
                Guid.TryParse(epStatusId, out epsStatus);

                instructions = context.InstructionListGet(
                    companyId,
                    InstructionGroup,
                    period == -1 ? 1 : period,
                    beginTime,
                    endTime,
                    station == Guid.Empty ? (Guid?)null : station,
                    agreement == Guid.Empty ? (Guid?)null : agreement,
                    agrManager == Guid.Empty ? (Guid?)null : agrManager,
                    template == Guid.Empty ? (Guid?)null : template,
                    status == Guid.Empty ? (Guid?)null : status,
                    epsStatus == Guid.Empty ? (Guid?)null : epsStatus).ToList();
            }

            return instructions;
        }
    }

}
