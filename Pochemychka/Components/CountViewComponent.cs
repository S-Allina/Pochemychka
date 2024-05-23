using Pochemychka.Models;

namespace Pochemychka.Components
{
    public class CountViewComponent
    {
        private readonly PochemychkaContext _context;

        public CountViewComponent(PochemychkaContext context)
        {
            _context = context;
        }
        public string Invoke(int idT, int idQ)
        {
            string s = $"{_context.Questions.Where(t => t.IdTest == idT && t.IdQuestion <= idQ).Count()} / {_context.Questions.Where(t => t.IdTest == idT).Count()}";
            return s;
        }
    }
}
