using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.Utils;

namespace Services
{
    public class DeleteService
    {
        public void DeleteAllInfoForService(int serviceId, IUnitOfWork UoW)
        {
            var service = UoW.GetRepository<IServiceReadRepository>().GetById(serviceId);

            DeleteImages(serviceId, UoW);
            DeleteReports(serviceId, UoW);
            DeleteMarks(serviceId,UoW);
            DeletePromoActions(serviceId, UoW);
            DeleteReservations(serviceId, UoW);
            DeleteSubscriptions(serviceId, UoW);
            DeleteIssues(serviceId, UoW);


            UoW.GetRepository<IServiceWriteRepository>().Delete(service);
        }

        private void DeleteIssues(int serviceId, IUnitOfWork UoW)
        {
            var issuesFromRegularUsers = UoW.GetRepository<IIssueReadRepository>()
                .GetAll()
                .Where(x => x.UserId.In(UoW.GetRepository<IUserReadRepository>()
                    .GetAll()
                    .Where(y => y.UserType == UserType.Registered)
                    .Select(y => y.UserId)
                    .ToArray()));

            var issuesToDelete = issuesFromRegularUsers.Where(x => x.IssuedEntityId == serviceId);
            foreach (var issue in issuesToDelete)
            {
                UoW.GetRepository<IIssueWriteRepository>().Delete(issue);
            }
        }

        private void DeleteSubscriptions(int serviceId, IUnitOfWork UoW)
        {
            var subscriptions = UoW.GetRepository<ISubscriptionReadRepository>().GetAll()
                .Where(x => x.ServiceId == serviceId);
            foreach (var sub in subscriptions)
            {
                UoW.GetRepository<ISubscriptionWriteRepository>().Delete(sub);
            }
        }

        private void DeleteReservations(int serviceId, IUnitOfWork UoW)
        {
            var reservations = UoW.GetRepository<IReservationReadRepository>().GetAll()
                .Where(x => x.ServiceId == serviceId);
            foreach (var reservation in reservations)
            {
                UoW.GetRepository<IReservationWriteRepository>().Delete(reservation);
            }
        }

        private void DeleteImages(int serviceId, IUnitOfWork UoW)
        {
            var images = UoW.GetRepository<IImageReadRepository>().GetAll().Where(x => x.ServiceId == serviceId);
            foreach (var image in images)
            {
                UoW.GetRepository<IImageWriteRepository>().Delete(image);
            }
        }

        private void DeletePromoActions(int serviceId, IUnitOfWork UoW)
        {
            var promoActions = UoW.GetRepository<IPromoActionReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == serviceId);
            foreach (var promoAction in promoActions)
            {
                UoW.GetRepository<IPromoActionWriteRepository>().Delete(promoAction);
            }
        }

        private void DeleteMarks(int serviceId, IUnitOfWork UoW)
        {
            var marks = UoW.GetRepository<IMarkReadRepository>().GetAll().Where(x => x.ServiceId == serviceId);
            foreach (var mark in marks)
            {
                UoW.GetRepository<IMarkWriteRepository>().Delete(mark);
            }
        }

        private void DeleteReports(int serviceId, IUnitOfWork UoW)
        {
            var reports = UoW.GetRepository<IReportReadRepository>().GetAll()
                .Where(x => x.ReportId.In(UoW.GetRepository<IReservationReadRepository>()
                    .GetAll()
                    .Where(y => y.ReportId != null && y.ServiceId == serviceId)
                    .Select(y => y.ReportId.Value)
                    .ToArray()));
            foreach (var report in reports)
            {
                UoW.GetRepository<IReportWriteRepository>().Delete(report);
            }
        }
    }
}
