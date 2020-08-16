namespace MakerTracker.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MakerTracker.DBModels;
    using MakerTracker.Models;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using SendGrid;
    using SendGrid.Helpers.Mail;


    public class FeedbackController : ApiBaseController
    {
        private readonly IMapper _mapper;
        private SendGridClient SendGridClient { get; }
        private MailSettings MailSettings { get; }
        private IConfiguration Config { get; }

        public FeedbackController(MakerTrackerContext context, IMapper mapper, SendGridClient sendGridClient, MailSettings mailSettings, IConfiguration config) : base(context)
        {
            _mapper = mapper;
            SendGridClient = sendGridClient;
            MailSettings = mailSettings;
            Config = config;
        }

        // GET: api/Feedback
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [EnableQuery]
        public ActionResult<IQueryable<FeedbackDto>> GetFeedback()
        {
            return Ok(_context.Feedback.ProjectTo<FeedbackDto>(_mapper.ConfigurationProvider));
        }

        // GET: api/Feedback
        [Authorize(Roles = "Admin")]
        [HttpGet("Query")]
        [EnableQuery]
        public IActionResult QueryFeedback()
        {
            return Ok(_context.Feedback);
        }

        // GET: api/Feedback/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedback(int id)
        {
            var product = await _context.Feedback.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Feedback>> PostFeedback(FeedbackDto entry)
        {
            var profile = await GetLoggedInProfile();
            var feedback = _mapper.Map<Feedback>(entry);
            feedback.CreatedDate = DateTime.Now;
            feedback.ProfileId = profile.Id;
            _context.Feedback.Add(feedback);
            await _context.SaveChangesAsync();
            feedback.Profile = profile;
            await SendEmail(feedback);

            return CreatedAtAction("PostFeedback", new { id = entry.Id }, entry);
        }

        public async Task SendEmail(Feedback feedback)
        {
            SendGridMessage message = MailHelper.CreateSingleEmail(
                new EmailAddress("no-reply@arhub.org"),
                new EmailAddress(Config.GetValue<string>("FeedbackRecipient")),
                $"[{feedback.Type}] Feedback received from Maker Tracker",
                FormatFeedbackPlainText(feedback), FormatFeedbackHtml(feedback)
                );
            message.MailSettings = MailSettings;
            await SendGridClient.SendEmailAsync(message);
        }

        protected string FormatFeedbackPlainText(Feedback feedback)
        {
            return $@"{feedback.Profile.FirstName} {feedback.Profile.LastName} Wrote:
{feedback.Comment}

url: {feedback.Url}";
        }

        protected string FormatFeedbackHtml(Feedback feedback)
        {
            return $@"<p><strong>{feedback.Profile.FirstName} {feedback.Profile.LastName} Wrote:</strong></p>
<blockquote>{HttpUtility.HtmlEncode(feedback.Comment)}</blockquote>

<p>url: <a href=""{feedback.Url}"">{feedback.Url}</a>";
        }

        [Authorize(Roles="Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<FeedbackDto>> DeleteFeedback(int id)
        {
            var entry = await _context.Feedback.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            _context.Feedback.Remove(entry);
            await _context.SaveChangesAsync();

            return _mapper.Map<FeedbackDto>(entry);
        }
    }
}
