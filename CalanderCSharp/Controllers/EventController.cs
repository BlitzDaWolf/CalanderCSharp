using CalanderCSharp.Context;
using CalanderCSharp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CalanderCSharp.Controllers
{
    public struct AddCalanderEvent
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        public readonly CalanderContext context;

        public EventController(CalanderContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IList<CalanderEvent> index()
        {
            return context.Events.Where(x => x.End >= DateTime.Now).Take(10).ToList();
        }

        [HttpPost("add")]
        public CalanderEvent Add(AddCalanderEvent aEvent)
        {
            CalanderEvent e = new CalanderEvent { Description = aEvent.Description, Start = aEvent.Start, End = aEvent.End, Title = aEvent.Title };
            context.Events.Add(e);
            context.SaveChanges();
            return e;
        }

        [HttpGet("free")]
        public IList<DateTime> GetFreeTime(double dururation, double start, double end)
        {
            var t = end - start;
            var v = Math.Floor(t / dururation);

            TimeSpan ts = TimeSpan.FromHours(dururation);
            TimeSpan startTs = TimeSpan.FromHours(start);
            TimeSpan RealEnd = startTs + (ts * v);

            var ev = index();

            List<DateTime> FreeTime = new List<DateTime>();
            for (int i = 0; i < 3; i++)
            {
                var StartTime = DateTime.Now;
                StartTime = StartTime.AddHours(-StartTime.Hour + startTs.Hours);
                StartTime = StartTime.AddMinutes(-StartTime.Minute + startTs.Minutes);
                StartTime = StartTime.AddSeconds(-StartTime.Second);
                StartTime = StartTime.AddMicroseconds(-StartTime.Microsecond);
                StartTime = StartTime.AddDays(i);
                for (int j = 0; j < v; j++)
                {
                    var time = StartTime.AddMinutes(ts.TotalMinutes * j);
                    if(time > DateTime.Now)
                    {
                        if (ev.Count(x => x.Start < time && x.End > time) == 0)
                        {
                            FreeTime.Add(time);
                        }
                    }
                }
            }

            return FreeTime;
        }

        [HttpDelete("Delete/id/{id}")]
        public void Delete(int id)
        {
            context.Events.Remove(context.Events.FirstOrDefault(x => x.Id == id));
            context.SaveChanges();
        }
        
        [HttpDelete("Delete/all")]
        public void DeleteAll()
        {
            context.Events.ToList()
                .ForEach(x=> context.Events.Remove(x));
            context.SaveChanges();
        }
    }
}
