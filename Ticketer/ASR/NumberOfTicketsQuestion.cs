using Microsoft.Speech.Recognition;
using Ticketer.MyProperties;

namespace Ticketer.ASR
{
    class NumberOfTicketsQuestion : ASRAbstract
    {
        private SpeechRecognitionEngine pSRE;

        public NumberOfTicketsQuestion()
        {
            pSRE = new SpeechRecognitionEngine(MyCultureInfo.PolishCulture);
            pSRE.SetInputToDefaultAudioDevice();
            PrepareGrammar(pSRE);
        }

        public override string GetAnswer()
        {
            string count = AnswerReceiver.GetAnswer(pSRE);
            if (count.Equals("Dwa"))
            {
                count = "2";
            }
            return count; 
        }

        protected override void PrepareGrammar(SpeechRecognitionEngine engine)
        {
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            Choices choices = new Choices();
            
            for(int i = 1; i < 10; i++)
            {
                if(i == 2)
                {
                    choices.Add("Dwa");
                } else
                {
                    choices.Add(i.ToString());
                }
            }

            grammarBuilder.Append(choices);
            Grammar grammar = new Grammar(grammarBuilder);

            engine.LoadGrammar(grammar);
        }
    }
}
