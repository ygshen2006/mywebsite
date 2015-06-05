﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.MainBoundedContext.Reports.Aggregates;
using Infrastructor.MainBoundedContext.UnitWorks;
using Infrastructor.SeedWork;

namespace Infrastructor.MainBoundedContext.Repositories.Reports
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        public ReportRepository() : base(new MainDBUnitWorkContext()) { }
        public ReportRepository(MainDBUnitWorkContext context)
            : base(context)
        {

        }


        public void AddReport(Domain.MainBoundedContext.Reports.Aggregates.Report r)
        {

        }

        public void UpdateReport(Domain.MainBoundedContext.Reports.Aggregates.Report r)
        {
            throw new NotImplementedException();
        }

        public void RemoveReport(Domain.MainBoundedContext.Reports.Aggregates.Report r)
        {
            throw new NotImplementedException();
        }
    }
}
