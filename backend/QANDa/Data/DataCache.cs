using QANDa.Model;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace QANDa.Data
{
    public class DataCache : IDataCache
    {
        private readonly MemoryCache _memoryCache;
        private string GetCacheKey(int id)=> $"Question-{id}";
        public DataCache()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions {SizeLimit=100});
        }

        QuestionGetSingleResponse IDataCache.Get(int questionId)
        {
            _memoryCache.TryGetValue(GetCacheKey(questionId), out QuestionGetSingleResponse questionResponse);
            return questionResponse;
        }

        void IDataCache.Remove(int questionId)
        {
            _memoryCache.Remove(GetCacheKey(questionId));
        }

        QuestionGetSingleResponse IDataCache.Set(QuestionGetSingleResponse question)
        {
            var memoryOption = new MemoryCacheEntryOptions()
                                    .SetSize(1)
                                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));
            _memoryCache.Set(GetCacheKey(question.QuestionId),question,memoryOption);
            return question;
        }
    }
}
