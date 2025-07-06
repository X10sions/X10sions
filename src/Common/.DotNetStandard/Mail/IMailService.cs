namespace Common.Mail;

public interface IMailService {
  Task SendAsync(MailRequest request);
}



//public class MailService : IMailService { 


//}