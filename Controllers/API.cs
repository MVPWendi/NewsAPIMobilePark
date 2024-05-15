using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAPIMobilePark.Models.DTOs;
using NewsAPIMobilePark.Services;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NewsAPIMobilePark.Controllers
{
    [Controller]
    public class APIController(NewsProcessor processor, NewsContext db) : Controller
    {

        private readonly NewsProcessor _processor = processor;
        private readonly NewsContext _context = db;
        [HttpGet]
        public async Task<string> GetNews(string query, int startIndex, int endIndex, string language)
        {
            Log log = new()
            {
                End = endIndex,
                Start = startIndex,
                Language = language,
                Query = query
            };
            try
            {
                if (string.IsNullOrEmpty(query) || string.IsNullOrEmpty(language))
                {
                    log.Result = "Не заданы необходимые параметры";
                    await _context.Logs.AddAsync(log);
                    _context.SaveChanges();
                    return log.Result;
                }
                string result =await _processor.GetResults(query, startIndex, endIndex, language);
                log.Result = result;
                await _context.Logs.AddAsync(log);
                _context.SaveChanges();
                return result;
            }
            catch(Exception ex)
            {
                log.Result = ex.Message;
                await _context.Logs.AddAsync(log);
                _context.SaveChanges();
                return ex.Message + "\n" + ex.InnerException;

            }
        }

    }
}
