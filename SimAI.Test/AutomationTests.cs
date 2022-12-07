using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SimAI.Core.OpenAI;
using SimAI.Test.Utilities;

namespace SimAI.Test {

    [TestClass]
    public class AutomationTests {
        // Example prompts:
        // Open notepad and type hello world
        // Save it as foo.txt to the C drive and then close notepad
        
        // Count how many times I click the left mouse button over the next 5 seconds

        // Whenever I create a file in C:\ named rename.txt, rename it to foo.txt

        // If a majority of my screen is red, then send me an email with a screenshot of it

        // Ask what email
        // Ask if 'majority' is 50%

        // Intent -> Plan -> Approval -> Execute -> Report
    }
}