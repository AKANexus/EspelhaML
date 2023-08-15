﻿using EspelhaML.Domain;
using EspelhaML.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace EspelhaML.Services
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
            if (questionResponse.data?.Id is null)
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
                    questionResponse.data.DateCreated, questionResponse.data.Status, questionResponse.data.ItemId!);
            }
            else
            {
                tentativo.QuestionStatus = questionResponse.data.Status;
                tentativo.QuestionText = questionResponse.data.Text ?? "NULL";
            }

            if (questionResponse.data.Answer != null)
            {
                tentativo.AnswerStatus = questionResponse.data.Answer.Status;
                tentativo.DateReplied = questionResponse.data.Answer.DateCreated;
                tentativo.AnswerText = questionResponse.data.Answer.Text;
            }

            context.Questions.Update(tentativo);
            await context.SaveChangesAsync();

        }
    }
}
