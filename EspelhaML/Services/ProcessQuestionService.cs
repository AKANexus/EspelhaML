using Microsoft.EntityFrameworkCore;
using MlSuite.Domain;
using MlSuite.EntityFramework.EntityFramework;

namespace MlSuite.MlSynch.Services
{
    public class ProcessQuestionService
    {
        private readonly IServiceProvider _provider;

        public ProcessQuestionService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task ProcessInfo(ulong resourceId, string apiToken)
        {
            IServiceProvider scopedProvider = _provider.CreateScope().ServiceProvider;
            MlApiService mlApi = scopedProvider.GetRequiredService<MlApiService>();
            TrilhaDbContext context = scopedProvider.GetRequiredService<TrilhaDbContext>();
            var questionResponse = await mlApi.GetQuestionById(apiToken, resourceId.ToString());
            if (questionResponse.data?.Id is null || questionResponse.data?.Id == 0)
            {
                context.Logs.Add(new EspelhoLog(nameof(ProcessQuestionService),
                    $"Falha ao obter os dados requisitados: {questionResponse.data?.Error}"));
                await context.SaveChangesAsync();
                return;
            }

            Question? tentativo = await context.Questions.FirstOrDefaultAsync(x => x.Id == questionResponse.data.Id);
            if (tentativo == null)
            {
                tentativo = new((long)questionResponse.data.Id, questionResponse.data.Text!, questionResponse.data.From!.Id,
                    questionResponse.data.DateCreated, questionResponse.data.Status, questionResponse.data.ItemId!,
                    questionResponse.data.SellerId);

                if (questionResponse.data.Answer != null)
                {
                    tentativo.AnswerStatus = questionResponse.data.Answer.Status;
                    tentativo.DateReplied = questionResponse.data.Answer.DateCreated;
                    tentativo.AnswerText = questionResponse.data.Answer.Text;
                }
            }
            else if (tentativo.QuestionStatus != questionResponse.data.Status || tentativo.AnswerStatus != questionResponse.data.Answer?.Status)
            {
                tentativo.QuestionStatus = questionResponse.data.Status;
                tentativo.QuestionText = questionResponse.data.Text ?? "NULL";

                if (questionResponse.data.Answer != null)
                {
                    tentativo.AnswerStatus = questionResponse.data.Answer.Status;
                    tentativo.DateReplied = questionResponse.data.Answer.DateCreated;
                    tentativo.AnswerText = questionResponse.data.Answer.Text;
                }
            }

            else
            {
                return;
            }

            context.Questions.Update(tentativo);
            await context.SaveChangesAsync();

        }
    }
}
