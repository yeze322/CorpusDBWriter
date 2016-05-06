using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace DataNormalizer.DataEntity
{
    internal enum IMap
    {
        Id=0,IncidentId,ProductName,ClassificationPath,
        UnitOfWork,CreateDate,CreatedBy,ImportDate,
        WorkGroup,SupportOffering,ProblemType,FormFactor,
        CompanyName,IncidentStatus,ModifiedBy,ModifiedDate,
        ReviewerComments,IncidentTitle,ProdClassType,MsSolveSrNumber,
        NumberOfViews,NumberOfVotes,NumberOfMessages,NumberOfUniqueUsers,
        CaseNotes,Match,toJson,generateJsonTemplate,
    }
    public class Incident : IDataEntity
    {
        #region dataMembers
        public int Id { get; set; }
        public string IncidentId { get; set; }
        public string ProductName { get; set; }
        public string ClassificationPath { get; set; }
        public int UnitOfWork { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ImportDate { get; set; }
        public string WorkGroup { get; set; }
        public string SupportOffering { get; set; }
        public string ProblemType { get; set; }
        public string FormFactor { get; set; }
        public string CompanyName { get; set; }
        public string IncidentStatus { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ReviewerComments { get; set; }
        public string IncidentTitle { get; set; }
        public string ProdClassType { get; set; }
        public int MsSolveSrNumber { get; set; }
        public int NumberOfViews { get; set; }
        public int NumberOfVotes { get; set; }
        public int NumberOfMessages { get; set; }
        public int NumberOfUniqueUsers { get; set; }
        public string CaseNotes { get; set; }
        #endregion

        public Incident(Match match)
        {
            this.IncidentId = match.Groups[(int)IMap.IncidentId].Value;
        }
        public string toJson() { return null; }
        public string generateJsonTemplate() { return null; }
    }
}
