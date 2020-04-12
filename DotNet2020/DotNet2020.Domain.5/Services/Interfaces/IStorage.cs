using DotNet2020.Domain._5.Entities;
using System.Linq;

namespace DotNet2020.Domain._5.Services.Interfaces
{
    public interface IStorage
    {
        /// <summary>
        /// Save report
        /// </summary>
        /// <param name="report">Report to be saved</param>
        RequestResult SaveReport(Report report);

        /// <summary>
        /// Get report by report id
        /// </summary>
        /// <param name="reportId">Report id</param>
        RequestResult<Report> GetReport(int reportId);

        /// <summary>
        /// Get report by report name
        /// </summary>
        /// <param name="reportName">Report name</param>
        RequestResult<Report> GetReport(string reportName);

        /// <summary>
        /// Remove report by report id
        /// </summary>
        /// <param name="reportId">Report id</param>
        RequestResult RemoveReport(int reportId);

        /// <summary>
        /// Remove report by report name
        /// </summary>
        /// <param name="reportName">Report name</param>
        RequestResult RemoveReport(string reportName);

        /// <summary>
        /// Edit report
        /// </summary>
        /// <param name="oldReportId">Old report id</param>
        /// <param name="newReport">New report</param>
        RequestResult<Report> EditReport(int oldReportId, Report newReport);

        /// <summary>
        /// Get all reports
        /// </summary>
        RequestResult<IQueryable<Report>> GetAllReports();
    }
}
