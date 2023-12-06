using Weblab.Models;

namespace Weblab.Architecture.Interfaces;

public interface IDbFeedback
{
    public Task<bool> AddFeedback(FeedbackModel model, string login);
}