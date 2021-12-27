using Core.Domain;
using Core.Repositories;
using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task CreatePrivate(int uid, NoteDTO n)
        {
            Note note = new Note
            {
                Owner = await _profileRepository.ReadAsync(uid),
                Encrypted = false,
                SharedPublically = false,
                Text = n.Text,
                ShareRecipients = new List<Profile>(),
                AttachedPhotos = new List<Photo>()
            };
            await _noteRepository.CreateAsync(note);
        }

        public async Task Delete(int uid, int id)
        {
            Note n = await _noteRepository.ReadAsyncWithOwner(id);
            if (n == null)
                throw new NullReferenceException();
            if (n.Owner.UserId != uid)
                throw new ArgumentException("User id doesn't match note id");
            await _noteRepository.DeleteAsync(n);
        }

        public async Task<IEnumerable<NoteDTOwithID>> ReadAll(int uid)
        {
            var notes = await _noteRepository.ReadAllAsync(uid);

            return notes.Select(x => new NoteDTOwithID(x));
        }
    }
}
