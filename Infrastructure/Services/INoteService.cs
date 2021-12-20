using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface INoteService
    {
        Task CreatePrivate(NoteDTO n);
    }
}
