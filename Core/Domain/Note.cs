using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    public class Note : IUpdateable<Note>
    {
        public int Id { get; set; }
        public Profile? Owner { get; set; }
        public bool Encrypted { get; set; }
        public bool? SharedPublically { get; set; }
        public string Text { get; set; }
        public List<Profile>? ShareRecipients { get; set; }
        public List<Photo>? AttachedPhotos { get; set; }

        public void updateValues(Note n)
        {
            Owner = n.Owner;
            Encrypted = n.Encrypted;
            SharedPublically = Encrypted ? null : n.SharedPublically ?? SharedPublically;
            Text = n.Text;
            ShareRecipients = n.ShareRecipients;
            AttachedPhotos = n.AttachedPhotos;
        }
    }
}
