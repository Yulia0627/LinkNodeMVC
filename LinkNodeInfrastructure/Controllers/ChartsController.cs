using Microsoft.AspNetCore.Mvc;               
using Microsoft.EntityFrameworkCore;          
using LinkNodeDomain.Model;



namespace LinkNodeInfrastructure.Controllers
{
    [Route("api/stats")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly DbLinkNodeContext _context;
        public ChartsController(DbLinkNodeContext context)
        {
            _context = context;
        }

        [HttpGet("freelancer-proposals-activity")]
        public async Task<ActionResult<IEnumerable<ProposalActivity>>> GetFreelancerProposalsActivity(
            int freelancerId,
            CancellationToken cancellationToken)
        {
            var trailingYear = DateTime.SpecifyKind(DateTime.UtcNow.AddMonths(-12), DateTimeKind.Unspecified);
            var activityData = await _context.Proposals
                .Where(p => p.FreelancerId == freelancerId && p.CreatedDate >= trailingYear)
                .GroupBy(p => new { p.CreatedDate.Year, p.CreatedDate.Month })
                .Select(group => new ProposalActivity
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    ProposalsCount = group.Count()
                })
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ToListAsync(cancellationToken);

            return Ok(activityData);
        }



        [HttpGet("client-jobs-activity")]
        public async Task<ActionResult<IEnumerable<JobActivity>>> GetClientJobsActivity(
            int clientId,
            CancellationToken cancellationToken)
        {
            
            if (clientId <= 0) return BadRequest("Invalid Client ID");
            var trailingYear = DateTime.SpecifyKind(DateTime.UtcNow.AddMonths(-12), DateTimeKind.Unspecified);
            var activityData = await _context.Vacancies
                .Where(v => v.ClientId == clientId
                       && v.CreatedDate != DateTime.MinValue && v.CreatedDate >= trailingYear) 
                .GroupBy(v => new
                {
                    v.CreatedDate.Year,
                    v.CreatedDate.Month
                })
                .Select(group => new JobActivity
                {

                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    JobsCount = group.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync(cancellationToken);

            return Ok(activityData);
        }
    }
}

