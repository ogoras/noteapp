﻿using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface INoteService
    {
        public Task Create(int uid, NoteDTO n);
        public Task<IEnumerable<NoteDTOwithID>> ReadAll(int uid);
        public Task Delete(int uid, int id);
    }
}
