using Entities.Formats;
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
    public class MigrationCandidates
    {
        public MigrationCandidates(List<CandidatesFormat> candidates, List<byte[]> files)
        {
            Candidates = candidates;
            Files = files;
        }

        [JsonPropertyName("candidates")]
        public List<CandidatesFormat> Candidates { get; set; }

        [JsonPropertyName("files")]
        public List<byte[]> Files { get; set; }


        [JsonPropertyName("path")]
        public string Path { get; set; }


        [JsonPropertyName("nameFile")]
        public List<string> NameFile { get; set; }

    }
}
