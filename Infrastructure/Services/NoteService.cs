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

        public async Task Create(int uid, NoteDTO n)
        {
            Note note = new Note
            {
                Owner = await _profileRepository.ReadAsync(uid),
                Encrypted = n.Encrypted,
                SharedPublically = n.Encrypted ? null : (bool?)(n.SharedPublically ?? false),
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

        public async Task<NoteDTOwithID> Read(int id)
        {
            var note = await _noteRepository.ReadAsync(id);

            return new NoteDTOwithID(note);
        }

        public async Task<IEnumerable<NoteDTOwithID>> ReadAll(int uid)
        {
            var notes = await _noteRepository.ReadAllAsync(uid);

            return notes.Select(x => new NoteDTOwithID(x));
        }

        public async Task<IEnumerable<NoteDTOwithID>> ReadPublic()
        {
            var notes = await _noteRepository.ReadPublicAsync();

            return notes.Select(x => new NoteDTOwithID(x));
        }

        public async Task Update(int uid, int id, NoteDTO n)
        {
            Note? original = await _noteRepository.ReadAsync(id);

            if (original == null)
                throw new NullReferenceException("Note doesn't exist");

            Note updated = new Note()
            {
                AttachedPhotos = original.AttachedPhotos,
                Encrypted = n.Encrypted,
                SharedPublically = n.SharedPublically,
                Text = n.Text,
                Id = original.Id,
                ShareRecipients = original.ShareRecipients,
                Owner = original.Owner
            };

            await _noteRepository.UpdateAsync(updated);
        }
    }
}
