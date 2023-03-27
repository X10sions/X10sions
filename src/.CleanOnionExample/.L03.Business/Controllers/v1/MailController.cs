namespace CleanOnionExample.Controllers;

[ApiVersion("1.0")]
public class MailController : BaseApiVersionController<MailController> {
  private readonly IEmailService mailService;
  public MailController(IEmailService mailService) {
    this.mailService = mailService;
  }
  [HttpPost("send")]
  public async Task<IActionResult> SendMail([FromForm] MailRequest request) {
    await mailService.SendEmailAsync(request);
    return Ok();
  }

}
