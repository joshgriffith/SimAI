using System;
using System.Collections.Generic;
using System.IO;
using SimAI.Core.Entities;
using SimAI.Core.Intent;
using SimAI.Core.Skills;

namespace SimAI.Knowledge.Skills {

    [Skill("file", "storage")]
    //[Entity("path", @"{letter}:[\[{word}]]")]
    [Entity("path", @"{letter}:\{word}")]
    public class FileSystemSkills {
        
        [Intent("get files", "list files", "files")]
        //[Sample("what files are (in|at) {path}")]
        [Sample("what files are at {path}")]
        public IEnumerable<string> GetFiles(string path) {
            return Directory.GetFiles(path);
        }

        [Intent("copy")]
        [Sample("copy {path} to {path}")]
        public void CopyFile(string from, string to) {

        }
    }
}