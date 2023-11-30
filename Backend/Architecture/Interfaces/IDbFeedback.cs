using Weblab.Models;

namespace Weblab.Architecture.Interfaces;

public interface IDbFeedback
{
    public Task AddFeedback(FeedbackModel model, string login);
}