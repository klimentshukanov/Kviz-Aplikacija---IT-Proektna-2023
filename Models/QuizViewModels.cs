using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kviz_Aplikacija___IT_Proektna_2023.Models
{
    

    public class SolveQuizVM
    {
        public Quiz Quiz { get; set; }  
        public List<Question> questions { get; set; }
        public List<Choice> choices { get; set; }

        public SolveQuizVM() { }

        public SolveQuizVM(Quiz quiz, List<Question> questions, List<Choice> choices)
        {
            Quiz = quiz;
            this.questions = questions;
            this.choices = choices;
        }
    }

    public class EditAndCreateQuizVM
    {
        public Quiz Quiz { get; set; }

        public List<Question> Questions { get; set; }

        public List<Choice> Choices { get; set; }
    }

    
}