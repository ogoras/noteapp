using Core.Domain;
using Core.Repositories;
using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IProfileRepository _profileRepository;
        public NoteService(INoteRepository noteRepository, IProfileRepository profileRepository)
        {
            _noteRepository = noteRepository;
            _profileRepository = profileRepository;
        }

        public async Task CreatePrivate(NoteDTO n)
        {
            Note note = new Note
            {
                Owner = n.Uid == null ? null : await _profileRepository.ReadAsync((int)n.Uid),
                Encrypted = false,
                SharedPublically = false,
                Text = n.Text,
                ShareRecipients = new List<Profile>(),
                AttachedPhotos = new List<Photo>()
            };
            await _noteRepository.CreateAsync(note);
        }
    }
}
