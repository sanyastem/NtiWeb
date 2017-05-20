using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASUVP.Online.Web.Tools
{
    public class Status
    {
        public string ImgPath { get; set; }
        public string StatusName { get; set; }
    }

    public static class StatusManager
    {
        public static Status GetApprovalStatus(int statusId)
        {
            switch (statusId)
            {
                case 0:
                    {
                        return new Status() { ImgPath = "/Content/img/status/01_doc.png", StatusName = "Согласовано ЗАО «Русагротранс»; Согласовано Клиентом" };
                    }
                case 1:
                    {
                        return new Status() { ImgPath = "/Content/img/status/02_doc.png", StatusName = "На согласовании ЗАО «Русагротранс»" };
                    }
                case 2:
                    {
                        return new Status() { ImgPath = "/Content/img/status/03_doc.png", StatusName = "Отклонено ЗАО «Русагротранс»" };
                    }
                case 3:
                    {
                        return new Status() { ImgPath = "/Content/img/status/04_doc.png", StatusName = "Отклонено ЗАО «Русагротранс»" };
                    }
                default:
                    {
                        return new Status() { ImgPath = null, StatusName = null };
                    }
            }
        }

        public static Status GetSigningStatus(int statusId)
        {
            switch (statusId)
            {
                case 0:
                    {
                        return new Status() { ImgPath = "/Content/img/status/sign_perf.png", StatusName = "Подписано ЭП Экспедитора" };
                    }
                case 1:
                    {
                        return new Status() { ImgPath = "/Content/img/status/sign_cust.png", StatusName = "Подписано ЭП Клиента" };
                    }                   
                default:
                    {
                        return new Status() { ImgPath = null, StatusName = null };
                    }
            }
        }
    }
}