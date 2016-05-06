using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CorpusSpliter.DataEntiyy
{
    public class Incident
    {
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
        public virtual ICollection<ChatLog> ChatLogs { get; set; }
        public virtual ICollection<QALog> QaLogs { get; set; }
    }
}
