using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Abstraction.Repositories;

public interface ITranslationJobRepository
{
    Task<int> CreateJob(TranslationJob job);
    Task<TranslationJob> GetById(int id);
    Task<List<TranslationJob>> GetJobs();
}

public interface ITranslatorRepository
{
    Task<int> Create(Translator translator);
    Task<Translator> GetById(int id);
    Task<Translator> GetByName(string name);
    Task<List<Translator>> GetAll();
}
