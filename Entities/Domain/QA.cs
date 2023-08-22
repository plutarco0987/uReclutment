using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DataContext
{
    public class QA
    {
        public QA()
        {            
        }

        public QA(string noteID, string? noteName, string? parentNoteID, int? commentId, int? answerId)
        {
            NoteID = noteID;
            NoteName = noteName;
            ParentNoteID = parentNoteID;
            CommentId = commentId;
            AnswerId = answerId;
        }

        [JsonPropertyName("noteID")]
        public string NoteID { get; set; }
        [JsonPropertyName("noteName")]        
        public string? NoteName { get; set; }
        [JsonPropertyName("parentNoteID")]        
        public string? ParentNoteID { get; set; }
        [JsonPropertyName("commentId")]
        public int? CommentId { get; set; }

        [JsonPropertyName("answerId")]
        public int? AnswerId { get; set; }

    }
}
